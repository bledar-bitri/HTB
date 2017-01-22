using System;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using HTB.Database;
using System.Text;
using System.Collections;
using HTB.Database.HTB.StoredProcs;
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

namespace HTB.v2.intranetx.aktenink
{
    public partial class ViewAktInk : Page, IWorkflow
    {
        public string AktId;
        public qryCustAktEdit QryInkassoakt = new qryCustAktEdit();
        
        private ArrayList _intAktList = new ArrayList();
        private ArrayList _invoiceList = new ArrayList();
        private ArrayList _inkassoAktions = new ArrayList();
        private readonly ArrayList _interventionAktions = new ArrayList();
        public readonly ArrayList AktionsList = new ArrayList();

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
        private double _unappliedPay;

        public PermissionsEditAktInk Permissions = new PermissionsEditAktInk();
        private spGetNextWorkflowActionCode _nextWflActionCode;

        protected void Page_Load(object sender, EventArgs e)
        {
            var sw = new Stopwatch();
            sw.Start();
            Permissions.LoadPermissions(GlobalUtilArea.GetUserId(Session));
            if (Request.QueryString["ID"] != null && !Request.QueryString["ID"].Trim().Equals(""))
            {
                AktId = Request.QueryString["ID"];

                LoadRecords();
                CombineActions();
                SetTotalInvoiceAmounts();
                if (!IsPostBack)
                {
                    GlobalUtilArea.LoadDropdownList(ddlLawyer,
                       "SELECT * FROM tblLawyer ORDER BY LawyerName1 ASC",
                       typeof(tblLawyer),
                       "LawyerID",
                       "LawyerName1", false);
                    SetValues();
                }
                Session["tmpInkAktID"] = Request.QueryString["ID"];
            }
            if (!Permissions.GrantInkassoEdit)
                ddlSendBericht.Enabled = false;
            
//            lblLoadingTimes.Text = sw.ElapsedMilliseconds.ToString();
            sw.Stop();
        }

        private void LoadRecords()
        {
            int aktId = GlobalUtilArea.GetZeroIfConvertToIntError(Request.QueryString["ID"]);
            _interventionAktions.Clear();
            ArrayList[] results = HTBUtils.GetMultipleListsFromStoredProcedure("spGetInkassoAktDataView", new ArrayList
                                                                                                 {
                                                                                                     new StoredProcedureParameter("aktId", SqlDbType.Int, aktId)
                                                                                                 },
                                                                                                 new[]
                                                                                                 {
                                                                                                     typeof(qryCustAktEdit), 
                                                                                                     typeof(tblCustInkAktInvoice), 
                                                                                                     typeof(qryCustInkAktAktionen), 
                                                                                                     typeof(qryDoksInkAkten), 
                                                                                                     typeof(tblAktenInt), 
                                                                                                     typeof(spGetNextWorkflowActionCode),
                                                                                                 }
                                                                                                 );
            try
            {
                QryInkassoakt = (qryCustAktEdit) results[0][0];
                _invoiceList = results[1];
                _inkassoAktions = results[2];
                _docsList = results[3];
                _intAktList = results[4];
                _nextWflActionCode = (spGetNextWorkflowActionCode) results[5][0];
            }
            catch
            {
            }
            if (_intAktList != null && _intAktList.Count > 0)
            {
                foreach (tblAktenInt intAkt in _intAktList)
                {
                    HTBUtils.AddListToList(HTBUtils.GetSqlRecords("SELECT * FROM qryInktAktAction WHERE AktIntActionAkt = " + intAkt.AktIntID + " ORDER BY AktIntActionDate DESC", typeof (qryInktAktAction)), _interventionAktions);
                    if (intAkt.AktIntStatus < 2)
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
                    HTBUtils.AddListToList(HTBUtils.GetSqlRecords("SELECT * FROM qryDoksIntAkten WHERE AktIntID = " + intAkt.AktIntID, typeof (qryDoksIntAkten)), _docsList);
                }
            }
        }

