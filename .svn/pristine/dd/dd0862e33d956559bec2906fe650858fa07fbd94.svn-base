using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using HTBUtilities;

namespace HTBExtras
{
    public class AussendienstCollectedRecordComparer : IComparer
    {
        public const string SortByAktId = "AktID";
        public const string SortByAktAz = "AktAZ";
        public const string SortByCollectedDate = "CollectedDate";
        public const string SortByCollectedAmount = "CollectedAmount";
        public const string SortByGegnerName = "GegnerName";
        public const string SortByCollectorName = "CollectorName";
        public const string SortByComment = "Comment";

        private readonly SortDirection _direction = SortDirection.Asc;
        private readonly string _sortField;

        public AussendienstCollectedRecordComparer() : this(SortByAktId) { }
        public AussendienstCollectedRecordComparer(string sortField) : this(sortField, SortDirection.Asc) { }

        public AussendienstCollectedRecordComparer(string sortField, SortDirection direction)
        {
            this._direction = direction;
            this._sortField = sortField;
        }

        int IComparer.Compare(object x, object y)
        {

            var rec1 = (AussendienstCollectedRecord)x;
            var rec2 = (AussendienstCollectedRecord)y;

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
                    case SortByAktId:
                        return (this._direction == SortDirection.Asc) ? rec1.AktId.CompareTo(rec2.AktId) : rec2.AktId.CompareTo(rec1.AktId);
                    case SortByAktAz:
                        return (this._direction == SortDirection.Asc) ? rec1.AktAZ.CompareTo(rec2.AktAZ) : rec2.AktAZ.CompareTo(rec1.AktAZ);
                    case SortByCollectedDate:
                        return (this._direction == SortDirection.Asc) ? rec1.CollectedDate.CompareTo(rec2.CollectedDate) : rec2.CollectedDate.CompareTo(rec1.CollectedDate);
                    case SortByCollectedAmount:
                        return (this._direction == SortDirection.Asc) ? rec1.CollectedAmount.CompareTo(rec2.CollectedAmount) : rec2.CollectedAmount.CompareTo(rec1.CollectedAmount);
                    case SortByGegnerName:
                        return (this._direction == SortDirection.Asc) ? rec1.GegnerName.CompareTo(rec2.GegnerName) : rec2.GegnerName.CompareTo(rec1.GegnerName);
                    case SortByCollectorName:
                        return (this._direction == SortDirection.Asc) ? rec1.CollectorName.CompareTo(rec2.CollectorName) : rec2.CollectorName.CompareTo(rec1.CollectorName);
                    case SortByComment:
                        return (this._direction == SortDirection.Asc) ? rec1.Comment.CompareTo(rec2.Comment) : rec2.Comment.CompareTo(rec1.Comment);
                    default :
                        return 0;
                }
            }
        }
    }
}