using System.Collections;
using System.Collections.Generic;
using HTB.Database;

namespace HTBExtras.XML
{
    public class XmlChangedAktsResponseRecord : Record
    {
        public ArrayList AktIntIdList { get; set; }

        public XmlChangedAktsResponseRecord()
        {
            AktIntIdList = new ArrayList();
        }

        public void SetAktIntIds(List<StringValueRec> list)
        {
            AktIntIdList.Clear();
            AktIntIdList.AddRange(list);
        }
    }
}
