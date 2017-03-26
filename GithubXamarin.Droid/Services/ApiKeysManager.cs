using System;
using System.Reflection;
using System.Threading.Tasks;

namespace GithubXamarin.Droid.Services
{
    public class ApiKeysManager
    {
        public static string GithubClientId;
        public static string GithubClientSecret;


        public static Task KeyRetriever()
        {
            try
            {
                GithubClientId = ResourceLoader.GetEmbeddedResourceString(Assembly.GetAssembly(typeof(ResourceLoader)),
                    "ClientId.txt");
                GithubClientSecret = ResourceLoader.GetEmbeddedResourceString(Assembly.GetAssembly(typeof(ResourceLoader)),
                    "ClientSecret.txt");

            }
            catch (Exception)
            {
                
            }
            return Task.CompletedTask;
        }
    }
}