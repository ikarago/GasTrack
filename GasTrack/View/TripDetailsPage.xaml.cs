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
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Animation;
using GasTrack.Model.Helpers;
using System.Diagnostics;

namespace GasTrack.View
{

    public sealed partial class TripDetailsPage : Page
    {
        // Helpers
        private SettingsHelper settingsHelper = new SettingsHelper();


        TripManagerViewModel TripManager;
        CarManagerViewModel CarManager;
        TripViewModel SelectedTrip;
        CarViewModel SelectedCar;
        public int SelectedCarId;

        public TripDetailsPage()
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
                catch // If stuff goes to the shitter, go back to the CarSummary
                { 
                    Frame.Navigate(typeof(View.CarSummaryPage), new DrillInNavigationTransitionInfo());
                }
            }

            // Register for hardware and software back request from the system
            SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested += Page_BackRequested;
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;


            // Set selected car for navigation
            try
            {
                this.SelectedCarId = settingsHelper.GetSelectedCarId();
                Debug.WriteLine("TripDetailsPage - Navigation - CarId recieved - Car id is: " + this.SelectedCarId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                // Go back event
            }

            // TODO --> In Try - Catch zetten?
            TripManager = new TripManagerViewModel(this.SelectedCarId);
            CarManager = new CarManagerViewModel();


            // Get trip from navigation
            int tripId = (int)e.Parameter;
            this.SelectedTrip = TripManager.Trips.Where(x => x.TripId == tripId).FirstOrDefault();
            TripManager.SelectedIndex = TripManager.Trips.IndexOf(this.SelectedTrip);

            this.SelectedCar = CarManager.Cars.Where(x => x.CarId == this.SelectedCarId).FirstOrDefault();
            CarManager.SelectedIndex = CarManager.Cars.IndexOf(this.SelectedCar);

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
            Frame.Navigate(typeof(View.CarSummaryPage), this.SelectedCarId);    // Well, just go back to the CarBummery
        }




    }
}
