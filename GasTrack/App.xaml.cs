using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite.Net;
using Windows.Storage;
using GasTrack.Model;
using System.Threading.Tasks;
using GasTrack.Model.Helpers;

namespace GasTrack
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        /// 

        // Initialization for dbase shiz
        public static string DB_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "db_v1.sqlite");
        public static SQLite.Net.Platform.WinRT.SQLitePlatformWinRT SQLITE_PLATFORM;



        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            // Now create the tables and such required for the dbase
            SQLITE_PLATFORM = new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT();
            if (!CheckFileExists("db_v1.sqlite").Result)
            {
                using (var db = new SQLiteConnection(SQLITE_PLATFORM, DB_PATH))
                {
                    db.CreateTable<Trip>();
                    db.CreateTable<Car>();
                }
            }
        }

        // More database shiz. Now to check whether the dbase file already exists or not.
        private async Task<bool> CheckFileExists(string fileName)
        {
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
                //await Windows.Storage.ApplicationData.Current.ClearAsync();
            }
            catch
            {
            }
            return false;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter

                    // Check if there was a car selected from a previous session. If true, automatically go to that car-page
                    //int checkSelectedCar = 0;
                    //SettingsHelper settingsHelper = new SettingsHelper();
                    //checkSelectedCar = settingsHelper.GetSelectedCarId();

                    //if (checkSelectedCar != 0)
                    //{
                    //    rootFrame.Navigate(typeof(View.CarSummaryPage));
                    //    //TODO: Add Garage-page to the back-stack
                    //}
                    //else
                    //{
                    //    rootFrame.Navigate(typeof(View.GaragePage), e.Arguments);
                    //}
                    rootFrame.Navigate(typeof(View.GaragePage), e.Arguments);

                }
                // Ensure the current window is active
                Window.Current.Activate();
                this.ShowStatusBar();
            }
        }


        // Show the StatusBar
        private async void ShowStatusBar()
        {
            // Show StatusBar on Win10 Mobile, in theme of the pass
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusBar.BackgroundOpacity = 1;
                await statusBar.ShowAsync();
                // use Accentcolour in statusbar
                var accentColour = Application.Current.Resources["SystemControlHighlightAccentBrush"] as SolidColorBrush;
                statusBar.BackgroundColor = accentColour.Color;
                statusBar.ForegroundColor = new Windows.UI.Color() { R = 255, G = 255, B = 255 };

                // Example colour
                //statusBar.BackgroundColor = new Windows.UI.Color() { R = 81, G = 146, B = 66 };
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
