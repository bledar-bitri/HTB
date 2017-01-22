/*
 * Author:			Generated Code
 * Date Created:	27.07.2011
 * Description:		Represents a row in the tblMandant table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblMandant : Record
	{
		#region Property Declaration
		private int _mandantID;
		private string _mandantCaption;
		private string _mandantLangtext;
		private string _mandantBankBez1;
		private string _mandantBankBLZ1;
		private string _mandantBankKto1;
		private string _mandantBankBez2;
		private string _mandantBankBLZ2;
		private string _mandantBankKto2;
		private string _mandantBankBez3;
		private string _mandantBankBLZ3;
		private string _mandantBankKto3;
		private string _mandantBankBez4;
		private string _mandantBankBLZ4;
		private string _mandantBankKto4;
		private string _mandantBankBez5;
		private string _mandantBankBLZ5;
		private string _mandantBankKto5;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int MandantID
		{
			get { return _mandantID; }
			set { _mandantID = value; }
		}
		public string MandantCaption
		{
			get { return _mandantCaption; }
			set { _mandantCaption = value; }
		}
		public string MandantLangtext
		{
			get { return _mandantLangtext; }
			set { _mandantLangtext = value; }
		}
		public string MandantBankBez1
		{
			get { return _mandantBankBez1; }
			set { _mandantBankBez1 = value; }
		}
		public string MandantBankBLZ1
		{
			get { return _mandantBankBLZ1; }
			set { _mandantBankBLZ1 = value; }
		}
		public string MandantBankKto1
		{
			get { return _mandantBankKto1; }
			set { _mandantBankKto1 = value; }
		}
		public string MandantBankBez2
		{
			get { return _mandantBankBez2; }
			set { _mandantBankBez2 = value; }
		}
		public string MandantBankBLZ2
		{
			get { return _mandantBankBLZ2; }
			set { _mandantBankBLZ2 = value; }
		}
		public string MandantBankKto2
		{
			get { return _mandantBankKto2; }
			set { _mandantBankKto2 = value; }
		}
		public string MandantBankBez3
		{
			get { return _mandantBankBez3; }
			set { _mandantBankBez3 = value; }
		}
		public string MandantBankBLZ3
		{
			get { return _mandantBankBLZ3; }
			set { _mandantBankBLZ3 = value; }
		}
		public string MandantBankKto3
		{
			get { return _mandantBankKto3; }
			set { _mandantBankKto3 = value; }
		}
		public string MandantBankBez4
		{
			get { return _mandantBankBez4; }
			set { _mandantBankBez4 = value; }
		}
		public string MandantBankBLZ4
		{
			get { return _mandantBankBLZ4; }
			set { _mandantBankBLZ4 = value; }
		}
		public string MandantBankKto4
		{
			get { return _mandantBankKto4; }
			set { _mandantBankKto4 = value; }
		}
		public string MandantBankBez5
		{
			get { return _mandantBankBez5; }
			set { _mandantBankBez5 = value; }
		}
		public string MandantBankBLZ5
		{
			get { return _mandantBankBLZ5; }
			set { _mandantBankBLZ5 = value; }
		}
		public string MandantBankKto5
		{
			get { return _mandantBankKto5; }
			set { _mandantBankKto5 = value; }
		}
		#endregion
	}
}
