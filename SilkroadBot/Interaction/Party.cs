using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LeoBot
{
    class Party
    {
        public static List<string> playerName = new List<string>();
        public static List<uint> playerID = new List<uint>();

        public static byte membercount = 1;

        public static void CreateParty()
        {
            Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_CREATEPARTY);
            NewPacket.WriteUInt64(0x0000000000000000);
            NewPacket.WriteUInt8(CheckPartyType());
            NewPacket.WriteUInt8(0);
            NewPacket.WriteUInt8(Convert.ToByte(Globals.MainWindow.ReadText(Globals.MainWindow.minlevel)));
            NewPacket.WriteUInt8(Convert.ToByte(Globals.MainWindow.ReadText(Globals.MainWindow.maxlevel)));
            string name = Globals.MainWindow.ReadText(Globals.MainWindow.partyname).ToString();
            NewPacket.WriteUInt16(name.Length);
            NewPacket.WriteUInt8Array(Globals.StringToByteArray(Globals.StringToHex(name)));        
            Proxy.ag_remote_security.Send(NewPacket);
            Data.hasparty = true;
        }

        public static byte CheckPartyType()
        {
            byte type = 0;
            if (Globals.MainWindow.Checked(Globals.MainWindow.allowinvite) == true)
            {
                if (Globals.MainWindow.Checked(Globals.MainWindow.expdis) == true && Globals.MainWindow.Checked(Globals.MainWindow.itemdis) == true)
                {
                    type = 7;
                }
                if (Globals.MainWindow.Checked(Globals.MainWindow.expdis) == true && (Globals.MainWindow.Checked(Globals.MainWindow.noitemdis) == true))
                {
                    type = 5;
                }
                if (Globals.MainWindow.Checked(Globals.MainWindow.noexpdis) == true && Globals.MainWindow.Checked(Globals.MainWindow.itemdis) == true)
                {
                    type = 6;
                }
                if (Globals.MainWindow.Checked(Globals.MainWindow.noexpdis) == true && Globals.MainWindow.Checked(Globals.MainWindow.noitemdis) == true)
                {
                    type = 4;
                }
            }
            else
            {
                if (Globals.MainWindow.Checked(Globals.MainWindow.expdis) == true && Globals.MainWindow.Checked(Globals.MainWindow.itemdis) == true)
                {
                    type = 3;
                }
                if (Globals.MainWindow.Checked(Globals.MainWindow.expdis) == true && (Globals.MainWindow.Checked(Globals.MainWindow.noitemdis) == true))
                {
                    type = 1;
                }
                if (Globals.MainWindow.Checked(Globals.MainWindow.noexpdis) == true && Globals.MainWindow.Checked(Globals.MainWindow.itemdis) == true)
                {
                    type = 2;
                }
                if (Globals.MainWindow.Checked(Globals.MainWindow.noexpdis) == true && Globals.MainWindow.Checked(Globals.MainWindow.noitemdis) == true)
                {
                    type = 0;
                }
            }
            return (type);
        }        

        public static void ReformParty(Packet packet)
        {          
            byte check = packet.ReadUInt8();
            if (check == 1)
            {
                if (Globals.MainWindow.Checked(Globals.MainWindow.autoparty) == true)
                {
                    Party.CreateParty();
                }                
            }
            else if ((check == 3) && (membercount <= 4))
            {
                Party.CreateParty();
                membercount--;
            }
        }

        public static void AcceptParty(Packet packet)
        {
            if (Globals.MainWindow.Checked(Globals.MainWindow.allowparty) == true)
            {
                byte check = packet.ReadUInt8();
                byte check1 = packet.ReadUInt8();
                byte check2 = packet.ReadUInt8();
                uint ID = packet.ReadUInt32();
                byte tag = packet.ReadUInt8();
                Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_ACCEPTPARTYREQUEST);
                NewPacket.WriteUInt8(check);
                NewPacket.WriteUInt8(check1);
                NewPacket.WriteUInt8(check2);
                NewPacket.WriteUInt32(ID);
                NewPacket.WriteUInt8(tag);
                NewPacket.WriteUInt8(1);
                Proxy.ag_remote_security.Send(NewPacket);
                membercount++;
            }           
        }
    }
}
