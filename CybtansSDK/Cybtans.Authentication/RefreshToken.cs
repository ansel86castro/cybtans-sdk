
using Cybtans.Entities;

namespace Cybtans.Authentication
{
    public class RefreshToken :IEntity<string>
    {
        public string Id { get; set; }

        public string UserId { get; init; }

        public string DeviceId { get; init; }

        public DateTime ExpireAt { get; init; }
    }


}
