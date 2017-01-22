using System;
using HTB.Database;
using HTB.Database.Views;

namespace HTBExtras
{
    public class AktDocument : Record
    {
        public string DocCreationTimeStamp { get; set; }
        public string DocTypeCaption { get; set; }
        public string DocCaption { get; set; }
        public string DocAttachment { get; set; }
        public string DocURL { get; set; }
        public DateTime DocChangeDate { get; set; }
        [MappingAttribute(FieldType = MappingAttribute.NO_DB_SAVE)]
        public long DocTimestamp { get; set; }
        [MappingAttribute(FieldType = MappingAttribute.NO_DB_SAVE)]
        public string DocEmail { get; set; }

        public AktDocument()
        {
            
        }
        public AktDocument(Record rec, string scheme, string host)
        {
            AssignRecord(rec, scheme, host);
        }

        private void AssignRecord(Record rec, string scheme, string host)
        {
            if(rec is qryDoksIntAkten)
            {
                Assign((qryDoksIntAkten)rec, scheme, host);
            }
            else if (rec is qryDoksInkAkten)
            {
                Assign((qryDoksInkAkten)rec, scheme, host);
            }
        }
        private void Assign(qryDoksIntAkten rec, string scheme, string host)
        {
            DocCreationTimeStamp = rec.DokCreationTimeStamp + " von " + rec.UserVorname + " " + rec.UserNachname;
            DocTypeCaption = rec.DokTypeCaption;
            DocCaption = rec.DokCaption;
            DocAttachment = rec.DokAttachment;
            if (!rec.DokAttachment.StartsWith("http"))
                DocURL = scheme + "://" + host + "/v2/intranet/documents/files/" + DocAttachment;
            else
                DocURL = DocAttachment;
            DocChangeDate = rec.DokChangeDate;
            DocTimestamp = rec.DokTimestamp;
        }

        private void Assign(qryDoksInkAkten rec, string scheme, string host)
        {
            DocCreationTimeStamp = rec.DokCreationTimeStamp + " von " + rec.UserVorname + " " + rec.UserNachname;
            DocTypeCaption = rec.DokTypeCaption;
            DocCaption = rec.DokCaption;
            DocAttachment = rec.DokAttachment;
            if (!rec.DokAttachment.StartsWith("http"))
                DocURL = scheme + "://" + host + "/v2/intranet/documents/files/" + DocAttachment;
            else
                DocURL = DocAttachment;
            DocChangeDate = rec.DokChangeDate;
            DocTimestamp = rec.DokTimestamp;
        }
    }
}
