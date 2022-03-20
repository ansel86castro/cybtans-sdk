using System.Threading.Tasks;

namespace Cybtans.Automation
{
#nullable enable

    public interface IPage : ITestComponent
    {
        string? BaseUrl { get; set; }

        string? RelativeUrl { get; }

        void Load();

        public Task LoadAsync()
        {
            return Task.Run(() => Load());
        }
    }

#nullable restore
}
