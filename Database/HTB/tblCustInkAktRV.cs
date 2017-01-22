/*
 * Author:			Generated Code
 * Date Created:	14.03.2011
 * Description:		Represents a row in the tblCustInkAktRV table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblCustInkAktRV : Record
	{
		#region Property Declaration
		private int _custInkAktRVID;
		private int _custInkAktRVAktID;
		private DateTime _custInkAktRVStartDate;
		private DateTime _custInkAktRVEndDate;
		private double _custInkAktRVAnzahlung;
		private double _custInkAktRVTeilbetrag;
		private double _custInkAktRVTeilbetragOffen;
		private int _custInkAktRVNoTeilbetrag;
		private int _custInkAktRVNoTeilbetragOffen;
		private double _custInkAktRvLastRate;
		private DateTime _custInkAktRVNextWFL;
		private int _custInkAktRVRatenerinnerung;
		private DateTime _custInkAktRVNextWFLRE;
		private int _custInkAktRVUser;
		private int _custInkAktRVRatebezahlt;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int CustInkAktRVID
		{
			get { return _custInkAktRVID; }
			set { _custInkAktRVID = value; }
		}
		public int CustInkAktRVAktID
		{
			get { return _custInkAktRVAktID; }
			set { _custInkAktRVAktID = value; }
		}
		public DateTime CustInkAktRVStartDate
		{
			get { return _custInkAktRVStartDate; }
			set { _custInkAktRVStartDate = value; }
		}
		public DateTime CustInkAktRVEndDate
		{
			get { return _custInkAktRVEndDate; }
			set { _custInkAktRVEndDate = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double CustInkAktRVAnzahlung
		{
			get { return _custInkAktRVAnzahlung; }
			set { _custInkAktRVAnzahlung = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double CustInkAktRVTeilbetrag
		{
			get { return _custInkAktRVTeilbetrag; }
			set { _custInkAktRVTeilbetrag = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double CustInkAktRVTeilbetragOffen
		{
			get { return _custInkAktRVTeilbetragOffen; }
			set { _custInkAktRVTeilbetragOffen = value; }
		}
		public int CustInkAktRVNoTeilbetrag
		{
			get { return _custInkAktRVNoTeilbetrag; }
			set { _custInkAktRVNoTeilbetrag = value; }
		}
		public int CustInkAktRVNoTeilbetragOffen
		{
			get { return _custInkAktRVNoTeilbetragOffen; }
			set { _custInkAktRVNoTeilbetragOffen = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double CustInkAktRvLastRate
		{
			get { return _custInkAktRvLastRate; }
			set { _custInkAktRvLastRate = value; }
		}
		public DateTime CustInkAktRVNextWFL
		{
			get { return _custInkAktRVNextWFL; }
			set { _custInkAktRVNextWFL = value; }
		}
		public int CustInkAktRVRatenerinnerung
		{
			get { return _custInkAktRVRatenerinnerung; }
			set { _custInkAktRVRatenerinnerung = value; }
		}
		public DateTime CustInkAktRVNextWFLRE
		{
			get { return _custInkAktRVNextWFLRE; }
			set { _custInkAktRVNextWFLRE = value; }
		}
		public int CustInkAktRVUser
		{
			get { return _custInkAktRVUser; }
			set { _custInkAktRVUser = value; }
		}
		public int CustInkAktRVRatebezahlt
		{
			get { return _custInkAktRVRatebezahlt; }
			set { _custInkAktRVRatebezahlt = value; }
		}
		#endregion
	}
}
