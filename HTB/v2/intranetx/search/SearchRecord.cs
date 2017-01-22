using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTB.v2.intranetx.search
{
    public class SearchRecord
    {
        public string SearchName { get; set; }
        public string SearchNumber { get; set; }
        public string SearchPhone { get; set; }

        public bool IsEmpty ()
        {
            return string.IsNullOrEmpty(SearchName) && 
                string.IsNullOrEmpty(SearchNumber) &&
                string.IsNullOrEmpty(SearchPhone);
        }
        public void Assign(SearchRecord rec)
        {
            if (rec != null)
            {
                SearchName = rec.SearchName;
                SearchNumber = rec.SearchNumber;
                SearchPhone = rec.SearchPhone;
            }
        }
    }
}