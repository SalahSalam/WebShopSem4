namespace Webshop.Services
{
    // TODO: If making PasswordService move these methods to that class.
    public class PwnedPasswordService
    {
        private readonly HttpClient _httpClient;
        private readonly HashingService _hashingService;
        private const string HIBP_API = "https://api.pwnedpasswords.com/range/";

        public PwnedPasswordService(HttpClient httpClient, HashingService hashingService)
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
    }
}
