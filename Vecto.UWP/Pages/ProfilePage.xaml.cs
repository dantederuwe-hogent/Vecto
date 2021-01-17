using System;
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
            InitializeComponent();
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            // clear token
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values.Remove("token");

            // clear password if found in vault
            var passwordVault = new PasswordVault();
            try
            {
                var credentials = passwordVault.FindAllByResource("Vecto");
                if(credentials.Count < 1) throw new Exception();

                var credentialEnumerator = credentials.GetEnumerator();
                credentialEnumerator.MoveNext();
                passwordVault.Remove(credentialEnumerator.Current);
                credentialEnumerator.Dispose();
            }
            catch { }
            finally
            {
                // get root frame and use that to navigate otherwise we navigate within the nav menu
                var parentFrame = Window.Current.Content as Frame;
                parentFrame.Navigate(typeof(LoginPage));
            }
        }
    }
}
