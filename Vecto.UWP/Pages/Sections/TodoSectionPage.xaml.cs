using System;
using System.Collections.ObjectModel;
using Vecto.Application.Items;
using Vecto.Core.Entities;
using Vecto.UWP.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Vecto.UWP.Pages.Sections
{
    public sealed partial class TodoSectionPage : Page
    {
        private readonly IApiService _service = CustomRefitService.ForAuthenticated<IApiService>();
        private ObservableCollection<TodoItem> _todoItems;
        private Guid _tripId, _sectionId;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            dynamic guids = e.Parameter;
            _tripId = guids.TripId;
            _sectionId = guids.SectionId;

            var items = await _service.GetTodoItems(_tripId, _sectionId);
            _todoItems = new ObservableCollection<TodoItem>(items);

            InitializeComponent();
        }

        private async void CheckBox_Toggle(object sender, RoutedEventArgs e)
        {
            try
            {
                var itemId = GetItemIdFromCheckBox(sender as CheckBox);
                await _service.ToggleItem(_tripId, _sectionId, itemId);
                Bindings.Update();
            }
            catch
            {
                //TODO error handling
            }
        }

        private Guid GetItemIdFromCheckBox(CheckBox cb)
        {
            if (cb is null || cb.Tag is null) throw new Exception();

            var success = Guid.TryParse(cb.Tag.ToString(), out var itemId);
            return success ? itemId : throw new Exception();
        }


        private async void AddTodo_Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (await AddTodoDialog.ShowAsync() != ContentDialogResult.Primary) return;

            try
            {
                var todo = new ItemDTO()
                {
                    Title = AddTodoName.Text,
                    Description = AddTodoDesc.Text
                };

                var newItem = await _service.AddTodoItem(_tripId, _sectionId, todo);
                _todoItems.Add(newItem);

                Bindings.Update();
            }
            catch
            {
                //TODO exception handling
            }
        }

        private async void EditTodo_Click(object sender, RoutedEventArgs e)
        {
            var todo = (sender as MenuFlyoutItem).DataContext as TodoItem;

            EditTodoName.Text = todo.Title;
            EditTodoDesc.Text = todo.Description;
            if (await EditTodoDialog.ShowAsync() != ContentDialogResult.Primary) return;

            try
            {
                var editedTodo = new ItemDTO()
                {
                    Title = EditTodoName.Text,
                    Description = EditTodoDesc.Text
                };

                var updated = (TodoItem)await _service.UpdateTodoItem(_tripId, _sectionId, todo.Id, editedTodo);
                var index = _todoItems.IndexOf(todo);
                if (index != -1) _todoItems[index] = updated;

                //Frame.Navigate(GetType(), new SurpressNavigationTransitionInfo());
            }
            catch
            {
                //TODO exception handling
            }
        }

        private async void DeleteTodo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var todo = (sender as MenuFlyoutItem).DataContext as TodoItem;
                await _service.DeleteItem(_tripId, _sectionId, todo.Id);
                _todoItems.Remove(todo);
                Bindings.Update();
            }
            catch
            {
                //TODO error handling
            }
        }

        private void TodoRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }
    }
}