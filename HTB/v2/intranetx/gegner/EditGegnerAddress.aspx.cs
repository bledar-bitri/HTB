using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.gegner
{
    public partial class EditGegnerAddress : System.Web.UI.Page
    {
        private int _adrId, _gegnerId;
        private tblGegnerAdressen _gaddress;
        private bool _isNew = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            _isNew = true;
            _adrId = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.ADDRESS_ID]);
            _gegnerId = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.GEGNER_ID]);
            if(_gegnerId <= 0)
            {
                ctlMessage.ShowError("Schuldner ID nicht empfangen!");
                DisableScreen();
            }
            if (_adrId > 0 && !HTBUtils.GetBoolValue(hdnIsAddNewClicked.Value))
            {
                _gaddress = (tblGegnerAdressen)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegnerAdressen WHERE GAID = " + _adrId, typeof(tblGegnerAdressen));
                _isNew = _gaddress == null;
            }
            if (_isNew)
                _gaddress = new tblGegnerAdressen();

            if(!IsPostBack)
            {
                SetValues();
            }
        }

        private void SetValues()
        {
            if (_isNew)
                ddlGAType.SelectedIndex = 0;
            else
                ddlGAType.SelectedValue = _gaddress.GAType.ToString();
            
            txtGAName1.Text = _gaddress.GAName1;
            txtGAName2.Text = _gaddress.GAName2;
            txtGAName3.Text = _gaddress.GAName3;
            txtGAOrt.Text = _gaddress.GAOrt;
            txtGAStrasse.Text = _gaddress.GAStrasse;
            txtGAZip.Text = _gaddress.GAZIP;
            txtGAZipPrefix.Text = _gaddress.GAZipPrefix;
            txtDescription.Text = _gaddress.GADescription;
            
            PopulateGrid();
        }

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            hdnIsAddNewClicked.Value = false.ToString();
            try
            {
                var gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerID = " + _gegnerId, typeof (tblGegner));
                _gaddress.GAGegner = _gegnerId;
                _gaddress.GAType = Convert.ToInt32(ddlGAType.SelectedValue);
                _gaddress.GAName1 = txtGAName1.Text;
                _gaddress.GAName2 = txtGAName2.Text;
                _gaddress.GAName3 = txtGAName3.Text;
                _gaddress.GAOrt = txtGAOrt.Text;
                _gaddress.GAStrasse = txtGAStrasse.Text;
                _gaddress.GAZIP = txtGAZip.Text;
                _gaddress.GAZipPrefix = txtGAZipPrefix.Text;
                _gaddress.GADescription = txtDescription.Text;
                
                _gaddress.GATimeStamp = DateTime.Now;
                _gaddress.GAUserID = GlobalUtilArea.GetUserId(Session);

                bool ok = _isNew ? RecordSet.Insert(_gaddress) : RecordSet.Update(_gaddress);
                if(ok)
                {
                    // TblGegnerAdressen has a triger to update TblGegner fields (we want to keep the existing Gegner info intact)
                    RecordSet.Update(gegner);   // important
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
            Response.Redirect(
                    GetReturnToUrl(
                        GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL]),
                        GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.EXTRA_PARAMS]),
                        null
                    )
                );
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            _isNew = true;
            _gaddress = new tblGegnerAdressen();
            hdnIsAddNewClicked.Value = true.ToString();
            SetValues();
        }

        protected void txtGAZip_TextChanged(object sender, EventArgs e)
        {
            txtGAOrt.Text = HTBUtils.GetCityForZip(txtGAZip.Text);
            txtGAOrt.Focus();
        }
        #endregion

        private void ClearScreen()
        {
            ddlGAType.SelectedIndex = 0;
            txtGAName1.Text = "";
            txtGAName2.Text = "";
            txtGAName3.Text = "";
            txtGAOrt.Text = "";
            txtGAStrasse.Text = "";
            txtGAZip.Text = "";
            txtGAZipPrefix.Text = "";
            txtDescription.Text = "";
        }

        private void DisableScreen()
        {
            ddlGAType.Enabled = false;
            txtGAName1.Enabled = false;
            txtGAName2.Enabled = false;
            txtGAName3.Enabled = false;
            txtGAOrt.Enabled = false;
            txtGAStrasse.Enabled = false;
            txtGAZip.Enabled = false;
            txtGAZipPrefix.Enabled = false;
            txtDescription.Enabled = false;
            btnSubmit.Enabled = false;
            btnAddNew.Enabled = false;
        }

        #region Grid
        private void PopulateGrid()
        {
            ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM tblGegnerAdressen WHERE GAGegner = " + _gegnerId + " ORDER BY GATimeStamp", typeof(tblGegnerAdressen));
            DataTable dt = GetDataTableStructure();
            foreach (tblGegnerAdressen address in list)
            {
                DataRow dr = dt.NewRow();

                var sb = new StringBuilder("<a href=\"EditGegnerAddress.aspx?");
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ADDRESS_ID, address.GAID.ToString(), false);
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb,GlobalHtmlParams.GEGNER_ID, _gegnerId.ToString());
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, Request.QueryString[GlobalHtmlParams.ID]);
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL, GlobalHtmlParams.URL_NEW_AKT_INT_AUTO);
                sb.Append("\">");
                sb.Append("<img src=\"../../intranet/images/edit.gif\" width=\"16\" height=\"16\" alt=\"&Auml;ndern diesen Datensatz.\" style=\"border-color:White;border-width:0px;\"/>");
                sb.Append("</a>");
                string type = "";
                switch (address.GAType)
                {
                    case 1:
                        type = "<img src=\"../../intranet/images/kl48.gif\" width=\"48\" height=\"16\">";
                        break;
                    case 2:
                        type = "<img src=\"../../intranet/images/zmr48.gif\" width=\"48\" height=\"16\">";
                        break;
                    case 3:
                        type = "<img src=\"../../intranet/images/ae48.gif\" width=\"48\" height=\"16\">";
                        break;
                }

                dr["EditUrl"] = sb.ToString();
                dr["Type"] = type;
                dr["Description"] = address.GADescription;
                dr["Name1"] = address.GAName1;
                dr["Name2"] = address.GAName2;
                dr["Date"] = address.GATimeStamp.ToShortDateString();
                dr["Address"] = address.GAStrasse + "<BR/>" + address.GAZipPrefix + "-" + address.GAZIP+" "+address.GAOrt;

                dt.Rows.Add(dr);
            }
            gvGegnerAddresses.DataSource = dt;
            gvGegnerAddresses.DataBind();
        }
        
        private DataTable GetDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("EditUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("Type", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Name1", typeof(string)));
            dt.Columns.Add(new DataColumn("Name2", typeof(string)));
            dt.Columns.Add(new DataColumn("Date", typeof(string)));
            dt.Columns.Add(new DataColumn("Address", typeof(string)));
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