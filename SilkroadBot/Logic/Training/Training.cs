using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LeoBot
{
    class Training
    {
        public static bool monster_selected = false;

        public static byte monster_type = 0;
        public static string monster_type_string = "";
        public static string monster_name = "";

        public static int distance = 0;

        public static uint currentlyselected = 0;
        public static uint monster_id = 0;

        public static double X = 0;
        public static double Y = 0;

        public static Monster MonsterFilter(List<Monster> moblist, int plevel)
        {
            Monster selected = null;
            int distance = 100;
            foreach (Monster monster in moblist.ToList())
            {
                System.Threading.Thread.Sleep(2);
                if ((MonsterControl.CheckAvoidMob(monster) == false) && (MonsterControl.CheckRange(monster) == true) && (MonsterControl.CheckStatus(monster) == true))
                {
                    if ((monster.Priority == plevel) && (monster.Distance < distance))
                    {
                        distance = monster.Distance;
                        selected = monster;
                    }
                }
            }
            return selected;
        }

        public static void SelectMonster()
        {
            {
                if (monster_selected)
                {
                    Berserk.CheckBerserk(Training.monster_id, monster_type_string);
                    Skills.CheckSkills();
                }
                else 
                {
                    uint id = 0;                   
                    Monster selecting = null;
                    int prioritylevel = MonsterControl.CheckPriorityLevel(Monster.SpawnMob);
                    while (prioritylevel >= 0)
                    {
                        selecting = MonsterFilter(Monster.SpawnMob, prioritylevel);
                        if (selecting == null)
                        {                           
                            prioritylevel--;
                        }
                        else
                        {
                            id = selecting.UniqueID;
                            monster_type = selecting.MobType;
                            monster_type_string = selecting.MobTypename;
                            monster_name = selecting.AdvanceName;
                            distance = selecting.Distance;
                            X = selecting.X;
                            Y = selecting.Y;
                            break;
                        }
                    }
                    if (id == 0)
                    {
                        Walking.walking_circle = true;
                        Walking.walking_path = true;
                     
                        if (Walking.running == false)
                        {
                            Walking.WalkManager();   
                        }                                                    
                    }
                    else
                    {
                        monster_id = id;

                        if (Walking.walking_center)
                        {
                            Walking.walking_center = false;
                        }
                        if (Walking.walking_circle)
                        {
                            Walking.walking_circle = false;
                        }
                        if (Walking.walking_path)
                        {
                            Walking.walking_path = false;
                        }

                        Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTSELECT);
                        NewPacket.WriteUInt32(monster_id);
                        Proxy.ag_remote_security.Send(NewPacket);

                        monster_selected = true;
                    }
                }                 
            }
        }

        public static bool normal_attack = false;
        public static void Attack(uint UniqueID)
        {
            normal_attack = true;

            Packet NewPacket = new Packet(0x7074);
            NewPacket.WriteUInt8(1);
            NewPacket.WriteUInt8(1);
            NewPacket.WriteUInt8(1);
            NewPacket.WriteUInt32(UniqueID);
            Proxy.ag_remote_security.Send(NewPacket);               
        }

        public static void Selected(Packet packet)
        {
            try
            {
                if (packet.ReadUInt8() == 1)
                {
                    Training.currentlyselected = packet.ReadUInt32();

                    try
                    {
                        Data.selectednpctype = Spawns.NPCType[Spawns.NPCID.IndexOf(Training.currentlyselected)];
                    }
                    catch { }

                    #region Loop
                    if (Data.loop && Data.bot)
                    {
                        if (Data.loopaction == "storage")
                        {
                            if (Data.storageopened == 0)
                            {
                                StorageControl.GetStorageItems(Spawns.NPCID[Spawns.NPCType.IndexOf(Data.selectednpctype)]);
                            }
                            else
                            {
                                StorageControl.OpenStorage1();
                            }
                        }
                        if (Data.loopaction == "blacksmith")
                        {
                            SellControl.SellManager(Spawns.NPCID[Spawns.NPCType.IndexOf(Data.selectednpctype)]);
                        }
                        if (Data.loopaction == "stable" || Data.loopaction == "accessory" || Data.loopaction == "potion")
                        {
                            BuyControl.BuyManager(Spawns.NPCID[Spawns.NPCType.IndexOf(Data.selectednpctype)]);
                        }
                    }
                    #endregion

                    else
                    {
                        if (Data.bot && monster_selected)
                        {
                            PortConfigs.TrainWindow.Label(PortConfigs.TrainWindow.monstername, monster_name);
                            PortConfigs.TrainWindow.Label(PortConfigs.TrainWindow.monstertype, monster_type_string);

                            if (packet.ReadUInt8() == 0x01)
                            {
                                if (Data.bot)
                                {
                                    if (monster_selected)
                                    {
                                        Movement.stuck_count = 0;
                                    }
                                }
                                uint hp = packet.ReadUInt32();
                                PortConfigs.TrainWindow.Label(PortConfigs.TrainWindow.monsterHP, Convert.ToString(hp));
                                if (hp > 0)
                                {
                                    if (currentlyselected == monster_id)
                                    {
                                        Skills.GhostWalk(distance, X, Y);
                                        if (Globals.MainWindow.Checked(Globals.MainWindow.petattack) == true)
                                        {
                                            BotAction.AttackWithPet();
                                        }
                                        Berserk.CheckBerserk(Training.monster_id, monster_type_string);
                                        /*if ((Globals.MainWindow.buffs_list3.Items.Count != 0 || Globals.MainWindow.buffs_list4.Items.Count != 0) && monster_type > 1)
                                        {
                                            Buffas.buff_waiting = true;
                                        }*/
                                        LogicControl.Manager();
                                    }
                                }
                                else
                                {
                                    Training.monster_selected = false;
                                    Training.monster_id = 0;

                                    LogicControl.Manager();
                                }
                            }
                        }
                    }
                }
                else
                {
                    LeoBot.Stuck.AddMob(Training.monster_id, 3);

                    Training.monster_selected = false;
                    Training.monster_id = 0;

                    LogicControl.Manager();
                }
            }
            catch (Exception)
            {
                Training.monster_selected = false;
                Training.monster_id = 0;

                LogicControl.Manager();
            }
        }
    }
}
