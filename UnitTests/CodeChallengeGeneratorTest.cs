using OneStopShopIdaBackend.Services;
using System.Security.Cryptography;
using System.Text;

namespace UnitTests
{
    public class CodeChallengeGeneratorTest
    {
        private readonly CodeChallengeGeneratorService _codeChallengeGeneratorService;
        public CodeChallengeGeneratorTest() {
            _codeChallengeGeneratorService = new CodeChallengeGeneratorService();
        }
        [Fact]
        public void PassingTest()
        {
            // Arrange
            string codeVerifier = _codeChallengeGeneratorService.CodeVerifier;
            string codeChallenge = _codeChallengeGeneratorService.CodeChallenge;
            string hashedCodeVerifier;

            // Act
            // Create a new instance of SHA256
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash of the input string
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));

                // Convert the byte array to a URL Encoded string
                hashedCodeVerifier = Convert.ToBase64String(hashBytes).Replace("=", "").Replace("+", "-").Replace("/", "_");
            }

            // Assert
            Assert.Equal(codeChallenge, hashedCodeVerifier);
        }
    }
}