using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using HTB.Database;
using HTBMailboxMonitor.at.or.ecp.mail;
using HTBUtilities;

namespace HTBMailboxMonitor
{
    internal class InstallmentMailMonitor
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private ExchangeServiceBinding _esb;
        private readonly tblControl _control = HTBUtils.GetControlRecord();
        private readonly string _mailbox = HTBUtils.GetConfigValue("InstallmentSignedMailbox");
        private readonly string _subjectIn = HTBUtils.GetConfigValue("InstallmentSignedSubjectIn");
        private readonly string _subjectOut = HTBUtils.GetConfigValue("InstallmentSignedSubjectOut");
        private readonly string _attachmentStart = HTBUtils.GetConfigValue("InstallmentSignedAttachmentStartsWith");
        private readonly string _docFolder = HTBUtils.GetConfigValue("DocumentsFolder");

        public void CheckMailbox()
        {
            ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            _esb = new ExchangeServiceBinding
            {
                RequestServerVersionValue = new RequestServerVersion { Version = ExchangeVersionType.Exchange2007 },
                Credentials = new NetworkCredential(_control.SMTPUser, _control.SMTPPW, "ecp.local"),
                Url = @"https://mail.ecp.or.at/EWS/Exchange.asmx"
            };

            //Create unread only restriction 
            var restriction = new RestrictionType();
            var isEqualTo = new IsEqualToType();
            var pathToFieldType = new PathToUnindexedFieldType {FieldURI = UnindexedFieldURIType.messageIsRead};

            var constantType = new FieldURIOrConstantType();
            var constantValueType = new ConstantValueType {Value = "0"};
            constantType.Item = constantValueType;
            isEqualTo.Item = pathToFieldType;
            isEqualTo.FieldURIOrConstant = constantType;
            restriction.Item = isEqualTo;
            
            var fit = new FindItemType
            {
                ItemShape = new ItemResponseShapeType { BaseShape = DefaultShapeNamesType.AllProperties },
                ParentFolderIds = new DistinguishedFolderIdType[]
                                                    {
                                                        new DistinguishedFolderIdType
                                                            {
                                                                Mailbox = new EmailAddressType {EmailAddress = _mailbox},
                                                                Id = DistinguishedFolderIdNameType.inbox
                                                            }
                                                    },
                Traversal = ItemQueryTraversalType.Shallow,
                Restriction = restriction
            };
            FindItemResponseType firt = _esb.FindItem(fit);

            foreach (ResponseMessageType rmt in firt.ResponseMessages.Items)
            {
                // One FindItemResponseMessageType per folder searched.
                var firmt = rmt as FindItemResponseMessageType;

                if (firmt == null || firmt.RootFolder == null)
                    continue;

                FindItemParentType fipt = firmt.RootFolder;
                object obj = fipt.Item;

                // FindItem contains an array of items.
                if (obj is ArrayOfRealItemsType)
                {
                    var items = (obj as ArrayOfRealItemsType);
                    if (items.Items != null)
                    {
                        foreach (ItemType it in items.Items)
                        {
                            if (it is MessageType)
                            {
                                var msg = (MessageType)it;
                                if (msg.Subject == _subjectIn)
                                {
                                    Log.Info("=================");
                                    Log.Info("Subj: " + msg.Subject);
                                    Log.Info("Read: " + msg.IsRead);

                                    if (msg.HasAttachments)
                                    {
                                        ItemType attchItemType = GetItem(msg.ItemId);
                                        List<AttachmentInfoResponseMessageType> attachList = GetAttachments(attchItemType.Attachments);
                                        foreach (AttachmentInfoResponseMessageType att in attachList)
                                        {
                                            switch (att.Attachments[0].GetType().Name)
                                            {

                                                //attachments can be of type FileAttachmentType or ItemAttachmentType
                                                //so we need to figure out which type we have before we manipulate it
                                                case "FileAttachmentType":
                                                    var theAttachment = (FileAttachmentType)att.Attachments[0];
                                                    string name = theAttachment.Name;
                                                    if (IsSignedPDF(name))
                                                    {
                                                        int aktNumber = GetActNumber(name);
                                                        if (aktNumber > 0)
                                                        {
                                                            using (Stream fileToDisk = new FileStream(_docFolder + @"\Signed_" + name, FileMode.OpenOrCreate))
                                                            {

                                                                fileToDisk.Write(theAttachment.Content, 0, theAttachment.Content.Length);
                                                                fileToDisk.Flush();
                                                                fileToDisk.Close();
                                                                fileToDisk.Dispose();
                                                            }
                                                            SaveDocumentRecord(aktNumber, "Signed_" + name);
                                                            SendEmail(aktNumber, theAttachment);
                                                            msg.IsRead = true;
                                                            SetReadFlagForMessage(msg.ItemId);
                                                        }
                                                    }
                                                    break;
                                                /*
                                        case "ItemAttachmentType":
                                            //save to disk
                                            ItemType theItemAttachment = ((ItemAttachmentType)att.Attachments[0]).Item;
                                            using (Stream fileToDisk = new FileStream(Folder + @".\" + att.Attachments[0].Name + ".eml", FileMode.OpenOrCreate))
                                            {
                                                byte[] contentBytes = Convert.FromBase64String(theItemAttachment.MimeContent.Value);
                                                fileToDisk.Write(contentBytes, 0, contentBytes.Length);
                                                fileToDisk.Flush();
                                                fileToDisk.Close();
                                            }
                                            break;
                                             */
                                            }
                                        }
                                    }
                                    List<ItemType> bodyList = GetBody(msg.ItemId);
                                    foreach (var itemType in bodyList)
                                    {
                                        Console.WriteLine(itemType.Body.Value);
                                    }
                                    Console.WriteLine("=================");
                                }
                            }
                        }
                    }
                }
            }
        }

