namespace GithubXamarin.Core.Utility
{
    public static class HtmlUrlToRepositoryNameConverter
    {
        /// <summary>
        /// Returns name of repository from htmlUrl received from Github API
        /// </summary>
        /// <param name="htmlUrl">HtmlUrl received from the Github API</param>
        /// <param name="userName">Login for the user</param>
        /// <returns>Repository Name</returns>
        public static string Convert(string htmlUrl, string userName)
        {
            if (string.IsNullOrWhiteSpace(htmlUrl) || string.IsNullOrWhiteSpace(userName))
            {
                return string.Empty;
            }

            htmlUrl = htmlUrl.Remove(0, "https://github.com/".Length);
            htmlUrl = htmlUrl.Remove(0, userName.Length + 1);
            var index = htmlUrl.IndexOf("/");
            htmlUrl = htmlUrl.Remove(index, htmlUrl.Length - index);
            return htmlUrl;
        }
    }
}
