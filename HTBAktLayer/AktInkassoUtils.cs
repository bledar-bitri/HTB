using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using HTB.Database;
using HTBUtilities;
using System.Reflection;
using HTB.Database.Views;

namespace HTBAktLayer
{
    public class AktInkassoUtils
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int _currentAktId = -1;
        private ArrayList _aktInvoicesList = new ArrayList();
        private readonly AktUtils _aktUtils;

        public AktInkassoUtils(AktUtils aktU)
        {
            _aktUtils = aktU;
            _currentAktId = _aktUtils.InkAktId;
            LoadAllInvoicesForAkt();
        }
        public double GetDistance(string sdZip)
        {
            double distance = 0;
            int adSB = 32; // Thomas Jaky
            string adZip = "5020";
            int EarthRadius = 6380; // KM
            log.Info("Calculating Distance for PLZ: " + sdZip);

            qryADGebiete qryGebiete = (qryADGebiete)HTBUtils.GetSqlSingleRecord("SELECT * FROM dbo.qryADGebiete WHERE ADGEBIETSTARTZIP <= " + sdZip + " AND ADGEBIETENDZIP >= " + sdZip, typeof(qryADGebiete));
            if (qryGebiete != null)
            {
                adSB = qryGebiete.ADGebietUser;
                adZip = qryGebiete.UserZIP;
                tblOrte ortSB = (tblOrte)HTBUtils.GetSqlSingleRecord("SELECT * FROM dbo.tblOrte WHERE " + HTBUtils.GetZipWhere(adZip), typeof(tblOrte));
                tblOrte ortGE = (tblOrte)HTBUtils.GetSqlSingleRecord("SELECT * FROM dbo.tblOrte WHERE " + HTBUtils.GetZipWhere(sdZip), typeof(tblOrte));
                if (ortGE != null && ortSB != null)
                {
                    if (!HTBUtils.IsZero(ortSB.B) && !HTBUtils.IsZero(ortGE.B) && !HTBUtils.IsZero(ortSB.L) && !HTBUtils.IsZero(ortGE.L))
                    {
                        log.Info(" Sachbearbeiter Ort: " + ortSB.Ort + "\n Laenge: " + ortSB.L + "\n Breite: " + ortSB.B);
                        log.Info(" Gegner Ort: " + ortGE.Ort + "\n Laenge: " + ortGE.L + "\n Breite: " + ortGE.B);
                        double A_Lon = ortSB.B * (3.1415925 / 180);
                        double B_Lon = ortGE.B * (3.1415925 / 180);
                        double A_Lat = ortSB.L * (3.1415925 / 180);
                        double B_Lat = ortGE.L * (3.1415925 / 180);
                        distance = ArcCos(ArcCos(Math.Sin(B_Lat) * Math.Sin(A_Lat) + Math.Cos(B_Lat) * Math.Cos(A_Lat) * Math.Cos(B_Lon - A_Lon)) * EarthRadius);
                    }
                }
            }
            return distance;
        }

        public double GetDistance(tblUser user, string sdZip)
        {
            double distance = 0;
            if (user != null)
            {
                string adZip = "5020";
                int EarthRadius = 6380; // KM
                log.Info("Calculating Distance for PLZ: " + sdZip);

                adZip = user.UserZIP;
                tblOrte ortSB = (tblOrte)HTBUtils.GetSqlSingleRecord("SELECT * FROM dbo.tblOrte WHERE " + HTBUtils.GetZipWhere(adZip), typeof(tblOrte));
                tblOrte ortGE = (tblOrte)HTBUtils.GetSqlSingleRecord("SELECT * FROM dbo.tblOrte WHERE " + HTBUtils.GetZipWhere(sdZip), typeof(tblOrte));
                if (ortGE != null && ortSB != null)
                {
                    if (ortSB.B != 0 && ortGE.B != 0 && ortSB.L != 0 && ortGE.L != 0)
                    {
                        log.Info(" Sachbearbeiter Ort: " + ortSB.Ort + "\n Laenge: " + ortSB.L + "\n Breite: " + ortSB.B);
                        log.Info(" Gegner Ort: " + ortGE.Ort + "\n Laenge: " + ortGE.L + "\n Breite: " + ortGE.B);
                        double A_Lon = ortSB.B * (3.1415925 / 180);
                        double B_Lon = ortGE.B * (3.1415925 / 180);
                        double A_Lat = ortSB.L * (3.1415925 / 180);
                        double B_Lat = ortGE.L * (3.1415925 / 180);
                        log.Info(" A_Lon: " + A_Lon + "\n B_Lon: " + B_Lon + "\n A_Lat: " + A_Lat + "\n B_Lat: " + B_Lat);
                        distance = ArcCos(Math.Sin(B_Lat) * Math.Sin(A_Lat) + Math.Cos(B_Lat) * Math.Cos(A_Lat) * Math.Cos(B_Lon - A_Lon)) * EarthRadius;
                    }
                }
            }
            return distance;
        }

        public double GetWeggebuehr(int distance)
        {

            tblWege wege = (tblWege)HTBUtils.GetSqlSingleRecord("SELECT * FROM dbo.tblWege where WegVon <= " + distance + " AND WegBis >= " + distance, typeof(tblWege));
            if (wege != null)
            {
                return wege.Preis;
            }
            return 0;
        }

        public double ArcCos(double x)
        {
            try
            {
                return Math.Atan(-x / Math.Sqrt(-x * x + 1)) + 2 * Math.Atan(1);
            }
            catch
            {
                return 0;
            }
        }

