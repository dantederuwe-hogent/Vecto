using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Vecto.UWP.Pages
{
    public sealed partial class SettingsPage : Page
    {
        private ApplicationDataContainer _localSettings;

        public SettingsPage()
        {
            _localSettings = ApplicationData.Current.LocalSettings;
            this.InitializeComponent();
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ((ComboBox)sender).SelectedIndex;
            _localSettings.Values["theme"] = index;
            ((Frame)Window.Current.Content).RequestedTheme = (ElementTheme)index;
        }

        private void ThemeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (_localSettings.Values.Keys.Contains("theme"))
                ((ComboBox)sender).SelectedIndex = (int)_localSettings.Values["theme"];
        }
    }
}
