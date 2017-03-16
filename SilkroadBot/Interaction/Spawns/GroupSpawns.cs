using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeoBot
{
    class GroupSpawns
    {
        public static Packet GroupeSpawnPacket = new Packet((ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_GROUPESPAWN);

        #region Ready New Packet
        public static void GroupeSpawnB(Packet packet)
        {            
            GroupeSpawnPacket = new Packet((ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_GROUPESPAWN);
            if (packet.ReadUInt8() == 1)
            {
                Data.groupespawncount = (int)packet.ReadUInt16();
                Data.groupespawninfo = 1;
            }
            else
            {
                Data.groupespawncount = (int)packet.ReadUInt16();
                Data.groupespawninfo = 2;
            }
        }
        #endregion

        #region Create Packet
        public static void Manager(Packet packet)
        {
            for (int i = 0; i < packet.GetBytes().Length; i++)
            {
                GroupeSpawnPacket.WriteUInt8(packet.ReadUInt8());
            }           
        }
        #endregion

        #region Parse Created Packet
        public static void GroupSpawned()
        {
            GroupeSpawnPacket.Lock();
            Spawn.GroupSpawns(GroupeSpawnPacket);
        }
        #endregion
    }
}