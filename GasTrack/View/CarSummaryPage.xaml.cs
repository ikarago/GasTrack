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
using Windows.UI.Xaml.Media.Animation;
using System.Diagnostics;
using GasTrack.Model.Helpers;
using Windows.UI.Core;

namespace GasTrack.View
{
    public sealed partial class CarSummaryPage : Page
    {
        // Helpers
        private SettingsHelper settingsHelper = new SettingsHelper();

        // Variables
        public TripManagerViewModel tripManager;
        public CarManagerViewModel carManager;
        public CarViewModel selectedCar;
        public int selectedCarId;


        public CarSummaryPage()
        {
            this.InitializeComponent();



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
                catch // If stuff goes to the shitter, go back to the Garage
                {
                    Debug.WriteLine("CarSummaryPage - Naviagtion to the shits! Go back to garage");
                    this.settingsHelper.UpdateSelectedCarId(-1);
                    Frame.Navigate(typeof(View.GaragePage), new DrillInNavigationTransitionInfo());
                }
            }

            // Register for hardware and software back request from the system
            SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested += Page_BackRequested;
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            


            // Try to get the CarId. If this has not been filled in (due to an error or going back to the GaragePage) let the user go back to the GaragePage
            try
            {
                this.selectedCarId = settingsHelper.GetSelectedCarId();
                Debug.WriteLine("CarSummaryPage - Navigation - CarId recieved - Car id is: " + this.selectedCarId );
                carManager = new CarManagerViewModel();
                selectedCar = carManager.GetCarById(selectedCarId);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                // Go back event
                Debug.WriteLine("CarSummaryPage - Navigation - CarId empty -> Go back to GaragePage");
                Frame.Navigate(typeof(View.GaragePage));
            }


            this.tripManager = new TripManagerViewModel(this.selectedCarId);

            // Layout check for a current trip
            this.UpdateButtons();

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
            settingsHelper.UpdateSelectedCarId(-1);
            Frame.Navigate(typeof(View.GaragePage));    // Well, just go back to the CarSummary
        }


        // Methods
        private void UpdateTotals()
        {
            tblBMTotalsDistance.Text = tripManager.TotalDistance.ToString();
            tblSMTotalsDistance.Text = tripManager.TotalDistance.ToString();
            tblBMTotalsCost.Text = tripManager.TotalCost.ToString();
            tblSMTotalsCost.Text = tripManager.TotalCost.ToString();
        }

        private void UpdateButtons()
        {
            if (tripManager.currentTrip == null)
            {
                rpBottomMessageFinishTrip.Visibility = Visibility.Collapsed;
                rpTopMessageTotalsBig.Visibility = Visibility.Visible;
                rpTopMessageTotalsSmall.Visibility = Visibility.Collapsed;
                rpBottomMessageStartTrip.Visibility = Visibility.Visible;
            }
            else
            {
                rpTopMessageTotalsBig.Visibility = Visibility.Collapsed;
                rpBottomMessageFinishTrip.Visibility = Visibility.Visible;
                rpBottomMessageStartTrip.Visibility = Visibility.Collapsed;
                rpTopMessageTotalsSmall.Visibility = Visibility.Visible;
            }
        }




        // Buttons
        private void btnStartTrip_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(View.NewTripPage), new DrillInNavigationTransitionInfo());
            rpTopMessageTotalsBig.Visibility = Visibility.Collapsed;
            rpBottomMessageFinishTrip.Visibility = Visibility.Visible;
            rpBottomMessageStartTrip.Visibility = Visibility.Collapsed;
            rpTopMessageTotalsSmall.Visibility = Visibility.Visible;
        }

        private void btnFinishTrip_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(View.FinishTripPage), new DrillInNavigationTransitionInfo());
            rpBottomMessageFinishTrip.Visibility = Visibility.Collapsed;
            rpTopMessageTotalsBig.Visibility = Visibility.Visible;
            rpTopMessageTotalsSmall.Visibility = Visibility.Collapsed;
            rpBottomMessageStartTrip.Visibility = Visibility.Visible;
        }

        private void cbtnClearList_Click(object sender, RoutedEventArgs e)
        {
            tripManager.DeleteAll();
            this.UpdateTotals();
            this.UpdateButtons();
        }

        private void cbtnSettings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(View.SettingsPage), new DrillInNavigationTransitionInfo());
        }

        private void cbtnDeleteTrip_Click(object sender, RoutedEventArgs e)
        {
            this.tripManager.Delete(this.tripManager.SelectedTrip);
        }



        private void lvTrips_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("CarSummary - Selected trip: " + this.tripManager.SelectedTrip.TripId + " - " + this.tripManager.SelectedTrip.TripName);
            Frame.Navigate(typeof(View.TripDetailsPage), this.tripManager.SelectedTrip.TripId); // Navigate to the TripDetailsPage by sending the TripId --> The tripManager will use this to find the correct trips
        }

        private void cbtnGarage_Click(object sender, RoutedEventArgs e)
        {
            settingsHelper.UpdateSelectedCarId(-1);
            Frame.Navigate(typeof(View.GaragePage));
        }
    }
}
