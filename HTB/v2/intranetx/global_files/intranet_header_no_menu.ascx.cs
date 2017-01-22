﻿using System;
using HTB.v2.intranetx.util;

namespace HTB.intranetx.global_files
{
    public partial class intranet_header_no_menu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GlobalUtilArea.ValidateSession(Session, Response);
            int wuserAuthorization = -1;
            try {
                wuserAuthorization = Convert.ToInt32(Session["MM_UserAuthorization"]);
            }
            catch {}
            if (wuserAuthorization < 10)
            {
                divUserInfo.InnerHtml = " Nicht angemeldet.";
            }
            else
            {
                divUserInfo.InnerHtml = "<b>" + Session["MM_Vorname"] + 
                    " " + Session["MM_Nachname"] + "</b> [" + Session["MM_Levelname"] + "]" + 
                    "<a href=\"#\" onClick=\"MM_openBrWindow('/v2/intranet/faq.asp','popWindow','menubar=yes,resizable=yes,width=800,height=600')\">FAQ</a> "+
                    "| Forum &nbsp;| <a href=\"http://80.120.63.157/wiki/index.php?title=Hauptseite\" target=\"_blank\">WIKI</a> | Helpdesk  | &nbsp;"+
                    "<a href=\"#\" onClick=\"MM_openBrWindow('/v2/intranet/user/popwhoisonline.asp','popWindow','menubar=yes,scrollbars=yes,resizable=yes,width=640,height=480')\">" +
                    "wer ist online?</a><br><a href=\"/v2/intranet/login/logout.asp\">abmelden</a>";
            }
            divTime.InnerHtml = "<br>"+DateTime.Now;
        }
    }
}