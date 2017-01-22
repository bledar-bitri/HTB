/*
 * Author:			Generated Code
 * Date Created:	15.02.2011
 * Description:		Represents a row in the tblUserRoles table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblUserRoles
	{
		#region Property Declaration
		private int _userRoleID;
		private int _userRoleUser;
		private int _userRoleRole;
		public int UserRoleID
		{
			get { return _userRoleID; }
			set { _userRoleID = value; }
		}
		public int UserRoleUser
		{
			get { return _userRoleUser; }
			set { _userRoleUser = value; }
		}
		public int UserRoleRole
		{
			get { return _userRoleRole; }
			set { _userRoleRole = value; }
		}
		#endregion
	}
}
