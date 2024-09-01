using System.Runtime.InteropServices;

namespace SoT_Spoofer.Wrappers
{
    internal class NativeMethods
    {
        [DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool CredEnumerate(string filter, int flags, out int count, out IntPtr credentials);

        [DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool CredDelete(string targetName, CRED_TYPE type, int flags);

        [DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern void CredFree(IntPtr buffer);

        public enum CRED_TYPE : int
        {
            CRED_TYPE_GENERIC = 1,
            CRED_TYPE_DOMAIN_PASSWORD,
            CRED_TYPE_DOMAIN_CERTIFICATE,
            CRED_TYPE_DOMAIN_VISIBLE_PASSWORD,
            CRED_TYPE_GENERIC_CERTIFICATE,
            CRED_TYPE_DOMAIN_EXTENDED,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct CREDENTIAL
        {
            public int Flags;
            public CRED_TYPE Type;
            public string TargetName;
            public string Comment;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
            public int CredentialBlobSize;
            public IntPtr CredentialBlob;
            public int Persist;
            public int AttributeCount;
            public IntPtr Attributes;
            public string TargetAlias;
            public string UserName;
        }
    }
}
