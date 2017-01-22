using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections;
using HTB.v2.intranetx.util;
using HTBUtilities;
using HTB.Database;
using System.Data;
using HTB.Database.LookupRecords;
using System.ComponentModel;
using System.Text;
using HTB.Database.HTB.LookupRecords;

namespace HTB.v2.intranetx.global_files
{
    public partial class CtlBrowserDealer : UserControl
    {
        private static int dealerCount;
        private static string dealerName = "";
        private static int startIndex = 0;
        private static int endIndex = 0;

        private static string _id = "";

        private static string returnToURL = "";
        private static string returnExtraParams = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.SEARCH_FOR]).Equals(string.Empty))
                {
                    txtDealerName.Text = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.SEARCH_FOR]);
                    btnSubmit_Click(null, null);
                }
                else
                {
                    txtDealerName.Text = "";
                    dealerName = "";
                }
                _id = Request[GlobalHtmlParams.ID];
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            dealerName = txtDealerName.Text;
            gvDealer.DataBind();
            lblTotalDealerMsg.Text = "Handler " + startIndex + " bis " + endIndex + " von " + dealerCount;
            txtDealerName.Focus();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(GetReturnToUrl(returnToURL, GlobalUtilArea.DecodeUrl(returnExtraParams), null));
        }

        public void gvGegner_DataBound(object sender, EventArgs e)
        {
            lblTotalDealerMsg.Text = "Handler " + startIndex + " bis " + endIndex + " von " + dealerCount;
        }

        public List<AutoDealerLookup> GetDealer(int startIndex, int pageSize, string sortBy, ref int totalDealers)
        {
            var parameters = new ArrayList
                                 {
                                     new StoredProcedureParameter("startIndex", SqlDbType.Int, startIndex),
                                     new StoredProcedureParameter("pageSize", SqlDbType.Int, pageSize),
                                     new StoredProcedureParameter("sortBy", SqlDbType.Int, sortBy),
                                     new StoredProcedureParameter("totalDealer", SqlDbType.Int, totalDealers, ParameterDirection.Output)
                                 };

            if (dealerName != null && dealerName.Trim() != string.Empty)
                parameters.Add(new StoredProcedureParameter("@name", SqlDbType.NVarChar, dealerName));

            ArrayList list = HTBUtils.GetStoredProcedureRecords("spSearchAutoDealer", parameters, typeof(AutoDealerLookup));
            foreach (AutoDealerLookup dealer in list)
            {
                dealer.DealerAddress = dealer.AutoDealerLKZ + " - " + dealer.AutoDealerStrasse+ "<br/>" + dealer.AutoDealerOrt;
                dealer.DealerNameLink = "<a href=\"" + GetReturnToUrl(returnToURL, returnExtraParams, dealer) + "\">" + dealer.AutoDealerName + "</a>";
                
            }
            foreach (object o in parameters)
            {
                if (o is ArrayList)
                {
                    var outputList = (ArrayList)o;
                    foreach (StoredProcedureParameter p in outputList)
                    {
                        if (p.Name.IndexOf("totalDealer") >= 0)
                        {
                            totalDealers = Convert.ToInt32(p.Value);
                        }
                    }
                }
            }
            CtlBrowserDealer.startIndex = startIndex + 1;
            CtlBrowserDealer.endIndex = startIndex + list.Count;
            return list.Cast<AutoDealerLookup>().ToList();
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<AutoDealerLookup> GetAllDealers(int startIndex, int pageSize, string sortBy)
        {
            int totalDealers=0;
            if (string.IsNullOrEmpty(sortBy))
                sortBy = "AutoDealerName";
            List<AutoDealerLookup> result = GetDealer(startIndex, pageSize, sortBy, ref totalDealers);
            dealerCount = totalDealers;
            return result;
        }

        private string GetReturnToUrl(string returnToUrlCode, string returnExtraParams, AutoDealerLookup dealer)
        {
            var sb = new StringBuilder();
            switch (returnToUrlCode)
            {
                case GlobalHtmlParams.URL_NEW_AKT:
                    sb.Append("../aktenink/NewAktInk.aspx?");
                    break;
                case GlobalHtmlParams.URL_NEW_AKT_INT_AUTO:
                    sb.Append("../aktenint/EditAktIntAuto.aspx?");
                    break;
            }
            if(!sb.ToString().Trim().Equals(""))
            {
                AppendURLParams(sb, dealer, returnExtraParams);
                return sb.ToString();
            }
            return "";
        }
        private void AppendURLParams(StringBuilder sb, AutoDealerLookup gegner, string returnExtraParams)
        {
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, _id);
            if (gegner != null)
            {
                if(sb.Length > 0)
                    sb.Append("&");
                sb.Append(GlobalHtmlParams.DEALER_ID);
                sb.Append("=");
                sb.Append(gegner.AutoDealerID.ToString());

            }
            if (returnExtraParams != "")
            {
                if (gegner != null)
                    sb.Append("&");
                sb.Append(HttpUtility.UrlPathEncode(returnExtraParams));
            }
        }

        public int GetTotalDealerCount()
        {
            return dealerCount;
        }

        public void SetReturnToURL(string ret)
        {
            returnToURL = ret;
        }
        public void SetReturnExtraParams(string ret)
        {
            returnExtraParams = ret;
        }
    }
}