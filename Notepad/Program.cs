using Notepad.Controls;
using Notepad.Properties;

namespace Notepad
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
            AppDomain.CurrentDomain.UnhandledException += (_, e) => new CrashWindow(Resources.Notification_AppCrash, e.ExceptionObject).ShowDialog();

            if (new LoginForm().ShowDialog() is not DialogResult.OK)
            {
                Application.Exit();

                return;
            }

            Application.Run(new MainForm());
        }
    }
}