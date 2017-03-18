using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace GithubXamarin.UWP.UserControls
{
    public sealed partial class LabelsControl : UserControl
    {
        public LabelsControl()
        {
            this.InitializeComponent();
        }

        public object LabelsSource
        {
            get { return (object)GetValue(LabelsSourceProperty); }
            set { SetValue(LabelsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelsSourceProperty =
            DependencyProperty.Register("LabelsSource", typeof(object), typeof(LabelsControl), null);


    }
}
