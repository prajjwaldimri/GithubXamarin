using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace GithubXamarin.UWP.UserControls
{
    public sealed partial class IconAndTextBlocksControl : UserControl
    {
        public IconAndTextBlocksControl()
        {
            this.InitializeComponent();
        }

        public string MiddleTextBlockProperty
        {
            get { return (string)GetValue(MiddleTextBlockPropertyProperty); }
            set { SetValue(MiddleTextBlockPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MiddleTextBlockProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MiddleTextBlockPropertyProperty =
            DependencyProperty.Register("MiddleTextBlockProperty", typeof(string), typeof(IconAndTextBlocksControl), null);

        public string LastTextBlockProperty
        {
            get { return (string)GetValue(LastTextBlockPropertyProperty); }
            set { SetValue(LastTextBlockPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LastTextBlockProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LastTextBlockPropertyProperty =
            DependencyProperty.Register("LastTextBlockProperty", typeof(string), typeof(IconAndTextBlocksControl), null);



        public string FontIconGlyph
        {
            get { return (string)GetValue(FontIconGlyphProperty); }
            set { SetValue(FontIconGlyphProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FontIconGlyph.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FontIconGlyphProperty =
            DependencyProperty.Register("FontIconGlyph", typeof(string), typeof(IconAndTextBlocksControl), null);


    }
}
