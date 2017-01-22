using System.IO;

namespace HTBUtilities
{
    public class HTBZipEntry
    {
        public readonly Stream Stream;
        public readonly string Name;

        public HTBZipEntry(Stream stream, string name)
        {
            Stream = stream;
            Name = name;
        }
    }
}