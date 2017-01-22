
using System.IO;
using HTB.Database;
using HTBUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HTBUtilitiesTests
{
    [TestClass]
    public class HtbUtilsUnitTests
    {

        private tblControl _control;
        private string _documentsDirectory;

        #region test initialization
        [TestInitialize]
        public void InitializeTest()
        {
            _control = HTBUtils.GetControlRecord();
            _documentsDirectory = HTBUtils.GetConfigValue("DocumentsFolder");
            DeleteLeftoverRecords();
        }
        #endregion


        #region test methods
       
        [TestMethod]
        public void InkassoAkt_With_One_Document_DeletionTest()
        {

            // set up
            const string aktAz = "test_akt_deletion";
            const string testFileName = "_tmpHtbUtilsUnitTestFile.txt";
            const string oldid = aktAz;
            const string docCaption = aktAz;
            
            TestHelper.CreateTestDocumentFile(_documentsDirectory, testFileName);

            RecordSet.Insert(TestHelper.GetGegner(oldid));
            RecordSet.Insert(TestHelper.GetKlient(oldid));
            var gegner = TestHelper.GetInsertedGegner(oldid);
            var klient = TestHelper.GetInsertedKlient(oldid);

            Assert.IsNotNull(gegner);
            Assert.IsNotNull(klient);

            RecordSet.Insert(TestHelper.GetCustInkAkt(aktAz, gegner.GegnerID, klient.KlientID));
            var akt = TestHelper.GetInsertedCustInkAkt(aktAz);
            Assert.IsNotNull(akt);


            RecordSet.Insert(TestHelper.GetDokument(akt.CustInkAktID, _control.AutoUserId, docCaption, testFileName));
            var document = TestHelper.GetInsertedDokument(docCaption);
            Assert.IsNotNull(akt);

            RecordSet.Insert(TestHelper.GetAktenDokumente(akt.CustInkAktID, document.DokID));
            var aktenDokumente = TestHelper.GetInsertedAktenDokumente(akt.CustInkAktID, document.DokID);
            Assert.IsNotNull(aktenDokumente);

            // keep a hold of the aktId and docId for aktenDokumente lookup
            var aktId = akt.CustInkAktID;
            var docId = document.DokID;
            
            // Act
            HTBUtils.DeleteInkassoAkt(akt.CustInkAktID);

            // check
            akt = TestHelper.GetInsertedCustInkAkt(aktAz);
            Assert.IsNull(akt);

            document = TestHelper.GetInsertedDokument(docCaption);
            Assert.IsNull(document);

            aktenDokumente = TestHelper.GetInsertedAktenDokumente(aktId, docId);
            Assert.IsNull(aktenDokumente);

            Assert.IsFalse(File.Exists(string.Format("{0}/{1}",_documentsDirectory, testFileName)));

            // clean up
            RecordSet.Delete(gegner);
            gegner = TestHelper.GetInsertedGegner(oldid);
            Assert.IsNull(gegner);

            RecordSet.Delete(klient);
            klient = TestHelper.GetInsertedKlient(oldid);
            Assert.IsNull(klient);
        }

        [TestMethod]
        public void InkassoAkt_With_Multiple_DocumentsD_eletionTest()
        {

            // set up
            const string aktAz = "test_akt_deletion";
            const string testFileName1 = "_tmpHtbUtilsUnitTestFile.txt";
            const string testFileName2 = "_tmpHtbUtilsUnitTestFile2.txt";
            const string oldid = aktAz;
            const string docCaption1 = aktAz;
            string docCaption2 = string.Format("{0}2",docCaption1);

            TestHelper.CreateTestDocumentFile(_documentsDirectory, testFileName1);
            TestHelper.CreateTestDocumentFile(_documentsDirectory, testFileName2);

            RecordSet.Insert(TestHelper.GetGegner(oldid));
            RecordSet.Insert(TestHelper.GetKlient(oldid));
            var gegner = TestHelper.GetInsertedGegner(oldid);
            var klient = TestHelper.GetInsertedKlient(oldid);

            Assert.IsNotNull(gegner);
            Assert.IsNotNull(klient);

            RecordSet.Insert(TestHelper.GetCustInkAkt(aktAz, gegner.GegnerID, klient.KlientID));
            var akt = TestHelper.GetInsertedCustInkAkt(aktAz);
            Assert.IsNotNull(akt);


            RecordSet.Insert(TestHelper.GetDokument(akt.CustInkAktID, _control.AutoUserId, docCaption1, testFileName1));
            var document1 = TestHelper.GetInsertedDokument(docCaption1);
            Assert.IsNotNull(document1);

            RecordSet.Insert(TestHelper.GetAktenDokumente(akt.CustInkAktID, document1.DokID));
            var aktenDokumente1 = TestHelper.GetInsertedAktenDokumente(akt.CustInkAktID, document1.DokID);
            Assert.IsNotNull(aktenDokumente1);
            // second document
            RecordSet.Insert(TestHelper.GetDokument(akt.CustInkAktID, _control.AutoUserId, docCaption2, testFileName2));
            var document2 = TestHelper.GetInsertedDokument(docCaption2);
            Assert.IsNotNull(document2);

            RecordSet.Insert(TestHelper.GetAktenDokumente(akt.CustInkAktID, document2.DokID));
            var aktenDokumente2 = TestHelper.GetInsertedAktenDokumente(akt.CustInkAktID, document2.DokID);
            Assert.IsNotNull(aktenDokumente2);

            // keep a hold of the aktId and docId for aktenDokumente lookup
            var aktId = akt.CustInkAktID;
            var docId1 = document1.DokID;
            var docId2 = document2.DokID;

            // Act
            HTBUtils.DeleteInkassoAkt(akt.CustInkAktID);

            // check
            akt = TestHelper.GetInsertedCustInkAkt(aktAz);
            Assert.IsNull(akt);

            document1 = TestHelper.GetInsertedDokument(docCaption1);
            Assert.IsNull(document1);
            
            document2 = TestHelper.GetInsertedDokument(docCaption2);
            Assert.IsNull(document2);

            aktenDokumente1 = TestHelper.GetInsertedAktenDokumente(aktId, docId1);
            Assert.IsNull(aktenDokumente1);

            aktenDokumente2 = TestHelper.GetInsertedAktenDokumente(aktId, docId2);
            Assert.IsNull(aktenDokumente2);

            Assert.IsFalse(File.Exists(string.Format("{0}/{1}", _documentsDirectory, testFileName1)));
            Assert.IsFalse(File.Exists(string.Format("{0}/{1}", _documentsDirectory, testFileName2)));

            // clean up
            RecordSet.Delete(gegner);
            gegner = TestHelper.GetInsertedGegner(oldid);
            Assert.IsNull(gegner);

            RecordSet.Delete(klient);
            klient = TestHelper.GetInsertedKlient(oldid);
            Assert.IsNull(klient);
        }


        #endregion

        #region helper methods
        private void DeleteLeftoverRecords()
        {
            const string oldid = "test_akt_deletion";
            var gegner = TestHelper.GetInsertedGegner(oldid);
            if (gegner != null)
                RecordSet.Delete(gegner);

            var klient = TestHelper.GetInsertedKlient(oldid);
            if (klient != null)
                RecordSet.Delete(klient);

        }
        #endregion

    }
}
