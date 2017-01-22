/*
 * Author:			Generated Code
 * Date Created:	31.03.2011
 * Description:		Represents a row in the tblADGebiete table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblADGebiete : Record
	{
		#region Property Declaration
		private int _aDGebieteID;
		private int _aDGebietUser;
		private int _aDGebietStartZIP;
		private int _aDGebietEndZip;
		public int ADGebieteID
		{
			get { return _aDGebieteID; }
			set { _aDGebieteID = value; }
		}
		public int ADGebietUser
		{
			get { return _aDGebietUser; }
			set { _aDGebietUser = value; }
		}
		public int ADGebietStartZIP
		{
			get { return _aDGebietStartZIP; }
			set { _aDGebietStartZIP = value; }
		}
		public int ADGebietEndZip
		{
			get { return _aDGebietEndZip; }
			set { _aDGebietEndZip = value; }
		}
		#endregion
	}
}
