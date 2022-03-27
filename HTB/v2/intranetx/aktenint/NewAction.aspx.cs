using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using HTB.v2.intranetx.permissions;
using HTB.v2.intranetx.util;
using HTB.Database;
using HTBUtilities;
using System.Drawing;
using HTBAktLayer;
using System.Text;
using HTB.Database.Views;
using HTBExtras;
using HTBServices;
using HTBServices.Mail;

namespace HTB.v2.intranetx.aktenint
{
    public partial class NewAction : Page
    {
        private int _aktId;
        tblAktenInt _akt;
        tblUser _user;
        tblAuftraggeber _ag;
        tblAktenIntAction _aktAction;
        bool _isNewAction = true;
        tblAktenIntActionType _actionType;

        private readonly PermissionsNewAction _permissions = new PermissionsNewAction();

        protected void Page_Load(object sender, EventArgs e)
        {
            _permissions.LoadPermissions(GlobalUtilArea.GetUserId(Session));
            ClearErrors();
            txtProvision.Visible = false;
            lblProvision.Visible = true;
            trSaveWithMissingBeleg.Visible = false;
            if (Request["INTID"] != null && !Request["INTID"].Equals(""))
            {
                if (Request["AktionID"] != null && !Request["AktionID"].Equals(""))
                {
                    _aktAction = (tblAktenIntAction)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntAction WHERE AktIntActionID = " + Request["AktionID"], typeof(tblAktenIntAction));
                    if (_aktAction != null)
                    {
                        _isNewAction = false;
                        if(!_permissions.GrantChangeBeleg)
                            txtBeleg.ReadOnly = true;
                    }
                }
                _aktId = Convert.ToInt32(Request["INTID"]);
                _akt = (tblAktenInt)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenInt WHERE AktIntID = " + _aktId, typeof(tblAktenInt));
                _user = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT UserID, UserRoleFB FROM tblUser WHERE UserID = " + _akt.AktIntSB, typeof(tblUser));
                _ag = (tblAuftraggeber)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeber WHERE AuftraggeberID = " + _akt.AktIntAuftraggeber, typeof(tblAuftraggeber));
                if (!IsPostBack)
                {
                    SetValues();
                }
            }
            lblPrice.Text = "&nbsp;";
            if (Session["MM_UserLevelLevel"] != null && Session["MM_UserLevelLevel"].ToString() == "255")
            {
                txtProvision.Visible = true;
                lblProvision.Visible = false;
            }
            if (lblProvision.Text.Trim() == string.Empty)
                lblProvision.Text = "&nbsp;";
            if (ddlAktion.SelectedValue == GlobalUtilArea.DefaultDropdownValue)
                trBetrag.Visible = false;
        }

        private void SetValues()
        {
            // check to see if there are actions attached to this auftraggeber
            //double balance = GetBalance();
            GlobalUtilArea.LoadDropdownList(ddlAktion, GetActions(),
                    "ActionID",
                    "ActionCaption", true);

            
            SetDateControlVisible();
            if (!_isNewAction)
            {
                ddlAktion.SelectedValue = _aktAction.AktIntActionType.ToString();
                PopulateActionFields();
                txtBetrag.Text = HTBUtils.FormatCurrencyNumber(_aktAction.AktIntActionBetrag);
                txtBeleg.Text = _aktAction.AktIntActionBeleg;
                txtAuftraggeberSB.Text = _aktAction.AktIntActionAuftraggeberSB;
                if (_aktAction.AktIntActionProvision > 0)
                    txtProvision.Text = HTBUtils.FormatCurrencyNumber(_aktAction.AktIntActionProvision);
                else if (_aktAction.AktIntActionHonorar > 0)
                    txtProvision.Text = HTBUtils.FormatCurrencyNumber(_aktAction.AktIntActionHonorar);

                lblProvision.Text = txtProvision.Text;
                txtMemo.Text = _aktAction.AktIntActionMemo;
                txtDate.Text = _aktAction.AktIntActionDate.ToShortDateString();
                txtTime.Text = _aktAction.AktIntActionTime.ToShortTimeString();
                ddlDate.SelectedValue = _aktAction.AktIntActionDate.ToShortDateString();
            }
            else
            {
                txtDate.Text = DateTime.Now.ToShortDateString();
                txtTime.Text = "00:00";
            }
        }

        private void SetDateControlVisible()
        {

            if (Session["MM_UserLevelLevel"] != null && Session["MM_UserLevelLevel"].ToString().Trim() == "255")
            {
                txtDate.Visible = true;
                Date_CalendarButton.Visible = true;
                ddlDate.Visible = false;
            }
            else
            {
                trDate.Visible = true;
                txtDate.Visible = false;
                Date_CalendarButton.Visible = false;
                ddlDate.Visible = true;

                ddlDate.Items.Clear();

                ArrayList dates = new ArrayList();
                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        trDate.Visible = false;
                        txtDate.Visible = false;
                        Date_CalendarButton.Visible = false;
                        ddlDate.Visible = false;
                        break;
                    case DayOfWeek.Tuesday:
                        dates.Add(DateTime.Now);
                        break;
                    case DayOfWeek.Wednesday:
                        dates.Add(DateTime.Now);
                        dates.Add(DateTime.Now.AddDays(-1));
                        break;
                    case DayOfWeek.Thursday:
                        dates.Add(DateTime.Now);
                        dates.Add(DateTime.Now.AddDays(-1));
                        dates.Add(DateTime.Now.AddDays(-2));
                        break;
                    case DayOfWeek.Friday:
                        dates.Add(DateTime.Now);
                        dates.Add(DateTime.Now.AddDays(-1));
                        dates.Add(DateTime.Now.AddDays(-2));
                        dates.Add(DateTime.Now.AddDays(-3));
                        break;
                    case DayOfWeek.Saturday:
                        dates.Add(DateTime.Now);
                        dates.Add(DateTime.Now.AddDays(-1));
                        dates.Add(DateTime.Now.AddDays(-2));
                        dates.Add(DateTime.Now.AddDays(-3));
                        dates.Add(DateTime.Now.AddDays(-4));
                        break;
                    case DayOfWeek.Sunday:
                        dates.Add(DateTime.Now);
                        dates.Add(DateTime.Now.AddDays(-1));
                        dates.Add(DateTime.Now.AddDays(-2));
                        dates.Add(DateTime.Now.AddDays(-3));
                        dates.Add(DateTime.Now.AddDays(-4));
                        dates.Add(DateTime.Now.AddDays(-5));
                        break;
                }
                foreach (DateTime dte in dates)
                {
                    ddlDate.Items.Add(new ListItem(dte.ToShortDateString(), dte.ToShortDateString()));
                }
            }
        }

