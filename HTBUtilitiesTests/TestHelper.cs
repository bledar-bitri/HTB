using System;
using HTB.Database;
using HTBUtilities;

namespace HTBUtilitiesTests
{
    public static class TestHelper
    {
        /// <summary>
        /// send the gegnerOldId to identify the gegner
        /// </summary>
        /// <param name="gegnerOldId">the gegner old id</param>
        /// <returns></returns>
        public static tblGegner GetGegner(string gegnerOldId)
        {
            return new tblGegner
            {
                GegnerAnrede = "HERR",
                GegnerAnsprech = "Tester",
                GegnerAnsprechAnrede = "Frau",
                GegnerAnsprechVorname = "Maria",
                GegnerArbeitgeber = "MusterAG",
                GegnerBeruf = "SuchtJob",
                GegnerCreateDate = DateTime.Now,
                GegnerCreateSB = 99,
                GegnerOldID = gegnerOldId,
                GegnerFax = "111 111 1111",
                GegnerFaxCity = "111",
                GegnerFaxCountry = "43",
                GegnerGebDat = new DateTime(1970, 1, 1),
                GegnerImported = 0,
                GegnerLastName1 = "Muster",
                GegnerLastName2 = "John",
                GegnerLastName3 = "",
                GegnerLastOrt = "SBG",
                GegnerLastStrasse = "Musterstrasse 1",
                GegnerLastZip = "12345",
                GegnerLastZipPrefix = "A"
            };
        }

        /// <summary>
        /// finds the inserted gegner
        /// </summary>
        /// <param name="gegnerOldId">the gegner old id</param>
        /// <returns>the found gegner</returns>
        public static tblGegner GetInsertedGegner(string gegnerOldId)
        {
            return (tblGegner)HTBUtils.GetSqlSingleRecord(string.Format("select * from tblgegner where GegnerOldID = '{0}'", gegnerOldId), typeof(tblGegner));
        }


        /// <summary>
        /// send the klientOldId to identify the klient... with a 'k' ;)
        /// </summary>
        /// <param name="klientOldId">the klientOldId</param>
        /// <returns>new klient</returns>
        public static tblKlient GetKlient(string klientOldId)
        {
            return new tblKlient
            {
                KlientType = 5,
                KlientAnrede = "HERR",
                KlientAnsprech = "Tester",
                KlientAccountManager = 0,
                KlientAccountManager2 = 1,
                KlientBIC = "MusterBIC",
                KlientBLZ1 = "MusetrBLZ1",
                KlientBLZ2 = "MusterBLZ2",
                KlientBLZ3 = "MusterBLZ3",
                KlientOldID = klientOldId,
                KlientFax = "111 111 1111",
                KlientFaxCity = "111",
                KlientFaxCountry = "43",
                KlientINKISImportDate = DateTime.Now,
                KlientShowGebdat = 0,
                KlientName1 = "Musternachname",
                KlientName2 = "John",
                KlientName3 = "",
                KlientOrt = "SBG",
                KlientStrasse = "Musterstrasse 1",
                KlientPLZ = "12345",
                KlientLKZ = "A"
            };
        }

        /// <summary>
        /// send klientOldId to identify the klient
        /// </summary>
        /// <returns>the first klient found in the tblKlient</returns>
        public static tblKlient GetInsertedKlient(string klientOldId)
        {
            return (tblKlient)HTBUtils.GetSqlSingleRecord(string.Format("select * from tblKlient where KlientOldID = '{0}'", klientOldId), typeof(tblKlient));
        }


