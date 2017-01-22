using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using HTB.Database.HTB.Views;
using HTBUtilities;
using System.Collections;
using System.Data;
using HTBAktLayer;
using HTB.v2.intranetx.util;
using HTB.Database.Views;
using HTB.v2.intranetx.permissions;
using System.Text;

namespace HTB.v2.intranetx.aktenint
{
    public partial class WorkAktInt : System.Web.UI.Page
    {
        public qryAktenInt aktInt = new qryAktenInt();
        public int aktId = 0;
        public ArrayList posList = new ArrayList();
        public ArrayList actionsList = new ArrayList();
        private ArrayList _docsList = new ArrayList();
        public double htotal = 0;
        public double ptotal = 0;
        public double btotal = 0;

        public PermissionsWorkAktInt permissions = new PermissionsWorkAktInt();

        protected void Page_Load(object sender, EventArgs e)
        {
            permissions.LoadPermissions(GlobalUtilArea.GetUserId(Session));
            GlobalUtilArea.ValidateSession(Session, Response);
            if (Request["ID"] != null && !Request["ID"].Equals(""))
            {
                aktId = Convert.ToInt32(Request["ID"]);
                if (!IsPostBack)
                {
                    LoadRecords();
                    if (aktInt != null)
                    {
                        SetHonorarAndProvision();
                        SetValues();
                        if (aktInt.IsInkasso())
                        {
                            PopulateInstallmentsGrid();
                            pnlInkassoRate.Visible = true;
                            pnlInterventionRate.Visible = false;
                        }
                    }
                }
            }
        }

        public double GetTotalDue()
        {
            double ret = 0;
            foreach (HTB.Database.tblAktenIntPos AktIntPos in posList)
            {
                ret += AktIntPos.AktIntPosBetrag;
            }
            ret += aktInt.AKTIntZinsenBetrag;
            ret += aktInt.AKTIntKosten;
            return ret;
        }

        public string GetInstallmentLink()
        {
            if (aktInt.IsInkasso())
            {
                return "../aktenink/ActionsRat.aspx?ID=" + aktInt.AktIntCustInkAktID + "&INTID=" + aktId;
            }
            return "../../intranet/aktenint/newrv.asp?Summe=" + GetTotalDue() + aktInt.AktIntWeggebuehr + "&ID=" + aktId;
        }

