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
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Animation;

namespace GasTrack.View
{    
    public sealed partial class NewCarPage : Page
    {
        CarManagerViewModel carManager;
        CarViewModel selectedCar;
        bool updateTime = false;

        public NewCarPage()
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
                    Frame.Navigate(typeof(View.GaragePage), new DrillInNavigationTransitionInfo());
                }
            }

            // Register for hardware and software back request from the system
            SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested += Page_BackRequested;
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;


            // Check if an car has been send with it
            try
            {
                selectedCar = this.carManager.GetCarById((int)e.Parameter);
                txtCarName.Text = selectedCar.CarName;
                txtOwner.Text = selectedCar.CarOwner;
                txtLicense.Text = selectedCar.LicensePlate;
                txtCost.Text = selectedCar.CostPerDistance.ToString();
            }
            catch
            {
                Debug.WriteLine("NewCarPage - Time for a new car!");
            }

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
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            double costPerDistance = 0;
            try
            {
                costPerDistance = Convert.ToDouble(txtCost.Text);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("NewCarPage - Can't convert cost");
                Debug.WriteLine(ex);
            }

            if (selectedCar != null)
            {
                carManager.Update(selectedCar, txtOwner.Text, txtCarName.Text, txtLicense.Text, costPerDistance);
            }
            else
            {
                carManager.Add(txtOwner.Text, txtCarName.Text, txtLicense.Text, costPerDistance);
            }

            // Go back to the Garage
            Frame.Navigate(typeof(View.GaragePage));
        }
    }
}
