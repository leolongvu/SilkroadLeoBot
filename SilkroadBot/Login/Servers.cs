using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoBot
{
    class Servers
    {
        public static void GatewayRespond(Packet packet)
        {
            string name = null;
            byte OperationFlag = packet.ReadUInt8();
            while (OperationFlag == 1)
            {
                packet.ReadUInt8();
                packet.ReadAscii();
                OperationFlag = packet.ReadUInt8();
            }
            Data.ServerName.Clear();
            Data.ServerID.Clear();
            byte server = packet.ReadUInt8();
            while (server == 1)
            {
                ushort serverid = packet.ReadUInt16();
                name = packet.ReadAscii();
                ushort currentusers = packet.ReadUInt16();
                ushort capacity = packet.ReadUInt16();
                byte serverstatus = packet.ReadUInt8();
                packet.ReadUInt8();
                server = packet.ReadUInt8();
                Data.ServerName.Add(name);
                Data.ServerID.Add(serverid);               
            }
            Globals.MainWindow.Updateserverlist();
            if (Globals.MainWindow.Checked(Globals.MainWindow.autologin) == true)
            {
                Packet NewPacket = new Packet((ushort)LoginServerOpcodes.CLIENT_OPCODES.LOGIN, true);
                NewPacket.WriteUInt8(22);
                NewPacket.WriteAscii(Globals.MainWindow.ReadText(Globals.MainWindow.username));
                NewPacket.WriteAscii(Globals.MainWindow.ReadText(Globals.MainWindow.password));
                NewPacket.WriteUInt16(Data.ServerID[Data.ServerName.IndexOf(Globals.MainWindow.ReadText1(Globals.MainWindow.serverlist))]);
                System.Threading.Thread.Sleep(300);
                Proxy.gw_remote_security.Send(NewPacket);
            }
        }

        public static void AgentRespond(Packet packet)
        {
            byte check = packet.ReadUInt8();
            if (check == 2)
            {
                byte result = packet.ReadUInt8();
                switch (result)
                {
                    case 1:
                        Globals.MainWindow.UpdateLogs("Failed to connect to server. (C9)");
                        break;
                    case 2:
                        Globals.MainWindow.UpdateLogs("Failed to connect to server. (C10)");
                        break;
                    case 3:
                        Globals.MainWindow.UpdateLogs("Failed to connect to server. (C10)");
                        break;
                    case 4:
                        Globals.MainWindow.UpdateLogs("Server is full! Disconnected! Please restart the bot!");
                        break;
                    case 5:
                        Globals.MainWindow.UpdateLogs("Failed to connect to server because access to current IP has exceeded its limit.");
                        break;
                }
            }
            else if (check == 1)
            {
                Globals.MainWindow.UnEnable(Globals.MainWindow.capans);
                Globals.MainWindow.UnEnable(Globals.MainWindow.sendcapans);
            }
        }
    }
}
