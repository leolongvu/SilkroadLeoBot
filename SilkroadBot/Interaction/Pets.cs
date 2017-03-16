using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace LeoBot
{
    class PetsData
    {
        public static Timer hgp = new Timer();

        public static void PetInfo(Packet packet)
        {
            uint pet_id = packet.ReadUInt32();

            Pets CharPets = new Pets(pet_id);

            for (int i = 0; i < Spawns.pets.Length; i++)
            {
                if (Spawns.pets[i].id == pet_id)
                {
                    CharPets.Speed = Spawns.pets[i].speed;
                    break;
                }
            }

            uint pet_model = packet.ReadUInt32();
            string pet_type = Mobs_Info.mobstypelist[Mobs_Info.mobsidlist.IndexOf(pet_model)];
            if (pet_type.StartsWith("COS_C_HORSE") || pet_type.StartsWith("COS_C_DHORSE"))
            {
                Pets.HorseCurrentHP = packet.ReadUInt32();
                Pets.HorseMaxHP = Mobs_Info.mobshplist[Mobs_Info.mobsidlist.IndexOf(pet_model)];
                packet.ReadUInt32();
                Data.char_horseid = pet_id;
                Globals.MainWindow.SetText(Globals.MainWindow.horseid, String.Format("{0:X8}", Data.char_horseid));
                Globals.MainWindow.UpdateLogs("Horse Summoned!");
                Globals.MainWindow.UpdateBar();
            }
            if (pet_type.StartsWith("COS_P_WOLF") || pet_type.StartsWith("COS_P_WOLF_WHITE") || pet_type.StartsWith("COS_P_BEAR") || pet_type.StartsWith("COS_P_KANGAROO") || pet_type.StartsWith("COS_P_PENGUIN") || pet_type.StartsWith("COS_P_RAVEN") || pet_type.StartsWith("COS_P_FOX") || pet_type.StartsWith("COS_P_JINN"))
            {
                Pets.CurrentHP = packet.ReadUInt32();
                Pets.MaxHP = Mobs_Info.mobshplist[Mobs_Info.mobsidlist.IndexOf(pet_model)];
                packet.ReadUInt32(); // Unknown
                packet.ReadUInt64(); // EXP
                packet.ReadUInt8(); // Level
                CharPets.HGP = packet.ReadUInt16(); // HGP
                Pets.CurrentHGP = CharPets.HGP;
                packet.ReadUInt32(); // Unknown
                CharPets.Name = packet.ReadAscii();
                packet.ReadUInt8(); // Unknown
                packet.ReadUInt32(); // Char ID
                packet.ReadUInt8(); // Unknown
                Data.char_attackpetid = pet_id;
                Globals.MainWindow.UpdateLogs("Found Attack Pet: " + CharPets.Name);
                try
                {
                    hgp.Stop();
                    hgp.Dispose();
                }
                catch { }
                hgp = new System.Timers.Timer();
                hgp.Interval = 3000;
                hgp.Elapsed += new ElapsedEventHandler(hgp_Elapsed);
                hgp.Start();
                hgp.Enabled = true;
                Globals.MainWindow.SetText(Globals.MainWindow.attackname, String.Format("{0}", CharPets.Name));
                Globals.MainWindow.SetText(Globals.MainWindow.attackid, String.Format("{0:X8}", Data.char_attackpetid));
                Globals.MainWindow.UpdateBar();
            }
            if (Data.Types.grab_types.IndexOf(pet_type) != -1)
            {
                //Globals.MainWindow.pet_inv_list.Items.Clear();
                Data.char_grabpetid = pet_id;
                packet.ReadUInt64(); // Unknown
                packet.ReadUInt32(); // Unknown
                CharPets.Name = packet.ReadAscii(); // Petname
                Globals.MainWindow.UpdateLogs("Found Grab Pet: " + CharPets.Name);
                Pets.Inventory_[] Inventory = new Pets.Inventory_[packet.ReadUInt8()];
                byte items_count = packet.ReadUInt8();
                for (int i = 0; i < items_count; i++)
                {
                    byte slot = packet.ReadUInt8();
                    packet.ReadUInt32();
                    uint item_id = packet.ReadUInt32();
                    int index = Items_Info.itemsidlist.IndexOf(item_id);
                    string type = Items_Info.itemstypelist[index];
                    string name = Items_Info.itemsnamelist[index];
                    byte level = Items_Info.itemslevellist[index];
                    Inventory[slot].name = name;
                    Inventory[slot].id = item_id;
                    Inventory[slot].slot = slot;
                    Inventory[slot].type = type;
                    Inventory[slot].level = level;
                    Inventory[slot].maxdurability = Items_Info.itemsdurabilitylist[index];

                    if (type.StartsWith("ITEM_CH") || type.StartsWith("ITEM_EU") || type.StartsWith("ITEM_MALL_AVATAR") || type.StartsWith("ITEM_ETC_E060529_GOLDDRAGONFLAG") || type.StartsWith("ITEM_EVENT_CH") || type.StartsWith("ITEM_EVENT_EU"))
                    {
                        byte item_plus = packet.ReadUInt8();
                        packet.ReadUInt64();
                        Inventory[slot].durability = packet.ReadUInt32();
                        byte blueamm = packet.ReadUInt8();
                        for (int a = 0; a < blueamm; a++)
                        {
                            packet.ReadUInt8();
                            packet.ReadUInt16();
                            packet.ReadUInt32();
                            packet.ReadUInt8();
                        }
                        packet.ReadUInt32();
                        Inventory[slot].count = 1;
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
                        Inventory[slot].count = 1;
                        Inventory[slot].durability = 0;
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
                        Inventory[slot].count = 1;
                        Inventory[slot].durability = 0;
                    }
                    else if (type == "ITEM_ETC_TRANS_MONSTER")
                    {
                        packet.ReadUInt32();
                        Inventory[slot].count = 1;
                        Inventory[slot].durability = 0;
                    }
                    else if (type.StartsWith("ITEM_MALL_MAGIC_CUBE"))
                    {
                        packet.ReadUInt32();
                        Inventory[slot].count = 1;
                        Inventory[slot].durability = 0;
                    }
                    else
                    {
                        ushort count = packet.ReadUInt16();
                        if (type.Contains("ITEM_ETC_ARCHEMY_ATTRSTONE") || type.Contains("ITEM_ETC_ARCHEMY_MAGICSTONE"))
                        {
                            packet.ReadUInt8();
                        }
                        Inventory[slot].count = count;
                        Inventory[slot].durability = 0;
                    }
                }
                Globals.MainWindow.SetText(Globals.MainWindow.grabname, String.Format("{0}", CharPets.Name));
                Globals.MainWindow.SetText(Globals.MainWindow.grabid, String.Format("{0:X8}", Data.char_grabpetid));
                CharPets.Inventory = Inventory;               
            }

            Pets.CharPets.Add(CharPets);
        }

        public static void hgp_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Data.char_attackpetid != 0)
            {
                foreach (Pets AttackPet in Pets.CharPets)
                {
                    if (AttackPet.UniqueID == Data.char_attackpetid)
                    {
                        if (AttackPet.HGP > 0)
                        {
                            AttackPet.HGP--;
                            Pets.CurrentHGP--;
                            if (Globals.MainWindow.Checked(Globals.MainWindow.pethgpuse) == true)
                            {
                                uint hgp = (uint)AttackPet.HGP * 100 / 10000;
                                if (hgp < Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.pethgptext)))
                                {
                                    Autopot.UseHGP();
                                    AttackPet.HGP = (ushort)(AttackPet.HGP + 1000);
                                    Pets.CurrentHGP = (ushort)(Pets.CurrentHGP + 1000);
                                }
                            }
                            Globals.MainWindow.UpdateBar();
                        }
                        else
                        {
                            hgp.Stop();
                            hgp.Dispose();
                        }
                    }
                    else
                    {
                        hgp.Stop();
                        hgp.Dispose();
                    }
                }
            }
        }

        public static void PetStats(Packet packet)
        {
            uint pet_id = packet.ReadUInt32();
            byte type = packet.ReadUInt8();
            switch (type)
            {
                case 0x01:
                    if (pet_id == Data.char_attackpetid)
                    {
                        Data.char_attackpetid = 0;
                    }
                    if (pet_id == Data.char_grabpetid)
                    {
                        Data.char_grabpetid = 0;
                    }

                    foreach (Pets CharPet in Pets.CharPets)
                    {
                        if (CharPet.UniqueID == pet_id)
                        {
                             Pets.CharPets.Remove(CharPet);
                             break;
                        }                        
                    }
                    break;
            }
        }

        public static void HorseAction(Packet packet)
        {
            if (packet.ReadUInt8() == 1)
            {
                uint char_id = packet.ReadUInt32();
                if (char_id == Character.UniqueID) 
                {                                      
                    byte action = packet.ReadUInt8();
                    uint pet_id = packet.ReadUInt32();
                    for (int i = 0; i < Pets.CharPets.Count; i++)
                    {
                        if (Pets.CharPets[i].UniqueID == pet_id)
                        {
                            switch (action)
                            {
                                case 0:
                                    Data.char_horseid = 0;
                                    Data.char_horsespeed = 0;
                                    if (Data.loopaction == "dismounthorse")
                                    {
                                        Data.loopaction = "";
                                        StartLooping.Start();
                                    }
                                    break;
                                case 1:
                                    Data.char_horseid = pet_id;
                                    Data.char_horsespeed = Pets.CharPets[i].Speed;
                                    if (Data.loopaction == "mounthorse")
                                    {
                                        Data.loopaction = "";
                                        Data.loopend = 1;
                                        StartLooping.LoadTrainScript();
                                    }
                                    break;
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}
