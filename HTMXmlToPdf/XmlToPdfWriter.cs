using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using HTBPdf;
using iTextSharp.text;

namespace HTBXmlToPdf
{
    public class XmlToPdfWriter
    {
        List<object> objects = new List<object>();
        XmlDocFont defaultFont = new XmlDocFont("Tahoma") { Size = 10 };
        private const string fileName = @"C:\source\c#\XmlToPdf\XmlToPdf\Document.xml";

        private const string XML_NODE_LINE = "Line";
        private const string XML_NODE_TEXT = "Text";
        private const string XML_NODE_IMAGE = "Image";
        private const string XML_NODE_FONT = "Font";
        private const string XML_NODE_DEFAULT_FONT = "DefaultFont";

        private const string XML_ATTRIBUTE_NAME = "name";
        private const string XML_ATTRIBUTE_LINE_NUMBER = "lineNumber";
        private const string XML_ATTRIBUTE_SOURCE = "src";
        private const string XML_ATTRIBUTE_SIZE = "size";
        private const string XML_ATTRIBUTE_COLOR = "color";
        private const string XML_ATTRIBUTE_IS_BOLD = "isBold";
        private const string XML_ATTRIBUTE_IS_UNDERLINE = "isUnderline";
        private const string XML_ATTRIBUTE_IS_ITALICS = "isItalics";
        private const string XML_ATTRIBUTE_ALIGNMENT = "align";
        private const string XML_ATTRIBUTE_WIDTH = "width";

        private const string XML_ATTRIBUTE_X = "x";
        private const string XML_ATTRIBUTE_Y = "y";
        private const string XML_ATTRIBUTE_START_X = "startX";
        private const string XML_ATTRIBUTE_START_Y = "startY";
        private const string XML_ATTRIBUTE_END_X = "endX";
        private const string XML_ATTRIBUTE_END_Y = "endY";
        private const string XML_ATTRIBUTE_CHARS_PER_LINE = "charsPerLine";
        private const string XML_ATTRIBUTE_LINE_SPACING = "lineSpacing";
        private const string XML_ATTRIBUTE_RATIO = "ratio";

        ECPPdfWriter writer;

        private void LoadXml(string xmlText)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xmlText);

            foreach (XmlNode node in doc.DocumentElement)
            {
                switch (node.Name)
                {
                    case XML_NODE_IMAGE:
                        ParseXmlDocImage(node);
                        break;
                    case XML_NODE_TEXT:
                        ParseXmlDocText(node);
                        break;
                    case XML_NODE_DEFAULT_FONT:
                        defaultFont = GetParsedFontFromNode(node);
                        break;
                    case XML_NODE_LINE:
                        ParseXmlDocLine(node);
                        break;
                }
            }

