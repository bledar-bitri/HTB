using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using HTBExtras.XML;

namespace HTB.v2.intranetx.wfc.aktenint
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGetData" in both code and config file together.
    [ServiceContract]
    public interface IGetData
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/GetPhoneTypes")]
        string GetPhoneTypes();

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/GetAktTypes")]
        string GetAktTypes();
    }
}
