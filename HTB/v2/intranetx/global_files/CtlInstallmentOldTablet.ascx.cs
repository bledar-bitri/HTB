using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using HTB.v2.intranetx.aktenint;
using HTBUtilities;
using HTBAktLayer;
using System.Collections;
using HTB.v2.intranetx.util;
using System.Drawing;
using System.Text;

namespace HTB.v2.intranetx.global_files
{
    public partial class CtlInstallmentOldTablet : System.Web.UI.UserControl, IInstallment
    {
        tblAktenInt aktInt;
        private tblGegner gegner;
        public AuftragTablet Parent { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlInstallmentInfo.parent = this;
        }

        public void LoadAll()
        {
            aktInt = (tblAktenInt) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenInt WHERE AktIntID = " + GetAktIntId(), typeof (tblAktenInt));
            if (aktInt != null)
            {
                gegner = (tblGegner) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerOldID = " + aktInt.AktIntGegner, typeof (tblGegner));
                SetValues();
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
                ctlInstallmentInfo.SetGegnerInfo(gegner);
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
            aktInt = (tblAktenInt)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenInt WHERE AktIntID = " + GetAktIntId(), typeof(tblAktenInt));
            if (aktInt == null)
            {
                return false;
            }

            aktInt.AKTIntRVInkassoType = Convert.ToInt16(ddlZW.SelectedValue);
            aktInt.AKTIntRVStartDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDate.Text);
            aktInt.AKTIntRVAmmount = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtBetrag.Text);
            aktInt.AKTIntRVIntervallDay = GlobalUtilArea.GetZeroIfConvertToIntError(txtDay.Text);
            aktInt.AKTIntRVNoMonth = GlobalUtilArea.GetZeroIfConvertToIntError(txtDuration.Text);
            aktInt.AKTIntRVInfo = txtPaymentRythm.Text;
            if (IsEntryValid() && RecordSet.Update(aktInt))
            {
                ctlMessage.ShowSuccess("Raten gespechert!");
                return true;
            }
            return false;
        }

        public bool IsEntryValid()
        {
            var sb = new StringBuilder();
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
                ctlMessage.ShowError(sb.ToString());
            }
            return ok;
        }

        public void SetAktIntId(int id)
        {
            hdnAktIntId.Text = id.ToString();
            ctlInstallmentInfo.SetAktId(id);
        }
        public int GetAktIntId()
        {
            return GlobalUtilArea.GetZeroIfConvertToIntError(hdnAktIntId.Text);
        }

        public void SetInstallmentInfo()
        {
            DateTime firstInstallment = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDate.Text);
            ctlInstallmentInfo.SetInstallmentAmount(GlobalUtilArea.GetZeroIfConvertToDoubleError(txtBetrag.Text));
            ctlInstallmentInfo.SetFirstInstallmentDate(firstInstallment);
            ctlInstallmentInfo.SetPayment(GlobalUtilArea.GetZeroIfConvertToDoubleError(txtBetrag));
            ctlInstallmentInfo.SetLastInstallmentDate(firstInstallment.AddDays(GlobalUtilArea.GetZeroIfConvertToIntError(txtDuration.Text)));
            ctlInstallmentInfo.SetPayment(Parent.GetCollectedAmount());
        }
    }
}