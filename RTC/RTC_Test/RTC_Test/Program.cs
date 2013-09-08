using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using RTC_DS1307;
namespace RTC_Test
{
    public class Program
    {
        public static void Main()
        {
            SysTime SystemTime = new SysTime();
            SystemTime.Init();
            //SystemTime.SetTime(13, 6, 3, 12, 55, 30, System.DayOfWeek.Monday);
            Utility.SetLocalTime(SystemTime.Now);

            while (true)
            {
                Debug.Print(DateTime.Now.ToString());
                Thread.Sleep(1000);
            }
        }

    }
}
