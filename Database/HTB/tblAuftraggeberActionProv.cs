/*
 * Author:			Generated Code
 * Date Created:	08.06.2011
 * Description:		Represents a row in the tblAuftraggeberActionProv table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblAuftraggeberActionProv : Record
	{
		#region Property Declaration
		private int _aGActionProvID;
		private int _aGAktionProvAuftraggeberID;
		private int _aGActionProvAktAktionTypeID;
		private double _aGActionProvAmount;
		private double _aGActionProvPct;
		private double _aGActionProvAmountForZeroCollection;
        private double _aGActionProvPrice;
        private int _aGActionProvHonGrpID;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int AGActionProvID
		{
			get { return _aGActionProvID; }
			set { _aGActionProvID = value; }
		}
		public int AGAktionProvAuftraggeberID
		{
			get { return _aGAktionProvAuftraggeberID; }
			set { _aGAktionProvAuftraggeberID = value; }
		}
		public int AGActionProvAktAktionTypeID
		{
			get { return _aGActionProvAktAktionTypeID; }
			set { _aGActionProvAktAktionTypeID = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double AGActionProvAmount
		{
			get { return _aGActionProvAmount; }
			set { _aGActionProvAmount = value; }
		}
		public double AGActionProvPct
		{
			get { return _aGActionProvPct; }
			set { _aGActionProvPct = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double AGActionProvAmountForZeroCollection
		{
			get { return _aGActionProvAmountForZeroCollection; }
			set { _aGActionProvAmountForZeroCollection = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double AGActionProvPrice
        {
            get { return _aGActionProvPrice; }
            set { _aGActionProvPrice = value; }
        }
        public int AGActionProvHonGrpID
        {
            get { return _aGActionProvHonGrpID; }
            set { _aGActionProvHonGrpID = value; }
        }
        #endregion
	}
}
