using System.IO;

namespace HTBUtilities
{
    public class HTBEmailAttachment
    {
        public Stream AttachmentStream { get; set; }
        public string AttachmentStreamNamePrefix { get; set; }
        public string AttachmentStreamName { get; set; }
        public string AttachmentStreamMime { get; set; }
        
        public HTBEmailAttachment(Stream stream, string name, string mime)
        {
            AttachmentStream = stream;
            AttachmentStreamNamePrefix = "";
            AttachmentStreamName = name;
            AttachmentStreamMime = mime;
        }
    }
}
