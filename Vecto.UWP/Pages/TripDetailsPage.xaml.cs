using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vecto.Application.Sections;
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
        private readonly IApiService _service = CustomRefitService.ForAuthenticated<IApiService>();

        private Trip _trip;
        private IList<string> _sectionTypes;
        private NavigationView _navigationView;
        
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            dynamic parameters = e.Parameter;
            _trip = parameters.trip;
            _navigationView = parameters._navigationView;

            _sectionTypes = await _service.GetSectionTypes();

            InitializeComponent(); //Initialize here


            var sections = await _service.GetTripSections(_trip.Id);
            SetupSections(sections);
        }

        private async Task SetupSections(IEnumerable<SectionDTO> sections, bool selectLastSection=false)
        {
            var last = SectionsPivot.Items?.Last();

            SectionsPivot.Items?.Clear();
            sections.ToList().ForEach(section => SectionsPivot.Items?.Add(new PivotItem {Header = section.Name}));
            
            SectionsPivot.Items?.Add(last);
            SectionsPivot.SelectedIndex = selectLastSection ? SectionsPivot.Items.Count -2 : 0;
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

        private async void CreateSection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var model = new SectionDTO()
                {
                    Name = CreateSectionName.Text,
                    SectionType = CreateSectionType.SelectedItem.ToString()
                };

                var sections = await _service.AddTripSection(_trip.Id, model);

                await SetupSections(sections, true);
            }
            catch
            {
                //TODO
            }
        }
    }
}