        //EWS code to update the IsRead flag, *ONLY* for MessageType
        private bool SetReadFlagForMessage(ItemIdType messageId)
        {

            var message = new MessageType {IsRead = true, IsReadSpecified = true};
            var path = new PathToUnindexedFieldType {FieldURI = UnindexedFieldURIType.messageIsRead};
            var setField = new SetItemFieldType {Item1 = message, Item = path};


            var updatedItems = new ItemChangeType[1];
            updatedItems[0] = new ItemChangeType {Updates = new ItemChangeDescriptionType[1]};
            updatedItems[0].Updates[0] = setField;

            var updates = new ItemChangeDescriptionType[1];
            updates[0] = new ItemChangeDescriptionType {Item = path};

            updatedItems[0].Item = new ItemIdType();
            ((ItemIdType)updatedItems[0].Item).Id = messageId.Id;
            ((ItemIdType)updatedItems[0].Item).ChangeKey = messageId.ChangeKey;


            var request = new UpdateItemType
                              {
                                  ItemChanges = updatedItems,
                                  ConflictResolution = ConflictResolutionType.AutoResolve,
                                  MessageDisposition = MessageDispositionType.SaveOnly,
                                  MessageDispositionSpecified = true,
                                  SendMeetingInvitationsOrCancellations = CalendarItemUpdateOperationType.SendToChangedAndSaveCopy,
                                  SendMeetingInvitationsOrCancellationsSpecified = true
                              };

            UpdateItemResponseType response = _esb.UpdateItem(request);

            return response.ResponseMessages.Items[0].ResponseClass != ResponseClassType.Success;
        }
        
        private ItemType GetItem(BaseItemIdType itemId)
        {
            var itemType = new GetItemType
            {
                ItemShape = new ItemResponseShapeType
                {
                    BaseShape = DefaultShapeNamesType.AllProperties,
                },
                ItemIds = new ItemIdType[1]
            };


            itemType.ItemIds[0] = itemId;

            GetItemResponseType itemResponse = _esb.GetItem(itemType);

            if (itemResponse.ResponseMessages.Items.Length == 0)
            {
                throw new InvalidOperationException("Unable to get specified items");
            }

            var itemInfo = itemResponse.ResponseMessages.Items[0] as ItemInfoResponseMessageType;
            if (itemInfo != null && itemInfo.ResponseClass == ResponseClassType.Success && (null != itemInfo.Items.Items) && (itemInfo.Items.Items.Length > 0))
            {
                return itemInfo.Items.Items[0];
            }

            return null;
        }

