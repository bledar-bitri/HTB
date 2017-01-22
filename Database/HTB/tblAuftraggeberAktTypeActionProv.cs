/*
 * Author:			Generated Code
 * Date Created:	08.06.2011
 * Description:		Represents a row in the tblAuftraggeberAktTypeActionProv table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblAuftraggeberAktTypeActionProv : Record
	{
		#region Property Declaration
		private int _aGAktTypeActionProvID;
		private int _aGAktTypeAktionProvAuftraggeberID;
		private int _aGAktTypeActionProvAktTypeIntID;
		private int _aGAktTypeActionProvAktAktionTypeID;
		private double _aGAktTypeActionProvAmount;
		private double _aGAktTypeActionProvPct;
		private double _aGAktTypeActionProvAmountForZeroCollection;
        private double _aGAktTypeActionProvPrice;
        private int _aGAktTypeActionProvHonGrpID;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int AGAktTypeActionProvID
		{
			get { return _aGAktTypeActionProvID; }
			set { _aGAktTypeActionProvID = value; }
		}
		public int AGAktTypeAktionProvAuftraggeberID
		{
			get { return _aGAktTypeAktionProvAuftraggeberID; }
			set { _aGAktTypeAktionProvAuftraggeberID = value; }
		}
		public int AGAktTypeActionProvAktTypeIntID
		{
			get { return _aGAktTypeActionProvAktTypeIntID; }
			set { _aGAktTypeActionProvAktTypeIntID = value; }
		}
		public int AGAktTypeActionProvAktAktionTypeID
		{
			get { return _aGAktTypeActionProvAktAktionTypeID; }
			set { _aGAktTypeActionProvAktAktionTypeID = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double AGAktTypeActionProvAmount
		{
			get { return _aGAktTypeActionProvAmount; }
			set { _aGAktTypeActionProvAmount = value; }
		}
		public double AGAktTypeActionProvPct
		{
			get { return _aGAktTypeActionProvPct; }
			set { _aGAktTypeActionProvPct = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double AGAktTypeActionProvAmountForZeroCollection
		{
			get { return _aGAktTypeActionProvAmountForZeroCollection; }
			set { _aGAktTypeActionProvAmountForZeroCollection = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double AGAktTypeActionProvPrice
        {
            get { return _aGAktTypeActionProvPrice; }
            set { _aGAktTypeActionProvPrice = value; }
        }
        public int AGAktTypeActionProvHonGrpID
        {
            get { return _aGAktTypeActionProvHonGrpID; }
            set { _aGAktTypeActionProvHonGrpID = value; }
        }
        #endregion
	}
}
