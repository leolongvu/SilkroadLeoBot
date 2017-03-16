using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace LeoBot
{
    class BuyControl
    {
        public static void Buy(uint id, byte tab, byte slot, uint count)
        {
            Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYMOVEMENT);
            NewPacket.WriteUInt8(8);
            NewPacket.WriteUInt8(tab);
            NewPacket.WriteUInt8(slot);
            NewPacket.WriteUInt16((ushort)count);
            NewPacket.WriteUInt32(id);
            Proxy.ag_remote_security.Send(NewPacket);
        }

        public static void OpenShop()
        {
            uint id = 0;
            for (int i = 0; i < Spawns.NPCID.Count; i++)
            {
                if (Spawns.NPCType[i] != null)
                {
                    string type = Spawns.NPCType[i];
                    if (Data.loopaction == "stable")
                    {
                        switch (StartLooping.type)
                        {
                            case "ch":
                                if (Spawns.NPCType[i].Contains("CH_HORSE"))
                                {
                                    id = Spawns.NPCID[i];
                                    break;
                                }
                                break;
                            case "wc":
                                if (Spawns.NPCType[i].Contains("WC_HORSE"))
                                {
                                    id = Spawns.NPCID[i];
                                    break;
                                }                                  
                                break;
                            case "kt":
                                if (Spawns.NPCType[i].Contains("KT_HORSE"))
                                {
                                    id = Spawns.NPCID[i];
                                    break;
                                }
                                break;
                            case "eu":
                                if (Spawns.NPCType[i].Contains("EU_HORSE"))
                                {
                                    id = Spawns.NPCID[i];
                                    break;
                                }
                                break;
                            case "ca":
                                if (Spawns.NPCType[i].Contains("CA_HORSE"))
                                {
                                    id = Spawns.NPCID[i];
                                    break;
                                }
                                break;
                        }
                    }
                    if (Data.loopaction == "accessory")
                    {
                        switch (StartLooping.type)
                        {
                            case "ch":
                                if (Spawns.NPCType[i].Contains("CH_ACCESSORY"))
                                {
                                    id = Spawns.NPCID[i];
                                    break;
                                }
                                break;
                            case "wc":
                                if (Spawns.NPCType[i].Contains("WC_ACCESSORY"))
                                {
                                    id = Spawns.NPCID[i];
                                    break;
                                }
                                break;
                            case "kt":
                                if (Spawns.NPCType[i].Contains("KT_ACCESSORY"))
                                {
                                    id = Spawns.NPCID[i];
                                    break;
                                }
                                break;
                            case "eu":
                                if (Spawns.NPCType[i].Contains("EU_ACCESSORY"))
                                {
                                    id = Spawns.NPCID[i];
                                    break;
                                }
                                break;
                            case "ca":
                                if (Spawns.NPCType[i].Contains("CA_ACCESSORY"))
                                {
                                    id = Spawns.NPCID[i];
                                    break;
                                }
                                break;
                        }
                    }
                    if (Data.loopaction == "potion")
                    {
                        switch (StartLooping.type)
                        {
                            case "ch":
                                if (Spawns.NPCType[i].Contains("CH_POTION"))
                                {
                                    id = Spawns.NPCID[i];
                                    break;
                                }
                                break;
                            case "wc":
                                if (Spawns.NPCType[i].Contains("WC_POTION"))
                                {
                                    id = Spawns.NPCID[i];
                                    break;
                                }
                                break;
                            case "kt":
                                if (Spawns.NPCType[i].Contains("KT_POTION"))
                                {
                                    id = Spawns.NPCID[i];
                                    break;
                                }
                                break;
                            case "eu":
                                if (Spawns.NPCType[i].Contains("EU_POTION"))
                                {
                                    id = Spawns.NPCID[i];
                                    break;
                                }
                                break;
                            case "ca":
                                if (Spawns.NPCType[i].Contains("CA_POTION"))
                                {
                                    id = Spawns.NPCID[i];
                                    break;
                                }
                                break;
                        }
                    }
                }
            }
            if (id != 0)
            {
                Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTSELECT);
                NewPacket.WriteUInt32(id);
                Proxy.ag_remote_security.Send(NewPacket);
            }
            else
            {
                Globals.MainWindow.UpdateLogs("Shop Not In Range!");
            }
        }

        public static void BuyManager(uint id)
        {
            if (Data.loop && Data.bot)
            {
                System.Threading.Thread.Sleep(2);
                string npc_type = Spawns.NPCType[Spawns.NPCID.IndexOf(id)];
                while (true)
                {
                    //Smith
                    if (Data.loopaction == "weapon")
                    {
                        if (Globals.MainWindow.Checked(Globals.MainWindow.repairall) == true)
                        {
                            System.Threading.Thread.Sleep(1000);

                            Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_REPAIR);
                            NewPacket.WriteUInt32(Training.currentlyselected);
                            NewPacket.WriteUInt8(2);
                            Proxy.ag_remote_security.Send(NewPacket);

                            System.Threading.Thread.Sleep(2000);
                        }
                        if (Data.itemscount.arrows < Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.arrows_count)))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.arrows_count)) - Data.itemscount.arrows > 250)
                            {
                                if (npc_type.StartsWith("NPC_CH_SMITH") || npc_type.StartsWith("NPC_WC_SMITH"))
                                {
                                    Buy(id, 2, 0, 250);
                                    break;
                                }
                                else
                                {
                                    if (npc_type.StartsWith("NPC_KT_SMITH"))
                                    {
                                        Buy(id, 5, 0, 250);
                                        break;
                                    }
                                    else
                                    {
                                        Data.bot = false;
                                        Data.loop = false;
                                        Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                        Globals.MainWindow.UpdateLogs("Cannot Buy Arrows!");
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.arrows_count)) - Data.itemscount.arrows;
                                if (npc_type.StartsWith("NPC_CH_SMITH") || npc_type.StartsWith("NPC_WC_SMITH"))
                                {
                                    Buy(id, 2, 0, count);
                                    break;
                                }
                                else
                                {
                                    if (npc_type.StartsWith("NPC_KT_SMITH"))
                                    {
                                        Buy(id, 5, 0, count);
                                        break;
                                    }
                                    else
                                    {
                                        Data.bot = false;
                                        Data.loop = false;
                                        Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                        Globals.MainWindow.UpdateLogs("Cannot Buy Arrows!");
                                        break;
                                    }
                                }
                            }
                        }
                        if (Data.itemscount.bolts < Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.bolts_count)))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.bolts_count)) - Data.itemscount.bolts > 250)
                            {
                                if (npc_type.StartsWith("NPC_CA_SMITH") || npc_type.StartsWith("NPC_EU_SMITH") || npc_type.StartsWith("NPC_KT_SMITH"))
                                {
                                    Buy(id, 2, 0, 250);
                                    break;
                                }
                                else
                                {
                                    Data.bot = false;
                                    Data.loop = false;
                                    Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                    Globals.MainWindow.UpdateLogs("Cannot Buy Bolts!");
                                    break;
                                }
                            }
                            else
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.bolts_count)) - Data.itemscount.bolts;
                                if (npc_type.StartsWith("NPC_CA_SMITH") || npc_type.StartsWith("NPC_EU_SMITH") || npc_type.StartsWith("NPC_KT_SMITH"))
                                {
                                    Buy(id, 2, 0, count);
                                    break;
                                }
                                else
                                {
                                    Data.bot = false;
                                    Data.loop = false;
                                    Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                    Globals.MainWindow.UpdateLogs("Cannot Buy Bolts!");
                                    break;
                                }
                            }
                        }
                        Close();
                        break;
                    }
                    //Smith end
                    //Stable
                    if (Data.loopaction == "stable")
                    {
                        if (Data.itemscount.horse < Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.horse_count)))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.horse_count)) - Data.itemscount.horse > 50)
                            {
                                switch (Globals.MainWindow.ReadText(Globals.MainWindow.horse_buy))
                                {
                                    case "Red Horse":
                                        Buy(id, 0, 0, 50);
                                        break;
                                    case "Shadow Horse":
                                        Buy(id, 0, 1, 50);
                                        break;
                                    case "Dragon Horse":
                                        Buy(id, 0, 2, 50);
                                        break;
                                    case "Ironclad Horse":
                                        Buy(id, 0, 3, 50);
                                        break;
                                }
                            }
                            else
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.horse_count)) - Data.itemscount.horse;
                                switch (Globals.MainWindow.ReadText(Globals.MainWindow.horse_buy))
                                {
                                    case "Red Horse":
                                        Buy(id, 0, 0, count);
                                        break;
                                    case "Shadow Horse":
                                        Buy(id, 0, 1, count);
                                        break;
                                    case "Dragon Horse":
                                        Buy(id, 0, 2, count);
                                        break;
                                    case "Ironclad Horse":
                                        Buy(id, 0, 3, count);
                                        break;
                                }
                            }
                        }                                                   
                        if (Data.itemscount.glass < Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.revive_count)))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.revive_count)) - Data.itemscount.glass > 50)
                            {
                                Buy(id, 3, 2, 50);
                                break;
                            }
                            else
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.revive_count)) - Data.itemscount.glass;
                                Buy(id, 3, 2, count);
                                break;
                            }
                        }
                        if (Data.itemscount.hgp < Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.hgp_count)))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.hgp_count)) - Data.itemscount.hgp > 50)
                            {
                                Buy(id, 3, 3, 50);
                                break;
                            }
                            else
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.hgp_count)) - Data.itemscount.hgp;
                                Buy(id, 3, 3, count);
                                break;
                            }
                        }
                        if (Data.itemscount.pet_hp < Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.php_count)))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.php_count)) - Data.itemscount.pet_hp > 50)
                            {
                                switch (Globals.MainWindow.ReadText(Globals.MainWindow.php_buy))
                                {
                                    case "Recovery kit (small)":
                                        Buy(id, 3, 0, 50);
                                        break;
                                    case "Recovery kit (large)":
                                        Buy(id, 3, 1, 50);
                                        break;
                                    case "Recovery kit (x-large)":
                                        Buy(id, 3, 6, 50);
                                        break;
                                }
                            }
                            else
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.php_count)) - Data.itemscount.pet_hp;
                                switch (Globals.MainWindow.ReadText(Globals.MainWindow.php_buy))
                                {
                                    case "Recovery kit (small)":
                                        Buy(id, 3, 0, count);
                                        break;
                                    case "Recovery kit (large)":
                                        Buy(id, 3, 1, count);
                                        break;
                                    case "Recovery kit (x-large)":
                                        Buy(id, 3, 6, count);
                                        break;
                                }
                            }                          
                        }
                        if (Data.itemscount.petuni < Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.pet_status_count)))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.pet_status_count)) - Data.itemscount.petuni > 50)
                            {
                                switch (Globals.MainWindow.ReadText(Globals.MainWindow.pet_status_buy))
                                {
                                    case "Abnormal state recovery potion (small)":
                                        Buy(id, 3, 4, 50);
                                        break;
                                    case "Abnormal state recovery potion (medium)":
                                        Buy(id, 3, 5, 50);
                                        break;
                                }
                            }
                            else
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.pet_status_count)) - Data.itemscount.petuni;
                                switch (Globals.MainWindow.ReadText(Globals.MainWindow.pet_status_buy))
                                {
                                    case "Abnormal state recovery potion (small)":
                                        Buy(id, 3, 4, count);
                                        break;
                                    case "Abnormal state recovery potion (medium)":
                                        Buy(id, 3, 5, count);
                                        break;
                                }
                            }
                        }
                        Packet NewPacket = new Packet((ushort)0x704B);
                        NewPacket.WriteUInt32(Training.currentlyselected);
                        Proxy.ag_remote_security.Send(NewPacket);
                        Training.currentlyselected = 0;
                        break;
                    }
                    //Stable end
                    //Accessory
                    if (Data.loopaction == "accessory")
                    {
                        if (Data.itemscount.return_scrool < Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.return_count)))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.return_count)) - Data.itemscount.return_scrool > 50)
                            {
                                switch (Globals.MainWindow.ReadText(Globals.MainWindow.return_buy))
                                {
                                    case "Return Scroll":
                                        Buy(id, 1, 0, 50);
                                        break;
                                    case "Special Return Scroll":
                                        Buy(id, 1, 1, 50);
                                        break;
                                }
                                break;
                            }
                            else       
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.return_count)) - Data.itemscount.return_scrool;
                                switch (Globals.MainWindow.ReadText(Globals.MainWindow.return_buy))
                                {
                                    case "Return Scroll":
                                        Buy(id, 1, 0, count);
                                        break;
                                    case "Special Return Scroll":
                                        Buy(id, 1, 1, count);
                                        break;
                                }
                                break;
                            }
                        }
                        if (Data.itemscount.speed_pots < Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.speed_count)))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.speed_count)) - Data.itemscount.speed_pots > 10)
                            {
                                if (npc_type.StartsWith("NPC_CH_ACCESSORY"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.speed_buy))
                                    {
                                        case "Drug of wind":
                                            Buy(id, 2, 11, 10);
                                            break;
                                        case "Drug of typoon":
                                            Buy(id, 2, 12, 10);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_EU_ACCESSORY"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.speed_buy))
                                    {
                                        case "Drug of wind":
                                            Buy(id, 2, 11, 10);
                                            break;
                                        case "Drug of typoon":
                                            Buy(id, 2, 12, 10);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_WC_ACCESSORY") || npc_type.StartsWith("NPC_CA_ACCESSORY"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.speed_buy))
                                    {
                                        case "Drug of wind":
                                            Buy(id, 2, 8, 10);
                                            break;
                                        case "Drug of typoon":
                                            Buy(id, 2, 9, 10);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_KT_ACCESSORY"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.speed_buy))
                                    {
                                        case "Drug of wind":
                                            Buy(id, 2, 7, 10);
                                            break;
                                        case "Drug of typoon":
                                            Buy(id, 2, 8, 10);
                                            break;
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.speed_count)) - Data.itemscount.speed_pots;
                                if (npc_type.StartsWith("NPC_CH_ACCESSORY"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.speed_buy))
                                    {
                                        case "Drug of wind":
                                            Buy(id, 2, 8, count);
                                            break;
                                        case "Drug of typoon":
                                            Buy(id, 2, 9, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_EU_ACCESSORY"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.speed_buy))
                                    {
                                        case "Drug of wind":
                                            Buy(id, 2, 11, count);
                                            break;
                                        case "Drug of typoon":
                                            Buy(id, 2, 12, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_WC_ACCESSORY") || npc_type.StartsWith("NPC_CA_ACCESSORY"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.speed_buy))
                                    {
                                        case "Drug of wind":
                                            Buy(id, 2, 8, count);
                                            break;
                                        case "Drug of typoon":
                                            Buy(id, 2, 9, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_KT_ACCESSORY"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.speed_buy))
                                    {
                                        case "Drug of wind":
                                            Buy(id, 2, 7, count);
                                            break;
                                        case "Drug of typoon":
                                            Buy(id, 2, 8, count);
                                            break;
                                    }
                                    break;
                                }
                            }
                        }
                        Close();
                        break;
                    }
                    //Accesory end
                    //Potion shop
                    if (Data.loopaction == "potion")
                    {
                        if (Data.itemscount.hp_pots < Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.hp_count)))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.hp_count)) - Data.itemscount.hp_pots > 50)
                            {                            
                                if (npc_type.StartsWith("NPC_CH_POTION") || npc_type.StartsWith("NPC_EU_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.hp_buy))
                                    {
                                        case "HP Recovery Herb":
                                            Buy(id, 0, 0, 50);
                                            break;
                                        case "HP Recovery Potion (Small)":
                                            Buy(id, 0, 1, 50);
                                            break;
                                        case "HP Recovery Potion (Medium)":
                                            Buy(id, 0, 2, 50);
                                            break;
                                        case "HP Recovery Potion (Large)":
                                            Buy(id, 0, 3, 50);
                                            break;
                                        case "HP Recovery Potion (X-Large)":
                                            Buy(id, 0, 4, 50);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_CA_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.hp_buy))
                                    {
                                        case "HP Recovery Herb":
                                            Data.bot = false;
                                            Data.loop = false;
                                            Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                            Globals.MainWindow.UpdateLogs("Cannot Buy HP Pots! Cannot Find [HP Recovery Herb] In This Shop!");
                                            break;
                                        case "HP Recovery Potion (Small)":
                                            Buy(id, 0, 0, 50);
                                            break;
                                        case "HP Recovery Potion (Medium)":
                                            Buy(id, 0, 1, 50);
                                            break;
                                        case "HP Recovery Potion (Large)":
                                            Buy(id, 0, 2, 50);
                                            break;
                                        case "HP Recovery Potion (X-Large)":
                                            Buy(id, 0, 3, 50);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_WC_POTION")) 
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.hp_buy))
                                    {
                                        case "HP Recovery Herb":
                                            Buy(id, 0, 0, 50);
                                            break;
                                        case "HP Recovery Potion (Small)":
                                            Buy(id, 0, 1, 50);
                                            break;
                                        case "HP Recovery Potion (Medium)":
                                            Buy(id, 0, 2, 50);
                                            break;
                                        case "HP Recovery Potion (Large)":
                                            Buy(id, 0, 3, 50);
                                            break;
                                        case "HP Recovery Potion (X-Large)":
                                            Buy(id, 0, 4, 50);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_KT_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.hp_buy))
                                    {
                                        case "HP Recovery Herb":
                                            Data.bot = false;
                                            Data.loop = false;
                                            Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                            Globals.MainWindow.UpdateLogs("Cannot Buy HP Pots! Cannot Find [HP Recovery Herb] In This Shop!");
                                            break;
                                        case "HP Recovery Potion (Small)":
                                            Data.bot = false;
                                            Data.loop = false;
                                            Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                            Globals.MainWindow.UpdateLogs("Cannot Buy HP Pots! Cannot Find [HP Recovery Potion (Small)] In This Shop!");
                                            break;
                                        case "HP Recovery Potion (Medium)":
                                            Buy(id, 0, 0, 50);
                                            break;
                                        case "HP Recovery Potion (Large)":
                                            Buy(id, 0, 1, 50);
                                            break;
                                        case "HP Recovery Potion (X-Large)":
                                            Buy(id, 0, 2, 50);
                                            break;
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.hp_count)) - Data.itemscount.hp_pots;
                                if (npc_type.StartsWith("NPC_CH_POTION") || npc_type.StartsWith("NPC_EU_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.hp_buy))
                                    {
                                        case "HP Recovery Herb":
                                            Buy(id, 0, 0, count);
                                            break;
                                        case "HP Recovery Potion (Small)":
                                            Buy(id, 0, 1, count);
                                            break;
                                        case "HP Recovery Potion (Medium)":
                                            Buy(id, 0, 2, count);
                                            break;
                                        case "HP Recovery Potion (Large)":                                            
                                            Buy(id, 0, 3, count);
                                            break;
                                        case "HP Recovery Potion (X-Large)":
                                            Buy(id, 0, 4, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_CA_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.hp_buy))
                                    {
                                        case "HP Recovery Herb":
                                            Data.bot = false;
                                            Data.loop = false;
                                            Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                            Globals.MainWindow.UpdateLogs("Cannot Buy HP Pots! Cannot Find [HP Recovery Herb] In This Shop!");
                                            break;
                                        case "HP Recovery Potion (Small)":
                                            Buy(id, 0, 0, count);
                                            break;
                                        case "HP Recovery Potion (Medium)":
                                            Buy(id, 0, 1, count);
                                            break;
                                        case "HP Recovery Potion (Large)":
                                            Buy(id, 0, 2, count);
                                            break;
                                        case "HP Recovery Potion (X-Large)":
                                            Buy(id, 0, 3, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_WC_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.hp_buy))
                                    {
                                        case "HP Recovery Herb":
                                            Buy(id, 0, 0, count);
                                            break;
                                        case "HP Recovery Potion (Small)":
                                            Buy(id, 0, 1, count);
                                            break;
                                        case "HP Recovery Potion (Medium)":
                                            Buy(id, 0, 2, count);
                                            break;
                                        case "HP Recovery Potion (Large)":
                                            Buy(id, 0, 3, count);
                                            break;
                                        case "HP Recovery Potion (X-Large)":
                                            Buy(id, 0, 4, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_KT_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.hp_buy))
                                    {
                                        case "HP Recovery Herb":
                                            Data.bot = false;
                                            Data.loop = false;
                                            Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                            Globals.MainWindow.UpdateLogs("Cannot Buy HP Pots! Cannot Find [HP Recovery Herb] In This Shop!");
                                            break;
                                        case "HP Recovery Potion (Small)":
                                            Data.bot = false;
                                            Data.loop = false;
                                            Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                            Globals.MainWindow.UpdateLogs("Cannot Buy HP Pots! Cannot Find [HP Recovery Potion (Small)] In This Shop!");
                                            break;
                                        case "HP Recovery Potion (Medium)":
                                            Buy(id, 0, 0, count);
                                            break;
                                        case "HP Recovery Potion (Large)":
                                            Buy(id, 0, 1, count);
                                            break;
                                        case "HP Recovery Potion (X-Large)":
                                            Buy(id, 0, 2, count);
                                            break;
                                    }
                                    break;
                                }
                            }
                        }

                        if (Data.itemscount.mp_pots < Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.mp_count)))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.mp_count)) - Data.itemscount.mp_pots > 50)
                            {
                                if (npc_type.StartsWith("NPC_CH_POTION") || npc_type.StartsWith("NPC_EU_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.mp_buy))
                                    {
                                        case "MP Recovery Herb":
                                            Buy(id, 0, 5, 50);
                                            break;
                                        case "MP Recovery Potion (Small)":
                                            Buy(id, 0, 6, 50);
                                            break;
                                        case "MP Recovery Potion (Medium)":
                                            Buy(id, 0, 7, 50);
                                            break;
                                        case "MP Recovery Potion (Large)":
                                            Buy(id, 0, 8, 50);
                                            break;
                                        case "MP Recovery Potion (X-Large)":
                                            Buy(id, 0, 9, 50);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_WC_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.mp_buy))
                                    {
                                        case "MP Recovery Herb":
                                            Buy(id, 0, 5, 50);
                                            break;
                                        case "MP Recovery Potion (Small)":
                                            Buy(id, 0, 6, 50);
                                            break;
                                        case "MP Recovery Potion (Medium)":
                                            Buy(id, 0, 7, 50);
                                            break;
                                        case "MP Recovery Potion (Large)":
                                            Buy(id, 0, 8, 50);
                                            break;
                                        case "MP Recovery Potion (X-Large)":
                                            Buy(id, 0, 9, 50);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_CA_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.mp_buy))
                                    {
                                        case "MP Recovery Herb":
                                            Data.bot = false;
                                            Data.loop = false;
                                            Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                            Globals.MainWindow.UpdateLogs("Cannot Buy MP Pots! Cannot Find [MP Recovery Herb] In This Shop!");
                                            break;
                                        case "MP Recovery Potion (Small)":
                                            Buy(id, 0, 4, 50);
                                            break;
                                        case "MP Recovery Potion (Medium)":
                                            Buy(id, 0, 5, 50);
                                            break;
                                        case "MP Recovery Potion (Large)":
                                            Buy(id, 0, 6, 50);
                                            break;
                                        case "MP Recovery Potion (X-Large)":
                                            Buy(id, 0, 7, 50);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_KT_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.mp_buy))
                                    {
                                        case "MP Recovery Herb":
                                            Data.bot = false;
                                            Data.loop = false;
                                            Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                            Globals.MainWindow.UpdateLogs("Cannot Buy MP Pots! Cannot Find [MP Recovery Herb] In This Shop!");
                                            break;
                                        case "MP Recovery Potion (Small)":
                                            Data.bot = false;
                                            Data.loop = false;
                                            Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                            Globals.MainWindow.UpdateLogs("Cannot Buy MP Pots! Cannot Find [MP Recovery Potion (Small)] In This Shop!");
                                            break;
                                        case "MP Recovery Potion (Medium)":
                                            Buy(id, 0, 3, 50);
                                            break;
                                        case "MP Recovery Potion (Large)":
                                            Buy(id, 0, 4, 50);
                                            break;
                                        case "MP Recovery Potion (X-Large)":
                                            Buy(id, 0, 5, 50);
                                            break;
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.mp_count)) - Data.itemscount.mp_pots;
                                if (npc_type.StartsWith("NPC_CH_POTION") || npc_type.StartsWith("NPC_EU_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.mp_buy))
                                    {
                                        case "MP Recovery Herb":
                                            Buy(id, 0, 5, count);
                                            break;
                                        case "MP Recovery Potion (Small)":
                                            Buy(id, 0, 6, count);
                                            break;
                                        case "MP Recovery Potion (Medium)":
                                            Buy(id, 0, 7, count);
                                            break;
                                        case "MP Recovery Potion (Large)":
                                            Buy(id, 0, 8, count);
                                            break;
                                        case "MP Recovery Potion (X-Large)":
                                            Buy(id, 0, 9, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_WC_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.mp_buy))
                                    {
                                        case "MP Recovery Herb":
                                            Buy(id, 0, 5, 50);
                                            break;
                                        case "MP Recovery Potion (Small)":
                                            Buy(id, 0, 6, 50);
                                            break;
                                        case "MP Recovery Potion (Medium)":
                                            Buy(id, 0, 7, 50);
                                            break;
                                        case "MP Recovery Potion (Large)":
                                            Buy(id, 0, 8, 50);
                                            break;
                                        case "MP Recovery Potion (X-Large)":
                                            Buy(id, 0, 9, 50);
                                            break;
                                    }
                                    break;
                                }
                                if  (npc_type.StartsWith("NPC_CA_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.mp_buy))
                                    {
                                        case "MP Recovery Herb":
                                            Data.bot = false;
                                            Data.loop = false;
                                            Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                            Globals.MainWindow.UpdateLogs("Cannot Buy MP Pots! Cannot Find [MP Recovery Herb] In This Shop!");
                                            break;
                                        case "MP Recovery Potion (Small)":
                                            Buy(id, 0, 4, count);
                                            break;
                                        case "MP Recovery Potion (Medium)":
                                            Buy(id, 0, 5, count);
                                            break;
                                        case "MP Recovery Potion (Large)":
                                            Buy(id, 0, 6, count);
                                            break;
                                        case "MP Recovery Potion (X-Large)":
                                            Buy(id, 0, 7, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_KT_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.mp_buy))
                                    {
                                        case "MP Recovery Herb":
                                            Data.bot = false;
                                            Data.loop = false;
                                            Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                            Globals.MainWindow.UpdateLogs("Cannot Buy MP Pots! Cannot Find [MP Recovery Herb] In This Shop!");
                                            break;
                                        case "MP Recovery Potion (Small)":
                                            Data.bot = false;
                                            Data.loop = false;
                                            Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                            Globals.MainWindow.UpdateLogs("Cannot Buy MP Pots! Cannot Find [MP Recovery Potion (Small)] In This Shop!");
                                            break;
                                        case "MP Recovery Potion (Medium)":
                                            Buy(id, 0, 3, count);
                                            break;
                                        case "MP Recovery Potion (Large)":
                                            Buy(id, 0, 4, count);
                                            break;
                                        case "MP Recovery Potion (X-Large)":
                                            Buy(id, 0, 5, count);
                                            break;
                                    }
                                    break;
                                }
                            }
                        }

                        if (Data.itemscount.uni_pills < Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.uni_count)))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.uni_count)) - Data.itemscount.uni_pills > 50)
                            {
                                if (npc_type.StartsWith("NPC_CH_POTION") || npc_type.StartsWith("NPC_EU_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.uni_buy))
                                    {
                                        case "Universal Pill (small)":
                                            Buy(id, 0, 10, 50);
                                            break;
                                        case "Universal Pill (medium)":
                                            Buy(id, 0, 11, 50);
                                            break;
                                        case "Universal Pill (large)":
                                            Buy(id, 0, 12, 50);
                                            break;
                                        case "Special Universal Pill (small)":
                                            Buy(id, 0, 13, 50);
                                            break;
                                        case "Special Universal Pill (medium)":
                                            Buy(id, 0, 14, 50);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_CA_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.uni_buy))
                                    {
                                        case "Universal Pill (small)":
                                            Buy(id, 0, 8, 50);
                                            break;
                                        case "Universal Pill (medium)":
                                            Buy(id, 0, 9, 50);
                                            break;
                                        case "Universal Pill (large)":
                                            Buy(id, 0, 10, 50);
                                            break;
                                        case "Special Universal Pill (small)":
                                            Buy(id, 0, 11, 50);
                                            break;
                                        case "Special Universal Pill (medium)":
                                            Buy(id, 0, 12, 50);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_WC_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.uni_buy))
                                    {
                                        case "Universal Pill (small)":
                                            Data.bot = false;
                                            Data.loop = false;
                                            Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                            Globals.MainWindow.UpdateLogs("Cannot Buy Universal Pill! Cannot Find [Universal Pill (small)] In This Shop!");
                                            break;
                                        case "Universal Pill (medium)":
                                            Buy(id, 0, 10, 50);
                                            break;
                                        case "Universal Pill (large)":
                                            Buy(id, 0, 11, 50);
                                            break;
                                        case "Special Universal Pill (small)":
                                            Buy(id, 0, 12, 50);
                                            break;
                                        case "Special Universal Pill (medium)":
                                            Buy(id, 0, 13, 50);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_KT_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.uni_buy))
                                    {
                                        case "Universal Pill (small)":
                                            Buy(id, 0, 6, 50);
                                            break;
                                        case "Universal Pill (medium)":
                                            Buy(id, 0, 7, 50);
                                            break;
                                        case "Universal Pill (large)":
                                            Buy(id, 0, 8, 50);
                                            break;
                                        case "Special Universal Pill (small)":
                                            Buy(id, 0, 9, 50);
                                            break;
                                        case "Special Universal Pill (medium)":
                                            Buy(id, 0, 10, 50);
                                            break;
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.uni_count)) - Data.itemscount.uni_pills;
                                if (npc_type.StartsWith("NPC_CH_POTION") || npc_type.StartsWith("NPC_EU_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.uni_buy))
                                    {
                                        case "Universal Pill (small)":
                                            Buy(id, 0, 10, count);
                                            break;
                                        case "Universal Pill (medium)":
                                            Buy(id, 0, 11, count);
                                            break;
                                        case "Universal Pill (large)":
                                            Buy(id, 0, 12, count);
                                            break;
                                        case "Special Universal Pill (small)":
                                            Buy(id, 0, 13, count);
                                            break;
                                        case "Special Universal Pill (medium)":
                                            Buy(id, 0, 14, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_WC_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.uni_buy))
                                    {
                                        case "Universal Pill (small)":
                                            Data.bot = false;
                                            Data.loop = false;
                                            Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                            Globals.MainWindow.UpdateLogs("Cannot Buy Universal Pill! Cannot Find [Universal Pill (small)] In This Shop!");
                                            break;
                                        case "Universal Pill (medium)":
                                            Buy(id, 0, 10, count);
                                            break;
                                        case "Universal Pill (large)":
                                            Buy(id, 0, 11, count);
                                            break;
                                        case "Special Universal Pill (small)":
                                            Buy(id, 0, 12, count);
                                            break;
                                        case "Special Universal Pill (medium)":
                                            Buy(id, 0, 13, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_CA_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.uni_buy))
                                    {
                                        case "Universal Pill (small)":
                                            Buy(id, 0, 8, count);
                                            break;
                                        case "Universal Pill (medium)":
                                            Buy(id, 0, 9, count);
                                            break;
                                        case "Universal Pill (large)":
                                            Buy(id, 0, 10, count);
                                            break;
                                        case "Special Universal Pill (small)":
                                            Buy(id, 0, 11, count);
                                            break;
                                        case "Special Universal Pill (medium)":
                                            Buy(id, 0, 12, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_KT_POTION"))
                                {
                                    switch (Globals.MainWindow.ReadText(Globals.MainWindow.uni_buy))
                                    {
                                        case "Universal Pill (small)":
                                            Buy(id, 0, 6, count);
                                            break;
                                        case "Universal Pill (medium)":
                                            Buy(id, 0, 7, count);
                                            break;
                                        case "Universal Pill (large)":
                                            Buy(id, 0, 8, count);
                                            break;
                                        case "Special Universal Pill (small)":
                                            Buy(id, 0, 9, count);                                       
                                            break;
                                        case "Special Universal Pill (medium)":
                                            Buy(id, 0, 10, count);
                                            break;
                                    }
                                    break;
                                }
                            }
                        }
                        Close();
                        break;
                    }
                }
            }
        }

        public static void Close()
        {
            Packet NewPacket = new Packet((ushort)0x704B);
            NewPacket.WriteUInt32(Training.currentlyselected);
            Proxy.ag_remote_security.Send(NewPacket);

            Training.currentlyselected = 0;
            InventoryControl.MergeItems();      
        }
    }
}