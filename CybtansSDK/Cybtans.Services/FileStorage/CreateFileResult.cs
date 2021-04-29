using System;

#nullable enable

namespace Cybtans.Services.FileStorage
{
    public class CreateFileResult
    {
        public CreateFileResult(Uri uri)
        {
            Uri = uri;
        }

        public byte[]? ContentHash { get; set; }

        public string? VersionId { get; set; }
        public Uri Uri { get; }
    }
}
