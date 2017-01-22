/*
 * Author:			Generated Code
 * Date Created:	23.02.2011
 * Description:		Represents a row in the tblMahnung table
*/

using System;
using System.Collections;
namespace HTB.Database
{
    public class tblMahnung : Record
	{
		#region Property Declaration

       
        [MappingAttribute(FieldType=MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber=true)]
        public int MahnungID { get; set; }

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID)]
        public int MahnungNr { get; set; }

        public DateTime MahnungDate { get; set; }

        public string MahnungAnrede { get; set; }

        public int MahnungKlient { get; set; }

        public int MahnungStatus { get; set; }

        public int MahnungAktIntID { get; set; }

        public int MahnungAktID { get; set; }

        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double MahnungSumme { get; set; }

        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double MahnungSteuer { get; set; }

        public int MahnungType { get; set; }

        public int MahnungRateID { get; set; }
        public string MahnungXMLPath { get; set; }

        #endregion

        private ArrayList _kostenList = new ArrayList();
        
        [MappingAttribute(FieldType=MappingAttribute.NO_DB_SAVE)]
        public ArrayList KostenList
        {
            get { return _kostenList; }
            set { _kostenList = value; }
        }
	}
}
