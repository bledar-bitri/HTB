/*
 * Author:			Generated Code
 * Date Created:	30.06.2011
 * Description:		Represents a row in the tblCommunicationLog table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblCommunicationLog : Record
	{
        public const int COMMUNICATION_TYPE_RECEIPT = 1;
        public const int COMMUNICATION_TYPE_NOTIFICATION = 2;
        public const int COMMUNICATION_TYPE_EXTENSION_REQUEST = 3;

		#region Property Declaration
		private int _comLogID;
		private int _comLogKlientID;
		private int _comLogType;
		private DateTime _comLogDate;
		private int _comLogAuftraggeberID;
		private int _comLogCusInkAktID;
		private int _comLogAktIntID;
        private string _comLogAuftraggeberSB;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int ComLogID
		{
			get { return _comLogID; }
			set { _comLogID = value; }
		}
		public int ComLogKlientID
		{
			get { return _comLogKlientID; }
			set { _comLogKlientID = value; }
		}
		public int ComLogType
		{
			get { return _comLogType; }
			set { _comLogType = value; }
		}
		public DateTime ComLogDate
		{
			get { return _comLogDate; }
			set { _comLogDate = value; }
		}
		public int ComLogAuftraggeberID
		{
			get { return _comLogAuftraggeberID; }
			set { _comLogAuftraggeberID = value; }
		}
		public int ComLogCusInkAktID
		{
			get { return _comLogCusInkAktID; }
			set { _comLogCusInkAktID = value; }
		}
		public int ComLogAktIntID
		{
			get { return _comLogAktIntID; }
			set { _comLogAktIntID = value; }
		}
        public string ComLogAuftraggeberSB
        {
            get { return _comLogAuftraggeberSB; }
            set { _comLogAuftraggeberSB = value; }
        }
        #endregion
	}
}
