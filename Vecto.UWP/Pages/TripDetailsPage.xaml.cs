using System;
using Vecto.Application.Trips;
using Vecto.Core.Entities;
using Vecto.UWP.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Vecto.UWP.Pages
{
    public sealed partial class TripDetailsPage : Page
    {
        private readonly IApiService _service;

        private Trip _trip;
        private NavigationView _navigationView;

        public TripDetailsPage()
        {
            _service = CustomRefitService.ForAuthenticated<IApiService>();

            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            dynamic parameters = e.Parameter;
            _trip = parameters.trip;
            _navigationView = parameters._navigationView;
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            TripNameTextBox.Text = _trip.Name;
            TripStartDatePicker.Date = new DateTimeOffset((DateTime)_trip.StartDateTime);
            TripEndDatePicker.Date = new DateTimeOffset((DateTime)_trip.EndDateTime);

            if (await EditTripDialog.ShowAsync() == ContentDialogResult.Primary)
            {
                var editedTrip = new TripDTO
                {
                    Name = TripNameTextBox.Text,
                    StartDateTime = TripStartDatePicker.Date.DateTime,
                    EndDateTime = TripEndDatePicker.Date.DateTime
                };

                try
                {
                    _trip = await _service.UpdateTrip(_trip.Id, editedTrip);

                    _navigationView.Header = _trip.Name;
                    Bindings.Update();
                }
                catch (Exception)
                {
                    //TODO: Exception handling
                }
            }
        }
    }
}