        private void SetValues()
        {
            lblAktIntAZ.Text = aktInt.AktIntAZ;
            lblAuftraggeberName1.Text = aktInt.AuftraggeberName1;
            lblAuftraggeberName2.Text = aktInt.AuftraggeberName2;
            lblKlientName1.Text = aktInt.KlientName1;
            lblKlientName2.Text = aktInt.KlientName2;
            lblGegnerName1.Text = aktInt.GegnerName1;
            lblGegnerName2.Text = aktInt.GegnerLastName2;
            lblGegnerLastStrasse.Text =  aktInt.GegnerLastStrasse;
            lblGegnerLastZipPrefix.Text = aktInt.GegnerLastZipPrefix;
            lblGegnerLastZip.Text = aktInt.GegnerLastZip;
            lblGegnerLastOrt.Text = aktInt.GegnerLastOrt;
            lblInkassant.Text = aktInt.UserVorname + "&nbsp;" + aktInt.UserNachname;

            if (aktInt.IsInkasso())
            {
                SetKostenBasedOnInkassoAkt();
            }
            else
            {
                lblAKTIntZinsenBetrag.Text = HTBUtils.FormatCurrencyNumber(aktInt.AKTIntZinsenBetrag);
                lblAKTIntKosten.Text = HTBUtils.FormatCurrencyNumber(aktInt.AKTIntKosten);
                lblTotal.Text = HTBUtils.FormatCurrencyNumber(GetTotalDue());
                lblAktIntWeggebuehr.Text = HTBUtils.FormatCurrencyNumber(aktInt.AktIntWeggebuehr);
                lblTotalWithtWeggebuehr.Text = HTBUtils.FormatCurrencyNumber(GetTotalDue() + aktInt.AktIntWeggebuehr);
            }
            lblHTotal.Text = HTBUtils.FormatCurrency(htotal);
            lblPTotal.Text = HTBUtils.FormatCurrency(ptotal);
            lblBTotal.Text = HTBUtils.FormatCurrency(btotal);
            
            if (!aktInt.AKTIntRVStartDate.ToShortDateString().Equals(HTBUtils.DefaultShortDate))
            {
                pnlInterventionRate.Visible = true;

                if (aktInt.AKTIntRVInkassoType == 0)
                    lblCollectionType.Text = "Erlagschein";
                else if (aktInt.AKTIntRVInkassoType == 1)
                    lblCollectionType.Text = "Persönliches CollectionInvoice";
                else
                    lblCollectionType.Text = "&nbsp;";

                lblAktIntRVAmmount.Text = aktInt.AKTIntRVAmmount.ToString();
                lblAktIntRVStartDate.Text = aktInt.AKTIntRVStartDate.ToShortDateString();
                lblAktIntRVNoMonth.Text = aktInt.AKTIntRVNoMonth.ToString();
                lblAktIntRVIntervallDay.Text = aktInt.AKTIntRVIntervallDay.ToString();
                if (!GlobalUtilArea.GetEmptyIfNull(aktInt.AKTIntRVInfo).Equals(""))
                {
                    lblZahlungsrhythmus.Text = "Zahlungsrhythmus: " + aktInt.AKTIntRVInfo;
                }
            }
            else if (!aktInt.IsInkasso() || gvRates.Rows.Count <= 0) {
                //pnlBtnNewInstallment.Visible = true;
                if (aktInt.AktTypeINTCode == 2 || aktInt.AktTypeINTCode == 10)
                {
                    pnlBtnProtocol.Visible = true;
                }
            }

            if (aktInt.AktIntStatus >= 0 && aktInt.AktIntStatus <= 3)
            {
                trAktIntStatus.Visible = true;
                ddlNewStatus.Items.Clear();
                
                if (aktInt.AktIntStatus == 0)
                    ddlNewStatus.Items.Add(new ListItem("0 - Neu Erfasst", "0"));

                ddlNewStatus.Items.Add(new ListItem("1 - In Bearbeitung", "1"));
                ddlNewStatus.Items.Add(new ListItem("2 - Abgegeben", "2"));
                if (Session["MM_RoleIntAkt"] != null && Session["MM_RoleIntAkt"].ToString().Equals("1"))
                    ddlNewStatus.Items.Add(new ListItem("3 - Fertig", "3"));
                try
                {
                    ddlNewStatus.SelectedValue = aktInt.AktIntStatus.ToString();
                }
                catch { }
            }
            lblOriginalMemo.Text = aktInt.AktIntOriginalMemo;
            txtMemo.Text = aktInt.AKTIntMemo;

            if (GlobalUtilArea.GetEmptyIfNull(Session["MM_UserID"]) == "99" || GlobalUtilArea.GetEmptyIfNull(Session["MM_UserID"]) == "378" || GlobalUtilArea.GetEmptyIfNull(Session["MM_UserID"]) == "488") {
                ddlUserEdit.SelectedValue = aktInt.AktIntUserEdit.Trim();
                ddlUserEdit.Visible = true;
            }

            if (permissions.GrantInkasso && aktInt.IsInkasso())
            {
                var sb = new StringBuilder();
                sb.Append("<a href=\"");
                sb.Append("../aktenink/EditAktInk.aspx?");
                sb.Append(GlobalHtmlParams.ID);
                sb.Append("=");
                sb.Append(aktInt.AktIntCustInkAktID);
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL, GlobalHtmlParams.BACK);
                sb.Append("\">");
                sb.Append(aktInt.AktIntCustInkAktID);
                sb.Append("</a>");
                lblInkassoAktNumber.Text = sb.ToString();
            }
            else
            {
                trInkassoNumber.Visible = false;
            }
            PopulateInvoicesGrid();
            PopulateDocumentsGrid();
        }

        private void SetKostenBasedOnInkassoAkt()
        {
            tblCustInkAkt inkAkt = HTBUtils.GetInkassoAkt(aktInt.AktIntCustInkAktID);
            if (inkAkt != null)
            {
                var aktUtils = new AktUtils(aktInt.AktIntCustInkAktID);
                
                double zahlungen = aktUtils.GetAktTotalPayments();
                
                lblAKTIntZinsenBetrag.Text = aktUtils.GetAktTotalInterest().ToString("N2");
                lblAKTIntKosten.Text = aktUtils.GetAktTotalCollectionNettoAmount().ToString("N2");
                lblTax.Text = aktUtils.GetAktTotalTax().ToString("N2");
                lblZahlung.Text = (zahlungen == 0 ? "" : "-" + zahlungen.ToString("N2"));
                lblTotal.Text = aktUtils.GetAktBalance().ToString("N2");
                
            }
        }

