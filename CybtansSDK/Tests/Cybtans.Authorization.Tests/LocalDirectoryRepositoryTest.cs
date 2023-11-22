using Cybtans.Authentication;

namespace Cybtans.Authorization.Tests
{

    public class LocalDirectoryRepositoryTest:IClassFixture<LocalDirectoryFixture>
    {
        private LocalDirectoryRepository _repo;

        public LocalDirectoryRepositoryTest(LocalDirectoryFixture fixture) 
        {
            _repo = fixture.Repo;
        }

        [Fact]
        public async Task GetCertificates()
        {
            var certs = await _repo.GetCertificates();
            Assert.NotNull(certs);
            Assert.True(certs.Any());
        }

        [Fact] 
        public async Task GetCertificate()
        {
            var cert = await _repo.GetDefaultCertificate();
            Assert.NotNull(cert);
            Assert.NotNull(cert.Certificate);
            Assert.NotNull(cert.KeyId);
        }
    }
}