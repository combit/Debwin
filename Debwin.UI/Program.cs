using combit.DebwinExtensions.MessageTypes;
using Debwin.Core.Controller;
using Debwin.Core.Metadata;
using Debwin.UI.Util;
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Debwin.UI
{
    static class Program
    {

        [STAThread]
        static void Main()
        {
            IList<string> cmdArgs = Environment.GetCommandLineArgs().ToList();

            // Returns true if this is the first Debwin instance, otherwise passes the cmdArgs to an existing instance (set in ShpowGUI)
            if (SingleInstanceHelper.TryInitializeAsFirstInstance("LE_Debwin4", cmdArgs))
            {
                ShowGUI();
            }
            SingleInstanceHelper.Cleanup();
        }


        private static void ShowGUI()
        {
            // use for troubleshooting start up problems
            // AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Try to load config            
            var configFile = GetUserSettingsFile();
            IUserPreferences userPreferences;
            if (File.Exists(configFile))
            {
                userPreferences = UserPreferences.LoadFromFile(configFile);
            }
            else
            {
                userPreferences = new UserPreferences();
            }

            // Load combit-specific extensions (+TODO eventually this should not be hardcoded anymore)
            LogMessageFactory.RegisterFactory(ListLabelLogMessage.TYPECODE_LL_MESSAGE, () => new ListLabelLogMessage());
            LogMessageFactory.RegisterFactory(ReportServerLogMessage.TYPECODE_RS_MESSAGE, () => new ReportServerLogMessage());

            // Show main GUI
            IDebwinController debwinController = new DebwinController();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var mainWindow = new MainWindow(debwinController, userPreferences);
            SingleInstanceHelper.ExistingInstance = mainWindow;
            Application.ThreadException += Application_ThreadException;
            Application.Run(mainWindow);

            // Save config
            (userPreferences as UserPreferences).SaveAsXml(GetUserSettingsFile());
        }

        // see above, this can be used to troubleshoot startup problems
        //private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        //{
        //    Debugger.Launch();
        //}

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            
        }

        private static string GetUserSettingsFile()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "combit", "Debwin4Settings.xml");
        }

        
    }
}
