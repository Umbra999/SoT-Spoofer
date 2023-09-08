using System.Runtime.InteropServices;

namespace SoT_Spoofer.Wrappers
{
    internal class NativeMethods
    {
        [DllImport("Advapi32.dll", SetLastError = true, EntryPoint = "CredDeleteW", CharSet = CharSet.Unicode)]
        public static extern bool CredDelete(string targetName, CRED_TYPE type, int flags);

        public enum CRED_TYPE
        {
            GENERIC = 1,
        }
    }
}
