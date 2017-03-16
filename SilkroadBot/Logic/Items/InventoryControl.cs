using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LeoBot
{
    class InventoryControl
    {

        public static void Durability(Packet packet)
        {
            byte slot = packet.ReadUInt8();
            uint new_durability = packet.ReadUInt32();
            int index = Data.inventoryslot.IndexOf(slot);
            if (index != -1)
            {
                Data.inventorydurability[index] = new_durability;
                if (Globals.MainWindow.Checked(Globals.MainWindow.lowdurability) == true)
                {
                    if (new_durability <= 1)
                    {
                        Globals.MainWindow.UpdateLogs("Returning To Town! Low Durability");
                        BotAction.UseReturn();
                    }
                }
            }
        }

        public static void Inventory_Update(Packet packet)
        {
            int type = packet.ReadUInt8();
            if (type == 1)
            {
                byte slot = packet.ReadUInt8();
                ushort count = packet.ReadUInt16();
                int index = Data.inventoryslot.IndexOf(slot);
                if (count > 0)
                {
                    Data.inventorycount[index] = count;
                }
                else
                {
                    Data.inventoryid.RemoveAt(index);
                    Data.inventorytype.RemoveAt(index);
                    Data.inventoryslot.RemoveAt(index);
                    Data.inventorycount.RemoveAt(index);
                    Data.inventorydurability.RemoveAt(index);
                    Data.inventorylevel.RemoveAt(index);
                    Data.inventoryname.RemoveAt(index);
                    Data.maxdurability.RemoveAt(index);
                }
            }
            ItemsCount.CountManager();
            Globals.MainWindow.UpdateInventory();
        }

        public static void Inventory_Update1(Packet packet)
        {
            Packet vienas = packet;
            int check = packet.ReadUInt8();
            if (check == 1)
            {
                int typ = packet.ReadUInt8();
                if (typ == 0) // Inventory <> Inventory
                {
                    byte inv_1 = packet.ReadUInt8();
                    byte inv_2 = packet.ReadUInt8();
                    ushort count = packet.ReadUInt16();
                    int index_1 = Data.inventoryslot.IndexOf(inv_1);                   
                    int index_2 = Data.inventoryslot.IndexOf(inv_2);
                    if (index_2 == -1)
                    {
                        // No item, Moving !
                        Data.inventoryslot[index_1] = inv_2;
                        if (itemid != 0)
                        {
                            Data.inventoryslot[Data.inventorytype.IndexOf(Items_Info.itemstypelist[Items_Info.itemsidlist.IndexOf(itemid)])] = intendtomove;
                            itemid = 0;
                            intendtomove = 0;
                        }
                    }
                    else
                    {
                        // The item exist !
                        if (Data.inventorytype[index_1] == Data.inventorytype[index_2])
                        {
                            // Items Are Same, Merge It !
                            if (Data.inventorycount[index_2] == Items_Info.items_maxlist[Items_Info.itemstypelist.IndexOf(Data.inventorytype[index_2])])
                            {
                                // Items Are Maxed, Move It !
                                Data.inventoryslot[index_1] = inv_2;
                                Data.inventoryslot[index_2] = inv_1;
                            }
                            else
                            {
                                // Items Are Same, Merge It !
                                if (Data.inventorycount[index_1] == count)
                                {
                                    // Merged Everything, Delete The First Item !
                                    Data.inventorycount[index_2] += count;

                                    Data.inventoryid.RemoveAt(index_1);
                                    Data.inventorytype.RemoveAt(index_1);
                                    Data.inventorycount.RemoveAt(index_1);
                                    Data.inventoryslot.RemoveAt(index_1);
                                    Data.inventorydurability.RemoveAt(index_1);
                                    Data.inventorylevel.RemoveAt(index_1);
                                    Data.inventoryname.RemoveAt(index_1);
                                    Data.maxdurability.RemoveAt(index_1);
                                }
                                else
                                {
                                    // Merged Not Everything, Recalculate Quantity !
                                    Data.inventorycount[index_2] += count;
                                    Data.inventorycount[index_1] -= count;
                                }
                            }
                        }
                        else
                        {
                            // Items Are Different, Move It !
                            Data.inventoryslot[index_1] = inv_2;
                            Data.inventoryslot[index_2] = inv_1;
                        }
                    }
                    Globals.MainWindow.UpdateInventory();
                    if (Buffas.changing_weapon)
                    {
                        LogicControl.Manager();
                    }
                    if (Data.loopaction == "merge")
                    {
                        InventoryControl.MergeItems();
                    }
                }
                if (typ == 1) // Storage -> Storage
                {
                    byte str_1 = packet.ReadUInt8();
                    byte str_2 = packet.ReadUInt8();
                    ushort count = packet.ReadUInt16();
                    int index_1 = Data.storageslot.IndexOf(str_1);
                    int index_2 = Data.storageslot.IndexOf(str_2);
                    if (index_2 == -1)
                    {
                        // No item, Moving !
                        Data.storageslot[index_1] = str_2;
                    }
                    else
                    {
                        //The item exist !
                        if (Data.storagetype[index_1] == Data.storagetype[index_2])
                        {
                            // Items Are Same, Merge It !
                            if (Data.storagecount[index_2] == Items_Info.items_maxlist[Items_Info.itemstypelist.IndexOf(Data.storagetype[index_2])])
                            {
                                // Items Are Maxed, Move It !
                                Data.storageslot[index_1] = str_2;
                                Data.storageslot[index_2] = str_1;
                            }
                            else
                            {
                                // Items Are Same, Merge It !
                                if (Data.storagecount[index_1] == count)
                                {
                                    // Merged Everything, Delete The First Item !
                                    Data.storagecount[index_2] += count;
                                    string name = Items_Info.itemsnamelist[Items_Info.itemstypelist.IndexOf(Data.storagetype[index_1])];
                                    //Globals.MainWindow.storage_list.Items.Remove(name);

                                    Data.storageid.RemoveAt(index_1);
                                    Data.storagetype.RemoveAt(index_1);
                                    Data.storagecount.RemoveAt(index_1);
                                    Data.storageslot.RemoveAt(index_1);
                                    Data.storagedurability.RemoveAt(index_1);
                                    Data.storagelevel.RemoveAt(index_1);
                                    Data.storagename.RemoveAt(index_1);
                                }
                                else
                                {
                                    // Merged Not Everything, Recalculate Quantity !
                                    Data.storagecount[index_2] += count;
                                    Data.storagecount[index_1] -= count;
                                }
                            }
                        }
                        else
                        {
                            // Items Are Different, Move It !
                            Data.storageslot[index_1] = str_2;
                            Data.storageslot[index_2] = str_1;
                        }
                    }
                }
                if (typ == 2) // From INV to STR
                {
                    byte slot_inv = packet.ReadUInt8();
                    byte slot_bnk = packet.ReadUInt8();
                    int index = Data.inventoryslot.IndexOf(slot_inv);
                    Data.storageid.Add(Data.inventoryid[index]);
                    Data.storagetype.Add(Data.inventorytype[index]);
                    Data.storageslot.Add(slot_bnk);
                    Data.storagecount.Add(Data.inventorycount[index]);
                    Data.storagedurability.Add(Data.inventorydurability[index]);
                    Data.storagelevel.Add(Data.inventorylevel[index]);
                    Data.storagename.Add(Data.inventoryname[index]);
                    Data.strmaxdurability.Add(Data.maxdurability[index]);

                    Data.inventoryid.RemoveAt(index);
                    Data.inventorytype.RemoveAt(index);
                    Data.inventoryslot.RemoveAt(index);
                    Data.inventorycount.RemoveAt(index);
                    Data.inventorydurability.RemoveAt(index);
                    Data.inventorylevel.RemoveAt(index);
                    Data.inventoryname.RemoveAt(index);
                    Data.maxdurability.RemoveAt(index);

                    Globals.MainWindow.UpdateInventory();
                    //Globals.MainWindow.storage_list.Items.Clear();
                    for (int i = 0; i < Data.storageid.Count; i++)
                    {
                        uint id = Data.storageid[i];
                        string name = Items_Info.itemsnamelist[Items_Info.itemsidlist.IndexOf(id)];
                        //Globals.MainWindow.storage_list.Items.Add(name);
                    }
                    ItemsCount.CountManager();
                    StorageControl.StorageManager(Spawns.NPCID[Spawns.NPCType.IndexOf(Data.selectednpctype)]);
                }
                if (typ == 3) // From STR to INV
                {
                    byte slot_bnk = packet.ReadUInt8();
                    byte slot_inv = packet.ReadUInt8();
                    int index = Data.storageslot.IndexOf(slot_bnk);

                    Data.inventoryid.Add(Data.storageid[index]);
                    Data.inventorytype.Add(Data.storagetype[index]);
                    Data.inventoryslot.Add(slot_inv);
                    Data.inventorycount.Add(Data.storagecount[index]);
                    Data.inventorydurability.Add(Data.storagedurability[index]);
                    Data.inventorylevel.Add(Data.storagelevel[index]);
                    Data.inventoryname.Add(Data.storagename[index]);
                    Data.maxdurability.Add(Data.strmaxdurability[index]);

                    Data.storageid.RemoveAt(index);
                    Data.storagetype.RemoveAt(index);
                    Data.storageslot.RemoveAt(index);
                    Data.storagecount.RemoveAt(index);
                    Data.storagedurability.RemoveAt(index);
                    Data.storagelevel.RemoveAt(index);
                    Data.storagename.RemoveAt(index);
                    Data.strmaxdurability.RemoveAt(index);

                    Globals.MainWindow.UpdateInventory();
                    //Globals.MainWindow.storage_list.Items.Clear();
                    for (int i = 0; i < Data.storageid.Count; i++)
                    {
                        uint id = Data.storageid[i];
                        string name = Items_Info.itemsnamelist[Items_Info.itemsidlist.IndexOf(id)];
                        //Globals.MainWindow.storage_list.Items.Add(name);
                    }
                    ItemsCount.CountManager();
                }
                if (typ == 6) // PICKED ITEM
                {                    
                    byte slot = packet.ReadUInt8();
                    if (slot == 254)
                    {
                        packet.ReadUInt32();
                    }
                    else
                    {
                        packet.ReadUInt32();
                        uint item_id = packet.ReadUInt32();
                        int index = Items_Info.itemsidlist.IndexOf(item_id);
                        string type = Items_Info.itemstypelist[index];
                        if (type.StartsWith("ITEM_CH") || type.StartsWith("ITEM_EU"))
                        {
                            byte item_plus = packet.ReadUInt8();
                            packet.ReadUInt64();
                            uint durability = packet.ReadUInt32();
                            byte blueamm = packet.ReadUInt8();
                            for (int i = 0; i < blueamm; i++)
                            {
                                packet.ReadUInt8();
                                packet.ReadUInt16();
                                packet.ReadUInt32();
                                packet.ReadUInt8();
                            }
                            Data.inventoryid.Add(item_id);
                            Data.inventorytype.Add(type);
                            Data.inventoryslot.Add(slot);
                            Data.inventorycount.Add(1);
                            Data.inventorydurability.Add(durability);
                            Data.inventorylevel.Add(Items_Info.itemslevellist[index]);
                            Data.inventoryname.Add(Items_Info.itemsnamelist[index]);
                            Data.maxdurability.Add(Items_Info.itemsdurabilitylist[index]);
                            Globals.MainWindow.UpdateInventory();
                        }
                        else
                        {
                            ushort count = packet.ReadUInt16();
                            int indexas = Data.inventoryslot.IndexOf(slot);
                            if (indexas != -1)
                            {
                                Data.inventorycount[indexas] = count;
                            }
                            else
                            {
                                Data.inventoryid.Add(item_id);
                                Data.inventorytype.Add(type);
                                Data.inventoryslot.Add(slot);
                                Data.inventorydurability.Add(0);
                                Data.inventorycount.Add(count);
                                Data.inventorylevel.Add(Items_Info.itemslevellist[index]);
                                Data.inventoryname.Add(Items_Info.itemsnamelist[index]);
                                Data.maxdurability.Add(Items_Info.itemsdurabilitylist[index]);

                                Globals.MainWindow.UpdateInventory();
                            }
                            ItemsCount.CountManager();
                        }
                    }
                }
                if (typ == 7)
                {
                    byte slot = packet.ReadUInt8();
                    int index = Data.inventoryslot.IndexOf(slot);
                    Data.inventoryid.RemoveAt(index);
                    Data.inventorytype.RemoveAt(index);
                    Data.inventoryslot.RemoveAt(index);
                    Data.inventorycount.RemoveAt(index);
                    Data.inventorydurability.RemoveAt(index);
                    Data.inventorylevel.RemoveAt(index);
                    Data.inventoryname.RemoveAt(index);
                    Data.maxdurability.RemoveAt(index);

                    Globals.MainWindow.UpdateInventory();
                    ItemsCount.CountManager();
                }
                if (typ == 8) // Shop => Inventory
                {
                    byte tab = packet.ReadUInt8();
                    byte slot = packet.ReadUInt8();
                    byte count = packet.ReadUInt8();
                    #region Finding Item Info
                    uint item_id = 0;
                    for (int i = 0; i < Data.ShopTabData.Length; i++)
                    {
                        if (Data.ShopTabData[i].StoreName.Replace("STORE_", "NPC_") == Data.selectednpctype)
                        {
                            item_id = Items_Info.itemsidlist[Items_Info.itemstypelist.IndexOf(Data.ShopTabData[i].Tab[tab].ItemType[slot])];
                            break;
                        }
                    }
                    string item_type = Items_Info.itemstypelist[Items_Info.itemsidlist.IndexOf(item_id)];
                    #endregion
                    if (count == 1)
                    {                     
                        byte inv_slot = packet.ReadUInt8();
                        ushort inv_count = packet.ReadUInt16();

                        Packet du = new Packet((ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_INVENTORYMOVEMENT);
                        du.WriteUInt8(1);
                        du.WriteUInt8(6);
                        du.WriteUInt8(inv_slot);
                        du.WriteUInt32(0);
                        du.WriteUInt32(item_id);
                        if (item_type.StartsWith("ITEM_CH") == false && item_type.StartsWith("ITEM_EU") == false)
                        {
                            du.WriteUInt16(inv_count);
                        }
                        else
                        {
                            du.WriteUInt8(0);
                            du.WriteUInt64(0);
                            du.WriteUInt32(Items_Info.itemsdurabilitylist[Items_Info.itemsidlist.IndexOf(item_id)]);
                            du.WriteUInt8(0);
                            du.WriteUInt16(1);
                            du.WriteUInt16(2);
                        }
                        Proxy.ag_local_security.Send(du);

                        Data.inventoryid.Add(item_id);
                        Data.inventorytype.Add(item_type);
                        Data.inventoryslot.Add(inv_slot);
                        Data.inventorycount.Add(inv_count);
                        Data.inventorydurability.Add(0);
                        Data.inventorylevel.Add(Items_Info.itemslevellist[Items_Info.itemsidlist.IndexOf(item_id)]);
                        Data.inventoryname.Add(Items_Info.itemsnamelist[Items_Info.itemsidlist.IndexOf(item_id)]);
                        Data.maxdurability.Add(Items_Info.itemsdurabilitylist[Items_Info.itemsidlist.IndexOf(item_id)]);
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            byte inv_slot = packet.ReadUInt8();

                            Packet du = new Packet((ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_INVENTORYMOVEMENT);
                            du.WriteUInt8(0x01);
                            du.WriteUInt8(0x06);
                            du.WriteUInt8(inv_slot);
                            du.WriteUInt32(0x00000000);
                            du.WriteUInt32(item_id);
                            if (item_type.StartsWith("ITEM_CH") == false && item_type.StartsWith("ITEM_EU") == false)
                            {
                                du.WriteUInt16(1);
                            }
                            else
                            {
                                du.WriteUInt8(0x00);
                                du.WriteUInt64(0x0000000000000000);
                                du.WriteUInt32(Items_Info.itemsdurabilitylist[Items_Info.itemsidlist.IndexOf(item_id)]);
                                du.WriteUInt8(0x00);
                                du.WriteUInt16(1);
                                du.WriteUInt16(2);
                            }
                            Proxy.ag_local_security.Send(du);

                            Data.inventoryid.Add(item_id);
                            Data.inventorytype.Add(item_type);
                            Data.inventoryslot.Add(inv_slot);
                            Data.inventorycount.Add(1);
                            Data.inventorydurability.Add(0);
                            Data.inventorylevel.Add(Items_Info.itemslevellist[Items_Info.itemsidlist.IndexOf(item_id)]);
                            Data.inventoryname.Add(Items_Info.itemsnamelist[Items_Info.itemsidlist.IndexOf(item_id)]);
                            Data.maxdurability.Add(Items_Info.itemsdurabilitylist[Items_Info.itemsidlist.IndexOf(item_id)]);
                        }
                    }
                    Globals.MainWindow.UpdateInventory();
                    ItemsCount.CountManager();
                    System.Threading.Thread.Sleep(500);
                    BuyControl.BuyManager(Spawns.NPCID[Spawns.NPCType.IndexOf(Data.selectednpctype)]);
                }
                if (typ == 9) // Inventory -> Shop
                {
                    byte inv_slot = packet.ReadUInt8();
                    ushort count = packet.ReadUInt16();

                    int index = Data.inventoryslot.IndexOf(inv_slot);
                    ushort real_count = Data.inventorycount[index];

                    if (count == real_count)
                    {
                        //Sold Everything - Delete Item
                        Data.inventoryid.RemoveAt(index);
                        Data.inventorytype.RemoveAt(index);
                        Data.inventoryslot.RemoveAt(index);
                        Data.inventorycount.RemoveAt(index);
                        Data.inventorydurability.RemoveAt(index);
                        Data.inventorylevel.RemoveAt(index);
                        Data.inventoryname.RemoveAt(index);
                        Data.maxdurability.RemoveAt(index);
                    }
                    else
                    {
                        //Reduce count of item
                        ushort new_count = (ushort)(real_count - count);
                        Data.inventorycount[index] = new_count;
                    }
                    Globals.MainWindow.UpdateInventory();
                    ItemsCount.CountManager();
                    SellControl.SellManager(Spawns.NPCID[Spawns.NPCType.IndexOf(Data.selectednpctype)]);
                }
                if (typ == 16) // From Pet Inventory To Pet Inventory
                {
                    uint petid = packet.ReadUInt32();
                    byte pet_1 = packet.ReadUInt8();
                    byte pet_2 = packet.ReadUInt8();
                    ushort count = packet.ReadUInt16();

                    for (int i = 0; i < Pets.CharPets.Count; i++)
                    {
                        if (Pets.CharPets[i].UniqueID == petid)
                        {
                            if (Pets.CharPets[i].Inventory[pet_1].type == Pets.CharPets[i].Inventory[pet_2].type)
                            {
                                // Items Are Same, Merge It !
                                if (Pets.CharPets[i].Inventory[pet_2].count == Items_Info.items_maxlist[Items_Info.itemstypelist.IndexOf(Pets.CharPets[i].Inventory[pet_2].type)])
                                {
                                    // Items Are Maxed, Move It !
                                    Pets.Inventory_ inv_temp = Pets.CharPets[i].Inventory[pet_1];
                                    Pets.CharPets[i].Inventory[pet_1] = Pets.CharPets[i].Inventory[pet_2];
                                    Pets.CharPets[i].Inventory[pet_2] = inv_temp;
                                }
                                else
                                {
                                    if (Pets.CharPets[i].Inventory[pet_1].count == count)
                                    {
                                        // Merged Everything, Delete The First Item !
                                        Pets.CharPets[i].Inventory[pet_2].count += count;
                                        string name = Items_Info.itemsnamelist[Items_Info.itemstypelist.IndexOf(Pets.CharPets[i].Inventory[pet_1].type)];
                                        //Globals.MainWindow.pet_inv_list.Items.Remove(name);
                                        Pets.CharPets[i].Inventory[pet_1] = new Pets.Inventory_();
                                    }
                                    else
                                    {
                                        // Merged Not Everything, Recalculate Quantity !
                                        Pets.CharPets[i].Inventory[pet_2].count += count;
                                        Pets.CharPets[i].Inventory[pet_1].count -= count;
                                    }
                                }
                            }
                            else
                            {
                                // Items Are Different, Move It !
                                Pets.Inventory_ inv_temp = Pets.CharPets[i].Inventory[pet_1];
                                Pets.CharPets[i].Inventory[pet_1] = Pets.CharPets[i].Inventory[pet_2];
                                Pets.CharPets[i].Inventory[pet_2] = inv_temp;
                            }
                            break;
                        }
                    }
                }
                if (typ == 26) // From Pet Inventory To Inventory
                {
                    uint pet_id = packet.ReadUInt32();
                    for (int i = 0; i < Pets.CharPets.Count; i++)
                    {
                        if (pet_id == Pets.CharPets[i].UniqueID)
                        {
                            byte pet_slot = packet.ReadUInt8();
                            byte inv_slot = packet.ReadUInt8();
                            Data.inventoryid.Add(Pets.CharPets[i].Inventory[pet_slot].id);
                            Data.inventorytype.Add(Pets.CharPets[i].Inventory[pet_slot].type);
                            Data.inventoryslot.Add(inv_slot);
                            Data.inventorydurability.Add(Pets.CharPets[i].Inventory[pet_slot].durability);
                            Data.inventorycount.Add(Pets.CharPets[i].Inventory[pet_slot].count);
                            Data.inventorylevel.Add(Pets.CharPets[i].Inventory[pet_slot].level);
                            Data.inventoryname.Add(Pets.CharPets[i].Inventory[pet_slot].name);
                            Data.maxdurability.Add(Pets.CharPets[i].Inventory[pet_slot].maxdurability);
                            string name = Items_Info.itemsnamelist[Items_Info.itemstypelist.IndexOf(Pets.CharPets[i].Inventory[pet_slot].type)];
                            Globals.MainWindow.UpdateInventory();
                            //Globals.MainWindow.pet_inv_list.Items.Remove(name);
                            Pets.CharPets[i].Inventory[pet_slot] = new Pets.Inventory_();
                            break;
                        }
                    }
                }
                if (typ == 27) // From Inventory To Pet Inventory
                {
                    uint pet_id = packet.ReadUInt32();
                    for (int i = 0; i < Pets.CharPets.Count; i++)
                    {
                        if (pet_id == Pets.CharPets[i].UniqueID)
                        {
                            byte inv_slot = packet.ReadUInt8();
                            byte pet_slot = packet.ReadUInt8();
                            int inv_index = Data.inventoryslot.IndexOf(inv_slot);
                            Pets.CharPets[i].Inventory[pet_slot].id = Data.inventoryid[inv_index];
                            Pets.CharPets[i].Inventory[pet_slot].type = Data.inventorytype[inv_index];
                            Pets.CharPets[i].Inventory[pet_slot].slot = pet_slot;
                            Pets.CharPets[i].Inventory[pet_slot].durability = Data.inventorydurability[inv_index];
                            Pets.CharPets[i].Inventory[pet_slot].count = Data.inventorycount[inv_index];
                            Pets.CharPets[i].Inventory[pet_slot].level = Data.inventorylevel[inv_index];
                            Pets.CharPets[i].Inventory[pet_slot].name = Data.inventoryname[inv_index];
                            Pets.CharPets[i].Inventory[pet_slot].maxdurability = Data.maxdurability[inv_index];
                            //Globals.MainWindow.pet_inv_list.Items.Add(Items_Info.itemsnamelist[Items_Info.itemstypelist.IndexOf(Data.inventorytype[inv_index])]);
                            Data.inventoryid.RemoveAt(inv_index);
                            Data.inventorytype.RemoveAt(inv_index);
                            Data.inventorycount.RemoveAt(inv_index);
                            Data.inventorydurability.RemoveAt(inv_index);
                            Data.inventoryslot.RemoveAt(inv_index);
                            Data.inventorylevel.RemoveAt(inv_index);
                            Data.inventoryname.RemoveAt(inv_index);
                            Data.maxdurability.RemoveAt(inv_index);

                            Globals.MainWindow.UpdateInventory();
                            break;
                        }
                    }
                }
                if (typ != 8)
                {
                    Proxy.ag_local_security.Send(vienas);
                }
            }
            if (check == 2)
            {
                byte check1 = packet.ReadUInt8();
                switch (check1)
                {
                    case 0x03:
                        //Unknown
                        break;
                }
            }
        }

        public static uint itemid = 0;
        public static byte intendtomove = 0;
        public static void Inventory_Update2(Packet packet)
        {
            uint ID = packet.ReadUInt32();
            if (ID == Character.UniqueID)
            {
                byte slot = packet.ReadUInt8();
                if (slot < 9)
                {
                    itemid = packet.ReadUInt32();
                    for (byte i = 13; i < Character.InventorySize; i++)
                    {                      
                        if (Data.inventoryslot.IndexOf(i) == -1)
                        {
                            intendtomove = i;
                            break;
                        }
                    }
                }
            }
        }

        public static void MergeItems()
        {
            List<string> mergetypewaiting = new List<string>();
            List<byte> mergeslotwaiting = new List<byte>();
            Data.loopaction = "merge";
            for (int i = 0; i < Data.inventoryid.Count; i++)
            {
                System.Threading.Thread.Sleep(5);
                if (!Data.inventorytype[i].StartsWith("ITEM_CH") && !Data.inventorytype[i].StartsWith("ITEM_EU") && Data.inventoryslot[i] >= 13)
                {
                    if (Data.inventorycount[i] < Items_Info.items_maxlist[Items_Info.itemstypelist.IndexOf(Data.inventorytype[i])])
                    {
                        if (mergetypewaiting.IndexOf(Data.inventorytype[i]) != -1)
                        {
                            //There are another not merged same type item
                            //Merge IT
                            Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYMOVEMENT);
                            NewPacket.WriteUInt8(0);
                            NewPacket.WriteUInt8(Data.inventoryslot[i]);
                            NewPacket.WriteUInt8(mergeslotwaiting[mergetypewaiting.IndexOf(Data.inventorytype[i])]);
                            NewPacket.WriteUInt8((byte)Data.inventorycount[i]); // Count
                            NewPacket.WriteUInt8(0);
                            Proxy.ag_remote_security.Send(NewPacket);
                            break;
                        }
                        else
                        {
                            mergetypewaiting.Add(Data.inventorytype[i]);
                            mergeslotwaiting.Add(Data.inventoryslot[i]);
                        }
                    }
                }
                if (i + 1 >= Data.inventoryid.Count && Data.loop == true)
                {
                    LoopControl.WalkScript();
                }
            }
        }

        /*public static void ItemFixed(Packet packet)
        {
            if (packet.ReadUInt8() == 1)
            {
                if (Data.loop && Data.bot)
                {
                    Training.currentlyselected = 0;
                    System.Threading.Thread.Sleep(1000);
                    InventoryControl.MergeItems();
                }
            }
        }*/
    }
}

