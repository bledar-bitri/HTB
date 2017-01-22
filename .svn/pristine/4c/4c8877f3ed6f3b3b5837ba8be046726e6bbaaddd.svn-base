using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.global_files
{
    public partial class CtlWorkflow : UserControl
    {

        public ArrayList WorkflowList { get; set; }
        public int InkassoAktID { get; set; }

        private IWorkflow _wflInterface;

        public void SetWftInterface(IWorkflow intf)
        {
            _wflInterface = intf;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WorkflowList = new ArrayList();
            if (!IsPostBack)
            {
                GlobalUtilArea.LoadDropdownList(ddlAktMainStatus,
                                                "SELECT * FROM tblCustInkAktStatus ORDER BY CustInkAktStatusCaption ASC",
                                                typeof (tblCustInkAktStatus),
                                                "CustInkAktStatusCode",
                                                "CustInkAktStatusCaption", false);

                GlobalUtilArea.LoadDropdownList(ddlAktCurrentStatus,
                                                "SELECT * FROM qryCustInkAktStatusKZ ORDER BY KZCaption ASC",
                                                typeof (tblKZ),
                                                "KZID",
                                                "KZCaption", false);

                int aktId = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.ID]);
                if (InkassoAktID > 0)
                    aktId = InkassoAktID;
                if (aktId > 0)
                {
                    tblCustInkAkt akt = HTBUtils.GetInkassoAkt(aktId);
                    if (akt != null)
                    {
                        if (!ExistsAktWorkflow(akt.CustInkAktID))
                            LoadWorkFlowFromClient(akt.CustInkAktKlient, akt.CustInkAktID);
                        PopulateWorkFlow(akt.CustInkAktID);
                    }
                }

                string selectedMainStatus = _wflInterface.GetSelectedMainStatus();
                string selectedSecondaryStatus = _wflInterface.GetSelectedSecondaryStatus();

                if (!string.IsNullOrEmpty(selectedMainStatus))
                    ddlAktMainStatus.SelectedValue = selectedMainStatus;

                if (!string.IsNullOrEmpty(selectedSecondaryStatus))
                    ddlAktCurrentStatus.SelectedValue = selectedSecondaryStatus;
            }
        }
        
        public void PopulateWorkFlow(int aktId)
        {
            ClearWorkFlowScreen();

            #region Current Action Section (on top)
            tblCustInkAkt akt = HTBUtils.GetInkassoAkt(aktId);
            if (akt != null)
            {
                LoadNextActionDropDownList();
                if (akt.CustInkAktCurrentStep == -1)
                {
                    lblCurrentAction.Text = "Fertig";
                    try
                    {
                        ddlNextAction.SelectedValue = akt.CustInkAktCurrentStep.ToString();
                    }
                    catch(Exception ex)
                    {
                        ctlMessage.ShowException(ex);
                    }
                }
                else if (akt.CustInkAktCurrentStep >= 0)
                {
                    string crrAction = GetWorkFlowStepAction(akt.CustInkAktCurrentStep);
                    string nextAction = GetWorkFlowStepAction(akt.CustInkAktCurrentStep + 1);
                    if (akt.CustInkAktCurrentStep == 0)
                    {
                        crrAction = "Start";
                    }
                    if (crrAction != null)
                    {
                        lblCurrentAction.Text = crrAction;
                        txtNextActionExecDate.Text = akt.CustInkAktNextWFLStep.ToShortDateString();
                        try
                        {
                            ddlNextAction.SelectedValue = nextAction != null ? (akt.CustInkAktCurrentStep + 1).ToString() : "-1";
                        }
                        catch(Exception ex)
                        {
                            ctlMessage.ShowException(ex);
                        }
                    }
                    else
                    {
                        lblCurrentAction.Text = "ERROR (Kontakt ECP)";
                    }
                }
                try
                {
                    ddlAktMainStatus.SelectedValue = akt.CustInkAktStatus.ToString();
                }
                catch (Exception ex)
                {
                    ctlMessage.ShowException(ex);
                }
                try
                {
                    ddlAktCurrentStatus.SelectedValue = akt.CustInkAktCurStatus.ToString();
                }
                catch (Exception ex)
                {
                    ctlMessage.ShowException(ex);
                }
                SetWflStopped(akt.CustInkAktIsWflStopped);
            }
            #endregion

            bool hasIntervention = false;
            foreach (tblWFA wfa in WorkflowList)
            {
                switch (wfa.WFPAktion)
                {
                    case 1:
                        chk1Mah.Checked = true;
                        txtDuration1Mah.Text = GetWorkFlowStepDuration(wfa.WFPPosition + 1).ToString(); // get the wait time from the next step
                        break;
                    case 2:
                        chk2Mah.Checked = true;
                        txtDuration2Mah.Text = GetWorkFlowStepDuration(wfa.WFPPosition + 1).ToString(); // get the wait time from the next step
                        break;
                    case 3:
                        chk3Mah.Checked = true;
                        txtDuration3Mah.Text = GetWorkFlowStepDuration(wfa.WFPPosition + 1).ToString(); // get the wait time from the next step
                        break;
                    case 6:
                        chkIntervention.Checked = true;
                        txtDurationIntervention.Text = GetWorkFlowStepDuration(wfa.WFPPosition + 1).ToString(); // get the wait time from the next step
                        hasIntervention = true;
                        break;
                    case 18:
                        if (!hasIntervention)
                        {
                            chkTelefonInk.Checked = true;
                            txtDurationTelefon.Text = GetWorkFlowStepDuration(wfa.WFPPosition + 1).ToString(); // get the wait time from the next step
                        }
                        else
                        {
                            chkTelefonInkAfterInt.Checked = true;
                            txtDurationTelefonAfterInt.Text = GetWorkFlowStepDuration(wfa.WFPPosition + 1).ToString(); // get the wait time from the next step
                        }
                        break;
                    case 14:
                        chkRechtsanwalt.Checked = true;
                        txtDurationRechtsanwalt.Text = GetWorkFlowStepDuration(wfa.WFPPosition + 1).ToString(); // get the wait time from the next step
                        break;
                    case 19:
                        chkRechtsanwaltErinerung.Checked = true;
                        txtDurationRechtsanwaltErinerung.Text = GetWorkFlowStepDuration(wfa.WFPPosition + 1).ToString(); // get the wait time from the next step
                        break;
                }
            }
        }

        private int GetWorkFlowStepDuration(int position)
        {
            return (from tblWFA wfa in WorkflowList where wfa.WFPPosition == position select wfa.WFPPreTime).FirstOrDefault();
        }

        public string GetWorkFlowStepAction(int position)
        {
            return (from tblWFA wfa in WorkflowList where wfa.WFPPosition == position select GetActionText(wfa.WFPAktion)).FirstOrDefault();
        }

        public static string GetActionText(int action)
        {
            switch (action)
            {
                case 1:
                    return "Erste Mahnung";
                case 2:
                    return "Zweite Mahnung";
                case 3:
                    return "Dritte Mahnung";
                case 6:
                    return "Intervention";
                case 18:
                    return "Telefoninkasso";
                case 14:
                    return "Rechtsanwalt";
                case 19:
                    return "Erinerung EMail an Rechtsanwalt";
                case 68:
                    return "Fertig";
                default:
                    return "Unbekant";
            }
        }

        public bool ValidateWorkflow(bool isInterventionAllowed = true)
        {
            if (chkTelefonInkAfterInt.Checked && !chkIntervention.Checked)
            {
                ctlMessage.ShowError("Sie m&uuml;ssen Intervention w&auml;hlen wenn Sie Telefoninkasso  (nach Intervention) w&auml;hlen!");
                return false;
            }
            if(chkIntervention.Checked && !isInterventionAllowed)
            {
                ctlMessage.ShowError("<strong><i>Gegner im Ausland:</i></strong> kein Intervention m&ouml;glich!<br/>");
                return false;
            }
            return true;
        }

        public void SaveWorkFlow(int id, bool isClient)
        {

            DeleteAktWorkFlow(id, isClient);
            if (!isClient)
                SaveAktWorkFlowStepStatusAndDate(id);
        
            int position = 1;
            int preTime = 0;
            if (chk1Mah.Checked)
            {
                CreateWorkFlowRecord(id, 1, position, preTime, isClient);
                position++;
                preTime = GlobalUtilArea.GetZeroIfConvertToIntError(txtDuration1Mah.Text);
            }
            if (chkTelefonInk.Checked)
            {
                CreateWorkFlowRecord(id, 18, position, preTime, isClient);
                position++;
                preTime = GlobalUtilArea.GetZeroIfConvertToIntError(txtDurationTelefon.Text);
            }
            if (chkIntervention.Checked)
            {
                CreateWorkFlowRecord(id, 6, position, preTime, isClient);
                position++;
                preTime = GlobalUtilArea.GetZeroIfConvertToIntError(txtDurationIntervention.Text);
            }
            if (chkTelefonInkAfterInt.Checked)
            {
                CreateWorkFlowRecord(id, 18, position, preTime, isClient);
                position++;
                preTime = GlobalUtilArea.GetZeroIfConvertToIntError(txtDurationTelefonAfterInt.Text);
            }
            if (chk2Mah.Checked)
            {
                CreateWorkFlowRecord(id, 2, position, preTime, isClient);
                position++;
                preTime = GlobalUtilArea.GetZeroIfConvertToIntError(txtDuration2Mah.Text);
            }
            if (chk3Mah.Checked)
            {
                CreateWorkFlowRecord(id, 3, position, preTime, isClient);
                position++;
                preTime = GlobalUtilArea.GetZeroIfConvertToIntError(txtDuration3Mah.Text);
            }
            if (chkRechtsanwalt.Checked)
            {
                CreateWorkFlowRecord(id, 14, position, preTime, isClient);
                position++;
                preTime = GlobalUtilArea.GetZeroIfConvertToIntError(txtDurationRechtsanwalt.Text);
            }
            if (chkRechtsanwaltErinerung.Checked)
            {
                CreateWorkFlowRecord(id, 19, position, preTime, isClient);
                position++;
                preTime = GlobalUtilArea.GetZeroIfConvertToIntError(txtDurationRechtsanwaltErinerung.Text);
            }
            if(!isClient)
                CreateWorkFlowRecord(id, 68, position, preTime, isClient);
        }

        private void SaveAktWorkFlowStepStatusAndDate(int aktId)
        {
            var set = new RecordSet();
            try
            {
                int selectedNextWFPosition = Convert.ToInt32(ddlNextAction.SelectedValue);

                var status = new StringBuilder();
                status.Append("CustInkAktStatus = ");
                status.Append(ddlAktMainStatus.SelectedValue);
                status.Append(", CustInkAktCurStatus = ");
                status.Append(ddlAktCurrentStatus.SelectedValue);
                status.Append(", CustInkAktIsWflStopped = ");
                status.Append(chkStopWorkflow.Checked ? "1" : "0");

                if (selectedNextWFPosition > 0)
                {
                    var nextWFDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtNextActionExecDate.Text);
                    if (nextWFDate.Year == 1900)
                        nextWFDate = DateTime.Now;
                    set.ExecuteNonQuery("UPDATE tblCustInkAkt set " + status + ", CustInkAktCurrentStep = " + (selectedNextWFPosition - 1) + ", CustInkAktNextWFLStep = '" + nextWFDate.ToShortDateString() + "' WHERE CustInkAktID = " + aktId);
                }
                else if (selectedNextWFPosition == -1)
                {
                    set.ExecuteNonQuery("UPDATE tblCustInkAkt set " + status + ", CustInkAktCurrentStep = " + selectedNextWFPosition + " WHERE CustInkAktID = " + aktId);
                }
            }
            catch{}
        }

        private void CreateWorkFlowRecord(int id, int action, int position, int preTime, bool isClient)
        {
            if(isClient)
                RecordSet.Insert(new tblWFK { WFPKlient = id, WFPAktion = action, WFPPosition = position, WFPPreTime = preTime });
            else
                RecordSet.Insert(new tblWFA { WFPAkt = id, WFPAktion = action, WFPPosition = position, WFPPreTime = preTime });
        }
        
        private bool ExistsAktWorkflow(int aktId)
        {
            WorkflowList = HTBUtils.GetSqlRecords("SELECT * FROM tblWFA WHERE WFPAkt = " + aktId + " ORDER BY WFPPosition", typeof(tblWFA));
            return WorkflowList.Count > 0;
        }

        private void LoadWorkFlowFromClient(int klientId, int aktId)
        {
            ArrayList wfkList = HTBUtils.GetSqlRecords("SELECT * FROM tblWFK WHERE WFPKlient = " + klientId + " ORDER BY WFPPosition", typeof(tblWFK));
            WorkflowList.Clear();
            foreach (tblWFK wfk in wfkList)
            {
                var wfa = new tblWFA();
                wfa.Assign(wfk);
                if (aktId > 0)
                    wfa.WFPAkt = aktId;
                WorkflowList.Add(wfa);
            }
        }

        public void LoadWorkFlow(int aktId)
        {
            ExistsAktWorkflow(aktId);
        }
        
        public void ClearEntireScreen()
        {
            txtNextActionExecDate.Text = "";
            ddlAktMainStatus.SelectedValue = "0";
            ddlAktCurrentStatus.SelectedValue = "1";

            lblCurrentAction.Text = "&nbsp;";
            ddlNextAction.Items.Clear();
            txtNextActionExecDate.Text = "";

            ClearWorkFlowScreen();

        }
        public void ClearWorkFlowScreen()
        {
            chk1Mah.Checked = false;
            chk2Mah.Checked = false;
            chk3Mah.Checked = false;
            chkTelefonInk.Checked = false;
            chkTelefonInkAfterInt.Checked = false;
            chkIntervention.Checked = false;
            chkRechtsanwalt.Checked = false;
            chkRechtsanwaltErinerung.Checked = false;

            txtDuration1Mah.Text = "";
            txtDuration2Mah.Text = "";
            txtDuration3Mah.Text = "";
            txtDurationTelefon.Text = "";
            txtDurationTelefonAfterInt.Text = "";
            txtDurationIntervention.Text = "";
            txtDurationRechtsanwalt.Text = "";
            txtDurationRechtsanwaltErinerung.Text = "";
        }

        private void DeleteAktWorkFlow(int aktId, bool isClient)
        {
            var set = new RecordSet();
            if(isClient)
                set.ExecuteNonQuery("DELETE FROM tblWFK WHERE WFPKlient = " + aktId);
            else
                set.ExecuteNonQuery("DELETE FROM tblWFA WHERE WFPAkt = " + aktId);
        }

        public bool IsWorkflowEntered()
        {
            return chk1Mah.Checked
                || chk1Mah.Checked
                || chk3Mah.Checked
                || chkTelefonInk.Checked
                || chkIntervention.Checked
                || chkTelefonInkAfterInt.Checked
                || chkRechtsanwalt.Checked
                || chkRechtsanwaltErinerung.Checked;

        }

        private void LoadNextActionDropDownList()
        {
            ddlNextAction.Items.Clear();
            foreach (tblWFA wfa in WorkflowList)
            {
                ddlNextAction.Items.Add(new ListItem(GetWorkFlowStepAction(wfa.WFPPosition), wfa.WFPPosition.ToString()));
            }
        }

        #region Event Handlers
        protected void ddlNextAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAktMainStatus.SelectedIndex = ddlAktMainStatus.Items.IndexOf(ddlAktMainStatus.Items.FindByText("CollectionInvoice"));
        }

        protected void cmdLoadWFL_Click(object sender, EventArgs e)
        {
            string nextActionDate = GetNextActionExecDateText(); // keep a copy of this date coz it gets wiped out during the process
            ClearMessage();
            ctlMessage.Clear();
            int klientId = GlobalUtilArea.GetZeroIfConvertToIntError(_wflInterface.GetKlientID());
            if (klientId > 0)
            {
                LoadWorkFlowFromClient(GlobalUtilArea.GetZeroIfConvertToIntError(_wflInterface.GetKlientID()), -1);
                PopulateWorkFlow(-1);
                SetNextActionExecDateText(nextActionDate); // set the date back to what it was
            }
            else
            {
                ShowError("Bitte wählen Sie einen Klient");
            }
        }
        protected void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            int aktId = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.ID]);
            
            if (aktId <= 0 || !ValidateWorkflow()) return;

            SaveWorkFlow(aktId, false);
            ExistsAktWorkflow(aktId);
            PopulateWorkFlow(aktId);
            ctlMessage.ShowSuccess("Status gespeichert!");
        }
        #endregion

        #region Message
        public void ClearMessage()
        {
            ctlMessage.Clear();
        }
        public void ShowError(string message)
        {
            ctlMessage.ShowError(message);
        }
        public void ShowSuccess(string message)
        {
            ctlMessage.ShowSuccess(message);
        }
        public void ShowException(Exception e)
        {
            ctlMessage.ShowException(e);
        }
        public void AppendError(string message)
        {
            ctlMessage.AppendError(message);
        }
        #endregion

        #region Access Methods
        public void SetDateDescription(string description)
        {
            lblDateDescription.Text = description;
        }
        public bool IsWflStopped()
        {
            return chkStopWorkflow.Checked;
        }
        public void SetWflStopped(bool stopped)
        {
            chkStopWorkflow.Checked = stopped;
        }
        public DateTime GetNextWFLStepDate()
        {
            return GlobalUtilArea.GetNowIfConvertToDateError(txtNextActionExecDate);
        }
        public int GetMainStatus()
        {
            return GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktMainStatus.SelectedValue);
            
        }
        public int GetSecondaryStatus()
        {
            return GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktCurrentStatus.SelectedValue);
        }
        public string GetMainStatusText()
        {
            return ddlAktMainStatus.SelectedValue;

        }
        public string GetSecondaryStatusText()
        {
            return ddlAktCurrentStatus.SelectedValue;
        }
        public string GetNextActionExecDateText()
        {
            return txtNextActionExecDate.Text;
        }
        public void SetNextActionExecDateText(string text)
        {
            txtNextActionExecDate.Text = text;
        }
        public DropDownList GetDdlMainStatus()
        {
            return ddlAktMainStatus;
        }
        public DropDownList GetDdlSecondaryStatus()
        {
            return ddlAktCurrentStatus;
        }
        public TextBox GetTxtNextActionExecDate()
        {
            return txtNextActionExecDate;
        }
        public CheckBox GetChkIsWflStopped()
        {
            return chkStopWorkflow;
        }

        public void setDurationForMahnung1(int period)
        {
            if(period >= 0)
            {
                chk1Mah.Checked = true;
                txtDuration1Mah.Text = period.ToString();
            }
            else
            {
                chk1Mah.Checked = false;
                txtDuration1Mah.Text = "";
            }
        }
        public void setDurationForMahnung2(int period)
        {
            if (period >= 0)
            {
                chk2Mah.Checked = true;
                txtDuration2Mah.Text = period.ToString();
            }
            else
            {
                chk2Mah.Checked = false;
                txtDuration2Mah.Text = "";
            }
        }
        public void setDurationForMahnung3(int period)
        {
            if (period >= 0)
            {
                chk3Mah.Checked = true;
                txtDuration3Mah.Text = period.ToString();
            }
            else
            {
                chk3Mah.Checked = false;
                txtDuration3Mah.Text = "";
            }
        }
        public void setDurationForTelefonInkasso(int period)
        {
            if (period >= 0)
            {
                chkTelefonInk.Checked = true;
                txtDurationTelefon.Text = period.ToString();
            }
            else
            {
                chkTelefonInk.Checked = false;
                txtDurationTelefon.Text = "";
            }
        }
        public void setDurationForTelefonInkassoAfterIntervention(int period)
        {
            if (period >= 0)
            {
                chkTelefonInkAfterInt.Checked = true;
                txtDurationTelefonAfterInt.Text = period.ToString();
            }
            else
            {
                chkTelefonInkAfterInt.Checked = false;
                txtDurationTelefonAfterInt.Text = "";
            }
        }
        public void setDurationForIntervention(int period)
        {
            if (period >= 0)
            {
                chkIntervention.Checked = true;
                txtDurationIntervention.Text = period.ToString();
            }
            else
            {
                chkIntervention.Checked = false;
                txtDurationIntervention.Text = "";
            }
        }
        public void setDurationForRechtsanwalt(int period)
        {
            if (period >= 0)
            {
                chkRechtsanwalt.Checked = true;
                txtDurationRechtsanwalt.Text = period.ToString();
            }
            else
            {
                chkRechtsanwalt.Checked = false;
                txtDurationRechtsanwalt.Text = "";
            }
        }
        public void setDurationForRechtsanwaltErinerung(int period)
        {
            if (period >= 0)
            {
                chkRechtsanwaltErinerung.Checked = true;
                txtDurationRechtsanwaltErinerung.Text = period.ToString();
            }
            else
            {
                chkRechtsanwaltErinerung.Checked = false;
                txtDurationRechtsanwaltErinerung.Text = "";
            }
        }
        public void ShowClientScreen()
        {
            SetLastActionVisble(false);
            SetNextActionVisible(false);
            SetDateVisble(false);
            SetMainStatusVisible(false);
            SetSecondaryStatusVisible(false);
            SetStopWorkFlowVisible(false);
            SetLoadWflFromClientVisible(false);
        }
       
        #endregion

        #region Switch Visible / Invisible 
        public void SetLastActionVisble(bool visible)
        {
            trLastAction.Visible = visible;
        }
        public void SetNextActionVisible(bool visible)
        {
            trNextAction.Visible = visible;
        }
        public void SetDateVisble(bool visible)
        {
            trDate.Visible = visible;
        }
        public void SetMainStatusVisible(bool visible)
        {
            trMainStatus.Visible = visible;
        }
        public void SetSecondaryStatusVisible(bool visible)
        {
            trSecondaryStatus.Visible = visible;
        }
        public void SetStopWorkFlowVisible(bool visible)
        {
            trStopWfl.Visible = visible;
        }
        public void SetLoadWflFromClientVisible(bool visible)
        {
            trLoadWflFromClient.Visible = visible;
        }
        public void SetBtnUpdateStatusVisible(bool visible)
        {
            btnUpdateStatus.Visible = visible;
        }
        #endregion
    }
}