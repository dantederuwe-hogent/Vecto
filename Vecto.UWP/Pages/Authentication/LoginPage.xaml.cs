using Vecto.Application.Login;
using Vecto.UWP.Services;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Vecto.UWP.Pages.Authentication
{
    public sealed partial class LoginPage : Page
    {
        private readonly IApiService _service;

        public LoginPage()
        {
            _service = CustomRefitService.Get();
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var model = new LoginDTO() { Email = EmailTextBox.Text, Password = PasswordBox.Password };
            bool rememberMe = RememberMe.IsChecked ?? false;

            var token = await _service.Login(model);

            if (rememberMe) StorePassword(model);
            StoreToken(token);

            Frame.Navigate(typeof(MainPage));
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage), null, new SuppressNavigationTransitionInfo());
        }

        private static void StorePassword(LoginDTO model)
        {
            var vault = new PasswordVault();
            vault.Add(new PasswordCredential("Vecto", model.Email, model.Password));
        }

        private static void StoreToken(string token)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["token"] = token.Replace(@"""", "");
        }
    }
}
