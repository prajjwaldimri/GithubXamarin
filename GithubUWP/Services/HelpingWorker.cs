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
            catch (Exception e)
            {
                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("IsLoggedIn"))
                    ApplicationData.Current.RoamingSettings.Values.Remove("IsLoggedIn");
            }


            return Task.CompletedTask;
        }

        public static Task VaultApiKeyAdder(OauthToken accessToken)
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

        public static Credentials VaultApiKeyRetriever()
        {
            var vault = new PasswordVault();
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("IsLoggedIn"))
            {
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
            }
            return null;
        }
    }
}
