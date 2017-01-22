using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.dv
{
    public partial class DVRedirect : System.Web.UI.Page
    {
        const string LoginURL = "https://www.deltavista-online.at/";
        private const string DVRedirectURL = "/v2/intranetx/dv/DVRedirect.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            string username = HTBUtils.GetConfigValue("DV_User_" + Request["UserID"]);
            string password = HTBUtils.GetConfigValue("DV_Pass_" + Request.Params[GlobalHtmlParams.USER_ID]);

            if(string.IsNullOrEmpty(username))
            {
                username = HTBUtils.GetConfigValue("DV_User");
            }
            if (string.IsNullOrEmpty(password))
            {
                password = HTBUtils.GetConfigValue("DV_Pass");
            }
            
            // first, request the login form to get the viewstate value
            var webRequest = WebRequest.Create(LoginURL) as HttpWebRequest;
            var responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
            string responseData = responseReader.ReadToEnd();
            responseReader.Close();

            // extract the viewstate value and build out POST data
            string viewState = ExtractViewState(responseData);
            string postData =
                String.Format(
                    "__VIEWSTATE={0}&txtName={1}&txtPasswort={2}&cmdOK=OK&service=direct/0/Login/loginForm&sp=S0&Form0=txtName,txtPasswort",
                    viewState, username, password
                    );

            // have a cookie container ready to receive the forms auth cookie
            var cookies = new CookieContainer();

            // now post to the login form
            webRequest = WebRequest.Create(LoginURL) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.CookieContainer = cookies;

            // write the form values into the request message
            var requestWriter = new StreamWriter(webRequest.GetRequestStream());
            requestWriter.Write(postData);
            requestWriter.Close();

            // we don't need the contents of the response, just the cookie it issues
            webRequest.GetResponse().Close();

            // now we can send out cookie along with a request for the protected page
            webRequest = WebRequest.Create(GlobalUtilArea.DecodeUrl(Request["URL"])) as HttpWebRequest;
            webRequest.CookieContainer = cookies;
            responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream(), Encoding.Default);

            // and read the response
            responseData = responseReader.ReadToEnd();
            responseReader.Close();
            Response.Write(ConvertForHTB(responseData));

//            Response.Write("<br/>DV_User_" + Request[GlobalHtmlParams.USER_ID] + " = " + HTBUtils.GetConfigValue("DV_User_" + Request[GlobalHtmlParams.USER_ID]));
//            Response.Write("<br/>DV_Pass_" + Request[GlobalHtmlParams.USER_ID] + " = " + HTBUtils.GetConfigValue("DV_Pass_" + Request[GlobalHtmlParams.USER_ID]));
//
//            Response.Write("<br/>User = " + username);
//            Response.Write("<br/>Pass = " + password);
        }

        private string ExtractViewState(string s)
        {
            string viewStateNameDelimiter = "__VIEWSTATE";
            string valueDelimiter = "value=\"";
            try
            {
                int viewStateNamePosition = s.IndexOf(viewStateNameDelimiter);
                int viewStateValuePosition = s.IndexOf(
                    valueDelimiter, viewStateNamePosition
                    );

                int viewStateStartPosition = viewStateValuePosition +
                                             valueDelimiter.Length;
                int viewStateEndPosition = s.IndexOf("\"", viewStateStartPosition);

                return HttpUtility.UrlEncodeUnicode(
                    s.Substring(
                        viewStateStartPosition,
                        viewStateEndPosition - viewStateStartPosition
                        )
                    );
            }
            catch
            {
                return "";
            }
        }


        private string ConvertForHTB(string data)
        {
            data = ReplaceShowDetail(data);
            data = GoToAddressDetails(data);
            data = ReplaceOK_App(data);
            string dir = "/v2/intranetx/dv/";
            return data.Replace("/css/", dir)
                .Replace("/images/", dir)
                .Replace("/js/", dir)
                .Replace("target=\"Overview\"", "")
                .Replace("../de/adr/AdressDetails.asp?mainID=",
                         DVRedirectURL + "?URL=" +
                         GlobalUtilArea.EncodeURL("https://www.deltavista-online.at/de/adr/AdressDetails.asp?mainId="));

        }

        private string ReplaceShowDetail(string data)
        {
            string strToReplace = "onclick=\"showDetail(";
            int idx = data.IndexOf(strToReplace);
            
            if (idx < 0)
                return data;
            
            int idx2 = data.IndexOf(">", idx);
            int idx3 = data.IndexOf("</td>", idx);
            
            var sb = new StringBuilder();
            
            sb.Append(data.Substring(0, idx));
            sb.Append("> <a href=\""+DVRedirectURL+"?URL=" + GlobalUtilArea.EncodeURL("https://www.deltavista-online.at/de/adr/AdressDetails.asp?mainId="));

            sb.Append(data.Substring(idx + strToReplace.Length, idx2 - (idx + strToReplace.Length) - 2));
            sb.Append("#\">");
            sb.Append(data.Substring(idx2+1, idx3 - idx2-1));
            sb.Append("</a>");
            sb.Append(data.Substring(idx3));
            return ReplaceShowDetail(sb.ToString());
        }

        private string ReplaceOK_App(string data)
        {
            string strToReplace = "/oks/app?";
            int idx = data.IndexOf(strToReplace);
            if (idx < 0)
                return data;

            int idx2 = data.IndexOf("'", idx);
            int idx3 = data.IndexOf("\"", idx);
            if (idx3 < idx2 || (idx2 < 0 && idx3 > 0)) 
                idx2 = idx3;
            var sb = new StringBuilder();
            sb.Append(data.Substring(0, idx));

            sb.Append(DVRedirectURL+"?URL=" + GlobalUtilArea.EncodeURL("https://www.deltavista-online.at/oks/app?"));
            sb.Append(data.Substring(idx2));
            return ReplaceOK_App(sb.ToString());
        }

        private string GoToAddressDetails(string data)
        {
            int idx = data.IndexOf("Adressdetails");
            if (idx < 0)
                return data;

            const string paramText = "Director.parms = \"";
            int paramsIdx = data.IndexOf(paramText);
            if (paramsIdx < 0)
                return data;

            int idx2 = data.IndexOf("\"", paramsIdx + paramText.Length);
            string prms = data.Substring(paramsIdx + paramText.Length, idx2 - (paramsIdx + paramText.Length) - 1);
            string addressURL = "https://www.deltavista-online.at/oks/app?service=external/MenuCommandDispatcher&Key=70&adm=0"+prms;
            Response.Redirect(DVRedirectURL + "?URL=" + GlobalUtilArea.EncodeURL(addressURL));
            return "";
        }
    }
}