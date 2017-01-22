using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Web;
using HTB.Database;
using HTB.Database.Views;
using HTB.v2.intranetx.util;
using HTBAktLayer;
using HTBExtras.XML;
using HTBReports;
using HTBUtilities;
using Microsoft.VisualBasic;

namespace HTB.v2.intranetx.aktenint.tablet
{
    public partial class DownloadAktInstallmentInfoTablet : System.Web.UI.Page
    {
        private static readonly tblControl Control = HTBUtils.GetControlRecord();
        private static readonly double AnnualInterestRate = Control.AnnualInterestRate / 100;
        private readonly string _signedInstallmentFilePrefix = HTBUtils.GetConfigValue("SignedInstallmentFilePrefix");

        protected void Page_Load(object sender, EventArgs e)
        {
            int aktId = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Params[GlobalHtmlParams.INTERVENTION_AKT]);
            string action = Request.Params[GlobalHtmlParams.ACTION_NAME];

            if (aktId <= 0)
            {
                Response.Write("Kein Aktenzahl!");
                return;
            }
            var akt = HTBUtils.GetInterventionAkt(aktId);
            if (akt == null)
            {
                Response.Write("Akt [" + aktId + "] Nicht Gefunden");
                return;
            }
            try
            {
                //Response.Write("Action: "+action);
                if (action == GlobalHtmlParams.ACTION_GET_INSTALLMENT_INFO)
                {
                    Response.Write(GetInstallmentInfo(akt).ToXmlString());
                }
                else if (action == GlobalHtmlParams.ACTION_CALCULATE_INSTALLMENT_BASED_ON_NUMBER_OF_INSTALLMENTS || action == GlobalHtmlParams.ACTION_CALCULATE_INSTALLMENT_BASED_ON_INSTALLMENT_AMOUNT)
                {
                    if (akt.IsInkasso())
                    {
                        var utils = new AktUtils(akt.AktIntCustInkAktID);
                        DateTime startDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(Request.Params[GlobalHtmlParams.START_DATE]);
                        string installmentPeriod = Request.Params[GlobalHtmlParams.INSTALLMENT_PERIOD];
                        double paid = GlobalUtilArea.GetDoubleAmountFromParameter(Request, GlobalHtmlParams.PAID);
                        double balance = utils.GetAktBalance() - paid;
//                        Response.Write("\n===========================\n\n");
//                        Response.Write(paid);
//                        Response.Write("\n===========================\n\n");
//                        Response.Write(Request.Params[GlobalHtmlParams.PAID].Replace(".", "").Replace(",", "."));
//                        Response.Write("\n===========================\n\n");
//                        Response.Write(Request.Params[GlobalHtmlParams.PAID]);
                        if (action == GlobalHtmlParams.ACTION_CALCULATE_INSTALLMENT_BASED_ON_NUMBER_OF_INSTALLMENTS)
                        {
                            int noi = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Params[GlobalHtmlParams.NUMBER_OF_INSTALLMENTS]);

                            var rec = CalculateBasedOnNubmerOfInstallments(startDate, installmentPeriod, akt.AktIntCustInkAktID, noi, balance);
                            Response.Write(rec.ToXmlString());
                        }
                        else
                        {
                            double installmentAmount = GlobalUtilArea.GetDoubleAmountFromParameter(Request, GlobalHtmlParams.INSTALLMENT_AMOUNT);
                            var rec = CalculateBasedOnInstallmentAmount(startDate, installmentPeriod, akt.AktIntCustInkAktID, installmentAmount, balance);
                            Response.Write(rec.ToXmlString());
                        }
                    }
                    else
                    {
                        Response.Write("ERROR: Akt ["+aktId+"] ist kein CollectionInvoice Akt");
                    }
                }
                else if (action == GlobalHtmlParams.ACTION_GENERATE_RV_PDF)
                {
//                    Response.Write("action: generate pdf\n");
                    qryAktenInt qryAkt = HTBUtils.GetInterventionAktQry(aktId);
                    tblGegner gegner = LoadGegnerInfo(qryAkt.GegnerID);
                    byte[] signature = null;
                    byte[] partnerSignature = null;
                    if (gegner != null)
                    {
//                        Response.Write("gegner not null\n");
//                        GlobalUtilArea.PrintFormParameters(Request, Response);
                        SaveGegnerInfo(gegner);
                        try
                        {
                            HttpFileCollection uploadFiles = Request.Files;
                            // Loop over the uploaded files and save to disk.
                            int i;
//                            Response.Write("files count"+uploadFiles.Count+"\n");
                            for (i = 0; i < uploadFiles.Count; i++)
                            {
                                HttpPostedFile postedFile = uploadFiles[i];

                                // Access the uploaded file's content in-memory:
                                Stream inStream = postedFile.InputStream;
                                var fileData = new byte[postedFile.ContentLength];
                                inStream.Read(fileData, 0, postedFile.ContentLength);

//                                string imageName = aktId.ToString() + "_" + DateTime.Now.ToString().Replace(" ", "-").Replace(":", "") + "_" + postedFile.FileName;
                                
                                // Save the posted file in our "data" virtual directory.
//                                postedFile.SaveAs(HTBUtils.GetConfigValue("DocumentsFolder") + imageName);
//                                Response.Write(imageName+"\n");
                                if (i == 0) 
                                    signature = fileData;
                                else 
                                    partnerSignature = fileData;
                            }
//                            Response.Write("OKK");
                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex.Message);
                            Response.Write(ex.StackTrace);
                        }
                        var fileName = _signedInstallmentFilePrefix + akt.AktIntID + ".pdf";
                        string filePath = HTBUtils.GetConfigValue("DocumentsFolder") + fileName;
                        HTBUtils.DeleteFile(filePath);
                        var ms = new FileStream(filePath, FileMode.OpenOrCreate);
                        var rpt = new RatenansuchenIntTablet();
                        rpt.GenerateRatenansuchen(qryAkt, AktInterventionUtils.GetAktAmounts(qryAkt), GetRate(), ms, signature, partnerSignature);
                        ms.Close();
                        ms.Dispose();
                        Thread.Sleep(1000);
                        SaveDocumentRecord(qryAkt.AktIntID, fileName);
                        Response.Write(Request.Url.Scheme + "://" + Request.Url.Host + "/v2/intranet/documents/files/" + fileName);
                    }
                    else
                    {
                        Response.Write("ERROR: Schuldner Nicht Gefunden!");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("EXCEPTION: "+ex.Message+"<br/>");
                Response.Write(ex.StackTrace);
            }
        }

