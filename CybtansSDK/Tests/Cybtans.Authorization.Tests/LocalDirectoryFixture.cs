using Cybtans.Authentication;

namespace Cybtans.Authorization.Tests
{
    public class LocalDirectoryFixture: IAsyncLifetime
    {
        private LocalDirectoryRepository _repository;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1);        

        public LocalDirectoryFixture() : this("certs") { }
        protected LocalDirectoryFixture(string directory) 
        {
            _repository = new XmlLocalDirectoryRepository(directory);       
        }


        public LocalDirectoryRepository Repo => _repository;

        public Task DisposeAsync()
        {
            _repository.Dispose();
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            await _semaphore.WaitAsync();
            try
            {               
               await _repository.Initialize();              
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}