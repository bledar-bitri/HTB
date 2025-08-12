using HTB.v2.intranetx.progress;
using HTB.v2.intranetx.routeplanner.dto;
using HTB.v2.intranetx.util;
using HTBExtras.XML;
using HTBUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Xml.Serialization;

namespace HTB.v2.intranetx.routeplanner.tabletApp
{
    public partial class FixAddressesAndRecalculateRouteAutomaticTabletApp : Page
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
       
        private string _taskId;
        private int _userId;
        private string _routeName;


        protected void Page_Load(object sender, EventArgs e)
        {
            Log.Info("Starting Recalculation");
            Server.ScriptTimeout = 3600 * 3; // hours
            
            _routeName = GlobalUtilArea.GetEmptyIfNull(Request.Params[GlobalHtmlParams.ROAD_NAME]);
            _userId = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Params[GlobalHtmlParams.INKASSANT_ID]);
            if (_routeName == string.Empty)
                _routeName = GlobalUtilArea.GetEmptyIfNull(Request.Form[GlobalHtmlParams.ROAD_NAME]);
            if (_userId == 0)
                _userId = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Form[GlobalHtmlParams.INKASSANT_ID]);

            Log.Info("Getting RpManager");

            var rpManager = FileSerializer<RoutePlanerManager>.DeSerialize(RoutePlanerManager.GetRouteFilePath(_userId, _routeName));
            rpManager.Source = GetType().Name;

            _taskId = _userId + HtbConstants.AddressFixAndRecalculationAutomaticTaskStatus;
            
            var taskStatus = new TaskStatus();
            Context.Cache[_taskId] = taskStatus;
            rpManager.TaskStatus = taskStatus;

            Log.Info("Getting Addresses");

            var addresses = GetAddresses(GetAttachmentText());
            Log.Info("Got Addresses");

            Log.Info("Replacing Addresses");
            ReplaceAddresses(rpManager, addresses);
            rpManager.ClearBadAddresses();

            rpManager.Run();

            Response.Write("DONE!");
        
        }

        private static void ReplaceAddresses(RoutePlanerManager rpManager, IList<XmlAddressRecord> addresses)
        {
            foreach (var addr in addresses)
            {
                Log.Info($"Replacing {addr.AddressId} ==> {addr.Address}");
                rpManager.ReplaceAddress(int.Parse(addr.AddressId), addr.Address);
            }
        }

        private string GetAttachmentText()
        {
            var result = new List<AddressWithID>();
            var uploadFiles = Request.Files;
            int i;
            Log.Info($"Upload File count [{uploadFiles.Count}]");

            if (uploadFiles.Count == 0)
            {
                Log.Error("No Attachment Found ");
                throw new ArgumentException("No Attachment Found");
            }
            if (uploadFiles.Count > 1)
            {
                Log.Error("More than one Attachment Found ");
                throw new ArgumentException("More than one Attachment Found");
            }

            var postedFile = uploadFiles[0];
            Log.Info($"Processing [{uploadFiles.Count}]");

            // Access the uploaded file's content in-memory:
            if (postedFile == null)
            {
                Log.Error("Could not read the content of the attachment");
                throw new ArgumentException("Could not read the content of the attachment");
            }

            Log.Info($"Processing file [{postedFile.FileName}] with size {postedFile.ContentLength}");
            var inStream = postedFile.InputStream;
            var fileData = new byte[postedFile.ContentLength];
            inStream.Read(fileData, 0, postedFile.ContentLength);
            var xmlData = Encoding.UTF8.GetString(fileData);
            Log.Info($"Xml Data {xmlData}");
            return xmlData;
        }

        private IList<XmlAddressRecord> GetAddresses(string xmlData)
        {
            var serializer = new XmlSerializer(typeof(List<XmlAddressRecord>), new XmlRootAttribute("Root"));
            var wrapped = "<Root>" + xmlData + "</Root>";
            using (var reader = new StringReader(wrapped))
            {
                var records = (List<XmlAddressRecord>)serializer.Deserialize(reader);

                foreach (var record in records)
                {
                    Log.Info($"ID: {record.AddressId}, Text: {record.Address}");
                }
                return records;
            }
        }

    }
}