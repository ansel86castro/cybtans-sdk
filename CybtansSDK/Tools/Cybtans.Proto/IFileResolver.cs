#nullable enable

using System.IO;
using System.Linq;

namespace Cybtans.Proto
{
    public interface IFileResolver
    {
        FileInfo GetFile(string path);
    }

    public class SearchPathFileResolver : IFileResolver
    {
        DirectoryInfo[] _searchPath;
        DirectoryInfo _baseDir;
        public SearchPathFileResolver(DirectoryInfo baseDir, DirectoryInfo[] searchPath)
        {
            this._searchPath = searchPath;
            this._baseDir = baseDir;
        }

        public FileInfo GetFile(string path)
        {
            if (path.StartsWith(".") || path.StartsWith(".."))
            {
                var fullpath = Path.Combine(_baseDir.FullName, path);
                if (!File.Exists(fullpath))
                    throw new FileNotFoundException($"File {path} not found in {_baseDir.FullName}");

                return new FileInfo(fullpath);
            }

            foreach (var dir in _searchPath)
            {
                var file = FindFile(dir, path);
                if (file != null)
                    return file;
            }

            throw new FileNotFoundException($"File {path} not found in { string.Join(", ", _searchPath.Select(x => x.FullName))}");
        }

        private FileInfo? FindFile(DirectoryInfo dir, string path)
        {
            var fullpath = Path.Combine(dir.FullName, path);
            if (!File.Exists(fullpath))
                return null;

            return new FileInfo(fullpath);
        }
    }
}
