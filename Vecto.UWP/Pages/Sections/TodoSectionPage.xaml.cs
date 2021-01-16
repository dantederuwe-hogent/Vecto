using System.Collections.ObjectModel;
using Vecto.Core.Entities;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Vecto.UWP.Pages.Sections
{
    public sealed partial class TodoSectionPage : Page
    {
        private ObservableCollection<TodoItem> _todoItems;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var sectionIndex = e.Parameter;

            _todoItems = new ObservableCollection<TodoItem>() //TODO
            {
                new TodoItem(){Title = "test1"},
                new TodoItem(){Title = "test2"},
                new TodoItem(){Title = "test3"},
            };

            InitializeComponent();
        }
    }
}
