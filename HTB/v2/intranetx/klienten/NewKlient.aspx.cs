using System;
using System.Web;
using System.Web.UI;
using HTB.v2.intranetx.util;
using HTB.Database;
using HTBUtilities;
using System.Text;
using HTB.v2.intranetx.global_files;

namespace HTB.v2.intranetx.klienten
{
    public partial class NewKlient : Page, IWorkflow
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GlobalUtilArea.LoadDropdownList(ddlClientType,
                    "SELECT * FROM tblKlientType ORDER BY KlientTypeCaption ASC",
                    typeof(tblKlientType),
                    "KlientTypeID",
                    "KlientTypeCaption", false);

                GlobalUtilArea.LoadDropdownList(ddlUserState,
                    "SELECT * FROM tblState ORDER BY tblStateID ASC",
                    typeof(tblState),
                    "tblStateID",
                    "tblStateCaption", false);
                GlobalUtilArea.LoadDropdownList(ddlLawyer,
                    "SELECT * FROM tblLawyer ORDER BY LawyerName1 ASC",
                    typeof(tblLawyer),
                    "LawyerID",
                    "LawyerName1", false);
            }
            ctlWorkflow.SetWftInterface(this);
            ctlWorkflow.ShowClientScreen();
            
            ctlWorkflow.setDurationForMahnung1(21);
            ctlWorkflow.setDurationForIntervention(21);
            ctlWorkflow.setDurationForTelefonInkassoAfterIntervention(5);
            ctlWorkflow.setDurationForMahnung2(14);
            ctlWorkflow.setDurationForRechtsanwalt(21);
            ctlWorkflow.setDurationForRechtsanwaltErinerung(0);
        }
        
        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ValidateEntry())
            {
                tblKlient klient = GetKlientRecordFromScreen();
                RecordSet.Insert(klient);
                klient = (tblKlient)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblKlient ORDER BY KlientID DESC", typeof(tblKlient));
                if (klient != null)
                {

                    klient.KlientOldID = "1000" + klient.KlientID + ".002";
                    RecordSet.Update(klient);
                    ctlWorkflow.SaveWorkFlow(klient.KlientID, true);
                    CreateUser(klient);

                }
                if (Request.QueryString[GlobalHtmlParams.IS_POPUP] != null && Request.QueryString[GlobalHtmlParams.IS_POPUP] == "true")
                {
                    // populate parent fields and close window
                    var sb = new StringBuilder();
                    GlobalUtilArea.AppentWindowOpenerValue(Request, sb, "IdKlnControl", klient.KlientID.ToString());
                    GlobalUtilArea.AppentWindowOpenerValue(Request, sb, "OldKLNIdControl", klient.KlientOldID.ToString());
                    GlobalUtilArea.AppentWindowOpenerInnerHTML(Request, sb, "KLNTextSection", GetClientTextSection(klient));
                    sb.Append("window.close();");
                    ScriptManager.RegisterStartupScript(this.updPanel1, typeof(string), "closeScript", sb.ToString(), true);
                }
                else if (!GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT]).Equals(string.Empty))
                {
                    Response.Redirect(
                        GetReturnToUrl(
                            GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT]),
                            GlobalUtilArea.DecodeUrl(GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.EXTRA_PARAMS])),
                            klient
                        )
                    );
                }
                else
                {
                    Response.Redirect("../../intranet/klienten/klienten.asp?" + Session["var"]);
                }
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
            else if (!GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT]).Equals(string.Empty))
            {
                Response.Redirect(
                    GetReturnToUrl(
                        GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT]),
                        GlobalUtilArea.DecodeUrl(GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.EXTRA_PARAMS])),
                        null
                    )
                );
            }
            else
            {
                Response.Redirect("../../intranet/klienten/klienten.asp?" + Session["var"]);
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
        #endregion

        private tblKlient GetKlientRecordFromScreen()
        {
            var klient = new tblKlient();
            try
            {
                klient.KlientType = Convert.ToInt32(ddlClientType.SelectedValue);
            }
            catch { }
            try
            {
                klient.KlientCreator = GlobalUtilArea.GetUserId(Session);
            }
            catch { }
            try
            {
                klient.KlientLawyerId = Convert.ToInt32(ddlLawyer.SelectedValue);
            }
            catch { }
            klient.KlientInkasso = chkInkasso.Checked ? 1 : 0;
            klient.KlientDetektei = chkDetektei.Checked ? 1 : 0;
            klient.KlientAnrede = txtAnrede.Text;
            klient.KlientTitel = txtTitel.Text;
            klient.KlientName1 = txtName1.Text;
            klient.KlientName2 = txtName2.Text;
            klient.KlientName3 = txtName3.Text;
            klient.KlientStrasse = txtStrasse.Text;
            klient.KlientLKZ = txtLKZ.Text.ToUpper();
            klient.KlientPLZ = txtZIP.Text;
            klient.KlientOrt = txtOrt.Text;
            try
            {
                klient.KlientStaat = Convert.ToInt32(ddlUserState.SelectedValue);
            }
            catch { }
            klient.KlientAnsprech = txtAnsprech.Text;
            klient.KlientBLZ1 = txtBLZ1.Text;
            klient.KlientKtoNr1 = txtKTO1.Text;
            klient.KlientBankCaption1 = txtBank1.Text;
            klient.KlientBLZ2 = txtBLZ2.Text;
            klient.KlientKtoNr2 = txtKTO2.Text;
            klient.KlientBankCaption2 = txtBank2.Text;
            klient.KlientBLZ3 = txtBLZ3.Text;
            klient.KlientKtoNr3 = txtKTO3.Text;
            klient.KlientBankCaption3 = txtBank3.Text;
            klient.KlientMemo = txtMemo.Text;
            klient.KlientPhoneCountry = txtPhoneCountry.Text;
            klient.KlientPhoneCity = txtPhoneCity.Text;
            klient.KlientPhone = txtPhone.Text;
            klient.KlientFaxCountry = txtFaxCountry.Text;
            klient.KlientFaxCity = txtFaxCity.Text;
            klient.KlientFax = txtFax.Text;
            
            klient.KlientEMail = txtEMail.Text;

            try
            {
                klient.KlientContacter = Convert.ToInt32(hdnContacterID.Value);
            }
            catch { }
            try
            {
                klient.KlientSalesPromoter = Convert.ToInt32(hdnSalesPromoterID.Value);
            }
            catch { }

            klient.KlientFirmenbuchnummer = txtFirmenbuchnummer.Text;
            klient.KlientVersicherung = txtVersicherung.Text;
            klient.KlientPolizzennummer = txtPolizzennummer.Text;
            klient.KlientReceivesInterest = chkKlientReceivesInterest.Checked;

            klient.KlientTimeStam = DateTime.Now;
            klient.KlientLastChange = DateTime.Now;
            klient.KlientINKISImportDate = new DateTime(1900, 1, 1);
            klient.KlientNachricht = Convert.ToInt32(ddlKlientNachricht.SelectedValue);
            klient.KlientAccountManager = 378; // nadine auer
            klient.KlientAccountManager2 = 378; // sonia holzman

            return klient;
        }
        private string GetClientTextSection(tblKlient klient)
        {
            StringBuilder sb = new StringBuilder();
            GlobalUtilArea.AddNonEmptyHtmlLine(sb, klient.KlientName1);
            GlobalUtilArea.AddNonEmptyHtmlLine(sb, klient.KlientName2);
            GlobalUtilArea.AddNonEmptyHtmlLine(sb, klient.KlientName3);
            GlobalUtilArea.AddNonEmptyHtmlLine(sb, klient.KlientStrasse);
            GlobalUtilArea.AddNonEmptyHtmlLine(sb, klient.KlientLKZ + " " + klient.KlientPLZ + " " + klient.KlientOrt);
            return sb.ToString();
        }

        private string GetReturnToUrl(string returnToUrlCode, string returnExtraParams, tblKlient klient)
        {
            switch (returnToUrlCode)
            {
                case GlobalHtmlParams.URL_NEW_AKT:
                    StringBuilder sb = new StringBuilder();
                    sb.Append("../aktenink/NewAktInk.aspx?");
                    if (klient != null)
                    {
                        sb.Append(GlobalHtmlParams.CLIENT_ID);
                        sb.Append("=");
                        sb.Append(klient.KlientID.ToString());
                        
                    }
                    if (returnExtraParams != "")
                    {
                        if (klient != null)
                            sb.Append("&");
                        sb.Append(HttpUtility.UrlPathEncode(returnExtraParams));
                    }

                    return sb.ToString();
            }
            return "";
        }

        private bool ValidateEntry()
        {
            bool ok = ctlWorkflow.ValidateWorkflow();
            if (Convert.ToInt32(ddlKlientNachricht.SelectedValue) == 2 && !HTBUtils.IsValidEmail(txtEMail.Text))
            {
                ctlMessage.AppendError("Ungültig Emailadresse");
                ctlMessage2.AppendError("Ungültig Emailadresse");
                ok = false;
            }
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                ctlMessage.AppendError("Ungültig Benutzername");
                ctlMessage2.AppendError("Ungültig Benutzername");
                ok = false;
            }
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                ctlMessage.AppendError("Ungültig Passwort");
                ctlMessage2.AppendError("Ungültig Passwort");
                ok = false;
            }
            return ok;
        }

        #region User
        private void CreateUser(tblKlient klient)
        {
            var user = new tblUser
                           {
                               UserKlient = klient.KlientID,
                               UserVorname = klient.KlientName1,
                               UserNachname = klient.KlientName2,
                               UserStrasse = klient.KlientStrasse,
                               UserZIPPrefix = klient.KlientLKZ,
                               UserZIP = klient.KlientPLZ,
                               UserOrt = klient.KlientOrt,
                               UserState = klient.KlientStaat,
                               UserPhoneOfficeCountry = klient.KlientPhoneCountry,
                               UserPhoneOfficeCity = klient.KlientPhoneCity,
                               UserPhoneOffice = klient.KlientPhone,
                               USERFaxOfficeCountry = klient.KlientFaxCountry,
                               USERFaxOfficeCity = klient.KlientFaxCity,
                               USERFaxOffice = klient.KlientFax,
                               UserEMailOffice = klient.KlientEMail,
                               UserSex = 1,
                               UserStatus = 1,
                               UserGebDat = Globals.Globals.EMPTY_DATE,
                               UserLastPWChange = Globals.Globals.EMPTY_DATE,
                               UserLastLogin = Globals.Globals.EMPTY_DATE,
                               UserPic = "nopic.jpg",
                               UserMandant = 8,
                               UserAbteilung = 10,
                               UserRowCount = 10,
                               UserAG = 41,
                               UserLevel = 100,
                               UserEintrittsdatum = DateTime.Now,
                               UserIsSbAdmin = true,
                               UserUsername = txtUserName.Text,
                               UserPasswort = txtPassword.Text

                           };


            // security level
            /*
            string userName = CreateUserName(user);
            if (userName != null)
            {
                user.UserUsername = userName;
                user.UserPasswort = userName;
                RecordSet.Insert(user);
                SendLoginInfoEMail(user);
            }
             */
            RecordSet.Insert(user);
            SendLoginInfoEMail(user);
        }

        private string CreateUserName(tblUser user)
        {
            string userName = user.UserVorname.ToLower().
                Replace(" ", "").
                Replace("!", "").
                Replace("@", "").
                Replace("#", "").
                Replace("$", "").
                Replace("%", "").
                Replace("^", "").
                Replace("&", "").
                Replace("*", "").
                Replace("(", "").
                Replace(")", "").
                Replace("-", "").
                Replace("+", "").
                Replace("=", "").
                Replace("[", "").
                Replace("]", "").
                Replace("{", "").
                Replace("}", "").
                Replace("'", "").
                Replace("\"", "").
                Replace("\\", "").
                Replace("/", "").
                Replace("?", "").
                Replace(">", "").
                Replace("<", "").
                Replace(",", "").
                Replace("`", "").
                Replace("~", "");

            if (userName.Length > 5) 
                userName = userName.Substring(0, 5);
            userName += DateTime.Now.Day;
            userName += DateTime.Now.Month;
            int counter = 1;
            bool userNameCreated = false;
            var existingUser = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserUsername = '" + userName + "'", typeof(tblUser));
            if (existingUser == null)
                userNameCreated = true;

            while (existingUser != null && counter < 10)
            {
                userName = userName + counter;
                existingUser = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserUsername = '" + userName + "'", typeof(tblUser));
                if (existingUser == null)
                    userNameCreated = true;
                counter++;
            }
            if (!userNameCreated)
                return null;
            return userName;
        }

        private void SendLoginInfoEMail(tblUser user)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Hallo ");
            sb.Append(user.UserVorname);
            sb.Append(",<br/><p/>");
            sb.Append("Herzlich Willkommen bei ECP. <br/> <p/>");
            sb.Append("URL:&nbsp;&nbsp;http://htb.ecp.or.at");
            sb.Append("<br/>");
            sb.Append("User Name:&nbsp;&nbsp;");
            sb.Append(user.UserUsername);
            sb.Append("<br/>");
            sb.Append("Password:&nbsp;&nbsp;&nbsp;&nbsp;");
            sb.Append(user.UserPasswort);
            sb.Append("<br/><p/>");
            sb.Append("Mit Freundlichen Gruessen, <br/>Ihr ECP Team");

            new HTBEmail().SendGenericEmail(new [] { HTBUtils.GetConfigValue("Default_EMail_Addr") }, "ECP - HTB Login Information", sb.ToString());
        }
        
        #endregion

        public string GetKlientID()
        {
            return null;
        }

        public string GetSelectedMainStatus()
        {
            return null;
        }

        public string GetSelectedSecondaryStatus()
        {
            return null;
        }
    }
}