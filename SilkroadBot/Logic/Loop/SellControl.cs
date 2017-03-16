using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace LeoBot
{
    class SellControl
    {
        public static void OpenShop()
        {
            uint id = 0;
            switch (StartLooping.type)
            {
                case "ch":
                    for (int i = 0; i < Spawns.NPCID.Count; i++)
                    {
                        if (Spawns.NPCType[i].Contains("CH_SMITH"))
                        {
                            id = Spawns.NPCID[i];
                            break;
                        }
                    }
                    break;
                case "wc":
                    for (int i = 0; i < Spawns.NPCID.Count; i++)
                    {
                        if (Spawns.NPCType[i].Contains("WC_SMITH"))
                        {
                            id = Spawns.NPCID[i];
                            break;
                        }
                    }
                    break;
                case "kt":
                    for (int i = 0; i < Spawns.NPCID.Count; i++)
                    {
                        if (Spawns.NPCType[i].Contains("KT_SMITH"))
                        {
                            id = Spawns.NPCID[i];
                            break;
                        }
                    }
                    break;
                case "eu":
                    for (int i = 0; i < Spawns.NPCID.Count; i++)
                    {
                        if (Spawns.NPCType[i].Contains("EU_SMITH"))
                        {
                            id = Spawns.NPCID[i];
                            break;
                        }
                    }
                    break;
                case "ca":
                    for (int i = 0; i < Spawns.NPCID.Count; i++)
                    {
                        if (Spawns.NPCType[i].Contains("CA_SMITH"))
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

                System.Threading.Thread.Sleep(500);
            }
            else
            {
                Globals.MainWindow.UpdateLogs("Weapon Shop Not In Range!");
            }
        }

        public static void SellManager(uint id)
        {
            if (Data.loop && Data.bot)
            {
                if (Globals.MainWindow.Checked(Globals.MainWindow.nopick) == true)
                {
                    Data.loopaction = "weapon";
                    BuyControl.BuyManager(Spawns.NPCID[Spawns.NPCType.IndexOf(Data.selectednpctype)]);
                }
                else if (Globals.MainWindow.Checked(Globals.MainWindow.normalpick) == true)
                {                    
                    for (byte slot = 13; slot < Character.InventorySize; slot++)
                    {
                        int index = Data.inventoryslot.IndexOf(slot);
                        if (index != -1)
                        {
                            string type = Data.inventorytype[index];
                            if (type != null && type.Contains("RARE") == false && type.Contains("ITEM_MALL") == false)
                            {
                                if (type.StartsWith("ITEM_CH_SWORD") || type.StartsWith("ITEM_CH_BLADE") || type.StartsWith("ITEM_CH_SPEAR") || type.StartsWith("ITEM_CH_TBLADE") || type.StartsWith("ITEM_CH_BOW") || type.StartsWith("ITEM_CH_SHIELD") || type.StartsWith("ITEM_EU_DAGGER") || type.StartsWith("ITEM_EU_SWORD") || type.StartsWith("ITEM_EU_TSWORD") || type.StartsWith("ITEM_EU_AXE") || type.StartsWith("ITEM_EU_CROSSBOW") || type.StartsWith("ITEM_EU_DARKSTAFF") || type.StartsWith("ITEM_EU_TSTAFF") || type.StartsWith("ITEM_EU_HARP") || type.StartsWith("ITEM_EU_STAFF") || type.StartsWith("ITEM_EU_SHIELD"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.wep_drop) == "Sell" && type != Data.f_wep_name && type != Data.s_wep_name)
                                    {
                                        Send(slot, Data.inventorycount[index], id);
                                        break;
                                    }
                                }
                                if (type.StartsWith("ITEM_CH_M_HEAVY") || type.StartsWith("ITEM_CH_M_LIGHT") || type.StartsWith("ITEM_CH_M_CLOTHES") || type.StartsWith("ITEM_CH_W_HEAVY") || type.StartsWith("ITEM_CH_W_LIGHT") || type.StartsWith("ITEM_CH_W_CLOTHES") || type.StartsWith("ITEM_EU_M_HEAVY") || type.StartsWith("ITEM_EU_M_LIGHT") || type.StartsWith("ITEM_EU_M_CLOTHES") || type.StartsWith("ITEM_EU_W_HEAVY") || type.StartsWith("ITEM_EU_W_LIGHT") || type.StartsWith("ITEM_EU_W_CLOTHES"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.armor_drop) == "Sell")
                                    {
                                        Send(slot, Data.inventorycount[index], id);
                                        break;
                                    }
                                }
                                if (type.StartsWith("ITEM_EU_RING") || type.StartsWith("ITEM_EU_EARRING") || type.StartsWith("ITEM_EU_NECKLACE") || type.StartsWith("ITEM_CH_RING") || type.StartsWith("ITEM_CH_EARRING") || type.StartsWith("ITEM_CH_NECKLACE"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.acc_drop) == "Sell")
                                    {
                                        Send(slot, Data.inventorycount[index], id);
                                        break;
                                    }
                                }
                                if (type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_WEAPON"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.wepe_drop) == "Sell")
                                    {
                                        Send(slot, Data.inventorycount[index], id);
                                        break;
                                    }
                                }
                                if (type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_SHIELD"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.shielde_drop) == "Sell")
                                    {
                                        Send(slot, Data.inventorycount[index], id);
                                        break;
                                    }
                                }
                                if (type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_ARMOR"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.prote_drop) == "Sell")
                                    {
                                        Send(slot, Data.inventorycount[index], id);
                                        break;
                                    }
                                }
                                if (type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_ACCESSARY"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.acce_drop) == "Sell")
                                    {
                                        Send(slot, Data.inventorycount[index], id);
                                        break;
                                    }
                                }
                                if (type.StartsWith("ITEM_ETC_ARCHEMY_ATTRTABLET") || type.StartsWith("ITEM_ETC_ARCHEMY_MAGICSTONE"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.tablets_drop) == "Sell")
                                    {
                                        Send(slot, Data.inventorycount[index], id);
                                        break;
                                    }
                                }
                                if (type.StartsWith("ITEM_ETC_ARCHEMY_MATERIAL"))
                                {
                                    if (Globals.MainWindow.ReadText(Globals.MainWindow.materials_drop) == "Sell")
                                    {
                                        Send(slot, Data.inventorycount[index], id);
                                        break;
                                    }
                                }
                            }
                        }                       
                        if (slot + 1 >= Character.InventorySize)
                        {
                            Data.loopaction = "weapon";
                            BuyControl.BuyManager(Spawns.NPCID[Spawns.NPCType.IndexOf(Data.selectednpctype)]);
                            break;
                        }
                    }
                }
                else if (Globals.MainWindow.Checked(Globals.MainWindow.advancepick) == true)
                {
                    for (byte slot = 13; slot < Character.InventorySize; slot++)
                    {
                        int index = Data.inventoryslot.IndexOf(slot);
                        if (index != -1)
                        {
                            string type = Data.inventorytype[index];
                            if (CheckSell(type) == true)
                            {
                                Send(slot, Data.inventorycount[index], id);
                                break;
                            }
                        }
                        if (slot + 1 >= Character.InventorySize)
                        {
                            Data.loopaction = "weapon";
                            BuyControl.BuyManager(Spawns.NPCID[Spawns.NPCType.IndexOf(Data.selectednpctype)]);
                            break;
                        }
                    }
                }
            }
        }

        public static void Send(byte slot, ushort count, uint id)
        {
            Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYMOVEMENT);
            NewPacket.WriteUInt8(0x09); //Sell
            NewPacket.WriteUInt8(slot); //That says everything
            NewPacket.WriteUInt16(count); //Hmmm?
            NewPacket.WriteUInt32(id); //NPC ID
            Proxy.ag_remote_security.Send(NewPacket);
        }

        public static bool CheckSell(string type)
        {
            if (type == null || type.Contains("RARE") || type.Contains("ITEM_MALL") || type.Contains("ITEM_QSP") || type.Contains("ITEM_QNO"))
            {
                return false;
            }
            else
            {
                if (type.StartsWith("ITEM_CH_SWORD") || type.StartsWith("ITEM_CH_BLADE") || type.StartsWith("ITEM_CH_SPEAR") || type.StartsWith("ITEM_CH_TBLADE") || type.StartsWith("ITEM_CH_BOW") || type.StartsWith("ITEM_CH_SHIELD") || type.StartsWith("ITEM_EU_DAGGER") || type.StartsWith("ITEM_EU_SWORD") || type.StartsWith("ITEM_EU_TSWORD") || type.StartsWith("ITEM_EU_AXE") || type.StartsWith("ITEM_EU_CROSSBOW") || type.StartsWith("ITEM_EU_DARKSTAFF") || type.StartsWith("ITEM_EU_TSTAFF") || type.StartsWith("ITEM_EU_HARP") || type.StartsWith("ITEM_EU_STAFF") || type.StartsWith("ITEM_EU_SHIELD"))
                {
                    int a = 0;
                    while ((a < AdvanceItem.Weapon.Count))
                    {
                        if (AdvanceItem.Weapon[a].AdvanceType == type && type != Data.f_wep_name && type != Data.s_wep_name)
                        {
                            if (AdvanceItem.Weapon[a].Action == "Sell")
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
                            if (AdvanceItem.Armor[a].Action == "Sell")
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
                            if (AdvanceItem.Accessorise[a].Action == "Sell")
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
                            if (AdvanceItem.Elixir[a].Action == "Sell")
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
                            if (AdvanceItem.Arrow[a].Action == "Sell")
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
                            if (AdvanceItem.Return[a].Action == "Sell")
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
                            if (AdvanceItem.Potion[a].Action == "Sell")
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
                            if (AdvanceItem.Tablet[a].Action == "Sell")
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
                            if (AdvanceItem.Material[a].Action == "Sell")
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
                            if (AdvanceItem.Allitems[a].Action == "Sell")
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