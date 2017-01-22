using System.Collections;
using HTBUtilities;

namespace HTBExtras
{
    public class KlientTransferRecordComparer : IComparer
    {
        public const string SortByAktId = "AktID";
        public const string SortByAktAz = "AktAZ";
        public const string SortByTransferDate = "TransferDate";
        public const string SortByTransferAmount = "TransferAmount";
        public const string SortByKlientAmount = "KlientAmount";
        public const string SortByKlientBalance = "KlientBalance";
        public const string SortByComment = "Comment";

        private readonly SortDirection _direction = SortDirection.Asc;
        private readonly string _sortField;
        public KlientTransferRecordComparer() : this(SortByAktId) { }
        public KlientTransferRecordComparer(string sortField) : this(sortField, SortDirection.Asc) { }

        public KlientTransferRecordComparer(string sortField, SortDirection direction)
        {
            this._direction = direction;
            this._sortField = sortField;
        }

        int IComparer.Compare(object x, object y)
        {

            var rec1 = (KlientTransferRecord)x;
            var rec2 = (KlientTransferRecord)y;

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
                    case SortByTransferDate:
                        return (this._direction == SortDirection.Asc) ? rec1.TransferDate.CompareTo(rec2.TransferDate) : rec2.TransferDate.CompareTo(rec1.TransferDate);
                    case SortByTransferAmount:
                        return (this._direction == SortDirection.Asc) ? rec1.TransferAmount.CompareTo(rec2.TransferAmount) : rec2.TransferAmount.CompareTo(rec1.TransferAmount);
                    case SortByKlientAmount:
                        return (this._direction == SortDirection.Asc) ? rec1.KlientAmount.CompareTo(rec2.KlientAmount) : rec2.KlientAmount.CompareTo(rec1.KlientAmount);
                    case SortByKlientBalance:
                        return (this._direction == SortDirection.Asc) ? rec1.KlientBalance.CompareTo(rec2.KlientBalance) : rec2.KlientBalance.CompareTo(rec1.KlientBalance);
                    case SortByComment:
                        return (this._direction == SortDirection.Asc) ? rec1.Comment.CompareTo(rec2.Comment) : rec2.Comment.CompareTo(rec1.Comment);
                    default :
                        return 0;
                }
            }
        }
    }
}