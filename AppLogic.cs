using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fot
{
    public class AppLogic
    {
        public static int RemainingAttempts { get; set; } = 4;

        public static string GenerateHexStr()
        {
            Random random = new Random();
            string hexChars = "0123456789ABCDEF";
            char[] hexCode = new char[4];

            for (int i = 0; i < 4; i++)
            {
                hexCode[i] = hexChars[random.Next(hexChars.Length)];
            }

            return "0x" + new string(hexCode);
        }
    }
}
