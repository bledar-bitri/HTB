namespace HTBXmlToPdf
{
    public class XmlDocFont
    {

        private bool isBoldSet = false;
        private bool isUnderlineSet = false;
        private bool isItalicsSet = false;

        private bool isBold = false;
        private bool isUnderline = false;
        private bool isItalics = false;

        public string Name { get; private set; }
        public int Size { get; set; }

        public bool IsBold
        {
            get { return isBold; }
            set
            {
                isBold = value; 
                isBoldSet = true;
            } 
        }

        public bool IsUnderline
        {
            get { return isUnderline; }
            set
            {
                isUnderline = value;
                isUnderlineSet = true;
            }
        }

        public bool IsItalics
        {
            get
            {
                return isItalics;
            }
            set
            {
                isItalics = value;
                isItalicsSet = true;
            }
        }

        public XmlDocFont(string name)
        {
            this.Name = name;
        }

        public void SetMissingProperties(XmlDocFont font)
        {
            if (Size == 0)
                Size = font.Size;
            if (!isBoldSet)
                IsBold = font.IsBold;
            if (!isUnderlineSet)
                IsUnderline = font.isUnderline;
            if (!isItalicsSet)
                IsItalics = font.isItalics;

        }
        public override string ToString()
        {
            return string.Format("Font: {0} [size: {1}] [isBold: {2}]", Name, Size, isBold);
        }
    }
}
