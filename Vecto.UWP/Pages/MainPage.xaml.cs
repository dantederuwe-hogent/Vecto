﻿using System;
using Vecto.UWP.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            ContentFrame.Navigate(typeof(TripsPage));
        }

        private async void ProfileNavItem_Loaded(object sender, RoutedEventArgs e)
        {
            var profileDTO = await _service.GetProfile();
            ProfileNavItem.Content = profileDTO.FirstName + " " + profileDTO.LastName;
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
                return;
            }

            switch (((NavigationViewItem)sender.SelectedItem).Tag.ToString())
            {
                case "profile":
                    ContentFrame.Navigate(typeof(ProfilePage));
                    return;
                case "trips":
                    ContentFrame.Navigate(typeof(TripsPage));
                    return;
            }
        }
    }
}