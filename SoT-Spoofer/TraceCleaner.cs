using Microsoft.Win32;
using SoT_Spoofer.Wrappers;

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

            DeleteCredential("Xbl|DeviceKey");
            DeleteCredential("XblGrts|DeviceKey");
            DeleteCredential("XboxLive");
            DeleteCredential("Xbl_Ticket|1717113201|Production|860a62fd4fd198c4");

            SpoofProfileGUID();
            SpoofMachineID();
            SpoofMachineGUID();

            FlushDNS();
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

        private static void DeleteCredential(string Key)
        {
            NativeMethods.CredDelete(Key, NativeMethods.CRED_TYPE.GENERIC, 0);
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

        private static void FlushDNS()
        {
            Utils.RunAsProcess("ipconfig /release");
            Utils.RunAsProcess("ipconfig /flushdns");
            Utils.RunAsProcess("ipconfig /renew");
            Utils.RunAsProcess("ipconfig /flushdns");
            Utils.RunAsProcess("ping localhost -n 3 >nul");
        }
    }
}
