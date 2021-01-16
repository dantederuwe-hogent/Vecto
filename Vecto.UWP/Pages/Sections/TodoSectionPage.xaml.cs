using System;
using System.Collections.ObjectModel;
using Vecto.Core.Entities;
using Vecto.UWP.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Vecto.UWP.Pages.Sections
{
    public sealed partial class TodoSectionPage : Page
    {
        private readonly IApiService _service = CustomRefitService.ForAuthenticated<IApiService>();
        private ObservableCollection<TodoItem> _todoItems;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            dynamic guids = e.Parameter;
            (var tripId, var sectionId) = ((Guid) guids.TripId, (Guid) guids.SectionId);

            var items = await _service.GetTodoItems(tripId, sectionId);
            _todoItems = new ObservableCollection<TodoItem>(items);

            InitializeComponent();
        }
    }
}
