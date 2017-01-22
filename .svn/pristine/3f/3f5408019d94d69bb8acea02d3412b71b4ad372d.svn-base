using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
    public partial class CtlBrowserGegner : System.Web.UI.UserControl
    {
        private static int gegnerCount;
        private static string gegnerName = "";
        private static int startIndex = 0;
        private static int endIndex = 0;
        
        private static string _id = "";

        private static string _returnToURL = "";
        private static string _returnExtraParams = "";
        private static bool _returnGegner2;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.SEARCH_FOR]).Equals(string.Empty))
                {
                    txtGegnerName.Text = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.SEARCH_FOR]);
                    btnSubmit_Click(null, null);
                }
                else
                {
                    txtGegnerName.Text = "";
                    gegnerName = "";
                }
                _id = Request[GlobalHtmlParams.ID];
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            gegnerName = txtGegnerName.Text;
            gvGegner.DataBind();
            lblTotalGegnerMsg.Text = "Gegner " + startIndex + " bis " + endIndex + " von " + gegnerCount;
            txtGegnerName.Focus();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(GetReturnToUrl(_returnToURL, GlobalUtilArea.DecodeUrl(_returnExtraParams), null));
        }

        public void gvGegner_DataBound(object sender, EventArgs e)
        {
            lblTotalGegnerMsg.Text = "Gegner " + CtlBrowserGegner.startIndex + " bis " + CtlBrowserGegner.endIndex + " von " + CtlBrowserGegner.gegnerCount;
        }

        public List<GegnerLookup> GetGegner(int startIndex, int pageSize, string sortBy, ref int totalGegner)
        {
            var parameters = new ArrayList();
            parameters.Add(new StoredProcedureParameter("@startIndex", SqlDbType.Int, startIndex));
            parameters.Add(new StoredProcedureParameter("@pageSize", SqlDbType.Int, pageSize));
            parameters.Add(new StoredProcedureParameter("@sortBy", SqlDbType.Int,sortBy));
            parameters.Add(new StoredProcedureParameter("@totalGegner", SqlDbType.Int, totalGegner, ParameterDirection.Output));

            if (gegnerName != null && gegnerName.Trim() != string.Empty)
                parameters.Add(new StoredProcedureParameter("@name", SqlDbType.NVarChar, gegnerName));
            
            ArrayList list = HTBUtils.GetStoredProcedureRecords("spSearchGegner", parameters, typeof(GegnerLookup));
            foreach (GegnerLookup gegner in list)
            {
                gegner.GegnerAddress = gegner.GegnerZipPrefix + " - " + gegner.GegnerStrasse + "<br/>" + gegner.GegnerOrt;
                /* Both addresses are the same
                gegner.GegnerLastAddress = gegner.GegnerLastZipPrefix + " - " + gegner.GegnerLastStrasse + "<br/>" + gegner.GegnerLastOrt;
                if (HTBUtils.RemoveAllSpecialChars(gegner.GegnerAddress, true).Equals(HTBUtils.RemoveAllSpecialChars(gegner.GegnerLastAddress, true)))
                {
                    gegner.GegnerLastAddress = "&nbsp;";
                }
                 */
                if (gegner.GegnerType >= 0)
                {
                    gegner.GegnerNameLink = "<a href=\"" + GetReturnToUrl(_returnToURL, _returnExtraParams, gegner) + "\">" + gegner.GegnerName + "</a>";
                }
                else
                {
                    var sb = new StringBuilder();
                    sb.Append(gegner.GegnerName);
                    sb.Append("&nbsp;&nbsp;&nbsp;<a href=\"../gegner/EditGegner.aspx?ID=");
                    sb.Append(gegner.GegnerID);
                    GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT, GlobalHtmlParams.URL_NEW_AKT);
                    GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL, GlobalHtmlParams.URL_BROWSER_GEGNER);
                    GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.SEARCH_FOR, gegnerName); 
                    GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.EXTRA_PARAMS, GlobalUtilArea.EncodeURL(_returnExtraParams));
                    
                    sb.Append("\"><strong>Reparieren</strong></a>");
                    gegner.GegnerNameLink = sb.ToString();
                }
            }
            foreach (object o in parameters)
            {
                if (o is ArrayList)
                {
                    var outputList = (ArrayList)o;
                    foreach (StoredProcedureParameter p in outputList)
                    {
                        if (p.Name.IndexOf("totalGegner") >= 0)
                        {
                            totalGegner = Convert.ToInt32(p.Value);
                        }
                    }
                }
            }
            CtlBrowserGegner.startIndex = startIndex + 1;
            CtlBrowserGegner.endIndex = startIndex + list.Count;
            return list.Cast<GegnerLookup>().ToList();
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<GegnerLookup> GetAllGegner(int startIndex, int pageSize, string sortBy)
        {

            List<GegnerLookup> result;
            int totalGegner=0;
            if (string.IsNullOrEmpty(sortBy))
                sortBy = "GegnerName1";
            result = GetGegner(startIndex, pageSize, sortBy, ref totalGegner);
            gegnerCount = totalGegner;
            return result;
        }

        private string GetReturnToUrl(string returnToUrlCode, string returnExtraParams, GegnerLookup gegner)
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
                AppendURLParams(sb, gegner, returnExtraParams);
                return sb.ToString();
            }
            return "";
        }
        private void AppendURLParams(StringBuilder sb, GegnerLookup gegner, string returnExtraParameters)
        {   
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, _id);
            if (gegner != null)
            {
                if(sb.Length > 0)
                    sb.Append("&");
                sb.Append(_returnGegner2 ? GlobalHtmlParams.GEGNER2_ID : GlobalHtmlParams.GEGNER_ID);
                sb.Append("=");
                sb.Append(gegner.GegnerID.ToString());
            }
            if (!string.IsNullOrEmpty(returnExtraParameters))
            {
                if (gegner != null)
                    sb.Append("&");
                sb.Append(HttpUtility.UrlPathEncode(returnExtraParameters));
            }
        }

        public int GetTotalGegnerCount()
        {
            return gegnerCount;
        }

        public void SetReturnToURL(string ret)
        {
            _returnToURL = ret;
        }
        public void SetReturnExtraParams(string ret)
        {
            _returnExtraParams = ret;
        }
        public void SetReturnGegner2(bool ret)
        {
            _returnGegner2 = ret;
        }
    }
}