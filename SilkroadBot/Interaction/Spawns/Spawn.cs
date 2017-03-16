using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeoBot
{
    class Spawn
    {
        public static void SingleSpawn(Packet packet)
        {
            uint model = packet.ReadUInt32();
            int index = Mobs_Info.mobsidlist.IndexOf(model);         
            int itemsindex = Items_Info.itemsidlist.IndexOf(model);
            if (itemsindex != -1)
            {
                #region ItemsParsing
                DataParser.ParseItem(packet, itemsindex);
                #endregion

            }
            if (index != -1)
            {

                #region MobsParsing
                if (Mobs_Info.mobstypelist[index].StartsWith("MOB"))
                {
                    DataParser.ParseMob(packet, index);
                }
                #endregion

                #region PetsParsing
                else if (Mobs_Info.mobstypelist[index].StartsWith("COS"))
                {
                    DataParser.ParsePets(packet, index);
                }
                #endregion

                #region NPCParsing
                else if (Mobs_Info.mobstypelist[index].StartsWith("NPC"))
                {
                    DataParser.ParseNPC(packet, index);
                }
                #endregion

                #region CharParsing
                else if (Mobs_Info.mobstypelist[index].StartsWith("CHAR"))
                {
                    DataParser.ParseChar(packet, index);
                }
                #endregion

                #region PortalParsing
                else if (Mobs_Info.mobstypelist[index].Contains("_GATE"))
                {
                    DataParser.ParsePortal(packet, index);
                }
                #endregion

                #region StructureParsing
                else if (Mobs_Info.mobstypelist[index].StartsWith("STRUCTURE"))
                {
                    DataParser.ParseStructure(packet, index);
                }
                #endregion

                #region OtherParsing
                else
                {
                    DataParser.ParseOthers(packet, index);
                }
                #endregion

            }
        }

        public static void GroupSpawns(Packet packet)
        {
            try
            {
                if (Data.groupespawninfo == 1)
                {
                    for (int i = 0; i < Data.groupespawncount; i++)
                    {
                        #region DetectType
                        uint model = packet.ReadUInt32();
                        int index = Mobs_Info.mobsidlist.IndexOf(model);
                        int itemsindex = Items_Info.itemsidlist.IndexOf(model);
                        #endregion

                        if (itemsindex != -1)
                        {
                            #region ItemsParsing
                            DataParser.ParseItem(packet, itemsindex);
                            #endregion
                        }
                        if (index != -1)
                        {                          
                            #region MobsParsing
                            if (Mobs_Info.mobstypelist[index].StartsWith("MOB"))
                            {
                                DataParser.ParseMob(packet, index);
                            }
                            #endregion

                            #region PetsParsing
                            else if (Mobs_Info.mobstypelist[index].StartsWith("COS"))
                            {
                                DataParser.ParsePets(packet, index);
                            }
                            #endregion

                            #region NPCParsing
                            else if (Mobs_Info.mobstypelist[index].StartsWith("NPC"))
                            {
                                DataParser.ParseNPC(packet, index);
                            }
                            #endregion

                            #region CharParsing
                            else if (Mobs_Info.mobstypelist[index].StartsWith("CHAR"))
                            {
                                DataParser.ParseChar(packet, index);
                            }
                            #endregion

                            #region PortalParsing
                            else if (Mobs_Info.mobstypelist[index].Contains("_GATE"))
                            {
                                DataParser.ParsePortal(packet, index);
                            }
                            #endregion

                            #region StructureParsing
                            else if (Mobs_Info.mobstypelist[index].StartsWith("STRUCTURE"))
                            {
                                DataParser.ParseStructure(packet, index);
                            }
                            #endregion

                            #region OtherParsing
                            else
                            {
                                DataParser.ParseOthers(packet, index);
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Data.groupespawncount; i++)
                    {
                        #region Deselect
                        uint id = packet.ReadUInt32();
                        if (id == Training.currentlyselected)
                        {
                            Training.currentlyselected = 0;
                        }
                        #endregion

                        #region Items
                        foreach (Item item in Item.SpawnItem)
                        {
                            if (id == item.UniqueID)
                            {
                                Item.SpawnItem.Remove(item);
                                break;
                            }
                        }
                        foreach (Item item in Item.PickableItem)
                        {
                            if (id == item.UniqueID)
                            {
                                Item.PickableItem.Remove(item);
                                break;
                            }
                        }
                        #endregion

                        #region Chars
                        foreach (OtherCharacter character in OtherCharacter.Characters)
                        {
                            if (id == character.UniqueID)
                            {
                                OtherCharacter.Characters.Remove(character);
                                break;
                            }
                        }
                        #endregion

                        #region Mobs
                        foreach (Monster monster in Monster.SpawnMob)
                        {
                            if (monster.UniqueID == id)
                            {
                                Monster.SpawnMob.Remove(monster);
                                Stuck.DeleteMob(id);
                                if (id == Training.monster_id)
                                {
                                    Training.monster_selected = false;
                                    Training.monster_id = 0;
                                    //Pause the character and call the Logic!

                                    //Select New Monster, cause selected just disapeared  
                                    LogicControl.Manager();
                                }
                                break;
                            }
                        }
                        #endregion

                        #region Pets
                        for (int z = 0; z < Spawns.pets.Length; z++)
                        {
                            if (id == Spawns.pets[z].id)
                            {
                                Spawns.pets[z] = new Spawns.Pets_();
                                break;
                            }
                        }
                        #endregion
                    }
                }
            }
            catch { }
        }

        public static void SingleDeSpawn(Packet packet)
        {
            #region Deselect
            uint id = packet.ReadUInt32();
            if (id == Training.currentlyselected)
            {
                Training.currentlyselected = 0;
            }
            #endregion

            #region Items
            foreach (Item item in Item.SpawnItem)
            {
                if (id == item.UniqueID)
                {
                    Item.SpawnItem.Remove(item);
                    break;
                }
            }
            foreach (Item item in Item.PickableItem)
            {
                if (id == item.UniqueID)
                {
                    Item.PickableItem.Remove(item);
                    break;
                }
            }
            #endregion

            #region Pets
            for (int z = 0; z < Spawns.pets.Length; z++)
            {
                if (id == Spawns.pets[z].id)
                {
                    Spawns.pets[z] = new Spawns.Pets_();
                    break;
                }
            }
            #endregion

            #region Chars
            foreach (OtherCharacter character in OtherCharacter.Characters)
            {
                if (id == character.UniqueID)
                {
                    OtherCharacter.Characters.Remove(character);
                    break;
                }
            }
            #endregion

            #region Mobs
            foreach (Monster monster in Monster.SpawnMob)
            {
                if (monster.UniqueID == id)
                {                   
                    Monster.SpawnMob.Remove(monster);
                    Stuck.DeleteMob(id);
                    if (id == Training.monster_id)
                    {
                        Training.monster_selected = false;
                        Training.monster_id = 0;                       
                        //Pause the character and call the Logic!
                       
                        //Select New Monster, cause selected just disapeared 
                        LogicControl.Manager();
                    }
                    break;
                }
            }
            #endregion
        }
    }
}