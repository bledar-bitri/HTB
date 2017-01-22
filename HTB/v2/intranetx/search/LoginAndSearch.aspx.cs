using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using HTB.Database.HTB.Views;
using HTB.Database.LookupRecords;
using HTB.Database.Views;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.search
{
    public partial class LoginAndSearch : Page
    {
        private const string SessionSearchName = "Search_Record";
        private const int PageSize = 15;

        private int GegnerId;
        private int ClientId;
        private int AgId;
        private double TotalInkassoForderung = 0;
        private double TotalInkassoCharges = 0;
        private double TotalInkassoPaid = 0;

        private double TotalInterventionForderung = 0;
        private double TotalInterventionCharges = 0;
        private double TotalInterventionPaid = 0;

        private tblGegner currentGegner = null;
        private tblKlient currentClient = null;
        private tblAuftraggeber currentAg = null;
        private SearchRecord searchRecord = new SearchRecord();
        private bool _isNewSearch = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            ClearScreen();
            
            if (!IsPostBack)
            {
                bool ok = true;
                if (GlobalUtilArea.GetUserId(Session) <= 0)
                {
                    string userName = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.USER_NAME]).Replace("'", "''");
                    string password = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.PASSWORD]).Replace("'", "''");
            
                    ok = GlobalUtilArea.Login(userName, password, Session, Response);
                }
                if(ok)
                {

                    searchRecord.SearchName = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.SEARCH_FOR_NAME]);
                    searchRecord.SearchNumber = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.SEARCH_FOR_NUMBER]);
                    searchRecord.SearchPhone = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.SEARCH_FOR_PHONE]);

                    _isNewSearch = HTBUtils.GetBoolValue(GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.NEW_SEARCH]));

                    GegnerId = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.GEGNER_ID]);
                    ClientId = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.CLIENT_ID]);
                    AgId = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.AUFTRAGGEBER_ID]);
                    
                    if(!_isNewSearch)
                        searchRecord.Assign((SearchRecord) Session[SessionSearchName]);

                    PopulateFieldsFromSearchRecord(searchRecord);

                    InitIndexes();

                    if (GegnerId == 0 && ClientId == 0 && AgId == 0)
                    {
                        if (searchRecord != null && !searchRecord.IsEmpty())
                        {
                            Search(searchRecord);
                        }
                    }
                    else if (GegnerId > 0)
                    {
                        ctlMessage.ShowInfo(GegnerId.ToString());
                        LookupGegner();

                    }
                    else if (AgId > 0)
                    {
                        ctlMessage.ShowInfo(AgId.ToString());
                        LookupAg();
                    }
                }
            }
        }

        #region Event Handlers

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            InitIndexes();
            Search(new SearchRecord
            {
                SearchName = txtName.Text,
                SearchNumber = txtAkt.Text,
                SearchPhone = txtPhone.Text
            });
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }


        protected void gvGegner_RowCreated(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // when mouse is over the row, save original color to new attribute, and change it to highlight yellow color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#2dc6ee'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
                e.Row.Attributes.Add("onClick", "location.href='LoginAndSearch.aspx?" + GlobalHtmlParams.GEGNER_ID + "=" + DataBinder.Eval(e.Row.DataItem, "ID") + "'");
            }
        }

        protected void gvKlient_RowCreated(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // when mouse is over the row, save original color to new attribute, and change it to highlight yellow color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#2dc6ee'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
                e.Row.Attributes.Add("onClick", "location.href='LoginAndSearch.aspx?" + GlobalHtmlParams.CLIENT_ID + "=" + DataBinder.Eval(e.Row.DataItem, "ID") + "'");
            }
        }

        protected void gvAG_RowCreated(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // when mouse is over the row, save original color to new attribute, and change it to highlight yellow color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#2dc6ee'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
                e.Row.Attributes.Add("onClick", "location.href='LoginAndSearch.aspx?" + GlobalHtmlParams.AUFTRAGGEBER_ID + "=" + DataBinder.Eval(e.Row.DataItem, "ID") + "'");
            }
        }
        
        protected void gvInkasso_RowCreated(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // when mouse is over the row, save original color to new attribute, and change it to highlight yellow color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#2dc6ee'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
                e.Row.Attributes.Add("onClick", "location.href='/v2/intranetx/aktenink/EditAktInk.aspx?" + GlobalHtmlParams.ID + "=" + DataBinder.Eval(e.Row.DataItem, "ID") + "&" +
                                                GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL +"=" +GlobalHtmlParams.BACK+"'");
            }
        }

        protected void gvIntervention_RowCreated(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // when mouse is over the row, save original color to new attribute, and change it to highlight yellow color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#2dc6ee'");

                // when mouse leaves the row, change the bg color to its original value   
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
                e.Row.Attributes.Add("onClick", "location.href='/v2/intranetx/aktenint/workaktint.aspx?" + GlobalHtmlParams.ID + "=" + DataBinder.Eval(e.Row.DataItem, "ID") + "'");
            }
        } 

        protected void gvGegnerAddress_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // when mouse is over the row, save original color to new attribute, and change it to highlight yellow color
