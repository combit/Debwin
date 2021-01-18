using Debwin.Core.MessageSources;
using Debwin.Core.Metadata;
using Microsoft.Win32;
using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;

namespace combit.DebwinExtensions.MessageSources
{

    /// <summary>
    /// Extends the regular UDP listener with a shared memory area (via memory-mapped file) that allows LL instances
    /// to detect that UDP logging is active and contains required information like the port and flags.
    /// </summary>
    public class CombitUdpMessageSource : UdpMessageSource  // base class implements IDisposable and calls Stop() on Dispose
    {
        private const string DEBWIN_SHAREDMEM_GLOBALNAME = "Global\\DEBWINSharedData";
        private const string DEBWIN_SHAREDMEM_LOCALNAME = "Local\\DEBWINSharedData";

        private MemoryMappedFile _debwinInfoFile;

        [StructLayout(LayoutKind.Sequential)]
        private struct DebwinInfo40
        {
            public UInt32 Size;
            public UInt32 Version;
            public UInt32 Port;
            public UInt64 Flags;
        }

        private MemoryMappedFile CreateMemoryMappedFile()
        {
            var structSize = Marshal.SizeOf(typeof(DebwinInfo40));

            var debwinInfo = new DebwinInfo40();
            debwinInfo.Size = Convert.ToUInt32(structSize);
            debwinInfo.Version = 40;

            UInt64 flags = UInt64.MaxValue;

            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\combit\Debwin4", false);

                if (key != null)
                    flags = Convert.ToUInt64(key.GetValue("DebugFlags", UInt64.MaxValue));
            }
            catch
            {
            }

            debwinInfo.Flags = flags;
            debwinInfo.Port = Convert.ToUInt32(this.Port);

            MemoryMappedFileSecurity security = new MemoryMappedFileSecurity();
            SecurityIdentifier everyoneSid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            security.AddAccessRule(new AccessRule<MemoryMappedFileRights>(everyoneSid, MemoryMappedFileRights.Read, AccessControlType.Allow));
            security.AddAccessRule(new AccessRule<MemoryMappedFileRights>(WindowsIdentity.GetCurrent().User, MemoryMappedFileRights.ReadWrite, AccessControlType.Allow));

            MemoryMappedFile mf;
            string sharedMemName = (IsElevatedUser() ? DEBWIN_SHAREDMEM_GLOBALNAME : DEBWIN_SHAREDMEM_LOCALNAME);
            try
            {
                mf = MemoryMappedFile.OpenExisting(sharedMemName, MemoryMappedFileRights.ReadWrite);
            }
            catch (FileNotFoundException)
            {
                mf = MemoryMappedFile.CreateOrOpen(sharedMemName, structSize, MemoryMappedFileAccess.ReadWrite, MemoryMappedFileOptions.None, security, System.IO.HandleInheritability.Inheritable);
            }

            using (var accessor = mf.CreateViewAccessor(0, structSize))
            {
                accessor.Write(0, ref debwinInfo);
            }
            return mf;
        }

        private bool IsElevatedUser()
        {
            return WindowsIdentity.GetCurrent().Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);
        }


        public override void Start()
        {
            if (_debwinInfoFile == null)
            {
                _debwinInfoFile = CreateMemoryMappedFile();
            }
            base.Start();
        }

        public override void Stop()
        {
            base.Stop();
            if (_debwinInfoFile != null)
            {
                _debwinInfoFile.Dispose();
                _debwinInfoFile = null;
            }
        }

    }
}
