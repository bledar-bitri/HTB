/*
 * Author:			Generated Code
 * Date Created:	01.04.2011
 * Description:		Represents a row in the tblDokument table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
    public class tblDokument : Record
    {
        #region Property Declaration

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
        public int DokID { get; set; }

        public int DokDokType { get; set; }
        public int DokCreator { get; set; }
        public string DokCaption { get; set; }
        public int DokLevel { get; set; }
        public int DokStatus { get; set; }
        public DateTime DokCreationTimeStamp { get; set; }
        public int DokKlient { get; set; }
        public int DokGegner { get; set; }
        public string DokAttachment { get; set; }
        public int DokPublish { get; set; }
        public int DokAbteilung { get; set; }
        public int DokAuftraggeber { get; set; }
        public int DokProjekt { get; set; }
        public int DokAEAkt { get; set; }
        public int DokInkAkt { get; set; }
        public int DokIntAkt { get; set; }
//	    public int DokAutoAkt { get; set; }
        public DateTime DokChangeDate { get; set; }
        public int DokChangeUser { get; set; }
        public string DokText { get; set; }

        [MappingAttribute(FieldType = MappingAttribute.NO_DB_SAVE)]
        public long DokTimestamp { get; set; }

        public bool DokSourceIsIPad { get; set; }

    #endregion
    }
}
