using Vecto.UWP.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Vecto.UWP.Pages
{
    public sealed partial class MainPage : Page
    {
        private readonly IApiService _service;

        public MainPage()
        {
            _service = CustomRefitService.GetAuthenticated();
            InitializeComponent();
        }

        private async void ProfileNavItem_Loaded(object sender, RoutedEventArgs e)
        {
            var profileDTO = await _service.GetProfile();
            ProfileNavItem.Content = profileDTO.FirstName + " " + profileDTO.LastName;
        }
    }
}
