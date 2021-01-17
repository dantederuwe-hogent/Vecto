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
    public sealed partial class PackingSectionPage : Page
    {
        private readonly IApiService _service = CustomRefitService.ForAuthenticated<IApiService>();
        private ObservableCollection<PackingItem> _items;
        private Guid _tripId, _sectionId;
        
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            dynamic guids = e.Parameter;
            _tripId = guids.TripId;
            _sectionId = guids.SectionId;

            var items = await _service.GetPackingItems(_tripId, _sectionId);
            _items = new ObservableCollection<PackingItem>(items);

            InitializeComponent();
            //AddItemAmount.NumberFormatter = new DecimalFormatter() { FractionDigits = 0 };
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
                //
                // error handling
            }
        }

        private Guid GetItemIdFromCheckBox(CheckBox cb)
        {
            if (cb is null || cb.Tag is null) throw new Exception();

            var success = Guid.TryParse(cb.Tag.ToString(), out var itemId);
            return success ? itemId : throw new Exception();
        }


        private async void AddItem_Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (await AddItemDialog.ShowAsync() != ContentDialogResult.Primary) return;

            try
            {
                var item = new ItemDTO()
                {
                    Title = AddItemName.Text,
                    Description = AddItemDesc.Text,
                    Amount = (int) Math.Ceiling(AddItemAmount.Value)
                };

                var newItem = await _service.AddPackingItem(_tripId, _sectionId, item);
                _items.Add(newItem);

                Bindings.Update();
            }
            catch
            {
                //TODO exception handling
            }
        }

        private async void EditItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as MenuFlyoutItem).DataContext as PackingItem;

            EditItemName.Text = item.Title;
            EditItemDesc.Text = item.Description;
            EditItemAmount.Value = item.Amount;

            if (await EditItemDialog.ShowAsync() != ContentDialogResult.Primary) return;

            try
            {
                var editedItem = new ItemDTO()
                {
                    Title = EditItemName.Text,
                    Description = EditItemDesc.Text,
                    Amount= (int)EditItemAmount.Value
                };

                var updated = (PackingItem)await _service.UpdatePackingItem(_tripId, _sectionId, item.Id, editedItem);
                var index = _items.IndexOf(item);
                if (index != -1) _items[index] = updated;


            }
            catch
            {
                //TODO exception handling
            }
        }

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = (sender as MenuFlyoutItem).DataContext as PackingItem;
                await _service.DeleteItem(_tripId, _sectionId, item.Id);
                _items.Remove(item);
                Bindings.Update();
            }
            catch
            {
                //TODO error handling
            }
        }

        private void ItemRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }
    }
}