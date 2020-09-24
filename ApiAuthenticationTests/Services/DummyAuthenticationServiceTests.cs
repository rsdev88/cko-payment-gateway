using APIAuthentication.Entities;
using APIAuthentication.Services;
using NUnit.Framework;
using System.Threading.Tasks;

namespace ApiAuthenticationTests.Services
{
    [TestFixture]
    public class DummyAuthenticationServiceTests
    {
        [Test]
        public async Task AuthenticateReturnsUserIfCredentialsAreCorrect()
        {
            //Arrange
            var service = new DummyAuthenticationService();
            var expectedUser = new User()
            {
                Username = "testuser",
                Password = "password1"
            };

            //Act
            var result = await service.Authenticate("testuser", "password1");

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUser.Username, result.Username);
            Assert.AreEqual(expectedUser.Password, result.Password);
        }

        [TestCase("", "")]
        [TestCase("", "password1")]
        [TestCase("testuser", "")]
        [TestCase("incorrectUsername", "password1")]
        [TestCase("testuser", "incorrectPassword")]
        public async Task AuthenticateReturnsNullIfCredentialsAreMissingOrIncorrect(string username, string password)
        {
            //Arrange
            var service = new DummyAuthenticationService();

            //Act
            var result = await service.Authenticate(username, password);

            //Assert
            Assert.IsNull(result);
        }
    }
}
