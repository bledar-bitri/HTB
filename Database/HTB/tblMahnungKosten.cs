/*
 * Author:			Generated Code
 * Date Created:	23.02.2011
 * Description:		Represents a row in the tblMahnungKosten table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblMahnungKosten : Record
	{
		#region Property Declaration
		private int _mahnungKostenID;
		private int _mahnungID;
		private int _mahnungKostenNumber;
		private double _mahnungKostenPrice;
		private string _mahnungKostenText;
		private double _mahnungKostenSum;
		private string _mahnungKostenEH;
		private string _mahnungKostenArtnumber;
		private int _kostenType;
        private bool _isZinsen;
        private bool _isInkassoZinsen;
        private bool _isSteuer;
        private bool _isTaxable;
        
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int MahnungKostenID
		{
			get { return _mahnungKostenID; }
			set { _mahnungKostenID = value; }
		}
		public int MahnungID
		{
			get { return _mahnungID; }
			set { _mahnungID = value; }
		}
		public int MahnungKostenNumber
		{
			get { return _mahnungKostenNumber; }
			set { _mahnungKostenNumber = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double MahnungKostenPrice
		{
			get { return _mahnungKostenPrice; }
			set { _mahnungKostenPrice = value; }
		}
		public string MahnungKostenText
		{
			get { return _mahnungKostenText; }
			set { _mahnungKostenText = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double MahnungKostenSum
		{
			get { return _mahnungKostenSum; }
			set { _mahnungKostenSum = value; }
		}
		public string MahnungKostenEH
		{
			get { return _mahnungKostenEH; }
			set { _mahnungKostenEH = value; }
		}
		public string MahnungKostenArtnumber
		{
			get { return _mahnungKostenArtnumber; }
			set { _mahnungKostenArtnumber = value; }
		}
		public int KostenType
		{
			get { return _kostenType; }
			set { _kostenType = value; }
		}
        public bool IsZinsen
        {
            get { return _isZinsen; }
            set { _isZinsen = value; }
        }
        public bool IsInkassoZinsen
        {
            get { return _isInkassoZinsen; }
            set { _isInkassoZinsen = value; }
        }
        public bool IsSteuer
        {
            get { return _isSteuer; }
            set { _isSteuer = value; }
        }
        public bool IsTaxable
        {
            get { return _isTaxable; }
            set { _isTaxable = value; }
        }
        #endregion
	}
}
