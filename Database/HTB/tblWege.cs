/*
 * Author:			Generated Code
 * Date Created:	31.03.2011
 * Description:		Represents a row in the tblWege table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblWege : Record
	{
		#region Property Declaration
		private int _wEGID;
		private double _wegVon;
		private double _wegBis;
		private double _preis;
		
		public int WEGID
		{
			get { return _wEGID; }
			set { _wEGID = value; }
		}
		public double WegVon
		{
			get { return _wegVon; }
			set { _wegVon = value; }
		}
		public double WegBis
		{
			get { return _wegBis; }
			set { _wegBis = value; }
		}
		public double Preis
		{
			get { return _preis; }
			set { _preis = value; }
		}
		#endregion
	}
}
