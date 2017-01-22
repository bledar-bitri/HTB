using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using HTBUtilities;
using System.IO.MemoryMappedFiles;
using System.IO;
using HTBReports;
using HTB.v2.intranetx.util;
using System.Collections;

namespace HTB.v2.intranetx.klienten
{
    public partial class AuftragReceipt : System.Web.UI.Page
    {
        public string klientId;
        tblKlient klient;
        ArrayList contactList;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null && !Request.QueryString["ID"].ToString().Trim().Equals(""))
            {
                klientId = Request.QueryString["ID"];
                klient = (tblKlient)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKlient WHERE KlientId = " + klientId, typeof(tblKlient));
                if (klient != null)
                {
                    contactList = HTBUtils.GetSqlRecords("SELECT * FROM tblAnsprechpartner WHERE AnsprechKlient = " + klientId, typeof(tblAnsprechpartner));
                    if (!IsPostBack)
                    {
                        GlobalUtilArea.LoadDropdownList(ddlKlientSB,
                            "SELECT UserID, (CASE WHEN UserVorname IS NULL THEN '' ELSE UserVorname END) + ' ' + (CASE WHEN UserNachname IS NULL THEN '' ELSE UserNachname END) [UserVorname] FROM tblUser where UserKlient = " +
                            klientId,
                            typeof (tblUser),
                            "UserID",
                            "UserVorname", true);
                        SetValues();
                    }
                    else
                    {
                        btnSubmit.Visible = false;
                    }
                }
            }
        }

        private void SetValues()
        {
            lblAnsprech.Text = "";
            lblKlient.Text = klient.KlientName1 + "<BR>" + klient.KlientName2;
            if (contactList.Count > 0)
            {
                foreach (tblAnsprechpartner contact in contactList)
                {
                    lblAnsprech.Text += contact.AnsprechTitel + " " + contact.AnsprechVorname + " " + contact.AnsprechNachname + "<BR>";
                }
            }
            else
            {
                lblAnsprech.Text = "&nbsp;";
            }
            txtFromDate.Text = DateTime.Now.ToShortDateString();
            txtToDate.Text = DateTime.Now.ToShortDateString();
        }

        private void PrintAuftragReceipt()
        {
            var ms = new MemoryStream();
            var paramaters = new ReportParameters
                                 {
                                     StartKlient = klient.KlientID,  
                                     EndKlient = klient.KlientID, 
                                     StartDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtFromDate.Text), 
                                     EndDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtToDate.Text),
                                     KlientSB = GlobalUtilArea.GetZeroIfConvertToIntError(ddlKlientSB.SelectedValue)
                                 };

            var receipt = new HTBReports.AuftragReceipt();

            receipt.GenerateClientReceipt(paramaters, ms);
               
            Response.Clear();
            Response.ContentType = "Application/pdf";
            Response.BinaryWrite(ms.ToArray());
            Response.End();
            
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            PrintAuftragReceipt();
        }
    }
}