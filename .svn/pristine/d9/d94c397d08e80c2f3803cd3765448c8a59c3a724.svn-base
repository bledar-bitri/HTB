using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using HTB.v2.intranetx.util;
using HTBUtilities;
using HTB.Database;
using System.Data;
using HTB.Database.LookupRecords;
using System.ComponentModel;
using System.Text;

namespace HTB.v2.intranetx.global_files
{
    public partial class CtlBrowserClient : System.Web.UI.UserControl
    {
        private static int _clientsCount;
        private static string _clientName = "";
        private static string _clientType = GlobalUtilArea.DefaultDropdownValue;
        private static int _startIndex;
        private static int _endIndex;
        private static string _returnToURL = "";
        private static string _returnExtraParams = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GlobalUtilArea.LoadDropdownList(ddlClientType,
                    "SELECT * FROM tblKlientType ORDER BY KlientTypeCaption ASC",
                    typeof(tblKlientType),
                    "KlientTypeId",
                    "KlientTypeCaption",
                    true
                    );
                txtClientName.Text = "";
                _clientName = "";
                _clientType = GlobalUtilArea.DefaultDropdownValue;
                SetSelectedClientType(GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.SELECTED_VALUE]));
                _clientType = ddlClientType.SelectedValue;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            _clientName = txtClientName.Text;
            _clientType = ddlClientType.SelectedValue;
            gvClients.DataBind();
            lblTotalClientsMsg.Text = "Klienten " + _startIndex + " bis " + _endIndex + " von " + _clientsCount;
            txtClientName.Focus();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(GetReturnToUrl(_returnToURL, GlobalUtilArea.DecodeUrl(_returnExtraParams), null));
        }

        public void gvClients_DataBound(object sender, EventArgs e)
        {
            lblTotalClientsMsg.Text = "Klienten " + _startIndex + " bis " + _endIndex + " von " + _clientsCount;
        }

        public List<KlientLookup> GetClients(int startIndex, int pageSize, string sortBy, ref int totalClients)
        {
            var parameters = new ArrayList
                                 {
                                     new StoredProcedureParameter("@startIndex", SqlDbType.Int, startIndex),
                                     new StoredProcedureParameter("@pageSize", SqlDbType.Int, pageSize),
                                     new StoredProcedureParameter("@sortBy", SqlDbType.Int, sortBy),
                                     new StoredProcedureParameter("@totalKlients", SqlDbType.Int, totalClients, ParameterDirection.Output)
                                 };

            if (_clientName != null && _clientName.Trim() != string.Empty)
                parameters.Add(new StoredProcedureParameter("@name", SqlDbType.NVarChar, _clientName));
            if (_clientType != GlobalUtilArea.DefaultDropdownValue)
                parameters.Add(new StoredProcedureParameter("@type", SqlDbType.Int, _clientType));

            ArrayList list = HTBUtils.GetStoredProcedureRecords("spSearchClients", parameters, typeof(KlientLookup));
            foreach (KlientLookup client in list)
            {
                client.KlientNameLink = GetReturnToUrl(_returnToURL, _returnExtraParams, client);
            }
            foreach (object o in parameters)
            {
                if (o is ArrayList)
                {
                    var outputList = (ArrayList)o;
                    foreach (StoredProcedureParameter p in outputList)
                    {
                        if (p.Name.IndexOf("totalKlients") >= 0)
                        {
                            totalClients = Convert.ToInt32(p.Value);
                        }
                    }
                }
            }
            _startIndex = startIndex+1;
            _endIndex = startIndex+list.Count;
            return list.Cast<KlientLookup>().ToList();
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<KlientLookup> GetAllClients(int startIndex, int pageSize, string sortBy)
        {

            List<KlientLookup> result;
            int totalClients=0;
            if (string.IsNullOrEmpty(sortBy))
                sortBy = "KlientName1";
            result = GetClients(startIndex, pageSize, sortBy, ref totalClients);
            _clientsCount = totalClients;
            return result;
        }

        private string GetReturnToUrl(string returnToUrlCode, string returnExtraParams, KlientLookup client)
        {
            switch (returnToUrlCode)
            {
                case GlobalHtmlParams.URL_NEW_AKT:
                    StringBuilder sb = new StringBuilder();
                    sb.Append("../aktenink/NewAktInk.aspx?");
                    if (client != null)
                    {
                        sb.Append(GlobalHtmlParams.CLIENT_ID);
                        sb.Append("=");
                        sb.Append(client.KlientID.ToString());

                    }
                    if (returnExtraParams != "")
                    {
                        if (client != null)
                            sb.Append("&");
                        sb.Append(HttpUtility.UrlPathEncode(returnExtraParams));
                    }

                    return sb.ToString();
            }
            return "";
        }

        public int GetTotalClientsCount()
        {
            return _clientsCount;
        }

        public void SetReturnToURL(string ret)
        {
            _returnToURL = ret;
        }
        public void SetReturnExtraParams(string ret)
        {
            _returnExtraParams = ret;
        }

        private void SetSelectedClientType(string type)
        {
            if(!string.IsNullOrEmpty(type))
                ddlClientType.SelectedValue = type;
        }
    }
}