        private void LoadRecords()
        {
            aktInt = (qryAktenInt)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenInt WHERE AktIntID = " + aktId, typeof(qryAktenInt));
            posList = HTBUtils.GetSqlRecords("SELECT * FROM qryAktenIntPos WHERE AktIntPosAkt = " + aktId, typeof(qryAktenIntPos));
            actionsList = HTBUtils.GetSqlRecords("SELECT * FROM qryInktAktAction WHERE AktIntActionAkt = " + aktId +" ORDER BY AktIntActionDate DESC", typeof(qryInktAktAction));
            _docsList = HTBUtils.GetSqlRecords("SELECT * FROM qryDoksIntAkten WHERE AktIntID = " + aktId, typeof(qryDoksIntAkten));
            if (aktInt.IsInkasso())
                HTBUtils.AddListToList(HTBUtils.GetSqlRecords("SELECT * FROM qryDoksInkAkten WHERE CustInkAktID = " + aktInt.AktIntCustInkAktID, typeof(qryDoksInkAkten)), _docsList);

            if (aktInt == null)
            {
                aktInt = new qryAktenInt();
            }
        }

        private void SetHonorarAndProvision()
        {
            htotal = 0;
            ptotal = 0;
            btotal = 0;
            foreach (qryInktAktAction action in actionsList)
            {
                htotal += action.AktIntActionHonorar;
                ptotal += action.AktIntActionProvision;
                btotal += action.AktIntActionBetrag;
            }
        }

        private void PopulateInstallmentsGrid()
        {
            ArrayList installmentList = HTBUtils.GetSqlRecords("SELECT * FROM qryCustInkRate WHERE CustInkAktRateAktId = " + aktInt.AktIntCustInkAktID + " ORDER BY CustInkAktRateDueDate", typeof(qryCustInkRate));
            DataTable dt = GetGridDataTableStructure();
            lblPayType.Text = "Erlagschein";
            foreach (tblCustInkAktRate installment in installmentList)
            {
                DataRow dr = dt.NewRow();
                dr["InstallmentDate"] = installment.CustInkAktRateDueDate.ToShortDateString();
                dr["InstallmentAmount"] = HTBUtils.FormatCurrency(installment.CustInkAktRateAmount);
                dt.Rows.Add(dr);
                if (installment.CustInkAktRatePaymentType == 1)
                    lblPayType.Text = "Pers&ouml;nliches CollectionInvoice";
                
            }
            gvRates.DataSource = dt;
            gvRates.DataBind();
        }

        private DataTable GetGridDataTableStructure()
        {
            DataTable dt = new DataTable();
            DataColumn dc;

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "InstallmentDate";
            dt.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "InstallmentAmount";
            dt.Columns.Add(dc);

            return dt;
        }

