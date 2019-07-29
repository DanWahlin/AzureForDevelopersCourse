using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;

// Based on https://docs.microsoft.com/en-us/azure/key-vault/tutorial-net-create-vault-azure-web-app
 
 public class KeyVaultModel : PageModel
 {
     public string Secret { get; set; }

     public async Task OnGetAsync()
     {
         try
         {
             /* The next four lines of code show you how to use AppAuthentication library to fetch secrets from your key vault */
             AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
             KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
             var secret = await keyVaultClient.GetSecretAsync("https://azurefordevskeyvault.vault.azure.net", "AppSecret")
                     .ConfigureAwait(false);
             Secret = secret.Value;
         }
         /* If you have throttling errors see this tutorial https://docs.microsoft.com/azure/key-vault/tutorial-net-create-vault-azure-web-app */
         catch (KeyVaultErrorException keyVaultException)
         {
             Secret = keyVaultException.Message;
         }
     }

     // This method implements exponential backoff if there are 429 errors from Azure Key Vault
     private static long getWaitTime(int retryCount)
     {
         long waitTime = ((long)Math.Pow(2, retryCount) * 100L);
         return waitTime;
     }

     // This method fetches a token from Azure Active Directory, which can then be provided to Azure Key Vault to authenticate
     public async Task<string> GetAccessTokenAsync()
     {
         var azureServiceTokenProvider = new AzureServiceTokenProvider();
         string accessToken = await azureServiceTokenProvider.GetAccessTokenAsync("https://vault.azure.net");
         return accessToken;
     }
 }