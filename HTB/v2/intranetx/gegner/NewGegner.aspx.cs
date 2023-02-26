using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBUtilities;
using System.Text;

namespace HTB.v2.intranetx.gegner
{
    public partial class NewGegner : Page
    {
        private const string DdlPhoneTypePrefix = "txtPhoneType_";
        private const string TxtPhoneCountryPrefix = "txtPhoneCountry_";
        private const string TxtPhoneCityPrefix = "txtPhoneCity_";
        private const string TxtPhonePrefix = "txtPhone_";
        private ArrayList phoneTypes = new ArrayList();
        protected void Page_Load(object sender, EventArgs e)
        {
            phoneTypes = HTBUtils.GetSqlRecords("SELECT PhoneTypeCode, PhoneTypeCaption FROM tblPhoneType ORDER BY PhoneTypeSequence", typeof (tblPhoneType));
            DrawPhoneTable();
            if (!IsPostBack)
            {
                LoadGegnerType();
            }
        }

        private void LoadGegnerType()
        {
            ddlGegnerType.Items.Clear();
            ddlGegnerType.Items.Add(new ListItem("*** bitte auswählen ***", "-1"));
            ddlGegnerType.Items.Add(new ListItem("Firma", "0"));
            ddlGegnerType.Items.Add(new ListItem("Herr", "1"));
            ddlGegnerType.Items.Add(new ListItem("Frau", "2"));
        }

        private tblGegner SaveGegner()
        {
            var set = new RecordSet();
            var gegner = new tblGegner
                             {
                                 GegnerType = Convert.ToInt16(ddlGegnerType.SelectedValue),
                                 GegnerAnrede = txtAnrede.Text,
                                 GegnerName1 = txtName1.Text,
                                 GegnerName2 = txtName2.Text,
                                 GegnerName3 = txtName3.Text,
                                 GegnerLastName1 = txtName1.Text,
                                 GegnerLastName2 = txtName2.Text,
                                 GegnerLastName3 = txtName3.Text,
                                 GegnerStrasse = txtStrasse.Text,
                                 GegnerZipPrefix = txtZIPPrefix.Text,
                                 GegnerZip = txtZIP.Text,
                                 GegnerOrt = txtOrt.Text,
//                                 GegnerPhoneCountry = txtPhoneCountry.Text,
//                                 GegnerPhoneCity = txtPhoneCity.Text,
//                                 GegnerPhone = txtPhone.Text,
                                 GegnerFaxCountry = txtFaxCountry.Text,
                                 GegnerFaxCity = txtFaxCity.Text,
                                 GegnerFax = txtFax.Text,
                                 GegnerEmail = txtEmail.Text,
                                 GegnerAnsprechAnrede = txtAnsprechAnrede.Text,
                                 GegnerAnsprech = txtAnsprech.Text,
                                 GegnerAnsprechVorname = txtAnsprechVorname.Text,
                                 GegnerVVZ = chkVVZ.Checked ? 1 : 0,
                                 GegnerVVZDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtVVZDatum.Text),
                                 GegnerGebDat = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtGebDat.Text),
                                 GegnerMemo = txtMemo.Text,
                                 GegnerCreateDate = DateTime.Now,
                                 GegnerCreateSB = GlobalUtilArea.GetUserId(Session),
                                 GegnerImported = 0,
                                 GegnerVVZEnterDate = new DateTime(1900, 1, 1)
                             };


