using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTB.Database.Views
{
    public class qryKosten : tblKostenArt
    {
        #region Property Declaration from tblKosten
        private int _kostenId;
        private int _kostenArtId;
        private decimal _von;
        private decimal _bis;
        private decimal _kostenPct;
        private decimal _kostenAmount;
        public int KostenId
        {
            get { return _kostenId; }
            set { _kostenId = value; }
        }
        public int KostenArtId
        {
            get { return _kostenArtId; }
            set { _kostenArtId = value; }
        }
        public decimal Von
        {
            get { return _von; }
            set { _von = value; }
        }
        public decimal Bis
        {
            get { return _bis; }
            set { _bis = value; }
        }
        public decimal KostenPct
        {
            get { return _kostenPct; }
            set { _kostenPct = value; }
        }
        public decimal KostenAmount
        {
            get { return _kostenAmount; }
            set { _kostenAmount = value; }
        }
        #endregion
    }
}
