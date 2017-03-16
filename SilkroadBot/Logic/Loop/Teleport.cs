using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace LeoBot
{
    class Teleport
    {
        public static void Tele(uint id, byte type, uint data)
        {
            Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_TELEPORT);
            NewPacket.WriteUInt32(id);
            NewPacket.WriteUInt8(type);
            NewPacket.WriteUInt32(data);
            Proxy.ag_remote_security.Send(NewPacket);
            //LoopControl.WalkScript();
        }

        public static void ClientTeleport(Packet packet)
        {
            if (Data.loopaction == "record")
            {
                uint id = packet.ReadUInt32();
                uint model = Mobs_Info.mobsidlist[Mobs_Info.mobstypelist.IndexOf(Spawns.NPCType[Spawns.NPCID.IndexOf(id)])];
                byte type = packet.ReadUInt8();
                uint data = packet.ReadUInt32();
                string text = "teleport," + model + "," + type + "," + data;
                //Globals.MainWindow.script_record_box.Items.Add(text);
            }
        }
    }
}