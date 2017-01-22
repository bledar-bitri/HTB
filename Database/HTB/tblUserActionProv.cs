/*
 * Author:			Generated Code
 * Date Created:	09.06.2011
 * Description:		Represents a row in the tblUserActionProv table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblUserActionProv : Record
	{
		#region Property Declaration
		private int _userActionProvID;
		private int _userAktionProvUserID;
		private int _userActionProvAktAktionTypeID;
		private double _userActionProvAmount;
		private double _userActionProvPct;
		private double _userActionProvAmountForZeroCollection;
        private double _userActionProvPrice;
        private int _userActionProvHonGrpID;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int UserActionProvID
		{
			get { return _userActionProvID; }
			set { _userActionProvID = value; }
		}
		public int UserAktionProvUserID
		{
			get { return _userAktionProvUserID; }
			set { _userAktionProvUserID = value; }
		}
		public int UserActionProvAktAktionTypeID
		{
			get { return _userActionProvAktAktionTypeID; }
			set { _userActionProvAktAktionTypeID = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double UserActionProvAmount
		{
			get { return _userActionProvAmount; }
			set { _userActionProvAmount = value; }
		}
		public double UserActionProvPct
		{
			get { return _userActionProvPct; }
			set { _userActionProvPct = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double UserActionProvAmountForZeroCollection
		{
			get { return _userActionProvAmountForZeroCollection; }
			set { _userActionProvAmountForZeroCollection = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double UserActionProvPrice
        {
            get { return _userActionProvPrice; }
            set { _userActionProvPrice = value; }
        }
        public int UserActionProvHonGrpID
        {
            get { return _userActionProvHonGrpID; }
            set { _userActionProvHonGrpID = value; }
        }
        #endregion
	}
}
