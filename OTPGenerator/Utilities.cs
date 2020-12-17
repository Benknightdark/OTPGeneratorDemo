using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace OTPGenerator
{
 public   class Utilities
    {
        public static String Dec2Hex(double value, int maxDecimals)
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

        public static String Base32ToDec(String base32)
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

        public static byte[] HexToByte(string hexString)
        {
            byte[] byteOUT = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i = i + 2)
            {
                byteOUT[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return byteOUT;
        }

        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }

        public static int Hex2Dec(String s)
        {
            return Convert.ToInt32(s, 16);
        }
 public  static String getGACode()
        {
            double totalMilliseconds = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

            double epoch = totalMilliseconds / 1000.0;
            String epochHex = Utilities.Dec2Hex(Math.Floor(epoch / 30), 20).PadLeft(16, '0');
            
            HMACSHA1 hmac = new HMACSHA1();

            String hexKey = Utilities.Base32ToDec("SDFEDS6P&&&&&E46G5HJ2"); //這邊的字串須提供金鑰
            hmac.Key = Utilities.HexToByte(hexKey);

            byte[] resultArray = hmac.ComputeHash(Utilities.HexToByte(epochHex));
            String resultText = Utilities.ByteToString(resultArray);

            int offset = Convert.ToInt32(Utilities.Hex2Dec(resultText.Substring(resultText.Length - 1)));
            
            String otp = (Utilities.Hex2Dec(resultText.Substring(offset * 2, 8)) & Utilities.Hex2Dec("7fffffff")).ToString();
            otp = (otp).Substring(otp.Length - 6, 6);

            return otp;
        }
    }
}
