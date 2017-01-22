using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using HTBUtilities;

namespace HTBExtras
{
    public class StatisticRecordComparer : IComparer
    {
        public const string SortBy_Description = "Description";
        public const string SortBy_Akts = "Akts";
        public const string SortBy_PercentAkt = "PercentAkt";
        public const string SortBy_Amount = "Amount";
        public const string SortBy_PercentAmount = "PercentAmount";

        private SortDirection m_direction = SortDirection.Asc;
        private string m_sortField;
        public StatisticRecordComparer() : this(SortBy_Description) { }
        public StatisticRecordComparer(string sortField) : this(sortField, SortDirection.Asc) { }

        public StatisticRecordComparer(string sortField, SortDirection direction)
        {
            this.m_direction = direction;
            this.m_sortField = sortField;
        }

        int IComparer.Compare(object x, object y)
        {

            StatisticRecord rec1 = (StatisticRecord)x;
            StatisticRecord rec2 = (StatisticRecord)y;

            if (rec1 == null && rec2 == null)
            {
                return 0;
            }
            else if (rec1 == null && rec2 != null)
            {
                return (this.m_direction == SortDirection.Asc) ? -1 : 1;
            }
            else if (rec1 != null && rec2 == null)
            {
                return (this.m_direction == SortDirection.Asc) ? 1 : -1;
            }
            else
            {
                switch (m_sortField)
                {
                    case SortBy_Description:
                        return (this.m_direction == SortDirection.Asc) ? rec1.Description.CompareTo(rec2.Description) : rec2.Description.CompareTo(rec1.Description);
                    case SortBy_Akts:
                        return (this.m_direction == SortDirection.Asc) ? rec1.Akts.CompareTo(rec2.Akts) : rec2.Akts.CompareTo(rec1.Akts);
                    case SortBy_Amount:
                        return (this.m_direction == SortDirection.Asc) ? rec1.Amount.CompareTo(rec2.Amount) : rec2.Amount.CompareTo(rec1.Amount);
                    case SortBy_PercentAkt:
                        return (this.m_direction == SortDirection.Asc) ? rec1.PercentAkt.CompareTo(rec2.PercentAkt) : rec2.PercentAkt.CompareTo(rec1.PercentAkt);
                    case SortBy_PercentAmount:
                        return (this.m_direction == SortDirection.Asc) ? rec1.PercentAmount.CompareTo(rec2.PercentAmount) : rec2.PercentAmount.CompareTo(rec1.PercentAmount);
                    default :
                        return 0;
                }
            }
        }
    }
}