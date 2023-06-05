using System;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;
using AWSFileUploader.Models;

namespace AWSFileUploader.Utilities
{
    public static class SettingsHelper
    {
        private const string CredentialTarget = "YourAppCredentials";

        [DllImport("advapi32.dll", EntryPoint = "CredReadW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CredRead(string target, uint type, int reservedFlag, out IntPtr credentialPtr);

        [DllImport("advapi32.dll", EntryPoint = "CredWriteW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CredWrite(ref CREDENTIAL credential, uint flags);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CredFree(IntPtr credential);

        public static void SaveSettings(Settings settings)
        {
            string json = JsonConvert.SerializeObject(settings);
            byte[] encryptedData = EncryptData(json);
            SaveCredential(CredentialTarget, encryptedData);
        }

        public static Settings LoadSettings()
        {
            if (ReadCredential(CredentialTarget, out byte[] encryptedData))
            {
                string json = DecryptData(encryptedData);
                return JsonConvert.DeserializeObject<Settings>(json);
            }
            return null;
        }

        private static byte[] EncryptData(string data)
        {
            string encryptedString = AesOperation.EncryptString(data);
            return Encoding.UTF8.GetBytes(encryptedString);
        }

        private static string DecryptData(byte[] encryptedData)
        {
            string encryptedString = Encoding.UTF8.GetString(encryptedData);
            return AesOperation.DecryptString(encryptedString);
        }

        private static bool SaveCredential(string target, byte[] credentialData)
        {
            var credential = new CREDENTIAL
            {
                TargetName = Marshal.StringToCoTaskMemUni(target),
                Type = CRED_TYPE.GENERIC,
                CredentialBlobSize = (uint)credentialData.Length,
                CredentialBlob = Marshal.AllocHGlobal(credentialData.Length),
                Persist = CRED_PERSIST.LOCAL_MACHINE
            };

            Marshal.Copy(credentialData, 0, credential.CredentialBlob, credentialData.Length);

            bool result = CredWrite(ref credential, 0);

            Marshal.FreeHGlobal(credential.CredentialBlob);
            Marshal.FreeCoTaskMem(credential.TargetName);

            return result;
        }

        private static bool ReadCredential(string target, out byte[] credentialData)
        {
            bool result = CredRead(target, (uint)CRED_TYPE.GENERIC, 0, out IntPtr credentialPtr);
            if (result)
            {
                var credential = (CREDENTIAL)Marshal.PtrToStructure(credentialPtr, typeof(CREDENTIAL));
                credentialData = new byte[credential.CredentialBlobSize];
                Marshal.Copy(credential.CredentialBlob, credentialData, 0, credentialData.Length);

                CredFree(credentialPtr);
                return true;
            }

            credentialData = null;
            return false;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct CREDENTIAL
    {
        public uint Flags;
        public CRED_TYPE Type;
        public IntPtr TargetName;
        public IntPtr Comment;
        public long LastWritten;
        public uint CredentialBlobSize;
        public IntPtr CredentialBlob;
        public CRED_PERSIST Persist;
        public uint AttributeCount;
        public IntPtr Attributes;
        public IntPtr TargetAlias;
        public IntPtr UserName;
    }

    internal enum CRED_TYPE : uint
    {
        GENERIC = 1
    }

    internal enum CRED_PERSIST : uint
    {
        SESSION = 1,
        LOCAL_MACHINE = 2,
        ENTERPRISE = 3
    }
}