//                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#2dc6ee'");

                // when mouse leaves the row, change the bg color to its original value   
//                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
//                e.Row.Attributes.Add("onClick", "location.href='/v2/intranetx/aktenint/workaktint.aspx?" + GlobalHtmlParams.ID + "=" + DataBinder.Eval(e.Row.DataItem, "ID") + "'");
            }
        }
        protected void gvGegnerPhone_RowCreated(object sender, GridViewRowEventArgs e)
        {
            
        }

        protected void cmdPreviousGegner_Click(object sender, EventArgs e)
        {
            SetGegnerIndex(GetGegnerIndex() - 15);
            Search(new SearchRecord
                           {
                               SearchName = txtName.Text,
                               SearchNumber = txtAkt.Text,
                               SearchPhone = txtPhone.Text
                           });
        }
        protected void cmdNextGegner_Click(object sender, EventArgs e)
        {
            SetGegnerIndex(GetGegnerIndex() + 15);
            Search(new SearchRecord
            {
                SearchName = txtName.Text,
                SearchNumber = txtAkt.Text,
                SearchPhone = txtPhone.Text
            });
        }
        
        protected void cmdPreviousClient_Click(object sender, EventArgs e)
        {
            SetClientIndex(GetClientIndex() - 15);
            Search(new SearchRecord
            {
                SearchName = txtName.Text,
                SearchNumber = txtAkt.Text,
                SearchPhone = txtPhone.Text
            });
        }
        protected void cmdNextClient_Click(object sender, EventArgs e)
        {
            SetClientIndex(GetClientIndex() + 15);
            Search(new SearchRecord
            {
                SearchName = txtName.Text,
                SearchNumber = txtAkt.Text,
                SearchPhone = txtPhone.Text
            });
        }

        protected void cmdPreviousAg_Click(object sender, EventArgs e)
        {
            SetAgIndex(GetAgIndex() - 15);
            Search(new SearchRecord
            {
                SearchName = txtName.Text,
                SearchNumber = txtAkt.Text,
                SearchPhone = txtPhone.Text
            });
        }
        protected void cmdNextAg_Click(object sender, EventArgs e)
        {
            SetAgIndex(GetAgIndex() + 15);
            Search(new SearchRecord
            {
                SearchName = txtName.Text,
                SearchNumber = txtAkt.Text,
                SearchPhone = txtPhone.Text
            });
        }
            
        #endregion

        private void Search(SearchRecord searchRec)
        {
            if(!string.IsNullOrEmpty(searchRec.SearchNumber))
            {
#region Search for number
                var parameters = new ArrayList
                                    {
                                        new StoredProcedureParameter("number", SqlDbType.VarChar, searchRec.SearchNumber.ToString())
                                    };

                ArrayList[] lists = HTBUtils.GetMultipleListsFromStoredProcedure("spSearchAktsForNumber", parameters, new Type[] { typeof(SearchAktLookup), typeof(SearchAktLookup) });
                ctlMessage.ShowInfo("<strong/>Suchnummer: "+searchRec.SearchNumber+"</strong>");
                    
                PopulateInkassoGrid(lists[0], true, true);
                PopulateInterventionGrid(lists[1], true, true);
                tabContainer1.Visible = true;
                tabContainer1.Tabs[1].Visible = false;
#endregion
            }
            else if (!string.IsNullOrEmpty(searchRec.SearchName))
            {
#region Search for name
                var parameters = new ArrayList
                                     {
                                         new StoredProcedureParameter("startGegnerIdx", SqlDbType.Int, GetGegnerIndex()),
                                         new StoredProcedureParameter("startClientIdx", SqlDbType.Int, GetClientIndex()),
                                         new StoredProcedureParameter("startAuftraggeberIdx", SqlDbType.Int, GetAgIndex()),

                                         new StoredProcedureParameter("countGegner", SqlDbType.Int, 1),
                                         new StoredProcedureParameter("countClients", SqlDbType.Int, 1),
                                         new StoredProcedureParameter("countAuftraggeber", SqlDbType.Int, 1),

                                         new StoredProcedureParameter("totalGegner", SqlDbType.Int, 0, ParameterDirection.Output),
                                         new StoredProcedureParameter("totalClients", SqlDbType.Int, 0, ParameterDirection.Output),
                                         new StoredProcedureParameter("totalAuftraggeber", SqlDbType.Int, 0, ParameterDirection.Output),

                                         new StoredProcedureParameter("pageSize", SqlDbType.Int, PageSize),

                                         new StoredProcedureParameter("name", SqlDbType.NVarChar, searchRec.SearchName)
                                     };

                ArrayList[] lists = HTBUtils.GetMultipleListsFromStoredProcedure("spSearchDetail", parameters, new Type[] {typeof (GegnerDetailLookup), typeof (KlientDetailLookup), typeof (AuftraggeberDetailLookup)});

                foreach (Object obj in parameters)
                {
                    if (obj is ArrayList)
                    {
                        var outParams = (ArrayList) obj;
                        foreach (StoredProcedureParameter outParam in outParams)
                        {
                            if (outParam.Name == "totalGegner")
                                ShowFoundCounter(lblGegnerCount, GetGegnerIndex(), outParam.Value.ToString());
                            else if (outParam.Name == "totalClients")
                                ShowFoundCounter(lblClientCount, GetClientIndex(), outParam.Value.ToString());
                            else if (outParam.Name == "totalAuftraggeber")
                                ShowFoundCounter(lblAgCount, GetAgIndex(), outParam.Value.ToString());
                        }
                    }
                }

                PopulateGegnerGrid(lists[0]);
                PopulateKlientGrid(lists[1]);
                PopulateAgGrid(lists[2]);
#endregion
            }
            else if (!string.IsNullOrEmpty(searchRec.SearchPhone))
            {
#region Search for phone
                var parameters = new ArrayList
                                     {
                                         new StoredProcedureParameter("startGegnerIdx", SqlDbType.Int, GetGegnerIndex()),
                                         new StoredProcedureParameter("startClientIdx", SqlDbType.Int, GetClientIndex()),
                                         new StoredProcedureParameter("startAuftraggeberIdx", SqlDbType.Int, GetAgIndex()),

                                         new StoredProcedureParameter("countGegner", SqlDbType.Int, 1),
                                         new StoredProcedureParameter("countClients", SqlDbType.Int, 1),
                                         new StoredProcedureParameter("countAuftraggeber", SqlDbType.Int, 1),

                                         new StoredProcedureParameter("totalGegner", SqlDbType.Int, 0, ParameterDirection.Output),
                                         new StoredProcedureParameter("totalClients", SqlDbType.Int, 0, ParameterDirection.Output),
                                         new StoredProcedureParameter("totalAuftraggeber", SqlDbType.Int, 0, ParameterDirection.Output),

                                         new StoredProcedureParameter("pageSize", SqlDbType.Int, PageSize),

                                         new StoredProcedureParameter("phone", SqlDbType.NVarChar, searchRec.SearchPhone)
                                     };

                ArrayList[] lists = HTBUtils.GetMultipleListsFromStoredProcedure("spSearchDetailForPhoneNumber", parameters, new Type[] { typeof(GegnerPhoneDetailLookup), typeof(GegnerDetailLookup), typeof(KlientDetailLookup), typeof(AuftraggeberDetailLookup) });

                foreach (Object obj in parameters)
                {
                    if (obj is ArrayList)
                    {
                        var outParams = (ArrayList)obj;
                        foreach (StoredProcedureParameter outParam in outParams)
                        {
                            if (outParam.Name == "totalGegner")
                                ShowFoundCounter(lblGegnerCount, GetGegnerIndex(), outParam.Value.ToString());
                            else if (outParam.Name == "totalClients")
                                ShowFoundCounter(lblClientCount, GetClientIndex(), outParam.Value.ToString());
                            else if (outParam.Name == "totalAuftraggeber")
                                ShowFoundCounter(lblAgCount, GetAgIndex(), outParam.Value.ToString());
                        }
                    }
                }

                PopulateGegnerGrid(lists[1], lists[0]);
                PopulateKlientGrid(lists[2]);
                PopulateAgGrid(lists[3]);
#endregion
            }
            Session[SessionSearchName] = searchRec;
        }

        private void LookupGegner()
        {
            var parameters = new ArrayList
                                    {
                                        new StoredProcedureParameter("@gegnerID", SqlDbType.Int, GegnerId)
                                    };

            ArrayList[] lists = HTBUtils.GetMultipleListsFromStoredProcedure("spGetAllAktsByGegnerID", parameters, new Type[] { typeof(tblGegner), typeof(tblGegnerAdressen), typeof(qryGegnerPhone), typeof(SearchAktLookup), typeof(SearchAktLookup) });
            try
            {
                currentGegner = (tblGegner)lists[0][0];
                if (currentGegner != null)
                {
                    ctlMessage.ShowInfo(currentGegner.GegnerName2 + " " + currentGegner.GegnerName1 + "<br/><br/>" + currentGegner.GegnerLastStrasse + "<br/>" + currentGegner.GegnerLastZipPrefix + " - " + currentGegner.GegnerLastZip + " " + currentGegner.GegnerOrt);
                    if(!string.IsNullOrEmpty(currentGegner.GegnerMemo))
                        lblGegnerMemo.Text = "<b>Schuldner Memo:</b><BR/>" + currentGegner.GegnerMemo+"<BR/>";
                }
            }
            catch
            {
            }
            PopulateGegnerAddressGrid(lists[1]);
            PopulateGegnerPhoneGrid(lists[2]);
            PopulateInkassoGrid(lists[3], true);
            PopulateInterventionGrid(lists[4], true);
            tabContainer1.Visible = true;
            tabContainer1.Tabs[1].Visible = true;
        }

        private void LookupClient()
        {
            var parameters = new ArrayList
                                    {
                                        new StoredProcedureParameter("@clientID", SqlDbType.Int, ClientId)
                                    };

            ArrayList[] lists = HTBUtils.GetMultipleListsFromStoredProcedure("spGetAllAktsByClientID", parameters, new Type[] { typeof(tblKlient), typeof(SearchAktLookup), typeof(SearchAktLookup) });
            try
            {
                currentClient = (tblKlient)lists[0][0];
                if (currentClient != null)
                {
                    ctlMessage.ShowInfo(currentClient.KlientName1 + " " + currentClient.KlientName2 + "<br/><br/>" + currentClient.KlientStrasse + "<br/>" + currentClient.KlientStaat + " - " + currentClient.KlientPLZ + " " + currentClient.KlientOrt);
                    if (!string.IsNullOrEmpty(currentClient.KlientMemo))
                        lblGegnerMemo.Text = "<b>Klient Memo:</b><BR/>" + currentClient.KlientMemo + "<BR/>";
                }
            }
            catch
            {
            }
            PopulateInkassoGrid(lists[1], false, true);
            PopulateInterventionGrid(lists[2], false, true);
            tabContainer1.Visible = true;
            tabContainer1.Tabs[1].Visible = false;
        }

        private void LookupAg()
        {
            var parameters = new ArrayList
                                    {
                                        new StoredProcedureParameter("@auftraggeberID", SqlDbType.Int, AgId)
                                    };

            ArrayList[] lists = HTBUtils.GetMultipleListsFromStoredProcedure("spGetAllAktsByAuftraggeberID", parameters, new Type[] { typeof(tblAuftraggeber), typeof(SearchAktLookup) });
            try
            {
                currentAg = (tblAuftraggeber)lists[0][0];
                if (currentAg != null)
                {
                    ctlMessage.ShowInfo(currentAg.AuftraggeberName1 + " " + currentAg.AuftraggeberName2 + "<br/><br/>" + currentAg.AuftraggeberStrasse + "<br/>" + currentAg.AuftraggeberStaat + " - " + currentAg.AuftraggeberPLZ + " " + currentAg.AuftraggeberOrt);
                    if (!string.IsNullOrEmpty(currentAg.AuftraggeberMemo))
                        lblGegnerMemo.Text = "<b>Klient Memo:</b><BR/>" + currentAg.AuftraggeberMemo + "<BR/>";
                }
            }
            catch
            {
            }
            PopulateInterventionGrid(lists[1], false, true);
            tabContainer1.Visible = true;
            tabContainer1.Tabs[1].Visible = false;
        }
        
        private DataTable GetDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("ID", typeof(string)));
            dt.Columns.Add(new DataColumn("OldID", typeof(string)));

            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("LKZ", typeof(string)));
            dt.Columns.Add(new DataColumn("Ort", typeof(string)));
            dt.Columns.Add(new DataColumn("Strasse", typeof(string)));
            dt.Columns.Add(new DataColumn("DOB", typeof(string)));
            dt.Columns.Add(new DataColumn("Phone", typeof(string)));
            dt.Columns.Add(new DataColumn("InterventionAkte", typeof(string)));
            dt.Columns.Add(new DataColumn("InkassoAkte", typeof(string)));
            dt.Columns.Add(new DataColumn("InkassoBalance", typeof(string)));
            dt.Columns.Add(new DataColumn("Date", typeof(string)));
            return dt;
        }

        private DataTable GetAktDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("ID", typeof(string)));
            dt.Columns.Add(new DataColumn("AZ", typeof(string)));

            dt.Columns.Add(new DataColumn("AktEnteredDate", typeof(string)));
            dt.Columns.Add(new DataColumn("KlientName", typeof(string)));
            dt.Columns.Add(new DataColumn("GegnerInfo", typeof(string)));
            dt.Columns.Add(new DataColumn("AktStatus", typeof(string)));
            dt.Columns.Add(new DataColumn("AktCurStatus", typeof(string)));
            dt.Columns.Add(new DataColumn("Forderung", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalCharges", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalPaid", typeof(string)));
            dt.Columns.Add(new DataColumn("Balance", typeof(string)));
            return dt;
        }

        private DataTable GetPhoneDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("PhoneType", typeof(string)));
            dt.Columns.Add(new DataColumn("PhoneCountry", typeof(string)));
            dt.Columns.Add(new DataColumn("PhoneCity", typeof(string)));
            dt.Columns.Add(new DataColumn("Phone", typeof(string)));
            dt.Columns.Add(new DataColumn("PhoneDate", typeof(string)));
            dt.Columns.Add(new DataColumn("PhoneDescription", typeof(string)));
            return dt;
        }
        
        private void PopulateFieldsFromSearchRecord(SearchRecord searchRec)
        {
            if (searchRec != null)
            {
                txtName.Text = searchRec.SearchName;
                txtAkt.Text = searchRec.SearchNumber;
                txtPhone.Text = searchRec.SearchPhone;
            }
        }

        protected string GetTotalInkassoForderung()
        {
            return HTBUtils.FormatCurrency(TotalInkassoForderung, true);
        }
        protected string GetTotalInkassoCharges()
        {
            return HTBUtils.FormatCurrency(TotalInkassoCharges, true);
        }
        protected string GetTotalInkassoPaid()
        {
            return HTBUtils.FormatCurrency(TotalInkassoPaid, true);
        }
        protected string GetTotalInkassoBalance()
        {
            return HTBUtils.FormatCurrency(TotalInkassoCharges - TotalInkassoPaid, true);
        }


        protected string GetTotalInterventionForderung()
        {
            return HTBUtils.FormatCurrency(TotalInterventionForderung, true);
        }
        protected string GetTotalInterventionCharges()
        {
            return HTBUtils.FormatCurrency(TotalInterventionCharges, true);
        }
        protected string GetTotalInterventionPaid()
        {
            return HTBUtils.FormatCurrency(TotalInterventionPaid, true);
        }
        protected string GetTotalInterventionBalance()
        {
            return HTBUtils.FormatCurrency(TotalInterventionCharges - TotalInterventionPaid, true);
        }

        private void ClearScreen()
        {
            ClearGegnerGrid();
            ClearGegnerAddressGrid();
            ClearKlientGrid();
            ClearInkassoGrid();
            ClearInterventionGrid();

            trHeaderGegner.Visible = false;
            trHeaderKlient.Visible = false;
            trHeaderAg.Visible = false;
            trHeaderInkasso.Visible = false;
            trHeaderIntervention.Visible = false;
            trHeaderGegnerAddress.Visible = false;
            trHeaderGegnerPhone.Visible = false;

            tabContainer1.Visible = false;
        }

        #region Gegner Grid
        private void ClearGegnerGrid()
        {
            PopulateGegnerGrid(new ArrayList());
        }
        private void PopulateGegnerGrid(ArrayList list, ArrayList phonesList = null)
        {
            DataTable dt = GetDataTableStructure();
            string currentName = "";
            foreach (GegnerDetailLookup rec in list)
            {
                DataRow dr = dt.NewRow();

                var phones = new StringBuilder();
                if(phonesList!= null)
                {
                    GegnerDetailLookup gdl = rec;
                    foreach (GegnerPhoneDetailLookup gphone in phonesList.Cast<GegnerPhoneDetailLookup>().Where(gphone => gphone.GegnerID == gdl.GegnerID))
                    {
                        phones.Append("<BR/>");
                        phones.Append(gphone.GegnerPhoneCountry);
                        phones.Append(" ");
                        phones.Append(gphone.GegnerPhoneCity);
                        phones.Append(" ");
                        phones.Append(gphone.GegnerPhone);
                    }
                }

                dr["ID"] = rec.GegnerID.ToString();
                dr["OldID"] = rec.GegnerOldID;
                if(rec.GegnerName.ToLower() == currentName)
                {
                    dr["Name"] = "";
                }
                else
                {
                    dr["Name"] = rec.GegnerName;
                    currentName = rec.GegnerName.ToLower();
                }
                dr["LKZ"] = rec.GegnerLastZipPrefix;
                dr["Ort"] = rec.GegnerLastOrt;
                dr["Strasse"] = rec.GegnerLastStrasse;
                dr["DOB"] = (rec.GegnerDOB == HTBUtils.DefaultDate || rec.GegnerDOB.ToShortDateString() == "01.01.0001") ? "" : rec.GegnerDOB.ToShortDateString();
                dr["Phone"] = rec.GegnerPhoneCountry + " " + rec.GegnerPhoneCity + " " + rec.GegnerPhone + (phones.Length == 0 ? "" : phones.ToString());
                dr["InterventionAkte"] = rec.InterventionAkte.ToString();
                dr["InkassoAkte"] = rec.InkassoAkte.ToString();
                dr["InkassoBalance"] = HTBUtils.FormatCurrency(rec.InkassoBalance);

                
                dt.Rows.Add(dr);
            }
            gvGegner.DataSource = dt;
            gvGegner.DataBind();
            trHeaderGegner.Visible = gvGegner.Rows.Count > 0;
        }
        #endregion

        #region Gegner Address Grid
        private void ClearGegnerAddressGrid()
        {
            PopulateGegnerAddressGrid(new ArrayList());
        }
        private void PopulateGegnerAddressGrid(ArrayList list)
        {
            DataTable dt = GetDataTableStructure();
            string currentName = "";
            string name = "";

            foreach (tblGegnerAdressen rec in list)
            {
                DataRow dr = dt.NewRow();
                name = rec.GAName2.Trim() + " " + rec.GAName1.Trim();
                dr["ID"] = rec.GAID.ToString();

                if (name == currentName)
                {
                    dr["Name"] = "";
                }
                else
                {
                    dr["Name"] = name;
                    currentName = name;
                }
                dr["LKZ"] = rec.GAZipPrefix;
                dr["Ort"] = rec.GAOrt;
                dr["Strasse"] = rec.GAStrasse;
                dr["Date"] = rec.GATimeStamp.ToShortDateString();
                
                dt.Rows.Add(dr);
            }
            gvGegnerAddress.DataSource = dt;
            gvGegnerAddress.DataBind();
            trHeaderGegnerAddress.Visible = gvGegnerAddress.Rows.Count > 0;
        }
        #endregion

        #region Gegner Phone Grid
        private void ClearGegnerPhoneGrid()
        {
            PopulateGegnerPhoneGrid(new ArrayList());
        }
        private void PopulateGegnerPhoneGrid(ArrayList list)
        {
            DataTable dt = GetPhoneDataTableStructure();
            if (currentGegner != null && !string.IsNullOrEmpty(currentGegner.GegnerPhone))
            {
                DataRow dr = dt.NewRow();
                dr["PhoneType"] = "";

                dr["PhoneCity"] = currentGegner.GegnerPhoneCountry + " " + currentGegner.GegnerPhoneCity;
                dr["Phone"] = currentGegner.GegnerPhone;
                dr["PhoneDate"] = "Aktuell";
                dr["PhoneDescription"] = "";

                dt.Rows.Add(dr);
            }
            foreach (qryGegnerPhone rec in list)
            {
                DataRow dr = dt.NewRow();
                dr["PhoneType"] = rec.PhoneTypeCaption;

                dr["PhoneCity"] = rec.GPhoneCountry + " " + rec.GPhoneCity;
                dr["Phone"] = rec.GPhone;
                dr["PhoneDate"] = rec.GPhoneDate.ToShortDateString();
                dr["PhoneDescription"] = rec.GPhoneDescription;

                dt.Rows.Add(dr);
            }
            gvGegnerPhone.DataSource = dt;
            gvGegnerPhone.DataBind();
            trHeaderGegnerPhone.Visible = gvGegnerPhone.Rows.Count > 0;
        }
        #endregion

        #region Klient Grid
        private void ClearKlientGrid()
        {
            PopulateKlientGrid(new ArrayList());
        }
        private void PopulateKlientGrid(ArrayList list)
        {
            DataTable dt = GetDataTableStructure();
            string currentName = "";
            foreach (KlientDetailLookup rec in list)
            {
                DataRow dr = dt.NewRow();

                dr["ID"] = rec.KlientID.ToString();
                dr["OldID"] = rec.KlientOldID;
                if (rec.KlientName.ToLower() == currentName)
                {
                    dr["Name"] = "";
                }
                else
                {
                    dr["Name"] = rec.KlientName;
                    currentName = rec.KlientName.ToLower();
                }

                dr["LKZ"] = rec.KlientLKZ;
                dr["Ort"] = rec.KlientOrt;
                dr["Strasse"] = rec.KlientStrasse;
                dr["Phone"] = rec.KlientPhoneCountry + " " + rec.KlientPhoneCity + " " + rec.KlientPhone;
                dr["InterventionAkte"] = rec.InterventionAkte.ToString();
                dr["InkassoAkte"] = rec.InkassoAkte.ToString();
                dr["InkassoBalance"] = HTBUtils.FormatCurrency(rec.InkassoBalance, true);
                
                dt.Rows.Add(dr);
            }
            gvKlient.DataSource = dt;
            gvKlient.DataBind();
            trHeaderKlient.Visible = gvKlient.Rows.Count > 0;
        }
        #endregion

        #region Auftraggeber Grid
        private void ClearAgGrid()
        {
            PopulateAgGrid(new ArrayList());
        }
        private void PopulateAgGrid(ArrayList list)
        {
            DataTable dt = GetDataTableStructure();
            string currentName = "";
            foreach (AuftraggeberDetailLookup rec in list)
            {
                DataRow dr = dt.NewRow();

                dr["ID"] = rec.AgId.ToString();
                if (rec.AgName.ToLower() == currentName)
                {
                    dr["Name"] = "";
                }
                else
                {
                    dr["Name"] = rec.AgName;
                    currentName = rec.AgName.ToLower();
                }

                dr["LKZ"] = rec.AgLKZ;
                dr["Ort"] = rec.AgOrt;
                dr["Strasse"] = rec.AgStrasse;
                dr["Phone"] = rec.AgPhoneCountry + " " + rec.AgPhoneCity + " " + rec.AgPhone;
                dr["InterventionAkte"] = rec.InterventionAkte.ToString();
                dr["InkassoAkte"] = rec.InkassoAkte.ToString();
                dr["InkassoBalance"] = HTBUtils.FormatCurrency(rec.InkassoBalance, true);

                dt.Rows.Add(dr);
            }
            gvAg.DataSource = dt;
            gvAg.DataBind();
            trHeaderAg.Visible = gvAg.Rows.Count > 0;
        }
        #endregion

        #region CollectionInvoice Grid
        private void ClearInkassoGrid()
        {
            PopulateInkassoGrid(new ArrayList());
        }
        private void PopulateInkassoGrid(ArrayList list, bool isGegner = false, bool isClient = false)
        {
            TotalInkassoForderung = 0;
            TotalInkassoCharges = 0;
            TotalInkassoPaid = 0;
            
            DataTable dt = GetAktDataTableStructure();

            foreach (SearchAktLookup rec in list)
            {
                DataRow dr = dt.NewRow();

                dr["ID"] = rec.AktId.ToString();
                dr["AZ"] = !string.IsNullOrEmpty(rec.AktAZ) ? rec.AktAZ : rec.AktKunde;

                dr["AktEnteredDate"] = (rec.AktEnteredDate == HTBUtils.DefaultDate || rec.AktEnteredDate.ToShortDateString() == "01.01.0001") ? "" : rec.AktEnteredDate.ToShortDateString();
                dr["KlientName"] = rec.KlientName;
                dr["GegnerInfo"] = rec.GegnerName + "<br/><br/>" + rec.GegnerAddress + "<br/>" + rec.GegnerCountry + " - " + rec.GegnerZip + " " + rec.GegnerCity;
                dr["AktStatus"] = rec.AktStatusCaption + "<BR/>" + rec.AktCurStatusCaption;
                dr["Forderung"] = HTBUtils.FormatCurrency(rec.Forderung, true);
                dr["TotalCharges"] = HTBUtils.FormatCurrency(rec.TotalCharges, true);
                dr["TotalPaid"] = HTBUtils.FormatCurrency(rec.TotalPaid, true);
                dr["Balance"] = HTBUtils.FormatCurrency(rec.TotalCharges - rec.TotalPaid, true);

                TotalInkassoForderung += rec.Forderung;
                TotalInkassoCharges += rec.TotalCharges;
                TotalInkassoPaid += rec.TotalPaid;

                dt.Rows.Add(dr);
            }
            if (isGegner)
                gvInkasso.Columns[4].Visible = false;
            if(isClient)
                gvInkasso.Columns[3].Visible = false;

            gvInkasso.DataSource = dt;
            gvInkasso.DataBind();
            trHeaderInkasso.Visible = gvInkasso.Rows.Count > 0;
        }
        #endregion

        #region Intervention Grid
        private void ClearInterventionGrid()
        {
            PopulateInterventionGrid(new ArrayList());
        }
        private void PopulateInterventionGrid(ArrayList list, bool isGegner = false, bool isClient = false, bool isAg = false)
        {
            TotalInterventionForderung = 0;
            TotalInterventionCharges = 0;
            TotalInterventionPaid = 0;

            DataTable dt = GetAktDataTableStructure();
            
            foreach (SearchAktLookup rec in list)
            {
                DataRow dr = dt.NewRow();

                dr["ID"] = rec.AktId.ToString();
                dr["AZ"] = rec.AktAZ;

                dr["AktEnteredDate"] = (rec.AktEnteredDate == HTBUtils.DefaultDate || rec.AktEnteredDate.ToShortDateString() == "01.01.0001") ? "" : rec.AktEnteredDate.ToShortDateString();
                dr["KlientName"] = rec.KlientName;
                if(!isGegner)
                    dr["GegnerInfo"] = rec.GegnerName + "<br/><br/>" + rec.GegnerAddress + "<br/>" + rec.GegnerCountry + " - " + rec.GegnerZip + " " + rec.GegnerCity;

                dr["AktStatus"] = rec.AktStatusCaption;
                dr["AktCurStatus"] = rec.AktCurStatusCaption;
                dr["Forderung"] = HTBUtils.FormatCurrency(rec.Forderung, true);
                dr["TotalCharges"] = HTBUtils.FormatCurrency(rec.TotalCharges, true);
                dr["TotalPaid"] = HTBUtils.FormatCurrency(rec.TotalPaid, true);
                dr["Balance"] = HTBUtils.FormatCurrency(rec.TotalCharges - rec.TotalPaid, true);

                TotalInterventionForderung += rec.Forderung;
                TotalInterventionCharges += rec.TotalCharges;
                TotalInterventionPaid += rec.TotalPaid;

                dt.Rows.Add(dr);
            }
            if (isGegner)
                gvIntervention.Columns[4].Visible = false;
            if(isClient)
                gvIntervention.Columns[3].Visible = false;

            gvIntervention.DataSource = dt;
            gvIntervention.DataBind();
            trHeaderIntervention.Visible = gvIntervention.Rows.Count > 0;
        }
        #endregion

        private void ShowFoundCounter(Label label, int startIdx, string counter)
        {
            int count = 0;
            try
            {
                count = Convert.ToInt32(counter);
            }
            catch
            {
            }
            if (count > 15)
                label.Text = "["+startIdx+" - "+(startIdx+PageSize)+" von " + count + "]";
            else
                label.Text = "[" + count + " von " + count + "]";
            
        }
        private void InitIndexes()
        {
            SetGegnerIndex(1);
            SetClientIndex(1);
            SetAgIndex(1);
        }
#region Getter / Setter
        private int GetGegnerIndex()
        {
            return GetIndex(hdnGegnerIdx);
        }
        private int GetClientIndex()
        {
            return GetIndex(hdnClientIdx);
        }
        private int GetAgIndex()
        {
            return GetIndex(hdnAgIdx);
        }
        private void SetGegnerIndex(int idx)
        {
            SetIndex(hdnGegnerIdx, idx);
        }
        private void SetClientIndex(int idx)
        {
            SetIndex(hdnClientIdx, idx);
        }
        private void SetAgIndex(int idx)
        {
            SetIndex(hdnAgIdx, idx);
        }
        private int GetIndex(HiddenField field)
        {
            int idx = GlobalUtilArea.GetZeroIfConvertToIntError(field.Value);
            if (idx == 0)
                idx = 1;
            return idx;
        }

        private void SetIndex(HiddenField field, int value)
        {
            if (value < 1) 
                value = 1;
            field.Value = value.ToString();
        }
#endregion
    }
}
