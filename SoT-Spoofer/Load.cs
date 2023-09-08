using SoT_Spoofer.Wrappers;
using System.Diagnostics;
using System.Security.Principal;

namespace SoT_Spoofer
{
    internal class Load
    {
        public static void Main()
        {
            Console.Title = "SoT Spoofer | By Umbra";

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

            Logger.LogWarning("This Tool is made to Spoof Data for SoT");
            Logger.LogWarning("Spoofing is permanent, only use this tool if you really know what you are doing");
            Logger.LogWarning("Press Enter to start spoofing");
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
