using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoBot
{
    class Berserk
    {
        public static void UseZerk()
        {
            Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_ZERK);
            NewPacket.WriteUInt8(0x01);
            Proxy.ag_remote_security.Send(NewPacket);
        }

        public static void CheckBerserk(uint id, string mobtype)
        {
            if (Character.BerserkLevel == 5)
            {
                foreach (string eachline in Globals.MainWindow.berserk.Items)
                {
                    string[] type = eachline.Split(':');
                    if (mobtype == type[0])
                    {
                        if (type[1] == " Yes")
                        {
                            Berserk.UseZerk();
                        }
                        break;
                    }
                }
            }                     
        }

        public static string GetTypeName(byte type)
        {
            if (type == 0)
                return "Normal";
            else if (type == 1)
                return "Champion";
            else if (type == 2)
                return "Unique";
            else if (type == 4)
                return "Giant";
            else if (type == 5)
                return "Titan";
            else if ((type == 6) || (type == 7))
                return "Elite";
            else if (type == 16)
                return "Party";
            else if (type == 17)
                return "Party Champion";
            else if (type == 20)
                return "Party Giant";
            else return "";
        }
    }
}
