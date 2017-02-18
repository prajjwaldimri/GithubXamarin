using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace GithubXamarin.UWP.Services
{
    /// <summary>
    /// Retrieves the API Keys from a local file.
    /// </summary>
    public class ApiKeysManager
    {
        public static string GithubClientId;
        public static string GithubClientSecret;


        public static async Task KeyRetriever()
        {
            try
            {
                var clientId =
                    await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/ClientId.txt"));
                using (var sRead = new StreamReader(await clientId.OpenStreamForReadAsync()))
                {
                    GithubClientId = await sRead.ReadToEndAsync();
                }

                var clientSecret =
                    await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Assets/ClientSecret.txt"));
                using (var sRead = new StreamReader(await clientSecret.OpenStreamForReadAsync()))
                {
                    GithubClientSecret = await sRead.ReadToEndAsync();
                }
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("No API Keys Found. Check ApiKeysManager class to resolve this error");
            }
        }
    }
}
