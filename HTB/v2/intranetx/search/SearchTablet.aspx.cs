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
using HTB.v2.intranetx.util;
using HTBExtras.XML;
using HTBUtilities;

namespace HTB.v2.intranetx.search
{
    public partial class SearchTablet : Page
    {
        private const string SessionSearchName = "Search_Record";
        private int GegnerId;
        private int LookupName;
        private double _totalInkassoForderung = 0;
        private double _totalInkassoCharges = 0;
        private double _totalInkassoPaid = 0;

        private double _totalInterventionForderung = 0;
        private double _totalInterventionCharges = 0;
        private double _totalInterventionPaid = 0;
        private tblGegner _currentGegner = null;
        private int _totalGegner = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            GegnerId = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.GEGNER_ID]);
            if (GegnerId <= 0)
            {
                var searchRec = new SearchRecord {SearchName = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.NAME])};
                ArrayList gegnerList = SearchGegner(searchRec);
                var res = new XmlSearchResponseRecord {TotalGegnerFound = _totalGegner};
                res.SetGegnerDetailList(gegnerList.Cast<GegnerDetailLookup>().ToList());
                sb.Append(res.ToXmlString());
            }
            Response.Write(sb.ToString());
        }

        private ArrayList SearchGegner(SearchRecord searchRec)
        {
            var parameters = new ArrayList
                                    {
                                        new StoredProcedureParameter("countGegner", SqlDbType.Int, 1),
                                        new StoredProcedureParameter("totalGegner", SqlDbType.Int, 0, ParameterDirection.Output),
                                        new StoredProcedureParameter("name", SqlDbType.NVarChar, searchRec.SearchName)
                                    };

            ArrayList list = HTBUtils.GetStoredProcedureRecords("spSearchDetailTablet", parameters, typeof(GegnerDetailLookup));

            foreach (Object obj in parameters)
            {
                if (obj is ArrayList)
                {
                    var outParams = (ArrayList)obj;
                    foreach (StoredProcedureParameter outParam in outParams)
                    {
                        if (outParam.Name == "totalGegner")
                        {
                            try
                            {
                                _totalGegner = Convert.ToInt32(outParam.Value.ToString());
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            return list;
        }
        /*
        private void LookupGegner()
        {
            var parameters = new ArrayList
                                    {
                                        new StoredProcedureParameter("@gegnerID", SqlDbType.Int, GegnerId)
                                    };

            ArrayList[] lists = HTBUtils.GetMultipleListsFromStoredProcedure("spGetAllAktsByGegnerID", parameters, new Type[] { typeof(tblGegner), typeof(tblGegnerAdressen), typeof(tblGegnerPhone), typeof(SearchAktLookup), typeof(SearchAktLookup) });
            try
            {
                _currentGegner = (tblGegner)lists[0][0];
                if (_currentGegner != null)
                {
                    ctlMessage.ShowInfo(_currentGegner.GegnerName2 + " " + _currentGegner.GegnerName1 + "<br/><br/>" + _currentGegner.GegnerLastStrasse + "<br/>" + _currentGegner.GegnerLastZipPrefix + " - " + _currentGegner.GegnerLastZip + " " + _currentGegner.GegnerOrt);
                    if (!string.IsNullOrEmpty(_currentGegner.GegnerMemo))
                        lblGegnerMemo.Text = "<b>Schuldner Memo:</b><BR/>" + _currentGegner.GegnerMemo + "<BR/>";
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
                txtAkt.Text = searchRec.SearchNumber > 0 ? searchRec.SearchNumber.ToString() : "";
            }
        }

        protected string GetTotalInkassoForderung()
        {
            return HTBUtils.FormatCurrency(_totalInkassoForderung, true);
        }
        protected string GetTotalInkassoCharges()
        {
            return HTBUtils.FormatCurrency(_totalInkassoCharges, true);
        }
        protected string GetTotalInkassoPaid()
        {
            return HTBUtils.FormatCurrency(_totalInkassoPaid, true);
        }
        protected string GetTotalInkassoBalance()
        {
            return HTBUtils.FormatCurrency(_totalInkassoCharges - _totalInkassoPaid, true);
        }


        protected string GetTotalInterventionForderung()
        {
            return HTBUtils.FormatCurrency(_totalInterventionForderung, true);
        }
        protected string GetTotalInterventionCharges()
        {
            return HTBUtils.FormatCurrency(_totalInterventionCharges, true);
        }
        protected string GetTotalInterventionPaid()
        {
            return HTBUtils.FormatCurrency(_totalInterventionPaid, true);
        }
        protected string GetTotalInterventionBalance()
        {
            return HTBUtils.FormatCurrency(_totalInterventionCharges - _totalInterventionPaid, true);
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
        private void PopulateGegnerGrid(ArrayList list)
        {
            DataTable dt = GetDataTableStructure();
            string currentName = "";
            foreach (GegnerDetailLookup rec in list)
            {
                DataRow dr = dt.NewRow();

                dr["ID"] = rec.GegnerID.ToString();
                dr["OldID"] = rec.GegnerOldID;
                if (rec.GegnerName.ToLower() == currentName)
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
            if (_currentGegner != null && !string.IsNullOrEmpty(_currentGegner.GegnerPhone))
            {
                DataRow dr = dt.NewRow();
                dr["PhoneType"] = "";

                dr["PhoneCity"] = _currentGegner.GegnerPhoneCountry + " " + _currentGegner.GegnerPhoneCity;
                dr["Phone"] = _currentGegner.GegnerPhone;
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

        #region CollectionInvoice Grid
        private void ClearInkassoGrid()
        {
            PopulateInkassoGrid(new ArrayList());
        }
        private void PopulateInkassoGrid(ArrayList list, bool isGegner = false)
        {
            _totalInkassoForderung = 0;
            _totalInkassoCharges = 0;
            _totalInkassoPaid = 0;

            DataTable dt = GetAktDataTableStructure();

            foreach (SearchAktLookup rec in list)
            {
                DataRow dr = dt.NewRow();

                dr["ID"] = rec.AktId.ToString();
                dr["AZ"] = rec.AktAZ;

                dr["AktEnteredDate"] = (rec.AktEnteredDate == HTBUtils.DefaultDate || rec.AktEnteredDate.ToShortDateString() == "01.01.0001") ? "" : rec.AktEnteredDate.ToShortDateString();
                dr["KlientName"] = rec.KlientName;
                dr["GegnerInfo"] = rec.GegnerName + "<br/><br/>" + rec.GegnerAddress + "<br/>" + rec.GegnerCountry + " - " + rec.GegnerZip + " " + rec.GegnerCity;
                dr["AktStatus"] = rec.AktStatusCaption + "<BR/>" + rec.AktCurStatusCaption;
                dr["Forderung"] = HTBUtils.FormatCurrency(rec.Forderung, true);
                dr["TotalCharges"] = HTBUtils.FormatCurrency(rec.TotalCharges, true);
                dr["TotalPaid"] = HTBUtils.FormatCurrency(rec.TotalPaid, true);
                dr["Balance"] = HTBUtils.FormatCurrency(rec.TotalCharges - rec.TotalPaid, true);

                _totalInkassoForderung += rec.Forderung;
                _totalInkassoCharges += rec.TotalCharges;
                _totalInkassoPaid += rec.TotalPaid;

                dt.Rows.Add(dr);
            }
            if (isGegner)
                gvInkasso.Columns[4].Visible = false;

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
        private void PopulateInterventionGrid(ArrayList list, bool isGegner = false)
        {
            _totalInterventionForderung = 0;
            _totalInterventionCharges = 0;
            _totalInterventionPaid = 0;

            DataTable dt = GetAktDataTableStructure();

            foreach (SearchAktLookup rec in list)
            {
                DataRow dr = dt.NewRow();

                dr["ID"] = rec.AktId.ToString();
                dr["AZ"] = rec.AktAZ;

                dr["AktEnteredDate"] = (rec.AktEnteredDate == HTBUtils.DefaultDate || rec.AktEnteredDate.ToShortDateString() == "01.01.0001") ? "" : rec.AktEnteredDate.ToShortDateString();
                dr["KlientName"] = rec.KlientName;
                if (!isGegner)
                    dr["GegnerInfo"] = rec.GegnerName + "<br/><br/>" + rec.GegnerAddress + "<br/>" + rec.GegnerCountry + " - " + rec.GegnerZip + " " + rec.GegnerCity;

                dr["AktStatus"] = rec.AktStatusCaption;
                dr["AktCurStatus"] = rec.AktCurStatusCaption;
                dr["Forderung"] = HTBUtils.FormatCurrency(rec.Forderung, true);
                dr["TotalCharges"] = HTBUtils.FormatCurrency(rec.TotalCharges, true);
                dr["TotalPaid"] = HTBUtils.FormatCurrency(rec.TotalPaid, true);
                dr["Balance"] = HTBUtils.FormatCurrency(rec.TotalCharges - rec.TotalPaid, true);

                _totalInterventionForderung += rec.Forderung;
                _totalInterventionCharges += rec.TotalCharges;
                _totalInterventionPaid += rec.TotalPaid;

                dt.Rows.Add(dr);
            }
            if (isGegner)
                gvIntervention.Columns[4].Visible = false;

            gvIntervention.DataSource = dt;
            gvIntervention.DataBind();
            trHeaderIntervention.Visible = gvIntervention.Rows.Count > 0;
        }
        #endregion

        private void ShowFoundCounter(Label label, string counter)
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
                label.Text = "[15 von " + count + ".] Bitte definieren Sie ihre Suche genauer.";
            else
                label.Text = "[" + count + " von " + count + "]";
        }
          */    
    }
}