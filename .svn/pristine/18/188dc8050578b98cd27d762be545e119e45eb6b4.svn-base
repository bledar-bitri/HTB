
using System.IO;
using HTB.Database;
using HTBUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HTBUtilitiesTests
{
    [TestClass]
    public class InkassoUnitTests
    {

        private tblControl _control;
        private string _documentsDirectory;

        #region test initialization
        [TestInitialize]
        public void InitializeTest()
        {
            _control = HTBUtils.GetControlRecord();
            _documentsDirectory = HTBUtils.GetConfigValue("DocumentsFolder");
        }
        #endregion


        #region test methods
        [TestMethod]
        public void InkassoAktCreationTest()
        {
            const string aktAz = "test_inkasso_akt";
            const string oldid = aktAz;

            RecordSet.Insert(TestHelper.GetGegner(oldid));
            RecordSet.Insert(TestHelper.GetKlient(oldid));
            var gegner = TestHelper.GetInsertedGegner(oldid);
            var klient = TestHelper.GetInsertedKlient(oldid);

            Assert.IsNotNull(gegner);
            Assert.IsNotNull(klient);

            RecordSet.Insert(TestHelper.GetCustInkAkt(aktAz, gegner.GegnerID, klient.KlientID));
            var akt = TestHelper.GetInsertedCustInkAkt(aktAz);
            Assert.IsNotNull(akt);

            RecordSet.Delete(akt);
            akt = TestHelper.GetInsertedCustInkAkt(aktAz);
            Assert.IsNull(akt);

            RecordSet.Delete(gegner);
            gegner = TestHelper.GetInsertedGegner(oldid);
            Assert.IsNull(gegner);

            RecordSet.Delete(klient);
            klient = TestHelper.GetInsertedKlient(oldid);
            Assert.IsNull(klient);
        }

        [TestMethod]
        public void GegnerCreationTest()
        {
            const string oldid = "testgegnercreation";
            RecordSet.Insert(TestHelper.GetGegner(oldid));

            var gegner = TestHelper.GetInsertedGegner(oldid);
            Assert.IsNotNull(gegner);

            RecordSet.Delete(gegner);
            gegner = TestHelper.GetInsertedGegner(oldid);
            Assert.IsNull(gegner);
        }

        [TestMethod]
        public void KlientCreationTest()
        {
            const string oldid = "testklientcreation";
            RecordSet.Insert(TestHelper.GetKlient(oldid));

            var klient = TestHelper.GetInsertedKlient(oldid);
            Assert.IsNotNull(klient);

            RecordSet.Delete(klient); klient = TestHelper.GetInsertedKlient(oldid);
            Assert.IsNull(klient);
        }

        [TestMethod]
        public void InkassoDocumentCreationTest()
        {
            
            const string aktAz = "test_document_creation";
            const string testFileName = "_tmpUnitTestFile.txt";
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

            TestHelper.DeleteTestDocumentFile(_documentsDirectory, testFileName);

            RecordSet.Delete(aktenDokumente);
            RecordSet.Delete(document);
            RecordSet.Delete(akt);
            RecordSet.Delete(gegner);
            RecordSet.Delete(klient);


            akt = TestHelper.GetInsertedCustInkAkt(aktAz);
            Assert.IsNull(akt);

            document = TestHelper.GetInsertedDokument(docCaption);
            Assert.IsNull(document);

            aktenDokumente = TestHelper.GetInsertedAktenDokumente(aktId, docId);
            Assert.IsNull(aktenDokumente);

            Assert.IsFalse(File.Exists(string.Format("{0}/{1}", _documentsDirectory, testFileName)));

            gegner = TestHelper.GetInsertedGegner(oldid);
            Assert.IsNull(gegner);

            klient = TestHelper.GetInsertedKlient(oldid);
            Assert.IsNull(klient);
            
        }

       
        #endregion
    }
}
