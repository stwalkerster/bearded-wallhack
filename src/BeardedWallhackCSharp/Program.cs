// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BeardedWallhackCSharp
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// The program.
    /// </summary>
    internal static class Program
    {
        #region Static Fields

        /// <summary>
        /// The app.
        /// </summary>
        public static Form App;

        #endregion

        #region Methods

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            App = new MainForm();
            Application.Run(App);
        }

        #endregion
    }
}