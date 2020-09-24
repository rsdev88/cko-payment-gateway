using APIAuthentication.Entities;
using APIAuthentication.Services;
using ApiSharedLibrary.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentGatewayApi.Exceptions;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace APIAuthentication.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IApiAuthenicationService _authenticationService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IApiAuthenicationService authenticationService
            )
            : base(options, logger, encoder, clock)
        {
            this._authenticationService = authenticationService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                //The thrown HTTPExceptions in this class will be caught, logged and handled within the global exception middleware.
                throw new HttpException(HttpStatusCode.Unauthorized, 
                                        Resources.ErrorCode_UnauthenticatedMissingAuthorisationHeader, 
                                        Resources.ErrorMessage_UnauthenticatedMissingAuthorisationHeader);
            }

            User user;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var username = credentials[0];
                var password = credentials[1];
                user = await this._authenticationService.Authenticate(username, password);
            }
            catch
            {
                throw new HttpException(HttpStatusCode.Unauthorized,
                        Resources.ErrorCode_UnauthenticatedInvalidAuthorisationHeader,
                        Resources.ErrorMessage_UnauthenticatedInvalidAuthorisationHeader);
            }

            if (user == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized,
                        Resources.ErrorCode_UnauthenticatedIncorrectCredentials,
                        Resources.ErrorMessage_UnauthenticatedIncorrectCredentials);
            }

            var claims = new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Name, user.Username),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
