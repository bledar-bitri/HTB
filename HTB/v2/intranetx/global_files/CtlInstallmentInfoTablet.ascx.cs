using System;
using System.IO;
using HTB.Database;
using HTB.Database.Views;
using HTBExtras;
using HTBReports;
using HTBUtilities;
using System.Collections;
using HTB.v2.intranetx.util;
using HTBAktLayer;

namespace HTB.v2.intranetx.global_files
{
    public partial class CtlInstallmentInfoTablet : System.Web.UI.UserControl
    {
       
        private AktUtils _aktUtils;
        private readonly tblControl _control = HTBUtils.GetControlRecord();
        public IInstallment parent { get; set; }

        #region Event Handler
        protected void btnGeneratePdf_Click(object sender, EventArgs e)
        {
            parent.SetInstallmentInfo();
            qryAktenInt akt = HTBUtils.GetInterventionAktQry(GetAktId());
            if (akt != null)
            {
                SaveGegnerInfo();
                var fileName = "Ratenansuchen_" + akt.AktIntID + ".pdf";
                var ms = new FileStream(HTBUtils.GetConfigValue("DocumentsFolder") + fileName, FileMode.OpenOrCreate);
                var rpt = new RatenansuchenIntTablet();
                rpt.GenerateRatenansuchen(akt, AktInterventionUtils.GetAktAmounts(akt), GetRateEntered(), ms, true);
                ms.Close();
                ms.Dispose();
//                Response.Redirect("/v2/intranet/documents/files/" + fileName);
                ResponseHelper.Redirect("/v2/intranet/documents/files/" + fileName, "_mahnung", "");
            }
            else
            {
                Response.Write("ERROR!!!");
            }
        }

        protected void lnkMoveAddress_Click(object sender, EventArgs e)
        {
            txtAddressPartner.Text = txtAddress.Text;
        }
        #endregion

