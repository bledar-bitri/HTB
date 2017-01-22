using HTBPdf;
using HTBUtilities;
using System.IO;

namespace HTBReports
{
    public abstract class BasicReport : IReport
    {
        protected int col;
        protected int col1;
        protected int col2;
        protected int col3;
        protected int col4;
        protected int col5;
        protected int col6;
        protected int col7;
        protected int MAX_LINES = 2550;
        protected int startLine;
        protected int lin;
        protected int gap;
        protected int normalGap;

        protected string logoPath = HTBUtils.GetConfigValue("LogoPath_Mahnung");
        protected ECPPdfWriter writer;
        protected int pageNumber = 1;

        protected void Init(Stream os)
        {
            startLine = 400;
            lin = startLine;
            normalGap = 37;
            gap = normalGap;
            col = 400;
            col1 = col + 33;

            col2 = 150;         // this is the actual start columng (col 1)
            col3 = col2 + 320;  // Auftragsnummer
            col4 = col3 + 600;  // Gegnername
            col5 = col4 + 450;  // Betreff
            col6 = col5 + 200;  // Kapital
            col7 = col6 + 200;  // Uebergabe

            writer = new ECPPdfWriter();
            writer.setFormName("A4");
            writer.open(os);
        }

        public void SetHeadingFont()
        {
            writer.setFont("Calibri", 12, true, false, true);
        }
        public void SetLineFont()
        {
            writer.setFont("Calibri", 10);
        }
        public int NewPageOnOverflow(int lin)
        {
            if (CheckOverflow(lin))
            {
                writer.newPage();
                return PrintPageHeader();
            }
            return lin;
        }

        #region Report Interface
        public bool CheckOverflow(int plin)
        {
            return plin >= MAX_LINES;
        }
        public ECPPdfWriter GetWriter()
        {
            return writer;
        }
        public abstract int PrintPageHeader();
        #endregion
    }
}