            set.InsertRecord(gegner);
            gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblGegner ORDER BY GegnerID DESC", typeof(tblGegner));
            if (gegner != null)
            {
                gegner.GegnerOldID = "1000" + gegner.GegnerID + ".002";
                set.UpdateRecord(gegner);

                var lastPhoneCountry = "";
                var lastPhoneCity = "";
                var lastPhone = "";

                for (int i = GetNumberOfPlzRanges() - 1; i >=0; --i)
                {
                    var ddlPhoneType = (DropDownList)tblPhone.FindControl(DdlPhoneTypePrefix + i);
                    var txtPCountry = (TextBox) tblPhone.FindControl(TxtPhoneCountryPrefix + i);
                    var txtPCity = (TextBox) tblPhone.FindControl(TxtPhoneCityPrefix + i);
                    var txtP = (TextBox) tblPhone.FindControl(TxtPhonePrefix + i);

                    int phoneType = GlobalUtilArea.GetZeroIfConvertToIntError(ddlPhoneType.SelectedValue);
                    var phoneCountry = GlobalUtilArea.GetEmptyIfNull(txtPCountry.Text);
                    var phoneCity = GlobalUtilArea.GetEmptyIfNull(txtPCity.Text);
                    var phone = GlobalUtilArea.GetEmptyIfNull(txtP.Text);

                    if (phone.Trim().Length > 0)
                    {
                        set.InsertRecord(new tblGegnerPhone
                                             {
                                                 GPhone = phone,
                                                 GPhoneCity = phoneCity,
                                                 GPhoneCountry = phoneCountry,
                                                 GPhoneDate = DateTime.Now,
                                                 GPhoneType = phoneType,
                                                 GPhoneGegnerID = gegner.GegnerID
                                             });

                        lastPhoneCountry = phoneCountry;
                        lastPhoneCity = phoneCity;
                        lastPhone = phone;

                    }

                    if (lastPhone.Trim().Length > 0)
                    {
                        gegner.GegnerPhoneCountry = lastPhoneCountry;
                        gegner.GegnerPhoneCity = lastPhoneCity;
                        gegner.GegnerPhone = lastPhone;
                        set.UpdateRecord(gegner);
                    }
                }
            }
            return gegner;
        }

        private bool IsEntryValid()
        {
            bool ok = true;
            var sb = new StringBuilder();
            int gegnerType = -1;
            try {
                gegnerType = Convert.ToInt16(ddlGegnerType.SelectedValue);
                if (gegnerType < 0)
                {
                    sb.Append("Gegner Typ ist ungültig.<br>");
                    ok = false;
                }
            }
            catch {
                ok = false;
            }
            if (txtName1.Text.Trim() == "")
            {
                sb.Append("Nachname / Firma ist ungültig.<br>");
                ok = false;
            }
            if (txtName2.Text.Trim() == "" && gegnerType > 0)
            {
                sb.Append("Vorname ist ungültig.<br>");
                ok = false;
            }
            if (!ok)
            {
                ctlMessage.ShowError(sb.ToString());
            }
            return ok;
        }

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsEntryValid())
                {
                    tblGegner gegner = SaveGegner();
                    if (Request.QueryString["pop"] != null && Request.QueryString["pop"] == "true")
                    {
                        // populate parent fields and close window
                        var sb = new StringBuilder();
                        GlobalUtilArea.AppentWindowOpenerValue(Request, sb, "IdControl", gegner.GegnerID.ToString());
                        GlobalUtilArea.AppentWindowOpenerValue(Request, sb, "OldIdControl", gegner.GegnerOldID);
                        GlobalUtilArea.AppentWindowOpenerInnerHTML(Request, sb, "TextSection", GetClientTextSection(gegner));
                        sb.Append("window.close();");
                        ScriptManager.RegisterStartupScript(this.updPanel1, typeof (string), "closeScript", sb.ToString(), true);
                    }
                    else if (!GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT]).Equals(string.Empty))
                    {
                        Response.Redirect(
                            GetReturnToUrl(
                                GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT]),
                                GlobalUtilArea.DecodeUrl(GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.EXTRA_PARAMS])),
                                gegner
                                )
                            );
                    }
                    else
                    {
                        Response.Redirect("../../intranet/gegner/gegner.asp" + Session["var"]);
                    }
                }
            }
            catch(Exception ex)
            {
                ctlMessage.ShowException(ex);

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["pop"] != null && Request.QueryString["pop"] == "true")
            {
                // close window
                var sb = new StringBuilder();
                sb.Append("window.close();");
                ScriptManager.RegisterStartupScript(this.updPanel1, typeof(string), "closeScript", sb.ToString(), true);
            }
            else if (!GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL]).Equals(string.Empty))
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
                Response.Redirect("../../intranet/gegner/gegner.asp" + Session["var"]);
            }
        }

        protected void txtZip_TextChanged(object sender, EventArgs e)
        {
            int zip = 9999999;
            try
            {
                zip = Convert.ToInt32(txtZIP.Text);
            }
            catch { zip = 9999999; }
            tblOrte ort = (tblOrte)HTBUtils.GetSqlSingleRecord("SELECT * FROM dbo.tblOrte WHERE " + HTBUtils.GetZipWhere(zip), typeof(tblOrte));
            txtOrt.Text = ort != null ? ort.Ort : "unbekannt";
            txtOrt.Focus();
        }

        protected void lnkAddPhone_Clicked(object sender, EventArgs e)
        {
            int existingRows = GetNumberOfPlzRanges();
            AddRowToPhoneTable(existingRows);
            SetNumberOfPhones(existingRows + 1);
        }

        protected void btnDeltaVista_Clicked(object sender, EventArgs e)
        {
            var sbUrl =
                new StringBuilder(
                    "https://www.deltavista-online.at/oks/app?exactSearchButton=Exakte%20Suche&txtPLZ=&includePrivatePool=on&txtStrasse=&Form0=txtName,birthDate,exactSearchButton,txtStrasse,includePrivatePool,fuzzySearchButton,txtPLZ,txtOrt,includeCompanyPool,zmrSearchButton,txtCompanyRegistrationNumber,txtTel&service=direct/1/Search/$Form&txtOrt=&txtTel=&includeCompanyPool=on&txtCompanyRegistrationNumber=&sp=S0&txtName=");
            sbUrl.Append(HTBUtils.ReplaceUmlautsWithHtmlCodes(txtName1.Text));
            sbUrl.Append("%20");
            sbUrl.Append(HTBUtils.ReplaceUmlautsWithHtmlCodes(txtName2.Text));
            sbUrl.Append("&birthDate=");
            if(HTBUtils.IsDateValid(GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtGebDat)))
            {
                sbUrl.Append(GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtGebDat).ToShortDateString());
            }
            var sb = new StringBuilder("/v2/intranetx/aktenint/ResearchRedirect.aspx?Source=DeltaVista&UserID=");
            sb.Append(GlobalUtilArea.GetUserId(Session));
            sb.Append("&URL=");
            sb.Append(GlobalUtilArea.EncodeURL(HTBUtils.ReplaceUmlautsWithHtmlCodes(sbUrl.ToString())));
