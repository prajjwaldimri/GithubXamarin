using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Platform.Converters;
using Octokit;

namespace GithubXamarin.Core.Converters
{
    public class ContentToGlyphConverter : MvxValueConverter<RepositoryContent, string>
    {
        protected override string Convert(RepositoryContent value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.Type)
            {
                case ContentType.File:
                    return "\uf1c9";

                case ContentType.Dir:
                    return "\uf07b";

                case ContentType.Symlink:
                    return "";

                case ContentType.Submodule:
                    return "\uf2db";

                default:
                    return "";
            }
        }
    }
}
