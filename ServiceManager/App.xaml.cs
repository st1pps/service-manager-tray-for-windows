using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Serilog;
using SingleInstanceApp;

namespace ServiceManager
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : ISingleInstance
    {
        private const string UniqueKey = "F65F2391-7AA7-42DE-A415-229DA8C1E7D3";
        
        [STAThread]
        public static void Main(string[] args)
        {
            if (!SingleInstance<App>.InitializeAsFirstInstance(UniqueKey))
            {
                return;
            }

            var application = new App();
            application.InitializeComponent();
            application.Run();
            SingleInstance<App>.Cleanup();
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            throw new NotImplementedException();
        }
    }
}