            objects.OfType<XmlDocText>().ToList().ForEach(SetDefaultFontIfFontIsMissing);
        }

        private void ParseXmlDocLine(XmlNode node)
        {
            if (node == null) return;
            if (node.Attributes == null) return;

            var startX = TryToGetNodeAttributeInt(node, XML_ATTRIBUTE_START_X);
            var startY = TryToGetNodeAttributeInt(node, XML_ATTRIBUTE_START_Y);
            var endX = TryToGetNodeAttributeInt(node, XML_ATTRIBUTE_END_X);
            var endY = TryToGetNodeAttributeInt(node, XML_ATTRIBUTE_END_Y);
            var width = TryToGetNodeAttributeFloat(node, XML_ATTRIBUTE_WIDTH);


            objects.Add(new XmlDocLine
            {
                Start_X = startX,
                Start_Y = startY,
                End_X = endX,
                End_Y = endY,
                Width = width
            });
        }

        private void ParseXmlDocText(XmlNode node)
        {
            if (node == null) return;
            if (node.Attributes == null) return;

            var number = TryToGetNodeAttributeInt(node, XML_ATTRIBUTE_LINE_NUMBER);
            var x = TryToGetNodeAttributeInt(node, XML_ATTRIBUTE_X);
            var y = TryToGetNodeAttributeInt(node, XML_ATTRIBUTE_Y);
            var charsPerLine = TryToGetNodeAttributeInt(node, XML_ATTRIBUTE_CHARS_PER_LINE);
            var lineSpacing = TryToGetNodeAttributeInt(node, XML_ATTRIBUTE_LINE_SPACING);
            var color = TryToGetNodeAttributeString(node, XML_ATTRIBUTE_COLOR);
            var alignment = TryToGetNodeAttributeString(node, XML_ATTRIBUTE_ALIGNMENT);

            var text = node.InnerText.Trim();
            XmlDocFont font = null;

            foreach (XmlNode childNode in node.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case XML_NODE_FONT:
                        font = GetParsedFontFromNode(childNode);
                        break;
                }
            }


            objects.Add(new XmlDocText(number)
            {
                X = x,
                Y = y,
                CharsPerLine = charsPerLine,
                LineSpacing = lineSpacing,
                Text = text,
                Color = color,
                Alignment = alignment,
                Font = font
            });

        }

        private XmlDocFont GetParsedFontFromNode(XmlNode node)
        {
            if (node == null) return null;
            if (node.Attributes == null) return null;

            var name = TryToGetNodeAttributeString(node, XML_ATTRIBUTE_NAME);
            var size = TryToGetNodeAttributeInt(node, XML_ATTRIBUTE_SIZE);

            var font = new XmlDocFont(name)
            {
                Size = size,
            };

            try
            {
                font.IsBold = bool.Parse(node.Attributes[XML_ATTRIBUTE_IS_BOLD].Value);
            }
            catch
            {
            }

            try
            {
                font.IsUnderline = bool.Parse(node.Attributes[XML_ATTRIBUTE_IS_UNDERLINE].Value);
            }
            catch
            {
            }

            try
            {
                font.IsItalics = bool.Parse(node.Attributes[XML_ATTRIBUTE_IS_ITALICS].Value);
            }
            catch
            {
            }
            return font;
        }

        private void ParseXmlDocImage(XmlNode node)
        {
            if (node == null) return;
            if (node.Attributes == null) return;

            var name = node.Attributes[XML_ATTRIBUTE_NAME].Value;
            var src = node.Attributes[XML_ATTRIBUTE_SOURCE].Value;

            var x = int.Parse(node.Attributes[XML_ATTRIBUTE_X].Value);
            var y = int.Parse(node.Attributes[XML_ATTRIBUTE_Y].Value);
            int ratio;

            // here user getZeroIfError
            try
            {
                ratio = int.Parse(node.Attributes[XML_ATTRIBUTE_RATIO].Value);
            }
            catch
            {
                ratio = 100;
            }
            objects.Add(new XmlDocImage(src, name)
            {
                X = x,
                Y = y,
                Ratio = ratio,
            });
        }

        private string TryToGetNodeAttributeString(XmlNode node, string attributeName)
        {
            try
            {
                return node.Attributes[attributeName].Value;
            }
            catch
            {
                return null;
            }
        }
        private int TryToGetNodeAttributeInt(XmlNode node, string attributeName)
        {
            try
            {
                return int.Parse(node.Attributes[attributeName].Value);
            }
            catch
            {
                return 0;
            }
        }
        private float TryToGetNodeAttributeFloat(XmlNode node, string attributeName)
        {
            try
            {
                return float.Parse(node.Attributes[attributeName].Value);
            }
            catch
            {
                return 0;
            }
        }
        private bool TryToGetNodeAttributeBool(XmlNode node, string attributeName)
        {
            try
            {
                return bool.Parse(node.Attributes[attributeName].Value);
            }
            catch
            {
                return false;
            }
        }

        public void GeneratePdf(Stream stream, string xmlText)
        {
            LoadXml(xmlText);

            writer = new ECPPdfWriter();

            writer.setFormName("A4");
            writer.open(stream);

            objects.OfType<XmlDocImage>().ToList().ForEach(image => AddImage(writer, image));
            objects.OfType<XmlDocText>().ToList().ForEach(text => AddText(writer, text));
            objects.OfType<XmlDocLine>().ToList().ForEach(line => AddLines(writer, line));
            ClosePdf();
        }

        public void ClosePdf()
        {
            writer.Close();
        }
        private void AddLines(ECPPdfWriter writer, XmlDocLine line)
        {
           writer.drawLine(line.Start_Y, line.Start_X, line.End_Y, line.End_X, line.Width);
        }

        private void AddText(ECPPdfWriter writer, XmlDocText text)
        {
            writer.setFont(text.Font.Name, text.Font.Size, text.Font.IsBold, text.Font.IsUnderline, text.Font.IsItalics);
            BaseColor baseColor = BaseColor.BLACK;
            //char align = 'L';
            switch (text.Color)
            {
                case "gray":
                    baseColor = BaseColor.GRAY;
                    break;
            }
            if (text.CharsPerLine > 0)
                PrintTextInMultipleLines(writer, text.Y, text.X, text.LineSpacing, text.Text, text.CharsPerLine);
            else
                writer.print(text.Y, text.X, text.Text, text.Align, baseColor);
        }

        private void AddImage(ECPPdfWriter writer, XmlDocImage image)
        {
            if (File.Exists(image.Src))
            {
                var img = Image.GetInstance(image.Src);

                writer.drawBitmap(image.Y, image.X, img, image.Ratio);
            }
            else
            {
                throw new FileNotFoundException(string.Format("Image File Not Found: [{0}]", image.Src));
            }
        }

        private void SetDefaultFontIfFontIsMissing(XmlDocText text)
        {
            if (text.Font == null)
                text.Font = defaultFont;
            else
                text.Font.SetMissingProperties(defaultFont);
        }


        #region existing methods

        public static int PrintTextInMultipleLines(ECPPdfWriter writer, int lin, int col, int linGap, string text, int maxChars)
        {
            string[] lines = text.Split('\n');
            foreach (string line in lines)
            {
                IEnumerable<string> lins = SplitStringInPdfLines(line, maxChars);
                foreach (string l in lins)
                {
                    writer.print(lin, col, l);
                    lin += linGap;
                }
            }
            return lin;
        }

        public static IEnumerable<string> SplitStringInPdfLines(string lines, int maxLineLength)
        {
            var list = new ArrayList();
            if (lines.Length <= maxLineLength)
            {
                list.Add(lines);
            }
            else
            {
                var sb = new StringBuilder();
                string[] words = lines.Replace(" ", " `").Split();
                foreach (string word in words)
                {
                    if ((sb.Length + word.Length) > maxLineLength)
                    {
                        list.Add(sb.ToString().Trim());
                        sb.Clear();
                    }
                    sb.Append(word.Replace("`", " "));
                }
                list.Add(sb.ToString().Trim());
            }
            return list.Cast<string>().ToArray();
        }

        #endregion


    }
}
