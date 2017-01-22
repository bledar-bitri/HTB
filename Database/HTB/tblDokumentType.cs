/*
 * Author:			Generated Code
 * Date Created:	01.04.2011
 * Description:		Represents a row in the tblDokumentType table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblDokumentType : Record
	{
		#region Property Declaration
		private int _dokTypeID;
		private string _dokTypeCaption;
		public int DokTypeID
		{
			get { return _dokTypeID; }
			set { _dokTypeID = value; }
		}
		public string DokTypeCaption
		{
			get { return _dokTypeCaption; }
			set { _dokTypeCaption = value; }
		}
		#endregion
	}
}
