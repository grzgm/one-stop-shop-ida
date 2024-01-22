using System.Security.Cryptography;
using System.Text;

namespace OneStopShopIdaBackend.Services
{
    public class CodeChallengeGeneratorService
    {
        public string CodeVerifier { get; }
        public string CodeChallenge { get; }

        public CodeChallengeGeneratorService()
        {
            CodeVerifier = GenerateCodeVerifier();
            CodeChallenge = CreateCodeChallenge(CodeVerifier);
        }

        private string GenerateCodeVerifier()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[32]; // 32 bytes for 256 bits
                rng.GetBytes(randomBytes);
                return BitConverter.ToString(randomBytes).Replace("-", "").ToLower();
            }
        }

        private string CreateCodeChallenge(string codeVerifier)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
                return Base64UrlEncode(challengeBytes);
            }
        }

        private string Base64UrlEncode(byte[] bytes)
        {
            string base64 = Convert.ToBase64String(bytes);
            base64 = base64.Replace("=", "").Replace("+", "-").Replace("/", "_");
            return base64;
        }
    }
}
