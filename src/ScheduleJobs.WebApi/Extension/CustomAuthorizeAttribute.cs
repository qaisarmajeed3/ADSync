using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using  ScheduleJob.Domain.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace  ScheduleJob.WebApi.Extension
{
    /// <summary>
    /// Athorization attribute to validate the token from Azure AD
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public string? bearerToken;

        /// <summary>  
        /// This will Authorize User.
        /// </summary>  
        /// <returns></returns>  
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {

                if (context != null)
                {
                    bearerToken = GetTokenFromHeader(context);
                    if (bearerToken == null)
                    {
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                        context.Result = new JsonResult(MessageConstants.ProvideAuthtoken)
                        {
                            Value = new
                            {

                                Code = (int)HttpStatusCode.Unauthorized,
                                Message = MessageConstants.ProvideAuthtoken
                            },
                        };
                    }
                    else
                    {
                        ReadandValidateToken(context, bearerToken);
                    }
                }
            }
            catch (Exception)
            {
                if (context != null)
                {
                    context.Result = new JsonResult(MessageConstants.InvalidToken)
                    {
                        Value = new
                        {
                            Code = (int)HttpStatusCode.Unauthorized,
                            Message = MessageConstants.InvalidToken,
                        },
                    };
                }
            }
        }

        /// <summary>
        /// Read token from request header.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string? GetTokenFromHeader(AuthorizationFilterContext context)
        {
            context.HttpContext.Request.Headers.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues authTokens);
            string authHeader = context.HttpContext.Request.Headers["Authorization"];
            return authHeader?.Replace("Bearer ", string.Empty);
        }

        /// <summary>
        /// Validate token.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="token"></param>
        public void ReadandValidateToken(AuthorizationFilterContext context, string token)
        {
            JwtSecurityToken? securityToken = ReadToken();

            if (securityToken != null)
            {
                if (IsValidToken(token))
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

                }
                else
                {
                    context.Result = new JsonResult(MessageConstants.InvalidToken)
                    {
                        Value = new
                        {
                            Code = (int)HttpStatusCode.Unauthorized,
                            Message = MessageConstants.InvalidToken,
                        },
                    };
                }
            }
            else
            {
                context.Result = new JsonResult(MessageConstants.InvalidToken)
                {
                    Value = new
                    {
                        Code = (int)HttpStatusCode.Unauthorized,
                        Message = MessageConstants.InvalidToken,
                    },
                };
            }
        }

        /// <summary>
        /// Read token using token handler.
        /// </summary>
        /// <returns></returns>
        private JwtSecurityToken? ReadToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ReadToken(bearerToken) as JwtSecurityToken;
        }

        /// <summary>
        /// Check whether the token is valid or not
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool IsValidToken(string token)
        {
            bool isValid;
            try
            {
                var appConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                var tenantId = appConfig.GetValue<string>("AzureAd:TenantId");
                var loginUrl = appConfig.GetValue<string>("AzureAd:Instance");
                string stsDiscoveryEndpoint = loginUrl + tenantId + "/v2.0/.well-known/openid-configuration";
                var issuer = loginUrl + tenantId + "/v2.0";
                ConfigurationManager<OpenIdConnectConfiguration> configManager = new(stsDiscoveryEndpoint, new OpenIdConnectConfigurationRetriever());

                OpenIdConnectConfiguration config = configManager.GetConfigurationAsync().Result;

                TokenValidationParameters validationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateLifetime = true,
                    IssuerSigningKeys = config.SigningKeys,
                    ClockSkew = TimeSpan.Zero,
                };

                JwtSecurityTokenHandler tokendHandler = new();

                var jwtToken = tokendHandler.ValidateToken(token, validationParameters, out SecurityToken jwt);
                if (jwtToken.Identity != null)
                {
                    if (!jwtToken.Identity.IsAuthenticated)
                    {
                        isValid = true;
                    }
                    isValid = true;

                }
                else
                {
                    isValid = false;
                }
            }
            catch (Exception)
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
