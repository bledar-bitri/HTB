/*
 * Author:			Generated Code
 * Date Created:	09.06.2011
 * Description:		Represents a row in the tblUserAktionRel table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblUserAktionRel : Record
	{
		#region Property Declaration
		private int _userAktionAktIntActionTypeID;
		private int _userAktionUserID;
		public int UserAktionAktIntActionTypeID
		{
			get { return _userAktionAktIntActionTypeID; }
			set { _userAktionAktIntActionTypeID = value; }
		}
		public int UserAktionUserID
		{
			get { return _userAktionUserID; }
			set { _userAktionUserID = value; }
		}
		#endregion
	}
}
