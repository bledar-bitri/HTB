using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using HTB.Database;
using HTB.Database.Views;
using HTB.v2.intranetx.aktenintprovfix;
using HTB.v2.intranetx.util;
using HTBAktLayer;
using HTBExtras.XML;
using HTBReports;
using HTBUtilities;
using Tamir.SharpSsh.java.io;
using File = System.IO.File;

namespace HTB.v2.intranetx.aktenint.tablet
{
    public partial class SetActionTablet : System.Web.UI.Page
    {
        private readonly string _subjectOut = HTBUtils.GetConfigValue("InstallmentSignedSubjectOut");
        private readonly string _signedInstallmentFilePrefix = HTBUtils.GetConfigValue("SignedInstallmentFilePrefix");
        private readonly string _documentsFolder = HTBUtils.GetConfigValue("DocumentsFolder");
        private readonly string _defaultEmailAddr = HTBUtils.GetConfigValue("Default_EMail_Addr");
        private readonly string _body = HTBUtils.GetFileText(HTBUtils.GetConfigValue("Klient_Signed_Installment_Text"));

        protected void Page_Load(object sender, EventArgs e)
        {
            int aktId = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Params[GlobalHtmlParams.INTERVENTION_AKT]);
            int actionTypeId = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Params[GlobalHtmlParams.ACTION_TYPE_ID]);
            
