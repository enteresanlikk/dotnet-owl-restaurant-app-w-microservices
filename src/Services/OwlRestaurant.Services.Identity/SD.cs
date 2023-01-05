using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace OwlRestaurant.Services.Identity;

public static class SD
{
    public const string Admin = "Admin";
    public const string Customer = "Customer";

    public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>()
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Email(),
        new IdentityResources.Profile(),
    };

    public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>()
    {
        new ApiScope("owl", "Owl server."),
        new ApiScope("read", "Read data."),
        new ApiScope("write", "Write data."),
        new ApiScope("delete", "Delete data."),
    };

    public static IEnumerable<Client> Clients => new List<Client>()
    {
        new Client() { 
            ClientId = "clientid",
            ClientSecrets = { new Secret("testsecretchangethissecret".Sha256()) },
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes = new List<string>() {
                "read",
                "write",
                IdentityServerConstants.StandardScopes.Profile
            }
        },
        new Client() { 
            ClientId = "owl",
            ClientSecrets = { new Secret("testsecretchangethissecret".Sha256()) },
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris = { "https://localhost:7000/signin-oidc" },
            PostLogoutRedirectUris = { "https://localhost:7000/signout-callback-oidc" },
            AllowedScopes = new List<string>() {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Email,
                "owl"
            }
        }
    };
}
