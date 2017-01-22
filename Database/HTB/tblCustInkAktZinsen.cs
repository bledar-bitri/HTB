/*
 * Author:			Generated Code
 * Date Created:	07.03.2011
 * Description:		Represents a row in the tblCustInkAktZinsen table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblCustInkAktZinsen : Record
	{
		#region Property Declaration
		private int _custInkAktZinsenID;
		private double _custInkAktZinsenBetrag;
		private DateTime _custInkAktZinsenVonDatum;
		private DateTime _custInkAktZinsenBisDatum;
		private string _custInkAktZinsenCaption;
		private int _custInkAktZinsenAkt;
		private int _custInkAktZinsenProz;
		private DateTime _custInkAktZinsenDate;
		private int _custInkAktZinsenDelFlag;
		private int _custInkAktZinsenDZFlag;
		private double _custInkAktZinsenSaldo;
		private int _custInkAktZinsenZF;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int CustInkAktZinsenID
		{
			get { return _custInkAktZinsenID; }
			set { _custInkAktZinsenID = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double CustInkAktZinsenBetrag
		{
			get { return _custInkAktZinsenBetrag; }
			set { _custInkAktZinsenBetrag = value; }
		}
		public DateTime CustInkAktZinsenVonDatum
		{
			get { return _custInkAktZinsenVonDatum; }
			set { _custInkAktZinsenVonDatum = value; }
		}
		public DateTime CustInkAktZinsenBisDatum
		{
			get { return _custInkAktZinsenBisDatum; }
			set { _custInkAktZinsenBisDatum = value; }
		}
		public string CustInkAktZinsenCaption
		{
			get { return _custInkAktZinsenCaption; }
			set { _custInkAktZinsenCaption = value; }
		}
		public int CustInkAktZinsenAkt
		{
			get { return _custInkAktZinsenAkt; }
			set { _custInkAktZinsenAkt = value; }
		}
		public int CustInkAktZinsenProz
		{
			get { return _custInkAktZinsenProz; }
			set { _custInkAktZinsenProz = value; }
		}
		public DateTime CustInkAktZinsenDate
		{
			get { return _custInkAktZinsenDate; }
			set { _custInkAktZinsenDate = value; }
		}
		public int CustInkAktZinsenDelFlag
		{
			get { return _custInkAktZinsenDelFlag; }
			set { _custInkAktZinsenDelFlag = value; }
		}
		public int CustInkAktZinsenDZFlag
		{
			get { return _custInkAktZinsenDZFlag; }
			set { _custInkAktZinsenDZFlag = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double CustInkAktZinsenSaldo
		{
			get { return _custInkAktZinsenSaldo; }
			set { _custInkAktZinsenSaldo = value; }
		}
		public int CustInkAktZinsenZF
		{
			get { return _custInkAktZinsenZF; }
			set { _custInkAktZinsenZF = value; }
		}
		#endregion
	}
}
