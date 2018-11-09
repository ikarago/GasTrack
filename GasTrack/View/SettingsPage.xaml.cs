using GasTrack.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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

    public sealed partial class SettingsPage : Page
    {

        ApplicationDataContainer localSettings = null;
        const string distanceCost = "distanceCost";
        ResourceHelper fuck = new ResourceHelper();

        public SettingsPage()
        {
            this.InitializeComponent();
            localSettings = ApplicationData.Current.LocalSettings;
            getVersion();

            // #REMOVED: 06-11-2018: Commented out due to removal of the Store Engagement Framework
            /*if (Microsoft.Services.Store.Engagement.Feedback.IsSupported)
            {
                btnLaunchFeedbackHub.Visibility = Visibility.Visible;
            }*/
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

        // Get version of the app
        private void getVersion()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;
            appVersion.Text += (string.Format(" {0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision));
        }








        // Buttons
        private async void btnLaunchFeedbackHub_Click(object sender, RoutedEventArgs e)
        {
            // #REMOVED: 06-11-2018: Commented out due to removal of the Store Engagement Framework
            //await Microsoft.Services.Store.Engagement.Feedback.LaunchFeedbackAsync();
        }

        private async void btnSendEmail_Click(object sender, RoutedEventArgs e)
        {
            // Launch an URI-link
            var uriMailFeedback = new Uri(@"mailto:ikarago@outlook.com");
            var success = await Windows.System.Launcher.LaunchUriAsync(uriMailFeedback);
        }

        private async void btnStoreReview_Click(object sender, RoutedEventArgs e)
        {
            var uriStoreReview = new Uri(@"ms-windows-store://review/?ProductId=9NBLGGH4S06N");
            var success = await Windows.System.Launcher.LaunchUriAsync(uriStoreReview);
        }

        private async void btnTwitter_Click(object sender, RoutedEventArgs e)
        {
            var uriTwitter = new Uri(@"https://twitter.com/ikaragodev");
            var success = await Windows.System.Launcher.LaunchUriAsync(uriTwitter);
        }

        private async void btnTumblr_Click(object sender, RoutedEventArgs e)
        {
            var uriTumblr = new Uri(@"https://ikarago.tumblr.com/");
            var success = await Windows.System.Launcher.LaunchUriAsync(uriTumblr);
        }

        private async void btnOpenChangelog_Click(object sender, RoutedEventArgs e)
        {
            var uriChangelog = new Uri(@"https://ikarago.tumblr.com/snips-changelog");
            var success = await Windows.System.Launcher.LaunchUriAsync(uriChangelog);
        }





    }
}
