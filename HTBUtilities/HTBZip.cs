
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using Tamir.SharpSsh.java.io;

namespace HTBUtilities
{
    public static class HTBZip
    {
        public static void CreateZipFile (string fileName, IEnumerable<HTBZipEntry> entries)
        {
            CreateZipFile(new FileOutputStream (fileName), entries);
        }
        public static void CreateZipFile(Stream stream, IEnumerable<HTBZipEntry> entries )
        {
            var zip = new ZipFile();
            foreach (var entry in entries)
            {
                zip.AddEntry(entry.Name, entry.Stream);
            }
            zip.Save(stream);
        }
    }
}
