using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using LCM1602_I2C;

namespace LCD1602_Application
{
    public class Program
    {
                public static void Main()
        {

            Debug.Print("Program Start");

            LCD1602 LCD = new LCD1602(true);
            LCD.Init();

            while (true)
            {
                Debug.Print("Single Char Display!");

                LCD.WriteChar(1, 0, '1');
                LCD.WriteChar(1, 1, '2');
                LCD.WriteChar(1, 2, '3');
                LCD.WriteChar(1, 3, '4');


                Debug.Print("Short String Display!");
                LCD.WriteString(0, 0, "Hello World!");
                Thread.Sleep(500);

                Debug.Print("Very Long String Display!");
                LCD.WriteString(0, 0, "This is a really long long long long string display");
                Thread.Sleep(1000);

                LCD.ClearDisplay();
            }

        }

    }
}
