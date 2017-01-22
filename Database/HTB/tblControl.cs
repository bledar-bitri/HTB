/*
 * Author:			Generated Code
 * Date Created:	15.06.2011
 * Description:		Represents a row in the tblControl table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblControl : Record
	{
		#region Property Declaration

	    [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID)]
	    public string ControlCode { get; set; }

	    public double TaxRate { get; set; }

	    public int GracePeriod { get; set; }
        public int TerminverlustGracePeriod { get; set; }
        
	    public int InkassoAktInterventionStatus { get; set; }

	    public int MeldeKostenArtId { get; set; }

	    public int MeldePeriod { get; set; }

//	    public string LawyerEmail { get; set; }

	    public int MeldeSB { get; set; }

	    public string LawyerEmailSubject { get; set; }

	    public int ProcessCodeInstallment { get; set; }

	    public int ProcessCodeDone { get; set; }

	    public int InkassoAktStornoStatus { get; set; }

	    public int InkassoAktFertigStatus { get; set; }

	    public int MeldeKostenAuslandArtId { get; set; }

	    public int RechtsanwaldKostenArtId { get; set; }

	    public int InkassoAktMeldeStatus { get; set; }

	    public int InkassoAktInkassoStatus { get; set; }

	    public int MeldeResearchSB { get; set; }

	    public string MeldeEmailSubject { get; set; }

	    public int InkassoAktWaitingForReMeldeStatus { get; set; }

	    public int InkassoAktWaitForReMeldePeriod { get; set; }

	    public string MahnungEmail { get; set; }

	    public string MahnungEmailSubject { get; set; }

	    public string SMTPServer { get; set; }

	    public string SMTPUser { get; set; }

	    public string SMTPPW { get; set; }

        public int SMTPPort { get; set; }

	    public int AutoUserId { get; set; }

	    public int DefaultPaymentWaitPeriod { get; set; }

	    public string KlientReceiptEmailSubject { get; set; }

	    public string KlientReceiptTime { get; set; }

	    public string KlientNotificationSubject { get; set; }

	    public string KlientNotificationTime { get; set; }

	    public int KlientNotificationPeriod { get; set; }

	    public int InkassoAktWflDoneStatus { get; set; }
        public int InkassoAktSentToPartnerStatus { get; set; }

        public int InternalActionGegnerNotFoundPersonalVisit { get; set; }
        public int InternalActionGegnerNotFoundTelefon { get; set; }
        public int InkassoAktInstallmentStatus { get; set; }
        public int InkassoAktOverchargeStatus { get; set; }
        public int InterventionTypeTerminverlust { get; set; }
        public int InterventionKostenArtId { get; set; }
        public double AnnualInterestRate { get; set; }
        public int DefaultLawyerId { get; set; }
        public int DefaultSB { get; set; }
        #endregion
	}
}
