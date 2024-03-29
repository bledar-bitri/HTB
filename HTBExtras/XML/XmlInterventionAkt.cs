﻿using System.Collections;
using HTB.Database;
using HTB.Database.Views;

namespace HTBExtras.XML
{
    public class XmlInterventionAkt : qryAktenInt
    {
        public bool AktHasActions { get; set; }
        
        public AktIntAmounts aktIntAmounts { get; set; }
        public ArrayList aktDocuments { get; set; }
        public ArrayList addresses { get; set; }
        public ArrayList addresses2 { get; set; }
        public ArrayList phones { get; set; }
        public ArrayList phones2 { get; set; }

        public tblProtokol protocol { get; set; }
        public tblProtokolUbername protocolUbername { get; set; }

        public  XmlInterventionAkt()
        {
            aktIntAmounts = new AktIntAmounts();
            aktDocuments = new ArrayList();
            addresses = new ArrayList();
            addresses2 = new ArrayList();
            phones = new ArrayList();
            phones2 = new ArrayList();
            protocol = new tblProtokol();
            protocolUbername = new tblProtokolUbername();
        }

        public void AddDocument(Record doc, string scheme, string host)
        {
            aktDocuments.Add(new AktDocument(doc, scheme, host));
        }

        public void addAddress(Record address)
        {
            addresses.Add(address);
        }
        public void addAddress2(Record address)
        {
            addresses2.Add(address);
        }

        public void addPhone(Record phone)
        {
            phones.Add(phone);
        }
        public void addPhone2(Record phone)
        {
            phones2.Add(phone);
        }
    }
}
