using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using HTBUtilities;
using System.Collections;
using System.Text;
using HTB.Database.Views;
using HTBServices;
using HTBServices.Mail;

namespace HTB.v2.intranetx.aktenint
{
    public partial class SendMitteilung : System.Web.UI.Page
    {
        qryAktenInt aktInt = new qryAktenInt();
        ArrayList posList = new ArrayList();
        ArrayList invoicesList = new ArrayList();
        ArrayList actionsList = new ArrayList();
        int aktId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null && !Request.QueryString["ID"].Equals(""))
            {
                aktId = Convert.ToInt32(Request.QueryString["ID"]);
                LoadRecords();
                if (!IsPostBack)
                {
                    SetValues();
                }
            }
        }

        private void LoadRecords()
        {
            aktInt = (qryAktenInt)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenInt WHERE AktIntID = " + aktId, typeof(qryAktenInt));
            if (aktInt != null)
            {
                actionsList = HTBUtils.GetSqlRecords("SELECT * FROM qryInktAktAction WHERE AktIntActionAkt = " + aktId, typeof(qryInktAktAction));

                if (aktInt.IsInkasso())
                    invoicesList = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + aktId, typeof(tblCustInkAktInvoice));
                else
                    posList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntPos WHERE AktIntPosAkt = " + aktId, typeof(tblAktenIntPos));

            }
        }

        private void SetValues()
        {
            lblMessage.Text = "Zwischenbericht zum Akt wird an <strong>" + aktInt.AKTIntKSVEMail + "</strong> versendet!";
        }

        private void SendEmail()
        {
            ServiceFactory.Instance.GetService<IHTBEmail>().SendGenericEmail(aktInt.AKTIntKSVEMail, "Benachrichtigung: Interventionsakt " + aktInt.AktIntAZ, GetEmailBody(), false, aktInt.AktIntCustInkAktID);
        }
        private string GetEmailBody()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append("Sehr geehrter Geschäftspartner, \n\n");
            sb.Append("Interventionsakt " +aktInt.AktIntID + ":" + "\n" + "\n");
            sb.Append("Auftrag: " + "[AZ:" + aktInt.AktIntAZ + "]" + "\n");
            sb.Append("Inkassant: " + aktInt.UserVorname+ " " + aktInt.UserNachname + "\n");
            sb.Append("Termin: " + aktInt.AktIntTermin + "\n");
            sb.Append("Auftraggeber: " + aktInt.AuftraggeberName1 + "\n");
            sb.Append("Anlagedatum: " + aktInt.AktIntDatum.ToShortDateString() + "\n" + "\n");
            sb.Append("Klient: " + aktInt.KlientName1 + ", " + aktInt.KlientName2 + "\n");
            sb.Append(aktInt.KlientStrasse + "\n");
            sb.Append(aktInt.KlientLKZ + " " + aktInt.KlientPLZ + " " + aktInt.KlientOrt  + "\n" + "\n");
            sb.Append("Schuldner: " + aktInt.GegnerLastName1 + ", " + aktInt.GegnerLastName2 + "\n");
            sb.Append(aktInt.GegnerLastStrasse + "\n");
            sb.Append(aktInt.GegnerLastZipPrefix + " " + aktInt.GegnerLastZip + " " + aktInt.GegnerLastOrt + "\n" + "\n" + "\n");
            sb.Append("Tätigkeitsbericht: " + "\n");
            foreach(qryInktAktAction action in actionsList) {
            
                sb.Append(action.AktIntActionDate.ToShortDateString()+ ": " + action.AktIntActionTypeCaption + "\n");
                if (action.AktIntActionBetrag > 0)
                    sb.Append(" " + "Betrag: " + HTBUtils.FormatCurrency(action.AktIntActionBetrag) + " kassiert." + "\n");
                
            }

            if (DbUtil.FixDate(aktInt.AKTIntRVStartDate).ToShortDateString() != "01.01.1900")
            {
                sb.Append("Ratenzahlung beginnt am: " + aktInt.AKTIntRVStartDate + "\n");
                String rvType = "Persönliches CollectionInvoice";
                if (aktInt.AKTIntRVInkassoType == 0)
                {
                    rvType = "Erlagschein";
                }
                sb.Append("\n" + "Raten Zahlungsart: " + rvType + "\n");
                sb.Append("Ratenhöhe: " + aktInt.AKTIntRVAmmount + "\n");
                sb.Append("Anzahl Monate: " + aktInt.AKTIntRVNoMonth + "\n");
                sb.Append("Jeweils zum:" + aktInt.AKTIntRVIntervallDay + ". des Monats" + "\n");
                if (aktInt.AKTIntRVInfo != "")
                {
                    sb.Append("Zahlungsrhythmus: " + aktInt.AKTIntRVInfo + "\n");
                }
            }
            sb.Append("\n" + "Memo: " + aktInt.AKTIntMemo + "\n" + "\n");
            sb.Append("copyright 2009 E.C.P. - Alle Rechte vorbehalten");
            sb.Append("copyright 2009 ECP - Alle Rechte vorbehalten" + "\n" + "\n" + "\n" );
            sb.Append("Der Inhalt dieser E-Mail und aller enthaltenen Dateianhänge" + "\n");
            sb.Append("ist vertraulich und ausschließlich für den angegebenen Empfänger" + "\n");
            sb.Append("bestimmt. Es können Information enthalten sein, die rechtlich " + "\n");
            sb.Append("geschützt sind. Sollten Sie diese E-Mail fälschlicherweise erhalten" + "\n");
            sb.Append("haben, so ist die Offenlegung, die Reproduktion, das Kopieren, die " + "\n");
            sb.Append("Weitergabe, die Verteilung oder jegliche andere Verbreitung sowie" + "\n");
            sb.Append("Verwendung dieser E-Mail ausdrücklich untersagt. In diesem Fall " + "\n");
            sb.Append("ersuchen wir Sie, sich umgehend mit dem Absender der E-Mail in " + "\n");
            sb.Append("Verbindung zu setzen und diese E-Mail von Ihrem System zu löschen." + "\n");
            sb.Append("Der Absender übernimmt in Bezug auf die elektronische Übermittlung" + "\n" );
            sb.Append("keinerlei Gewährleistung sowie Haftung, für Inhalt, Richtigkeit und " + "\n");
            sb.Append("Vollständigkeit dieser E-Mail. Er übernimmt weiters keine Ver-" + "\n");
            sb.Append("antwortung für Veränderungen, die nach dem Versenden der E-Mail" + "\n");
            sb.Append("durch Dritte vorgenommen werden, oder andere Risiken welche auf" + "\n");
            sb.Append("Grund von Übertragung, Manipulation, Viren, etc. entstehen."	);
            return sb.ToString();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SendEmail();
            btnSubmit.Visible = false;
            lblMessage.Text = " Ein Zwischenbericht zum Akt wurde erfolgreich an <strong>" + aktInt.AKTIntKSVEMail + "</strong> versandt!";
        }
    }
}