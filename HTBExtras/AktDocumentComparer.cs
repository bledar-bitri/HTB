using System.Collections;
using HTBUtilities;

namespace HTBExtras
{
    public class AktDocumentComparer : IComparer
    {
        public const string SortByDocumentDate = "DocumentDate";
        private readonly SortDirection _direction = SortDirection.Asc;
        private readonly string _sortField;

        public AktDocumentComparer() : this(SortByDocumentDate) { }

        public AktDocumentComparer(string sortField, SortDirection direction = SortDirection.Desc)
        {
            _direction = direction;
            _sortField = sortField;
        }

        public int Compare(object x, object y)
        {
            var rec1 = (AktDocument)x;
            var rec2 = (AktDocument)y;

            if (rec1 == null && rec2 == null)
            {
                return 0;
            }
            if (rec1 == null && rec2 != null)
            {
                return (_direction == SortDirection.Asc) ? -1 : 1;
            }
            if (rec1 != null && rec2 == null)
            {
                return (_direction == SortDirection.Asc) ? 1 : -1;
            }
            switch (_sortField)
            {
                case SortByDocumentDate:
                    return (_direction == SortDirection.Asc) ? rec1.DocChangeDate.CompareTo(rec2.DocChangeDate) : rec2.DocChangeDate.CompareTo(rec1.DocChangeDate);
                default :
                    return 0;
            }
        }
    }
}
