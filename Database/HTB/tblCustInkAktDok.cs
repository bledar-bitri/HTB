/*
 * Author:			Generated Code
 * Date Created:	25.02.2011
 * Description:		Represents a row in the tblCustInkAktDok table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblCustInkAktDok : Record
	{
		#region Property Declaration
		private int _custInkAktDokID;
		private int _custInkAktDokAkt;
		private DateTime _custInkAktDokDate;
		private DateTime _custInkAktDokDueDate;
		private DateTime _custInkAktNextWorkflowStep;
		private double _custInkAktDokCapital;
		private double _custInkAktDokInterest;
		private double _custInkAktDokIncurredCosts;
		private double _custInkAktDokSumCosts;
		private double _custInkAktDokProcessingFee;
		private double _custInkAktDokFee;
		private double _custInkAktDokEvidenceFee;
		private double _custInkAktDokPostage;
		private double _custInkAktDokCostClient;
		private double _custInkAktDokSumCapital;
		private int _custInkAktDokType;
		private int _custInkAktDokPrintFlag;
		private int _custInkAktDokDelFlag;
		private int _custInkAktDokEnterUser;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber=true)]
		public int CustInkAktDokID
		{
			get { return _custInkAktDokID; }
			set { _custInkAktDokID = value; }
		}
		public int CustInkAktDokAkt
		{
			get { return _custInkAktDokAkt; }
			set { _custInkAktDokAkt = value; }
		}
		public DateTime CustInkAktDokDate
		{
			get { return _custInkAktDokDate; }
			set { _custInkAktDokDate = value; }
		}
		public DateTime CustInkAktDokDueDate
		{
			get { return _custInkAktDokDueDate; }
			set { _custInkAktDokDueDate = value; }
		}
		public DateTime CustInkAktNextWorkflowStep
		{
			get { return _custInkAktNextWorkflowStep; }
			set { _custInkAktNextWorkflowStep = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double CustInkAktDokCapital
		{
			get { return _custInkAktDokCapital; }
			set { _custInkAktDokCapital = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double CustInkAktDokInterest
		{
			get { return _custInkAktDokInterest; }
			set { _custInkAktDokInterest = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double CustInkAktDokIncurredCosts
		{
			get { return _custInkAktDokIncurredCosts; }
			set { _custInkAktDokIncurredCosts = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double CustInkAktDokSumCosts
		{
			get { return _custInkAktDokSumCosts; }
			set { _custInkAktDokSumCosts = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double CustInkAktDokProcessingFee
		{
			get { return _custInkAktDokProcessingFee; }
			set { _custInkAktDokProcessingFee = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double CustInkAktDokFee
		{
			get { return _custInkAktDokFee; }
			set { _custInkAktDokFee = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double CustInkAktDokEvidenceFee
		{
			get { return _custInkAktDokEvidenceFee; }
			set { _custInkAktDokEvidenceFee = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double CustInkAktDokPostage
		{
			get { return _custInkAktDokPostage; }
			set { _custInkAktDokPostage = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double CustInkAktDokCostClient
		{
			get { return _custInkAktDokCostClient; }
			set { _custInkAktDokCostClient = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double CustInkAktDokSumCapital
		{
			get { return _custInkAktDokSumCapital; }
			set { _custInkAktDokSumCapital = value; }
		}
		public int CustInkAktDokType
		{
			get { return _custInkAktDokType; }
			set { _custInkAktDokType = value; }
		}
		public int CustInkAktDokPrintFlag
		{
			get { return _custInkAktDokPrintFlag; }
			set { _custInkAktDokPrintFlag = value; }
		}
		public int CustInkAktDokDelFlag
		{
			get { return _custInkAktDokDelFlag; }
			set { _custInkAktDokDelFlag = value; }
		}
		public int CustInkAktDokEnterUser
		{
			get { return _custInkAktDokEnterUser; }
			set { _custInkAktDokEnterUser = value; }
		}
		#endregion
	}
}