            double actionLat = GlobalUtilArea.GetZeroIfConvertToDoubleError(GlobalUtilArea.GetEmptyIfNull(Request.Params[GlobalHtmlParams.LATITUDE]).Replace(".", ","));
            double actionLng = GlobalUtilArea.GetZeroIfConvertToDoubleError(GlobalUtilArea.GetEmptyIfNull(Request.Params[GlobalHtmlParams.LONGITUDE]).Replace(".",","));
            string actionMemo = GlobalUtilArea.GetEmptyIfNull(GlobalUtilArea.GetEmptyIfNull(Request.Params[GlobalHtmlParams.ACTION_MEMO]));

            
            var akt = HTBUtils.GetInterventionAkt(aktId);
            if (actionTypeId <= 0)
            {
                Response.Write("Error: Kein Akttyp ID!");
                return;
            }
            if(akt == null)
            {
                Response.Write("Error: Akt ["+aktId+"] nicht gefunden!");
                return;
            }
            var actionType = (tblAktenIntActionType)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntActionType WHERE AktIntActionTypeID = " + actionTypeId, typeof(tblAktenIntActionType));
            if(actionType == null)
            {
                Response.Write("Error: Aktion Type [" + actionTypeId + "] nicht gefunden!");
                return;
            }
            try
            {
                var errorMessage = new StringBuilder();
                bool okk = true;
                double collectedAmount = 0;
                var receipt = new XmlPrintableReceiptRecord();
//                Response.Write("\nIsTelAndEmailCollection: " + actionType.AktIntActionIsTelAndEmailCollection+"\n");
//                GlobalUtilArea.PrintFormParameters(Request, Response);
                if (actionType.AktIntActionTypeIsInstallment || actionType.AktIntActionIsTelAndEmailCollection)
                {
                    HttpFileCollection uploadFiles = Request.Files;
                    int i;
                    for (i = 0; i < uploadFiles.Count; i++)
                    {
                        HttpPostedFile postedFile = uploadFiles[i];

                        // Access the uploaded file's content in-memory:
                        Stream inStream = postedFile.InputStream;
                        var fileData = new byte[postedFile.ContentLength];
                        inStream.Read(fileData, 0, postedFile.ContentLength);
                        string xmlData = Encoding.UTF8.GetString(fileData);

                        if(actionType.AktIntActionIsTelAndEmailCollection)
                        {
                            XmlTelAndEmailRecord rec = getTelAndEmailRecord(xmlData);
                            if(!SaveTelAndEmail(akt.AktIntGegner, rec, errorMessage))
                            {
                                Response.Write("Error:"+errorMessage);
                                okk = false;
                            }
                        }
                        else
                        {
                            XmlAktInstallmentCalcRecord rec = getInstallmentCalcRecord(xmlData);
                            int paymentType = 0;
                            if (actionType.AktIntActionIsPersonalCollection)
                                paymentType = 1;
                            if(akt.IsInkasso())
                            {
                                tblCustInkAkt inkAkt = HTBUtils.GetInkassoAkt(akt.AktIntCustInkAktID);
                                if (inkAkt != null)
                                {
                                    if (!SaveInstallmentList(inkAkt.CustInkAktID, akt, rec.InstallmentsList, paymentType))
                                    {
                                        Response.Write("Error: Ratenvereinbarung konnte nicht gespeichert werden!\nWahrscheinlich gibt es schon Zahlungen und die RV kann nicht geändert werden.\n\nBitte kontaktieren Sie ECP.");
                                        okk = false;
                                    }
                                }
                                else
                                {
                                    Response.Write("Error: Inkassoakt nicht gefunden!");
                                    okk = false;
                                }
                            }
                            else
                            {
                                if(IsInterventionOnlyEntryValid(rec, errorMessage))
                                {
                                    akt.AKTIntRVInkassoType = paymentType;
                                    akt.AKTIntRVStartDate = rec.StartDate;
                                    akt.AKTIntRVAmmount = rec.InstallmentAmount;
                                    akt.AKTIntRVIntervallDay = rec.PaymentDay;
                                    akt.AKTIntRVNoMonth = rec.NumberOfInstallments;
                                    akt.AKTIntRVInfo = rec.PaymentPeriod;
                                    if(!RecordSet.Update(akt))
                                    {
                                        Response.Write("Error: Die [Akt] Information konnte nicht gespeichert werdern!\n");
                                    }
                                }
                                else
                                {
                                    Response.Write("Error: "+errorMessage);
                                    okk = false;
                                }
                            }
                            /*
                            Response.Write("\nStartDate: " + rec.StartDate.ToShortDateString());
                            Response.Write("\nEndDate: " + rec.EndDate.ToShortDateString());
                            Response.Write("\nInstallmentAmount: " + HTBUtils.FormatCurrency(rec.InstallmentAmount));
                            Response.Write("\nLastInstallmentAmount: " + HTBUtils.FormatCurrency(rec.LastInstallmentAmount));
                            Response.Write("\nNOI: " + rec.NumberOfInstallments);
                            Response.Write("\nTotal Interest: " + HTBUtils.FormatCurrency(rec.TotalInterest));

                            Response.Write("\nPaymentDay: " + rec.PaymentDay);
                            Response.Write("\nPaymentType: " + rec.PaymentType);
                            Response.Write("\nPaymentPeriod: " + rec.PaymentPeriod);
                            
                            Response.Write("\nRates ====================\n\n");
                            foreach (tblCustInkAktRate r in rec.InstallmentsList)
                            {
                                Response.Write(r.CustInkAktRateDueDate.ToShortDateString());
                                Response.Write("      ");
                                Response.Write(HTBUtils.FormatCurrency(r.CustInkAktRateAmount));
                                Response.Write("\n");
                            }
                            Response.Write("\n===========================\n\n\n");
                             */ 
                        }
                    }
                }
//                if (actionType.AktIntActionTypeIsInstallment && akt.IsInkasso())
                if (actionType.AktIntActionTypeIsInstallment)
                {
                    SendInstallmentEmail(akt.AktIntID);
                }
                if(actionType.AktIntActionIsWithCollection)
                {
                    collectedAmount = GlobalUtilArea.GetDoubleAmountFromParameter(Request, GlobalHtmlParams.PAID);
                    receipt = GetReceiptRecord(akt, CreateReceipt(akt, actionType, collectedAmount));
                }
                if (okk)
                {
                    if (SaveAction(-1, actionType, akt, actionMemo, collectedAmount, receipt.ReceiptId, actionLat, actionLng))
                    {
                        akt.AKTIntMemo = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.MEMO]);
                        akt.AktIntStatus = 2; // abgegeben
                        RecordSet.Update(akt);

                        if (IsAutoAktion(actionType))
                        {
                            var qryAkt = (qryAktenInt)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenInt WHERE AktIntID = " + akt.AktIntID, typeof(qryAktenInt));
                            GenerateProtokol(qryAkt); // Generate protocol based on the latest action
//                            NotifySB(qryAkt, actionType, collectedAmount);
                        }
                        
                        Response.Write(receipt.ToXmlString());
                        new HTBEmail().SendGenericEmail(new string[] { HTBUtils.GetConfigValue("Office_Email") }, "Akt " + akt.AktIntID + " abgegeben", "Zur Info: Akt " + akt.AktIntID+" wurde abgegeben.");
                    }
                    else
                    {
                        Response.Write("Error:Die Aktion konnte nicht gespeichert werdern!\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Error: ");
                Response.Write(ex.Message);
                Response.Write(ex.StackTrace);
            }
//            Response.Write("akt: "+akt.AktIntID+"\naktionTypdID: "+actionTypeId+"\naktionLat: "+aktionLat+"\naktionLng: "+aktionLng);
        }

        #region Action
        private bool SaveAction (int actionId, tblAktenIntActionType actionType, tblAktenInt akt, string memo, double collectedAmount, string receiptId, double lat, double lgn)
        {
            var provisionCalc = new ProvisionCalc();
            bool isNewAction = false;
            var aktAction = (tblAktenIntAction)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntAction WHERE AktIntActionID = " + actionId, typeof(tblAktenIntAction));
            if(aktAction == null)
            {
                aktAction = new tblAktenIntAction();
                isNewAction = true;
            }
            var ag = (tblAuftraggeber) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeber WHERE AuftraggeberID = " + akt.AktIntAuftraggeber, typeof (tblAuftraggeber));
            double balance = GetBalance(akt);

            aktAction.AktIntActionAkt = akt.AktIntID;
            aktAction.AktIntActionSB = akt.AktIntSB;

            aktAction.AktIntActionType = actionType.AktIntActionTypeID;
            aktAction.AktIntActionProvision = provisionCalc.GetProvision(collectedAmount, balance, akt.AktIntAuftraggeber, akt.AktIntAktType, akt.AktIntSB, actionType.AktIntActionTypeID);
            aktAction.AktIntActionHonorar = 0; // no more honorar (everything gets calculated into provision)
            aktAction.AktIntActionPrice = provisionCalc.GetPrice(collectedAmount, balance, akt.AktIntAuftraggeber, akt.AktIntAktType, akt.AktIntSB, actionType.AktIntActionTypeID);
            aktAction.AktIntActionProvAbzug = ag.AuftraggeberIntAktPovAbzug;
            aktAction.AktIntActionDate = DateTime.Now;
            aktAction.AktIntActionTime = DateTime.Now;
            aktAction.AktIntActionBeleg = receiptId;
            aktAction.AktIntActionReceiptID = GlobalUtilArea.GetZeroIfConvertToIntError(receiptId);
            aktAction.AktIntActionBetrag = collectedAmount;

            aktAction.AktIntActionLatitude = lat;
            aktAction.AktIntActionLongitude = lgn;
            aktAction.AktIntActionAddress = GlobalUtilArea.GetAddressFromLatitudeAndLongitude(lat, lgn);

            aktAction.AktIntActionMemo = memo;

            if (actionType.AktIntActionIsExtensionRequest)
            {
//                aktAction.AktIntActionAktIntExtID = ctlExtension.SaveExtensionRequest(aktAction);
            }
            return isNewAction ? RecordSet.Insert(aktAction) : RecordSet.Update(aktAction);
        }

        private double GetBalance(tblAktenInt akt)
        {
            double balance = 0;
            if (akt != null)
            {
                if (akt.IsInkasso())
                {
                    var aktUtils = new AktUtils(akt.AktIntCustInkAktID);
                    balance = aktUtils.GetAktBalance();
                }
                else
                {
                    ArrayList posList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntPos WHERE AktIntPosAkt = " + akt.AktIntID, typeof(tblAktenIntPos));
                    balance += posList.Cast<tblAktenIntPos>().Sum(aktIntPos => aktIntPos.AktIntPosBetrag);
                    balance += akt.AKTIntZinsenBetrag;
                    balance += akt.AKTIntKosten;
                    balance += akt.AktIntWeggebuehr;
                }
            }
            return balance;
        }
        #endregion

        #region Installment
        private XmlAktInstallmentCalcRecord getInstallmentCalcRecord(string xmlData)
        {
            var ds = new DataSet();
            ds.ReadXml(new StringReader(xmlData));
            var rec = new XmlAktInstallmentCalcRecord();
            foreach (DataTable tbl in ds.Tables)
            {
                if (tbl.TableName.ToUpper().Trim() == "XMLAKTINSTALLMENTCALCRECORD")
                {
                    foreach (DataRow dr in tbl.Rows)
                    {
                        rec.LoadFromDataRow(dr);
                    }
                }
                else if (tbl.TableName.ToUpper().Trim() == "CUSTINKAKTRATE")
                {

                    foreach (DataRow dr in tbl.Rows)
                    {
                        var rate = new tblCustInkAktRate();
                        rate.LoadFromDataRow(dr);
                        rec.InstallmentsList.Add(rate);
                    }
                }
            }
            return rec;
        }
        private bool SaveInstallmentList(int custAktId, tblAktenInt aktInt, ArrayList list, int paymentType)
        {
            if (list != null && list.Count > 0)
            {
                try
                {
                    HTBUtils.DeleteInstallmentPlan(custAktId);
                }
                catch(Exception)
                {
                    return false;
                }
                var set = new RecordSet();
                foreach (tblCustInkAktRate installment in list)
                {
                    installment.CustInkAktRateAktID = custAktId;
                    installment.CustInkAktRateLastChanged = DateTime.Now;
                    installment.CustInkAktRateReceivedAmount = 0;
                    installment.CustInkAktRateBalance = installment.CustInkAktRateAmount;
                    installment.CustInkAktRatePaymentType = paymentType;
                    installment.CustInkAktRatePostponeTillDate = HTBUtils.DefaultDate;
                    installment.CustInkAktRateNotifiedAD = HTBUtils.DefaultDate;
                    set.InsertRecord(installment);
                }
                UpdateInterventionAkt(aktInt, paymentType);
            }
            else
            {
                return false;
            }
            return true;
        }
        private void UpdateInterventionAkt(tblAktenInt akt, int paymentType)
        {
            tblControl control = HTBUtils.GetControlRecord();
            if (akt != null)
            {
                akt.AktIntProcessCode = control.ProcessCodeInstallment;
                akt.AKTIntRVInkassoType = paymentType;
                RecordSet.Update(akt);
            }
        }
        private bool IsInterventionOnlyEntryValid(XmlAktInstallmentCalcRecord rec, StringBuilder errors)
        {
            
            bool ok = true;
            
            if (rec.StartDate.ToShortDateString() == "01.01.1900")
            {
                errors.Append("Beginndatum eingeben\n");
                ok = false;
            }
            if (rec.InstallmentAmount <= 0)
            {
                errors.Append("Betrag/Monat eingeben\n");
                ok = false;
            }
            return ok;
        }

        private void SendInstallmentEmail(int intAktId)
        {
            var fileName = _signedInstallmentFilePrefix + intAktId + ".pdf";
            var attachment = _documentsFolder + fileName;
            if (File.Exists(attachment))
            {
                var to = new List<string>();

                string sbAddress = HTBUtils.GetSBEmailAddress(intAktId);
                tblAktenInt akt = HTBUtils.GetInterventionAkt(intAktId);
                if (sbAddress != null)
                {
                    to.Add(sbAddress);
                }
                to.Add(_defaultEmailAddr);
                string body = _body.Replace("[akt]", akt.AktIntAZ);
                new HTBEmail().SendGenericEmail(to, _subjectOut, body, true, new List<HTBEmailAttachment> { new HTBEmailAttachment(new FileInputStream(attachment), fileName, "application/pdf") }, 0, akt.AktIntID);
            }
        }
        #endregion

        #region Telefon and Email
        private XmlTelAndEmailRecord getTelAndEmailRecord(string xmlData)
        {
            var ds = new DataSet();
            ds.ReadXml(new StringReader(xmlData));
            var rec = new XmlTelAndEmailRecord();
            foreach (DataTable tbl in ds.Tables)
            {
                if (tbl.TableName.ToUpper().Trim() == "XMLTELANDEMAILRECORD")
                {
                    foreach (DataRow dr in tbl.Rows)
                    {
                        rec.LoadFromDataRow(dr);
                    }
                }
            }
            return rec;
        }
        internal bool SaveTelAndEmail(string gegnerOldID, XmlTelAndEmailRecord rec, StringBuilder errors)
        {
            if (IsTelAndEmailRecordValid(rec, errors))
            {
                var gegner = (tblGegner) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerOldId = '" + gegnerOldID + "'", typeof (tblGegner));
                gegner.GegnerPhoneCountry = "43";
                gegner.GegnerPhoneCity = rec.PhoneCity;
                gegner.GegnerPhone = rec.Phone;
                gegner.GegnerEmail = rec.Email;
                if(!RecordSet.Update(gegner))
                {
                    errors.Append("Die Information konnte nicht gespeichert werdern!\n");
                    return false;
                }
                return true;
            }
            return false;
        }

        private bool IsTelAndEmailRecordValid(XmlTelAndEmailRecord rec, StringBuilder errors)
        {
            bool ok = true;

            if (!rec.NoPhone && rec.PhoneCity.Trim().Equals(string.Empty))
            {
                errors.Append("Telefonnummer (vorwahl) eingeben!\n");
                ok = false;
            }
            if (!rec.NoPhone && rec.Phone.Trim().Equals(string.Empty))
            {
                errors.Append("Telefonnummer eingeben!\n");
                ok = false;
            }
            if (!rec.NoEmail && rec.Email.Trim().Equals(string.Empty))
            {
                errors.Append("Emailadresse eingeben!\n");
                ok = false;
            }
            if (ok)
            {
                if (!rec.PhoneCity.Trim().Equals(string.Empty))
                {
                    if (!HTBUtils.IsNumber(rec.PhoneCity))
                    {
                        errors.Append("Telefonnummer (vorwahl) muss ein Zahl sein!\n");
                        ok = false;
                    }
                }
                if (!rec.Email.Trim().Equals(string.Empty))
                {
                    if (!HTBUtils.IsValidEmail(rec.Email))
                    {
                        errors.Append("Emailadresse ist falsch!\n");
                        ok = false;
                    }
                }
            }
            return ok;
        }
        #endregion

        #region Extension Request
        #endregion

        #region Receipt
        private XmlPrintableReceiptRecord GetReceiptRecord(tblAktenInt akt, tblAktenIntReceipt receipt)
        {
            var ag = (tblAuftraggeber)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeber WHERE AuftraggeberID = " + receipt.AktIntReceiptAuftraggeber, typeof(tblAuftraggeber));
            var user = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT UserSex, UserVorname, UserNachname FROM tblUser WHERE UserID = " + receipt.AktIntReceiptUser, typeof(tblUser));

            var rec = new XmlPrintableReceiptRecord
                          {
                              ReceiptId = receipt.AktIntReceiptID.ToString(),
                              ReceiptDate = receipt.AktIntReceiptDate.ToShortDateString() + " " + receipt.AktIntReceiptDate.ToShortTimeString(),
                              City = receipt.AktIntReceiptCity,
                              AktId = receipt.AktIntReceiptAkt.ToString(),
                              AktAz = akt.AktIntAZ,
                              AgName = ag.AuftraggeberName1 + " " + ag.AuftraggeberName2,
                              AgZipCity = ag.AuftraggeberPLZ + " " + ag.AuftraggeberOrt,
                              AgStreet = ag.AuftraggeberStrasse,
                              AgTel = (ag.AuftraggeberPhoneCity.StartsWith("0") ? ag.AuftraggeberPhoneCity : "0" + ag.AuftraggeberPhoneCity) + " " + ag.AuftraggeberPhone,
                              AgEmail = ag.AuftraggeberEMail,
                              AgWeb = ag.AuftraggeberWeb,
                              AgSb = akt.AKTIntAGSB,
                              AgSbEmail = akt.AKTIntKSVEMail,
                              Collector = (user.UserSex == 1 ? "Herr " : "Frau ") + user.UserVorname + " " + user.UserNachname,
                              PaymentType = receipt.AktIntReceiptType == 1 ? "Gesamtinkasso" : "Teilzahlung",
                              PaymentAmount = HTBUtils.FormatCurrencyNumber(receipt.AktIntReceiptAmount),
                              PaymentTax = "0,00",
                              PaymentTotal = HTBUtils.FormatCurrencyNumber(receipt.AktIntReceiptAmount)
                          };
            return rec;
        }

        private tblAktenIntReceipt CreateReceipt(tblAktenInt akt, tblAktenIntActionType actionType, double amount)
        {
            var gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerOldID = " + akt.AktIntGegner, typeof(tblGegner));
            var receipt = new tblAktenIntReceipt
            {
                AktIntReceiptDate = DateTime.Now,
                AktIntReceiptUser = akt.AktIntSB,
                AktIntReceiptAmount = amount,
                AktIntReceiptAkt = akt.AktIntID,
                AktIntReceiptCity = gegner.GegnerLastOrt,
                AktIntReceiptType = actionType.AktIntActionIsTotalCollection ? 1 : 2,
                AktIntReceiptAuftraggeber = akt.AktIntAuftraggeber
            };
            if (RecordSet.Insert(receipt))
            {
                return (tblAktenIntReceipt)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblAktenIntReceipt ORDER BY AktIntReceiptID DESC", typeof(tblAktenIntReceipt));
            }
            return null;
        }
        #endregion

        #region Auto Akt
        public bool IsAutoAktion(tblAktenIntActionType actionType)
        {
            return actionType.AktIntActionIsAutoMoneyCollected || actionType.AktIntActionIsAutoRepossessed || actionType.AktIntActionIsAutoNegative;
        }
        public void GenerateProtokol(qryAktenInt akt)
        {
            
            var protocol = (tblProtokol)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblProtokol WHERE AktIntID = " + akt.AktIntID + " ORDER BY ProtokolID DESC", typeof(tblProtokol));
            if (protocol != null)
            {
                var action = (qryAktenIntActionWithType)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenIntActionWithType WHERE AktIntActionAkt = " + akt.AktIntID + " ORDER BY AktIntActionTime DESC", typeof(qryAktenIntActionWithType));
                if (action != null)
                {
                    var protokolUbername = (tblProtokolUbername)HTBUtils.GetSqlSingleRecord($"SELECT * FROM tblProtokolUbername WHERE UbernameAktIntID = { akt.AktIntID } ORDER BY UbernameDatum DESC", typeof(tblProtokolUbername));

                    // create protocol in pdf
                    var fileName = "Protocol_" + protocol.ProtokolAkt + ".pdf";
                    var ms = new FileStream(HTBUtils.GetConfigValue("DocumentsFolder") + fileName, FileMode.OpenOrCreate);
                    var rpt = new ProtokolTablet();
                    rpt.GenerateProtokol(akt, protocol, protokolUbername, action, ms, GlobalUtilArea.GetVisitedDates(akt.AktIntID), GlobalUtilArea.GetPosList(akt.AktIntID));
                    ms.Close();
                    ms.Dispose();
                    SaveDocumentRecord(protocol.ProtokolAkt, fileName, akt.AktIntSB);
                }
            }
        }
        private void SaveDocumentRecord(int aktNumber, string fileName, int userId)
        {
            var doc = (tblDokument)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblDokument WHERE DokInkAkt = " + aktNumber + " AND DokAttachment = '" + fileName + "'", typeof(tblDokument));
            if (doc == null)
            {
                doc = new tblDokument
                {
                    // CollectionInvoice
                    DokDokType = 25,
                    DokCaption = "Protokol",
                    DokInkAkt = aktNumber,
                    DokCreator = userId,
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
            else
            {
                doc.DokChangeDate = DateTime.Now;
                RecordSet.Update(doc);
            }
        }
        private void NotifySB(qryAktenInt akt, tblAktenIntActionType actionType, double collectedAmount)
        {
            if(IsAutoAktion(actionType))
            {
                string subject = "ECP Vertrag " + akt.AktIntAZ;
                var receipients = new List<string>
                                 {
                                     HTBUtils.GetConfigValue("Default_EMail_Addr"),
                                     HTBUtils.GetConfigValue("Office_Email")
                                 };

                if (HTBUtils.IsValidEmail(akt.AKTIntKSVEMail))
                    receipients.Add(akt.AKTIntKSVEMail);

                string text = "";
                if (actionType.AktIntActionIsAutoMoneyCollected)
                {
                    text = HTBUtils.GetFileText(HTBUtils.GetConfigValue("Auto_CashCollected_text"));
                    text = text.Replace("[Recipient]", HTBUtils.GetEmailSalutationAndName(akt.AktIntAgSbType)+akt.AKTIntAGSB);
                    text = text.Replace("[Akt]", akt.AktIntAZ);
                    text = text.Replace("[Amount]", HTBUtils.FormatCurrency(collectedAmount));
                }
                else if (actionType.AktIntActionIsAutoRepossessed)
                {
                    text = HTBUtils.GetFileText(HTBUtils.GetConfigValue("Auto_Repossessed_text"));
                    text = text.Replace("[Recipient]", HTBUtils.GetEmailSalutationAndName(akt.AktIntAgSbType) + akt.AKTIntAGSB);
                    text = text.Replace("[Akt]", akt.AktIntAZ);
                }
                else
                {
                    text = HTBUtils.GetFileText(HTBUtils.GetConfigValue("Auto_Negative_text "));
                    text = text.Replace("[Recipient]", HTBUtils.GetEmailSalutationAndName(akt.AktIntAgSbType) + akt.AKTIntAGSB);
                    text = text.Replace("[Akt]", akt.AktIntAZ);
                }

                new HTBEmail().SendGenericEmail(receipients, subject, text);
            }
        }
        
        #endregion

    }
}
