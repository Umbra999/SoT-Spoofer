using Microsoft.Win32;
using SoT_Spoofer.Wrappers;
using System.Net;
using System.Runtime.InteropServices;

namespace SoT_Spoofer
{
    internal class TraceCleaner
    {
        public static void ApplyCleaner()
        {
            DeleteKey(@"HKEY_CURRENT_USER\Software\Microsoft\IdentityCRL");
            DeleteKey(@"HKEY_USERS\.DEFAULT\Software\Microsoft\IdentityCRL");
            DeleteKey(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\XboxLive");
            DeleteKey(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Xbox");

            DeleteCredentialsStartingWith("Xbl");
            DeleteCredentialsStartingWith("XboxLive");

            SpoofProfileGUID();
            SpoofMachineID();
            SpoofMachineGUID();
        }

        public static void DeleteKey(string path)
        {
            string Type = path.Split('\\')[0];
            string SubPath = path.Replace(Type + "\\", "");

            switch (Type)
            {
                case "HKEY_CURRENT_USER":
                    DeletCurrentUserKey(SubPath);
                    break;

                case "HKEY_LOCAL_MACHINE":
                    DeletLocalMachineKey(SubPath);
                    break;

                case "HKEY_USERS":
                    DelettUsersKey(SubPath);
                    break;
            }
        }

        private static void DeletCurrentUserKey(string subpath)
        {
            if (Registry.CurrentUser.OpenSubKey(subpath) == null) return;

            Registry.CurrentUser.DeleteSubKeyTree(subpath);
        }

        private static void DeletLocalMachineKey(string subpath)
        {
            if (Registry.LocalMachine.OpenSubKey(subpath) == null) return;

            Registry.LocalMachine.DeleteSubKeyTree(subpath);
        }

        private static void DelettUsersKey(string subpath)
        {
            if (Registry.Users.OpenSubKey(subpath) == null) return;

            Registry.Users.DeleteSubKeyTree(subpath);
        }

        private static void SpoofProfileGUID()
        {
            RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\IDConfigDB\\Hardware Profiles\\0001", true);
            registryKey.SetValue("HwProfileGUID", "{" + Guid.NewGuid().ToString() + "}");
        }

        private static void SpoofMachineID()
        {
            RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\SQMClient", true);
            registryKey.SetValue("MachineId", "{" + Guid.NewGuid().ToString().ToUpper() + "}");
        }

        private static void SpoofMachineGUID()
        {
            RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\Cryptography", true);
            registryKey.SetValue("MachineGuid", Guid.NewGuid().ToString());
        }

        public static void DeleteCredentialsStartingWith(string prefix)
        {
            IntPtr pCredentials = IntPtr.Zero;
            int count = 0;

            if (NativeMethods.CredEnumerate(null, 0, out count, out pCredentials))
            {
                try
                {
                    for (int i = 0; i < count; i++)
                    {
                        IntPtr credentialPtr = Marshal.ReadIntPtr(pCredentials, i * Marshal.SizeOf(typeof(IntPtr)));
                        NativeMethods.CREDENTIAL credential = (NativeMethods.CREDENTIAL)Marshal.PtrToStructure(credentialPtr, typeof(NativeMethods.CREDENTIAL));

                        if (credential.TargetName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                        {
                            Logger.Log($"Deleting credential: {credential.TargetName}");

                            bool success = NativeMethods.CredDelete(credential.TargetName, credential.Type, 0);
                            if (!success)
                            {
                                Logger.LogError($"Failed to delete credential: {credential.TargetName}");
                            }
                        }
                    }
                }
                finally
                {
                    NativeMethods.CredFree(pCredentials);
                }
            }
            else
            {
                Console.WriteLine("Failed to enumerate credentials.");
            }
        }
    }
}
