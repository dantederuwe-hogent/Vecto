using Vecto.UWP.Pages.Authentication;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Vecto.UWP.Pages
{
    public sealed partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            this.InitializeComponent();
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            // clear token
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values.Remove("token");

            // clear password
            var passwordVault = new PasswordVault();
            var credentialEnumerator = passwordVault.FindAllByResource("Vecto").GetEnumerator();
            credentialEnumerator.MoveNext();
            passwordVault.Remove(credentialEnumerator.Current);

            // get root frame and use that to navigate otherwise we navigate within the nav menu
            var parentFrame = Window.Current.Content as Frame;
            parentFrame.Navigate(typeof(LoginPage));
        }
    }
}
