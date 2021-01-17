using Newtonsoft.Json.Linq;
using Refit;
using System.Net.Http;
using System.Text;
using Vecto.Application.Register;
using Vecto.UWP.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Vecto.UWP.Pages.Authentication
{
    public sealed partial class RegisterPage : Page
    {
        private readonly IApiService _service;

        public RegisterPage()
        {
            _service = CustomRefitService.For<IApiService>();
            InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RegisterProgressRing.Visibility = Visibility.Visible;

                RegisterDTO registerDTO = new RegisterDTO
                {
                    FirstName = FirstNameTextBox.Text,
                    LastName = LastNameTextBox.Text,
                    Email = EmailTextBox.Text,
                    Password = PasswordBox.Password,
                    ConfirmPassword = RepeatPasswordBox.Password
                };
                await _service.Register(registerDTO);
                Frame.Navigate(typeof(LoginPage), EmailTextBox.Text, new SuppressNavigationTransitionInfo());
            }
            catch (ApiException ex)
            {
                dynamic errors = JObject.Parse(ex.Content)["errors"];
                ErrorTextBlock.Text = errors[0].errorMessage;
            }
            catch (HttpRequestException)
            {
                ErrorTextBlock.Text = "Can't connect to API";
            }
            finally
            {
                RegisterProgressRing.Visibility = Visibility.Collapsed;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage), null, new SuppressNavigationTransitionInfo());
        }
    }
}
