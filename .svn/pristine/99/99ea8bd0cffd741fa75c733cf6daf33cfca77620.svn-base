using System;
using HTB.Database;
using System.Collections;
using System.Net;
using System.IO;
using System.Text;
using HTBUtilities;
using Google.Api.Maps.Service.Geocoding;
using Google.Api.Maps.Service;
using System.Data;
using System.Threading;

namespace HTB.v2.intranetx
{
    public partial class GetGegnerLatAndLng : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UpdateLatLong(HTBUtils.GetSqlRecords("SELECT Top 10 * FROM tblGegner WHERE GegnerLastZipPrefix = 'A' AND GegnerLatitude = 0", typeof(tblGegner)));
        }

        private void PopulateAddressgrid()
        {
            
            DataTable dt = GetInvoicesDataTableStructure();

            ArrayList gList = HTBUtilities.HTBUtils.GetSqlRecords("SELECT * FROM tblGegner WHERE GegnerLastZipPrefix = 'A' ", typeof(tblGegner));
            var request = new GeocodingRequest();

            foreach (tblGegner g in gList)
            {
                var address = g.GegnerLastStrasse + ", " + g.GegnerLastZip + ", " + g.GegnerLastOrt + ", " + HTBUtils.GetCountryName(g.GegnerLastZipPrefix);
                request.Address = address;
                request.Sensor = "false";
                var response = GeocodingService.GetResponse(request);
                DataRow dr = dt.NewRow();
                dr["Address"] = address;
                dr["Lat"] = "";
                dr["Lon"] = "";
                if (response.Status == ServiceResponseStatus.Ok)
                {
                    if (response.Results == null || response.Results.Length <= 0)
                        dr["Lat"] = "NIX";
                    else
                    {
                        foreach (var result in response.Results)
                        {
                            dr["Lat"] += result.Geometry.Location.Latitude + "&nbsp;&nbsp;&nbsp;&nbsp;";
                            dr["Lon"] += result.Geometry.Location.Longitude + "&nbsp;&nbsp;&nbsp;&nbsp;";
                            g.GegnerLatitude = Convert.ToDouble(result.Geometry.Location.Latitude);
                            g.GegnerLongitude = Convert.ToDouble(result.Geometry.Location.Longitude);
                            RecordSet.Update(g);

                        }
                    }
                }
                dt.Rows.Add(dr);
                Thread.Sleep(200);
            }

            gvAddress.DataSource = dt;
            gvAddress.DataBind();
        }


        private void UpdateLatLong(ArrayList list)
        {

            if (list.Count <= 0) return;
            var request = new GeocodingRequest();

            foreach (tblGegner g in list)
            {
                var address = g.GegnerLastStrasse + ", " + g.GegnerLastZip + ", " + g.GegnerLastOrt + ", " + HTBUtils.GetCountryName(g.GegnerLastZipPrefix);
                request.Address = address;
                request.Sensor = "false";
                var response = GeocodingService.GetResponse(request);
                if (response.Status == ServiceResponseStatus.Ok)
                {
                    foreach (var result in response.Results)
                    {
                        g.GegnerLatitude = Convert.ToDouble(result.Geometry.Location.Latitude);
                        g.GegnerLongitude = Convert.ToDouble(result.Geometry.Location.Longitude);
                        RecordSet.Update(g);

                    }
                }
            }
            Thread.Sleep(200);
            UpdateLatLong(HTBUtils.GetSqlRecords("SELECT Top 10 * FROM tblGegner WHERE GegnerLastZipPrefix = 'A' AND GegnerLatitude = 0", typeof(tblGegner)));
        }


        private DataTable GetInvoicesDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("Address", typeof(string)));
            dt.Columns.Add(new DataColumn("Lat", typeof(string)));
            dt.Columns.Add(new DataColumn("Lon", typeof(string)));
            return dt;
        }
    }
}