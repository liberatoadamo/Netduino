using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace LCM1602_I2C
{
    public class LCD1602
    {
        private I2CDevice LCD_I2C;
        private bool BackLight;
        private I2CDevice.Configuration LCD_I2C_Config;
        private int LCD_I2C_Bus_Timeout;
        private byte LCD_I2C_Address;
        private int LCD_I2C_Speed;
        /*Below Function Provide About 70us Delay*/
        private void DelayUs()
        {
            int DelayCount = 1;
            for (; DelayCount >= 0; DelayCount--) ;
        }
        private int I2C_WriteByte(byte Data)
        {
            I2CDevice.I2CTransaction[] Transaction = new I2CDevice.I2CTransaction[] 
            {
                I2CDevice.CreateWriteTransaction(new byte[1]{Data}),
            };
            return LCD_I2C.Execute(Transaction, LCD_I2C_Bus_Timeout);
        }

        private void WriteCommand(byte Command)
        {
            byte Temp;
            DelayUs();
            //Thread.Sleep(1);
            if (BackLight)
            {
                Temp = (byte)((Command & 0xF0) | 0x04 | 0x08);//Send High 4bit，RS=0,RW=0,EN=1,BKL=1,
            }
            else
            {
                Temp = (byte)((Command & 0xF0) | 0x04);//Send High 4bit，RS=0,RW=0,EN=1,BKL=1,
            }
            I2C_WriteByte(Temp);
            DelayUs();
            Temp &= 0xFB;//Pull Low EN To Trigger Data;
            I2C_WriteByte(Temp);
            DelayUs();

            if (BackLight)
            {
                Temp = (byte)((Command << 4) | 0x04|0x08);//Send High 4bit，EN=0,RW=0,RS=0,BKL=1,
            }
            else
            {
                Temp = (byte)((Command << 4)|0x04);//Send High 4bit，EN=0,RW=0,RS=0,BKL=0,
            }

            I2C_WriteByte(Temp);
            DelayUs();

            Temp &= 0xFB;//Pull Low EN To 0 To Trigger Data;
            I2C_WriteByte(Temp);
            DelayUs();
        }
        private void WriteData(byte Data)
        {
            byte Temp;
            DelayUs();

            if (BackLight)
            {
                Temp = (byte)((Data & 0xF0) | 0x01 | 0x04 | 0x08);//Send High 4bit,RS=1,RW=0,EN=1,BKL=1,
            }
            else
            {
                Temp = (byte)((Data & 0xF0) | 0x01 | 0x04);//Send High 4bit,RS=1,RW=0,EN=1,BKL=0,
            }

            I2C_WriteByte(Temp);
            DelayUs();

            Temp &= 0xFB;//Pull Low EN To 0 To Trigger Data;
            I2C_WriteByte(Temp);
            DelayUs();

            if (BackLight)
            {
                Temp = (byte)((Data << 4) | 0x01 | 0x04 | 0x08);//Send Low 4bit,RS=1,RW=0,EN=1,BKL=1,
            }
            else
            {
                Temp = (byte)((Data << 4) | 0x01 | 0x04);//Send Low 4bit,EN=0,RS=1,RW=0,EN=1,BKL=0,
            }
            I2C_WriteByte(Temp);
            DelayUs();
            Temp &= 0xFB;//Pull Low EN To 0 To Trigger Data;
            I2C_WriteByte(Temp);

        }

        private void SetPosition( byte Row,byte Column)
        {
            byte address;
            if (Row == 0)
            {
                address = (byte)(0x80 + Column);
            }
            else
            {
                address = (byte)(0xC0 + Column);
            }
            WriteCommand(address);
            DelayUs();
        }

        public LCD1602(byte Addr, int Speed, int Bus_TimeOut, bool Light)
        {
            LCD_I2C_Address = Addr;
            LCD_I2C_Speed = Speed;
            LCD_I2C_Bus_Timeout = Bus_TimeOut;
            BackLight = Light;
        }
        public LCD1602()
        {
            LCD_I2C_Address = 0x27;
            LCD_I2C_Speed = 100;
            LCD_I2C_Bus_Timeout = 1000;
            BackLight = true;
        }
        public LCD1602(bool BackLightOn)
        {
            LCD_I2C_Address = 0x27;
            LCD_I2C_Speed = 100;
            LCD_I2C_Bus_Timeout = 1000;
            BackLight = BackLightOn;
        }
        public void Init()
        {
            LCD_I2C_Config = new I2CDevice.Configuration(LCD_I2C_Address, LCD_I2C_Speed);
            LCD_I2C = new I2CDevice(LCD_I2C_Config);
            Thread.Sleep(10);

            WriteCommand(0x33);//Init Back to 8 bits mode  befor set to 4 bits mode
            Thread.Sleep(1);

            WriteCommand(0x32);//Init Back to 8 bits mode  befor set to 4 bits mode
            Thread.Sleep(1);

            WriteCommand(0x28);//Config to 4bits mode,2 Row display
            Thread.Sleep(1);


            WriteCommand(0x0C);//display on,cursor off
            Thread.Sleep(1);

            WriteCommand(0x01);//clear display
            Thread.Sleep(1);

        }
        public void WriteChar(byte Row, byte Column, char CharData)
        {
            SetPosition(Row, Column);
            WriteData((byte)CharData);
        }
        public void ClearDisplay()
        {
            WriteCommand(0x01);
            Thread.Sleep(1);
        }
        public void WriteString(byte Row, byte Column, string StringData)
        {
            int i=0;
           
            while (i<StringData.Length)
            {
                WriteChar(Row, Column++, StringData[i++]);
                if(Column==16)
                {
                    Column=0;
                    Row = (byte)(++Row % 2);
                    Thread.Sleep(500);//When Change Row,Insert a time break for user view
                }

            }
            
        }


    }
}
