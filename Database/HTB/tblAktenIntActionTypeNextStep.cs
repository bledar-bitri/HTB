/*
 * Author:			Generated Code
 * Date Created:	05.04.2011
 * Description:		Represents a row in the tblAktenIntActionTypeNextStep table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblAktenIntActionTypeNextStep : Record
	{
		#region Property Declaration
		private int _actionTypeNextStepID;
		private string _actionTypeNextStepCaption;
		private int _actionTypeNextStepSequenceNumber;
        private int _actionTypeNextStepCode;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int ActionTypeNextStepID
		{
			get { return _actionTypeNextStepID; }
			set { _actionTypeNextStepID = value; }
		}
		public string ActionTypeNextStepCaption
		{
			get { return _actionTypeNextStepCaption; }
			set { _actionTypeNextStepCaption = value; }
		}
		public int ActionTypeNextStepSequenceNumber
		{
			get { return _actionTypeNextStepSequenceNumber; }
			set { _actionTypeNextStepSequenceNumber = value; }
		}
        public int ActionTypeNextStepCode
        {
            get { return _actionTypeNextStepCode; }
            set { _actionTypeNextStepCode = value; }
        }
        #endregion
	}
}
