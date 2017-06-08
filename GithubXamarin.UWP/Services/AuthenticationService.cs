using System;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.Security.Cryptography;
using Windows.UI.Xaml;

namespace GithubXamarin.UWP.Services
{
    public class AuthenticationService
    {
        public static async void Authenticate()
        {
            var authResult = await AuthenticateUsingWindowsHello();
            if (!authResult)
            {
                Application.Current.Exit();
            }
        }

        private static async Task<bool> AuthenticateUsingWindowsHello()
        {
            if (await KeyCredentialManager.IsSupportedAsync())
            {
                // Get credentials for current user and app
                var result = await KeyCredentialManager.OpenAsync("GitIt-Hello");
                if (result.Credential != null)
                {
                    var signResult =
                        await result.Credential.RequestSignAsync(CryptographicBuffer.ConvertStringToBinary("LoginAuth",
                            BinaryStringEncoding.Utf8));

                    return signResult.Status == KeyCredentialStatus.Success;
                }

                // If no credentials found create one
                var creationResult = await KeyCredentialManager.RequestCreateAsync("GitIt-Hello",
                    KeyCredentialCreationOption.ReplaceExisting);
                return creationResult.Status == KeyCredentialStatus.Success;
            }
            return true;
        }

    }
}
