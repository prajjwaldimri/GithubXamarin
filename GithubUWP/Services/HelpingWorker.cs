 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.Storage;
using Octokit;

namespace GithubUWP.Services
{
    public static class HelpingWorker
    {
        /// <summary>
        /// Checks the validity of Roaming Key with PasswordVault. 
        /// PasswordVault uses higher resources than querying keys so it is better to check for it once in a while
        /// </summary>
        /// <returns>A Completed Task</returns>
        public static Task RoamingLoggedInKeyVerifier()
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            try
            {
                if (vault.FindAllByResource("GithubAccessToken") != null)
                {
                    if (!ApplicationData.Current.RoamingSettings.Values.ContainsKey("IsLoggedIn"))
                        ApplicationData.Current.RoamingSettings.Values.Add("IsLoggedIn", true);
                }
                else
                {
                    if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("IsLoggedIn"))
                        ApplicationData.Current.RoamingSettings.Values.Remove("IsLoggedIn");
                }
            }
            //TODO: Change the exception to a more specific one
            catch (Exception)
            {
                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("IsLoggedIn"))
                    ApplicationData.Current.RoamingSettings.Values.Remove("IsLoggedIn");
            }


            return Task.CompletedTask;
        }

        /// <summary>
        /// Adds the OAuth Access Token to the Password Vault
        /// </summary>
        /// <param name="accessToken">The OAuth Access Token to be added</param>
        /// <returns>A Completed Task</returns>
        public static Task VaultAccessTokenAdder(OauthToken accessToken)
        {
            //Storing Access Token in Credential Locker
            //More details: https://msdn.microsoft.com/en-us/windows/uwp/security/credential-locker
            var vault = new PasswordVault();
            try
            {
                if (vault.FindAllByResource("GithubAccessToken") != null)
                {
                    var vaultItems = vault.RetrieveAll();
                    foreach (var passwordCredential in vaultItems)
                    {
                        vault.Remove(passwordCredential);
                    }
                }
                vault.Add(new PasswordCredential("GithubAccessToken", "Github", accessToken.AccessToken));
            }
            catch (Exception e)
            {
                vault.Add(new PasswordCredential("GithubAccessToken", "Github", accessToken.AccessToken));
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Retrieves the OAuth AccessToken from the PasswordVault
        /// </summary>
        /// <returns>A Credentials object</returns>
        public static Credentials VaultAccessTokenRetriever()
        {
            var vault = new PasswordVault();
            if (!ApplicationData.Current.RoamingSettings.Values.ContainsKey("IsLoggedIn")) return null;
            try
            {
                if (vault.FindAllByResource("GithubAccessToken") != null)
                {
                    return new Credentials(vault.Retrieve("GithubAccessToken", "Github").Password);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }
        
    }
}
