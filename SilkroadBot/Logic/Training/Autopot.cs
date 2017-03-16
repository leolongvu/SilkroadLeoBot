using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeoBot
{
    class Autopot
    {
        public static void UseHP()
        {
            if (!Data.dead)
            {
                for (int i = 0; i < Data.inventoryid.Count; i++)
                {
                    string type = Data.inventorytype[i];
                    if (type.StartsWith("ITEM_ETC_HP_POTION"))
                    {
                        uint slot = Data.inventoryslot[i];
                        Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYUSE, true);
                        NewPacket.WriteUInt8((byte)slot);
                        NewPacket.WriteUInt16(0x08EC);
                        Proxy.ag_remote_security.Send(NewPacket);
                        break;
                    }
                }
            }
        }

        public static void UseSHP()
        {
            if (!Data.dead)
            {
                for (int i = 0; i < Data.inventoryid.Count; i++)
                {
                    string type = Data.inventorytype[i];
                    if (type.StartsWith("ITEM_ETC_HP_SPOTION"))
                    {
                        uint slot = Data.inventoryslot[i];
                        Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYUSE, true);
                        NewPacket.WriteUInt8((byte)slot);
                        NewPacket.WriteUInt16(0x08EC);
                        Proxy.ag_remote_security.Send(NewPacket);
                        break;
                    }
                }
            }
        }

        public static void UseHGP()
        {
            if (Data.char_attackpetid != 0)
            {
                for (int i = 0; i < Data.inventoryid.Count; i++)
                {
                    string type = Data.inventorytype[i];
                    if (type.StartsWith("ITEM_COS_P_HGP_POTION"))
                    {
                        uint slot = Data.inventoryslot[i];
                        Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYUSE, true);
                        NewPacket.WriteUInt8((byte)slot);
                        NewPacket.WriteUInt16(0x48EC);
                        NewPacket.WriteUInt32(Data.char_attackpetid);
                        Proxy.ag_remote_security.Send(NewPacket);
                        break;
                    }
                }
            }
        }

        public static void UsePetHP(uint id)
        {
            if (Data.char_horseid != 0 || Data.char_attackpetid != 0)
            {
                for (int i = 0; i < Data.inventoryid.Count; i++)
                {
                    string type = Data.inventorytype[i];
                    if (type.StartsWith("ITEM_ETC_COS_HP_POTION"))
                    {
                        uint slot = Data.inventoryslot[i];
                        Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYUSE, true);
                        NewPacket.WriteUInt8((byte)slot);
                        NewPacket.WriteUInt16(0x20EC);
                        NewPacket.WriteUInt32(id);
                        Proxy.ag_remote_security.Send(NewPacket);
                        break;
                    }
                }
            }
        }

        public static void UsePetUni(uint id)
        {
            if (Data.char_horseid != 0 || Data.char_attackpetid != 0)
            {
                for (int i = 0; i < Data.inventoryid.Count; i++)
                {
                    string type = Data.inventorytype[i];
                    if (type.StartsWith("ITEM_COS_P_CURE_ALL"))
                    {
                        uint slot = Data.inventoryslot[i];
                        Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYUSE, true);
                        NewPacket.WriteUInt8((byte)slot);
                        NewPacket.WriteUInt16(0x396C);
                        NewPacket.WriteUInt32(id);
                        Proxy.ag_remote_security.Send(NewPacket);
                        break;
                    }
                }
            }
        }

        public static void UseUni()
        {
            if (!Data.dead)
            {
                for (int i = 0; i < Data.inventoryid.Count; i++)
                {
                    string type = Data.inventorytype[i];
                    if (type.Contains("ITEM_ETC_CURE_ALL"))
                    {
                        uint slot = Data.inventoryslot[i];
                        Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYUSE, true);
                        NewPacket.WriteUInt8((byte)slot);
                        NewPacket.WriteUInt16(0x316C);
                        Proxy.ag_remote_security.Send(NewPacket);
                        break;
                    }
                }
            }
        }

        public static void UseVigor()
        {
            if (!Data.dead)
            {
                for (int i = 0; i < Data.inventoryid.Count; i++)
                {
                    string type = Data.inventorytype[i];
                    if (type.StartsWith("ITEM_ETC_ALL_POTION") || type.StartsWith("ITEM_ETC_ALL_SPOTION"))
                    {
                        uint slot = Data.inventoryslot[i];
                        Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYUSE, true);
                        NewPacket.WriteUInt8((byte)slot);
                        NewPacket.WriteUInt16(0x18EC);
                        Proxy.ag_remote_security.Send(NewPacket);
                        break;
                    }
                }
            }
        }

        public static void UseMP()
        {
            if (!Data.dead)
            {
                for (int i = 0; i < Data.inventoryid.Count; i++)
                {
                    string type = Data.inventorytype[i];
                    if (type.StartsWith("ITEM_ETC_MP_POTION"))
                    {
                        uint slot = Data.inventoryslot[i];
                        Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYUSE, true);
                        NewPacket.WriteUInt8((byte)slot);
                        NewPacket.WriteUInt16(0x10EC);
                        Proxy.ag_remote_security.Send(NewPacket);
                        break;
                    }
                }
            }
        }

        public static void UseSMP()
        {
            if (!Data.dead)
            {
                for (int i = 0; i < Data.inventoryid.Count; i++)
                {
                    string type = Data.inventorytype[i];
                    if (type.StartsWith("ITEM_ETC_MP_SPOTION"))
                    {
                        uint slot = Data.inventoryslot[i];
                        Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYUSE, true);
                        NewPacket.WriteUInt8((byte)slot);
                        NewPacket.WriteUInt16(0x10EC);
                        Proxy.ag_remote_security.Send(NewPacket);
                        break;
                    }
                }
            }
        }
    }
}