        private void SetValues()
        {
            lblCustInkAktID.Text = QryInkassoakt.CustInkAktID.ToString();
            if (QryInkassoakt.CustInkAktOldID != "")
            {
                lblCustInkAktID.Text+=(" [" + QryInkassoakt.CustInkAktOldID + "]");
            }
            lblCustInkAktEnterDate.Text = QryInkassoakt.CustInkAktEnterDate.ToShortDateString();

            lblAuftraggeber.Text = "<strong>" + QryInkassoakt.AuftraggeberName1 + "</strong>&nbsp;" + QryInkassoakt.AuftraggeberName2 + "<br/>" +
                                   QryInkassoakt.AuftraggeberStrasse + "<br>" +
                                   QryInkassoakt.AuftraggeberLKZ + "&nbsp;" + QryInkassoakt.AuftraggeberPLZ + "&nbsp;" + QryInkassoakt.AuftraggeberOrt;

            lblKlient.Text = "<strong>" + QryInkassoakt.KlientName1 + "</strong>&nbsp;" + QryInkassoakt.KlientName2 + "<br/>" +
                             QryInkassoakt.KlientStrasse + "<br/>" +
                             QryInkassoakt.KlientLKZ + "&nbsp;" + QryInkassoakt.KlientPLZ + "&nbsp;" + QryInkassoakt.KlientOrt;

            lblCustInkAktKunde.Text = !QryInkassoakt.CustInkAktKunde.Trim().Equals("") ? QryInkassoakt.CustInkAktKunde : "Keine Rechnungsnummer vorhanden!";
            lblCustInkAktGothiaNr.Text = !string.IsNullOrEmpty(QryInkassoakt.CustInkAktGothiaNr) ? QryInkassoakt.CustInkAktGothiaNr.ToString() : "&nbsp;";
            lblCustInkAktInvoiceDate.Text = QryInkassoakt.CustInkAktInvoiceDate.ToShortDateString();

            lblGegner.Text = "<strong><a href=\"#\" onClick=\"MM_openBrWindow('../../intranet/gegner/editgegner.asp?poprefresh=true&GegnerID="+QryInkassoakt.CustInkAktGegner+"','popWindow','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=600')\">";

            lblGegner.Text += QryInkassoakt.GegnerLastName1 + ", " + QryInkassoakt.GegnerLastName2 + "</a></strong><br/>";
            lblGegner.Text += QryInkassoakt.GegnerLastStrasse + "<br/>";
            lblGegner.Text += QryInkassoakt.GegnerLastZipPrefix + "&nbsp;" + QryInkassoakt.GegnerLastZip + "&nbsp;" + QryInkassoakt.GegnerLastOrt;


            lblKlient.Text = "<strong><a href=\"#\" onClick=\"MM_openBrWindow('/v2/intranet/klienten/editklient.asp?poprefresh=true&ID=" + QryInkassoakt.CustInkAktKlient + "','popWindow','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=600')\">";
            
            lblKlient.Text += "<strong>"+QryInkassoakt.KlientName1.Trim() + "</strong>&nbsp;" + QryInkassoakt.KlientName2.Trim() + "</strong></a><br/>";
            lblKlient.Text += QryInkassoakt.KlientStrasse + "<br/>";
            lblKlient.Text += QryInkassoakt.KlientLKZ + "&nbsp;" + QryInkassoakt.KlientPLZ + "&nbsp;" + QryInkassoakt.KlientOrt;

            /*
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
            ddlLawyer.SelectedValue = QryInkassoakt.CustInkAktLawyerId.ToString();
            ddlSendBericht.SelectedValue = QryInkassoakt.CustInkAktSendBericht ? "1" : "0";
            lblCustInkAktNextWFLStep.Text = QryInkassoakt.CustInkAktNextWFLStep.ToShortDateString();
            lblNetWflAction.Text = _nextWflActionCode != null ? CtlWorkflow.GetActionText(_nextWflActionCode.WfpActionCode) : "";

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
            PopulateDocumentsGrid();*/
        }
        
        #region Event Handlers
        
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalUtilArea.IsPopUp(Request))
                bdy.Attributes.Add("onLoad", "window.close()");

            else if (GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL]) == GlobalHtmlParams.BACK)
                bdy.Attributes.Add("onLoad", "javascript:history.go(-2)");

            else
                ShowParentScreen();
        }

        #endregion

        public void ShowParentScreen()
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

            AktionsList.Sort(new InkassoActionRecordComparer());
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

                    dr["DokAttachment"] = GetDocAttachmentLink(doc.DokAttachment);

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

                    dr["DokAttachment"] = GetDocAttachmentLink(doc.DokAttachment); 
                    dr["DokChangeDate"] = doc.DokCreationTimeStamp.ToShortDateString();
                    dr["DokTypeCaption"] = doc.DokTypeCaption;
                    dr["DokCaption"] = doc.DokCaption;
                    dr["DokUser"] = doc.UserVorname + " " + doc.UserNachname;
                    
                    dt.Rows.Add(dr);
                }
            }
//            gvDocs.DataSource = dt;
//            gvDocs.DataBind();
//            gvDocs.Columns[0].Visible = Permissions.GrantInkassoDokumentDel;
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
        private string GetDocAttachmentLink(string attachment)
        {
            var sb = new StringBuilder("<a href=\"javascript:void(window.open('../../intranet/documents/files/");
            sb.Append(attachment);
            sb.Append("','popSetAction','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800,top=10'))\">");
            sb.Append(attachment);
            sb.Append("</a>");
            return sb.ToString();
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
    }
}