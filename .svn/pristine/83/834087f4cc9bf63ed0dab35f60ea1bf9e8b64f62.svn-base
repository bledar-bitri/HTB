using System;
using System.Collections;

namespace HTBExtras
{
    public class UbergebeneAktenRecordList : ArrayList
    {
        public void Incr(UbergebeneAktenRecord rec)
        {
            bool found = false;
            foreach (UbergebeneAktenRecord current in this)
            {
                if(rec.AgSbName == current.AgSbName)
                {
                    current.Incr(rec);
                    found = true;
                }
            }
            if(!found)
            {
                Add(rec);
            }
        }

        public void Incr(AktIntShortRecord rec)
        {
            Incr(new UbergebeneAktenRecord(rec));
        }
    }
}
