using Azure.Identity;
using Common.Environment;
using Microsoft.Extensions.Configuration;

namespace KeyVault;

// Would like to put this under test. Found it challenging.
public static class KeyVault
{
    public static void AddKeyVaultUsingManagedIdentityOnlyIfNotRunningLocally(this IConfigurationBuilder configurationBuilder, Dictionary<CommonEnvironment, string> environmentToUrlMappings)
    {
        var environment = CommonEnvironmentExtensions.GetEnvironment();
        
        if (environment == CommonEnvironment.LocalDevelopment) return;
        if (!environmentToUrlMappings.TryGetValue(environment, out var url)) throw new ArgumentException($"No Key Vault URL mapping found for environment: {environment}");
        
        var keyVaultUrl = new Uri(url);
        AddAzureKeyVaultUsingManagedIdentity(configurationBuilder, keyVaultUrl);
    }

    public static void AddKeyVaultForDevelopersOnlyIfRunningLocally(this IConfigurationBuilder configurationBuilder, string url)
    {
        if (CommonEnvironmentExtensions.GetEnvironment() != CommonEnvironment.LocalDevelopment) return;
        AddAzureKeyVaultUsingAzCliOrInteractiveBrowserOrAzureCli(configurationBuilder, new Uri(url));
    }

    private static void AddAzureKeyVaultUsingAzCliOrInteractiveBrowserOrAzureCli(IConfigurationBuilder configurationBuilder, Uri keyVaultUrl)
    {
        configurationBuilder.AddAzureKeyVault(keyVaultUrl, new DefaultAzureCredential(new DefaultAzureCredentialOptions
        {
            ExcludeWorkloadIdentityCredential = true,
            ExcludeAzurePowerShellCredential = true,
            ExcludeAzureDeveloperCliCredential = true,
            ExcludeEnvironmentCredential = true,
            ExcludeVisualStudioCredential = true,
            ExcludeAzureCliCredential = false,
            ExcludeSharedTokenCacheCredential = true,
            ExcludeManagedIdentityCredential = true,
            ExcludeInteractiveBrowserCredential = false
        }));
    }
    
    private static void AddAzureKeyVaultUsingManagedIdentity(IConfigurationBuilder configurationBuilder, Uri keyVaultUrl)
    {
        configurationBuilder.AddAzureKeyVault(keyVaultUrl, new DefaultAzureCredential(new DefaultAzureCredentialOptions
        {
            ExcludeWorkloadIdentityCredential = false,
            ExcludeAzurePowerShellCredential = true,
            ExcludeAzureDeveloperCliCredential = true,
            ExcludeEnvironmentCredential = true,
            ExcludeVisualStudioCredential = true,
            ExcludeAzureCliCredential = true,
            ExcludeSharedTokenCacheCredential = true,
            ExcludeManagedIdentityCredential = false,
            ExcludeInteractiveBrowserCredential = true,
        }));
    }
}