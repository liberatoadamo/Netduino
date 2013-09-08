using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace I2CLearn
{
    public class Program
    {
        public static void Main()
        {
            OutputPort Led = new OutputPort(Pins.ONBOARD_LED, false);

            byte[] Addr=new byte[2];
            Addr[0]=0x00;
            Addr[1]=0x01;

            byte[] TxBuff = new byte[4];
            TxBuff[0] = (byte)'1';
            TxBuff[1] = (byte)'2';
            TxBuff[2] = (byte)'3';
            TxBuff[3] = (byte)'4';

            byte[] RxBuff= new byte[4];       

            I2CDevice.Configuration I2C_Configuration = new I2CDevice.Configuration(0x50, 400);
            I2CDevice I2C1 = new I2CDevice(I2C_Configuration);

            I2CDevice.I2CTransaction[] WriteTran = new I2CDevice.I2CTransaction[] 
            {
                I2CDevice.CreateWriteTransaction(Addr),
                I2CDevice.CreateWriteTransaction(TxBuff)
            };

            I2CDevice.I2CTransaction[] ReadTran = new I2CDevice.I2CTransaction[]
            {
                I2CDevice.CreateWriteTransaction(Addr),
                I2CDevice.CreateReadTransaction(RxBuff)
            };

            while(true)
            {
                Led.Write(true);
                I2C1.Execute(WriteTran, 1000);
                Debug.Print("Write Success!");
                Thread.Sleep(200);
                I2C1.Execute(ReadTran, 1000);
                Debug.Print("Read Success!");
                string ReadOut = new string(System.Text.Encoding.UTF8.GetChars(RxBuff));
                Debug.Print("EEPROM CONTENT:"+ReadOut);
                Led.Write(false);
                Thread.Sleep(200);
                
            }

        }

    }
}
