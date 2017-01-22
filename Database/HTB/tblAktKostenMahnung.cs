/*
 * Author:			Generated Code
 * Date Created:	21.02.2011
 * Description:		Represents a row in the tblAktKostenMahnung table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblAktKostenMahnung
	{
		#region Property Declaration
		private int _aktKostenMahnungID;
		private int _aktKostenID;
		private int _mahnungSeqNummer;
		private decimal _mahnungKosten;
		public int AktKostenMahnungID
		{
			get { return _aktKostenMahnungID; }
			set { _aktKostenMahnungID = value; }
		}
		public int AktKostenID
		{
			get { return _aktKostenID; }
			set { _aktKostenID = value; }
		}
		public int MahnungSeqNummer
		{
			get { return _mahnungSeqNummer; }
			set { _mahnungSeqNummer = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public decimal MahnungKosten
		{
			get { return _mahnungKosten; }
			set { _mahnungKosten = value; }
		}
		#endregion
	}
}
