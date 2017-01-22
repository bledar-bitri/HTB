using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using HTBUtilities;
using HTBAktLayer;
using System.Collections;
using HTB.v2.intranetx.util;
using System.Drawing;
using System.Text;

namespace HTB.v2.intranetx.global_files
{
    public partial class CtlInstallmentOld : System.Web.UI.UserControl
    {
        tblAktenInt aktInt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["INTID"] != null && !Request["INTID"].Equals(""))
            {
                int aktId = 0;
                try
                {
                    aktId = Convert.ToInt32(Request["INTID"]);
                }
                catch { }
                aktInt = (tblAktenInt)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenInt WHERE AktIntID = " + aktId, typeof(tblAktenInt));
                if (!IsPostBack)
                {
                    SetValues();
                }
            }
        }

        private void SetValues()
        {
            if (aktInt != null)
            {
                lblTotal.Text = HTBUtils.FormatCurrencyNumber(GetTotalDue());
                ddlZW.SelectedValue = aktInt.AKTIntRVInkassoType.ToString();
                txtDate.Text = aktInt.AKTIntRVStartDate.ToShortDateString() == "01.01.1900" ? "" : aktInt.AKTIntRVStartDate.ToShortDateString();
                txtBetrag.Text = aktInt.AKTIntRVAmmount == 0 ? "" : HTBUtils.FormatCurrencyNumber(aktInt.AKTIntRVAmmount);
                txtDay.Text = aktInt.AKTIntRVIntervallDay == 0 ? "" : aktInt.AKTIntRVIntervallDay.ToString();
                txtDuration.Text = aktInt.AKTIntRVNoMonth == 0 ? "" : aktInt.AKTIntRVNoMonth.ToString();
                txtPaymentRythm.Text = aktInt.AKTIntRVInfo;
            }
        }

        public double GetTotalDue()
        {
            double ret = 0;

            if (aktInt != null)
            {
                ArrayList posList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntPos WHERE AktIntPosAkt = " + aktInt.AktIntID, typeof(tblAktenIntPos));
                foreach (HTB.Database.tblAktenIntPos AktIntPos in posList)
                {
                    ret += AktIntPos.AktIntPosBetrag;
                }
                ret += aktInt.AKTIntZinsenBetrag;
                ret += aktInt.AKTIntKosten;
                ret += aktInt.AktIntWeggebuehr;
            }
            return ret;
        }

        public bool SaveInstallment()
        {
            if (aktInt == null)
            {
                return false;
            }
            else
            {
                aktInt.AKTIntRVInkassoType = Convert.ToInt16(ddlZW.SelectedValue);
                aktInt.AKTIntRVStartDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDate.Text);
                aktInt.AKTIntRVAmmount = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtBetrag.Text);
                aktInt.AKTIntRVIntervallDay = GlobalUtilArea.GetZeroIfConvertToIntError(txtDay.Text);
                aktInt.AKTIntRVNoMonth = GlobalUtilArea.GetZeroIfConvertToIntError(txtDuration.Text);
                aktInt.AKTIntRVInfo = txtPaymentRythm.Text;
                if (IsEntryValid() && RecordSet.Update(aktInt))
                {
                    ShowSuccess("Raten gespechert!");
                    return true;
                }
            }
            return false;
        }

        public bool IsEntryValid()
        {
            StringBuilder sb = new StringBuilder();
            bool ok = true;
            if (aktInt.AKTIntRVInkassoType != 0 && aktInt.AKTIntRVInkassoType != 1)
            {
                sb.Append("<i><strong>Zahlungsweise</strong></i> ausw&auml;hlen<br>");
                ok = false;
            }
            if (aktInt.AKTIntRVStartDate.ToShortDateString() == "01.01.1900")
            {
                sb.Append("<i><strong>Beginndatum</strong></i> eingeben<br>");
                ok = false;
            }
            if (aktInt.AKTIntRVAmmount <= 0)
            {
                sb.Append("<i><strong>Betrag/Monat</strong></i> eingeben<br>");
                ok = false;
            }
            if (!ok)
            {
                ShowError(sb.ToString());
            }
            return ok;
        }

        private void ShowError(String message)
        {
            lblMessage.ForeColor = Color.Red;
            lblMessage.Text = message;
            lblMessage.Visible = true;
            trMessage.Visible = true;
        }

        private void ShowSuccess(String message)
        {
            lblMessage.ForeColor = Color.Green;
            lblMessage.Text = message;
            lblMessage.Visible = true;
            trMessage.Visible = true;
        }
    }
}