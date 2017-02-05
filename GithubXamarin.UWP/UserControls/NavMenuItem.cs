using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace GithubXamarin.UWP.UserControls
{
    /// <summary>
    /// Reference: https://github.com/Microsoft/Windows-universal-samples/blob/master/Samples/XamlNavigation/cs/NavMenuItem.cs
    /// </summary>
    public class NavMenuItem : INotifyPropertyChanged
    {
        public string Label { get; set; }
        public string Glyph { get; set; }
        public FontFamily FontFamily { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected;}
            set
            {
                _isSelected = value;
                SelectedVis = value ? Visibility.Visible : Visibility.Collapsed;
                this.OnPropertyChanged("IsSelected");
            }
        }

        private Visibility _selectedVis = Visibility.Collapsed;
        public Visibility SelectedVis
        {
            get { return _selectedVis; }
            set
            {
                _selectedVis = value;
                this.OnPropertyChanged("SelectedVis");
            }
        }

        public Type DestPage { get; set; }
        public object Arguments { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged(string propertyName)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
