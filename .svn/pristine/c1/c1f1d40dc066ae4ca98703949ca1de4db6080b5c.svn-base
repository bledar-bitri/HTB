namespace HTBXmlToPdf
{
    public class XmlDocText
    {
        private char align = 'L';

        public int LineNumber { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int CharsPerLine { get; set; }
        public int LineSpacing { get; set; }

        public string Color { get; set; }

        public string Alignment
        {
            get {
                switch (align)
                {
                    case 'R':
                        return "right";
                    case 'C':
                        return "center";
                    default:
                        return "left";
                }
            }
            set
            {
                if (value == null)
                {
                    align = 'L';
                }
                else
                {
                    switch (value.ToLower())
                    {
                        case "right":
                            align = 'R';
                            break;
                        case "center":
                            align = 'C';
                            break;
                        default:
                            align = 'L';
                            break;
                    }
                }
            }
        }

        public char Align
        {
            get { return align; }
        }

        public string Text { get; set; }
        public XmlDocFont Font { get; set; }
        public XmlDocText(int lineNumber)
        {
            this.LineNumber = lineNumber;
        }

        public override string ToString()
        {
            return string.Format("Text: [Line Number {0}]", LineNumber);
        }
    }
}
