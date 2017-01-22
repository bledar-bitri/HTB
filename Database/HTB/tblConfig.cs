/*
 * Author:			Generated Code
 * Date Created:	08.03.2011
 * Description:		Represents a row in the tblConfig table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblConfig : Record
	{
		#region Property Declaration
		private int _configID;
		private string _configName;
		private string _configValue;
		public int ConfigID
		{
			get { return _configID; }
			set { _configID = value; }
		}
		public string ConfigName
		{
			get { return _configName; }
			set { _configName = value; }
		}
		public string ConfigValue
		{
			get { return _configValue; }
			set { _configValue = value; }
		}
		#endregion
	}
}
