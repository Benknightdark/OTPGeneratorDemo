using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace OTPGenerator
{
 public   class OTPCodeGeneratorService
    {
         private string key;
        private IConfiguration _config;
        public OTPCodeGeneratorService(IConfiguration config,string KeyParameter)
        {
           
            _config = config;
            key = _config[KeyParameter];
        }
        private  String Dec2Hex(double value, int maxDecimals)
        {
            string result = string.Empty;
            if (value < 0)
            {
                result += "-";
                value = -value;
            }
            if (value > ulong.MaxValue)
            {
                result += double.PositiveInfinity.ToString();
                return result;
            }
            ulong trunc = (ulong)value;
            result += trunc.ToString("X");
            value -= trunc;
            if (value == 0)
            {
                return result;
            }
            result += ".";
            byte hexdigit;
            while ((value != 0) && (maxDecimals != 0))
            {
                value *= 16;
                hexdigit = (byte)value;
                result += hexdigit.ToString("X");
                value -= hexdigit;
                maxDecimals--;
            }
            return result;
        }

        private  String Base32ToDec(String base32)
        {
            String base32chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            String bits = "";
            String hex = "";


            for (var i = 0; i < base32.Length; i++)
            {
                int val = base32chars.IndexOf(base32.Substring(i, 1).ToUpper());

                bits += Convert.ToString(val, 2).PadLeft(5, '0');
            }

            for (var i = 0; i + 4 <= bits.Length; i += 4)
            {
                String chunk = bits.Substring(i, 4);
                hex = hex + Convert.ToString(Convert.ToInt32(chunk, 2), 16);
            }
            return hex;
        }

        private  byte[] HexToByte(string hexString)
        {
            byte[] byteOUT = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i = i + 2)
            {
                byteOUT[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return byteOUT;
        }

        private  string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }

        private  int Hex2Dec(String s)
        {
            return Convert.ToInt32(s, 16);
        }
        public   String getGACode()
        {
            double totalMilliseconds = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

            double epoch = totalMilliseconds ;/// 1000.0
            String epochHex = Dec2Hex(Math.Floor(epoch / 30), 20).PadLeft(16, '0');
            
            HMACSHA1 hmac = new HMACSHA1();

            String hexKey = Base32ToDec(key); //這邊的字串須提供金鑰#"SDFEDS6P&&&&&E46G5HJ2"
            hmac.Key = HexToByte(hexKey);

            byte[] resultArray = hmac.ComputeHash(HexToByte(epochHex));
            String resultText = ByteToString(resultArray);

            int offset = Convert.ToInt32(Hex2Dec(resultText.Substring(resultText.Length - 1)));
            
            String otp = (Hex2Dec(resultText.Substring(offset * 2, 8)) & Hex2Dec("7fffffff")).ToString();
            otp = (otp).Substring(otp.Length - 6, 6);

            return otp;
        }
    }
}
