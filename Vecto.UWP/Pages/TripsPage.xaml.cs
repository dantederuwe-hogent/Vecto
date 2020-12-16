using System;
using Vecto.Application.Trips;
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
    }
}
