using SoT_Spoofer.Wrappers;
using System.Diagnostics;
using System.Security.Principal;

namespace SoT_Spoofer
{
    internal class Load
    {
        public static void Main()
        {
            Console.Title = "Advanced Sea of Thieves Cleaner";

            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
            {

                ProcessStartInfo startInfo = new()
                {
                    FileName = Environment.GetCommandLineArgs()[0],
                    UseShellExecute = true,
                    Verb = "runas",
                    Arguments = "/runas"
                };

                Process.Start(startInfo);
                return;
            }

            Logger.LogWarning("Press Enter to start cleaning");
            Console.ReadLine();

            Logger.LogImportant("Blocking Analytics...");
            HostBlocker.ApplyBlock();

            Logger.LogImportant("Cleaning traces...");
            TraceCleaner.ApplyCleaner();

            Logger.LogSuccess("Spoof done, press Enter to exit");
            Console.ReadLine();
        }
    }
}
