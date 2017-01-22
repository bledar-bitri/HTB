using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using HTBUtilities;

namespace HTBExtras
{
    public class DateComparer : IComparer
    {
        private SortDirection m_direction = SortDirection.Asc;

        public DateComparer() : base() { }

        public DateComparer(SortDirection direction)
        {
            this.m_direction = direction;
        }

        int IComparer.Compare(object x, object y)
        {

            DateTime dateX = (DateTime)x;
            DateTime dateY = (DateTime)y;

            if (dateX == null && dateY == null)
            {
                return 0;
            }
            else if (dateX == null && dateY != null)
            {
                return (this.m_direction == SortDirection.Asc) ? -1 : 1;
            }
            else if (dateX != null && dateY == null)
            {
                return (this.m_direction == SortDirection.Asc) ? 1 : -1;
            }
            else
            {
                return (this.m_direction == SortDirection.Asc) ?
                    dateX.CompareTo(dateY) :
                    dateY.CompareTo(dateX);
            }
        }
    }
}