using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;
using HTBExtras.XML;
using HTBUtilities;

namespace HTB.v2.intranetx.wfc.aktenint
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GetData" in code, svc and config file together.
    public class GetData : IGetData
    {
        public string GetPhoneTypes()
        {
            ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM tblPhoneType Order By PhoneTypeSequence", typeof(XmlPhoneType));
            var phoneTypes = new List<object>();
            foreach (XmlPhoneType rec in list)
            {
                rec.TableName = "x";
                phoneTypes.Add(rec);
            }
            return new JavaScriptSerializer().Serialize(phoneTypes);
        }

        public string GetAktTypes()
        {
            ArrayList list = HTBUtils.GetSqlRecords("SELECT AktTypeIntId, AktTypeIntCaption FROM tblAktTypeInt WHERE AktTypeINTCaption NOT LIKE '%Auto%' Order By AktTypeIntCaption ", typeof(XmlAktTypeIntRecord));
            var aktTypes = new List<object>();
            foreach (XmlAktTypeIntRecord rec in list)
            {
                rec.TableName = "x";
                aktTypes.Add(rec);
            }
            return new JavaScriptSerializer().Serialize(aktTypes);
        }
    }
}
