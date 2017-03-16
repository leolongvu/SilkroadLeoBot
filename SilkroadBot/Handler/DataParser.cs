using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoBot
{
    class DataParser
    {

        public static int NormalPriority = 8;
        public static int ChampionPriority = 7;
        public static int GiantPriority = 6;
        public static int PartyPriority = 5;
        public static int PartyChampionPriority = 4;
        public static int PartyGiantPriority = 3;
        public static int TitanPriority = 2;
        public static int ElitePriority = 1;
        public static int UniquePriority = 0;
        public static void ParseMob(Packet packet, int index)
        {
            try
            {
                uint UniqueID = packet.ReadUInt32(); //ModID
                byte xsec = packet.ReadUInt8();
                byte ysec = packet.ReadUInt8();
             
                float xcoord = packet.ReadSingle();
                packet.ReadSingle();
                float ycoord = packet.ReadSingle();

                packet.ReadUInt8(); // Unknown
                packet.ReadUInt8(); // Unknown

                byte move = packet.ReadUInt8(); // Flag            
                packet.ReadUInt8(); // Running
                if (move == 1)
                {
                    xsec = packet.ReadUInt8();
                    ysec = packet.ReadUInt8();
                    xcoord = packet.ReadUInt16();
                    packet.ReadUInt16();
                    ycoord = packet.ReadUInt16();
                }
                else
                {
                    packet.ReadUInt8(); // Unknown
                    packet.ReadUInt16(); // Unknwon
                }

                float X = Convert.ToSingle((xsec - 135) * 192 + (xcoord / 10));
                float Y = Convert.ToSingle((ysec - 92) * 192 + (ycoord / 10));
                int Distance = (int)(Math.Abs((X - Character.X)) + Math.Abs((Y - Character.Y)));

                byte alive = packet.ReadUInt8(); // Alive
                packet.ReadUInt8(); // Unknown
                packet.ReadUInt8(); // Unknown
                packet.ReadUInt8(); // Zerk Active
                packet.ReadSingle(); // Walk Speed
                packet.ReadSingle(); // Run Speed
                packet.ReadSingle(); // Zerk Speed
                packet.ReadUInt32(); // Unknown
                byte type = packet.ReadUInt8();
                int priority = 0;
                switch (type)
                {
                    case 0:
                        priority = NormalPriority;
                        break;
                    case 1:
                        priority = ChampionPriority;
                        break;
                    case 3:
                        priority = UniquePriority;
                        break;
                    case 4:
                        priority = GiantPriority;
                        break;
                    case 5:
                        priority = TitanPriority;
                        break;
                    case 6:
                        priority = ElitePriority;
                        break;
                    case 7:
                        priority = ElitePriority;
                        break;
                    case 16:
                        priority = PartyPriority;
                        break;
                    case 17:
                        priority = PartyChampionPriority;
                        break;
                    case 20:
                        priority = PartyGiantPriority;
                        break;
                }

                Monster NewMonster = new Monster(UniqueID, Mobs_Info.mobstypelist[index], type, Berserk.GetTypeName(type),
                    Mobs_Info.mobsnamelist[index], Mobs_Info.mobslevellist[index], priority, 0, 
                    Monster.ModNameList[Monster.ModTypeList.IndexOf(Mobs_Info.mobstypelist[index])]);
                NewMonster.X = X;
                NewMonster.Y = Y;
                NewMonster.Distance = Distance;

                if (alive == 1)
                    Monster.SpawnMob.Add(NewMonster);
            }
            catch { }
        }

        public static void ParseItem(Packet packet, int itemsindex)
        {
            try
            {
                string type = Items_Info.itemstypelist[itemsindex];
                if (type.StartsWith("ITEM_ETC_GOLD"))
                {
                    packet.ReadUInt32(); // Ammount
                }
                if (type.StartsWith("ITEM_QSP") || type.StartsWith("ITEM_QNO"))
                {
                    packet.ReadAscii(); // Name
                }
                if (type.StartsWith("ITEM_CH") || type.StartsWith("ITEM_EU"))
                {
                    packet.ReadUInt8(); // Plus
                }
                uint id = packet.ReadUInt32(); // ID

                packet.ReadUInt16(); //Redion ID
                packet.ReadSingle(); //X
                packet.ReadSingle(); //Z
                packet.ReadSingle(); //Y

                packet.ReadUInt16(); //POS

                Item NewItem = new Item(id, 0, type, Items_Info.itemsnamelist[itemsindex]);
                Item.SpawnItem.Add(NewItem);
                if (!type.StartsWith("ITEM_QSP") || !type.StartsWith("ITEM_QNO"))
                {
                    if (packet.ReadUInt8() == 1) // Owner exist
                    {
                        if (packet.ReadUInt32() == Character.AccountID) // Owner ID
                        {
                            Item.PickableItem.Add(NewItem);
                            PickupControl.there_is_pickable = true;
                        }
                    }
                }              
                packet.ReadUInt8(); //Rarity

                if (packet.Opcode == 0x3015)
                {
                    byte Source = packet.ReadUInt8();
                    switch (Source)
                    {
                        case 5:
                            packet.ReadUInt32();//Dropper ID - Monster
                            break;
                        case 6:
                            packet.ReadUInt32();//Dropper ID - Player
                            break;
                    }
                }
            }
            catch { }
        }

        public static void ParseNPC(Packet packet, int index)
        {
            try
            {
                uint id = packet.ReadUInt32();
                int countid = 0;
                foreach (uint checkid in Spawns.NPCID)
                {
                    if (id == checkid)
                    {
                        countid++;
                        break;
                    }                      
                }
                if (countid == 0)
                {
                    Spawns.NPCID.Add(id);
                    Spawns.NPCType.Add(Mobs_Info.mobstypelist[index]);
                }
                byte xsec = packet.ReadUInt8();
                byte ysec = packet.ReadUInt8();
                float xcoord = packet.ReadSingle();
                packet.ReadSingle();
                float ycoord = packet.ReadSingle();

                packet.ReadUInt8();
                packet.ReadUInt8();

                byte move = packet.ReadUInt8(); // Flag            
                packet.ReadUInt8(); // Running
                if (move == 1)
                {
                    xsec = packet.ReadUInt8();
                    ysec = packet.ReadUInt8();
                    xcoord = packet.ReadUInt16();
                    packet.ReadUInt16();
                    ycoord = packet.ReadUInt16();
                }
                else
                {
                    packet.ReadUInt8(); // Unknown
                    packet.ReadUInt16(); // Unknwon
                }

                packet.ReadUInt8(); //StageFlag
                packet.ReadUInt8(); //Unknown
                packet.ReadUInt8(); //Action
                packet.ReadUInt8(); //Status
                packet.ReadSingle();
                packet.ReadSingle();
                packet.ReadSingle();
                packet.ReadUInt8();
                byte talkflag = packet.ReadUInt8();
                if (talkflag == 2)
                {
                    byte talkoptioncount = packet.ReadUInt8();
                    for (int i = 0; i < talkoptioncount; i++)
                    {
                        packet.ReadUInt8();
                    }
                }
            }
            catch { }
        }

        public static void ParseChar(Packet packet, int index)
        {
            try
            {
                int trade = 0;
                packet.ReadUInt8(); // Volume/Height
                packet.ReadUInt8(); // BerserkLevel
                packet.ReadUInt8(); // Icons
                packet.ReadUInt8(); // AutoInvestEXP
                packet.ReadUInt8(); // InventorySize
                OtherCharacter.Items_[] Items = new OtherCharacter.Items_[40];
                int items_count = packet.ReadUInt8();
                for (int a = 0; a < items_count; a++)
                {
                    uint itemid = packet.ReadUInt32();
                    Items[a].ID = itemid;
                    int itemindex = Items_Info.itemsidlist.IndexOf(itemid);                   
                    Items[a].Name = Items_Info.itemsnamelist[itemindex];
                    if (Items_Info.itemstypelist[itemindex].StartsWith("ITEM_CH") || Items_Info.itemstypelist[itemindex].StartsWith("ITEM_EU") || Items_Info.itemstypelist[itemindex].StartsWith("ITEM_FORT") || Items_Info.itemstypelist[itemindex].StartsWith("ITEM_ROC_CH") || Items_Info.itemstypelist[itemindex].StartsWith("ITEM_ROC_EU"))
                    {
                        byte plus = packet.ReadUInt8(); // Item Plus
                    }                
                    if (Items_Info.itemstypelist[itemindex].StartsWith("ITEM_EU_M_TRADE") || Items_Info.itemstypelist[itemindex].StartsWith("ITEM_EU_F_TRADE") || Items_Info.itemstypelist[itemindex].StartsWith("ITEM_CH_M_TRADE") || Items_Info.itemstypelist[itemindex].StartsWith("ITEM_CH_W_TRADE"))
                    {
                        trade = 1;
                    }
                }
                packet.ReadUInt8(); // Max Avatars Slot
                int avatar_count = packet.ReadUInt8();
                for (int a = 0; a < avatar_count; a++)
                {
                    uint avatarid = packet.ReadUInt32();
                    byte plus = packet.ReadUInt8(); // Avatar Plus
                }               
                byte mask = packet.ReadUInt8();
                if (mask == 1)
                {
                    uint id = packet.ReadUInt32();
                    string type = Mobs_Info.mobstypelist[Mobs_Info.mobsidlist.IndexOf(id)];
                    if (type.StartsWith("CHAR"))
                    {
                        packet.ReadUInt8();
                        byte count = packet.ReadUInt8();
                        for (int i = 0; i < count; i++)
                        {
                            packet.ReadUInt32();
                        }
                    }
                }
                uint UniqueID = packet.ReadUInt32();

                ushort xsec = packet.ReadUInt8();
                ushort ysec = packet.ReadUInt8();
                
                float xcoord = packet.ReadSingle();
                packet.ReadSingle();
                float ycoord = packet.ReadSingle();

                packet.ReadUInt8();
                packet.ReadUInt8();

                byte move = packet.ReadUInt8(); // Flag 
                packet.ReadUInt8(); // Running
                if (move == 1)
                {
                    xsec = packet.ReadUInt8();
                    ysec = packet.ReadUInt8();
                    xcoord = packet.ReadUInt16();
                    packet.ReadUInt16();
                    ycoord = packet.ReadUInt16();
                }
                else
                {
                    packet.ReadUInt8(); // Unknown
                    packet.ReadUInt16(); // Unknwon
                }

                byte StateFlag = packet.ReadUInt8();    
                packet.ReadUInt8(); //Action: 0 = None, 2 = Walking, 3 = Running, 4 = Sitting
                packet.ReadUInt8(); //Status: 0 = None, 2 = ??*GrowthPet, 3 = Invincible, 4 = Invisible
                packet.ReadUInt8();

                packet.ReadSingle(); // Walking speed
                float speed = packet.ReadSingle(); // Running speed 
                packet.ReadSingle(); // Berserk speed

                int active_skills = packet.ReadUInt8(); // Buffs count 
                OtherCharacter.Buffs_[] Buffs = new OtherCharacter.Buffs_[100];
                for (int a = 0; a < active_skills; a++)
                {
                    uint skillid = packet.ReadUInt32();
                    int buffindex = Skills_Info.skillsidlist.IndexOf(skillid);
                    Buffs[a].Name = Skills_Info.skillsnamelist[buffindex];
                    string type = Skills_Info.skillstypelist[buffindex];
                    Buffs[a].TempID = packet.ReadUInt32(); //Temp ID
                    if (type.StartsWith("SKILL_EU_CLERIC_RECOVERYA_GROUP") || type.StartsWith("SKILL_EU_BARD_BATTLAA_GUARD") || type.StartsWith("SKILL_EU_BARD_DANCEA") || type.StartsWith("SKILL_EU_BARD_SPEEDUPA_HITRATE"))
                    {
                        packet.ReadUInt8();
                    }
                }
                string name = packet.ReadAscii();
                byte jobtype = packet.ReadUInt8(); // Job type                
                packet.ReadUInt8(); // Job level
                packet.ReadUInt8();
                int cnt = packet.ReadUInt8();
                packet.ReadUInt8();
                if (cnt == 1)
                {
                    packet.ReadUInt32();
                }
                packet.ReadUInt8(); // Unknown
                byte stall_flag = packet.ReadUInt8(); // Stall flag, 4 => Stalling    
                packet.ReadUInt8(); // Unknown                
                
                string guild = packet.ReadAscii(); // Guild   
                if (trade != 1)
                {
                    packet.ReadUInt32(); //Guild ID

                    string nickname = packet.ReadAscii();
                    packet.ReadUInt32(); //Guild CurCrestRev

                    packet.ReadUInt32(); //UnionID
                    packet.ReadUInt32();

                    packet.ReadUInt8();
                    packet.ReadUInt8();

                    if (stall_flag == 4)
                    {
                        packet.ReadUInt32();
                    }
                    packet.ReadUInt8();                   
                }

                OtherCharacter Characters = new OtherCharacter(UniqueID, name, guild, Buffs, Items, StateFlag);
                OtherCharacter.Characters.Add(Characters);
            }
            catch { }
        }

        public static void ParsePortal(Packet packet, int index)
        {
            try
            {
                uint id = packet.ReadUInt32();
                if (Spawns.NPCID.IndexOf(id) == -1)
                {
                    Spawns.NPCID.Add(id);
                    Spawns.NPCType.Add(Mobs_Info.mobstypelist[index]);
                }                
                byte xsec = packet.ReadUInt8();
                byte ysec = packet.ReadUInt8();
                
                float xcoord = packet.ReadSingle();
                packet.ReadSingle();
                float ycoord = packet.ReadSingle();

                packet.ReadUInt16(); // Position
                packet.ReadUInt8();
                packet.ReadUInt8();
                packet.ReadUInt8();
                byte ukn4 = packet.ReadUInt8();
                if (ukn4 == 1)
                {
                    packet.ReadUInt32();
                    packet.ReadUInt32();
                }
                else if (ukn4 == 6)
                {
                    packet.ReadAscii();
                    packet.ReadUInt32();
                }
            }
            catch { }
        }

        public static void ParseStructure(Packet packet, int index)
        {
            packet.ReadUInt32(); //HP
            packet.ReadUInt32(); //RefEvent Structure ID
            packet.ReadUInt16(); //State
            packet.ReadUInt32(); //ID
            byte xsec = packet.ReadUInt8();
            byte ysec = packet.ReadUInt8();
            packet.ReadUInt8();
            packet.ReadUInt8();
            byte move = packet.ReadUInt8(); // Flag            
            packet.ReadUInt8(); // Running
            if (move == 1)
            {
                xsec = packet.ReadUInt8();
                ysec = packet.ReadUInt8();
                packet.ReadUInt16();
                packet.ReadUInt16();
                packet.ReadUInt16();
            }
            else
            {
                packet.ReadUInt8(); // Unknown
                packet.ReadUInt16(); // Unknwon
            }

            byte alive = packet.ReadUInt8(); // Alive
            packet.ReadUInt8(); // Unknown
            packet.ReadUInt8(); // Unknown
            packet.ReadUInt8(); // Unknown

            packet.ReadSingle(); // Walking speed
            packet.ReadSingle(); // Running speed
            packet.ReadSingle(); // Berserk speed
            packet.ReadUInt8();
            packet.ReadUInt8();
            packet.ReadUInt32();
        }

        public static void ParsePets(Packet packet, int index)
        {
            try
            {
                int s_index = 0;
                for (int i = 0; i < Spawns.pets.Length; i++)
                {
                    if (Spawns.pets[i].type == null)
                    {
                        s_index = i;
                        break;
                    }
                }
                uint pet_id = packet.ReadUInt32(); // PET ID
                Spawns.pets[s_index].id = pet_id;
                Spawns.pets[s_index].type = Mobs_Info.mobstypelist[index];
                byte xsec = packet.ReadUInt8();
                byte ysec = packet.ReadUInt8();
 
                float xcoord = packet.ReadSingle();
                packet.ReadSingle();
                float ycoord = packet.ReadSingle();

                packet.ReadUInt8();
                packet.ReadUInt8();

                byte move = packet.ReadUInt8(); // Flag    
                packet.ReadUInt8(); // Running
                if (move == 1)
                {
                    xsec = packet.ReadUInt8();
                    ysec = packet.ReadUInt8();
                    xcoord = packet.ReadUInt16();
                    packet.ReadUInt16();
                    ycoord = packet.ReadUInt16();
                }
                else
                {
                    packet.ReadUInt8(); // Unknown
                    packet.ReadUInt16(); // Unknwon
                }

                Spawns.pets[s_index].x = Convert.ToInt32((xsec - 135) * 192 + (xcoord / 10));
                Spawns.pets[s_index].y = Convert.ToInt32((ysec - 92) * 192 + (ycoord / 10));

                packet.ReadUInt8(); //Stage Flag
                packet.ReadUInt8();
                packet.ReadUInt8(); //Action
                packet.ReadUInt8(); //Status
                packet.ReadSingle();
                Spawns.pets[s_index].speed = packet.ReadSingle();
                packet.ReadSingle(); //Berserk Speed
                packet.ReadUInt16();

                string type = Mobs_Info.mobstypelist[index];
                int count = 0;
                foreach (string pettype in Data.Types.attack_types)
                {
                    if (type.StartsWith(pettype))
                    {
                        count++;
                        break;
                    }
                }
                if (Data.Types.grab_types.IndexOf(type) != -1 || (count > 0) || type.StartsWith("COS_T"))
                {
                    packet.ReadAscii(); //Name
                    packet.ReadAscii(); //Owner Name      
                    packet.ReadUInt32();
                    packet.ReadUInt16();
                }
                /*if ((count > 0) || type.StartsWith("COS_T"))
                {
                    packet.ReadUInt8(); //Murder Flag
                }*/                              
            }
            catch { }
        }

        public static void ParseOthers(Packet packet, int index)
        {
            try
            {
                packet.ReadUInt16();
                packet.ReadUInt32();
                packet.ReadUInt32();
                packet.ReadUInt16();
                packet.ReadUInt32();
                packet.ReadUInt32();
                packet.ReadUInt32();
                packet.ReadUInt16();
            }
            catch { }
        }
    }
}