        private void PopulateActionFields()
        {
            try
            {
                _actionType = GetSelectedActionType();
                if (_actionType != null)
                {
                    if (_actionType.AktIntActionTypeIsInstallment || _actionType.AktIntActionIsExtensionRequest || _actionType.AktIntActionIsTelAndEmailCollection)
                    {
                        trExtra.Visible = true;
                        trExtraBlank.Visible = true;
                        if (_actionType.AktIntActionIsExtensionRequest)
                        {
                            ctlExtension.PopulateFields(_akt.AktIntID);
                            ctlExtension.Visible = true;
                        }
                        else if (_actionType.AktIntActionIsTelAndEmailCollection)
                        {
                            ctlTelAndEmail.PopulateFields(_akt.AktIntGegner);
                            ctlTelAndEmail.Visible = true;
                        }
                        else // Installment
                        {
                            if (_akt.IsInkasso())
                            {
                                if(_actionType.AktIntActionIsPersonalCollection)
                                    ctlInstallment.ShowPersonalCollection();
                                else 
                                    ctlInstallment.ShowErlagschein();

                                ctlInstallment.Visible = true;
                            }
                            else
                            {
                                ctlInstallmentOld.Visible = true;
                            }
                            bdy.Attributes.Add("onload", "window.resizeTo(800,600);");
                        }
                    }
                    if (!_actionType.AktIntActionIsWithCollection && !_actionType.AktIntActionIsDirectPayment)
                    {
                        txtBetrag.Text = "";
                        txtBeleg.Text = "";
                        trBetrag.Visible = false;
                    }
                    if (_actionType.AktIntActionIsTotalCollection)
                    {
                        txtBetrag.Text = HTBUtils.FormatCurrencyNumber(GetBalance());
                    }
                    if(_actionType.AktIntActionIsDirectPayment)
                    {
                        trAuftraggeberSB.Visible = true;
                    }
                    double balance = GetBalance();
                    GetPrice(GlobalUtilArea.GetZeroIfConvertToDoubleError(txtBetrag.Text), balance, _akt.AktIntAuftraggeber, _akt.AktIntAktType, _akt.AktIntSB, GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktion.SelectedValue));
                    txtProvision.Text = HTBUtils.FormatCurrencyNumber(GetProvision(GlobalUtilArea.GetZeroIfConvertToDoubleError(txtBetrag.Text), balance, _akt.AktIntAuftraggeber, _akt.AktIntAktType, _akt.AktIntSB, GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktion.SelectedValue)));
                    lblProvision.Text = txtProvision.Text;
                }
            }
            catch (Exception ex)
            {
                ctlMessage.ShowError(ex.Message + "<BR/>" + ex.StackTrace.Replace(" ", "<BR/>"));
            }
        }

        private ArrayList GetActions()
        {
            /* Try AG + Type First */
            string sqlQuery = "SELECT * FROM qryAuftraggeberAktTypeAction " +
                                " WHERE AGAktionTypeAuftraggeberID = " + _akt.AktIntAuftraggeber +
                                " AND AGAktionTypeAktTypeIntID = " + _akt.AktIntAktType +
                                " ORDER BY AktIntActionTypeCaption";
            ArrayList list = GetActions(sqlQuery, typeof(qryAuftraggeberAktTypeAction));
            if (list.Count > 0)
            {
                return list;
            }

            /* Than Try Just Type */
            sqlQuery = "SELECT * FROM qryAktTypeAction " +
                        " WHERE AktTypeActionAktTypeIntID = " + _akt.AktIntAktType +
                        " ORDER BY AktIntActionTypeCaption";
            list = GetActions(sqlQuery, typeof(qryAktTypeAction));
            if (list.Count > 0)
            {
                return list;
            }

            /* Than Try Just AG */
            sqlQuery = "SELECT * FROM qryAuftraggeberAction " +
                        " WHERE AGAktionAuftraggeberID = " + _akt.AktIntAuftraggeber +
                        " ORDER BY AktIntActionTypeCaption";
            list = GetActions(sqlQuery, typeof(qryAuftraggeberAction));
            if (list.Count > 0)
            {
                return list;
            }

            /*If All fails show default actions */
            sqlQuery = "SELECT * FROM tblAktenIntActionType WHERE AktIntActionIsDefault = 1 ORDER BY AktIntActionTypeCaption";
            return GetActions(sqlQuery, typeof(tblAktenIntActionType));
        }
        private ArrayList GetActions(string slqQuery, Type classType)
        {
            var resultsList = HTBUtils.GetSqlRecords(slqQuery, classType);
            var list = new ArrayList();

            if (resultsList.Count > 0)
            {
                foreach (Record action in resultsList)
                {
                    list.Add(new ActionRecord(action));
                }
                return GetUserActions(list);
            }
            return list;
        }

        private ArrayList GetUserActions(ArrayList actionsList)
        {
//            ArrayList userActionsList = HTBUtils.GetSqlRecords("SELECT * FROM qryUserAktionen WHERE UserAktionUserID = " + GlobalUtilArea.GetUserId(Session), typeof(qryUserAktionen));
            ArrayList userActionsList = HTBUtils.GetSqlRecords("SELECT * FROM qryUserAktionen WHERE UserAktionUserID = " + _akt.AktIntSB, typeof(qryUserAktionen));
            if (userActionsList.Count > 0)
            {
                var resultsList = new ArrayList();
                foreach (ActionRecord agAction in actionsList)
                {
                    foreach (qryUserAktionen userAction in userActionsList)
                    {
                        if (agAction.ActionID == userAction.UserAktionAktIntActionTypeID)
                        {
                            resultsList.Add(agAction);
                        }
                    }
                }
                return resultsList;
            }
            return actionsList;
        }
        #region Event Handlers
        protected void ddlAktion_SelectedIndexChanged(object sender, EventArgs e)
        {
            trExtra.Visible = false;
            trAuftraggeberSB.Visible = false;
            ctlInstallment.Visible = false;
            ctlInstallmentOld.Visible = false;
            ctlExtension.Visible = false;
            ctlTelAndEmail.Visible = false;
            trExtraBlank.Visible = false;
            trBetrag.Visible = true;
            PopulateActionFields();
        }
        protected void txtBetrag_TextChanged(object sender, EventArgs e)
        {
            _actionType = GetSelectedActionType();
            double balance = GetBalance();
            double paid = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtBetrag.Text);
            txtProvision.Text = HTBUtils.FormatCurrencyNumber(GetProvision(paid, balance, _akt.AktIntAuftraggeber, _akt.AktIntAktType, _akt.AktIntSB, GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktion.SelectedValue)));
            lblProvision.Text = txtProvision.Text;
            lblPrice.Text = HTBUtils.FormatCurrencyNumber(GetPrice(paid, balance, _akt.AktIntAuftraggeber, _akt.AktIntAktType, _akt.AktIntSB, GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktion.SelectedValue)));

            if (_actionType != null && _actionType.AktIntActionTypeIsInstallment)
            {
                ctlInstallment.SetCurrentPayment(paid);
                ctlInstallment.LoadAll();
                ctlInstallment.RefreshScreen();
            }
            
            txtBeleg.Focus();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SaveAction();
        }
        protected void btnSaveWithMissingBeleg_Click(object sender, EventArgs e)
        {
            SaveAction(true);
        }
        #endregion

        private void SaveAction(bool saveBelegError = false)
        {
            try
            {
                LoadActionFromScreen();
                bool ok = IsActionValid(_aktAction, saveBelegError);
                if (ok)
                {
                    if (ctlInstallment.Visible)
                    {
                        ok &= ctlInstallment.SaveInstallment();
                    }
                    else if (ctlInstallmentOld.Visible)
                    {
                        ok &= ctlInstallmentOld.SaveInstallment();
                    }
                    if (ctlTelAndEmail.Visible)
                    {
                        ok &= ctlTelAndEmail.Save(_akt.AktIntGegner);
                    }
                }
                if (ok)
                {
                    SetLastBelegUsed(_aktAction);
                    if (_isNewAction)
                        RecordSet.Insert(_aktAction);
                    else
                        RecordSet.Update(_aktAction);
                    ScriptManager.RegisterStartupScript(updPanel1, typeof(string), "closeScript", "MM_refreshParentAndClose();", true);
                }

            }
            catch (Exception e)
            {
                ctlMessage.ShowError(e.Message);
            }
        }

        private void LoadActionFromScreen()
        {
            if (_isNewAction)
            {
                _aktAction = new tblAktenIntAction();
            }
            _actionType = GetSelectedActionType();
            _aktAction.AktIntActionAkt = _akt.AktIntID;
            _aktAction.AktIntActionSB = _akt.AktIntSB;

            _aktAction.AktIntActionType = GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktion.SelectedValue);
            _aktAction.AktIntActionProvision = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtProvision.Text);
            _aktAction.AktIntActionHonorar = 0; // no more honorar (everything gets calculated into provision)
            _aktAction.AktIntActionPrice = GetPrice(GlobalUtilArea.GetZeroIfConvertToDoubleError(txtBetrag.Text), GetBalance(), _akt.AktIntAuftraggeber, _akt.AktIntAktType, _akt.AktIntSB, GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktion.SelectedValue));
            _aktAction.AktIntActionProvAbzug = _ag.AuftraggeberIntAktPovAbzug;

            _aktAction.AktIntActionDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(ddlDate.Visible ? ddlDate.SelectedValue : txtDate.Text);
            _aktAction.AktIntActionTime = GlobalUtilArea.GetDefaultDateIfConvertToDateError(_aktAction.AktIntActionDate.ToShortDateString() + " " + txtTime.Text);
            if (trBetrag.Visible)
            {
                _aktAction.AktIntActionBeleg = HTBUtils.RemoveAllSpecialChars(txtBeleg.Text, true);
                _aktAction.AktIntActionBetrag = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtBetrag.Text);
            }
            if(_actionType.AktIntActionIsDirectPayment)
            {
                _aktAction.AktIntActionAuftraggeberSB = txtAuftraggeberSB.Text.Trim();
            }
            _aktAction.AktIntActionMemo = txtMemo.Text; //TODO: insert memo field

            if (_actionType != null && _actionType.AktIntActionIsExtensionRequest)
            {
                _aktAction.AktIntActionAktIntExtID = ctlExtension.SaveExtensionRequest(_aktAction);
            }
        }

        private tblAktenIntActionType GetSelectedActionType()
        {
            return (tblAktenIntActionType)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntActionType WHERE AktIntActionTypeID = " + ddlAktion.SelectedValue, typeof(tblAktenIntActionType));
        }
        private bool IsActionValid(tblAktenIntAction action, bool saveBelegError = false)
        {
            var sb = new StringBuilder();
            bool ok = true;
            if (action.AktIntActionDate.ToShortDateString() == "01.01.1900")
            {
                sb.Append("<i><strong>Datum</strong></i> ist falsch<br>");
                txtDate.BackColor = Color.Beige;
                ok = false;
            }
            if (action.AktIntActionType <= 0)
            {
                sb.Append("<i><strong>Aktion</strong></i> ausw&auml;hlen<br>");
                ddlAktion.BackColor = Color.Beige;
                ok = false;
            }
            if (txtBetrag.Visible)
            {
                if (HTBUtils.IsZero(action.AktIntActionBetrag))
                {
                    sb.Append("<i><strong>Betrag</strong></i> eingeben<br>");
                    txtBetrag.BackColor = Color.Beige;
                    ok = false;
                }
                if (!_actionType.AktIntActionIsDirectPayment)
                {
                    if (action.AktIntActionBeleg.Trim() == string.Empty)
                    {
                        sb.Append("<i><strong>Beleg</strong></i> eingeben<br>");
                        txtBeleg.BackColor = Color.Beige;
                        ok = false;
                    }
                    else // make sure Beleg is a number
                    {
                        if (_isNewAction)
                        {
                            double num;
                            bool isNum = Double.TryParse(action.AktIntActionBeleg, out num);
                            if (!isNum)
                            {
                                /*
                                string beleg = HTBUtils.RemoveAllSpecialChars(action.AktIntActionBeleg).Replace(" ", "").Trim().ToLower();
                                if (beleg != "zhlganikb" && beleg != "zahlunganikb")
                                {
                                    sb.Append("<i><strong>Beleg</strong></i> ist falsch [Beleg darf entwieder ein Beleg Nummer oder 'Zahlung an IKB' sein]<br>");
                                    txtBeleg.BackColor = Color.Beige;
                                    ok = false;
                                }
                                 */
                                sb.Append("<i><strong>Beleg</strong></i> ist falsch [Beleg darf nur ein Zahl sein]<br>");
                                ok = false;
                            }
                            else
                            {
                                if (saveBelegError)
                                    SaveBelegError((long) num, _akt.AktIntAuftraggeber, _akt.AktIntSB, sb);
                                else
                                    ok = ValidateBelegNumber((long) num, _akt.AktIntAuftraggeber, _akt.AktIntSB, sb);
                            }
                        }
                    }
                }
            }
            if (_actionType.AktIntActionIsDirectPayment && string.IsNullOrEmpty(txtAuftraggeberSB.Text))
            {
                sb.Append("<i><strong>Sachbearbeiter (AG)</strong></i> eingeben<br>");
                ok = false;
            }
            if (!ok)
                ctlMessage.ShowError(sb.ToString());
            return ok;
        }

        private bool ValidateBelegNumber(long belegNumber, int agId, int userId, StringBuilder sb)
        {
            bool ok = true;
            
            var block = GetKassaBlock(belegNumber, agId, userId);
            if (block == null)
            {
                ok = false;
                sb.Append("<i><strong>Belegsnummer</strong></i> ist nicht in die jetztige Blöcke gefunden<br>");
            }
            else
            {
                if (HTBUtils.IsZero(block.KassaBlockLastUsedNr))
                    block.KassaBlockLastUsedNr = block.KassaBlockNrVon - 1;

                if (!HTBUtils.IsZero(belegNumber - 1 - block.KassaBlockLastUsedNr)) // blok.KassaBlockLastUsedNr != belegNumber - 1
                {
                    ok = false;
                    if (block.KassaBlockLastUsedNr > (belegNumber - 1))
                    {
                        sb.Append("<i><strong>Belegsnummer</strong></i> wurde berreits verwendet<br>");
                    }
                    else
                    {
                        sb.Append("<i><strong>Folgende Belegsnummer(n) fehlen</strong></i>:<br>");
                        int count = 0;
                        for (var i = block.KassaBlockLastUsedNr + 1; i < belegNumber; i++)
                        {
                            if (count++ < 10)
                            {
                                sb.Append(i.ToString());
                                sb.Append("<br/>");
                            }
                        }
                        sb.Append("<br/>* Wenn Sie sicher sind, dass die Belegsnummer richtig ist, klicken sie den 'Trotzdem Speichern' Button.<br/>&nbsp;&nbsp;&nbsp;&nbsp;Die fehlenden Belege senden Sie bitte per Email an unser Office.<br/><br/>");
                        sb.Append("<b>* Wenn die fehlenden Belege innerhalb 7 Tagen nicht vorliegen, wird Ihr Account gesperrt!</b><br/>&nbsp;");
                    }
                }
                if (ok)
                {
                    block.KassaBlockLastUsedNr = belegNumber;
                    RecordSet.Update(block);
                }
            }
            if(!ok)
            {
                trSaveWithMissingBeleg.Visible = true;
            }
            return ok;
        }

        private void SaveBelegError(long belegNumber, int agId, int userId, StringBuilder sb)
        {
            var err = new tblKassaBlockError();
            var block = GetKassaBlock(belegNumber, agId, userId);
            var user = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserID = " + userId, typeof(tblUser));

            err.KassaBlockErrDate = DateTime.Now;
            err.KassaBlockErrUser = userId;
            err.KassaBlockErrBeleg = (double) belegNumber;
            err.KassaBlockErrAG = agId;

            bool ok = true;
            if(user != null)
            {
                sb.Append(user.UserVorname + " " + user.UserNachname + ": ");
            }
            if (block == null)
            {
                sb.Append("Belegsnummer [" + belegNumber + "] ist nicht in die jetztige Blöcke gefunden");
                ok = false;
            }
            else
            {
                if (HTBUtils.IsZero(block.KassaBlockLastUsedNr))
                    block.KassaBlockLastUsedNr = block.KassaBlockNrVon - 1;

                if (!HTBUtils.IsZero(belegNumber - 1 - block.KassaBlockLastUsedNr)) // blok.KassaBlockLastUsedNr != belegNumber - 1
                {
                    if (block.KassaBlockLastUsedNr > belegNumber)
                    {
                        sb.Append("Belegsnummer wurde berreits verwendet. Letzte belegnumber: " + block.KassaBlockLastUsedNr);
                        ok = false;
                    }
                    else
                    {
                        //Template missing Beleg record
                        var missTemplate = new tblKassaBlockMissingNr
                                               {
                                                   KbMissBlockID = block.KassaBlockID,
                                                   KbMissUser = userId,
                                                   KbMissDate = DateTime.Now,
                                                   KbMissReceivedDate = HTBUtils.DefaultDate
                                               };

                        sb.Append("Folgende Belegsnummer(n) fehlen: [");
                        int count = 0;
                        for (var i = block.KassaBlockLastUsedNr + 1; i < belegNumber; i++)
                        {
                            if (count++ < 10)
                            {
                                sb.Append(i.ToString());
                                sb.Append(" ");
                                ok = false;
                            }
                            //Create missing Beleg record
                            try
                            {
                                RecordSet.Insert(new tblKassaBlockMissingNr(missTemplate) {KbMissNr = (int) i});
                            }
                            catch
                            {
                            }
                        }
                        sb.Append("]");
                    }
                }
            }
            sb.Append(" ");
            sb.Append(_user.UserVorname);
            sb.Append(" ");
            sb.Append(_user.UserNachname);
            err.KassaBlockErrMessage = sb.ToString();
            RecordSet.Insert(err);
            if(!ok)
                ServiceFactory.Instance.GetService<IHTBEmail>().SendGenericEmail(new string[] { HTBUtils.GetConfigValue("Default_EMail_Addr"), HTBUtils.GetConfigValue("Office_Email"), "b.bitri@ecp.or.at" },
                                            _user.UserVorname + " " + _user.UserNachname + ":  Belegsnummer Fehler ",
                                            sb.ToString());
        
    }

        private void SetLastBelegUsed(tblAktenIntAction action)
        {
            double num;
            if (txtBetrag.Visible && action.AktIntActionBeleg != null)
            {
                bool isNum = Double.TryParse(HTBUtils.RemoveAllSpecialChars(action.AktIntActionBeleg, true), out num);
                if (isNum)
                {
                    var belegNumber = (long) num;
                    var block = GetKassaBlock(belegNumber, _akt.AktIntAuftraggeber, _akt.AktIntSB);
                    if (block != null)
                    {
                        block.KassaBlockLastUsedNr = belegNumber;
                        RecordSet.Update(block);
                    }
                }
            }
        }

        private tblKassablock GetKassaBlock(long belegNumber, int agId, int userId)
        {
            var sql = new StringBuilder("SELECT * FROM tblKassablock WHERE (KassaBlockDatumErhalten IS NULL OR KassaBlockDatumErhalten = '01.01.1900') AND KassaBlockNrVon <= ");
            sql.Append(belegNumber);
            sql.Append(" AND KassaBlockNrBis >= ");
            sql.Append(belegNumber);
            sql.Append(" AND KassaBlockUser = ");
            sql.Append(userId);
            sql.Append(" AND KassaBlockAuftraggeber = ");
            sql.Append(agId);
            return (tblKassablock)HTBUtils.GetSqlSingleRecord(sql.ToString(), typeof(tblKassablock));
        }

        private bool IsActionVoid()
        {
            return GetSelectedActionType().AktIntActionIsVoid;
        }

        #region Messages
        private void ClearErrors()
        {
            ctlMessage.Clear();
            txtDate.BackColor = Color.White;
            ddlAktion.BackColor = Color.White;
            txtBetrag.BackColor = Color.White;
            txtBeleg.BackColor = Color.White;
        }
        #endregion

        #region Calculations
        private double GetProvision(double collectedAmount, double balance, int agId, int aktIntTypeId, int userId, int actionTypeId)
        {
            string sqlWhere = " WHERE AGAktTypeAktionUserProvAuftraggeberID = " + agId +
                       " AND AGAktTypeActionUserProvAktTypeIntID = " + aktIntTypeId +
                       " AND AGAktTypeActionUserProvUserID = " + userId +
                       " AND AGAktTypeActionUserProvAktAktionTypeID = " + actionTypeId;
            string provType = "AG_TYPE_ACTION_USER";
            Record prov = (tblAuftraggeberAktTypeActionUserProv)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeberAktTypeActionUserProv" + sqlWhere, typeof(tblAuftraggeberAktTypeActionUserProv));
            if (prov == null)
            {
                sqlWhere = " WHERE UserAktTypeAktionProvUserID = " + userId +
                           " AND UserAktTypeActionProvAktTypeIntID = " + aktIntTypeId +
                           " AND UserAktTypeActionProvAktAktionTypeID = " + actionTypeId;
                prov = (tblUserAktTypeActionProv)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUserAktTypeActionProv " + sqlWhere, typeof(tblUserAktTypeActionProv));
                provType = "USER_TYPE_ACTION";

            }
            if (prov == null)
            {
                sqlWhere = " WHERE AGAktTypeAktionProvAuftraggeberID = " + agId +
                           " AND AGAktTypeActionProvAktTypeIntID = " + aktIntTypeId +
                           " AND AGAktTypeActionProvAktAktionTypeID = " + actionTypeId;
                prov = (tblAuftraggeberAktTypeActionProv)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeberAktTypeActionProv " + sqlWhere, typeof(tblAuftraggeberAktTypeActionProv));
                provType = "AG_TYPE_ACTION";
            }
            if (prov == null)
            {
                sqlWhere = " WHERE AGUserProvAuftraggeberID = " + agId +
                                 " AND AGUserProvUserID = " + userId +
                                 " AND AGUserProvAktAktionTypeID = " + actionTypeId;
                prov = (tblAuftraggeberUserProv)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeberUserProv " + sqlWhere, typeof(tblAuftraggeberUserProv));
                provType = "AG_USER";
            }
            if (prov == null)
            {
                sqlWhere = " WHERE UserAktionProvUserID = " + userId +
                           " AND UserActionProvAktAktionTypeID = " + actionTypeId;
                prov = (tblUserActionProv)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUserActionProv " + sqlWhere, typeof(tblUserActionProv));
                provType = "USER_ACTION";
            }
            if (prov == null)
            {
                sqlWhere = " WHERE AGAktionProvAuftraggeberID = " + agId +
                           " AND AGActionProvAktAktionTypeID = " + actionTypeId;
                prov = (tblAuftraggeberActionProv)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeberActionProv " + sqlWhere, typeof(tblAuftraggeberActionProv));
                provType = "AG_ACTION";
            }
            if (prov == null)
            {
                sqlWhere = " WHERE AGAktTypeProvAuftraggeberID = " + agId +
                           " AND AGAktTypeProvAktTypeIntID = " + aktIntTypeId;
                prov = (tblAuftraggeberAktTypeProv)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeberAktTypeProv " + sqlWhere, typeof(tblAuftraggeberAktTypeProv));
                provType = "AG_TYPE";
            }
            if (prov == null)
            {
                sqlWhere = " WHERE AktIntActionTypeID = " + actionTypeId;
                prov = GetSelectedActionType();
                provType = "NONE";
            }
            //ShowInfoMessage("Provision TYPE: "+provType);
            return GetProvision(collectedAmount, balance, prov);
        }
        private double GetProvision(double collectedAmount, double balance, Record provRecord)
        {
            if (provRecord is tblAuftraggeberAktTypeActionUserProv)
            {
                var rec = (tblAuftraggeberAktTypeActionUserProv)provRecord;
                return GetProvision(collectedAmount, balance, rec.AGAktTypeActionUserProvHonGrpID, rec.AGAktTypeActionUserProvAmount, rec.AGAktTypeActionUserProvAmountForZeroCollection, rec.AGAktTypeActionUserProvPct);
            }
            if (provRecord is tblUserAktTypeActionProv)
            {
                var rec = (tblUserAktTypeActionProv)provRecord;
                return GetProvision(collectedAmount, balance, rec.UserAktTypeActionProvHonGrpID, rec.UserAktTypeActionProvAmount, rec.UserAktTypeActionProvAmountForZeroCollection, rec.UserAktTypeActionProvPct);
            }
            if (provRecord is tblAuftraggeberUserProv)
            {
                var rec = (tblAuftraggeberUserProv)provRecord;
                return GetProvision(collectedAmount, balance, rec.AGUserProvHonGrpID, rec.AGUserProvAmount, rec.AGUserProvAmountForZeroCollection, rec.AGUserProvPct);
            }
            if (provRecord is tblAuftraggeberAktTypeActionProv)
            {
                var rec = (tblAuftraggeberAktTypeActionProv)provRecord;
                return GetProvision(collectedAmount, balance, rec.AGAktTypeActionProvHonGrpID, rec.AGAktTypeActionProvAmount, rec.AGAktTypeActionProvAmountForZeroCollection, rec.AGAktTypeActionProvPct);
            }
            if (provRecord is tblUserActionProv)
            {
                var rec = (tblUserActionProv)provRecord;
                return GetProvision(collectedAmount, balance, rec.UserActionProvHonGrpID, rec.UserActionProvAmount, rec.UserActionProvAmountForZeroCollection, rec.UserActionProvPct);
            }
            if (provRecord is tblAuftraggeberActionProv)
            {
                var rec = (tblAuftraggeberActionProv)provRecord;
                return GetProvision(collectedAmount, balance, rec.AGActionProvHonGrpID, rec.AGActionProvAmount, rec.AGActionProvAmountForZeroCollection, rec.AGActionProvPct);
            }
            if (provRecord is tblAuftraggeberAktTypeProv)
            {
                var rec = (tblAuftraggeberAktTypeProv)provRecord;
                return GetProvision(collectedAmount, balance, rec.AGAktTypeProvProvisionHonGrpID, rec.AGAktTypeProvProvisionAmount, rec.AGAktTypeProvProvisionAmountForZeroCollection, rec.AGAktTypeProvProvisionPct);
            }
            if (provRecord is tblAktenIntActionType)
            {
                var rec = (tblAktenIntActionType)provRecord;
                return GetProvision(collectedAmount, balance, rec.AktIntActionProvHonGrpID, rec.AktIntActionProvAmount, rec.AktIntActionProvAmountForZeroCollection, rec.AktIntActionProvPct);
            }
            return 0;
        }

        private double GetProvision(double collectedAmount, double balance, int honorarGrpId, double amt, double amtForZero, double amtPct)
        {
            double ret = 0;
            if (honorarGrpId > 0)
            {
                return GetHonorar(honorarGrpId, collectedAmount, balance);
            }
            if (collectedAmount == 0)
            {
                ret = amtForZero;
            }
            else
            {
                if (amtPct != 0)
                {
                    ret = collectedAmount * (amtPct / 100);
                }
                else
                {
                    ret = amt;
                }
            }
            return ret;
        }
        private double GetHonorar(int honorarGrpId, double collectedAmount, double balance)
        {
            double ret = 0;
            if (balance > 0 && trBetrag.Visible)
            {
                string sqlQuery = "SELECT * FROM qryAktenIntGroupHonorar WHERE AktIntHonGrpID = " + honorarGrpId + " AND AktIntHonFrom <= " + collectedAmount + " AND AktIntHonTo >= " + collectedAmount;
                qryAktenIntGroupHonorar honorar = (qryAktenIntGroupHonorar)HTBUtils.GetSqlSingleRecord(sqlQuery.Replace(",", "."), typeof(qryAktenIntGroupHonorar));
                if (honorar != null)
                {
                    ret = honorar.AktIntHonProvAmount;
                    if (honorar.AktIntHonProvPct > 0)
                    {
                        if (honorar.AktIntHonProvPctOf == 0)
                        {
                            ret += (honorar.AktIntHonProvPct / 100) * collectedAmount;
                        }
                        else
                        {
                            ret += (honorar.AktIntHonProvPct / 100) * balance;
                        }
                    }
                    if (ret > honorar.AktIntHonMaxProvAmount)
                    {
                        ret = honorar.AktIntHonMaxProvAmount;
                    }
                }
            }
            return ret;
        }

        private double GetPrice(double collectedAmount, double balance, int agId, int aktIntTypeId, int userId, int actionTypeId)
        {
            double price = 0;
            if (IsActionVoid())
                return 0;

            string sqlWhere = " WHERE AGAktTypeAktionUserProvAuftraggeberID = " + agId +
                       " AND AGAktTypeActionUserProvAktTypeIntID = " + aktIntTypeId +
                       " AND AGAktTypeActionUserProvUserID = " + userId +
                       " AND AGAktTypeActionUserProvAktAktionTypeID = " + actionTypeId;


            tblAuftraggeberAktTypeActionUserProv agAktTypeUserProv = (tblAuftraggeberAktTypeActionUserProv)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeberAktTypeActionUserProv" + sqlWhere, typeof(tblAuftraggeberAktTypeActionUserProv));

            if (agAktTypeUserProv != null)
            {
                if (agAktTypeUserProv.AGAktTypeActionUserProvHonGrpID > 0)
                    price = GetHonorarPrice(agAktTypeUserProv.AGAktTypeActionUserProvHonGrpID, collectedAmount, balance);
                else if (agAktTypeUserProv.AGAktTypeActionUserProvPrice > 0)
                    price = agAktTypeUserProv.AGAktTypeActionUserProvPrice;
            }
            if (price == 0)
            {
                sqlWhere = " WHERE UserAktTypeAktionProvUserID = " + userId +
                           " AND UserAktTypeActionProvAktTypeIntID = " + aktIntTypeId +
                           " AND UserAktTypeActionProvAktAktionTypeID = " + actionTypeId;
                tblUserAktTypeActionProv userAktTypeProv = (tblUserAktTypeActionProv)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUserAktTypeActionProv " + sqlWhere, typeof(tblUserAktTypeActionProv));
                if (userAktTypeProv != null)
                {
                    if (userAktTypeProv.UserAktTypeActionProvHonGrpID > 0)
                        price = GetHonorarPrice(userAktTypeProv.UserAktTypeActionProvHonGrpID, collectedAmount, balance);
                    else if (userAktTypeProv.UserAktTypeActionProvPrice > 0)
                        price = userAktTypeProv.UserAktTypeActionProvPrice;
                }
            }
            if (price == 0)
            {
                sqlWhere = " WHERE AGAktTypeAktionProvAuftraggeberID = " + agId +
                           " AND AGAktTypeActionProvAktTypeIntID = " + aktIntTypeId +
                           " AND AGAktTypeActionProvAktAktionTypeID = " + actionTypeId;
                tblAuftraggeberAktTypeActionProv agAktTypeProv = (tblAuftraggeberAktTypeActionProv)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeberAktTypeActionProv " + sqlWhere, typeof(tblAuftraggeberAktTypeActionProv));
                if (agAktTypeProv != null)
                {
                    if (agAktTypeProv.AGAktTypeActionProvHonGrpID > 0)
                        price = GetHonorarPrice(agAktTypeProv.AGAktTypeActionProvHonGrpID, collectedAmount, balance);
                    else if (agAktTypeProv.AGAktTypeActionProvPrice > 0)
                        price = agAktTypeProv.AGAktTypeActionProvPrice;
                }
            }
            if (price == 0)
            {
                sqlWhere = " WHERE AGUserProvAuftraggeberID = " + agId +
                                 " AND AGUserProvUserID = " + userId +
                                 " AND AGUserProvAktAktionTypeID = " + actionTypeId;
                tblAuftraggeberUserProv agUserProv = (tblAuftraggeberUserProv)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeberUserProv " + sqlWhere, typeof(tblAuftraggeberUserProv));
                if (agUserProv != null)
                {
                    if (agUserProv.AGUserProvHonGrpID > 0)
                        price = GetHonorarPrice(agUserProv.AGUserProvHonGrpID, collectedAmount, balance);
                    else if (agUserProv.AGUserProvPrice > 0)
                        price = agUserProv.AGUserProvPrice;
                }
            }
            if (price == 0)
            {
                sqlWhere = " WHERE UserAktionProvUserID = " + userId +
                           " AND UserActionProvAktAktionTypeID = " + actionTypeId;
                tblUserActionProv userActionProv = (tblUserActionProv)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUserActionProv " + sqlWhere, typeof(tblUserActionProv));
                if (userActionProv != null)
                {
                    if (userActionProv.UserActionProvHonGrpID > 0)
                        price = GetHonorarPrice(userActionProv.UserActionProvHonGrpID, collectedAmount, balance);
                    else if (userActionProv.UserActionProvPrice > 0)
                        price = userActionProv.UserActionProvPrice;
                }
            }
            if (price == 0)
            {
                sqlWhere = " WHERE AGAktionProvAuftraggeberID = " + agId +
                           " AND AGActionProvAktAktionTypeID = " + actionTypeId;
                tblAuftraggeberActionProv agActionProv = (tblAuftraggeberActionProv)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeberActionProv " + sqlWhere, typeof(tblAuftraggeberActionProv));
                if (agActionProv != null)
                {
                    if (agActionProv.AGActionProvHonGrpID > 0)
                        price = GetHonorarPrice(agActionProv.AGActionProvHonGrpID, collectedAmount, balance);
                    else if (agActionProv.AGActionProvPrice > 0)
                        price = agActionProv.AGActionProvPrice;
                }
            }
            if (price == 0)
            {
                sqlWhere = " WHERE AGAktTypeProvAuftraggeberID = " + agId +
                           " AND AGAktTypeProvAktTypeIntID = " + aktIntTypeId;
                tblAuftraggeberAktTypeProv agType = (tblAuftraggeberAktTypeProv)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeberAktTypeProv " + sqlWhere, typeof(tblAuftraggeberAktTypeProv));
                if (agType != null)
                {
                    if (agType.AGAktTypeProvProvisionHonGrpID > 0)
                        price = GetHonorarPrice(agType.AGAktTypeProvProvisionHonGrpID, collectedAmount, balance);
                    else if (agType.AGAktTypeProvProvisionPrice > 0)
                        price = agType.AGAktTypeProvProvisionPrice;
                }
            }
            if (price == 0)
            {
                sqlWhere = " WHERE AktIntActionTypeID = " + actionTypeId;
                tblAktenIntActionType action = GetSelectedActionType();
                if (action != null)
                {
                    if (action.AktIntActionProvHonGrpID > 0)
                        price = GetHonorarPrice(action.AktIntActionProvHonGrpID, collectedAmount, balance);
                    else if (action.AktIntActionProvPrice > 0)
                        price = action.AktIntActionProvPrice;
                }
            }
            if (price == 0)
            {
                sqlWhere = " WHERE AuftraggeberID = " + agId;
                tblAuftraggeber ag = (tblAuftraggeber)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeber " + sqlWhere, typeof(tblAuftraggeber));
                if (ag != null)
                {
                    if (ag.AuftraggeberInterventionsKost > 0)
                        price = ag.AuftraggeberInterventionsKost;
                }
            }
            return price;
        }

        private double GetHonorarPrice(int honorarGrpId, double collectedAmount, double balance)
        {
            double ret = 0;
            string sqlQuery = "SELECT * FROM qryAktenIntGroupHonorar WHERE AktIntHonGrpID = " + honorarGrpId + " AND AktIntHonFrom <= " + collectedAmount + " AND AktIntHonTo >= " + collectedAmount;
            qryAktenIntGroupHonorar honorar = (qryAktenIntGroupHonorar)HTBUtils.GetSqlSingleRecord(sqlQuery.Replace(",", "."), typeof(qryAktenIntGroupHonorar));
            if (honorar != null)
            {
                ret = honorar.AktIntHonPrice;
                if (honorar.AktIntHonPct > 0)
                {
                    if (honorar.AktIntHonPctOf == 0)
                    {
                        ret += (honorar.AktIntHonPct / 100) * collectedAmount;
                    }
                    else
                    {
                        ret += (honorar.AktIntHonPct / 100) * balance;
                    }
                }
                if (ret > honorar.AktIntHonMaxPrice)
                {
                    ret = honorar.AktIntHonMaxPrice;
                }
            }

            return ret;
        }

        private double GetBalance()
        {
            double balance = 0;
            if (_akt != null)
            {
                if (_akt.IsInkasso())
                {
                    AktUtils aktUtils = new AktUtils(_akt.AktIntCustInkAktID);
                    balance = aktUtils.GetAktBalance();
                }
                else
                {
                    ArrayList posList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntPos WHERE AktIntPosAkt = " + _akt.AktIntID, typeof(tblAktenIntPos));
                    foreach (HTB.Database.tblAktenIntPos AktIntPos in posList)
                    {
                        balance += AktIntPos.AktIntPosBetrag;
                    }
                    balance += _akt.AKTIntZinsenBetrag;
                    balance += _akt.AKTIntKosten;
                    balance += _akt.AktIntWeggebuehr;
                }
            }
            return balance;
        }
        #endregion

    }
}