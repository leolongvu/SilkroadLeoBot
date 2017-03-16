using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace LeoBot
{
    class StorageControl
    {
        public static void OpenStorage()
        {
            uint id = 0;
            switch (StartLooping.type)
            {
                case "ch":
                    for (int i = 0; i < Spawns.NPCID.Count; i++)
                    {
                        if (Spawns.NPCType[i].Contains("CH_WAREHOUSE"))
                        {
                            id = Spawns.NPCID[i];
                            break;
                        }
                    }
                    break;
                case "wc":
                    for (int i = 0; i < Spawns.NPCID.Count; i++)
                    {
                        if (Spawns.NPCType[i].Contains("WC_WAREHOUSE"))
                        {
                            id = Spawns.NPCID[i];
                            break;
                        }
                    }
                    break;
                case "kt":
                    for (int i = 0; i < Spawns.NPCID.Count; i++)
                    {
                        if (Spawns.NPCType[i].Contains("KT_WAREHOUSE"))
                        {
                            id = Spawns.NPCID[i];
                            break;
                        }
                    }
                    break;
                case "eu":
                    for (int i = 0; i < Spawns.NPCID.Count; i++)
                    {
                        if (Spawns.NPCType[i].Contains("EU_WAREHOUSE"))
                        {
                            id = Spawns.NPCID[i];
                            break;
                        }
                    }
                    break;
                case "ca":
                    for (int i = 0; i < Spawns.NPCID.Count; i++)
                    {
                        if (Spawns.NPCType[i].Contains("CA_WAREHOUSE"))
                        {
                            id = Spawns.NPCID[i];
                            break;
                        }
                    }
                    break;
            }
            
            if (id != 0)
            {
                Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTSELECT);
                NewPacket.WriteUInt32(id);
                Proxy.ag_remote_security.Send(NewPacket);
            }
            else
            {
                Globals.MainWindow.UpdateLogs("Storage Keeper Not In Range!");
            }
        }

        public static void GetStorageItems(uint id)
        {
            Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_GETSTORAGEITEMS);
            NewPacket.WriteUInt32(id);
            NewPacket.WriteUInt8(0x00);
            Proxy.ag_remote_security.Send(NewPacket);
        }

        public static void StorageGold(Packet packet)
        {
            Data.storagegold = packet.ReadUInt64();
            Globals.MainWindow.UpdateLogs("Gold in storage: " + Data.storagegold);
        }

        public static void ParseStorageItems(Packet packet)
        {
            {
                if (packet.ReadUInt8() == 150)
                {
                    Data.storageopened = 1;
                    int items_count = packet.ReadUInt8();
                    for (int i = 0; i < items_count; i++)
                    {
                        byte slot = packet.ReadUInt8();
                        packet.ReadUInt32();
                        uint id = packet.ReadUInt32();
                        string type = Items_Info.itemstypelist[Items_Info.itemsidlist.IndexOf(id)];
                        Data.storageid.Add(id);
                        Data.storageslot.Add(slot);
                        Data.storagetype.Add(type);
                        Data.strmaxdurability.Add(Items_Info.itemsdurabilitylist[Items_Info.itemsidlist.IndexOf(id)]);
                        if (type.StartsWith("ITEM_CH") || type.StartsWith("ITEM_EU") || type.StartsWith("ITEM_MALL_AVATAR") || type.StartsWith("ITEM_ETC_E060529_GOLDDRAGONFLAG") || type.StartsWith("ITEM_EVENT_CH") || type.StartsWith("ITEM_EVENT_EU") || type.StartsWith("ITEM_EVENT_AVATAR_W_NASRUN") || type.StartsWith("ITEM_EVENT_AVATAR_M_NASRUN"))
                        {
                            byte item_plus = packet.ReadUInt8();
                            packet.ReadUInt64();
                            Data.storagedurability.Add(packet.ReadUInt32());
                            byte blueamm = packet.ReadUInt8();
                            for (int a = 0; a < blueamm; a++)
                            {
                                packet.ReadUInt8();
                                packet.ReadUInt16();
                                packet.ReadUInt32();
                                packet.ReadUInt8();
                            }
                            packet.ReadUInt32();
                            Data.storagecount.Add(1);
                        }
                        else if ((type.StartsWith("ITEM_COS") && type.Contains("SILK")) || (type.StartsWith("ITEM_EVENT_COS") && !type.Contains("_C_")))
                        {
                            byte flag = packet.ReadUInt8();
                            if (flag == 2 || flag == 3 || flag == 4)
                            {
                                packet.ReadUInt32(); //Model
                                packet.ReadAscii();
                                packet.ReadUInt8();
                                if (Data.Types.attack_spawn_types.IndexOf(type) == -1)
                                {
                                    packet.ReadUInt32();
                                }
                            }
                            Data.storagecount.Add(1);
                            Data.storagedurability.Add(0);
                        }
                        else if (Data.Types.grabpet_spawn_types.IndexOf(type) != -1 || Data.Types.attack_spawn_types.IndexOf(type) != -1)
                        {
                            byte flag = packet.ReadUInt8();
                            if (flag == 2 || flag == 3 || flag == 4)
                            {
                                packet.ReadUInt32(); //Model
                                packet.ReadAscii();
                                if (Data.Types.attack_spawn_types.IndexOf(type) == -1)
                                {
                                    packet.ReadUInt32();
                                }
                                packet.ReadUInt8();
                            }
                            Data.storagecount.Add(1);
                            Data.storagedurability.Add(0);
                        }
                        else if (type == "ITEM_ETC_TRANS_MONSTER")
                        {
                            packet.ReadUInt32();
                            Data.storagecount.Add(1);
                            Data.storagedurability.Add(0);
                        }
                        else if (type.StartsWith("ITEM_MALL_MAGIC_CUBE"))
                        {
                            packet.ReadUInt32();
                            Data.storagecount.Add(1);
                            Data.storagedurability.Add(0);
                        }
                        else
                        {
                            ushort count = packet.ReadUInt16();
                            if (type.Contains("ITEM_ETC_ARCHEMY_ATTRSTONE") || type.Contains("ITEM_ETC_ARCHEMY_MAGICSTONE"))
                            {
                                packet.ReadUInt8();
                            }
                            Data.storagecount.Add(count);
                            Data.storagedurability.Add(0);
                        }
                    }
                    //Globals.MainWindow.storage_list.Items.Clear();
                    for (int i = 0; i < Data.storageid.Count; i++)
                    {
                        uint id = Data.storageid[i];
                        string name = Items_Info.itemsnamelist[Items_Info.itemsidlist.IndexOf(id)];
                        //Globals.MainWindow.storage_list.Items.Add(name);
                    }
                }
            }
            OpenStorage1();
        }

        public static void OpenStorage1()
        {
            if (Data.loop && Data.bot)
            {
                Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_NPCSELECT);
                NewPacket.WriteUInt32(Spawns.NPCID[Spawns.NPCType.IndexOf(Data.selectednpctype)]);
                NewPacket.WriteUInt32(0x00000004);
                Proxy.ag_remote_security.Send(NewPacket);
            }
        }

        public static void Send(byte slot_inv, byte slot_bnk, uint id)
        {
            Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYMOVEMENT);
            NewPacket.WriteUInt8(0x02);
            NewPacket.WriteUInt8(slot_inv);
            NewPacket.WriteUInt8(slot_bnk);
            NewPacket.WriteUInt32(id);
            Proxy.ag_remote_security.Send(NewPacket);
        }

        public static void StorageManager(uint id)
        {
            if (Data.bot && Data.loop)
            {
                if (Globals.MainWindow.Checked(Globals.MainWindow.nopick) == true)
                {
                    Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_NPCDESELECT);
                    NewPacket.WriteUInt32(Spawns.NPCID[Spawns.NPCType.IndexOf(Data.selectednpctype)]);
                    Proxy.ag_remote_security.Send(NewPacket);
                }
                else if (Globals.MainWindow.Checked(Globals.MainWindow.normalpick) == true)
                {
                    for (byte i = 13; i < Character.InventorySize; i++)
                    {
                        int index = Data.inventoryslot.IndexOf(i);
                        if (index != -1)
                        {
                            string type = Data.inventorytype[index];
                            if (type != null && type.Contains("_A_DEF") == false)
                            {
                                byte slot = 0;
                                for (slot = 0; slot < 150; slot++)
                                {
                                    if (Data.storageslot.IndexOf(slot) == -1)
                                    {
                                        break;
                                    }
                                }
                                if (type.Contains("RARE"))
                                {
                                    if (type != Data.f_wep_name && type != Data.s_wep_name)
                                    {
                                        Send(i, slot, id);
                                        break;
                                    }
                                }
                                if (type.StartsWith("ITEM_CH_SWORD") || type.StartsWith("ITEM_CH_BLADE") || type.StartsWith("ITEM_CH_SPEAR") || type.StartsWith("ITEM_CH_TBLADE") || type.StartsWith("ITEM_CH_BOW") || type.StartsWith("ITEM_CH_SHIELD") || type.StartsWith("ITEM_EU_DAGGER") || type.StartsWith("ITEM_EU_SWORD") || type.StartsWith("ITEM_EU_TSWORD") || type.StartsWith("ITEM_EU_AXE") || type.StartsWith("ITEM_EU_CROSSBOW") || type.StartsWith("ITEM_EU_DARKSTAFF") || type.StartsWith("ITEM_EU_TSTAFF") || type.StartsWith("ITEM_EU_HARP") || type.StartsWith("ITEM_EU_STAFF") || type.StartsWith("ITEM_EU_SHIELD"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.wep_drop) == "Store" && type != Data.f_wep_name && type != Data.s_wep_name)
                                    {
                                        Send(i, slot, id);
                                        break;
                                    }
                                }
                                if (type.StartsWith("ITEM_CH_M_HEAVY") || type.StartsWith("ITEM_CH_M_LIGHT") || type.StartsWith("ITEM_CH_M_CLOTHES") || type.StartsWith("ITEM_CH_W_HEAVY") || type.StartsWith("ITEM_CH_W_LIGHT") || type.StartsWith("ITEM_CH_W_CLOTHES") || type.StartsWith("ITEM_EU_M_HEAVY") || type.StartsWith("ITEM_EU_M_LIGHT") || type.StartsWith("ITEM_EU_M_CLOTHES") || type.StartsWith("ITEM_EU_W_HEAVY") || type.StartsWith("ITEM_EU_W_LIGHT") || type.StartsWith("ITEM_EU_W_CLOTHES"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.armor_drop) == "Store")
                                    {
                                        Send(i, slot, id);
                                        break;
                                    }
                                }
                                if (type.StartsWith("ITEM_EU_RING") || type.StartsWith("ITEM_EU_EARRING") || type.StartsWith("ITEM_EU_NECKLACE") || type.StartsWith("ITEM_CH_RING") || type.StartsWith("ITEM_CH_EARRING") || type.StartsWith("ITEM_CH_NECKLACE"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.acc_drop) == "Store")
                                    {
                                        Send(i, slot, id);
                                        break;
                                    }
                                }
                                if (type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_WEAPON"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.wepe_drop) == "Store")
                                    {
                                        Send(i, slot, id);
                                        break;
                                    }
                                }
                                if (type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_SHIELD"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.shielde_drop) == "Store")
                                    {
                                        Send(i, slot, id);
                                        break;
                                    }
                                }
                                if (type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_ARMOR"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.prote_drop) == "Store")
                                    {
                                        Send(i, slot, id);
                                        break;
                                    }
                                }
                                if (type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_ACCESSARY"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.acce_drop) == "Store")
                                    {
                                        Send(i, slot, id);
                                        break;
                                    }
                                }
                                if (type.StartsWith("ITEM_ETC_ALL_SPOTION_01"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.vigorg_drop) == "Store")
                                    {
                                        Send(i, slot, id);
                                        break;
                                    }
                                }

                                if (type.StartsWith("ITEM_ETC_ALL_POTION"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.vigorp_drop) == "Store")
                                    {
                                        Send(i, slot, id);
                                        break;
                                    }
                                }

                                if (type.StartsWith("ITEM_ETC_SCROLL_RETURN_01") || type.StartsWith("ITEM_ETC_SCROLL_RETURN_02") || type.StartsWith("ITEM_ETC_SCROLL_RETURN_03"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.return_drop) == "Store")
                                    {
                                        Send(i, slot, id);
                                        break;
                                    }
                                }
                                if (type.StartsWith("ITEM_ETC_ARCHEMY_ATTRTABLET") || type.StartsWith("ITEM_ETC_ARCHEMY_MAGICSTONE"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.tablets_drop) == "Store")
                                    {
                                        Send(i, slot, id);
                                        break;
                                    }
                                }
                                if (type.StartsWith("ITEM_ETC_ARCHEMY_MATERIAL"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.materials_drop) == "Store")
                                    {
                                        Send(i, slot, id);
                                        break;
                                    }
                                }
                            }
                        }
                        if (i + 1 >= Character.InventorySize)
                        {
                            Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_NPCDESELECT);
                            NewPacket.WriteUInt32(Spawns.NPCID[Spawns.NPCType.IndexOf(Data.selectednpctype)]);
                            Proxy.ag_remote_security.Send(NewPacket);
                        }
                    }
                }
                else if (Globals.MainWindow.Checked(Globals.MainWindow.advancepick) == true)
                {
                    for (byte i = 13; i < Character.InventorySize; i++)
                    {
                        int index = Data.inventoryslot.IndexOf(i);
                        if (index != -1)
                        {
                            string type = Data.inventorytype[index];
                            if (type != null && type.Contains("_A_DEF") == false)
                            {
                                byte slot = 0;
                                for (slot = 0; slot < 150; slot++)
                                {
                                    if (Data.storageslot.IndexOf(slot) == -1)
                                    {
                                        break;
                                    }
                                }
                                if (CheckStore(type) == true)
                                {
                                    Send(i, slot, id);
                                    break;
                                }
                            }                           
                        }
                        if (i + 1 >= Character.InventorySize)
                        {
                            Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_NPCDESELECT);
                            NewPacket.WriteUInt32(Spawns.NPCID[Spawns.NPCType.IndexOf(Data.selectednpctype)]);
                            Proxy.ag_remote_security.Send(NewPacket);
                        }
                    }
                }
            }
        }

        public static bool CheckStore(string type)
        {
            if (type != null && type.Contains("_A_DEF") == false)
            {
                if (type.StartsWith("ITEM_CH_SWORD") || type.StartsWith("ITEM_CH_BLADE") || type.StartsWith("ITEM_CH_SPEAR") || type.StartsWith("ITEM_CH_TBLADE") || type.StartsWith("ITEM_CH_BOW") || type.StartsWith("ITEM_CH_SHIELD") || type.StartsWith("ITEM_EU_DAGGER") || type.StartsWith("ITEM_EU_SWORD") || type.StartsWith("ITEM_EU_TSWORD") || type.StartsWith("ITEM_EU_AXE") || type.StartsWith("ITEM_EU_CROSSBOW") || type.StartsWith("ITEM_EU_DARKSTAFF") || type.StartsWith("ITEM_EU_TSTAFF") || type.StartsWith("ITEM_EU_HARP") || type.StartsWith("ITEM_EU_STAFF") || type.StartsWith("ITEM_EU_SHIELD"))
                {
                    int a = 0;
                    while ((a < AdvanceItem.Weapon.Count))
                    {
                        if (AdvanceItem.Weapon[a].AdvanceType == type && type != Data.f_wep_name && type != Data.s_wep_name)
                        {
                            if (AdvanceItem.Weapon[a].Action == "Store")
                                return true;
                            break;
                        }
                        a++;
                    }
                }
                else if (type.StartsWith("ITEM_CH_M_HEAVY") || type.StartsWith("ITEM_CH_M_LIGHT") || type.StartsWith("ITEM_CH_M_CLOTHES") || type.StartsWith("ITEM_CH_W_HEAVY") || type.StartsWith("ITEM_CH_W_LIGHT") || type.StartsWith("ITEM_CH_W_CLOTHES") || type.StartsWith("ITEM_EU_M_HEAVY") || type.StartsWith("ITEM_EU_M_LIGHT") || type.StartsWith("ITEM_EU_M_CLOTHES") || type.StartsWith("ITEM_EU_W_HEAVY") || type.StartsWith("ITEM_EU_W_LIGHT") || type.StartsWith("ITEM_EU_W_CLOTHES"))
                {
                    int a = 0;
                    while ((a < AdvanceItem.Armor.Count))
                    {
                        if (AdvanceItem.Armor[a].AdvanceType == type)
                        {
                            if (AdvanceItem.Armor[a].Action == "Store")
                                return true;
                            break;
                        }
                        a++;
                    }
                }
                else if (type.StartsWith("ITEM_EU_RING") || type.StartsWith("ITEM_EU_EARRING") || type.StartsWith("ITEM_EU_NECKLACE") || type.StartsWith("ITEM_CH_RING") || type.StartsWith("ITEM_CH_EARRING") || type.StartsWith("ITEM_CH_NECKLACE"))
                {
                    int a = 0;
                    while ((a < AdvanceItem.Accessorise.Count))
                    {
                        if (AdvanceItem.Accessorise[a].AdvanceType == type)
                        {
                            if (AdvanceItem.Accessorise[a].Action == "Store")
                                return true;
                            break;
                        }
                        a++;
                    }
                }
                else if (type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_WEAPON") || type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_SHIELD") || type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_ARMOR") || type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_ACCESSARY"))
                {
                    int a = 0;
                    while ((a < AdvanceItem.Elixir.Count))
                    {
                        if (AdvanceItem.Elixir[a].AdvanceType == type)
                        {
                            if (AdvanceItem.Elixir[a].Action == "Store")
                                return true;
                            break;
                        }
                        a++;
                    }
                }
                else if (type.StartsWith("ITEM_ETC_AMMO_ARROW") || type.StartsWith("ITEM_ETC_AMMO_BOLT"))
                {
                    int a = 0;
                    while ((a < AdvanceItem.Arrow.Count))
                    {
                        if (AdvanceItem.Arrow[a].AdvanceType == type)
                        {
                            if (AdvanceItem.Arrow[a].Action == "Store")
                                return true;
                            break;
                        }
                        a++;
                    }
                }
                else if (type.StartsWith("ITEM_ETC_SCROLL_RETURN"))
                {
                    int a = 0;
                    while ((a < AdvanceItem.Return.Count))
                    {
                        if (AdvanceItem.Return[a].AdvanceType == type)
                        {
                            if (AdvanceItem.Return[a].Action == "Store")
                                return true;
                            break;
                        }
                        a++;
                    }
                }
                else if (type.StartsWith("ITEM_ETC_CURE_ALL") || type.StartsWith("ITEM_ETC_ALL_SPOTION") || type.StartsWith("ITEM_ETC_ALL_POTION") || type.StartsWith("ITEM_ETC_MP_POTION") || type.StartsWith("ITEM_ETC_HP_POTION"))
                {
                    int a = 0;
                    while ((a < AdvanceItem.Potion.Count))
                    {
                        if (AdvanceItem.Potion[a].AdvanceType == type)
                        {
                            if (AdvanceItem.Potion[a].Action == "Store")
                                return true;
                            break;
                        }
                        a++;
                    }
                }
                else if (type.StartsWith("ITEM_ETC_ARCHEMY_MAGICTABLET") || type.StartsWith("ITEM_ETC_ARCHEMY_ATTRTABLET") || type.Contains("MAGICSTONE"))
                {
                    int a = 0;
                    while ((a < AdvanceItem.Tablet.Count))
                    {
                        if (AdvanceItem.Tablet[a].AdvanceType == type)
                        {
                            if (AdvanceItem.Tablet[a].Action == "Store")
                                return true;
                            break;
                        }
                        a++;
                    }
                }
                else if (type.StartsWith("ITEM_ETC_ARCHEMY_MATERIAL"))
                {
                    int a = 0;
                    while ((a < AdvanceItem.Material.Count))
                    {
                        if (AdvanceItem.Material[a].AdvanceType == type)
                        {
                            if (AdvanceItem.Material[a].Action == "Store")
                                return true;
                            break;
                        }
                        a++;
                    }
                }
                else if (type.Contains("ITEM_MALL") || type.Contains("ITEM_EVENT_CH") || type.Contains("ITEM_EVENT_EU"))
                {
                    int a = 0;
                    while ((a < AdvanceItem.Mall.Count))
                    {
                        if (AdvanceItem.Mall[a].AdvanceType == type)
                        {
                            if (AdvanceItem.Mall[a].Action == "Store")
                                return true;
                            break;
                        }
                        a++;
                    }
                }
                else if (type.Contains("ITEM_QSP") || type.Contains("ITEM_QNO"))
                {
                    int a = 0;
                    while ((a < AdvanceItem.Quest.Count))
                    {
                        if (AdvanceItem.Quest[a].AdvanceType == type)
                        {
                            if (AdvanceItem.Quest[a].Action == "Store")
                                return true;
                            break;
                        }
                        a++;
                    }
                }
                else
                {
                    int a = 0;
                    while ((a < AdvanceItem.Allitems.Count))
                    {
                        if (AdvanceItem.Allitems[a].AdvanceType == type)
                        {
                            if (AdvanceItem.Allitems[a].Action == "Store")
                                return true;
                            break;
                        }
                        a++;
                    }
                }
            }
            return false; 
        }
    }
}