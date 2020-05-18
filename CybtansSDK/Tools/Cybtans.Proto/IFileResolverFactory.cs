#nullable enable

using System.Linq;
using System.IO;
using System;

namespace Cybtans.Proto
{
    public interface IFileResolverFactory
    {
        IFileResolver GetResolver(string baseDirectory);

    }

    public class SearchPathFileResolverFactory : IFileResolverFactory
    {
        DirectoryInfo[] directorys;
        public SearchPathFileResolverFactory(string[] searchPath)
        {
            directorys = searchPath.Select(x => new DirectoryInfo(x)).ToArray();
        }

        public IFileResolver GetResolver(string baseDirectory)
        {
            if (string.IsNullOrEmpty(baseDirectory))
            {
                baseDirectory = Environment.CurrentDirectory;
            }
            return new SearchPathFileResolver(new DirectoryInfo(baseDirectory), directorys);
        }
    }


}
