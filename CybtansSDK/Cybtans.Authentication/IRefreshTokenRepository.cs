using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Authentication
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> AddAsync(RefreshToken refreshToken);
        Task<RefreshToken> GetAsync(string tid);
        Task DeleteAsync(string id);
        Task<long> DeleteAsync(string userId, string deviceId);
    }
}
