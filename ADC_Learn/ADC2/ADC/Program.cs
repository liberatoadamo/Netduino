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
            AnalogInput ADC1 = new AnalogInput(Cpu.AnalogChannel.ANALOG_0);
            ADC1.Scale = 3.3;
            while(true)
            {
                Voltage = (float)(ADC1.Read());
                Debug.Print("voltage is "+Voltage.ToString("f")+"V");
                Thread.Sleep(100);
            
            }// write your code here


        }

    }
}
