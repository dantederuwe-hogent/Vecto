using Vecto.Application.Profile;
using Vecto.UWP.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Vecto.UWP.Pages
{
    public sealed partial class MainPage : Page
    {
        private ProfileService _service = new ProfileService();

        public MainPage() => InitializeComponent();

        private async void ProfileNavItem_Loaded(object sender, RoutedEventArgs e)
        {
            ProfileDTO profileDTO = await _service.GetProfile();
            ProfileNavItem.Content = profileDTO.FirstName + " " + profileDTO.LastName;
        }
    }
}
