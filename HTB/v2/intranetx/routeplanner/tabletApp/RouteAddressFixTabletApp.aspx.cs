using HTB.Database.Views;
using HTB.v2.intranetx.util;
using HTBUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.v2.intranetx.routeplanner.dto;

namespace HTB.v2.intranetx.routeplanner.tabletApp
{
    public partial class RouteAddressFixTabletApp : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var routeName = GlobalUtilArea.GetEmptyIfNull(Request.Params[GlobalHtmlParams.ROAD_NAME]);
                var routeUser = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Params[GlobalHtmlParams.INKASSANT_ID]);
                
                var rpManager = FileSerializer<RoutePlanerManager>.DeSerialize(RoutePlanerManager.GetRouteFilePath(routeUser, routeName));
                var dt = GetDataTableStructure();
                foreach (var address in rpManager.BadAddresses)
                {
                    var dr = dt.NewRow();
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
            for (var i = 0; i < gvAddresses.Rows.Count; i++)
            {
                var row = gvAddresses.Rows[i];
                var lblId = (Label) row.FindControl("lblId");
                var txtReplacementAddress = (TextBox) row.FindControl("txtReplacementAddress");
                if (!string.IsNullOrEmpty(txtReplacementAddress.Text))
                {
                    list.Add(new AddressWithID(Convert.ToInt32(lblId.Text), txtReplacementAddress.Text));
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