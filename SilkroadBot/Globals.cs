using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Threading;
using System.Text.RegularExpressions;

namespace LeoBot
{
    class Globals
    {       

        #region Form Invoke

        public static MainWindow MainWindow;
        public static Images Images = new Images();

        #endregion

        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            string a = arr[6].ToString() + arr[7].ToString() + arr[4].ToString() + arr[5].ToString() + "0000";
            return a;
        }

        public static UInt32 String_To_UInt32(string s)
        {
            char[] arr = s.ToCharArray();
            string a = "0000" + arr[2] + arr[3] + arr[0] + arr[1];
            UInt32 aa = UInt32.Parse(a, System.Globalization.NumberStyles.HexNumber);
            return aa;
        }

        public static int CalculatePositionX(ushort xSector, float X)
        {
            return (int)((xSector - 135) * 192 + X / 10);
        }
        public static int CalculatePositionY(ushort ySector, float Y)
        {
            return (int)((ySector - 92) * 192 + Y / 10);
        }

        public static string StringToHex(String name)
        {
            char[] charValues = name.ToCharArray();
            string hexOutput = "";
            foreach (char _eachChar in charValues)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(_eachChar);
                // Convert the decimal value to a hexadecimal value in string form.
                hexOutput += String.Format("{0:X}", value);
            }
            return hexOutput;
        }

        public static byte[] StringToByteArray(String hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
