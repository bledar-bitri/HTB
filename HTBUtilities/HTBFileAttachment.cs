using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HTBUtilities
{
    public class HTBFileAttachment : HTBEmailAttachment
    {

        public string AttachmentDescription { get; set; }
        public HTBFileAttachment(HTBEmailAttachment attachment)
            : base(attachment.AttachmentStream, attachment.AttachmentStreamName, attachment.AttachmentStreamMime)
        {
            
        }
        public HTBFileAttachment(Stream stream, string name, string mime, string description = "") : base(stream, name, mime)
        {
            AttachmentDescription = description;
        }
    }
}
