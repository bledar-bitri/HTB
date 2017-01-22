/*
 * Author:			Generated Code
 * Date Created:	22.02.2011
 * Description:		Represents a row in the tblUser table
*/

using System;
namespace HTB.Database
{
	public class tblUser : Record
	{
		#region Property Declaration

	    [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
	    public int UserID { get; set; }

	    public string UserVorname { get; set; }

	    public string UserNachname { get; set; }

	    public string UserUsername { get; set; }

	    public string UserPasswort { get; set; }

	    public string UserStrasse { get; set; }

	    public string UserZIPPrefix { get; set; }

	    public string UserZIP { get; set; }

	    public string UserOrt { get; set; }

	    public int UserState { get; set; }

	    public int UserSex { get; set; }

	    public DateTime UserGebDat { get; set; }

	    public int UserLevel { get; set; }

	    public string UserEMailOffice { get; set; }

	    public string UserEMailPrivate { get; set; }

	    public string UserPhoneOfficeCountry { get; set; }

	    public string UserPhoneOfficeCity { get; set; }

	    public string UserPhoneOffice { get; set; }

	    public string UserPhonePrivateCountry { get; set; }

	    public string UserPhonePrivateCity { get; set; }

	    public string UserPhonePrivate { get; set; }

	    public string UserGSMOfficeCountry { get; set; }

	    public string UserGSMOfficeCity { get; set; }

	    public string UserGSMOffice { get; set; }

	    public string USERGSMPrivateCountry { get; set; }

	    public string USERGSMPrivateCity { get; set; }

	    public string USERGSMPrivate { get; set; }

	    public string USERFaxOfficeCountry { get; set; }

	    public string USERFaxOfficeCity { get; set; }

	    public string USERFaxOffice { get; set; }

	    public string UserFaxPrivateCountry { get; set; }

	    public string UserFaxPrivateCity { get; set; }

	    public string UserFaxPrivate { get; set; }

	    public DateTime UserLastLogin { get; set; }

	    public int UserStatus { get; set; }

	    public string UserPic { get; set; }

	    public int UserRowCount { get; set; }

	    public int UserMandant { get; set; }

	    public int UserAbteilung { get; set; }

	    public int UserKlient { get; set; }

	    public int UserRoleALAD { get; set; }

	    public int UserRoleGL { get; set; }

	    public int UserRoleALBonitaet { get; set; }

	    public int UserRoleIntAkt { get; set; }

	    public int UserChangesNewsLetter { get; set; }

	    public double UserBezBrutto { get; set; }

	    public double UserBezDGSV { get; set; }

	    public double UserBezKS { get; set; }

	    public double UserBezDB { get; set; }

	    public double UserBezDZ { get; set; }

	    public double UserBezUBahn { get; set; }

	    public double UserBezMV { get; set; }

	    public double UserBezSach { get; set; }

	    public string UserPopServer { get; set; }

	    public string UserPopAccount { get; set; }

	    public string UserPopPassword { get; set; }

	    public string UserPopEMailadresse { get; set; }

	    public int UserPOPPort { get; set; }

	    public DateTime UserLastPWChange { get; set; }

	    public DateTime UserEintrittsdatum { get; set; }

	    public int UserUrlaubstage { get; set; }

	    public int UserRoleFB { get; set; }

	    public int UserReadReceipt { get; set; }

	    public int UserAG { get; set; }

	    public bool UserIsSbAdmin { get; set; }
        public bool UserIsLocked { get; set; }
        public string UserLockReason { get; set; }
        public bool UserHasIPad { get; set; }

//        public double UserWerbungProvisionPct { get; set; }
//        public double UserWerbungProvisionAmount { get; set; }


	    #endregion
	}
}
