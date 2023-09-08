using System.Diagnostics;

namespace SoT_Spoofer.Wrappers
{
    internal class Utils
    {
        public static void RunAsProcess(string Code)
        {
            Process process = Process.Start(new ProcessStartInfo("cmd.exe", "/c " + Code)
            {
                CreateNoWindow = true,
                UseShellExecute = false
            });
            process.WaitForExit();
            process.Close();
        }
    }
}
