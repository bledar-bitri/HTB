/*
 * Author:			Generated Code
 * Date Created:	09.08.2011
 * Description:		Represents a row in the tblAkten table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblAkten : Record
	{
		#region Property Declaration
		private int _aktID;
		private string _aktCaption;
		private string _aktKlient;
		private string _aktGegner;
		private DateTime _aktDatdum;
		private double _aktStatus;
		private string _aktIZ;
		private int _akttyp;
		private int _aktSB;
		private string _aktOldID;
		private string _aktRechnungAnrede;
		private string _aKtRechnungName1;
		private string _aKtRechnungName2;
		private string _aKtRechnungName3;
		private string _aKtRechnungStrasse;
		private string _aKtRechnungLKZ;
		private string _aKtRechnungZIP;
		private string _aKtRechnungOrt;
		private int _aKtRechnungLand;
		private double _aKTPreis;
		private string _aKTBericht;
		private DateTime _aKTVersanddatum;
		private DateTime _aKTBearbeitungDate;
		private DateTime _aKTBearbeitungTime;
		private DateTime _aKTFertigDate;
		private DateTime _aKTFertigTime;
		private int _aKTPosNeg;
		private string _aKTMemo;
		private int _aKTVorlageStat;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int AktID
		{
			get { return _aktID; }
			set { _aktID = value; }
		}
		public string AktCaption
		{
			get { return _aktCaption; }
			set { _aktCaption = value; }
		}
		public string AktKlient
		{
			get { return _aktKlient; }
			set { _aktKlient = value; }
		}
		public string AktGegner
		{
			get { return _aktGegner; }
			set { _aktGegner = value; }
		}
		public DateTime AktDatdum
		{
			get { return _aktDatdum; }
			set { _aktDatdum = value; }
		}
		public double AktStatus
		{
			get { return _aktStatus; }
			set { _aktStatus = value; }
		}
		public string AktIZ
		{
			get { return _aktIZ; }
			set { _aktIZ = value; }
		}
		public int Akttyp
		{
			get { return _akttyp; }
			set { _akttyp = value; }
		}
		public int AktSB
		{
			get { return _aktSB; }
			set { _aktSB = value; }
		}
		public string AktOldID
		{
			get { return _aktOldID; }
			set { _aktOldID = value; }
		}
		public string AktRechnungAnrede
		{
			get { return _aktRechnungAnrede; }
			set { _aktRechnungAnrede = value; }
		}
		public string AKtRechnungName1
		{
			get { return _aKtRechnungName1; }
			set { _aKtRechnungName1 = value; }
		}
		public string AKtRechnungName2
		{
			get { return _aKtRechnungName2; }
			set { _aKtRechnungName2 = value; }
		}
		public string AKtRechnungName3
		{
			get { return _aKtRechnungName3; }
			set { _aKtRechnungName3 = value; }
		}
		public string AKtRechnungStrasse
		{
			get { return _aKtRechnungStrasse; }
			set { _aKtRechnungStrasse = value; }
		}
		public string AKtRechnungLKZ
		{
			get { return _aKtRechnungLKZ; }
			set { _aKtRechnungLKZ = value; }
		}
		public string AKtRechnungZIP
		{
			get { return _aKtRechnungZIP; }
			set { _aKtRechnungZIP = value; }
		}
		public string AKtRechnungOrt
		{
			get { return _aKtRechnungOrt; }
			set { _aKtRechnungOrt = value; }
		}
		public int AKtRechnungLand
		{
			get { return _aKtRechnungLand; }
			set { _aKtRechnungLand = value; }
		}
		public double AKTPreis
		{
			get { return _aKTPreis; }
			set { _aKTPreis = value; }
		}
		public string AKTBericht
		{
			get { return _aKTBericht; }
			set { _aKTBericht = value; }
		}
		public DateTime AKTVersanddatum
		{
			get { return _aKTVersanddatum; }
			set { _aKTVersanddatum = value; }
		}
		public DateTime AKTBearbeitungDate
		{
			get { return _aKTBearbeitungDate; }
			set { _aKTBearbeitungDate = value; }
		}
		public DateTime AKTBearbeitungTime
		{
			get { return _aKTBearbeitungTime; }
			set { _aKTBearbeitungTime = value; }
		}
		public DateTime AKTFertigDate
		{
			get { return _aKTFertigDate; }
			set { _aKTFertigDate = value; }
		}
		public DateTime AKTFertigTime
		{
			get { return _aKTFertigTime; }
			set { _aKTFertigTime = value; }
		}
		public int AKTPosNeg
		{
			get { return _aKTPosNeg; }
			set { _aKTPosNeg = value; }
		}
		public string AKTMemo
		{
			get { return _aKTMemo; }
			set { _aKTMemo = value; }
		}
		public int AKTVorlageStat
		{
			get { return _aKTVorlageStat; }
			set { _aKTVorlageStat = value; }
		}
		#endregion
	}
}