//            sb.Append("'");

            ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}', 'popWindow', 'menubar=yes,scrollbars=yes,resizable=yes,width=900,height=800');</script>", sb));
        }
        #endregion

        #region URL Creation
        private string GetReturnToUrl(string returnToUrlCode, string returnExtraParams, tblGegner gegner)
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
                sb.Append(HTBUtils.GetBoolValue(GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.RETURN_GEGNER2])) ? GlobalHtmlParams.GEGNER2_ID : GlobalHtmlParams.GEGNER_ID);
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

        private string GetClientTextSection(tblGegner gegner)
        {
            var sb = new StringBuilder();
            GlobalUtilArea.AddNonEmptyHtmlLine(sb, gegner.GegnerName1);
            GlobalUtilArea.AddNonEmptyHtmlLine(sb, gegner.GegnerName2);
            GlobalUtilArea.AddNonEmptyHtmlLine(sb, gegner.GegnerName3);
            GlobalUtilArea.AddNonEmptyHtmlLine(sb, gegner.GegnerStrasse);
            GlobalUtilArea.AddNonEmptyHtmlLine(sb, gegner.GegnerZipPrefix + " " + gegner.GegnerZip + " " + gegner.GegnerOrt);
            return sb.ToString();
        }

        private void DrawPhoneTable()
        {

            int existingRows = GetNumberOfPlzRanges();

            if (existingRows == 0)
                existingRows = 1;

            for (int i = 0; i < existingRows; ++i)
            {
                AddRowToPhoneTable(i);
            }

            SetNumberOfPhones(existingRows);
        }
        private void AddRowToPhoneTable(int index)
        {
            string phoneType;
            string country;
            string city;
            string phone;

            var row = new HtmlTableRow();
            var cell1 = new HtmlTableCell();

            var ddlPhoneType = new DropDownList();
            var txtPhoneCountry = new TextBox();
            var txtPhoneCity = new TextBox();
            var txtPhone = new TextBox();

            txtPhoneCountry.Text = "43";

            phoneType = GlobalUtilArea.GetEmptyIfNull(Request.Form[DdlPhoneTypePrefix + index]);
            country = GlobalUtilArea.GetEmptyIfNull(Request.Form[TxtPhoneCountryPrefix + index]);
            city = GlobalUtilArea.GetEmptyIfNull(Request.Form[TxtPhoneCityPrefix + index]);
            phone = GlobalUtilArea.GetEmptyIfNull(Request.Form[TxtPhonePrefix + index]);

            
            GlobalUtilArea.LoadDropdownList(ddlPhoneType, phoneTypes, "PhoneTypeCode", "PhoneTypeCaption", false);

            cell1.Controls.Add(ddlPhoneType);
            cell1.Controls.Add(new LiteralControl("&nbsp;&nbsp;+"));
            cell1.Controls.Add(txtPhoneCountry);
            cell1.Controls.Add(new LiteralControl("("));
            cell1.Controls.Add(txtPhoneCity);
            cell1.Controls.Add(new LiteralControl(")"));
            cell1.Controls.Add(txtPhone);

            ddlPhoneType.ID = DdlPhoneTypePrefix + index;
            txtPhoneCountry.CssClass = "docText";
            
            txtPhoneCountry.ID = TxtPhoneCountryPrefix + index;
            txtPhoneCountry.CssClass = "docText";
            txtPhoneCountry.Attributes.Add("onfocus", "this.style.backgroundColor='#DFF4FF'");
            txtPhoneCountry.Attributes.Add("onblur", "this.style.backgroundColor=''");
            txtPhoneCountry.Attributes.Add("size", "5'");
            txtPhoneCountry.MaxLength = 5;
            
            txtPhoneCity.ID = TxtPhoneCityPrefix + index;
            txtPhoneCity.CssClass = "docText";
            txtPhoneCity.Attributes.Add("onfocus", "this.style.backgroundColor='#DFF4FF';");
            txtPhoneCity.Attributes.Add("onblur", "this.style.backgroundColor=''");
            txtPhoneCity.Attributes.Add("size", "10'");
            txtPhoneCity.MaxLength = 10;
            
            txtPhone.ID = TxtPhonePrefix + index;
            txtPhone.CssClass = "docText";
            txtPhone.Attributes.Add("onfocus", "this.style.backgroundColor='#DFF4FF';");
            txtPhone.Attributes.Add("onblur", "this.style.backgroundColor=''");
            txtPhone.Attributes.Add("size", "45'");
            txtPhone.MaxLength = 45;
            
            row.Cells.Add(cell1);
            
            if (index == 0)
            {
                var cell5 = new HtmlTableCell();
                var lnk = new LinkButton
                {
                    ID = "lnkAddPhone" + index,
                    Text = "<strong>+</strong>"
                };
                lnk.Click += lnkAddPhone_Clicked;
                cell5.Controls.Add(lnk);
                row.Cells.Add(cell5);
            }

            tblPhone.Rows.Add(row);
        }

        #region setter / getter
        private void SetNumberOfPhones(int val)
        {
            hdnNumberOfPhones.Value = val.ToString();
        }
        private int GetNumberOfPlzRanges()
        {
            try
            {
                return Convert.ToInt32(hdnNumberOfPhones.Value);
            }
            catch
            {
                return 0;
            }
        }
        #endregion
    }
}