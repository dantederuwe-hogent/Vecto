using System;
using Vecto.Application.Trips;
using Vecto.Core.Entities;
using Vecto.UWP.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Vecto.UWP.Pages
{
    public sealed partial class TripsPage : Page
    {
        private readonly IApiService _service;

        private NavigationView _navigationView;

        public TripsPage()
        {
            _service = CustomRefitService.ForAuthenticated<IApiService>();
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _navigationView = (NavigationView)e.Parameter;

            var trips = await _service.GetTrips();
            cvsTrips.Source = trips;

            /*
             * TODO: This is an ugly fix at the moment, but it works
             * 
             * Fixes an issue where by default the application default focuses
             * the trips item on the navigation view, giving it an ugly border...
             */
            AddTripButton.Focus(FocusState.Pointer);
        }

        private async void AddTripButton_Click(object sender, RoutedEventArgs e)
        {
            NewTripNameTextBox.Text = "";
            NewTripStartDatePicker.Date = DateTimeOffset.Now;
            NewTripEndDatePicker.Date = DateTimeOffset.Now.AddDays(7);

            ContentDialogResult result = await AddTripDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                TripDTO newTrip = new TripDTO
                {
                    Name = NewTripNameTextBox.Text,
                    StartDateTime = NewTripStartDatePicker.Date.DateTime,
                    EndDateTime = NewTripEndDatePicker.Date.DateTime
                };

                cvsTrips.Source = await _service.AddTrip(newTrip);
            }
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var trip = (Trip)e.ClickedItem;
            _navigationView.Header = trip.Name;
            _navigationView.IsBackButtonVisible = NavigationViewBackButtonVisible.Visible;
            Frame.Navigate(typeof(TripDetailsPage), new { trip, _navigationView });
        }
    }
}
