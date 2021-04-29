using System;

namespace Cybtans.Services.FileStorage
{
    public class FileEntry
    {
        public Uri Uri { get; set; }

        public string Name { get; set; }

        public bool Deleted { get; set; }

        public string VersionId { get; set; }

        public long? ContentLength { get; set; }

        public string ContentType { get; set; }

        public DateTimeOffset? LastModified { get; set; }

        public bool CanAppend { get; set; }
    }
}