        /// <summary>
        /// returns a new inkasso akt record
        /// </summary>
        /// <param name="aktAz">to indentify the akt</param>
        /// <param name="gegnerId">the gegner id</param>
        /// <param name="klientId">the klient id</param>
        /// <returns>the akt</returns>
        public static tblCustInkAkt GetCustInkAkt(string aktAz, int gegnerId, int klientId)
        {
            return new tblCustInkAkt
            {
                CustInkAktAZ = aktAz,
                CustInkAktKlient = klientId,
                CustInkAktGegner = gegnerId,
                CustInkAktBetragOffen = 100,
                CustInkAktCurStatus = 2,
                CustInkAktStatus = 1,
                CustInkAktEnterDate = DateTime.Now,
                CustInkAktForderung = 100,
                CustInkAktInvoiceDate = new DateTime(1970, 1, 1),
                CustInkAktIsPartial = true,
                CustInkAktIsWflStopped = true,

            };
        }

        /// <summary>
        /// returns the first akt with the AZ found in the tblCustInkAkt table
        /// </summary>
        /// <param name="aktAz">the akt az</param>
        /// <returns>the found akt</returns>
        public static tblCustInkAkt GetInsertedCustInkAkt(string aktAz)
        {
            return (tblCustInkAkt)HTBUtils.GetSqlSingleRecord(string.Format("select * from tblCustInkAkt where CustInkAktAZ = '{0}'", aktAz), typeof(tblCustInkAkt));
        }


        /// <summary>
        /// creates new inkasso document record... the caption is used to identify the document
        /// </summary>
        /// <param name="aktId">the inkasso akt</param>
        /// <param name="userId">the user id </param>
        /// <param name="docCaption">document caption</param>
        /// <param name="fileName">the attachment file name</param>
        /// <returns>the document</returns>
        public static tblDokument GetDokument(int aktId, int userId, string docCaption, string fileName)
        {
            return new tblDokument
            {
                // Inkasso
                DokDokType = 25,
                DokCaption = docCaption,
                DokInkAkt = aktId,
                DokCreator = userId,
                DokAttachment = fileName,
                DokCreationTimeStamp = DateTime.Now,
                DokChangeDate = DateTime.Now
            };

        }

        /// <summary>
        /// returs the first document found with the caption...
        /// </summary>
        /// <param name="docCaption">the document caption to look for</param>
        /// <returns>the document found</returns>
        public static tblDokument GetInsertedDokument(string docCaption)
        {
            return (tblDokument)HTBUtils.GetSqlSingleRecord(string.Format("select * from tblDokument where DokCaption = '{0}'", docCaption), typeof(tblDokument));
        }

        /// <summary>
        /// creates new akten dokumente record
        /// </summary>
        /// <param name="aktId">the akt id</param>
        /// <param name="dokId">the document id</param>
        /// <returns>the created akten dokumente record</returns>
        public static tblAktenDokumente GetAktenDokumente(int aktId, int dokId)
        {

            return new tblAktenDokumente
            {
                ADAkt = aktId,
                ADDok = dokId,
                ADAkttyp = 1
            };
        }

        /// <summary>
        /// returns the first akten dokumente record found with aktid.. and dokId..
        /// </summary>
        /// <param name="aktId">the akt id</param>
        /// <param name="dokId">the document id</param>
        /// <returns>the first akten dokumente record found</returns>
        public static tblAktenDokumente GetInsertedAktenDokumente(int aktId, int dokId)
        {
            return (tblAktenDokumente)HTBUtils.GetSqlSingleRecord(string.Format("select * from tblAktenDokumente where ADAkt = {0} and ADDok = {1}", aktId, dokId), typeof(tblAktenDokumente));
        }

        /// <summary>
        /// creates a test document file in the "directory" folder
        /// </summary>
        /// <param name="directory">the directory path</param>
        /// <param name="fileName">the file name</param>
        public static void CreateTestDocumentFile(string directory, string fileName)
        {
            HTBUtils.SaveTextFile(string.Format("{0}/{1}", directory, fileName), "Unit Test File");
        }

        /// <summary>
        /// deletes the test document file in the "directory" folder
        /// </summary>
        /// <param name="directory">the directory path</param>
        /// <param name="fileName">the name of the file to delete</param>
        public static void DeleteTestDocumentFile(string directory, string fileName)
        {
            HTBUtils.DeleteFile(string.Format("{0}/{1}", directory, fileName));
        }
    }
}
