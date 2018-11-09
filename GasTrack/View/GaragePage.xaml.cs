using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GasTrack.ViewModel;
using System.Diagnostics;
using GasTrack.Model.Helpers;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Animation;

namespace GasTrack.View
{

    public sealed partial class GaragePage : Page
    {
        // Helpers
        private SettingsHelper settingsHelper = new SettingsHelper();

        // Variables
        CarManagerViewModel carManager;
        CarViewModel selectedCar;


        public GaragePage()
        {
            this.InitializeComponent();
            carManager = new CarManagerViewModel();

        }



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var backStack = Frame.BackStack;
            var backStackCount = backStack.Count;


            if (backStackCount > 0)
            {
                var masterPageEntry = backStack[backStackCount - 1];
                backStack.RemoveAt(backStackCount - 1);

                try
                {
                    var modifiedEntry = new PageStackEntry(masterPageEntry.SourcePageType, null, masterPageEntry.NavigationTransitionInfo);
                    backStack.Add(modifiedEntry);
                }
                catch // If stuff goes to the shitter, go back to the CarSummary
                {
                    Frame.Navigate(typeof(View.CarSummaryPage), new DrillInNavigationTransitionInfo());
                }
            }

            // Register for hardware and software back request from the system
            SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            //systemNavigationManager.BackRequested += Page_BackRequested;
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;


            // Set selected car for navigation
            try
            {
                //this.selectedCarId = settingsHelper.GetSelectedCarId();
                //Debug.WriteLine("FinishTripPage - Navigation - CarId recieved - Car id is: " + this.selectedCarId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                // Go back event
            }

            // TODO --> In Try - Catch zetten?
            //TripManager = new TripManagerViewModel(this.selectedCarId);
        }

        // Go back-stuff
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested -= Page_BackRequested;
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }
        private void Page_BackRequested(object sender, BackRequestedEventArgs e)
        {
            // Mark event as handled so we don't get bounced out of the app.
            e.Handled = true;

            OnBackRequested();
        }
        private void OnBackRequested()
        {
            Frame.Navigate(typeof(View.GaragePage));    // Well, just go back to the CarSummary
        }




        // Buttons
        private void cbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //this.CarManager.Add("Peter Pan", "Peter Pan's auto", "11-AA-BB", 0.12);
            Frame.Navigate(typeof(View.NewCarPage));
        }


        private void fcbtnDeleteCar_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCar != null)
            {
                bool success =  this.carManager.Delete(selectedCar);
                if (success == true)
                {
                    selectedCar = null;
                }
            }
        }

        private void fcbtnEditCar_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCar != null)
            {
                Frame.Navigate(typeof(View.NewCarPage), selectedCar.CarId);
            }
        }


        // Gestures
        private void rpCarItem_Holding(object sender, HoldingRoutedEventArgs e)
        {
            Debug.WriteLine("GaragePage - Rightclicked Car");

            FrameworkElement senderElement = sender as FrameworkElement;
            // To get the car that has been clicked on...
            selectedCar = senderElement.DataContext as CarViewModel;
            // Now set that car as the selected one in the CarManager
            int currentItemIndex = this.carManager.Cars.IndexOf(selectedCar);
            this.carManager.SelectedIndex = currentItemIndex;

            // Now show the flyout :)
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }
        private void rpCarItem_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            rpCarItem_Holding(sender, new HoldingRoutedEventArgs());
        }

        private void lvCars_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = (CarViewModel)e.ClickedItem;

            Debug.WriteLine("Garage - Selected car: " + clickedItem.CarId + " - " + clickedItem.CarName);
            this.settingsHelper.UpdateSelectedCarId(clickedItem.CarId);
            Frame.Navigate(typeof(View.CarSummaryPage)); // Navigate to the CarSummary by sending the CarId --> The TripManager will use this to find the correct trips
        }


    }
}
