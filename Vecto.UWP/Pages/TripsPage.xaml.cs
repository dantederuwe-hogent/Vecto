using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private readonly IApiService _service = CustomRefitService.ForAuthenticated<IApiService>();

        private NavigationView _navigationView;
        private ObservableCollection<Trip> _trips;


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _navigationView = (NavigationView)e.Parameter;

            var trips = await _service.GetTrips();
            SetupTrips(trips);
            
            /*
             * TODO: This is an ugly fix at the moment, but it works
             * 
             * Fixes an issue where by default the application default focuses
             * the trips item on the navigation view, giving it an ugly border...
             */
            InitializeComponent();
            AddTripButton.Focus(FocusState.Pointer);
        }

        private void SetupTrips(IEnumerable<Trip> trips)
        {
            _trips = new ObservableCollection<Trip>(trips.OrderBy(t => t.StartDateTime).AsEnumerable());
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

                var trips = await _service.AddTrip(newTrip);
                SetupTrips(trips);
                Bindings.Update();
            }
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var trip = e.ClickedItem as Trip;
            _navigationView.Header = trip.Name;
            _navigationView.IsBackButtonVisible = NavigationViewBackButtonVisible.Visible;
            Frame.Navigate(typeof(TripDetailsPage), new { trip, _navigationView });
        }
    }
}
