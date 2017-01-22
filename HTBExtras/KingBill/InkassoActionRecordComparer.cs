using System.Collections;
using HTBUtilities;

namespace HTBExtras.KingBill
{
    public class InkassoActionRecordComparer : IComparer
    {
        private readonly SortDirection _direction = SortDirection.Asc;

        public InkassoActionRecordComparer() : base() { }

        public InkassoActionRecordComparer(SortDirection direction)
        {
            this._direction = direction;
        }

        int IComparer.Compare(object x, object y)
        {

            var actionX = (InkassoActionRecord)x;
            var actionY = (InkassoActionRecord)y;

            if (actionX == null && actionY == null)
            {
                return 0;
            }
            else if (actionX == null && actionY != null)
            {
                return (this._direction == SortDirection.Asc) ? -1 : 1;
            }
            else if (actionX != null && actionY == null)
            {
                return (this._direction == SortDirection.Asc) ? 1 : -1;
            }
            else
            {
                return (this._direction == SortDirection.Asc) ? 
                    actionX.ActionDate.CompareTo(actionY.ActionDate) :
                    actionY.ActionDate.CompareTo(actionX.ActionDate);
            }
        }
    }
}