        private List<ItemType> GetBody(BaseItemIdType itemId)
        {
            var b = new GetItemType();

            b.ItemShape = new ItemResponseShapeType();
            b.ItemShape.BaseShape = DefaultShapeNamesType.AllProperties;

            b.ItemIds = new ItemIdType[1];
            b.ItemIds[0] = (BaseItemIdType)itemId;

            GetItemResponseType itemResponse = _esb.GetItem(b);

            if (itemResponse.ResponseMessages.Items.Length == 0)
            {

                throw new InvalidOperationException("Unable to get specified items");

            }

            List<ItemType> itemsList = new List<ItemType>();

            // Note: the number of responses does not indicate the number of items returned.
            // One has to look at the ResponseClass to tell if there's item info on each response.
            for (int i = 0; i < itemResponse.ResponseMessages.Items.Length; i++)
            {

                var itemInfo = itemResponse.ResponseMessages.Items[0] as ItemInfoResponseMessageType;

                if (itemInfo != null && itemInfo.ResponseClass == ResponseClassType.Success && (null != itemInfo.Items.Items) && (itemInfo.Items.Items.Length > 0))
                {
                    itemsList.AddRange(itemInfo.Items.Items);
                }
            }
            return itemsList;
        }

        private List<AttachmentInfoResponseMessageType> GetAttachments(AttachmentType[] attachments)
        {
            var arst = new AttachmentResponseShapeType { IncludeMimeContent = true, IncludeMimeContentSpecified = true };
            //create an array of attachment ids that we want to request
            var aita = new AttachmentIdType[attachments.Length];
            for (int i = 0; i < attachments.Length; i++)
            {
                aita[i] = new AttachmentIdType { Id = attachments[i].AttachmentId.Id };
            }
            var gat = new GetAttachmentType { AttachmentIds = aita, AttachmentShape = arst };
            GetAttachmentResponseType gart = _esb.GetAttachment(gat);
            return gart.ResponseMessages.Items.Cast<AttachmentInfoResponseMessageType>().ToList();
        }

        private bool IsSignedPDF(string name)
        {
            return name.IndexOf(_attachmentStart) == 0;
        }

        private int GetActNumber(string name)
        {
            int idx = name.IndexOf(_attachmentStart);
            if (idx < 0)
                return -1;

            string str = name.Substring(idx + _attachmentStart.Length + 1);
            idx = str.IndexOf("-");
            if (idx < 0)
                idx = str.IndexOf(".");
            if (idx < 0)
                return -1;
            str = str.Substring(0, idx);
            try
            {
                return Convert.ToInt32(str);
            }
            catch
            {

            }
            return -1;
        }
        
        private void SendEmail(int aktNumber, FileAttachmentType attachment)
        {
            var to = new List<string>();
            string sbAddress = HTBUtils.GetSBEmailAddress(aktNumber);
            tblAktenInt akt = HTBUtils.GetInterventionAkt(aktNumber);
            if (sbAddress != null)
            {
                to.Add(sbAddress);
            }
            to.Add(HTBUtils.GetConfigValue("Default_EMail_Addr"));
            var stream = new MemoryStream();
            stream.Write(attachment.Content, 0, attachment.Content.Length);
            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            string body = HTBUtils.GetFileText(HTBUtils.GetConfigValue("Klient_Signed_Installment_Text"));
            body = body.Replace("[akt]", akt.AktIntAZ);
            new HTBEmail().SendGenericEmail(to, _subjectOut, body, true, new List<HTBEmailAttachment> { new HTBEmailAttachment(stream, attachment.Name, attachment.ContentType) });
            stream.Close();
            stream.Dispose();
        }
        private void SaveDocumentRecord(int aktNumber, string fileName)
        {
            var doc = new tblDokument
            {
                // Inkasso
                DokDokType = 25,
                DokCaption = "Unterschriebene RV",
                DokInkAkt = aktNumber,
                DokCreator = _control.AutoUserId,
                DokAttachment = fileName,
                DokCreationTimeStamp = DateTime.Now,
                DokChangeDate = DateTime.Now
            };

            RecordSet.Insert(doc);

            doc = (tblDokument)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblDokument ORDER BY DokID DESC", typeof(tblDokument));
            if (doc != null)
            {
                RecordSet.Insert(new tblAktenDokumente { ADAkt = aktNumber, ADDok = doc.DokID, ADAkttyp = 3 });
            }
        }
    }
}
