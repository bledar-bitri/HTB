using System;
using HTB.v2.intranetx.util;
using System.Collections;
using HTBUtilities;
using HTB.Database.Views;
using System.Text;
using System.Net;
using System.IO;

namespace HTB.v2.intranetx.routeplaner.googlemaps
{
    public partial class RoutePlaner : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        #region Event Handler
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string aktIdList = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.ID]);
            if (aktIdList.Trim().Length > 0)
            {
                string qryAktCommand = "SELECT * FROM dbo.qryAktenInt WHERE AktIntID in (" + aktIdList + ")";
                ArrayList aktenList = HTBUtils.GetSqlRecords(qryAktCommand, typeof(qryAktenInt));

                WebRequest webRequest = WebRequest.Create(GetJSonRequest(aktenList)) as HttpWebRequest;
                StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream(), Encoding.Default);

                // and read the response
                string responseData = responseReader.ReadToEnd();
                responseReader.Close();

                Response.Write(responseData);
            }
        }
        #endregion

        public string GetJSonRequest(ArrayList akten)
        {
            return GetGoogleMapsRequest(new StringBuilder(HTBUtils.GetConfigValue("GoogleMapsJSonURL")), akten);
        }

        public string GetGoogleMapsRequest(StringBuilder sb, ArrayList akten)
        {
            string gwayPointSeparator = HTBUtils.GetConfigValue("GoogleMapsWaypointSeparator");
            sb.Append("sensor=false&origin=");
            sb.Append(string.IsNullOrEmpty(txtStart.Text) ? "Swartzparkstrasse+15,5020,Salzburg,Austria" : txtStart.Text);
            sb.Append("&destination=");
            sb.Append(string.IsNullOrEmpty(txtEnd.Text) ? "Swartzparkstrasse+15,5020,Salzburg,Austria" : txtEnd.Text);
            sb.Append("&waypoints=optimize:true");
            sb.Append(gwayPointSeparator);
            foreach (qryAktenInt akt in akten)
            {
                sb.Append(akt.GegnerLastStrasse);
                sb.Append(",");
                sb.Append(akt.GegnerLastZip);
                sb.Append(",");
                sb.Append(akt.GegnerLastOrt);
                sb.Append(",österreich");
                sb.Append(gwayPointSeparator);
            }
            return sb.ToString();
        }
    }
}