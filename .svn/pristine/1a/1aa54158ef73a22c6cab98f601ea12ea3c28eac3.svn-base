using System;
using System.Web.UI;
using HTB.Database;
using HTBUtilities;
using System.Collections;
using HTB.v2.intranetx.util;
using HTBAktLayer;
using System.Text;
using System.Data;
using HTB.Database.Views;
using HTB.v2.intranetx.global_files;

namespace HTB.v2.intranetx.aktenint
{
    public partial class EditAktInt : Page, IWorkflow
    {
        public qryAktenInt aktInt = new qryAktenInt();
        public int aktId = 0;
        public ArrayList posList = new ArrayList();
        public ArrayList actionsList = new ArrayList();
        public ArrayList docsList = new ArrayList();
        public ArrayList workflowList = new ArrayList();
        public double totalAmountDue;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request["ID"] != null && !Request["ID"].Equals(""))
                {
                    aktId = Convert.ToInt32(Request["ID"]);
                    LoadRecords();
                    if (!IsPostBack)
                    {
                        GlobalUtilArea.LoadDropdownList(ddlAktType,
                                                        "SELECT * FROM tblAktTypeINT ORDER BY AktTypeINTCaption ASC",
                                                        typeof (tblAktTypeInt),
                                                        "AktTypeINTID",
                                                        "AktTypeINTCaption", false);

                        GlobalUtilArea.LoadUserDropdownList(ddlSB, GlobalUtilArea.GetUsers(Session));

                        SetValues();
                    }
                }
                ctlWorkflow.SetWftInterface(this);
                ctlWorkflow.SetLoadWflFromClientVisible(false);
                ctlWorkflow.SetDateVisble(false);
                ctlWorkflow.SetLastActionVisble(false);
            }
            catch(Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }

        private void LoadRecords()
        {
            aktInt = (qryAktenInt)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenInt WHERE AktIntID = " + aktId, typeof(qryAktenInt)) ?? new qryAktenInt();
            if(aktInt.AktTypeINTCode == 2 || aktInt.AktTypeINTCode == 10)
            {
                // auto akt
                Session[GlobalHtmlParams.INTERVENTION_WORK_AKT] = null; // clear temporary akt in session
                Response.Redirect("EditAktIntAuto.aspx?" + GlobalHtmlParams.ID + "=" + aktId);
            }
            posList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntPos WHERE AktIntPosAkt = " + aktId, typeof(tblAktenIntPos));
            actionsList = HTBUtils.GetSqlRecords("SELECT * FROM qryInktAktAction WHERE AktIntActionAkt = " + aktId, typeof(qryInktAktAction));
            docsList = HTBUtils.GetSqlRecords("SELECT * FROM qryDoksIntAkten WHERE AktIntID = " + aktId, typeof(qryDoksIntAkten));
            if (aktInt != null && aktInt.IsInkasso())
            {
                ctlWorkflow.LoadWorkFlow(aktInt.AktIntCustInkAktID);
                ctlWorkflow.PopulateWorkFlow(aktInt.AktIntCustInkAktID);
                ctlWorkflow.InkassoAktID = aktInt.AktIntCustInkAktID;
            }
        }

        private void SetValues()
        {
            txtAktNr.Text = aktInt.AktIntID.ToString();
            txtAnlagedatum.Text = aktInt.AktIntDatum.ToShortDateString();
            txtAZ.Text = aktInt.AktIntAZ;
            txtAGN.Text = aktInt.AktIntAuftraggeber.ToString();
            txtAGNName.Text = aktInt.AuftraggeberName1;
            txtKLN.Text = aktInt.AktIntKlient;
            txtKLName.Text = aktInt.KlientName1;
            txtGEN.Text = aktInt.AktIntGegner;
            txtGEName.Text = aktInt.GegnerLastName1;
            txtAktIntIZ.Text = aktInt.AktIntIZ;
            txtWeg.Text = aktInt.AKTIntDistance.ToString();
            txtCash.Text = aktInt.AktIntWeggebuehr.ToString("N2");
            txtTermin.Text = aktInt.AktIntTermin.ToShortDateString();
            txtTerminAD.Text = aktInt.AktIntTerminAD.ToShortDateString();
            txtAktIntSBAG.Text = aktInt.AKTIntAGSB;
            txtZinssatz.Text = aktInt.AKTIntZinsen.ToString("N2");
            txtZinsen.Text = aktInt.AKTIntZinsenBetrag.ToString("N2");
            txtKosten.Text = aktInt.AKTIntKosten.ToString("N2");
            lblOriginalMemo.Text = aktInt.AktIntOriginalMemo;
            txtMemo.Text = aktInt.AKTIntMemo;
            txtKsvEmail.Text = aktInt.AKTIntKSVEMail;

            ddlAktType.SelectedValue = aktInt.AktIntAktType.ToString();
            ddlSB.SelectedValue = aktInt.AktIntSB.ToString();
            ddlSatzStatus.SelectedValue = aktInt.AktIntStatus.ToString();
            chkShowPrinted.Checked = aktInt.AKTIntDruckkennz == 1;
            chkWiedervorlage.Checked = aktInt.AKTIntVormerk == 1;

            tdKlientText.InnerHtml = GetClientTextSection();

            PopulateInvoicesGrid();
            PopulateDocumentsGrid();
        }
        
        #region Even Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                if(SaveEnteredData())
                    if (GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT]) == GlobalHtmlParams.BACK)
                        ScriptManager.RegisterStartupScript(this.updPanel1, typeof(string), "backScript", "javascript:history.go(-1);", true);
                    else
                        ShowParentScreen();
            }
            catch( Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
        protected void txtAGN_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int ag = -1;
                try
                {
                    ag = Convert.ToInt32(txtAGN.Text);
                }
                catch { ag = -1; }

                if (ag >= 0)
                {
                    var auftraggeber = (tblAuftraggeber)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeber WHERE AuftraggeberID = " + ag, typeof(tblAuftraggeber));
                    if (auftraggeber != null)
                    {
                        txtAGNName.Text = auftraggeber.AuftraggeberName1;
                        txtKLN.Focus();
                    }
                    else
                    {
                        txtAGNName.Text = "UNBEKANT";
                        txtAGN.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
        protected void txtKLN_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var klient = (tblKlient)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKlient WHERE KlientOldID = '" + txtKLN.Text + "'", typeof(tblKlient));
                if (klient != null)
                {
                    txtKLName.Text = klient.KlientName1;
                    tdKlientText.InnerHtml = GetClientTextSection(klient);
                    txtGEN.Focus();
                }
                else
                {
                    txtKLName.Text = "UNBEKANT";
                    txtKLN.Focus();
                }
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
        protected void txtGEN_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerOldID = '" + txtGEN.Text + "'", typeof(tblGegner));
                if (gegner != null)
                {
                    txtGEName.Text = gegner.GegnerName1;
                    SetDistance();
                    btnFindUser_Click(sender, e);
                    ddlSB.Focus();
                }
                else
                {
                    txtGEName.Text = "UNBEKANT";
                    txtGEN.Focus();
                }
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
        protected void cmdCalcWeg_Click(object sender, EventArgs e)
        {
            try{
            SetDistance();
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
        protected void btnFindUser_Click(object sender, EventArgs e)
        {
            try
            {
                var qryGebiete = (qryADGebiete)HTBUtils.GetSqlSingleRecord("SELECT * FROM dbo.qryADGebiete WHERE ADGEBIETSTARTZIP <= " + aktInt.GegnerLastZip + " AND ADGEBIETENDZIP >= " + aktInt.GegnerLastZip, typeof(qryADGebiete));
                if (qryGebiete != null)
                {
                    ddlSB.SelectedValue = qryGebiete.ADGebietUser.ToString();
                    SetDistance();
                }
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
        protected void btnCash_Click(object sender, EventArgs e)
        {
            try
            {
                SetWeggebuher();
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
        protected void btnMitteilung_Click(object sender, EventArgs e)
        {
            try{
                SaveEnteredData();
                ScriptManager.RegisterStartupScript(this.updPanel1, typeof(string), "EMailScript", "MM_openBrWindow('" + GetEmailLink() + "','sendmitteilung','menubar=yes,scrollbars=yes,resizable=yes,width=500,height=400')", true);
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
        protected void ddlSB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                SetDistance();
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT]) == GlobalHtmlParams.BACK)
                    ScriptManager.RegisterStartupScript(this.updPanel1, typeof(string), "backScript", "javascript:history.go(-1);", true);
                else
                    ShowParentScreen();
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
        #endregion

        private bool SaveEnteredData()
        {
            bool ok = true;
            try
            {
                aktInt.AktIntDatum = Convert.ToDateTime(txtAnlagedatum.Text);
                aktInt.AktIntAZ = txtAZ.Text;
                aktInt.AktIntAuftraggeber = Convert.ToInt32(txtAGN.Text);
                aktInt.AuftraggeberName1 = txtAGNName.Text;
                aktInt.AktIntKlient = txtKLN.Text;
                aktInt.KlientName1 = txtKLName.Text;
                aktInt.AktIntGegner = txtGEN.Text;
                aktInt.GegnerLastName1 = txtGEName.Text;
                aktInt.AktIntIZ = txtAktIntIZ.Text;
                aktInt.AKTIntDistance = Convert.ToInt32(txtWeg.Text);
                aktInt.AktIntWeggebuehr = Convert.ToDouble(txtCash.Text);
                aktInt.AktIntTermin = Convert.ToDateTime(txtTermin.Text);
                aktInt.AktIntTerminAD = Convert.ToDateTime(txtTerminAD.Text);
                aktInt.AKTIntAGSB = txtAktIntSBAG.Text;
                aktInt.AKTIntZinsen = Convert.ToDouble(txtZinssatz.Text);
                aktInt.AKTIntZinsenBetrag = Convert.ToDouble(txtZinsen.Text);
                aktInt.AKTIntKosten = Convert.ToDouble(txtKosten.Text);
                aktInt.AKTIntMemo = txtMemo.Text;
                aktInt.AKTIntKSVEMail = txtKsvEmail.Text;

                aktInt.AKTIntDruckkennz = chkShowPrinted.Checked ? 1 : 0;
                aktInt.AKTIntVormerk = chkWiedervorlage.Checked ? 1 : 0;

                aktInt.AktIntAktType = Convert.ToInt32(ddlAktType.SelectedValue);
                aktInt.AktIntSB = Convert.ToInt32(ddlSB.SelectedValue);
                aktInt.AktIntStatus = Convert.ToInt32(ddlSatzStatus.SelectedValue);

                var aktTmp = new tblAktenInt();
                aktTmp.Assign(aktInt);
                RecordSet.Update(aktTmp);

                //SaveWorkFlow();
                LoadRecords();
                SetValues();
            }
            catch( Exception ex)
            {
                ctlMessage.ShowException(ex);
                ok = false;
            }
            return ok;
        }

        private void SetDistance()
        {
            var aktUtils = new AktUtils(aktId);
            
            var user = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserId = " + ddlSB.SelectedValue, typeof(tblUser));
            var distance = (int)aktUtils.InkAktUtils.GetDistance(user, aktInt.GegnerLastZip);
            txtWeg.Text = distance.ToString();
            SetWeggebuher();
        }

        private void SetWeggebuher()
        {
            /*
            try
            {
                int km = Convert.ToInt32(txtWeg.Text);
                if (km > 0)
                {
                    tblWege wege = (tblWege)HTBUtils.GetSqlSingleRecord("SELECT * FROM dbo.tblWege where WegVon <= " + km + " and WegBis >= " + km, typeof(tblWege));
                    if (wege != null)
                    {
                        txtCash.Text = wege.Preis.ToString("N2");
                    }
                }
            }
            catch { }
             */
            txtCash.Text = "0";
            int ag = -1;
            try
            {
                ag = Convert.ToInt32(txtAGN.Text);
            }
            catch { ag = -1; }

            if (ag >= 0)
            {
                if (ag != 32)
                {
//                    txtCash.Text = "28"; // just default it for now...
                    txtCash.Text = "0"; // just default it for now...
                }
            }
        }

        private string GetClientTextSection()
        {
            var sb = new StringBuilder();
            AddKlientLine(sb, aktInt.KlientName1);
            AddKlientLine(sb, aktInt.KlientName2);
            AddKlientLine(sb, aktInt.KlientName3);
            AddKlientLine(sb, aktInt.KlientStrasse);
            AddKlientLine(sb, aktInt.KlientLKZ + " " + aktInt.KlientPLZ + " " + aktInt.KlientOrt);
            if (sb.ToString().Trim().Length <= 0)
            {
                sb.Append("&nbsp;");
            }
            return sb.ToString();
        }

        private string GetClientTextSection(tblKlient klient)
        {
            StringBuilder sb = new StringBuilder();
            AddKlientLine(sb, klient.KlientName1);
            AddKlientLine(sb, klient.KlientName2);
            AddKlientLine(sb, klient.KlientName3);
            AddKlientLine(sb, klient.KlientStrasse);
            AddKlientLine(sb, klient.KlientLKZ + " " + klient.KlientPLZ + " " + klient.KlientOrt);
            if (sb.ToString().Trim().Length <= 0)
            {
                sb.Append("&nbsp;");
            }
            return sb.ToString();
        }

        private void AddKlientLine(StringBuilder sb, string line)
        {

            if (line != null && line.Trim().Length > 0)
            {
                sb.Append(line);
                sb.Append("<br/>");
            }
        }

        public string GetEmailLink()
        {
            return "SendMitteilung.aspx?ID=" +aktInt.AktIntID;
        }

        public void ShowParentScreen()
        {
            Response.Redirect("../../intranet/aktenint/aktenint.asp?" + Session["var"]);
        }
        
        #region Invoices
        private ArrayList GetInvoicesList()
        {
            if (aktInt.IsInkasso())
            {
                //tdNewBuchung.Visible = false;
                return HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceStatus <> -1 AND InvoiceCustInkAktId = " + aktInt.AktIntCustInkAktID + " ORDER BY InvoiceDate", typeof(tblCustInkAktInvoice));
            }
            else
            {
                tdNewBuchung.Visible = true;
                ArrayList posList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntPos WHERE AktIntPosAkt = " + aktInt.AktIntID, typeof(tblAktenIntPos));
                ArrayList list = new ArrayList();
                foreach (tblAktenIntPos pos in posList)
                {
                    tblCustInkAktInvoice inv = new tblCustInkAktInvoice();
                    inv.InvoiceID = -1;
                    inv.InvoiceCustInkAktId = pos.AktIntPosID;
                    inv.InvoiceDate = pos.AktIntPosDatum;
                    inv.InvoiceDescription = pos.AktIntPosCaption;
                    inv.InvoiceAmount = pos.AktIntPosBetrag;
                    inv.InvoiceComment = pos.AktIntPosNr;
                    inv.InvoiceBalance = pos.AktIntPosBetrag;
                    inv.InvoiceDueDate = pos.AktIntPosDueDate;
                    list.Add(inv);
                }
                return list;
            }
        }
        private void PopulateInvoicesGrid()
        {
            ArrayList invList = GetInvoicesList();
            totalAmountDue = 0;
            DataTable dt = GetInvoicesDataTableStructure();
            foreach (tblCustInkAktInvoice inv in invList)
            {
                if (inv.IsPayment())
                {
                    inv.InvoiceAmount *= -1;
                    inv.InvoiceBalance *= -1;
                }
                totalAmountDue += inv.InvoiceBalance;
                DataRow dr = dt.NewRow();

                dr["DeleteUrl"] = "../../intranet/images/delete2hover.gif";
                dr["EditUrl"] = "../../intranet/images/edit.gif";
                dr["InvoiceID"] = inv.InvoiceID;
                dr["InvoiceDate"] = inv.InvoiceDate.ToShortDateString();
                dr["InvoiceDescription"] = inv.InvoiceDescription;
                dr["InvoiceAmount"] = HTBUtils.FormatCurrency(inv.InvoiceAmount);
                dr["DueDate"] = inv.InvoiceDueDate.ToShortDateString();
                if (inv.InvoiceID >= 0)
                {
                    dr["DeletePopupUrl"] = "../global_forms/GlobalDelete.aspx?titel=Position%20löschen&amp;frage=Sie%20sind%20dabei%20diese%20Position%20zu%20löschen:&amp;strTable=tblCustInkAktInvoice&amp;strTextField=InvoiceDescription&amp;strColumn=InvoiceID&amp;ID=" + inv.InvoiceID;
                    dr["EditPopupUrl"] = "../aktenink/EditInvoice.aspx?ID=" + inv.InvoiceID;
                }
                else
                {
                    dr["PosInvoiceID"] = inv.InvoiceComment;
                    dr["DeletePopupUrl"] = "../../intranet/global_forms/globaldelete.asp?titel=Position%20entfernen&amp;frage=Sie%20sind%20dabei%20diese%20Position%20zu%20entfernen:&amp;strTable=tblAktenIntPos&amp;strTextField=AktIntPosCaption&amp;strColumn=AktIntPosID&amp;ID=" + inv.InvoiceCustInkAktId;
                    dr["EditPopupUrl"] = "/v2/intranetx/aktenint/EditBooking.aspx?" + GlobalHtmlParams.INTERVENTION_AKT + "=" + aktInt.AktIntID + "&" + GlobalHtmlParams.ID + "=" + inv.InvoiceCustInkAktId;
                }
                dt.Rows.Add(dr);
            }
            // show / hide the right Invoice Number
            if (aktInt.IsInkasso())
            {
                gvInvoices.Columns[3].Visible = false;
                tdNewBuchung.Visible = false;
            }
            else
            {
                tdNewBuchung.Visible = true;
                gvInvoices.Columns[2].Visible = false;
            }
            gvInvoices.DataSource = dt;
            gvInvoices.DataBind();
        }
        private DataTable GetInvoicesDataTableStructure()
        {
            DataTable dt = new DataTable();
            
            dt.Columns.Add(new DataColumn("DeleteUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("EditUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("DeletePopupUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("EditPopupUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceID", typeof(int)));
            dt.Columns.Add(new DataColumn("PosInvoiceID", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceDescription", typeof(string)));
            dt.Columns.Add(new DataColumn("AppliedAmount", typeof(double)));
            dt.Columns.Add(new DataColumn("DueDate", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceAmount", typeof(string)));

            return dt;
        }
        public double GetTotalDue()
        {
            return totalAmountDue;
        }
        #endregion

        #region Documents
        private void PopulateDocumentsGrid()
        {
            DataTable dt = GetDocumentsDataTableStructure();
            foreach (qryDoksIntAkten doc in docsList)
            {
                DataRow dr = dt.NewRow();

                dr["DeleteUrl"] = "../../intranet/images/delete2hover.gif";
                dr["DeletePopupUrl"] = "../global_forms/GlobalDelete.aspx?titel=Position%20löschen&amp;frage=Sie%20sind%20dabei%20diese%20Position%20zu%20löschen:&amp;strTable=tblDokument&amp;strTextField=DokAttachment&amp;strColumn=DokID&amp;ID=" + doc.DokID;

                dr["DokCreationTimeStamp"] = doc.DokCreationTimeStamp + " von " + doc.UserVorname + " " + doc.UserNachname;
                dr["DokTypeCaption"] = doc.DokTypeCaption;
                dr["DokCaption"] = doc.DokCaption;
                dr["DokAttachment"] = doc.DokAttachment;
                dt.Rows.Add(dr);
            }
            gvDocs.DataSource = dt;
            gvDocs.DataBind();
        }
        private DataTable GetDocumentsDataTableStructure()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("DeleteUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("DeletePopupUrl", typeof(string)));
            
            dt.Columns.Add(new DataColumn("DokCreationTimeStamp", typeof(string)));
            dt.Columns.Add(new DataColumn("DokTypeCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("DokCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("DokAttachment", typeof(string)));
            return dt;
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