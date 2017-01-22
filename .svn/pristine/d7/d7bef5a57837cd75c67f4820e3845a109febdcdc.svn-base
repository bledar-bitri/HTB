using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web;
using HTB.Database;
using HTB.Database.HTB.Views;
using HTB.v2.intranetx.util;
using HTBUtilities;
using System.Web.UI;

namespace HTB.v2.intranetx.gegner
{
    public partial class EditGegnerPhone : Page
    {
        private int _phoneId, _gegnerId;
        private tblGegnerPhone _gphone;
        private bool _isNew = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            _isNew = true;
            _phoneId = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.PHONE_ID]);
            _gegnerId = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.GEGNER_ID]);
            if(_gegnerId <= 0)
            {
                ctlMessage.ShowError("Schuldner ID nicht empfangen!");
                ddlPhoneType.Enabled = false;
                txtPhoneCountry.Enabled = false;
                txtPhoneCity.Enabled = false;
                txtPhone.Enabled = false;
                txtDescription.Enabled = false;
                btnSubmit.Enabled = false;
                btnAddNew.Enabled = false;

            }
            if (_phoneId > 0 && !HTBUtils.GetBoolValue(hdnIsAddNewClicked.Value))
            {
                _gphone = (tblGegnerPhone)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegnerPhone WHERE GPhoneID = " + _phoneId, typeof(tblGegnerPhone));
                _isNew = _gphone == null;
            }
            if (_isNew)
                _gphone = new tblGegnerPhone();

            if(!IsPostBack)
            {
                GlobalUtilArea.LoadDropdownList(ddlPhoneType,
                       "SELECT * FROM tblPhoneType ORDER BY PhoneTypeSequence ASC",
                       typeof(tblPhoneType),
                       "PhoneTypeID",
                       "PhoneTypeCaption", false);
                SetValues();
            }
        }

        private void SetValues()
        {
            if(!_isNew)
                ddlPhoneType.SelectedValue = _gphone.GPhoneType.ToString();
            else
                ddlPhoneType.SelectedIndex = 0;
            
            txtPhoneCountry.Text = _gphone.GPhoneCountry;
            txtPhoneCity.Text = _gphone.GPhoneCity;
            txtPhone.Text = _gphone.GPhone;
            txtDescription.Text = _gphone.GPhoneDescription;
            PopulateGrid();
        }

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            hdnIsAddNewClicked.Value = false.ToString();
            try
            {
                _gphone.GPhoneGegnerID = _gegnerId;
                _gphone.GPhoneType = Convert.ToInt32(ddlPhoneType.SelectedValue);
                _gphone.GPhoneCountry = txtPhoneCountry.Text;
                _gphone.GPhoneCity = txtPhoneCity.Text;
                _gphone.GPhone = txtPhone.Text;
                _gphone.GPhoneDescription = txtDescription.Text;
                _gphone.GPhoneDate = DateTime.Now;
                bool ok = _isNew ? RecordSet.Insert(_gphone) : RecordSet.Update(_gphone);
                if(ok)
                {
                    ClearScreen();
                    PopulateGrid();
                }
                else
                {
                    ctlMessage.ShowError("Info nicht gespeichert! :(");
                }
            }
            catch(Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            hdnIsAddNewClicked.Value = false.ToString();
            if (GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL]).Equals(GlobalHtmlParams.CLOSE_WINDOW))
            {
                ScriptManager.RegisterStartupScript(updPanel1, typeof(string), "closeScript", "window.close();", true);
            }
            else
            {
                Response.Redirect(
                    GetReturnToUrl(
                        GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL]),
                        GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.EXTRA_PARAMS]),
                        null
                        )
                    );
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            _isNew = true;
            _gphone = new tblGegnerPhone();
            hdnIsAddNewClicked.Value = true.ToString();
            SetValues();
        }

        #endregion

        public void ClearScreen()
        {
            ddlPhoneType.SelectedIndex = 0;
            txtPhoneCountry.Text = "";
            txtPhoneCity.Text = "";
            txtPhone.Text = "";
        }

        #region Grid
        private void PopulateGrid()
        {
            ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM qryGegnerPhone WHERE GPhoneGegnerID = " + _gegnerId + " ORDER BY PhoneTypeSequence", typeof(qryGegnerPhone));
            DataTable dt = GetDataTableStructure();
            foreach (qryGegnerPhone phone in list)
            {
                DataRow dr = dt.NewRow();

                var sb = new StringBuilder("<a href=\"EditGegnerPhone.aspx?");
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb,GlobalHtmlParams.PHONE_ID, phone.GPhoneID.ToString(), false);
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb,GlobalHtmlParams.GEGNER_ID, _gegnerId.ToString());
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, Request[GlobalHtmlParams.ID]); 
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL, Request[GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL]);
                
                sb.Append("\">");
                sb.Append("<img src=\"../../intranet/images/edit.gif\" width=\"16\" height=\"16\" alt=\"&Auml;ndern diesen Datensatz.\" style=\"border-color:White;border-width:0px;\"/>");
                sb.Append("</a>");
                dr["EditUrl"] = sb.ToString();
                dr["PhoneNumber"] = phone.GPhoneCountry + " " + phone.GPhoneCity + " " + phone.GPhone;
                dr["PhoneType"] = phone.PhoneTypeCaption;
                dr["Description"] = phone.GPhoneDescription;
                

                dt.Rows.Add(dr);
            }
            gvGegnerPhones.DataSource = dt;
            gvGegnerPhones.DataBind();
        }
        
        private DataTable GetDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("EditUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("PhoneNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("PhoneType", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            return dt;
        }
        #endregion

        #region URL Creation
        private string GetReturnToUrl(string returnToUrlCode, string returnExtraParams, tblGegner gegner)
        {
            var sb = new StringBuilder();
            switch (returnToUrlCode)
            {
                case GlobalHtmlParams.URL_NEW_AKT_INT_AUTO:
                    sb.Append("../aktenint/EditAktIntAuto.aspx?");
                    AppendURLParams(sb, gegner, returnExtraParams);
                    return sb.ToString();
                case GlobalHtmlParams.URL_EDIT_GEGNER:
                    sb.Append("/v2/intranet/gegner/editgegner.asp?");
                    AppendURLParams(sb, gegner, returnExtraParams);
                    return sb.ToString();

            }
            return "";
        }

        private void AppendURLParams(StringBuilder sb, tblGegner gegner, string returnExtraParams)
        {
            AppendURLParams(sb, gegner, returnExtraParams, true);
        }
        private void AppendURLParams(StringBuilder sb, tblGegner gegner, string returnExtraParams, bool decodeExtraParams)
        {
            if (gegner != null)
            {
                sb.Append(GlobalHtmlParams.GEGNER_ID);
                sb.Append("=");
                sb.Append(gegner.GegnerID.ToString());

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
    }
}