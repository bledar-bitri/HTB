using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.v2.intranetx.util;
using HTBUtilities;
using System.Collections;
using HTB.Database;
using System.IO;
using Ionic.Zip;
using System.Text;
using HTB.Database.Views;

namespace HTB.v2.intranetx.aktenint
{
    public partial class WorkReportInfoScore : System.Web.UI.Page
    {
        DateTime startDate;
        DateTime endDate;
        string paternFile = HTBUtils.GetConfigValue("Work_Infoscore_Patern_File");
        StringBuilder sb = new StringBuilder();
        ZipFile zip = new ZipFile();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["startDate"] != null && !Request["startDate"].Equals(""))
                {
                    txtDateStart.Text = GlobalUtilArea.GetDefaultDateIfConvertToDateError(Request["startDate"].ToString()).ToShortDateString();
                }
                if (Request["endDate"] != null && !Request["endDate"].Equals(""))
                {
                    txtDateEnd.Text = GlobalUtilArea.GetDefaultDateIfConvertToDateError(Request["endDate"].ToString()).ToShortDateString();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            startDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateStart.Text);
            endDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateEnd.Text);
            GeneratePages();
        }

        private void GeneratePages()
        {
            //ArrayList users = HTBUtils.GetSqlRecords("SELECT * FROM tblUser WHERE UserStatus = 1 and UserAbteilung = 1", typeof(tblUser));
            ArrayList users = HTBUtils.GetSqlRecords("SELECT * FROM tblUser", typeof(tblUser));
            foreach (tblUser u in users)
            {
                GeneratePages(u);
            }
            ZipAndShipFiles();
        }

        private void GeneratePages(tblUser user)
        {
            string select = "SELECT * FROM qryWorkreportInfoscore ";
            string where = " WHERE AktIntActionDate BETWEEN '" + startDate.ToShortDateString() + "' AND '" + endDate.ToShortDateString() + "' AND UserID = "+user.UserID;
            string order = " ORDER BY AktIntAZ ASC";

            ArrayList list = HTBUtils.GetSqlRecords(select + where+order, typeof(qryWorkreportInfoscore));
            int counter =  1;
            if (list.Count > 0)
            {
                string fileName = "ECP-" + user.UserVorname + "_" + user.UserNachname + " " + DateTime.Now.ToShortDateString() + ".xls";

                OpenExcelFile(fileName);
                WriteHeader(user, fileName);
                foreach (qryWorkreportInfoscore action in list)
                {
                    string strINTCODE = "XXXX";
                    string strBC = "XXXX";
                    string strBERICHT = "falsche Eingabe!";
                    double price = 0;
                    string strPrice = "";

                    switch (action.AktIntActionType)
                    {
                        case 52:
                            strINTCODE = "NE001";
                            strBC = "unb";
                            strBERICHT = "SU ist unbekannt";
                            price = 3;
                            break;
                        case 53:
                            strINTCODE = "NE002";
                            strBC = "na";
                            strBERICHT = "SU nicht angetroffen";
                            price = 0;
                            break;
                        case 54:
                            strINTCODE = "NE003";
                            strBC = "esn";
                            strBERICHT = "SU hält Termin nicht ein";
                            price = 3;
                            break;
                        case 55:
                            strINTCODE = "NE006";
                            strBC = "unein";
                            strBERICHT = "Forderung uneinbringlich";
                            price = 3;
                            break;
                        case 56:
                            strINTCODE = "NE008";
                            strBC = "so";
                            strBERICHT = "sonstiges";
                            price = 3;
                            break;
                        case 57:
                            strINTCODE = "NE010";
                            strBC = "er";
                            strBERICHT = "Erhebung";
                            price = 5;
                            break;
                        case 58:
                        case 66:
                            strINTCODE = "NE011";
                            strBC = "rv";
                            strBERICHT = "Ratenvereinbarung";
                            if (action.AktIntActionType == 58)
                            {
                                if (action.AktIntActionBetrag < 25)
                                {
                                    price = action.AktIntActionProvision + (action.AktIntActionBetrag * .15);
                                }
                                else
                                {
                                    price = action.AktIntActionProvision + 5;
                                }
                            }
                            else
                            {
                                price = 3;
                            }
                            break;
                        case 59:
                            strINTCODE = "NE012";
                            strBC = "vi";
                            strBERICHT = "Gesamtinkasso";
                            break;
                        case 60:
                            strINTCODE = "NE013";
                            strBC = "best";
                            strBERICHT = "Forderung bestritten";
                            price = 3;
                            break;
                        case 61:
                            strINTCODE = "NE014";
                            strBC = "lfi";
                            strBERICHT = "Laufendes CollectionInvoice";
                            price = action.AktIntActionBetrag * .1;
                            if (price > 40)
                                price = 40;
                            break;
                        case 62:
                            strINTCODE = "NE004";
                            strBC = "esn";
                            strBERICHT = "SU will geklagt werden";
                            price = 3;
                            break;
                        case 63:
                            strINTCODE = "NE005";
                            strBC = "esn";
                            strBERICHT = "SU hat keine Mahnung erhalten";
                            price = 3;
                            break;
                        case 64:
                            strINTCODE = "NE007";
                            strBC = "esn";
                            strBERICHT = "SU wird ausfällig, bedroht AD";
                            price = 3;
                            break;
                        case 65:
                            strINTCODE = "NE009";
                            strBC = "sto";
                            strBERICHT = "Storno lt. Auftraggeber";
                            price = 0;
                            break;
                        default:
                            strINTCODE = "XXXX";
                            strBERICHT = "XXXX";
                            strBC = "XXXX";
                            price = 0;
                            break;
                    }
                   
                    strPrice = price.ToString();
                    
                    if (action.AktIntActionType == 59)
                    {
                        strPrice = "manuelle Eingabe!";
                    }

                    string strRV = "";
                    if (action.AKTIntRVStartDate.ToShortDateString() != "01.01.1900")
                    {
                        strRV = "RV Startdatum: " + action.AKTIntRVStartDate.ToShortDateString();
                        switch (action.AKTIntRVInkassoType)
                        {
                            case 0:
                                strRV += ", Zahlungsweise: Erlagschein";
                                break;
                            case 1:
                                strRV += ", Zahlungsweise: pers. CollectionInvoice";
                                break;
                            default:
                                strRV += ", Zahlungsweise: unbekannt";
                                break;
                        }
                        strRV += ", Betrag: " + action.AKTIntRVAmmount;
                        strRV += ", Tag: "+ action.AKTIntRVIntervallDay + ".";
                        strRV += ", Laufzeit: " + action.AKTIntRVNoMonth;
                        strRV += ", Zahlungsrhythmus: " + action.AKTIntRVInfo;

                    }
                    WriteLine("<tr height=20 style='height:15.0pt'>");
			        WriteLine("<td height=20 class=xl1518458 align=right style='height:15.0pt'>" + counter + "</td>");
                    WriteLine("<td class=xl8618458>" + action.AktIntAZ + "</td>");
                    WriteLine("<td class=xl8718458>" + strINTCODE + "</td>");
                    WriteLine("<td class=xl8818458>" + strBC + "</td>");
                    WriteLine("<td class=xl1518458 align=center>" + strBERICHT + "</td>");
			        WriteLine("<td class=xl8818458>" + action.AktIntActionBetrag+ "</td>");
                    WriteLine("<td class=xl7718458 align=right>" + DateTime.Now.ToShortDateString() + "</td>");
                    WriteLine("<td class=xl8918458 align=center>" + strPrice + "</td>");
                    WriteLine("<td class=xl8818458>" + action.AKTIntMemo + " " + strRV + "</td>");
                    WriteLine("</tr>");
                    counter++;
                }
                CloseFile(fileName);
            }
        }

        private void WriteHeader(tblUser user, string fileName)
        {
            TextReader tr = new StreamReader(paternFile);
            string line;
            while ((line = tr.ReadLine()) != null)
                WriteLine(line);
            tr.Close();
            tr.Dispose();

            WriteLine(@"<div id=""Mappe1_18458"" align=center x:publishsource=""Excel"">");
	        WriteLine(@"<table border=0 cellpadding=0 cellspacing=0 width=720 style='border-collapse:");
	        WriteLine(@"collapse;table-layout:fixed;width:540pt'>");
	        WriteLine(@"<col width=80 span=9 style='width:60pt'>");
	        WriteLine(@"<tr height=21 style='height:15.75pt'>");
	        WriteLine(@"<td height=21 class=xl6418458 width=80 style='height:15.75pt;width:60pt'>Inkassant:</td>");
	        WriteLine(@"<td class=xl6518458 width=80 style='width:60pt'>ECP - " + user.UserNachname + "<span");
	        WriteLine(@"style='display:none'> " + user.UserVorname + " </span></td>");
	        WriteLine(@"<td class=xl6618458 width=80 style='width:60pt'>Datum:</td>");
	        WriteLine(@"<td class=xl6718458 align=right width=80 style='width:60pt'>" + DateTime.Now.ToShortDateString() + "</td>");
	        WriteLine(@"<td class=xl6818458 width=80 style='width:60pt'>Dateiname:</td>");
	        WriteLine(@"<td colspan=4 class=xl6918458 width=320 style='border-right:1.0pt solid black;");
            WriteLine(@"width:240pt'>" + fileName + "</td>");
	        WriteLine(@"</tr>");
	        WriteLine(@"<tr height=21 style='height:15.75pt'>");
	        WriteLine(@"<td height=21 class=xl7218458 style='height:15.75pt;border-top:none'>&nbsp;</td>");
	        WriteLine(@"<td class=xl7318458 colspan=2>Sachbearbeitung</td>");
	        WriteLine(@"<td class=xl7318458 style='border-top:none'>Provision</td>");
	        WriteLine(@"<td class=xl7218458 style='border-top:none'>&nbsp;</td>");
	        WriteLine(@"<td class=xl7418458 style='border-top:none'>&nbsp;</td>");
	        WriteLine(@"<td class=xl7218458 style='border-top:none'>&nbsp;</td>");
	        WriteLine(@"<td class=xl7218458 style='border-top:none'>&nbsp;</td>");
	        WriteLine(@"<td class=xl7218458 style='border-top:none'>&nbsp;</td>");
	        WriteLine(@"</tr>");
	        WriteLine(@"<tr height=21 style='height:15.75pt'>");
	        WriteLine(@" <td height=21 class=xl1518458 style='height:15.75pt'></td>");
	        WriteLine(@"<td class=xl7518458></td>");
	        WriteLine(@"<td class=xl1518458></td>");
	        WriteLine(@"<td class=xl7518458></td>");
	        WriteLine(@"<td class=xl1518458></td>");
	        WriteLine(@"<td class=xl7618458></td>");
	        WriteLine(@"<td class=xl7718458></td>");
	        WriteLine(@"<td class=xl7618458></td>");
	        WriteLine(@"<td class=xl1518458></td>");
	        WriteLine(@"</tr>");
	
	        WriteLine(@"<tr height=21 style='height:15.75pt'>");
	        WriteLine(@"<td height=21 class=xl7818458 style='height:15.75pt'>lfd. Nr.</td>");
	        WriteLine(@"<td class=xl7918458>A Z</td>");
	        WriteLine(@"<td class=xl8018458>int.Code</td>");
	        WriteLine(@"<td class=xl8118458>BC</td>");
	        WriteLine(@"<td class=xl8218458>Bericht</td>");
	        WriteLine(@"<td class=xl8318458><span style='mso-spacerun:yes'></span>CollectionInvoice<span");
	        WriteLine(@"style='mso-spacerun:yes'></span></td>");
	        WriteLine(@"<td class=xl8418458>WO</td>");
	        WriteLine(@"<td class=xl8518458><span style='mso-spacerun:yes'></span>Prov<span");
	        WriteLine(@"style='mso-spacerun:yes'></span></td>");
	        WriteLine(@"<td class=xl8518458><span style='mso-spacerun:yes'></span>Kommentar<span");
	        WriteLine(@"style='mso-spacerun:yes'></span></td>");
            WriteLine(@"</tr>");
        }

        private void ZipAndShipFiles()
        {
            Response.Clear();
            // no buffering - allows large zip files to download as they are zipped
            Response.BufferOutput = false;
            String ReadmeText = "Dynamic content for a readme file...\n" +DateTime.Now.ToString("G");
            string archiveName = String.Format("Infoscore-{0}.zip", txtDateStart.Text+"_"+txtDateEnd.Text);
            Response.ContentType = "application/zip";
            Response.AddHeader("content-disposition", "attachment; filename=" + archiveName);
            
            zip.Save(Response.OutputStream);
            Response.Flush();
            Response.Close();
        }
        private void OpenExcelFile(string fileName)
        {
            sb.Clear();
        }
        private void CloseFile(string fileName)
        {
            zip.AddEntry(fileName, sb.ToString());
        }
        private void WriteLine(string line)
        {
            sb.Append(line);
        }
    }
}