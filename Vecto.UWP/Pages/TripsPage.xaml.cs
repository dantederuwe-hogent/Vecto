using Vecto.UWP.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Vecto.UWP.Pages
{
    public sealed partial class TripsPage : Page
    {
        private readonly IApiService _service;
        public TripsPage()
        {
            _service = CustomRefitService.ForAuthenticated<IApiService>();
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var trips = await _service.GetTrips();
            cvsTrips.Source = trips;
        }
    }
}
