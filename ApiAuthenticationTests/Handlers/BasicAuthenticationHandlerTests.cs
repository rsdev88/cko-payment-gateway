using APIAuthentication.Entities;
using APIAuthentication.Handlers;
using APIAuthentication.Services;
using ApiSharedLibrary.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using NUnit.Framework;
using PaymentGatewayApi.Exceptions;
using System;
using System.Net;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ApiAuthenticationTests.Handlers
{
    [TestFixture]
    public class BasicAuthenticationHandlerTests
    {
        private Mock<IOptionsMonitor<AuthenticationSchemeOptions>> _options;
        private Mock<ILoggerFactory> _logger;
        private Mock<UrlEncoder> _encoder;
        private Mock<ISystemClock> _clock;
        private Mock<IApiAuthenicationService> _authenticationService;
        private BasicAuthenticationHandler _authenticationHandler;

        [SetUp]
        public void BasicAuthenticationHandlerTests_Setup()
        {
            this._options = new Mock<IOptionsMonitor<AuthenticationSchemeOptions>>();
            this._logger = new Mock<ILoggerFactory>();
            this._encoder = new Mock<UrlEncoder>();
            this._clock = new Mock<ISystemClock>();
            this._authenticationService = new Mock<IApiAuthenicationService>();

            var logger = new Mock<ILogger<BasicAuthenticationHandler>>();
            this._logger.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

            this._authenticationHandler = new BasicAuthenticationHandler(this._options.Object,
                                                                         this._logger.Object,
                                                                         this._encoder.Object,
                                                                         this._clock.Object,
                                                                         this._authenticationService.Object);
        }

        [Test]
        public async Task HandleAuthenticateAsyncReturnsSuccessfulAuthenticateResultWhenAuthenticationSucceeds()
        {
            //Arrange
            var userName = "testuser";
            var password = "password1";

            var context = new DefaultHttpContext();
            var authorizationHeader = new StringValues("Basic dGVzdHVzZXI6cGFzc3dvcmQxCg==");
            context.Request.Headers.Add(HeaderNames.Authorization, authorizationHeader);

            this._authenticationService.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                                       .ReturnsAsync(new User()
                                       {
                                           Username = userName,
                                           Password = password
                                       });

            
            await _authenticationHandler.InitializeAsync(new AuthenticationScheme("BasicAuthentication",
                                                         "BasicAuthentication",
                                                         typeof(BasicAuthenticationHandler)), 
                                                         context);

            //Act
            var result = await this._authenticationHandler.AuthenticateAsync();

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual("BasicAuthentication", result.Ticket.AuthenticationScheme);
            Assert.AreEqual(userName, result.Ticket.Principal.Identity.Name);
        }

        [Test]
        public async Task HandleAuthenticateAsyncThrowsWhenAuthenticationHeaderIsMissing()
        {
            //Arrange
            var context = new DefaultHttpContext();
           
            await _authenticationHandler.InitializeAsync(new AuthenticationScheme("BasicAuthentication",
                                                         "BasicAuthentication",
                                                         typeof(BasicAuthenticationHandler)),
                                                         context);

            //Act - see Assertion.

            //Assert
            var ex = Assert.ThrowsAsync<HttpException>(() => this._authenticationHandler.AuthenticateAsync());
            Assert.AreEqual(HttpStatusCode.Unauthorized, ex.StatusCode);
            Assert.AreEqual(Resources.ErrorMessage_UnauthenticatedMissingAuthenticationHeader, ex.Message);
            Assert.AreEqual(Resources.ErrorCode_UnauthenticatedMissingAuthenticationHeader, ex.ErrorCode);
        }

        [Test]
        public async Task HandleAuthenticateAsyncThrowsWhenAuthenticationHeaderIsInvalid()
        {
            //Arrange
            var context = new DefaultHttpContext();
            var authorizationHeader = new StringValues("invalid");
            context.Request.Headers.Add(HeaderNames.Authorization, authorizationHeader);

            await _authenticationHandler.InitializeAsync(new AuthenticationScheme("BasicAuthentication",
                                                         "BasicAuthentication",
                                                         typeof(BasicAuthenticationHandler)),
                                                         context);

            //Act - see Assertion.

            //Assert
            var ex = Assert.ThrowsAsync<HttpException>(() => this._authenticationHandler.AuthenticateAsync());
            Assert.AreEqual(HttpStatusCode.Unauthorized, ex.StatusCode);
            Assert.AreEqual(Resources.ErrorMessage_UnauthenticatedInvalidAuthenticationHeader, ex.Message);
            Assert.AreEqual(Resources.ErrorCode_UnauthenticatedInvalidAuthenticationHeader, ex.ErrorCode);
        }

        [Test]
        public async Task HandleAuthenticateAsyncThrowsWhenAuthenticationCredentialsAreIncorrect()
        {
            //Arrange
            var context = new DefaultHttpContext();
            var authorizationHeader = new StringValues("Basic dGVzdHVzZXI6cGFzc3dvcmQxCg==");
            context.Request.Headers.Add(HeaderNames.Authorization, authorizationHeader);

            User user = null;
            this._authenticationService.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                                       .ReturnsAsync(user);

            await _authenticationHandler.InitializeAsync(new AuthenticationScheme("BasicAuthentication",
                                                         "BasicAuthentication",
                                                         typeof(BasicAuthenticationHandler)),
                                                         context);

            //Act - see Assertion.

            //Assert
            var ex = Assert.ThrowsAsync<HttpException>(() => this._authenticationHandler.AuthenticateAsync());
            Assert.AreEqual(HttpStatusCode.Unauthorized, ex.StatusCode);
            Assert.AreEqual(Resources.ErrorMessage_UnauthenticatedIncorrectCredentials, ex.Message);
            Assert.AreEqual(Resources.ErrorCode_UnauthenticatedIncorrectCredentials, ex.ErrorCode);
        }
    }
}
