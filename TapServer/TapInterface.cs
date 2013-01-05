using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace TapServer
{
    class TapInterface
    {
        private const uint METHOD_BUFFERED = 0;
        private const uint FILE_ANY_ACCESS = 0;
        private const uint FILE_DEVICE_UNKNOWN = 0x00000022;

        static FileStream Tap;
        static EventWaitHandle WaitObject, WaitObject2;
        static int BytesRead;

        [DllImport("Kernel32.dll", /* ExactSpelling = true, */ SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr CreateFile(
            string filename,
            [MarshalAs(UnmanagedType.U4)]FileAccess fileaccess,
            [MarshalAs(UnmanagedType.U4)]FileShare fileshare,
            int securityattributes,
            [MarshalAs(UnmanagedType.U4)]FileMode creationdisposition,
            int flags,
            IntPtr template);
        const int FILE_ATTRIBUTE_SYSTEM = 0x4;
        const int FILE_FLAG_OVERLAPPED = 0x40000000;

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode,
            IntPtr lpInBuffer, uint nInBufferSize,
            IntPtr lpOutBuffer, uint nOutBufferSize,
            out int lpBytesReturned, IntPtr lpOverlapped);

        const string UsermodeDeviceSpace = "\\\\.\\Global\\";
        IntPtr ptr;
        int len;
        IntPtr pstatus;
        IntPtr ptun;
        byte[] buf;
        AsyncCallback readCallback;
        AsyncCallback writeCallback;
        object state;
        object state2;
        IAsyncResult res, res2;
        public string devGuid;

        private Thread _readThread;

        public delegate void NewReadHandler(byte[] packet);
        public event NewReadHandler NewRead;

        private Queue<byte[]> writeQueue = new Queue<byte[]>();

        public TapInterface(int localIP, int networkIP, int netmaskIP)
        {
            devGuid = GetDeviceGuid();
            ptr = CreateFile(UsermodeDeviceSpace + devGuid + ".tap", FileAccess.ReadWrite, FileShare.ReadWrite, 0, FileMode.Open, FILE_ATTRIBUTE_SYSTEM | FILE_FLAG_OVERLAPPED, IntPtr.Zero);
            pstatus = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(pstatus, 1);
            DeviceIoControl(ptr, TAP_CONTROL_CODE(6, METHOD_BUFFERED) /* TAP_IOCTL_SET_MEDIA_STATUS */, pstatus, 4, pstatus, 4, out len, IntPtr.Zero);
            ptun = Marshal.AllocHGlobal(12);
            Marshal.WriteInt32(ptun, 0, localIP);
            Marshal.WriteInt32(ptun, 4, networkIP);
            Marshal.WriteInt32(ptun, 8, unchecked((int)(netmaskIP)));
            //disable this line to enable TAP mode
            DeviceIoControl(ptr, TAP_CONTROL_CODE(10, METHOD_BUFFERED) /* TAP_IOCTL_CONFIG_TUN */, ptun, 12, ptun, 12, out len, IntPtr.Zero);
            Tap = new FileStream(ptr, FileAccess.ReadWrite, true, 10000, true);
            buf = new byte[10000];
            state = new int();
            WaitObject = new EventWaitHandle(false, EventResetMode.AutoReset);
            state2 = new int();
            WaitObject2 = new EventWaitHandle(false, EventResetMode.AutoReset);
            readCallback = new AsyncCallback(ReadDataCallback);
            writeCallback = new AsyncCallback(WriteDataCallback);
            _readThread = new Thread(new ThreadStart(ReadThread));
            _readThread.Start();
        }



        public void ReadThread()
        {
            while (true)
            {
                res = Tap.BeginRead(buf, 0, 10000, readCallback, state);
                WaitObject.WaitOne();
                if (NewRead != null)
                    NewRead(buf);
            }
        }

        public void WriteThread()
        {
            byte[] toWrite;
            while (true)
            {
                if (writeQueue.Count != 0)
                {
                    toWrite = writeQueue.Dequeue();
                    res2 = Tap.BeginWrite(toWrite, 0, BytesRead, writeCallback, state2);
                    WaitObject2.WaitOne();
                }
            }
        }

        public static void WriteDataCallback(IAsyncResult asyncResult)
        {
            Tap.EndWrite(asyncResult);
            WaitObject2.Set();
        }

        public static void ReadDataCallback(IAsyncResult asyncResult)
        {
            BytesRead = Tap.EndRead(asyncResult);
            WaitObject.Set();
        }

        public void QueueWrite(byte[] packet)
        {
            writeQueue.Enqueue(packet);
        }


        private static uint CTL_CODE(uint DeviceType, uint Function, uint Method, uint Access)
        {
            return ((DeviceType << 16) | (Access << 14) | (Function << 2) | Method);
        }

        static uint TAP_CONTROL_CODE(uint request, uint method)
        {
            return CTL_CODE(FILE_DEVICE_UNKNOWN, request, method, FILE_ANY_ACCESS);
        }

        public string GetDeviceGuid()
        {
            const string AdapterKey = "SYSTEM\\CurrentControlSet\\Control\\Class\\{4D36E972-E325-11CE-BFC1-08002BE10318}";
            RegistryKey regAdapters = Registry.LocalMachine.OpenSubKey(AdapterKey, false);
            string[] keyNames = regAdapters.GetSubKeyNames();
            string devGuid = "";
            foreach (string x in keyNames)
            {
                if (x == "Properties")
                    break;
                RegistryKey regAdapter = regAdapters.OpenSubKey(x, false);
                object id = regAdapter.GetValue("ComponentId");
                if (id != null && id.ToString() == "tapstrong") devGuid = regAdapter.GetValue("NetCfgInstanceId").ToString();
            }
            return devGuid;
        }

        public string HumanName()
        {
            const string ConnectionKey = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}";
            if (devGuid != "")
            {
                RegistryKey regConnection = Registry.LocalMachine.OpenSubKey(ConnectionKey + "\\" + devGuid + "\\Connection", false);
                object id = regConnection.GetValue("Name");
                if (id != null) return id.ToString();
            }

            return "";
        }

    }
}
