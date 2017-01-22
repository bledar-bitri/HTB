using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using HTBUtilities;

namespace HTBExtras
{
    public class UbergebeneAktenRecordComparer : IComparer
    {
        public const string SortByAgSbName = "AgSbName";
        public const string SortByInterventionAkten = "InterventionAkten";
        public const string SortByDubAkten = "DubAkten";
        public const string SortByTestAkten = "TestAkten";
        public const string SortByTotalAkten = "TotalAkten";
       
        private readonly SortDirection _direction = SortDirection.Asc;
        private readonly string _sortField;

        public UbergebeneAktenRecordComparer() : this(SortByAgSbName) { }
        public UbergebeneAktenRecordComparer(string sortField) : this(sortField, SortDirection.Asc) { }

        public UbergebeneAktenRecordComparer(string sortField, SortDirection direction)
        {
            this._direction = direction;
            this._sortField = sortField;
        }

        int IComparer.Compare(object x, object y)
        {

            var rec1 = (UbergebeneAktenRecord)x;
            var rec2 = (UbergebeneAktenRecord)y;

            if (rec1 == null && rec2 == null)
            {
                return 0;
            }
            else if (rec1 == null && rec2 != null)
            {
                return (this._direction == SortDirection.Asc) ? -1 : 1;
            }
            else if (rec1 != null && rec2 == null)
            {
                return (this._direction == SortDirection.Asc) ? 1 : -1;
            }
            else
            {
                switch (_sortField)
                {
                    case SortByAgSbName:
                        return (this._direction == SortDirection.Asc) ? rec1.AgSbName.CompareTo(rec2.AgSbName) : rec2.AgSbName.CompareTo(rec1.AgSbName);
                    case SortByInterventionAkten:
                        return (this._direction == SortDirection.Asc) ? rec1.InterventionAkten.CompareTo(rec2.InterventionAkten) : rec2.InterventionAkten.CompareTo(rec1.InterventionAkten);
                    case SortByDubAkten:
                        return (this._direction == SortDirection.Asc) ? rec1.DubAkten.CompareTo(rec2.DubAkten) : rec2.DubAkten.CompareTo(rec1.DubAkten);
                    case SortByTestAkten:
                        return (this._direction == SortDirection.Asc) ? rec1.TestAkten.CompareTo(rec2.TestAkten) : rec2.TestAkten.CompareTo(rec1.TestAkten);
                    case SortByTotalAkten:
                        return (this._direction == SortDirection.Asc) ? rec1.TotalAkten.CompareTo(rec2.TotalAkten) : rec2.TotalAkten.CompareTo(rec1.TotalAkten);
                   default :
                        return 0;
                }
            }
        }
    }
}