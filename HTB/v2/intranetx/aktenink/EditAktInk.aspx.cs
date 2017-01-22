using System;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using HTB.Database;
using System.Text;
using System.Collections;
using HTB.v2.intranetx.global_files;
using HTBExtras.KingBill;
using HTBReports;
using HTBUtilities;
using System.Data;
using HTB.v2.intranetx.util;
using HTB.v2.intranetx.permissions;
using HTBDailyKosten;
using HTB.Database.Views;
using HTBExtras;
using HTB.Database.LookupRecords;

namespace HTB.v2.intranetx.aktenink
{
    public partial class EditAktInk : Page, IWorkflow
    {
        public string AktId;
        public qryCustAktEdit QryInkassoakt = new qryCustAktEdit();
        
        private ArrayList _intAktList = new ArrayList();
        private ArrayList _meldeResults;
        private ArrayList _invoiceList = new ArrayList();
        private ArrayList _installmentsList = new ArrayList();
        private ArrayList _inkassoAktions = new ArrayList();
        private readonly ArrayList _interventionAktions = new ArrayList();
        public readonly ArrayList AktionsList = new ArrayList();

        private ArrayList _paymentsAppliedToCollectionInvoicesList = new ArrayList();
        

        private ArrayList _docsList = new ArrayList();

        private double _initialAmount;
        private double _initialAmountPay;

        private double _initialClientCosts;
        private double _initialClientCostsPay;

        private double _interest;
        private double _interestPay;
        private double _collectionCosts;
        private double _collectionCostsPay;
        private double _collectionInterest;
        private double _collectionInterestPay;
        private double _balanceDue;
        private double _totalPay;
        private double _totalAmountDue;
        private double _unappliedPay;

        public PermissionsEditAktInk Permissions = new PermissionsEditAktInk();

        protected void Page_Load(object sender, EventArgs e)
        {
            Permissions.LoadPermissions(GlobalUtilArea.GetUserId(Session));
            
            lnkInterestDetail.Visible = true;
            lnkInterestSummary.Visible = false;
                
            if (Request.QueryString["ID"] != null && !Request.QueryString["ID"].Trim().Equals(""))
            {
                AktId = Request.QueryString["ID"];

                LoadRecords();
                CombineActions();
                LoadInstallmentsList();
                PopulateInstallmentsGrid();
                SetTotalInvoiceAmounts();
                if (!IsPostBack)
                {
                    GlobalUtilArea.LoadDropdownList(ddlLawyer,
                       "SELECT * FROM tblLawyer ORDER BY LawyerName1 ASC",
                       typeof(tblLawyer),
                       "LawyerID",
                       "LawyerName1", false);

                    GlobalUtilArea.LoadDropdownList(ddlClientSB,
                                                    "SELECT UserId, UserVorname + ' ' + UserNachname [UserName] FROM tblUser WHERE UserKlient = " + QryInkassoakt.KlientID + " ORDER BY UserVorname",
                                                    typeof (UserLookup),
                                                    "UserId",
                                                    "UserName",
                                                    true
                        );
                    SetValues();
                }
                Session["tmpInkAktID"] = Request.QueryString["ID"];
            }
            ctlWorkflow.SetWftInterface(this);
            ctlWorkflow.SetBtnUpdateStatusVisible(true);
            ctlWorkflow.SetDateDescription("Vereinbarter Zahlungstermin");
            ddlLawyer.Enabled = !HTBUtils.IsAktSentToLawyer(QryInkassoakt.CustInkAktID);
            if (!Permissions.GrantInkassoEdit)
            {
                ddlSendBericht.Enabled = false;
                tdShowDelete.Visible = false;
            }

        }

