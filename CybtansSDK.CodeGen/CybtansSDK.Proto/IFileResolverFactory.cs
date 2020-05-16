#nullable enable

using System.Linq;
using System.IO;

namespace CybtansSdk.Proto
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
            return new SearchPathFileResolver(new DirectoryInfo(baseDirectory), directorys);
        }
    }


}
