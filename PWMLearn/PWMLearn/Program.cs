using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace PWMLearn
{
    public class Program
    {
        public static void Main()
        {
            // write your code here

            PWM LED1 = new PWM(PWMChannels.PWM_ONBOARD_LED, 10000, 0.5, false);
            bool upside = true;
            LED1.Frequency = 10000;
            LED1.DutyCycle = 1;
            LED1.Start();
            while (true)
            {
               
                if (upside == true)
                {
                    Debug.Print("Plus");
                    LED1.DutyCycle+=0.01;
                    Debug.Print(LED1.DutyCycle.ToString());
                    if (LED1.DutyCycle >= 0.90)
                    {
                        upside = false;
                    }
                }
                else
                {   
                    Debug.Print("Minus");
                    LED1.DutyCycle-=0.01;
                    Debug.Print(LED1.DutyCycle.ToString());
                    if (LED1.DutyCycle <= 0.10)
                    {
                        upside = true;
                    }
                }

                Thread.Sleep(10);
            }

        }

    }
}
