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

namespace HTB.v2.intranetx.lawyer
{
    public partial class EditLawyer : Page
    {
        private int _lawyerId;
        private tblLawyer _lawyer;
        private bool _isNew = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            _isNew = true;
            _lawyerId = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.LAWYER_ID]);
            if (_lawyerId > 0 && !HTBUtils.GetBoolValue(hdnIsAddNewClicked.Value))
            {
                _lawyer = (tblLawyer)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblLawyer WHERE LawyerID = " + _lawyerId, typeof(tblLawyer));
                _isNew = _lawyer == null;
            }
            if (_isNew)
                _lawyer = new tblLawyer();

            if(!IsPostBack)
            {
                SetValues();
            }
        }

        private void SetValues()
        {
            try
            {
                ddlGender.SelectedValue = _lawyer.LawyerSex.ToString();
            }
            catch
            {
            }
            txtAnrede.Text = _lawyer.LawyerAnrede; 
            txtName1.Text = _lawyer.LawyerName1;
            txtName2.Text = _lawyer.LawyerName2;
            txtStreet.Text = _lawyer.LawyerStrasse;
            txtZipPrefix.Text = _lawyer.LawyerZipPrefix;
            txtZip.Text = _lawyer.LawyerZip;
            txtCity.Text = _lawyer.LawyerOrt;
            txtPhoneCountry.Text = _lawyer.LawyerPhoneCountry;
            txtPhoneCity.Text = _lawyer.LawyerPhoneCity;
            txtPhone.Text = _lawyer.LawyerPhone;
            txtFaxCountry.Text = _lawyer.LawyerFaxCountry;
            txtFaxCity.Text = _lawyer.LawyerFaxCity;
            txtFax.Text = _lawyer.LawyerFax;

            txtEmail.Text = _lawyer.LawyerEmail;
                
            PopulateGrid();
        }

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            hdnIsAddNewClicked.Value = false.ToString();
            try
            {
                LoadLawyerFromScreen();
                bool ok = _isNew ? RecordSet.Insert(_lawyer) : RecordSet.Update(_lawyer);
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
            _lawyer = new tblLawyer();
            hdnIsAddNewClicked.Value = true.ToString();
            SetValues();
        }

        #endregion

        private void ClearScreen()
        {
            _lawyer = new tblLawyer();
            SetValues();
        }

        private void LoadLawyerFromScreen()
        {
            _lawyer.LawyerSex = GlobalUtilArea.GetZeroIfConvertToIntError(ddlGender.SelectedValue);
            _lawyer.LawyerAnrede = txtAnrede.Text;
            _lawyer.LawyerName1 = txtName1.Text;
            _lawyer.LawyerName2 = txtName2.Text;
            _lawyer.LawyerStrasse = txtStreet.Text;
            _lawyer.LawyerZipPrefix = txtZipPrefix.Text;
            _lawyer.LawyerZip = txtZip.Text;
            _lawyer.LawyerOrt = txtCity.Text;
            _lawyer.LawyerPhoneCountry = txtPhoneCountry.Text;
            _lawyer.LawyerPhoneCity = txtPhoneCity.Text;
            _lawyer.LawyerPhone = txtPhone.Text;
            _lawyer.LawyerFaxCountry = txtFaxCountry.Text;
            _lawyer.LawyerFaxCity = txtFaxCity.Text;
            _lawyer.LawyerFax = txtFax.Text;

            _lawyer.LawyerEmail = txtEmail.Text;
        }

        #region Grid
        private void PopulateGrid()
        {
            ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM tblLawyer ORDER BY LawyerName1", typeof(tblLawyer));
            DataTable dt = GetDataTableStructure();
            foreach (tblLawyer lawyer in list)
            {
                DataRow dr = dt.NewRow();

                var sb = new StringBuilder("<a href=\"EditLawyer.aspx?");
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb,GlobalHtmlParams.LAWYER_ID, lawyer.LawyerID.ToString(), false);
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, Request[GlobalHtmlParams.ID]); 
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL, Request[GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL]);
                
                sb.Append("\">");
                sb.Append("<img src=\"../../intranet/images/edit.gif\" width=\"16\" height=\"16\" alt=\"&Auml;ndern diesen Datensatz.\" style=\"border-color:White;border-width:0px;\"/>");
                sb.Append("</a>");
                dr["EditUrl"] = sb.ToString();
                
                sb.Clear();
                sb.Append(string.IsNullOrEmpty(lawyer.LawyerAnrede) ? "" : lawyer.LawyerAnrede+" ");
                sb.Append(lawyer.LawyerName1);
                sb.Append(" ");
                sb.Append(lawyer.LawyerName2);

                dr["Name"] = sb.ToString();
                dr["Address"] = lawyer.LawyerStrasse+"<BR/>"+lawyer.LawyerZipPrefix+"-"+lawyer.LawyerZip+" "+lawyer.LawyerOrt;
                dr["Phone"] = lawyer.LawyerPhoneCountry + " " + lawyer.LawyerPhoneCity + " " + lawyer.LawyerPhone;
                dr["Fax"] = lawyer.LawyerFaxCountry + " " + lawyer.LawyerFaxCity + " " + lawyer.LawyerFax;
                dr["Email"] = lawyer.LawyerEmail;
                
                dt.Rows.Add(dr);
            }
            gvGegnerPhones.DataSource = dt;
            gvGegnerPhones.DataBind();
        }
        
        private DataTable GetDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("EditUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Address", typeof(string)));
            dt.Columns.Add(new DataColumn("Phone", typeof(string)));
            dt.Columns.Add(new DataColumn("Fax", typeof(string)));
            dt.Columns.Add(new DataColumn("EMail", typeof(string)));
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