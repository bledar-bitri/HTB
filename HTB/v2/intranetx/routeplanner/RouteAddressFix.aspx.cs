using HTB.Database.Views;
using HTB.v2.intranetx.util;
using HTBUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.v2.intranetx.routeplanner.dto;

namespace HTB.v2.intranetx.routeplanner
{
    public partial class RouteAddressFix : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session[GlobalHtmlParams.SessionReplacementAddresses] = null;
                var rpManager = (RoutePlanerManager) Session[GlobalHtmlParams.SessionRoutePlannerManager];
                DataTable dt = GetDataTableStructure();
                foreach (var address in rpManager.BadAddresses)
                {
                    DataRow dr = dt.NewRow();
                    dr["Address"] = address.Address;
                    dr["Akt"] = address.ID;
                    dr["Gegner"] = GetGegnerInfo(address.ID);
                    dt.Rows.Add(dr);
                }
                gvAddresses.DataSource = dt;
                gvAddresses.DataBind();
            }
        }

        private DataTable GetDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("Address", typeof(string)));
            dt.Columns.Add(new DataColumn("Akt", typeof(string)));
            dt.Columns.Add(new DataColumn("Gegner", typeof(string)));
            return dt;
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            var list = new List<AddressWithID>();
            for (int i = 0; i < gvAddresses.Rows.Count; i++)
            {
                GridViewRow row = gvAddresses.Rows[i];
                var lblID = (Label) row.FindControl("lblId");
                var txtReplacementAddress = (TextBox) row.FindControl("txtReplacementAddress");
                if (!string.IsNullOrEmpty(txtReplacementAddress.Text))
                {
                    list.Add(new AddressWithID(Convert.ToInt32(lblID.Text), txtReplacementAddress.Text));
                }
            }
            Session[GlobalHtmlParams.SessionReplacementAddresses] = list;
            var rpManager = (RoutePlanerManager) Session[GlobalHtmlParams.SessionRoutePlannerManager];
            Response.Redirect(rpManager.Source + "?" + GlobalHtmlParams.RecalculateRoute + "=" + GlobalHtmlParams.YES);
        }

        private string GetGegnerInfo(int aktId)
        {
            
            qryAktenInt akt = HTBUtils.GetInterventionAktQry(aktId);
            if(akt != null)
            {
                return akt.GegnerLastName1 + " " + akt.GegnerLastName2 + "<br>" + akt.GegnerPhoneCity + " " + akt.GegnerPhone;
            }
            return "";
        }
    }
}