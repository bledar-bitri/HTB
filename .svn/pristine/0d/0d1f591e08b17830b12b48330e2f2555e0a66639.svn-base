using System;

namespace HTBExtras
{
    public class UbergebeneAktenRecord
    {
        public string AgSbName { get; set; }
        public int InterventionAkten { get; set; }
        public int DubAkten { get; set; }
        public int TestAkten { get; set; }
        public int TotalAkten
        {
            get { return InterventionAkten + TestAkten + DubAkten; }            
        }

        public UbergebeneAktenRecord (AktIntShortRecord rec)
        {
            AgSbName = rec.AktIntAgSb;
            Incr(rec);
        }
        public void Incr(UbergebeneAktenRecord rec)
        {
            InterventionAkten += rec.InterventionAkten;
            DubAkten += rec.DubAkten;
            TestAkten += rec.TestAkten;
        }

        public void Incr(AktIntShortRecord rec)
        {
            switch (rec.AktIntAktType)
            {
                case 1:
                    InterventionAkten++;
                    break;
                case 4:
                    DubAkten++;
                    break;
                case 5:
                    TestAkten++;
                    break;
            }
        }
    }
}
