using GasTrack.Model;
using GasTrack.Model.Helpers;
using GasTrack.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;


namespace GasTrack.View
{
    public sealed partial class NewTripPage : Page
    {
        // Helpers
        private SettingsHelper settingsHelper = new SettingsHelper();
        ResourceHelper resourceHelper = new ResourceHelper();


        // Variabelen
        public TripManagerViewModel TripManager;
        public int SelectedCarId;


        public NewTripPage()
        {
            this.InitializeComponent();
            //TODO: Edit in CarId

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
                Debug.WriteLine("FinishTripPage - NewTrip - CarId recieved - Car id is: " + this.SelectedCarId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                // Go back event
            }

            // TODO --> In Try - Catch zetten?
            TripManager = new TripManagerViewModel(this.SelectedCarId);
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
            Frame.Navigate(typeof(View.CarSummaryPage));    // Well, just go back to the CarBummery
        }







        // Buttons
        private void btnStartTrip_Click(object sender, RoutedEventArgs e)
        {
            if (txtCounterStart.Text != null && txtCounterStart.Text != "")
            {
                //bool success = false;

                double startDecimal = 0;
                try { startDecimal = Convert.ToDouble(txtCounterStartDecimals.Text); }
                catch { }
                try
                {
                    if (Convert.ToDouble(txtCounterStart.Text) >= 0)
                    {
                        TripManager.AddNewTrip(SelectedCarId, Convert.ToDouble(txtCounterStart.Text), startDecimal);
                        Frame.Navigate(typeof(View.CarSummaryPage));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            else
            {
                resourceHelper.ShowErrorDialog("Message-WrongAmount");
            }
             
            

        }
    }
}