        private tblAktenIntRatenansuchen GetRateEntered()
        {
            var rate = new tblAktenIntRatenansuchen
                           {
                               AktIntRateRequestRateAmount = GetInstallmentAmount(),
                               AktIntRateRequestStartDate = GetFirstInstallmentDate(),
                               AktIntRateRequestEndDate = GetLastInstallmentDate(),
                               AktIntRateRequestPayment = GetPayment(),
                               AktIntRateRequestName = txtName.Text,
                               AktIntRateRequestNamePartner = txtNamePartner.Text,
                               AktIntRateRequestAddress = txtAddress.Text,
                               AktIntRateRequestAddressPartner = txtAddressPartner.Text,
                               AktIntRateRequestPhone = (txtPhoneCity.Text.Trim() == string.Empty ? "" : txtPhoneCity.Text.Trim() + " ") + txtPhone.Text.Trim(),
                               AktIntRateRequestPhonePartner = txtPhonePartner.Text.Trim(),
                               AktIntRateRequestDOB = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtBirthday.Text),
                               AktIntRateRequestDOBPartner = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtBirthdayPartner.Text),
                               AktIntRateRequestSVA = txtSva.Text,
                               AktIntRateRequestSVAPartner = txtSvaPartner.Text,
                               AktIntRateRequestJob = txtJob.Text,
                               AktIntRateRequestJobPartner = txtJobPartner.Text,
                               AktIntRateRequestEmployer = txtEmployer.Text,
                               AktIntRateRequestEmployerPartner = txtEmployerPartner.Text
                           };
            return rate;
        }

        private void SaveGegnerInfo()
        {
            var gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerID = " + GetGegnerId(), typeof(tblGegner));
            if (gegner != null)
            {
                gegner.GegnerGebDat = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtBirthday.Text.Trim());
                gegner.GegnerSVANummer = txtSva.Text.Trim();
                gegner.GegnerBeruf = txtJob.Text.Trim();
                gegner.GegnerArbeitgeber = txtEmployer.Text.Trim();

                gegner.GegnerPhoneCity = txtPhoneCity.Text;
                gegner.GegnerPhone = txtPhone.Text;

                gegner.GegnerPartnerName = txtNamePartner.Text.Trim();
                gegner.GegnerPartnerAdresse = txtAddressPartner.Text.Trim();
                gegner.GegnerPartnerPhone = txtPhonePartner.Text.Trim();
                gegner.GegnerPartnerGebDat = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtBirthdayPartner.Text.Trim());
                gegner.GegnerPartnerSVANummer = txtSvaPartner.Text.Trim();
                gegner.GegnerPartnerBeruf = txtJobPartner.Text.Trim();
                gegner.GegnerPartnerArbeitgeber = txtEmployer.Text.Trim();

                RecordSet.Update(gegner);
            }
        }

        #region Getter / Setter

        public void SetAktId(int id)
        {
            hdnAktId.Text = id.ToString();
        }
        public int GetAktId()
        {
            return GlobalUtilArea.GetZeroIfConvertToIntError(hdnAktId.Text);
        }

        public void SetGegnerId(int id)
        {
            hdnGegnerId.Text = id.ToString();
        }
        public int GetGegnerId()
        {
            return GlobalUtilArea.GetZeroIfConvertToIntError(hdnGegnerId.Text);
        }
        
        public void SetInstallmentAmount(double amount)
        {
            hdnRateAmount.Text = amount.ToString();
        }
        public void SetFirstInstallmentDate(DateTime dte)
        {
            hdnFirstRate.Text = dte.ToShortDateString();
        }
        public void SetPayment(double amount)
        {
            hdnPaymentAmount.Text = amount.ToString();
        }
        public void SetLastInstallmentDate(DateTime dte)
        {
            hdnLastRate.Text = dte.ToShortDateString();
        }


        public void SetName(string name)
        {
            txtName.Text = name;
        }
        public void SetAddress(string address)
        {
            txtAddress.Text = address;
        }
        public void SetPhoneCity(string phoneCity)
        {
            txtPhoneCity.Text = phoneCity;
        }
        public void SetPhone(string phone)
        {
            txtPhone.Text = phone;
        }
        public void SetBirthday(DateTime dte)
        {
            if(dte != HTBUtils.DefaultDate)
                txtBirthday.Text = dte.ToShortDateString();
        }
        public void SetSVA(string sva)
        {
            txtSva.Text = sva;
        }
        public void SetProfession(string profession)
        {
            txtJob.Text = profession;
        }
        public void SetEmployer(string employer)
        {
            txtEmployer.Text = employer;
        }

        public void SetPartnerName(string name)
        {
            txtNamePartner.Text = name;
        }
        public void SetPartnerAddress(string address)
        {
            txtAddressPartner.Text = address;
        }
        public void SetPartnerPhoneNumber(string phone)
        {
            txtPhonePartner.Text = phone;
        }
        public void SetPartnerBirthday(DateTime dte)
        {
            if (dte != HTBUtils.DefaultDate)
                txtBirthdayPartner.Text = dte.ToShortDateString();
        }
        public void SetPartnerSVA(string sva)
        {
            txtSvaPartner.Text = sva;
        }
        public void SetPartnerProfession(string profession)
        {
            txtJobPartner.Text = profession;
        }
        public void SetPartnerEmployer(string employer)
        {
            txtEmployerPartner.Text = employer;
        }


        public double GetInstallmentAmount()
        {
            return GlobalUtilArea.GetZeroIfConvertToDoubleError(hdnRateAmount.Text);
        }
        public DateTime GetFirstInstallmentDate()
        {
            return GlobalUtilArea.GetDefaultDateIfConvertToDateError(hdnFirstRate.Text);
        }
        public double GetPayment()
        {
            return GlobalUtilArea.GetZeroIfConvertToDoubleError(hdnPaymentAmount.Text);
        }
        public DateTime GetLastInstallmentDate()
        {
            return GlobalUtilArea.GetDefaultDateIfConvertToDateError(hdnLastRate.Text);
        }

        public void SetGegnerInfo(tblGegner gegner)
        {
            SetGegnerId(gegner.GegnerID);

            SetName(gegner.GegnerName2 + " " + gegner.GegnerName1);
            SetAddress(gegner.GegnerLastStrasse + " " + gegner.GegnerLastZip + ", " + gegner.GegnerLastOrt);
            SetPhoneCity(gegner.GegnerPhoneCity.Trim());
            SetPhone(gegner.GegnerPhone.Trim());
            if (gegner.GegnerGebDat != HTBUtils.DefaultDate)
                SetBirthday(gegner.GegnerGebDat);
            SetSVA(gegner.GegnerSVANummer);
            SetProfession(gegner.GegnerBeruf);
            SetEmployer(gegner.GegnerArbeitgeber);

            SetPartnerName(gegner.GegnerPartnerName);
            SetPartnerAddress(gegner.GegnerPartnerAdresse);
            SetPartnerPhoneNumber(gegner.GegnerPartnerPhone.Trim());
            if (gegner.GegnerPartnerGebDat != HTBUtils.DefaultDate)
                SetPartnerBirthday(gegner.GegnerPartnerGebDat);
            SetPartnerSVA(gegner.GegnerPartnerSVANummer);
            SetPartnerProfession(gegner.GegnerPartnerBeruf);
            SetPartnerEmployer(gegner.GegnerPartnerArbeitgeber);
        }
        #endregion
    }
}