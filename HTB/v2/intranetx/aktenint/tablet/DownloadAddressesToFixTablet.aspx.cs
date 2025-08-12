using HTB.v2.intranetx.routeplanner;
using HTB.v2.intranetx.util;
using System;
using System.Linq;
using HTBExtras.XML;

namespace HTB.v2.intranetx.aktenint.tablet
{
    public partial class DownloadAddressesToFixTablet : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            var routeName = GlobalUtilArea.GetEmptyIfNull(Request.Params[GlobalHtmlParams.ROAD_NAME]);
            var routeUser = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Params[GlobalHtmlParams.INKASSANT_ID]);

            try
            {
                var rpManager = FileSerializer<RoutePlanerManager>.DeSerialize(RoutePlanerManager.GetRouteFilePath(routeUser, routeName));
                if (rpManager != null)
                {
                    foreach (var address in rpManager.BadAddresses.Select(badAddress => new XmlAddressRecord
                             {
                                 AddressId = badAddress.ID.ToString(),
                                 Address = badAddress.Address,
                                 Latitude = string.Empty,
                                 Longitude = string.Empty
                             }))
                    {
                        Response.Write(address.ToXmlString());
                    }
                }
                else
                {
                    Response.Write("RoutePlanerManager not found.");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.Write(ex.StackTrace);
            }
        }
    }
}