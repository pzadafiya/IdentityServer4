using IdentityModel;
using IdentityServer.Common;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>

            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "role",
                    DisplayName="User Role",
                    Description="The application can see your role.",
                    UserClaims = new[]{JwtClaimTypes.Role,ClaimTypes.Role},
                    ShowInDiscoveryDocument = true,
                    Required=true,
                    Emphasize = true
                }
            };

        public static IEnumerable<ApiResource> GetApis() =>
           new List<ApiResource> {
                 new ApiResource("WebApi", new string[]{ "rc.api.identity"})
                 {
                    UserClaims = { "role" }
                 },
                new ApiResource("OtherApi", "My API",
                    claimTypes: new string[]
                    {
                        JwtClaimTypes.GivenName,
                        JwtClaimTypes.Name,
                        JwtClaimTypes.PreferredUserName,
                        JwtClaimTypes.FamilyName,
                        JwtClaimTypes.Email,
                        JwtClaimTypes.Role,
                        "office"
                    })
           };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>{
                new Client {
                    ClientId = "client_id",
                    ClientSecrets = { new Secret("client_secret".ToSha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "WebApi" }
                },
                new Client
                {
                    ClientId = "client_id_pass",
                    ClientSecrets = { new Secret("client_secret_pass".Sha256()) },
                     
                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword ,
                    
                    // scopes that client has access to
                    AllowedScopes = {
                        "WebApi",
                        "OtherApi"
                    },
                },
                 new Client
                 {
                    ClientId = "client_id_mvc",
                    ClientSecrets ={ new Secret("client_secret_mvc".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code ,
                    RedirectUris = { ConfigSettings.MvcClientURL + "signin-oidc" },
                    PostLogoutRedirectUris = { ConfigSettings.MvcClientURL + "Home/Index" },
                    RequirePkce = true,
                    AllowedScopes =
                     {
                         "WebApi",
                         IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                         IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                         "role"
                     },

                    // put all the claims in in id token
                     AlwaysIncludeUserClaimsInIdToken = false,
                     AllowOfflineAccess = true,
                     RequireConsent = true
                },
                 new Client
                 {
                    ClientId = "client_id_postman",
                    ClientSecrets ={ new Secret("client_secret_postman".Sha256()) },
                    ClientName="PostMan Login",
                    AllowedGrantTypes = { GrantType.AuthorizationCode },
                    AllowAccessTokensViaBrowser =true,
                    LogoUri = null,
                    RedirectUris = { "https://www.getpostman.com/oauth2/callback" },
                    Enabled = true,
                    PostLogoutRedirectUris = { ConfigSettings.MvcClientURL +"signout-callback-oidc" },
                    ClientUri = null,
                    AllowedScopes =
                     {
                         "WebApi",
                         "OtherApi",
                         IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                         IdentityServer4.IdentityServerConstants.StandardScopes.Profile
                     }
                }
            };
    }
}
