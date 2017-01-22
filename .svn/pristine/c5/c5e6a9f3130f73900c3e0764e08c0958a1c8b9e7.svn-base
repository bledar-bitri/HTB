using System.Collections;
using System.Collections.Generic;
using HTB.Database;
using HTB.Database.LookupRecords;

namespace HTBExtras.XML
{
    public class XmlSearchResponseRecord : Record
    {
        public ArrayList GegnerDetailList { get; set; }
        public int TotalGegnerFound { get; set; }

        public XmlSearchResponseRecord()
        {
            GegnerDetailList = new ArrayList();
            TotalGegnerFound = 0;
        }

        public void SetGegnerDetailList(List<GegnerDetailLookup> list)
        {
            GegnerDetailList.Clear();
            GegnerDetailList.AddRange(list);
        }
    }
}
