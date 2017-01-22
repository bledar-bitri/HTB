﻿namespace HTBXmlToPdf
{
    public class XmlDocLine
    {
        //private char align = 'L';

        public int LineNumber { get; private set; }
        public int Start_X { get; set; }
        public int Start_Y { get; set; }
        public int End_X { get; set; }
        public int End_Y { get; set; }
        public float Width { get; set; }
        public override string ToString()
        {
            return string.Format("Line: {0}", LineNumber);
        }
    }
}
