using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace ADC
{
    public class Program
    {
        public static void Main()
        {
            float Voltage = 0;
            SecretLabs.NETMF.Hardware.AnalogInput ADC1 = new SecretLabs.NETMF.Hardware.AnalogInput(Pins.GPIO_PIN_A0);
            while(true)
            {
                Voltage = (float)(3.3 * ADC1.Read() / 1024);
                Debug.Print("voltage is "+Voltage.ToString()+"V");
                Thread.Sleep(100);
            
            }// write your code here


        }

    }
}
