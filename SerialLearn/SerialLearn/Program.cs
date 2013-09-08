using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.IO.Ports;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace SerialLearn
{
    public class Program
    {
        public static void Main()
        {
            SerialPort serial = new SerialPort(SerialPorts.COM1, 115200, Parity.None, 8, StopBits.One);
            serial.Open();

            string Shadow = "Program Start!\r\n";
            byte[] TxBuff;
            TxBuff = System.Text.Encoding.UTF8.GetBytes(Shadow);
            serial.Write(TxBuff, 0, TxBuff.Length);

            TxBuff = new byte[5];//clear data
            while (true)
            {
                while (serial.BytesToRead>=1)
                {
                    serial.Read(TxBuff, 0, 1);
                    serial.Write(TxBuff, 0, 1);
                }
            }
        }

    }
}
