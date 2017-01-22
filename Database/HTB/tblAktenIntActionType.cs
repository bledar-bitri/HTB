/*
 * Author:			Generated Code
 * Date Created:	08.06.2011
 * Description:		Represents a row in the tblAktenIntActionType table
*/

namespace HTB.Database
{
	public class tblAktenIntActionType : Record
	{
		#region Property Declaration

	    [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
	    public int AktIntActionTypeID { get; set; }

	    public string AktIntActionTypeCaption { get; set; }

	    public int ActionTypeNextStepID { get; set; }

	    public bool AktIntActionTypeIsInstallment { get; set; }

	    public bool AktIntActionNeedsSpecialCalc { get; set; }

	    public bool AktIntActionIsDefault { get; set; }

	    [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double AktIntActionProvAmount { get; set; }

	    public double AktIntActionProvPct { get; set; }

	    [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double AktIntActionProvAmountForZeroCollection { get; set; }

	    [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double AktIntActionProvPrice { get; set; }
	    public bool AktIntActionIsWithCollection { get; set; }
	    public bool AktIntActionIsExtensionRequest { get; set; }
	    public int AktIntActionProvHonGrpID { get; set; }
	    public bool AktIntActionIsTotalCollection { get; set; }
	    public bool AktIntActionIsVoid { get; set; }
	    public bool AktIntActionIsTelAndEmailCollection { get; set; }
	    public bool AktIntActionIsPositive { get; set; }
        public bool AktIntActionIsPersonalCollection { get; set; }
        public bool AktIntActionIsInternal { get; set; }
        public bool AktIntActionIsThroughPhone { get; set; }

        public bool AktIntActionIsAutoRepossessed { get; set; }
        public bool AktIntActionIsAutoMoneyCollected { get; set; }
        public bool AktIntActionIsAutoNegative { get; set; }
        public bool AktIntActionIsAutoPaymentInquiry { get; set; }
        public bool AktIntActionIsAutoPayment { get; set; }
        public bool AktIntActionIsDirectPayment { get; set; }
        public bool AktIntActionIsReceivable { get; set; }

        #endregion
	}
}
