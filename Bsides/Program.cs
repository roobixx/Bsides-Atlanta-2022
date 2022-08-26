using System;
using System.Diagnostics;
using System.Linq;

namespace Bsides
{
    class Program
    {
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
        }
    }
}
