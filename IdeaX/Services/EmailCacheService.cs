using Microsoft.Extensions.Caching.Memory;

namespace IdeaX.Services
{
    public class EmailCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _expiry = TimeSpan.FromMinutes(5);
        public EmailCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public void saveEmail (string otp, string email)
        {
            _cache.Set(otp, email, _expiry);
        }
        public string getEmail(string otp)
        {
            _cache.TryGetValue(otp, out string _email);
            return _email;
        }
    }

}
