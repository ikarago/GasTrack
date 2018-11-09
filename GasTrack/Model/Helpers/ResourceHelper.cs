using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.Popups;

namespace GasTrack.Model
{
    class ResourceHelper
    {

        /// <summary>
        /// Get Localized strings from the resource-file
        /// </summary>
        /// <param name="resourceString"> // Put in string Resource like this --> "nameOfTheResourceString"</param>
        /// <returns>This function will retrieve and fill in the text in the proper language for the user </returns>
        public string GetString(string resourceString)
        {
            // Connect to the resource-files
            ResourceContext defaultContext = ResourceContext.GetForCurrentView();
            ResourceMap srm = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");

            // Put in string Resource like this --> "nameOfTheResourceString"
            // Get back the localized string
            string text = srm.GetValue(resourceString, defaultContext).ValueAsString;
            return text;
        }


        /// <summary>
        /// Show Error Dialog, with the standard "Close"-option.
        /// </summary>
        /// <param name="message">Insert messages like this --> "nameOfTheResourceString"</param>
        /// <param name="header">Insert header for the message like this --> "nameOfTheResourceString"</param>
        /// <param name="button">Insert messages like this --> "nameOfTheResourceString"
        ///  -- When empty, it will show a default 'Close'-button in the resources (Message-Close).</param>
        public async void ShowErrorDialog(string message, string header = "Message-Error", string button = "Message-Close")
        {
            // Show Error dialog
            var error = new MessageDialog(GetString(message), GetString(header));
            error.Commands.Add(new UICommand(GetString(button), new UICommandInvokedHandler(this.CommandInvokedHandler)));
            error.CancelCommandIndex = 0;   // Extra code to select the Close-option when an user presses on the Escape-button
            await error.ShowAsync();        // Show Dialog
        }
        private void CommandInvokedHandler(IUICommand command)
        {
            // Empty
        }


        /// <summary>
        /// Statusbar messages
        /// </summary>
        /// <param name="message">Insert messages like this --> "nameOfTheResourceString"</param>
        /// <param name="seconds">Optional; fill in the amount of seconds you want to show the message. 
        /// -- When left empty it'll display for 3 seconds.
        /// -- Use -1 to disable automatic hiding of the message!</param>
        public async void ShowStatusBarMessage(string message, int seconds = 0)
        {
            // Method to show messages on the Phones-statusBar
            // If you wish to show a more complex message, see catch statement!
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                await statusBar.ShowAsync();
                try // For normal commands
                {
                    statusBar.ProgressIndicator.Text = GetString(message);
                }
                catch // Kind of a hack; use this to sent a bit more complicated strings, examples: GetString("MP-HC-LoginMessageNeutral") + " " + u.user_firstname + "!")
                {
                    statusBar.ProgressIndicator.Text = message;
                }
                statusBar.ProgressIndicator.ProgressValue = 0;
                await statusBar.ProgressIndicator.ShowAsync();
                if (seconds == -1)
                {
                    // Do nothing, so the user has to manually end the Statusbar message
                }
                else
                {
                    HideStatusBarMessage(seconds);
                }
            }
        }

        /// <summary>
        /// Hide the statusbar
        /// </summary>
        /// <param name="seconds">Optional; fill in the amount of seconds you want to show the message. 
        /// -- When left empty it'll display for 3 seconds.
        /// -- Use -1 to immediately hide the message (for manual override)</param>
        public async void HideStatusBarMessage(int seconds = 0)
        {
            // Hide StatusBar-message
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                // Wait 3 seconds before hideing the message
                if (seconds == 0)
                {
                    await Task.Delay(TimeSpan.FromSeconds(3));
                }
                else if (seconds == -1)
                {
                    // No await, JUST DO IT
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(seconds));
                }
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                await statusBar.ShowAsync();
                await statusBar.ProgressIndicator.HideAsync();
            }
        }
    }
}
