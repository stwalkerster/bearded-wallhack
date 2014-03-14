// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BeebMaze.Properties
{
    using System.ComponentModel;
    using System.Configuration;

    // This class allows you to handle specific events on the settings class:
    // The SettingChanging event is raised before a setting's value is changed.
    // The PropertyChanged event is raised after a setting's value is changed.
    // The SettingsLoaded event is raised after the setting values are loaded.
    // The SettingsSaving event is raised before the setting values are saved.
    /// <summary>
    /// The settings.
    /// </summary>
    internal sealed partial class Settings
    {
        #region Methods

        /// <summary>
        /// The setting changing event handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
        {
            // Add code to handle the SettingChangingEvent event here.
        }

        /// <summary>
        /// The settings saving event handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
        {
            // Add code to handle the SettingsSaving event here.
        }

        #endregion
    }
}