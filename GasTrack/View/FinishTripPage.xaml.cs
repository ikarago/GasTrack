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
using System.Globalization;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Core;
using GasTrack.Model.Helpers;
using System.Diagnostics;

namespace GasTrack.View
{
    public sealed partial class FinishTripPage : Page
    {
        // Helpers
        private SettingsHelper settingsHelper = new SettingsHelper();

        // Variables
        public TripManagerViewModel TripManager;
        public int selectedCarId;


        public FinishTripPage()
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
                this.selectedCarId = settingsHelper.GetSelectedCarId();
                Debug.WriteLine("FinishTripPage - Navigation - CarId recieved - Car id is: " + this.selectedCarId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                // Go back event
            }

            // TODO --> In Try - Catch zetten?
            TripManager = new TripManagerViewModel(this.selectedCarId);
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
            Frame.Navigate(typeof(View.CarSummaryPage));    // Well, just go back to the CarSummary
        }





        // Buttons
        private void btnEndTrip_Click(object sender, RoutedEventArgs e)
        {
            if (txtCounterEnd.Text != null)
            {
                bool success = false;

                double endDecimal = 0;
                try { endDecimal = Convert.ToDouble(txtCounterEndDecimals.Text); }
                catch { }

                try { success = TripManager.FinishTrip(Convert.ToDouble(txtCounterEnd.Text), endDecimal); }
                catch { }

                if (success == true)
                {
                    Frame.Navigate(typeof(View.CarSummaryPage));
                }
            }
        }
    }
}
