using System;
using System.Collections.Generic;
using System.IO;
using HTB.Database;
using HTB.Database.Views;
using HTB.v2.intranetx.util;
using HTBExtras.XML;
using HTBReports;
using HTBUtilities;

/*
 * NOTE: THIS PAGE IS NOT CALLED [USED] FROM IPAD
 *          IN LINE TO BE DELETED
 */
namespace HTB.v2.intranetx.aktenint.tablet
{
    public partial class AddActionTablet : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            int aktId = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Params[GlobalHtmlParams.INTERVENTION_AKT]);
            int actionTypeId = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Params[GlobalHtmlParams.ACTION_TYPE_ID]);
            string actionMemo = GlobalUtilArea.GetEmptyIfNull(Request.Params[GlobalHtmlParams.ACTION_MEMO]);

            double aktionLat = GlobalUtilArea.GetZeroIfConvertToDoubleError(GlobalUtilArea.GetEmptyIfNull(Request.Params[GlobalHtmlParams.LATITUDE]).Replace(".", ","));
            double aktionLng = GlobalUtilArea.GetZeroIfConvertToDoubleError(GlobalUtilArea.GetEmptyIfNull(Request.Params[GlobalHtmlParams.LONGITUDE]).Replace(".",","));

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
                var receipt = new XmlPrintableReceiptRecord();
                double collectedAmount =  GlobalUtilArea.GetDoubleAmountFromParameter(Request, GlobalHtmlParams.PAID);
                if (actionType.AktIntActionIsWithCollection)
                {
                    receipt = GetReceiptRecord(akt, CreateReceipt(akt, actionType, collectedAmount));
                }
                if (SaveAction(-1, actionType, akt, collectedAmount, aktionLat, aktionLng, actionMemo))
                {
                    akt.AKTIntMemo = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.MEMO]);
                    RecordSet.Update(akt);
                    if(actionType.AktIntActionIsAutoPayment)
                    {
                        InsertPosRecord(akt.AktIntID, actionType.AktIntActionTypeCaption, collectedAmount * -1);
                    }
                    Response.Write(receipt.ToXmlString());
                    
                }
                else
                {
                    Response.Write("Error:Die Aktion konnte nicht gespeichert werdern!\n");
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
        private bool SaveAction (int actionId, tblAktenIntActionType actionType, tblAktenInt akt, double collectedAmount, double lat, double lgn, string memo)
        {
            bool isNewAction = false;
            var aktAction = (tblAktenIntAction)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntAction WHERE AktIntActionID = " + actionId, typeof(tblAktenIntAction));
            if(aktAction == null)
            {
                aktAction = new tblAktenIntAction();
                isNewAction = true;
            }
            var ag = (tblAuftraggeber) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeber WHERE AuftraggeberID = " + akt.AktIntAuftraggeber, typeof (tblAuftraggeber));
            
            aktAction.AktIntActionAkt = akt.AktIntID;
            aktAction.AktIntActionSB = akt.AktIntSB;

            aktAction.AktIntActionType = actionType.AktIntActionTypeID;
            aktAction.AktIntActionProvision = 0;
            aktAction.AktIntActionHonorar = 0; // no more honorar (everything gets calculated into provision)
            aktAction.AktIntActionPrice = 0;
            aktAction.AktIntActionProvAbzug = ag.AuftraggeberIntAktPovAbzug;
            aktAction.AktIntActionDate = DateTime.Now;
            aktAction.AktIntActionTime = DateTime.Now;
            aktAction.AktIntActionBeleg = "";
            aktAction.AktIntActionReceiptID = 0;
            aktAction.AktIntActionBetrag = collectedAmount;

            aktAction.AktIntActionMemo = memo;
            aktAction.AktIntActionLatitude = lat;
            aktAction.AktIntActionLongitude = lgn;
            aktAction.AktIntActionAddress = GlobalUtilArea.GetAddressFromLatitudeAndLongitude(lat, lgn);

            if (actionType.AktIntActionIsExtensionRequest)
            {
//                aktAction.AktIntActionAktIntExtID = ctlExtension.SaveExtensionRequest(aktAction);
            }
            return isNewAction ? RecordSet.Insert(aktAction) : RecordSet.Update(aktAction);
        }
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

        #region POS
        private void InsertPosRecord(int aktid, string posCaption, double posAmount)
        {
            if (!HTBUtils.IsZero(posAmount))
            {
                RecordSet.Insert(new tblAktenIntPos
                {
                    AktIntPosAkt = aktid,
                    AktIntPosBetrag = posAmount,
                    AktIntPosCaption = posCaption,
                    AktIntPosDatum = DateTime.Now,
                    AktIntPosDueDate = DateTime.Now,
                    AktIntPosNr = "Zahlung"
                });
            }
        }
        #endregion
    }
}
