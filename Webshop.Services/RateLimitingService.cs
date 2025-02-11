using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;

namespace Webshop.Services
{
    public class RateLimitingService
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, (int Attempts, DateTime LastAttempt)>> _attempts = new();
        private readonly Dictionary<string, (int MaxAttempts, TimeSpan LockoutDuration)> _rateLimitConfigs;

        public RateLimitingService()
        {
            _rateLimitConfigs = new Dictionary<string, (int MaxAttempts, TimeSpan LockoutDuration)>
            {
                { "Login", (3, TimeSpan.FromMinutes(10)) },
                { "PasswordReset", (3, TimeSpan.FromMinutes(60)) }
            };
        }

        public static string GenerateRateLimitKey(HttpContext httpContext, string? deviceFingerprint)
        {
            string ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string fingerprint = deviceFingerprint ?? "unknown";
            return $"{ipAddress}:{fingerprint}";
        }

        public bool IsRateLimited(string key, string action)
        {
            if (!_rateLimitConfigs.TryGetValue(action, out var config))
            {
                throw new ArgumentException($"Rate limiting configuration for action '{action}' not found.");
            }

            if (_attempts.TryGetValue(action, out var actionAttempts) &&
                    actionAttempts.TryGetValue(key, out var attemptInfo))
            {
                if (attemptInfo.Attempts >= config.MaxAttempts && DateTime.UtcNow - attemptInfo.LastAttempt < config.LockoutDuration)
                {
                    return true;
                }
            }

            return false;
        }

        public void RegisterAttempt(string key, string action)
        {
            var actionAttempts = _attempts.GetOrAdd(action, _ => new ConcurrentDictionary<string, (int Attempts, DateTime LastAttempt)>());
            actionAttempts.AddOrUpdate(
                key,
                addValueFactory: _ => (1, DateTime.UtcNow),
                updateValueFactory: (_, attemptInfo) => (attemptInfo.Attempts + 1, DateTime.UtcNow));
        }

        public void ResetAttempts(string key, string action)
        {
            if (_attempts.TryGetValue(action, out var actionAttempts))
            {
                actionAttempts.TryRemove(key, out _);
            }
        }
    }
}
