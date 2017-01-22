using System;
using System.Text;
using HTB.Database;
using HTB.Database.Views;

namespace HTBExtras.KingBill
{
    public class InkassoAktDokument : Record
    {
        public string DokumentBeschreibung { get; set; }
        public string DokumentURL { get; set; }
        public DateTime DokumentDatum { get; set; }

        public InkassoAktDokument()
        {
            
        }
        public InkassoAktDokument(Object doc)
        {
            Assign(doc);
        }
        public void Assign(Object doc)
        {
            if(doc is qryDoksIntAkten)
                Assign((qryDoksIntAkten)doc);
            else if (doc is qryDoksInkAkten)
                Assign((qryDoksInkAkten)doc);
        }

        public void Assign(qryDoksIntAkten doc)
        {
            DokumentURL = GetDocAttachmentURL(doc.DokAttachment);
            DokumentDatum = doc.DokCreationTimeStamp;
            DokumentBeschreibung = doc.DokCaption;
        }

        public void Assign(qryDoksInkAkten doc)
        {
            DokumentURL = GetDocAttachmentURL(doc.DokAttachment);
            DokumentDatum = doc.DokChangeDate;
            DokumentBeschreibung = doc.DokCaption;
        }

        private string GetDocAttachmentURL(string attachment)
        {
            var sb = new StringBuilder();
            if(attachment.ToLower().StartsWith("http"))
            {
                sb.Append(attachment);
            }
            else
            {
                sb.Append("http://htb.ecp.or.at/v2/intranet/documents/files/");
                sb.Append(attachment);
            }
            return sb.ToString();
        }
    }
}