        private void LoadRecords()
        {
            var sw = new Stopwatch();
            sw.Start();
            _interventionAktions.Clear();

            ArrayList[] results = HTBUtils.GetMultipleListsFromStoredProcedure("spGetInkassoAktData", new ArrayList
                                                                                                 {
                                                                                                     new StoredProcedureParameter("aktId", SqlDbType.Int, GlobalUtilArea.GetZeroIfConvertToIntError(Request.QueryString["ID"]))
                                                                                                 },
                                                                                                 new[]
                                                                                                 {
                                                                                                     typeof(qryCustAktEdit), 
                                                                                                     typeof(tblCustInkAktInvoice), 
                                                                                                     typeof(qryCustInkAktAktionen), 
                                                                                                     typeof(qryDoksInkAkten), 
                                                                                                     typeof(tblAktenInt), 
                                                                                                     typeof(qryMeldeResult),
                                                                                                     typeof(InvoiceIdAndAmountRecord),
                                                                                                     typeof(qryCustInkRate)
                                                                                                 }
                                                                                                 );
            QryInkassoakt = (qryCustAktEdit)results[0][0];
            _invoiceList = results[1];
            _inkassoAktions = results[2];
            _docsList = results[3];
            _intAktList = results[4];
            _meldeResults = results[5];
            _paymentsAppliedToCollectionInvoicesList = results[6];
            _installmentsList = results[7];

            if (_intAktList != null && _intAktList.Count > 0)
                foreach (tblAktenInt intAkt in _intAktList)
                {
                    HTBUtils.AddListToList(HTBUtils.GetSqlRecords("SELECT * FROM qryInktAktAction WHERE AktIntActionAkt = " + intAkt.AktIntID + " ORDER BY AktIntActionDate DESC", typeof (qryInktAktAction)), _interventionAktions);
                    if(intAkt.AktIntStatus < 2)
                    {
                        var sb = new StringBuilder("<strong><font color=\"red\">ACHTUNG: Akt ist beim Aussendienst!!!<br/>Status: ");
                        sb.Append(GlobalUtilArea.GetInterventionAktStatusText(intAkt.AktIntStatus));
                        if (intAkt.AktIntStatus == 1 && intAkt.AKTIntDruckkennz == 1)
                            sb.Append(" [gedruckt]");
                        sb.Append("</font></strong>");
                        
                        _interventionAktions.Add(new qryInktAktAction
                                                     {
                                                         AktIntActionAkt = intAkt.AktIntID,
                                                         AktIntActionTime = DateTime.Now,
                                                         AktIntActionTypeCaption = sb.ToString(),
                                                         AktIntActionLatitude = 0,
                                                         AktIntActionLongitude = 0,
                                                         AktIntActionAddress = ""
                                                     });
                    }
                    HTBUtils.AddListToList(HTBUtils.GetSqlRecords("SELECT * FROM qryDoksIntAkten WHERE AktIntID = " + intAkt.AktIntID, typeof(qryDoksIntAkten)), _docsList);
                }
            //QryInkassoakt.CustInkAktGothiaNr = (int)sw.ElapsedMilliseconds;

        }
        private void SetValues()
        {
            lblCustInkAktKunde.Text = !QryInkassoakt.CustInkAktKunde.Trim().Equals("") ? QryInkassoakt.CustInkAktKunde : "Keine Rechnungsnummer vorhanden!";
            lblCustInkAktGothiaNr.Text = !string.IsNullOrEmpty(QryInkassoakt.CustInkAktGothiaNr) ? QryInkassoakt.CustInkAktGothiaNr : "&nbsp;";

            lblGegner.Text = "<strong><a href=\"#\" onClick=\"MM_openBrWindow('../../intranet/gegner/editgegner.asp?poprefresh=true&GegnerID="+QryInkassoakt.CustInkAktGegner+"','popWindow','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=600')\">";

            lblGegner.Text += QryInkassoakt.GegnerLastName1 + ", " + QryInkassoakt.GegnerLastName2 + "</a></strong><br/>";
            lblGegner.Text += QryInkassoakt.GegnerLastStrasse + "<br/>";
            lblGegner.Text += QryInkassoakt.GegnerLastZipPrefix + "&nbsp;" + QryInkassoakt.GegnerLastZip + "&nbsp;" + QryInkassoakt.GegnerLastOrt;

            lblKlient.Text = "<strong><a href=\"#\" onClick=\"MM_openBrWindow('/v2/intranet/klienten/editklient.asp?poprefresh=true&ID=" + QryInkassoakt.CustInkAktKlient + "','popWindow','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=600')\">";
            
            lblKlient.Text += "<strong>"+QryInkassoakt.KlientName1.Trim() + "</strong>&nbsp;" + QryInkassoakt.KlientName2.Trim() + "</strong></a><br/>";
            lblKlient.Text += QryInkassoakt.KlientStrasse + "<br/>";
            lblKlient.Text += QryInkassoakt.KlientLKZ + "&nbsp;" + QryInkassoakt.KlientPLZ + "&nbsp;" + QryInkassoakt.KlientOrt;

            lblForderung.Text = HTBUtils.FormatCurrency(_initialAmount);
            lblZahlungen.Text = HTBUtils.FormatCurrency(_initialAmountPay);
            lblKostenKlientSumme.Text = HTBUtils.FormatCurrency(_initialClientCosts);
            lblKostenKlientZahlungen.Text = HTBUtils.FormatCurrency(_initialClientCostsPay);
            lblZinsen.Text = HTBUtils.FormatCurrency(_interest);
            lblZinsenZahlungen.Text = HTBUtils.FormatCurrency(_interestPay);

            lblEcpZinsen.Text = HTBUtils.FormatCurrency(_collectionInterest);
            lblEcpZinsenZahlungen.Text = HTBUtils.FormatCurrency(_collectionInterestPay);

            
            lblKostenSumme.Text = HTBUtils.FormatCurrency(_collectionCosts);
            lblKostenZahlungen.Text = HTBUtils.FormatCurrency(_collectionCostsPay);

            double totalAmount = (_initialAmount + _initialClientCosts + _interest + _collectionCosts + _collectionInterest);
            //totalPay = (initialAmountPay + initialClientCostsPay + interestPay + collectionCostsPay + collectionInterestPay);
            _balanceDue = totalAmount - _totalPay;
            lblSumGesFortBUE.Text = HTBUtils.FormatCurrency(totalAmount);
            lblSumGesZahlungenBUE.Text = HTBUtils.FormatCurrency(_totalPay);

            lblSaldo.Text = HTBUtils.FormatCurrency(_balanceDue);
            if (_unappliedPay > 0)
                lblUnappliedPay.Text = "Unapplied Zahlung: " + HTBUtils.FormatCurrency(_unappliedPay);
            else
                lblUnappliedPay.Text = "&nbsp;";

            txtMemo.Text = QryInkassoakt.CustInkAktMemo;

            GlobalUtilArea.SetSelectedValue(ddlLawyer, QryInkassoakt.CustInkAktLawyerId.ToString());
            GlobalUtilArea.SetSelectedValue(ddlClientSB, QryInkassoakt.CustInkAkKlientSB.ToString());
            ddlSendBericht.SelectedValue = QryInkassoakt.CustInkAktSendBericht ? "1" : "0";

            lblLawyerInfo.Text = GetLawyerInfo(QryInkassoakt);

            if (_intAktList.Count > 0)
            {
                var intAkt = (tblAktenInt) _intAktList[0];
                if (intAkt != null)
                {
                    var user = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserID = " + intAkt.AktIntSB, typeof (tblUser));
                    if(user != null)
                    {
                        lblAussendienst.Text = user.UserVorname + " " + user.UserNachname;
                        trAussendienst.Visible = true;
                    }
                    string intAktStatus = GlobalUtilArea.GetInterventionAktStatusText(intAkt.AktIntStatus);
                    if (intAkt.AktIntStatus == 1 && intAkt.AKTIntDruckkennz == 1)
                        intAktStatus += " [gedruckt]";
                    lblInterventionStatus.Text = intAktStatus;
                    trInterventionStatus.Visible = true;
                }
            }
            PopulateInvoicesGrid();
            PopulateDocumentsGrid();
//            ctlWorkflow.PopulateWorkFlow(QryInkassoakt.CustInkAktID);
        }
        
        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ctlWorkflow.ValidateWorkflow())
            {
                SaveAkt();
                ctlWorkflow.SaveWorkFlow(QryInkassoakt.CustInkAktID, false);
                LoadRecords();
                ctlWorkflow.PopulateWorkFlow(QryInkassoakt.CustInkAktID);
                ShowParentScreen();
            }
        }

        protected void btnShowDelete_Click(object sender, EventArgs e)
        {
            trDelete.Visible = true;
            ctlMessage.ShowSuccess("Um den Akt zu l&ouml;schen bitte nach unten scrollen und auf 'Akt endg&uuml;ltig l&ouml;schen' klicken.");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            HTBUtils.DeleteInkassoAkt(Convert.ToInt32(AktId));
            Response.Redirect("~/v2/intranet/aktenink/AktenStaff.asp");
        }

        protected void btnCancelDelete_Click(object sender, EventArgs e)
        {
            trDelete.Visible = false;
            ctlMessage.Clear();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalUtilArea.IsPopUp(Request))
                bdy.Attributes.Add("onLoad", "window.close()");

            else if (GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL]) == GlobalHtmlParams.BACK)
                bdy.Attributes.Add("onLoad", "javascript:history.go(-2)");

            else
                ShowParentScreen();
        }

        protected void btnDeleteInstallmentPlan_Click(object sender, EventArgs e)
        {
            HTBUtils.DeleteInstallmentPlan(Convert.ToInt32(AktId));
            LoadRecords();
            PopulateInstallmentsGrid();
            ctlMessage.ShowSuccess("Die Ratenvereinbarung wurde gel&ouml;scht!");
        }
        

        protected void ddlLawyer_SelectedindexChagned(object sender, EventArgs e)
        {
            lblLawyerInfo.Text = GetLawyerInfo((tblLawyer) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblLawyer WHERE LawyerID = " + ddlLawyer.SelectedValue, typeof (tblLawyer)));
        }

        protected void lnkInterestDetail_Click(object sender, EventArgs e)
        {
            PopulateInvoicesGrid(true);
            lnkInterestDetail.Visible = false;
            lnkInterestSummary.Visible = true;
        }
        protected void lnkInterestSummary_Click(object sender, EventArgs e)
        {
            PopulateInvoicesGrid();
            lnkInterestDetail.Visible = true;
            lnkInterestSummary.Visible = false;
        }
        
        #endregion

        private void ShowParentScreen()
        {
            Response.Redirect("../../intranet/aktenink/AktenStaff.asp?" + Session["var"]);
        }

        private void SetTotalInvoiceAmounts()
        {
            _unappliedPay = 0;
            foreach (tblCustInkAktInvoice inv in _invoiceList)
            {
                if (!inv.IsVoid())
                {
                    if (inv.IsPayment())
                    {
                        _totalPay += inv.InvoiceAmount;
                        _unappliedPay += inv.InvoiceBalance;
                    }
                    else
                    {
                        switch (inv.InvoiceType)
                        {
                            case tblCustInkAktInvoice.INVOICE_TYPE_ORIGINAL:
                                _initialAmount += inv.InvoiceAmount;
                                _initialAmountPay += inv.InvoiceAmount - inv.InvoiceBalance;
                                break;
                            case tblCustInkAktInvoice.INVOICE_TYPE_CLIENT_COST:
                                _initialClientCosts += inv.InvoiceAmount;
                                _initialClientCostsPay += inv.InvoiceAmount - inv.InvoiceBalance;
                                break;
                            case tblCustInkAktInvoice.INVOICE_TYPE_INTEREST_CLIENT:
                            case tblCustInkAktInvoice.INVOICE_TYPE_INTEREST_CLIENT_ORIGINAL:
                                _interest += inv.InvoiceAmount;
                                _interestPay += inv.InvoiceAmount - inv.InvoiceBalance;
                                break;
                            case tblCustInkAktInvoice.INVOICE_TYPE_INTEREST_COLECTION:
                                _collectionInterest += inv.InvoiceAmount;
                                _collectionInterestPay += inv.InvoiceAmount - inv.InvoiceBalance;
                                break;
                            default:
                                _collectionCosts += inv.InvoiceAmount;
                                _collectionCostsPay += inv.InvoiceAmount - inv.InvoiceBalance;
                                break;
                        }
                    }
                }
            }
        }

        private void CombineActions()
        {
            tblAktenInt intAkt = _intAktList.Count == 0 ? null : (tblAktenInt)_intAktList[0];
            foreach (qryCustInkAktAktionen inkAction in _inkassoAktions)
                AktionsList.Add(new InkassoActionRecord(inkAction, intAkt));

            foreach (qryInktAktAction intAction in _interventionAktions)
            {
                var action = new InkassoActionRecord(intAction, intAkt);
                if(!string.IsNullOrEmpty(intAction.AktIntActionAddress))
                {
                    action.ActionCaption += "<BR/><I>" + intAction.AktIntActionAddress+"</I>";
                }
                else if (action.AktIntActionLatitude > 0 && action.AktIntActionLongitude > 0)
                {
                    string address = GlobalUtilArea.GetAddressFromLatitudeAndLongitude(action.AktIntActionLatitude, action.AktIntActionLongitude);
                    if(!string.IsNullOrEmpty(address))
                    {
                        action.ActionCaption += "<BR/><I>" + address+"</I>";
                        // Update Action [set address based on coordinates]
                        try
                        {
                            var set = new RecordSet();
                            set.ExecuteNonQuery("UPDATE tblAktenIntAction SET AktIntActionAddress = " + address + " WHERE AktIntActionID = " + intAction.AktIntActionID);
                        }
                        catch
                        {
                        }
                    }
                }
                AktionsList.Add(action);
            }

            foreach (qryMeldeResult melde in _meldeResults)
                AktionsList.Add(new InkassoActionRecord(melde, intAkt));

            AktionsList.Sort(new InkassoActionRecordComparer());
        }
        
        private void SaveAkt()
        {
            if (Permissions.GrantInkassoEdit)
            {
                var akt = HTBUtils.GetInkassoAkt(Convert.ToInt32(AktId));
                akt.CustInkAktMemo = txtMemo.Text;
                akt.CustInkAktLawyerId = GlobalUtilArea.GetZeroIfConvertToIntError(ddlLawyer.SelectedValue);
                akt.CustInkAkKlientSB = GlobalUtilArea.GetZeroIfConvertToIntError(ddlClientSB.SelectedValue);
                akt.CustInkAktSendBericht = HTBUtils.GetBoolValue(ddlSendBericht.SelectedValue);
                RecordSet.Update(akt);
            }
        }

        #region Gets Methods
        public bool IsInkassoAction(Record rec)
        {
            return rec is qryCustInkAktAktionen;
        }
        public string GetActionType(Record rec)
        {
            if (rec is qryCustInkAktAktionen)
                return ((qryCustInkAktAktionen)rec).KZKZ;
            return "";
        }

        public string GetDeleteActionURL(Record rec)
        {
            if (rec is qryCustInkAktAktionen) {
                var action = (qryCustInkAktAktionen)rec;
                var sb = new StringBuilder("../global_forms/GlobalDelete.aspx?titel=Position%20löschen&amp;frage=Sie%20sind%20dabei%20diese%20Position%20zu%20löschen:&amp;strTable=tblCustInkAktAktion&amp;strTextField=");
                if (action.CustInkAktAktionCaption != null && action.CustInkAktAktionCaption.Trim() != string.Empty)
                    sb.Append("CustInkAktAktionCaption");
                else
                    sb.Append("CustInkAktAktionMemo");

                sb.Append("&amp;strColumn=CustInkAktAktionID&amp;ID=");
                sb.Append(action.CustInkAktAktionID);
                return sb.ToString();
            }
            return "";
        }

        public string GetEditActionURL(Record rec)
        {
            if (rec is qryCustInkAktAktionen)
            {
                return "EditInkAction.aspx?ID="+((qryCustInkAktAktionen)rec).CustInkAktAktionID;
            }
            return "";
        }

        public bool HasIntervention()
        {
            return _intAktList.Count > 0;
            
        }
        public string GetInterventionMemo()
        {
            var intAkt = GetInterventionAkt();
            if (intAkt != null)
            {
                var sb = new StringBuilder("<strong>&nbsp;&nbsp;&nbsp;&nbsp;Original Memo: </strong><br/>");
                sb.Append(intAkt.AktIntOriginalMemo.Replace(Environment.NewLine, "<BR/>"));
                sb.Append("<BR/><BR/><strong>&nbsp;&nbsp;&nbsp;&nbsp;Aussendienst Memo: </strong>");
                sb.Append(intAkt.AKTIntMemo.Replace(Environment.NewLine, "<BR/>"));
                return sb.ToString();
            }
            return "";
        }
        public string GetInverventionURL()
        {
            var intAkt = GetInterventionAkt();
            if (intAkt != null)
            {
                var sb = new StringBuilder("<strong>&nbsp;&nbsp;&nbsp;&nbsp;");
                sb.Append(intAkt.AktIntID.ToString());
                sb.Append(":&nbsp;&nbsp;</strong>");

                sb.Append("<a href=\"../aktenint/editaktint.aspx?");
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, intAkt.AktIntID.ToString(), false);
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT, GlobalHtmlParams.BACK);
                sb.Append("\">");
                sb.Append("<img src=\"/v2/intranet/images/edit.gif\" width=\"16\" height=\"16\" border=\"0\" title=\"&Auml;ndern.\">");
                sb.Append("</a>&nbsp;&nbsp;&nbsp;");

                sb.Append("<a href=\"../aktenint/workaktint.aspx?");
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, intAkt.AktIntID.ToString(), false);
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT, GlobalHtmlParams.BACK);
                sb.Append("\">");
                sb.Append("<img src=\"/v2/intranet/images/arrow16.gif\" width=\"16\" height=\"16\" border=\"0\" title=\"Buchen.\">");
                sb.Append("</a>&nbsp;&nbsp;&nbsp;");
                return sb.ToString();
                //return "<a href=\"../aktenint/workaktint.aspx?ID=" + intAkt.AktIntID + "&" + GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT + "=" + GlobalHtmlParams.BACK + "\">" + intAkt.AktIntID + "</a>";
            }
            return "";
        }
        private tblAktenInt GetInterventionAkt()
        {
            return _intAktList.Count == 0 ? null : (tblAktenInt)_intAktList[0];
        }

        public bool HasMelde()
        {
            return _meldeResults != null && _meldeResults.Count > 0;
        }
        public string GetMeldeResult()
        {
            var meldeAkt = GetMeldeAkt();
            if (meldeAkt != null)
                return meldeAkt.AMBericht.Replace(Environment.NewLine, "<BR/>");
            return "";
        }
        private qryMeldeResult GetMeldeAkt()
        {
            return _meldeResults.Count == 0 ? null : (qryMeldeResult)_meldeResults[0];
        }
        #endregion

        #region Invoices
        private ArrayList GetInvoicesList()
        {
            var ret = new ArrayList();
            foreach (tblCustInkAktInvoice inv in _invoiceList)
                if (inv.InvoiceStatus != -1)
                    ret.Add(inv);
            return ret;
//            return HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceStatus <> -1 AND InvoiceCustInkAktId = " + AktId + " ORDER BY InvoiceDate", typeof(tblCustInkAktInvoice));
        }
        private void PopulateInvoicesGrid(bool showDetailInterest = false)
        {
            ArrayList invList = GetInvoicesList();
            _totalAmountDue = 0;
            DataTable dt = GetInvoicesDataTableStructure();
            double totalInterest = 0;
            foreach (tblCustInkAktInvoice inv in invList)
            {
                double unappliedPay = 0;
                double orgInvAmount = inv.InvoiceAmount;
                if (!showDetailInterest && inv.IsInterest())
                {
                    totalInterest += inv.InvoiceAmount;
                    _totalAmountDue += inv.InvoiceBalance;
                }
                else
                {
                    if (inv.IsPayment())
                    {
                        unappliedPay = inv.InvoiceBalance;
                        inv.InvoiceAmount *= -1;
                        inv.InvoiceBalance *= -1;
                    }
                    _totalAmountDue += inv.InvoiceBalance;
                    DataRow dr = dt.NewRow();
                    string description = inv.InvoiceDescription;
                    if (inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_PAYMENT_CASH)
                        description += " [ Bar ] [ Beleg: " + inv.InvoiceBillNumber + " ]";
                    if (inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_PAYMENT_TRANSFER)
                        description += " [ &Uuml;berweisung ]";
                    if (inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_RETURNED)
                        description += " [ Returnware ]";
                    if (inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_PAYMENT_DIRECT_TO_CLIENT)
                        description += " [ Direktzahlung ]";
                    if (inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_CREDIT)
                        description += " [ Kosten Reduktion ]";

                    if (inv.InvoiceComment.Trim() != string.Empty)
                        description += "<br/>" + inv.InvoiceComment.Replace(Environment.NewLine, "<br/>");
                    
                    StringBuilder sb;
                    if (HTBUtils.IsZero(inv.InvoiceBalance - inv.InvoiceAmount)) // inv.InvoiceBalance == inv.InvoiceAmount
                    {
                        sb = new StringBuilder("<a href=\"javascript:void(window.open('");
                        // the actual link
                        sb.Append("../global_forms/GlobalDelete.aspx?titel=Position%20löschen&amp;frage=Sie%20sind%20dabei%20diese%20Position%20zu%20löschen:&amp;strTable=tblCustInkAktInvoice&amp;strTextField=InvoiceDescription&amp;strColumn=InvoiceID&amp;ID=");
                        sb.Append(inv.InvoiceID.ToString());
                        // continue with the popup params
                        sb.Append("','_blank','toolbar=no,menubar=no'))\">");
                        sb.Append("<img src=\"../../intranet/images/delete2hover.gif\" width=\"16\" height=\"16\" alt=\"L&ouml;scht diesen Datensatz.\" style=\"border-color:White;border-width:0px;\"/>");
                        sb.Append("</a>");
                        dr["DeleteUrl"] = sb.ToString();
                    }
                    else
                    {
                        dr["DeleteUrl"] = "<img src=\"../../intranet/images/delete2hover_dis.gif\" width=\"16\" height=\"16\" alt=\"L&ouml;scht diesen Datensatz.\" />";
                    }
                    
                    sb = new StringBuilder("<a href=\"javascript:void(window.open('");
                    sb.Append("EditInvoice.aspx?ID=");
                    sb.Append(inv.InvoiceID);
                    // continue with the popup params
                    sb.Append("','_blank','toolbar=no,menubar=no,width=800,height=800,top=10,screenY=10,scrollbars=yes'))\">");
                    sb.Append("<img src=\"../../intranet/images/edit.gif\" width=\"16\" height=\"16\" alt=\"&Auml;ndern diesen Datensatz.\" style=\"border-color:White;border-width:0px;\"/>");
                    sb.Append("</a>");
                    dr["EditUrl"] = sb.ToString();

                    dr["InvoiceID"] = inv.InvoiceID;
                    dr["InvoiceDescription"] = description;
                    dr["InvoiceAmount"] = HTBUtils.FormatCurrency(inv.InvoiceAmount);
                    if (inv.IsPayment() && unappliedPay > 0)
                    {
                        dr["UnappliedPay"] = HTBUtils.FormatCurrency(unappliedPay);
                    }
                    else
                    {
                        dr["UnappliedPay"] = "";
                    }
                    if (!inv.IsPayment())
                    {
                        dr["DueDate"] = inv.InvoiceDueDate.ToShortDateString();
                        dr["InvoiceDate"] = inv.InvoiceDate.ToShortDateString();
                        dr["TransferDate"] = "";
                    }
                    else
                    {
                        dr["DueDate"] = "";
                        dr["InvoiceDate"] = inv.InvoicePaymentReceivedDate.ToShortDateString();
                        if (//inv.IsBankTransferrable() && 
                            HTBUtils.IsPaymentTransferrable(inv.InvoiceID) &&
                            ((orgInvAmount - GetCollectionAmount(inv.InvoiceID)) > 0) || (inv.InvoicePaymentTransferToClientDate.ToShortDateString() != HTBUtils.DefaultShortDate))
                        {
                            if (inv.InvoicePaymentTransferToClientDate.ToShortDateString() != HTBUtils.DefaultShortDate)
                            {
                                dr["TransferDate"] = inv.InvoicePaymentTransferToClientDate.ToShortDateString();
                                dr["TransferAmount"] = HTBUtils.FormatCurrency(inv.InvoicePaymentTransferToClientAmount);
                            }
                            else
                            {
                                dr["TransferDate"] = "Noch nicht";
                                dr["TransferAmount"] = "";
                            }
                        }
                        else
                        {
                            dr["TransferDate"] = "";
                            dr["TransferAmount"] = "";
                        }
                    }

                    dr["EditPopupUrl"] = "EditInvoice.aspx?ID=" + inv.InvoiceID;
                    dt.Rows.Add(dr);
                }
            }
            if (!showDetailInterest && totalInterest > 0)
            {
                DataRow dr = dt.NewRow();
                dr["DeleteUrl"] = "<img src=\"../../intranet/images/delete2hover_dis.gif\" width=\"16\" height=\"16\" alt=\"L&ouml;scht diesen Datensatz.\" />";
                dr["EditUrl"] = "<img src=\"../../intranet/images/edit_dis.gif\" width=\"16\" height=\"16\" alt=\"&Auml;ndern diesen Datensatz.\" style=\"border-color:White;border-width:0px;\"/>";
                dr["InvoiceID"] = "";
                dr["InvoiceDescription"] = "Zinsen";
                dr["InvoiceAmount"] = HTBUtils.FormatCurrency(totalInterest);
                dr["UnappliedPay"] = "";
                dr["DueDate"] = "";
                dr["InvoiceDate"] = "";
                dr["TransferDate"] = "";
                dr["TransferAmount"] = "";
                dr["EditPopupUrl"] = "";
                dt.Rows.Add(dr);
            }
            gvInvoices.DataSource = dt;
            gvInvoices.DataBind();
            //gvInvoices.Columns[0].Visible = permissions.GrantInkassoBuchungDel;
            //gvInvoices.Columns[1].Visible = permissions.GrantInkassoBuchungEdit;
        }
        private DataTable GetInvoicesDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("DeleteUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("EditUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("EditPopupUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceID", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceDescription", typeof(string)));
            dt.Columns.Add(new DataColumn("AppliedAmount", typeof(double)));
            dt.Columns.Add(new DataColumn("DueDate", typeof(string)));
            dt.Columns.Add(new DataColumn("TransferDate", typeof(string)));
            dt.Columns.Add(new DataColumn("TransferAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("UnappliedPay", typeof(string)));
            return dt;
        }
        public double GetTotalDue()
        {
            return _totalAmountDue;
        }
        public string GetTotalUappliedPay()
        {
            return _unappliedPay > 0 ? HTBUtils.FormatCurrency(_unappliedPay) : "";
        }

        private double GetCollectionAmount(int invId)
        {
            /*
            const string qryName = "qryCustInkAktInvoiceApplyFrom";
            const string fieldName = "ApplyFromInvoiceId";

            string sql = "SELECT * FROM " + qryName + " WHERE " + fieldName + " = " + invId;

            ArrayList applyList = HTBUtils.GetSqlRecords(sql, typeof(qryCustInkAktInvoiceApply));
            return (from qryCustInkAktInvoiceApply applyRec in applyList where !applyRec.IsClientInvoice() select applyRec.ApplyAmount).Sum();
             */
            return (from InvoiceIdAndAmountRecord rec in _paymentsAppliedToCollectionInvoicesList where rec.InvoiceID == invId select rec.Amount).Sum();
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
                    var doc = (qryDoksInkAkten) rec;
                    int mahnungsNumber = GetDocMahnungNumber(doc.DokCaption);

                    var sb = new StringBuilder("<a href=\"javascript:void(window.open('");

                    if (mahnungsNumber > 0)
                    {
                        // the actual link
                        sb.Append("DeleteMahnung.aspx?");
                        sb.Append(GlobalHtmlParams.INKASSO_AKT);
                        sb.Append("=");
                        sb.Append(doc.CustInkAktID.ToString());
                        GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.MAHNUNG_NUMBER, mahnungsNumber.ToString());
                        GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.DOCUMENT_ID, doc.DokID.ToString());
                    }
                    else
                    {
                        sb.Append("../global_forms/GlobalDelete.aspx?titel=Position%20löschen&amp;frage=Sie%20sind%20dabei%20diese%20Position%20zu%20löschen:&amp;strTable=tblDokument&amp;strTextField=DokAttachment&amp;strColumn=DokID&amp;ID=");
                        sb.Append(doc.DokID);
                    }
                    // continue with the popup params
                    sb.Append("','_blank','toolbar=no,menubar=no'))\">");
                    sb.Append("<img src=\"../../intranet/images/delete2hover.gif\" width=\"16\" height=\"16\" alt=\"L&ouml;scht diesen Datensatz.\" style=\"border-color:White;border-width:0px;\"/>");
                    sb.Append("</a>");
                    dr["DeleteUrl"] = sb.ToString();

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
                    var doc = (qryDoksIntAkten) rec;
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
            gvDocs.Columns[0].Visible = Permissions.GrantInkassoDokumentDel;
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
            if(docCaption.IndexOf("Mahnung") >= 0)
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

        #region Installments
        private void LoadInstallmentsList()
        {
//            string where = "CustInkAktRateAktId = " + QryInkassoakt.CustInkAktID;
//            const string orderBy = "CustInkAktRatePostponeTillDate, CustInkAktRateDueDate";
//            _installmentsList = HTBUtils.GetSqlRecords("SELECT * FROM qryCustInkRate WHERE " + where + " ORDER BY " + orderBy, typeof(qryCustInkRate));
        }
        private void PopulateInstallmentsGrid()
        {
            DataTable dt = GetGridDataTableStructure();
            foreach (qryCustInkRate installment in _installmentsList)
            {
                DataRow dr = dt.NewRow();
                dr["InstallmentID"] = installment.CustInkAktRateID;
                dr["ReceivedID"] = installment.CustInkAktRateInvoiceID;
                dr["RateDueDate"] = installment.CustInkAktRateDueDate.ToShortDateString();
                dr["RateAmount"] = HTBUtils.FormatCurrency(installment.CustInkAktRateAmount, true);
                dr["RateBalance"] = HTBUtils.FormatCurrency(installment.CustInkAktRateBalance, true);
                dr["ClientName"] = installment.KlientName1 + "  " + installment.KlientName2;
                dr["BankAccountNumber"] = installment.KlientKtoNr1;
                dr["ReceivedDate"] = GetInstallmentReceivedDate(installment); // this call sets the TempReceivedAmount used in the line below
                dr["ReceivedAmount"] = installment.CustInkAktRateReceivedAmount > 0 ? HTBUtils.FormatCurrency(installment.CustInkAktRateReceivedAmount, true) : "";
                dr["PostponeTillDate"] = installment.CustInkAktRatePostponeTillDate != HTBUtils.DefaultDate ? installment.CustInkAktRatePostponeTillDate.ToShortDateString() : "";
                dr["PostponeReason"] = installment.CustInkAktRatePostponeReason.Replace(Environment.NewLine, "<br/>");
                dr["PostponeBy"] = installment.UserVorname + "&nbsp;" + installment.UserNachname;
                dr["PostponeURL"] = "../../intranet/images/edit.gif";
                //dr["PostponePopupURL"] = "PostponeRate.aspx?" + GlobalHtmlParams.ID + "=" + installment.CustInkAktRateID;

                if (!HTBUtils.IsZero(installment.CustInkAktRateBalance)) 
                {
                    var sb = new StringBuilder("<a href=\"javascript:void(window.open('");
                    // the actual link
                    sb.Append("PostponeRate.aspx?");
                    GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, installment.CustInkAktRateID.ToString());
                    // continue with the popup params
                    sb.Append("','_blank','toolbar=no,menubar=no,height=600,scrollbars=yes'))\">");
                    sb.Append("<img src=\"../../intranet/images/edit.gif\" width=\"16\" height=\"16\" alt=\"Rate Verschiben.\" style=\"border-color:White;border-width:0px;\"/>");
                    sb.Append("</a>");
                    dr["PostponePopupURL"] = sb.ToString();
                }
                else
                {
                    dr["PostponePopupURL"] = "<img src=\"../../intranet/images/edit_dis.gif\" width=\"16\" height=\"16\" alt=\"Rate Unverschiebbar.\" />";
                }


                
                dt.Rows.Add(dr);
            }
            gvInstallments.DataSource = dt;
            gvInstallments.DataBind();
        }
        private DataTable GetGridDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("InstallmentID", typeof(int)));
            dt.Columns.Add(new DataColumn("ReceivedID", typeof(int)));
            dt.Columns.Add(new DataColumn("RateDueDate", typeof(string)));
            dt.Columns.Add(new DataColumn("RateAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("RateBalance", typeof(string)));
            dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
            dt.Columns.Add(new DataColumn("BankAccountNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("TransferDate", typeof(string)));
            dt.Columns.Add(new DataColumn("ReceivedDate", typeof(string)));
            dt.Columns.Add(new DataColumn("ReceivedAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("PostponeTillDate", typeof(string)));
            dt.Columns.Add(new DataColumn("PostponeReason", typeof(string)));
            dt.Columns.Add(new DataColumn("PostponeBy", typeof(string)));
            dt.Columns.Add(new DataColumn("PostponePopupURL", typeof(string)));
            dt.Columns.Add(new DataColumn("PostponeURL", typeof(string)));
            
            
            return dt;
        }
        private string GetInstallmentReceivedDate(qryCustInkRate installment)
        {
            if (installment != null && installment.CustInkAktRateInvoiceID > 0)
            {
                var inv = (tblCustInkAktInvoice)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceID = " + installment.CustInkAktRateInvoiceID, typeof(tblCustInkAktInvoice));
                if (inv != null)
                {
                    installment.TempReceivedAmount = inv.InvoiceAmount;
                    if (inv.IsPayment())
                    {
                        return inv.InvoicePaymentReceivedDate.ToShortDateString();
                    }
                    return inv.InvoiceDate.ToShortDateString();
                }
            }
            return "";
        }
        #endregion

        protected void btnMahnung_Click(object sender, EventArgs e)
        {
            var kostenCalc = new KostenCalculator();
            kostenCalc.GenerateNewMahnung(QryInkassoakt.CustInkAktID);
            var mahMgr = new MahnungManager();
            mahMgr.GenerateMahnungen(GlobalUtilArea.GetUserId(Session));
            // refresh screen
            Response.Redirect("EditAktInk.aspx?ID=" + QryInkassoakt.CustInkAktID);
        }

        public string GetKlientID()
        {
            return QryInkassoakt.KlientID.ToString();
        }

        public string GetSelectedMainStatus()
        {
            return null;
        }

        public string GetSelectedSecondaryStatus()
        {
            return null;
        }

        public string GetLawyerInfo(Record rec)
        {
            if (rec == null)
                return "";

            var sb = new StringBuilder();
            if (rec is tblLawyer)
            {
                sb.Append(string.IsNullOrEmpty(((tblLawyer) rec).LawyerAnrede) ? "" : ((tblLawyer) rec).LawyerAnrede + " ");
                sb.Append(((tblLawyer) rec).LawyerName1);
                sb.Append(" ");
                sb.Append(((tblLawyer) rec).LawyerName2);
                sb.Append(" [");
                sb.Append(((tblLawyer) rec).LawyerEmail);
                sb.Append("]");
            }
            else if (rec is qryCustAktEdit)
            {
                sb.Append(string.IsNullOrEmpty(((qryCustAktEdit)rec).LawyerAnrede) ? "" : ((qryCustAktEdit)rec).LawyerAnrede + " ");
                sb.Append(((qryCustAktEdit)rec).LawyerName1);
                sb.Append(" ");
                sb.Append(((qryCustAktEdit)rec).LawyerName2);
                sb.Append(" [");
                sb.Append(((qryCustAktEdit)rec).LawyerEmail);
                sb.Append("]");
            }
            return sb.ToString();
        }
    }
}