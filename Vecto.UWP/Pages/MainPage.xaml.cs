using Vecto.UWP.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Vecto.UWP.Pages
{
    public sealed partial class MainPage : Page
    {
        private readonly IApiService _service;

        public MainPage()
        {
            _service = CustomRefitService.ForAuthenticated<IApiService>();
            InitializeComponent();

            // Select trips page by default
            VectoNavigationView.SelectedItem = TripsNavItem;
            VectoNavigationView.Header = "My Trips";
            ContentFrame.Navigate(typeof(TripsPage), VectoNavigationView);
        }

        private async void ProfileNavItem_Loaded(object sender, RoutedEventArgs e)
        {
            var profileDTO = await _service.GetProfile();
            ProfileNavItem.Content = profileDTO.FirstName;
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            VectoNavigationView.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;

            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
                VectoNavigationView.Header = "Settings";
                return;
            }

            switch (((NavigationViewItem)sender.SelectedItem).Tag.ToString())
            {
                case "profile":
                    ContentFrame.Navigate(typeof(ProfilePage));
                    VectoNavigationView.Header = "My Profile";
                    return;
                case "trips":
                    ContentFrame.Navigate(typeof(TripsPage), VectoNavigationView);
                    VectoNavigationView.Header = "My Trips";
                    return;
            }
        }

        private void VectoNavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            VectoNavigationView.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
            Frame.Navigate(typeof(MainPage), null, new SuppressNavigationTransitionInfo());
        }
    }
}
