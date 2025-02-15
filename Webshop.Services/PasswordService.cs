namespace Webshop.Services
{
    public class PasswordService
    {
        private readonly HttpClient _httpClient;
        private readonly HashingService _hashingService;
        private const string HIBP_API = "https://api.pwnedpasswords.com/range/";

        public PasswordService(HttpClient httpClient, HashingService hashingService)
        {
            _httpClient = httpClient;
            _hashingService = hashingService;
        }

        public async Task<bool> IsPasswordPwned(string password)
        {
            string sha1Hash = _hashingService.ComputeSha1Hash(password);
            string prefix = sha1Hash[..5];
            string suffix = sha1Hash[5..];

            var response = await _httpClient.GetStringAsync(HIBP_API + prefix);
            return response.Split("\n").Any(line => line.StartsWith(suffix));
        }

        public bool IsPasswordStrong(string password)
        {
            var score = Zxcvbn.Core.EvaluatePassword(password).Score; // Returns 0-4: very weak to very strong
            return score >= 2; // must be atleast fair
        }
    }
}
