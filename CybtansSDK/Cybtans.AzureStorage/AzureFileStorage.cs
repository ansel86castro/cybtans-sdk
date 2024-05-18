using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Options;
using Microsoft.IO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Services.FileStorage
{

    public class AzureStorageConfig
    {
        public string? ConnectionString { get; set; }

        public string? DefaultContainer { get; set; }

        public PublicAccessType DefaultAccessType { get; set; } = PublicAccessType.None;
    }


    public class AzureFileStorage : IFileStorage
    {
        private readonly RecyclableMemoryStreamManager _streamManager = new RecyclableMemoryStreamManager();

        private readonly AzureStorageConfig _options;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ConcurrentDictionary<string, BlobContainerClient> _blobClients = new ConcurrentDictionary<string, BlobContainerClient>();

        public AzureFileStorage(IOptions<AzureStorageConfig> options)
        {
            _options = options.Value;
            _blobServiceClient = new BlobServiceClient(_options.ConnectionString);
        }

        public async Task<CreateFileResult> CreateFile(string filename, Stream content, string? container = null, string contentType = "application/octet-stream", bool createContainer = true, FileOptions? options = null)
        {
            try
            {
                (BlobClient blobClient, BlobUploadOptions uploadOptions) = await GetBlobClient(filename, container, contentType, createContainer, options);

                var result = await blobClient.UploadAsync(content, uploadOptions);                
                return new CreateFileResult(blobClient.Uri)
                {
                    ContentHash = result.Value.ContentHash,
                    VersionId = result.Value.VersionId,
                };
            }
            catch (Azure.RequestFailedException e)
            {
                throw ConvertContainerRequestFailedException(e, container);
            }
        }
        public async Task<CreateFileResult> CreateFile(string filename, Memory<byte> content, string? container = null, string contentType = "application/octet-stream", bool createContainer = true, FileOptions? options = null)
        {
            try
            {
                (BlobClient blobClient, BlobUploadOptions uploadOptions) = await GetBlobClient(filename, container, contentType, createContainer, options);

                using (var stream = _streamManager.GetStream(content.Span))
                {
                    var result = await blobClient.UploadAsync(stream, uploadOptions);

                    return new CreateFileResult(blobClient.Uri)
                    {
                        ContentHash = result.Value.ContentHash,
                        VersionId = result.Value.VersionId,
                    };
                }
            }
            catch (Azure.RequestFailedException e)
            {
                throw ConvertContainerRequestFailedException(e, container);
            }
        }

        public async Task<bool> DeleteContainer(string container)
        {
            var containerClient = await GetContainerClient(container, false);
            var result = await containerClient.DeleteIfExistsAsync();
            return result.Value;
        }


        public async Task<Uri> AppendFile(string filename, IAsyncEnumerable<Stream> contents, string? container = null, string contentType = "application/octet-stream", bool createContainer = true, FileOptions? options = null)
        {
            try
            {
                AppendBlobClient blobClient = await GetAppendBlobClient(filename, container, contentType, createContainer, options);

                await foreach (var stream in contents)
                {
                    await blobClient.AppendBlockAsync(stream);
                }

                return blobClient.Uri;
            }
            catch (Azure.RequestFailedException e)
            {
                throw ConvertContainerRequestFailedException(e, container);
            }
        }


        public async Task<Uri> AppendFile(string filename, Stream content, string? container = null, string contentType = "application/octet-stream", bool createContainer = true, FileOptions? options = null)
        {
            try
            {
                AppendBlobClient blobClient = await GetAppendBlobClient(filename, container, contentType, createContainer, options);

                await blobClient.AppendBlockAsync(content);

                return blobClient.Uri;
            }
            catch (Azure.RequestFailedException e)
            {
                throw ConvertContainerRequestFailedException(e, container);
            }
        }

        public async Task<IFileAppendHandler> GetAppendHandler(string filename, string? container = null, string contentType = "application/octet-stream", bool createContainer = true, FileOptions? options = null)
        {
            try
            {
                AppendBlobClient blobClient = await GetAppendBlobClient(filename, container, contentType, createContainer, options);
                return new AppendBlobHandler(blobClient, this);
            }
            catch (Azure.RequestFailedException e)
            {
                throw ConvertContainerRequestFailedException(e, container);
            }
        }

        public async Task<bool> DeleteFile(string filename, string? container = null)
        {
            try
            {
                var containerClient = await GetContainerClient(container, false);
                var blobClient = containerClient.GetBlobClient(filename);
                var response = await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
                return response.Value;
            }
            catch (Azure.RequestFailedException e)
            {
                throw ConvertFileRequestFailedException(e, filename, container);
            }
        }

        public async Task<bool> FileExist(string filename, string? container = null)
        {
            try
            {
                var containerClient = await GetContainerClient(container, false);
                var blobClient = containerClient.GetBlobClient(filename);
                return await blobClient.ExistsAsync();
            }
            catch (Azure.RequestFailedException e)
            {
                throw ConvertContainerRequestFailedException(e, container);
            }
        }

        public async Task<FileContent> GetFileContent(string filename, string? container = null)
        {
            try
            {
                var containerClient = await GetContainerClient(container, false);
                var blobClient = containerClient.GetBlobClient(filename);
                var content = (await blobClient.DownloadAsync()).Value;
              
                return new FileContent(filename, content.ContentHash, content.ContentLength, content.ContentType, content.Details.LastModified, content.Content);
            }
            catch (Azure.RequestFailedException e)
            {
                throw ConvertFileRequestFailedException(e, filename, container);
            }
        }

        public async Task<FileEntry> GetFileEntry(string filename, string? container = null)
        {
            try
            {
                var containerClient = await GetContainerClient(container, false);
                var blobClient = containerClient.GetBlobClient(filename);
                var props = (await blobClient.GetPropertiesAsync()).Value;

                return new FileEntry
                {
                    ContentLength = props.ContentLength,
                    CanAppend = props.BlobType == BlobType.Append,
                    ContentType = props.ContentType,
                    Name = filename,
                    Uri = blobClient.Uri,
                    VersionId = props.VersionId,
                    LastModified = props.LastModified
                };
            }
            catch (Azure.RequestFailedException e)
            {
                throw ConvertFileRequestFailedException(e, filename, container);

            }
        }

        public async IAsyncEnumerable<FileEntry> GetFiles(string? prefix = null, string? container = null, bool deleted = false)
        {
            BlobContainerClient? containerClient;

            containerClient = await GetContainerClient(container, false);

            var asynStream = containerClient.GetBlobsAsync(BlobTraits.All, deleted ? BlobStates.Deleted : BlobStates.None, prefix);
            var enumerator = asynStream.GetAsyncEnumerator();

            try
            {
                while (true)
                {
                    BlobItem? item;
                    try
                    {
                        if (!await enumerator.MoveNextAsync()) break;
                        item = enumerator.Current;
                    }
                    catch (Azure.RequestFailedException e)
                    {
                        throw ConvertContainerRequestFailedException(e, container);
                    }

                    yield return new FileEntry
                    {
                        ContentLength = item.Properties.ContentLength,
                        ContentType = item.Properties.ContentType,
                        Deleted = item.Deleted,
                        Name = item.Name,
                        VersionId = item.VersionId,
                        CanAppend = item.Properties.BlobType == BlobType.Append,
                        Uri = new Uri(containerClient.Uri, item.Name),
                        LastModified = item.Properties.LastModified
                    };
                }
            }
            finally
            {
                await enumerator.DisposeAsync();
            }
        }

        private async ValueTask<BlobContainerClient> GetContainerClient(string? container, bool createContainer, FileOptions? options = null)
        {
            if (container == null && _options.DefaultContainer == null)
                throw new InvalidOperationException("container name is missing");

            var containerClient = _blobClients.GetOrAdd(container ?? _options.DefaultContainer!, name =>
            {
                return _blobServiceClient.GetBlobContainerClient(name);
            });

            if (createContainer)
            {
                await containerClient.CreateIfNotExistsAsync(options?.Visibility != null ? GetPublicAccessType(options.Visibility.Value) : _options.DefaultAccessType);
            }

            return containerClient;
        }

        private AccessTier GetAccessTier(FileAccessFrecuency type) => type switch
        {
            FileAccessFrecuency.Frecuently => AccessTier.Hot,
            FileAccessFrecuency.Infrecuently => AccessTier.Cool,
            _ => throw new InvalidOperationException("FileAccessType not supported")
        };

        private PublicAccessType GetPublicAccessType(FileContainerType type) => type switch
        {
            FileContainerType.Private => PublicAccessType.None,
            FileContainerType.Public => PublicAccessType.Blob,
            _ => PublicAccessType.None
        };

        private async ValueTask<(BlobClient, BlobUploadOptions)> GetBlobClient(string filename, string? container, string contentType, bool createContainer, FileOptions? options)
        {
            var containerClient = await GetContainerClient(container, createContainer, options);
            var blobClient = containerClient.GetBlobClient(filename);
            var uploadOptions = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = contentType },
            };
            if (options != null)
            {
                if (options.AccessType != null)
                    uploadOptions.AccessTier = GetAccessTier(options.AccessType.Value);
                if (options.CacheTTLInSeconds != null)
                    uploadOptions.HttpHeaders.CacheControl = $"max-age={options.CacheTTLInSeconds}";
            }

            return (blobClient, uploadOptions);
        }

        private async Task<AppendBlobClient> GetAppendBlobClient(string filename, string? container, string contentType, bool createContainer, FileOptions? options)
        {
            var containerClient = await GetContainerClient(container, createContainer, options);
            var blobClient = containerClient.GetAppendBlobClient(filename);

            if (!await blobClient.ExistsAsync())
            {
                var uploadOptions = new AppendBlobCreateOptions
                {
                    HttpHeaders = new BlobHttpHeaders { ContentType = contentType },
                };

                if (options?.CacheTTLInSeconds != null)
                {
                    uploadOptions.HttpHeaders.CacheControl = $"max-age={options.CacheTTLInSeconds}";
                }


                await blobClient.CreateAsync(uploadOptions);
            }

            return blobClient;
        }

        private Exception ConvertContainerRequestFailedException(Azure.RequestFailedException e, string? container)
        {
            if (e.Status == 404) return new FileNotFoundException($"The container {container ?? _options.DefaultContainer} can not be found");
            else if (e.Status == 401 || e.Status == 403) return new UnauthorizedAccessException(e.Message, e);
            return new IOException(e.Message, e);
        }

        private Exception ConvertFileRequestFailedException(Azure.RequestFailedException e, string filename, string? container)
        {
            if (e.Status == 404) return new FileNotFoundException($"The file {filename} can not be found in {container ?? _options.DefaultContainer}");
            else if (e.Status == 401 || e.Status == 403) return new UnauthorizedAccessException(e.Message, e);
            return new IOException(e.Message, e);
        }

        public sealed class AppendBlobHandler : IFileAppendHandler
        {
            private readonly AppendBlobClient _blobClient;
            private readonly AzureFileStorage _azureFileStorage;

            public AppendBlobHandler(AppendBlobClient blobClient, AzureFileStorage azureFileStorage)
            {
                _blobClient = blobClient;
                _azureFileStorage = azureFileStorage;
            }

            public Uri Uri => _blobClient.Uri;

            void IDisposable.Dispose()
            {
                //This implementation does not need to dispose resources
            }

            public async Task<bool> SealFile()
            {
                try
                {
                    var result = await _blobClient.SealAsync();
                    return result != null;
                }
                catch (Azure.RequestFailedException e)
                {
                    if (e.Status == 404) throw new FileNotFoundException($"The file {_blobClient.Name} can not be found");
                    else if (e.Status == 401) throw new UnauthorizedAccessException(e.Message, e);
                    throw new IOException(e.Message, e);
                }
            }

            public async Task Write(Stream stream)
            {
                try
                {
                    await _blobClient.AppendBlockAsync(stream);
                }
                catch (Azure.RequestFailedException e)
                {
                    if (e.Status == 404) throw new FileNotFoundException($"The file  {_blobClient.Name} can not be found");
                    else if (e.Status == 401) throw new UnauthorizedAccessException(e.Message, e);
                    throw new IOException(e.Message, e);
                }
            }

            public async Task Write(byte[] bytes, int offset, int length)
            {
                using (var stream = _azureFileStorage._streamManager.GetStream(new ReadOnlySpan<byte>(bytes, offset, length)))
                {
                    await _blobClient.AppendBlockAsync(stream);
                }
            }

            public async Task Write(Memory<byte> memory)
            {
                using (var stream = _azureFileStorage._streamManager.GetStream(memory.Span))
                {
                    await _blobClient.AppendBlockAsync(stream);
                }
            }
        }
    }
}
