using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace GithubUWP.UserControls
{
    public sealed partial class AlternatingRowListView : ListView
    {
        public static DependencyProperty OddRowBackgroundProperty= DependencyProperty.Register("OddRowBackground", typeof(Brush),
            typeof(AlternatingRowListView), null);

        public Brush OddRowBackground
        {
            get { return (Brush) GetValue(OddRowBackgroundProperty); }
            set { SetValue(OddRowBackgroundProperty, (Brush)value);}
        }

        public static DependencyProperty EvenRowBackgroundProperty = DependencyProperty.Register("EvenRowBackground", typeof(Brush),
            typeof(AlternatingRowListView), null);

        public Brush EvenRowBackground
        {
            get { return (Brush)GetValue(EvenRowBackgroundProperty); }
            set { SetValue(EvenRowBackgroundProperty, (Brush)value); }
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element,item);
            var listViewItem = element as ListViewItem;
            if (listViewItem == null) return;
            var index = IndexFromContainer(element);
            
            listViewItem.Background = (index + 1) % 2 == 1 ? OddRowBackground : EvenRowBackground;
        }
    }
}