        public tblAktenInt GetAkt()
        {
            return aktInt;
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
                var list = new ArrayList();
                foreach (qryAktenIntPos pos in posList)
                {
                    var inv = new tblCustInkAktInvoice();
                    inv.InvoiceID = -1;
                    inv.InvoiceCustInkAktId = pos.AktIntPosID;
                    inv.InvoiceDate = pos.AktIntPosDatum;
                    inv.InvoiceDescription = pos.AktIntPosCaption;
                    inv.InvoiceAmount = pos.AktIntPosBetrag;
                    inv.InvoiceComment = pos.AktIntPosNr;
                    inv.InvoiceBalance = pos.AktIntPosBetrag;
                    inv.InvoiceDueDate = pos.AktIntPosDueDate;
                    inv.InvoiceBillNumber = pos.AktIntPosTypeCaption;
                    list.Add(inv);
                }
                return list;
            }
        }
        private void PopulateInvoicesGrid()
        {
            ArrayList invList = GetInvoicesList();
            DataTable dt = GetInvoicesDataTableStructure();
            foreach (tblCustInkAktInvoice inv in invList)
            {
                if (inv.IsPayment())
                {
                    inv.InvoiceAmount *= -1;
                    inv.InvoiceBalance *= -1;
                }
                DataRow dr = dt.NewRow();

                dr["DeleteUrl"] = "../../intranet/images/delete2hover.gif";
                dr["EditUrl"] = "../../intranet/images/edit.gif";
                dr["InvoiceID"] = inv.InvoiceID;
                dr["InvoiceDate"] = inv.InvoiceDate.ToShortDateString();
                dr["InvoiceDescription"] = inv.InvoiceDescription;
                dr["InvoiceAmount"] = HTBUtils.FormatCurrency(inv.InvoiceAmount);
                dr["DueDate"] = inv.InvoiceDueDate.ToShortDateString();
                dr["PosInvoiceTypeCaption"] = inv.InvoiceBillNumber;
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
            var dt = new DataTable();

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
            dt.Columns.Add(new DataColumn("PosInvoiceTypeCaption", typeof(string)));

            return dt;
        }
        #endregion

        #region Documents
        private void PopulateDocumentsGrid()
        {
            DataTable dt = GetDocumentsDataTableStructure();
            foreach (Record rec in _docsList)
            {

                if (rec is qryDoksInkAkten)
                {
                    DataRow dr = dt.NewRow();
                    var doc = (qryDoksInkAkten)rec;
                    
                    dr["DeleteUrl"] = "";

                    dr["DokAttachment"] = GlobalUtilArea.GetDocAttachmentLink(doc.DokAttachment);

                    dr["DokChangeDate"] = doc.DokChangeDate.ToShortDateString();
                    dr["DokTypeCaption"] = doc.DokTypeCaption;
                    dr["DokCaption"] = doc.DokCaption;
                    dr["DokUser"] = doc.UserVorname + " " + doc.UserNachname;

                    dt.Rows.Add(dr);
                }
                else if (rec is qryDoksIntAkten)
                {
                    DataRow dr = dt.NewRow();
                    var doc = (qryDoksIntAkten)rec;
                    var sb = new StringBuilder("<a href=\"javascript:void(window.open('");

                    sb.Append("/v2/intranet/global_forms/globaldelete.asp?strTable=tblDokument&frage=Sind%20Sie%20sicher,%20dass%20sie%20dieses%20Dokument%20l&#246;schen%20wollen?&strTextField=DokAttachment&strColumn=DokID&ID=");
                    sb.Append(doc.DokID);

                    // continue with the popup params
                    sb.Append("','_blank','toolbar=no,menubar=no'))\">");
                    sb.Append("<img src=\"../../intranet/images/delete2hover.gif\" width=\"16\" height=\"16\" alt=\"L&ouml;scht diesen Datensatz.\" style=\"border-color:White;border-width:0px;\"/>");
                    sb.Append("</a>");

                    dr["DeleteUrl"] = sb.ToString();

                    dr["DokAttachment"] = GlobalUtilArea.GetDocAttachmentLink(doc.DokAttachment);
                    dr["DokChangeDate"] = doc.DokCreationTimeStamp.ToShortDateString();
                    dr["DokTypeCaption"] = doc.DokTypeCaption;
                    dr["DokCaption"] = doc.DokCaption;
                    dr["DokUser"] = doc.UserVorname + " " + doc.UserNachname;

                    dt.Rows.Add(dr);
                }
            }
            gvDocs.DataSource = dt;
            gvDocs.DataBind();
        }
        private DataTable GetDocumentsDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("DeleteUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("DeletePopupUrl", typeof(string)));

            dt.Columns.Add(new DataColumn("DokChangeDate", typeof(string)));
            dt.Columns.Add(new DataColumn("DokTypeCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("DokCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("DokUser", typeof(string)));
            dt.Columns.Add(new DataColumn("DokAttachment", typeof(string)));
            return dt;
        }
        private int GetDocMahnungNumber(string docCaption)
        {
            if (docCaption.IndexOf("Mahnung") >= 0)
            {
                try
                {
                    int mahnungsNum = Convert.ToInt32(docCaption.Substring(8));
                    return mahnungsNum;
                }
                catch
                {
                }
            }
            return 0;
        }
        #endregion

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                bool ok = true;
                tblAktenInt rec = HTBUtils.GetInterventionAkt(aktId);
                if (trAktIntStatus.Visible)
                {
                    rec.AktIntStatus = Convert.ToInt16(ddlNewStatus.SelectedValue);
                }
                if (ddlUserEdit.Visible)
                {
                    rec.AktIntUserEdit = ddlUserEdit.SelectedValue;
                }
                rec.AKTIntMemo = txtMemo.Text.Replace("'", "");
                try
                {
                    RecordSet.Update(rec);
                }
                catch(Exception updateException)
                {
                    ctlMessage.ShowException(updateException);
                    ok = false;
                }
                if (ok)
                {
                    if (GlobalUtilArea.IsPopUp(Request))
                        ScriptManager.RegisterStartupScript(this.updPanel1, typeof (string), "closeScript", "window.close();", true);
                    else if (GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT]) == GlobalHtmlParams.BACK)
                        ScriptManager.RegisterStartupScript(this.updPanel1, typeof (string), "backScript", "javascript:history.go(-1);", true);
                    else
                        Response.Redirect("../../intranet/aktenint/aktenint.asp?" + Session["var"]);
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if(GlobalUtilArea.IsPopUp(Request))
                ScriptManager.RegisterStartupScript(this.updPanel1, typeof(string), "closeScript", "window.close();", true);
            else
                ScriptManager.RegisterStartupScript(this.updPanel1, typeof(string), "backScript", "javascript:history.go(-1)", true);
        }
        #endregion

    }
}