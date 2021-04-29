using System;
using System.IO;

#nullable enable

namespace Cybtans.Services.FileStorage
{
    public class FileContent
    {
        public FileContent(string name, byte[] contentHash, long contentLength, string contentType, DateTimeOffset lastModified, Stream content)
        {
            Name = name;
            ContentHash = contentHash;
            ContentLength = contentLength;
            ContentType = contentType;
            LastModified = lastModified;
            Content = content;
        }

        public string Name { get; }

        public byte[]? ContentHash { get; }

        public long ContentLength { get; }

        public string ContentType { get; }

        public DateTimeOffset? LastModified { get; }

        public Stream Content { get; }

    }
}


