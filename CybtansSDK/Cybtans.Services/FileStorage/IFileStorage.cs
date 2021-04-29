using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Services.FileStorage
{
    public class FileOptions
    {
        /// <summary>
        /// Hint to indicate how the file will be used by users
        /// </summary>
        public FileAccessFrecuency? AccessType { get; set; }

        /// <summary>
        /// If set indicates how much time the file will be cached by CDN
        /// </summary>
        public int? CacheTTLInSeconds { get; set; }

        /// <summary>
        /// If create Container is true ,then this indicates if the file can be accessed anonymously or authentication is required
        /// </summary>
        public FileContainerType? Visibility { get; set; }
             
    }

    public enum FileAccessFrecuency
    {
        Frecuently,
        Infrecuently
    }

    public enum FileContainerType
    {
        Public,
        Private
    }

    public interface IFileAppendHandler : IDisposable
    {
        Uri Uri { get; }

        Task Write(Stream stream);

        Task Write(byte[] bytes, int offset, int length);

        Task Write(Memory<byte> memory);

        /// <summary>
        /// Makes the file readonly
        /// </summary>
        /// <returns></returns>
        Task<bool> SealFile();
    }

    public interface IFileStorage
    {
        public Task<FileContent> GetFileContent(string filename, string? container = null);

        public Task<bool> DeleteFile(string filename, string? container = null);

        public Task<bool> FileExist(string filename, string? container = null);

        public Task<CreateFileResult> CreateFile(string filename, Stream content, string? container = null, string contentType = "application/octet-stream", bool createContainer = true, FileOptions? options = null);

        public Task<CreateFileResult> CreateFile(string filename, Memory<byte> content, string? container = null, string contentType = "application/octet-stream", bool createContainer = true, FileOptions? options = null);

        Task<Uri> AppendFile(string filename, IAsyncEnumerable<Stream> contents, string? container = null, string contentType = "application/octet-stream", bool createContainer = true, FileOptions? options = null);

        Task<Uri> AppendFile(string filename, Stream content, string? container = null, string contentType = "application/octet-stream", bool createContainer = true, FileOptions? options = null);

        Task<IFileAppendHandler> GetAppendHandler(string filename, string? container = null, string contentType = "application/octet-stream", bool createContainer = true, FileOptions? options = null);

        Task<FileEntry> GetFileEntry(string filename, string? container = null);

        public IAsyncEnumerable<FileEntry> GetFiles(string? prefix = null, string? container = null, bool deleted = false);

        Task<bool> DeleteContainer(string container);
    }
}