        #region Installment Info
        private XmlAktInstallmentRecord GetInstallmentInfo(tblAktenInt aktInt)
        {
            if (aktInt.IsInkasso())
            {
                var utils = new AktUtils(aktInt.AktIntCustInkAktID);
                return new XmlAktInstallmentRecord { Balance = utils.GetAktBalance(), OriginalInvoiceAmount = utils.GetAktOriginalInvoiceAmount() };
            }
            return new XmlAktInstallmentRecord { Balance = GetInterventionTotalDue(aktInt), OriginalInvoiceAmount = 0 };
        }

        private double GetInterventionTotalDue(tblAktenInt aktInt)
        {
            double ret = 0;

            if (aktInt != null)
            {
                ArrayList posList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntPos WHERE AktIntPosAkt = " + aktInt.AktIntID, typeof(tblAktenIntPos));
                foreach (tblAktenIntPos AktIntPos in posList)
                {
                    ret += AktIntPos.AktIntPosBetrag;
                }
                ret += aktInt.AKTIntZinsenBetrag;
                ret += aktInt.AKTIntKosten;
                ret += aktInt.AktIntWeggebuehr;
            }
            return ret;
        }

        private void SaveDocumentRecord(int aktNumber, string fileName)
        {
            var doc = new tblDokument
            {
                // CollectionInvoice
                DokDokType = 25,
                DokCaption = "Unterschriebene RV",
                DokInkAkt = aktNumber,
                DokCreator = Control.AutoUserId,
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
        #endregion

        #region Calculate Installments

        private XmlAktInstallmentCalcRecord CalculateBasedOnNubmerOfInstallments(DateTime startDate, string installmentPeriod, int aktId, int noi, double balance)
        {
            var rec = new XmlAktInstallmentCalcRecord
                          {
                              CollectedAmount = GlobalUtilArea.GetDoubleAmountFromParameter(Request, GlobalHtmlParams.PAID)
                          };
            int interestPeriod = GetInterestPeriod(installmentPeriod);
            if (noi > 0)
            {
                rec.StartDate = startDate;
                DateTime endDate = startDate.AddMonths(noi - 1);
                if (noi == 1)
                {
                    rec.InstallmentAmount = balance;
                    rec.LastInstallmentAmount = balance;
                }
                else
                {
                    rec.InstallmentAmount = Financial.Pmt(AnnualInterestRate / interestPeriod, noi, -balance);
                    rec.LastInstallmentAmount = Financial.PPmt(AnnualInterestRate / interestPeriod, noi, noi, -balance);
                }
                rec.EndDate = endDate;
                rec.TotalInterest = ((rec.InstallmentAmount * (noi - 1)) + rec.LastInstallmentAmount - balance);
                LoadInstallmentsList(rec.InstallmentsList, startDate, aktId, installmentPeriod, noi, rec.InstallmentAmount, rec.LastInstallmentAmount);
            }
            else
            {
                Response.Write("ERROR: Invalid Number Of Installments");
            }
            return rec;
        }
        
        private XmlAktInstallmentCalcRecord CalculateBasedOnInstallmentAmount(DateTime startDate, string installmentPeriod, int aktId, double installmentAmount, double balance)
        {
            var rec = new XmlAktInstallmentCalcRecord
                          {
                              CollectedAmount = GlobalUtilArea.GetDoubleAmountFromParameter(Request, GlobalHtmlParams.PAID)
                          };
            var interestPeriod = GetInterestPeriod(installmentPeriod);
            rec.InstallmentAmount = installmentAmount;
            int numberOfInstallments;
            double latestInstallment;
            double totalInterest;

            HTBUtils.CalculateInstallmentPlanBasedOnPaymentAmount(balance, installmentAmount, AnnualInterestRate, interestPeriod, out numberOfInstallments, out latestInstallment, out totalInterest);
            rec.NumberOfInstallments = numberOfInstallments;
            rec.TotalInterest = totalInterest;
            LoadInstallmentsList(rec.InstallmentsList, startDate, aktId, installmentPeriod, rec.NumberOfInstallments, rec.InstallmentAmount, rec.LastInstallmentAmount);
            rec.EndDate = startDate.AddMonths(rec.NumberOfInstallments);
            return rec;
        }
        private int GetInterestPeriod(string installmentPeriod)
        {
            if (installmentPeriod == GlobalHtmlParams.INSTALLMENT_PERIOD_WEEKLY)
            {
                return 52;
            }
            return 12;
        }
       
        private void LoadInstallmentsList(ArrayList list, DateTime date, int aktId, string installmentPeriod, int numberOfInstallments, double normalAmount, double lastAmount)
        {
            list.Clear();
            for (int i = 0; i < numberOfInstallments; i++)
            {
                var installment = new tblCustInkAktRate
                {
                    CustInkAktRateAmount = i < numberOfInstallments - 1 ? normalAmount : lastAmount,
                    CustInkAktRateAktID = aktId,
                    CustInkAktRateDate = DateTime.Now,
                    CustInkAktRateDueDate = date
                };
                list.Add(installment);
                date = installmentPeriod == GlobalHtmlParams.INSTALLMENT_PERIOD_WEEKLY ? date.AddDays(7) : date.AddMonths(1);
            }
        }
        
        #endregion

        #region Installment PDF

        private tblGegner LoadGegnerInfo(int gegnerId)
        {
            var gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerID = " + gegnerId, typeof(tblGegner));
            if (gegner != null)
            {
                gegner.GegnerGebDat = GlobalUtilArea.GetDefaultDateIfConvertToDateError(Request.Params[GlobalHtmlParams.INSTALLMENT_RV_DATE_OF_BIRTH]);
                gegner.GegnerSVANummer = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_SVA];
                gegner.GegnerBeruf = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_JOB_DESCRIPTION];
                gegner.GegnerArbeitgeber = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_EMPLOYER];

                gegner.GegnerPhoneCountry = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_PHONE_COUNTRY];
                gegner.GegnerPhoneCity = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_PHONE_CITY];
                gegner.GegnerPhone = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_PHONE];

                gegner.GegnerPartnerName = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_NAME_PARTNER];
                gegner.GegnerPartnerAdresse = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_ADDRESS_PARTNER];
                gegner.GegnerPartnerPhone = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_PHONE_PARTNER];
                gegner.GegnerPartnerGebDat = GlobalUtilArea.GetDefaultDateIfConvertToDateError(Request.Params[GlobalHtmlParams.INSTALLMENT_RV_DATE_OF_BIRTH_PARTNER]);
                gegner.GegnerPartnerSVANummer = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_SVA_PARTNER];
                gegner.GegnerPartnerBeruf = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_JOB_DESCRIPTION_PARTNER];
                gegner.GegnerPartnerArbeitgeber = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_EMPLOYER_PARTNER];
                return gegner;
            }
            return null;
        }

        private void SaveGegnerInfo(tblGegner gegner)
        {
            if (gegner != null)
            {
                RecordSet.Update(gegner);
            }
        }

        private tblAktenIntRatenansuchen GetRate()
        {
            var rate = new tblAktenIntRatenansuchen
            {
                AktIntRateRequestRateAmount = GlobalUtilArea.GetDoubleAmountFromParameter(Request, GlobalHtmlParams.INSTALLMENT_AMOUNT),
                AktIntRateRequestStartDate =GlobalUtilArea.GetDefaultDateIfConvertToDateError(Request.Params[GlobalHtmlParams.START_DATE]),
                AktIntRateRequestEndDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(Request.Params[GlobalHtmlParams.END_DATE]),
                AktIntRateRequestPayment = GlobalUtilArea.GetDoubleAmountFromParameter(Request, GlobalHtmlParams.PAID),
                AktIntRateRequestName = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_NAME],
                AktIntRateRequestNamePartner = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_NAME_PARTNER],
                AktIntRateRequestAddress = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_ADDRESS],
                AktIntRateRequestAddressPartner = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_ADDRESS_PARTNER],
                AktIntRateRequestPhone = (Request.Params[GlobalHtmlParams.INSTALLMENT_RV_PHONE_COUNTRY].Trim() == string.Empty ? "" : Request.Params[GlobalHtmlParams.INSTALLMENT_RV_PHONE_COUNTRY].Trim() + " ") +
                                         (Request.Params[GlobalHtmlParams.INSTALLMENT_RV_PHONE_CITY].Trim() == string.Empty ? "" : Request.Params[GlobalHtmlParams.INSTALLMENT_RV_PHONE_CITY].Trim() + " ") +
                                         Request.Params[GlobalHtmlParams.INSTALLMENT_RV_PHONE].Trim(),
                AktIntRateRequestPhonePartner = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_PHONE_PARTNER].Trim(),
                AktIntRateRequestDOB = GlobalUtilArea.GetDefaultDateIfConvertToDateError(Request.Params[GlobalHtmlParams.INSTALLMENT_RV_DATE_OF_BIRTH]),
                AktIntRateRequestDOBPartner = GlobalUtilArea.GetDefaultDateIfConvertToDateError(Request.Params[GlobalHtmlParams.INSTALLMENT_RV_DATE_OF_BIRTH_PARTNER]),
                AktIntRateRequestSVA = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_SVA],
                AktIntRateRequestSVAPartner = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_SVA_PARTNER],
                AktIntRateRequestJob = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_JOB_DESCRIPTION],
                AktIntRateRequestJobPartner = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_JOB_DESCRIPTION_PARTNER],
                AktIntRateRequestEmployer = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_EMPLOYER],
                AktIntRateRequestEmployerPartner = Request.Params[GlobalHtmlParams.INSTALLMENT_RV_EMPLOYER_PARTNER]
            };
            return rate;
        }

        #endregion

        private string GetIndex(string fileName)
        {
            int idx = fileName.IndexOf("_");
            if (idx > 0)
            {
                string fname = fileName.Substring(idx + 1);
                idx = fname.IndexOf(".");
                if (idx > 0)
                    return fname.Substring(0, idx);
            }
            return "";
        }
    }
}