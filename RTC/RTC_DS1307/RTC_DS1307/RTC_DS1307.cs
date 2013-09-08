using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace RTC_DS1307
{
    public static class BcdConvertor
    {
        public static byte FromBCD(byte value)
        {
            byte Low = (byte)(value & 0x0F);
            byte High = (byte)((value & 0xF0) >> 4);
            byte retValue = (byte)(High * 10 + Low);
            return retValue;
        }
        public static byte ToBCD(byte value)
        {
            return (byte)((value / 10 << 4) + value % 10);
        }
    }

    public class SysTime
    {
        private I2CDevice RTC_I2C;
        private I2CDevice.Configuration RTC_I2C_Config;

        private int RTC_WriteReg(byte addr,byte[] data)
        {
            byte[] CombineBuffer = new byte[57];
            CombineBuffer[0] = addr;
            data.CopyTo(CombineBuffer,1);

            I2CDevice.I2CTransaction[] Transaction = new I2CDevice.I2CTransaction[]
            {
                I2CDevice.CreateWriteTransaction(CombineBuffer)
            };
            return RTC_I2C.Execute(Transaction,1000);
        }

        private int RTC_ReadReg(byte addr, byte[] data)
        {
            I2CDevice.I2CTransaction[] Transaction = new I2CDevice.I2CTransaction[]
            {
                I2CDevice.CreateWriteTransaction(new byte[] {addr}),
                I2CDevice.CreateReadTransaction(data)                  
            };
            return RTC_I2C.Execute(Transaction, 1000);
        }

        public SysTime()
        {
            RTC_I2C_Config = new I2CDevice.Configuration(0x68, 100);
            RTC_I2C = new I2CDevice(RTC_I2C_Config);
        }
        public int Init()
        {
            byte[] REG0= new byte[1];
            RTC_ReadReg(0x00, REG0);
            if ((REG0[0] & 0x80) != 0)
            {
                RTC_WriteReg(0x00, new byte[1]{0x00});
                return 1;
            }
            else
            {
                return 1;
            }
            
        }
        public DateTime Now
        {
            get
            {
                byte[] Data = new byte[7];
                int Success = RTC_ReadReg(0x00, Data);
                DateTime xNow = new DateTime(2000 + BcdConvertor.FromBCD(Data[6]),
                    BcdConvertor.FromBCD(Data[5]),
                    BcdConvertor.FromBCD(Data[4]),
                    BcdConvertor.FromBCD(((byte)(Data[2] & 0x3f))),
                    BcdConvertor.FromBCD(Data[1]),
                    BcdConvertor.FromBCD(((byte)(Data[0] & 0x7f))));
                return xNow;
            }

        }
        public int SetTime(byte Year, byte Month, byte Day, byte Hour, byte Minute, byte Second, DayOfWeek DayofWeek)
        {
            byte[] buffer = new byte[] 
            {
                BcdConvertor.ToBCD( (byte)Second),
                BcdConvertor.ToBCD((byte)Minute),
                BcdConvertor.ToBCD((byte)Hour),
                BcdConvertor.ToBCD(((byte)(DayofWeek +1))),
                BcdConvertor.ToBCD((byte)Day),
                BcdConvertor.ToBCD((byte)Month),
                BcdConvertor.ToBCD((byte)Year)
            };

            return RTC_WriteReg(0x00, buffer);
        }
    }
}
