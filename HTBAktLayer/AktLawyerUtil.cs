using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using HTB.Database;
using HTB.Database.LookupRecords;
using HTB.Database.StoredProcs;
using HTB.Database.Views;
using HTBExcel;
using HTBServices;
using HTBServices.Mail;
using HTBUtilities;
using HTBXmlToPdf;

namespace HTBAktLayer
{
    public class AktLawyerUtil
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly AktUtils _aktUtils;
        private List<tblCustInkAktInvoice> invoiceList;
        public AktLawyerUtil(AktUtils aktU)
        {
            _aktUtils = aktU;
        }

        public void SendInkassoPackageToLawyer(HTBEmailAttachment zwischenbericht)
        {
            SendInkassoPackageToLawyer(zwischenbericht, false, null);
        }

        public void SendInkassoPackageToLawyer(HTBEmailAttachment zwischenbericht, bool saveCopyToFolder, string folderPath)
        {
            var attachments = new List<HTBFileAttachment> { new HTBFileAttachment(zwischenbericht) };
            try
            {
                var lawyerPdfAttachmentName = HTBUtils.GetConfigValue("Lawyer_Pdf_File_Name");
                const string excelCostAttachmentName = "Kosten.xls";
                invoiceList = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + _aktUtils.InkAktId, typeof(tblCustInkAktInvoice)).Cast<tblCustInkAktInvoice>().ToList();
            
                var akt = (qryCustInkAkt)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryCustInkAkt WHERE CustInkAktID = " + _aktUtils.InkAktId, typeof(qryCustInkAkt));
                var lawyer = (tblLawyer)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblLawyer WHERE LawyerId = " + akt.CustInkAktLawyerId, typeof(tblLawyer));

                #region Generate new attachments

                AddExcelFile(attachments, excelCostAttachmentName);

                AddLawyerLetterIfNeeded(attachments, akt, lawyer, lawyerPdfAttachmentName);

                #endregion

                var attachmentsToZip = attachments.ToList();
                AddAdditionalDocumentsToZip(attachmentsToZip);
                var zipEntries = attachmentsToZip.Distinct().Select(attachment => new HTBZipEntry(new MemoryStream(((MemoryStream)attachment.AttachmentStream).ToArray()), attachment.AttachmentStreamName)).ToList();
                var zipStream = new MemoryStream();
                HTBZip.CreateZipFile(zipStream, zipEntries);

                #region Send Email
                var to = HTBUtils.GetValidEmailAddressesFromStrings(new[] { lawyer.LawyerEmail, HTBUtils.GetConfigValue("Office_Email") });
                var mailAttachment = new List<HTBEmailAttachment>
                {
                    new HTBEmailAttachment(new MemoryStream(zipStream.ToArray()), HTBUtils.GetConfigValue("Lawyer_Package_Attachment_Name"), "application/zip"),
                    attachments.FirstOrDefault(att => att.AttachmentStreamName == lawyerPdfAttachmentName)
                };

                if (to.Count > 0)
                    //new HTBEmail().SendLawyerPackage(to, akt, attachments,  GetLawyerEMailBody(akt, lawyer));
                    ServiceFactory.Instance.GetService<IHTBEmail>().SendLawyerPackage(to, akt, mailAttachment, GetLawyerEMailBody(akt, lawyer));
                #endregion

                #region Attach Documents To Case

                var listOfAttachmentNamesToSaveAsDocuments = new List<string>
                {
                    lawyerPdfAttachmentName,
                    excelCostAttachmentName
                };
                SaveAttachmetsAsDocuments(attachments, listOfAttachmentNamesToSaveAsDocuments, saveCopyToFolder, folderPath);

                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private string GetLawyerEMailBody(qryCustInkAkt akt, tblLawyer lawyer)
        {

            var text = HTBUtils.GetFileText(HTBUtils.GetConfigValue("Lawyer_EMail_Text"));
            var klient = (tblKlient)HTBUtils.GetSqlSingleRecord(string.Format("SELECT * FROM tblKlient WHERE KlientID = {0}", akt.KlientID), typeof (tblKlient));
            var versicherungsNummer = "";
            if (klient != null && !string.IsNullOrEmpty(klient.KlientVersicherung))
                versicherungsNummer = string.Format("Versicherung: {0} Polizzennummer: {1}", klient.KlientVersicherung, klient.KlientPolizzennummer);
            if (lawyer != null)
            {
                var sb = new StringBuilder();
                if (lawyer.LawyerSex == 1)
                    sb.Append("r " + lawyer.LawyerAnrede + " " + lawyer.LawyerName1);
                else
                    sb.Append(" " + lawyer.LawyerAnrede + " " + lawyer.LawyerName1);
                text = text.Replace("[Recipient]", sb.ToString());
            }
            text = text.Replace("[Gegner]", akt.GegnerName1 + " " + akt.GegnerName2);
            text = text.Replace("[Akt]", akt.CustInkAktID.ToString());
            text = text.Replace("[Forderung]", HTBUtils.FormatCurrency(_aktUtils.GetAktBalance()));
            text = text.Replace("[ClientInsuranceInfo]", versicherungsNummer);
            return text;
        }

        private void AddExcelFile(List<HTBFileAttachment> attachments, string attachmentName)
        {

            var spParams = new ArrayList
                                   {
                                       new StoredProcedureParameter("aktId", SqlDbType.Int, _aktUtils.InkAktId)
                                   };
            var list = HTBUtils.GetMultipleListsFromStoredProcedure("spGetInfoForLawyer", spParams, new[] { typeof(spGetInfoForLawyerAkt), typeof(tblCustInkAktInvoice) });

            var excelMs = new MemoryStream();
            new LawyerExcelGenerator().WriteExcelFile(excelMs, list);
            excelMs.Seek(0, SeekOrigin.Begin);
            attachments.Add(new HTBFileAttachment(excelMs, attachmentName, "application/ms-excel", "Rechtsanwalt Kosten"));
        }

        private void AddLawyerLetterIfNeeded(List<HTBFileAttachment> attachments, qryCustInkAkt akt, tblLawyer lawyer, string pdfName)
        {
            var letterPath = GetLetterPath(akt, lawyer);
            if (letterPath == null) return;
            var path = Path.Combine(HTBUtils.GetConfigValue("Lawyer_Letters_Path"), letterPath);
            var fileText = HTBUtils.GetFileText(path);
            fileText = ReplaceInformation(lawyer.LawyerID, akt, fileText);
            var lawyerMs = new MemoryStream();
            new XmlToPdfWriter().GeneratePdf(lawyerMs, fileText);
            attachments.Add(new HTBFileAttachment(new MemoryStream(lawyerMs.ToArray()), pdfName, "application/pdf", HTBUtils.GetConfigValue("Lawyer_Pdf_Description")));
        }

        private string GetLetterPath(qryCustInkAkt akt, tblLawyer lawyer)
        {
            if (lawyer == null)
                return null;
            var letters = HTBUtils.GetSqlRecords(string.Format("SELECT * FROM tblLawyerLetter WHERE LawyerLetterLawyerId = {0}", lawyer.LawyerID),typeof (tblLawyerLetter)).Cast<tblLawyerLetter>().ToList();
            if (!letters.Any()) return null;
            if (letters.Count() > 1)
            {
                var paymentPlan = HTBUtils.GetSqlRecords(string.Format("SELECT * FROM tblCustInkAktRate WHERE CustInkAktRateAktID = {0}",akt.CustInkAktID), typeof (tblCustInkAktRate)).Cast<tblCustInkAktRate>().ToList();
                if (paymentPlan.Count() > 1)
                {
                    if (GetInvoiceAmount(InvoiceSumType.Payment) > 0)
                    {
                        var installmentLetter = letters.FirstOrDefault(letter => letter.LawyerLetterType == "Installment_With_Payment");
                        if (installmentLetter != null)
                            return installmentLetter.LawyerLetterPath;
                    }
                }
                var defaultLetter = letters.FirstOrDefault(letter => letter.LawyerLetterType == "");
                if (defaultLetter != null)
                    return defaultLetter.LawyerLetterPath;
            }
            return letters[0].LawyerLetterPath;

        }

        private void SaveDataToFile(byte[] data, string fileName)
        {
            SaveDataToFile(data, HTBUtils.GetConfigValue("DocumentsFolder"), fileName);
        }
        private void SaveDataToFile(byte[] data, string folderName, string fileName)
        {
            try
            {
                var file = new FileStream(folderName + fileName, FileMode.Create);
                file.Write(data, 0, data.Length);
                file.Flush();
                file.Close();
                file.Dispose();
            }
            catch (Exception)
            {
                string user = HTBUtils.GetConfigValue("ImpersonatorUser");
                string domain = HTBUtils.GetConfigValue("ImpersonatorDomain");
                string password = HTBUtils.GetConfigValue("ImpersonatorPassword");

                using (new Impersonator(user, domain, password))
                {
                    var file = new FileStream(folderName + fileName, FileMode.Create);
                    file.Write(data, 0, data.Length);
                    file.Flush();
                    file.Close();
                    file.Dispose();
                }
            }
        }

        private void SaveAttachmetsAsDocuments(IEnumerable<HTBFileAttachment> attachments, List<string> attachmentNamesToSave, bool saveCopyToFolder, string folderPath)
        {
            int userId = _aktUtils.control.AutoUserId;
            foreach (var attachment in attachments)
            {
                var fileName = string.Format("{0}_{1}", _aktUtils.InkAktId, attachment.AttachmentStreamName);
                if (saveCopyToFolder)
                    SaveDataToFile(((MemoryStream)attachment.AttachmentStream).ToArray(), folderPath, fileName);

                if (attachmentNamesToSave.Any(attachmentName => attachmentName == attachment.AttachmentStreamName))
                {
                    SaveDataToFile(((MemoryStream)attachment.AttachmentStream).ToArray(), fileName);
                    HTBUtils.CreateInkassoDocumentRecord(_aktUtils.InkAktId, attachment.AttachmentDescription, fileName, userId);
                }
                try
                {
                    attachment.AttachmentStream.Close();
                    attachment.AttachmentStream.Dispose();
                }
                catch
                {
                }
            }
        }
        private List<string> GetDebtorLines(qryCustInkAkt akt, out string debtorName, out string firstLine)
        {
            var ret = new List<string>();
            switch (akt.GegnerType)
            {
                case 0:
                    firstLine = "Sehr geehrte Damen und Herren!";
                    debtorName = akt.GegnerName1 + " " + (akt.GegnerName2 ?? akt.GegnerName2);
                    ret.Add("Firma");
                    ret.Add(debtorName);
                    break;
                case 1:
                    firstLine = string.Format("Sehr geehrter Herr {0}!", akt.GegnerName1);
                    debtorName = akt.GegnerName2 + " " + (akt.GegnerName1 ?? akt.GegnerName1);
                    ret.Add("Herrn ");
                    ret.Add(debtorName);
                    break;
                case 2:
                    firstLine = string.Format("Sehr geehrte Frau {0}!", akt.GegnerName1);
                    debtorName = akt.GegnerName2 + " " + (akt.GegnerName1 ?? akt.GegnerName1);
                    ret.Add("Frau ");
                    ret.Add(debtorName);
                    break;
                default:
                    firstLine = "";
                    debtorName = "";
                    ret.Add("Fehler Schuldner Typ is nicht definiert!!!");
                    ret.Add("");
                    break;

            }
            ret.Add(akt.GegnerLastStrasse);
            ret.Add(akt.GegnerLastZip + " " + akt.GegnerLastOrt);
            return ret;
        }

        private static bool IsDebtorAPerson(qryCustInkAkt akt)
        {
            return akt.GegnerType > 0;
        }

        private string GetClientName(qryCustInkAkt akt)
        {
            var client = HTBUtils.GetKlientById(akt.KlientID);
            if (string.IsNullOrEmpty(client.KlientAnrede) && string.IsNullOrEmpty(client.KlientTitel))
                return client.KlientName1 + (client.KlientName2.Trim().Length > 0 ? " " + client.KlientName2 : "");
            var sb = new StringBuilder();
            sb.Append(string.IsNullOrEmpty(client.KlientAnrede) ? "" : client.KlientAnrede + " ");
            sb.Append(string.IsNullOrEmpty(client.KlientTitel) ? "" : client.KlientTitel + " ");
            sb.Append(string.IsNullOrEmpty(client.KlientName1) ? "" : client.KlientName1 + " ");
            sb.Append(string.IsNullOrEmpty(client.KlientName2) ? "" : client.KlientName2 + " ");
            sb.Append(string.IsNullOrEmpty(client.KlientName3) ? "" : client.KlientName3 + " ");
            return sb.ToString();

        }
        
        private string ReplaceInformation(int lawyerId, qryCustInkAkt akt, string xmlText)
        {
            string debtorName;
            string salutationLine;
            var debtorIno = GetDebtorLines(akt, out debtorName, out salutationLine);
            var originalInvoiceAmount = GetInvoiceAmount(InvoiceSumType.OpenedAmountOfOriginalInvoice);
            var paidAmount = GetInvoiceAmount(InvoiceSumType.Payment);
            var collectionAmount = GetInvoiceAmount(InvoiceSumType.OpenedAmountOfCollectionInvoices);
            collectionAmount += GetInvoiceAmount(InvoiceSumType.OpenedAmountOfClientCost);
            var invoicePlusCollectionAmount = originalInvoiceAmount + collectionAmount;
            var interestAmount = GetInterestAmount(akt);
            var lawyerFeeAmount = GetLawyerCost(lawyerId, invoicePlusCollectionAmount);
            var lawyerFeeTax = Math.Round(lawyerFeeAmount*0.2, 2);
            var totalAmount = invoicePlusCollectionAmount + interestAmount + lawyerFeeAmount + lawyerFeeTax - paidAmount;
            
            return xmlText
                .Replace("[DebtorAddressLine1]", debtorIno[0])
                .Replace("[DebtorAddressLine2]", debtorIno[1])
                .Replace("[DebtorAddressLine3]", debtorIno[2])
                .Replace("[DebtorAddressLine4]", debtorIno[3])
                .Replace("[DebtorName_Long]", debtorName)
                .Replace("[SalutationLine]", salutationLine)
                .Replace("[ClientName]", GetClientName(akt))
                .Replace("[TodaysDate]", DateTime.Now.ToShortDateString())
                .Replace("[InvoiceDate]", akt.CustInkAktInvoiceDate.ToShortDateString())
                .Replace("[InvoiceAmount]", HTBUtils.FormatCurrency(originalInvoiceAmount))
                .Replace("[PaidAmount]", HTBUtils.FormatCurrency(paidAmount))
                .Replace("[-PaidAmount]", HTBUtils.FormatCurrencyNumber(paidAmount*-1))
                .Replace("[CollectionAmount]", HTBUtils.FormatCurrency(collectionAmount))
                .Replace("[InvoicePlusCollectionAmount1]", HTBUtils.FormatCurrency(invoicePlusCollectionAmount))
                .Replace("[InvoicePlusCollectionAmount2]", HTBUtils.FormatCurrencyNumber(invoicePlusCollectionAmount))
                .Replace("[InterestRate]", HTBUtils.FormatPercent(GetInterestRate(akt) / 100, false))
                .Replace("[InterestEndDate]", DateTime.Now.ToShortDateString())
                .Replace("[LawyerInterestAmount]", HTBUtils.FormatCurrencyNumber(interestAmount))
                .Replace("[LawyerFeeAmount]", HTBUtils.FormatCurrencyNumber(lawyerFeeAmount + lawyerFeeTax))
                .Replace("[LawyerFeeTax]", HTBUtils.FormatCurrencyNumber(lawyerFeeTax))
                .Replace("[TotalAmount]", HTBUtils.FormatCurrencyNumber(totalAmount))
                .Replace("[DueDate]", DateTime.Now.AddDays(14).ToShortDateString())
                .Replace("&", "&amp;")
                ;
        }
        private double GetInvoiceAmount(InvoiceSumType invoiceSumType)
        {
            double ret = 0;
            foreach (var inv in invoiceList)
            {
                
                if (invoiceSumType == InvoiceSumType.Interest && inv.IsInterest())
                {
                    ret += inv.InvoiceAmount;
                }
                else if (!inv.IsInterest())
                {
                    if (invoiceSumType == InvoiceSumType.Payment && inv.IsPayment())
                    {
                        ret += inv.InvoiceAmount;
                    }
                    else if ((invoiceSumType == InvoiceSumType.OriginalInvoice ||
                              invoiceSumType == InvoiceSumType.OpenedAmountOfOriginalInvoice) &&
                             (inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_ORIGINAL))
                    {
                        if (invoiceSumType == InvoiceSumType.OriginalInvoice)
                            ret += inv.InvoiceAmount;
                        else
                            ret += inv.InvoiceBalance;
                    }
                    else if ((invoiceSumType == InvoiceSumType.ClientCost ||
                              invoiceSumType == InvoiceSumType.OpenedAmountOfClientCost) &&
                             (inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_CLIENT_COST))
                    {
                        if (invoiceSumType == InvoiceSumType.ClientCost)
                            ret += inv.InvoiceAmount;
                        else
                            ret += inv.InvoiceBalance;
                    }
                    else if ((invoiceSumType == InvoiceSumType.CollectionInvoice ||
                              invoiceSumType == InvoiceSumType.OpenedAmountOfCollectionInvoices) &&
                             (!inv.IsPayment()) &&
                             (inv.InvoiceType != tblCustInkAktInvoice.INVOICE_TYPE_ORIGINAL) &&
                             (inv.InvoiceType != tblCustInkAktInvoice.INVOICE_TYPE_CLIENT_COST)
                        )
                    {
                        if (invoiceSumType == InvoiceSumType.CollectionInvoice)
                            ret += inv.InvoiceAmount;
                        else
                            ret += inv.InvoiceBalance;
                    }
                    else if (invoiceSumType == InvoiceSumType.OpenedAll)
                    {
                        if (inv.IsPayment())
                            ret -= inv.InvoiceAmount;
                        else
                            ret += inv.InvoiceAmount;
                    }
                }
            }
            return ret;
        }

        private void AddAdditionalDocumentsToZip(List<HTBFileAttachment> attachmentsToZip)
        {
            var documentsFolder = HTBUtils.GetConfigValue("DocumentsFolder");
            var additionalDocs = HTBUtils.GetDocumentsForInkassoAkt(_aktUtils.InkAktId);
            var distinctAttachments = additionalDocs.Select(d => d.DokAttachment).Distinct().ToList();
            attachmentsToZip.AddRange(from attachment in distinctAttachments
                select additionalDocs.FirstOrDefault(d => d.DokAttachment == attachment)
                into doc
                where doc != null
                where
                    doc.DokAttachment.EndsWith(".pdf") || doc.DokAttachment.EndsWith(".xls") ||
                    doc.DokAttachment.EndsWith(".doc")
                let bytes =
                    doc.DokAttachment.ToLower().StartsWith("http")
                        ? HTBUtils.GetUrlData(doc.DokAttachment)
                        : HTBUtils.GetFileData(Path.Combine(documentsFolder, doc.DokAttachment))
                where bytes != null
                select
                    new HTBFileAttachment(
                        new MemoryStream(bytes),
                        HTBUtils.GetJustFileName(doc.DokAttachment.ToLower().StartsWith("http")
                            ? string.Format("{0}.{1}", doc.DokCaption, HTBUtils.GetFileExtenssion(doc.DokAttachment))
                            : doc.DokAttachment),
                        "",
                        doc.DokCaption));
        }
        private double GetLawyerCost(int lawyerId, double owedAmount)
        {
            var lawyerCost = (tblLawyerCost)HTBUtils.GetSqlSingleRecord(string.Format("SELECT * from tblLawyerCost WHERE LawyerCostLawyerid = {0} AND LawyerCostFrom <= {1} AND LawyerCostTo >= {1}", lawyerId, owedAmount.ToString(CultureInfo.InvariantCulture)), typeof(tblLawyerCost));
            if (lawyerCost == null)
                return 0;
            var ret = owedAmount * lawyerCost.LawyerCostPercent / 100;
            ret += lawyerCost.LawyerCostAmount;
            return ret;
        }

        private double GetInterestRate(qryCustInkAkt akt)
        {
            return IsDebtorAPerson(akt) ? Convert.ToDouble(HTBUtils.GetConfigValue("Interest_Rate_For_People")) : _aktUtils.control.AnnualInterestRate;
        }

        private double GetInterestAmount(qryCustInkAkt akt)
        {
            var interestAmount = GetInvoiceAmount(InvoiceSumType.Interest);
            if (!IsDebtorAPerson(akt))
                return interestAmount;

            var interestRatePeople = Convert.ToDouble(HTBUtils.GetConfigValue("Interest_Rate_For_People"));
            return interestAmount / _aktUtils.control.AnnualInterestRate * interestRatePeople;
        }
    }
}
