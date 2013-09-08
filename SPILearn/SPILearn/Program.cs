using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

/*Demo using a Winbond W25X16 SPI Flash Chip*/
namespace SPILearn
{
    public class Program
    {
        public static void Main()
        {
            byte[] TxBuff = new byte[4]{0x9F,0xA5,0xA5,0xA5};
            byte[] RxBuff = new byte[4];

            SPI.Configuration SPI_Config = new SPI.Configuration(Pins.GPIO_PIN_D10, false, 0, 1, false, true, 1000,SPI.SPI_module.SPI1);
            //the 6th parameter, clock edge ,only care whether MOSI data is latched on the SPI Clock rising edge(true) or falling edge(falling),not the MISO.

            SPI SPI1 = new SPI(SPI_Config);

            
            while (true)
            {
                SPI1.WriteRead(TxBuff, RxBuff);

                Debug.Print("Manufacture ID:"+RxBuff[1].ToString("X"));
                Debug.Print("Device ID High byte:"+RxBuff[2].ToString("X"));
                Debug.Print("Device ID Low byte:"+RxBuff[3].ToString("X"));

                Thread.Sleep(500);
            }


            // write your code here


        }

    }
}