        #region Amounts
        public double GetAktOriginalInvoiceAmount()
        {
            double rett = 0;
            foreach (tblCustInkAktInvoice inv in _aktInvoicesList)
            {
                if (inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_ORIGINAL)
                {
                    rett += inv.InvoiceAmount;
                }
            }
            return rett;
        }
        public double GetAktOriginalInvoiceBalance()
        {
            return (from tblCustInkAktInvoice inv in _aktInvoicesList where inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_ORIGINAL select inv.InvoiceBalance).Sum();
        }

        public double GetAktTotalInvoiceAmount()
        {
            return (from tblCustInkAktInvoice inv in _aktInvoicesList where !inv.IsPayment() select inv.InvoiceAmount).Sum();
        }

        public double GetAktTotalPayments()
        {
            return (from tblCustInkAktInvoice inv in _aktInvoicesList where inv.IsPayment() select inv.InvoiceAmount).Sum();
        }

        public double GetAktBalance()
        {
            return (from tblCustInkAktInvoice inv in _aktInvoicesList where !inv.IsPayment() select inv.InvoiceBalance).Sum();
        }

        public double GetAktTotalInterest()
        {
            return (from tblCustInkAktInvoice inv in _aktInvoicesList where inv.IsInterest() select inv.InvoiceAmount).Sum();
        }

        public double GetAktTotalInterestBalance()
        {
            return (from tblCustInkAktInvoice inv in _aktInvoicesList where !inv.IsInterest() select inv.InvoiceBalance).Sum();
        }

        public double GetAktKlientCostsBalance()
        {
            return (from tblCustInkAktInvoice inv in _aktInvoicesList where inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_CLIENT_COST select inv.InvoiceBalance).Sum();
        }

        public double GetAktKlientTotalBalance()
        {
            return (from tblCustInkAktInvoice inv in _aktInvoicesList where inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_CLIENT_COST || inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_ORIGINAL select inv.InvoiceBalance).Sum();
        }

        public double GetAktTotalCollectionBalance()
        {
            return (from tblCustInkAktInvoice inv in _aktInvoicesList where !inv.IsClientInvoice() && !inv.IsPayment() && !inv.IsInterest() select inv.InvoiceBalance).Sum();
        }

        public double GetAktTotalCollectionAmount()
        {
            return (from tblCustInkAktInvoice inv in _aktInvoicesList where !inv.IsClientInvoice() && !inv.IsPayment() && !inv.IsInterest() select inv.InvoiceAmount).Sum();
        }

        public double GetAktTotalCollectionNettoAmount()
        {
            return (from tblCustInkAktInvoice inv in _aktInvoicesList where !inv.IsClientInvoice() && !inv.IsPayment() && !inv.IsInterest() select inv.InvoiceAmountNetto).Sum();
        }

        public double GetAktTotalTax()
        {
            return (from tblCustInkAktInvoice inv in _aktInvoicesList where !inv.IsClientInvoice() && !inv.IsPayment() && !inv.IsInterest() select inv.InvoiceTax).Sum();
        }

        #endregion

        public void CreateAktion(int type, int userId)
        {
            CreateAktion(type, userId, "");
        }
        public void CreateAktion(int type, int userId, string memo)
        {
            var aktion = new tblCustInkAktAktion();
            var kz = (tblKZ)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKZ WHERE KZID = " + type, typeof(tblKZ));

            if (kz != null)
            {
                aktion.CustInkAktAktionCaption = kz.KZCaption;
                if (kz.KZKZ == "INT" && _aktUtils.IntAktUtils != null && _aktUtils.IntAktUtils.IntAkt != null)
                {
                    aktion.CustInkAktAktionCaption += ":  " + _aktUtils.IntAktUtils.IntAkt.AktIntID;
                }
            }
            if (memo != null)
                aktion.CustInkAktAktionMemo = memo;
            
            aktion.CustInkAktAktionTyp = type;
            aktion.CustInkAktAktionDate = DateTime.Now;
            aktion.CustInkAktAktionEditDate = DateTime.Now;
            aktion.CustInkAktAktionUserId = userId;
            aktion.CustInkAktAktionAktID = _currentAktId;
            RecordSet.Insert(aktion);
        }

        public bool IsLaywerInWorkflow()
        {
            var count = (SingleValue)HTBUtils.GetSqlSingleRecord("SELECT COUNT(*) [IntValue] FROM tblWFA WHERE WFPAkt = " + _currentAktId, typeof(SingleValue));
            if (count.IntValue > 0)
            {
                count = (SingleValue) HTBUtils.GetSqlSingleRecord("SELECT COUNT(*) [IntValue] FROM tblWFA WHERE WFPAktion = 14 AND WFPAkt = " + _currentAktId, typeof (SingleValue));
                return count.IntValue > 0;
            }


            var akt = HTBUtils.GetInkassoAkt(_currentAktId);
            count = (SingleValue)HTBUtils.GetSqlSingleRecord("SELECT COUNT(*) [IntValue] FROM tblWFK WHERE WFPAktion = 14 AND WFPKlient = " + akt.CustInkAktKlient, typeof(SingleValue));

            return count.IntValue > 0;
        }

        private void LoadAllInvoicesForAkt()
        {
            _aktInvoicesList = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + _currentAktId, typeof(tblCustInkAktInvoice));
        }
    }
}
