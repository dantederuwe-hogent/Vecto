using System;
using Vecto.Application.Login;
using Vecto.UWP.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Vecto.UWP.Pages.Authentication
{
    public sealed partial class LoginPage : Page
    {
        private readonly LoginService _service = new LoginService();

        public LoginPage() => InitializeComponent();

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var model = new LoginDTO() { Email = EmailTextBox.Text, Password = PasswordBox.Password };
            bool rememberMe = RememberMe.IsChecked ?? false;

            await _service.Login(model, rememberMe);

            Frame.Navigate(typeof(MainPage));
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage), null, new SuppressNavigationTransitionInfo());
        }
    }
}
