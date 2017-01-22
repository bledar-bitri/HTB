using System;
using System.Text;
using System.Web;
using System.Web.UI;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.dealer
{
    public partial class EditDealer : Page
    {
        private const string DefaultReturnUrl = "../../intranet/gegner/gegner.asp";
        private int _id;
        private tblAutoDealer _dealer;
        private bool _isNew = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            _isNew = true;
            _id = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.DEALER_ID]);
            if (_id > 0 && !HTBUtils.GetBoolValue(hdnIsAddNewClicked.Value))
            {
                _dealer = (tblAutoDealer)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAutoDealer WHERE AutoDealerID = " + _id, typeof(tblAutoDealer));
                _isNew = _dealer == null;
            }
            if(_isNew)
                _dealer = new tblAutoDealer();

            if (!IsPostBack)
            {
                SetValues();
            }
        }

        private void SetValues()
        {
            lblAutoDealerID.Text = !_isNew ? _dealer.AutoDealerID.ToString() : "(wird autom. vergeben)";
            
            txtAutoDealerName.Text = _dealer.AutoDealerName;
            txtAutoDealerStrasse.Text = _dealer.AutoDealerStrasse;
            txtAutoDealerLKZ.Text = _dealer.AutoDealerLKZ;
            txtAutoDealerPLZ.Text = _dealer.AutoDealerPLZ;
            txtAutoDealerOrt.Text = _dealer.AutoDealerOrt;
            txtPhoneCountry.Text = _dealer.AutoDealerPhoneCountry;
            txtPhoneCity.Text = _dealer.AutoDealerPhoneCity;
            txtPhone.Text = _dealer.AutoDealerPhone;
            txtFaxCountry.Text = _dealer.AutoDealerFaxCountry;
            txtFaxCity.Text = _dealer.AutoDealerFaxCity;
            txtFax.Text = _dealer.AutoDealerFax;
            txtEmail.Text = _dealer.AutoDealerEMail;
            txtAutoDealerWeb.Text = _dealer.AutoDealerWeb;
        }

        private bool SaveDealer()
        {
            _dealer.AutoDealerName = txtAutoDealerName.Text;
            _dealer.AutoDealerStrasse = txtAutoDealerStrasse.Text;
            _dealer.AutoDealerLKZ = txtAutoDealerLKZ.Text;
            _dealer.AutoDealerPLZ = txtAutoDealerPLZ.Text;
            _dealer.AutoDealerOrt = txtAutoDealerOrt.Text;
            _dealer.AutoDealerPhoneCountry = txtPhoneCountry.Text;
            _dealer.AutoDealerPhoneCity = txtPhoneCity.Text;
            _dealer.AutoDealerPhone = txtPhone.Text;
            _dealer.AutoDealerFaxCountry = txtFaxCountry.Text;
            _dealer.AutoDealerFaxCity = txtFaxCity.Text;
            _dealer.AutoDealerFax = txtFax.Text;
            _dealer.AutoDealerEMail = txtEmail.Text;
            _dealer.AutoDealerWeb = txtAutoDealerWeb.Text;
            try
            {
                if (_isNew)
                {
                    RecordSet.Insert(_dealer);
                    _dealer = (tblAutoDealer) HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblAutoDealer ORDER BY AutoDealerID DESC", typeof (tblAutoDealer));
                }
                RecordSet.Update(_dealer);
                ctlMessage.ShowSuccess("Handler gespeichert!");
                return true;
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
                return false;
            }
        }

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            hdnIsAddNewClicked.Value = false.ToString();
            try
            {
                if (IsEntryValid())
                {
                    if (SaveDealer())
                    {
                        if (!string.IsNullOrEmpty(Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT]))
                        {
                            Response.Redirect(
                                GetReturnToUrl(
                                    GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT]),
                                    GlobalUtilArea.DecodeUrl(GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.EXTRA_PARAMS])),
                                    _dealer
                                    )
                                );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            hdnIsAddNewClicked.Value = false.ToString();
            if (!string.IsNullOrEmpty(Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL]))
            {
                Response.Redirect(
                    GetReturnToUrl(
                        GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL]),
                        GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.EXTRA_PARAMS]),
                        null
                    )
                );
            }
            else
            {
                Response.Redirect(DefaultReturnUrl + Session["var"]);
            }
        }
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            _isNew = true;
            _dealer = new tblAutoDealer();
            hdnIsAddNewClicked.Value = true.ToString();
            SetValues();
        }
    
        

        protected void txtAutoDealerPLZ_TextChanged(object sender, EventArgs e)
        {
            txtAutoDealerOrt.Text = HTBUtils.GetCityForZip(txtAutoDealerPLZ.Text);
            txtAutoDealerOrt.Focus();
        }
        #endregion

        #region URL Creation
        private string GetReturnToUrl(string returnToUrlCode, string returnExtraParams, tblAutoDealer dealer)
        {
            var sb = new StringBuilder();
            switch (returnToUrlCode)
            {
                case GlobalHtmlParams.URL_NEW_AKT:
                    sb.Append("../aktenink/NewAktInk.aspx?");
                    break;
                case GlobalHtmlParams.URL_NEW_AKT_INT_AUTO:
                    sb.Append("../aktenint/EditAktIntAuto.aspx?");
                    break;
            }
            if (!sb.ToString().Trim().Equals(""))
            {
                AppendURLParams(sb, dealer, returnExtraParams);
                return sb.ToString();
            }
            return "";
        }

        private void AppendURLParams(StringBuilder sb, tblAutoDealer dealer, string returnExtraParams)
        {
            AppendURLParams(sb, dealer, returnExtraParams, true);
        }
        private void AppendURLParams(StringBuilder sb, tblAutoDealer dealer, string returnExtraParams, bool decodeExtraParams)
        {
            if (dealer != null)
            {
                sb.Append(GlobalHtmlParams.DEALER_ID);
                sb.Append("=");
                sb.Append(dealer.AutoDealerID.ToString());

            }
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT, Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT]);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.SEARCH_FOR, Request.QueryString[GlobalHtmlParams.SEARCH_FOR]);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, Request.QueryString[GlobalHtmlParams.ID]);
            if (returnExtraParams != "")
            {
                sb.Append("&");
                if (decodeExtraParams)
                    sb.Append(HttpUtility.UrlPathEncode(GlobalUtilArea.DecodeUrl(returnExtraParams)));
                else
                    GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.EXTRA_PARAMS, returnExtraParams);

            }
        }
        #endregion

        private bool IsEntryValid()
        {
            bool ok = true;
            var sb = new StringBuilder();
            if (txtAutoDealerName.Text.Trim() == "")
            {
                sb.Append("Firma eingeben!<br>");
                ok = false;
            }
            if (!ok)
            {
                ctlMessage.ShowError(sb.ToString());
            }
            return ok;
        }
    }
}