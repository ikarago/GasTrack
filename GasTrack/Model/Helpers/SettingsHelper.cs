using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace GasTrack.Model.Helpers
{
    public class SettingsHelper
    {
        private ResourceHelper fuck = new ResourceHelper();

        // Settings Handler for easily getting Settings from the AppDataContainer
        ApplicationDataContainer localSettings = null;


        // Constraints for the settings
        const string previousVersionMajor = "previousVersionMajor";
        const string previousVersionMinor = "previousVersionMinor";
        const string previousVersionBuild = "previousVersionBuild";
        const string previousVersionRevision = "previousVersionRevision";

        const string selectedCarId = "selectedCarId";




        // Constructor
        public SettingsHelper()
        {
            localSettings = ApplicationData.Current.LocalSettings;
        }



        // Methods

        public bool UpdateSelectedCarId(int newCarId)
        {
            bool success = false;

            try
            {
                if (newCarId == -1)
                {
                    localSettings.Values[selectedCarId] = null;
                }
                else
                {
                    localSettings.Values[selectedCarId] = newCarId;
                }
                success = true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return success;
        }

        public int GetSelectedCarId()
        {
            int carId = 0;
            try
            {
                carId = Convert.ToInt32(localSettings.Values[selectedCarId]);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return carId;
        }




        public async Task<bool> CheckIfAppIsUpdated()
        {
            bool isAppUpdated = false;
            bool toChangelog = false;


            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;


            // First, check if there was a previous version. If not, just fill in the current version as the previous version.
            if (localSettings.Values[previousVersionMajor] == null)
            {
                localSettings.Values[previousVersionMajor] = version.Major;
                localSettings.Values[previousVersionMinor] = version.Minor;
                localSettings.Values[previousVersionBuild] = version.Build;
                localSettings.Values[previousVersionRevision] = version.Revision;
            }


            // Second, check if the app has been updated by comparing the old agains the new version
            if (localSettings.Values[previousVersionMajor] != null)
            {
                // Get the values for the old version
                ushort oldVerMajor = Convert.ToUInt16(localSettings.Values[previousVersionMajor]);
                ushort oldVerMinor = Convert.ToUInt16(localSettings.Values[previousVersionMinor]);
                ushort oldVerBuild = Convert.ToUInt16(localSettings.Values[previousVersionBuild]);
                ushort oldVerRevision = Convert.ToUInt16(localSettings.Values[previousVersionRevision]);

                if (version.Major > oldVerMajor)    // Check if it's a major version-update
                {
                    isAppUpdated = true;
                    localSettings.Values[previousVersionMajor] = version.Major;
                    localSettings.Values[previousVersionMinor] = version.Minor;
                    localSettings.Values[previousVersionBuild] = version.Build;
                    localSettings.Values[previousVersionRevision] = version.Revision;
                }
                else if (version.Minor > oldVerMinor)   // check if it's an minor version-update
                {
                    isAppUpdated = true;
                    localSettings.Values[previousVersionMajor] = version.Major;
                    localSettings.Values[previousVersionMinor] = version.Minor;
                    localSettings.Values[previousVersionBuild] = version.Build;
                    localSettings.Values[previousVersionRevision] = version.Revision;
                }
                else if (version.Build > oldVerBuild)   // check if the build has been updated
                {
                    isAppUpdated = true;
                    localSettings.Values[previousVersionMajor] = version.Major;
                    localSettings.Values[previousVersionMinor] = version.Minor;
                    localSettings.Values[previousVersionBuild] = version.Build;
                    localSettings.Values[previousVersionRevision] = version.Revision;
                }
                else if (version.Revision > oldVerRevision) // This probably won't be used, but is here anyway; check if the revision has been updated
                {
                    isAppUpdated = true;
                    localSettings.Values[previousVersionMajor] = version.Major;
                    localSettings.Values[previousVersionMinor] = version.Minor;
                    localSettings.Values[previousVersionBuild] = version.Build;
                    localSettings.Values[previousVersionRevision] = version.Revision;
                }
            }

            if (isAppUpdated == true)
            {
                //toChangelog = await fuck.ShowAppUpdatedDialog("Error-AppUpdated-Text", "Error-AppUpdated-Title");
                Debug.WriteLine("toChangelog = " + toChangelog);
            }
            
            return toChangelog;
        }
    }
}
