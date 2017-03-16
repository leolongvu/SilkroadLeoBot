using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoBot
{
    class MonsterControl
    {
        public static bool CheckRange(Monster monster)
        {
            try
            {
                if (Globals.MainWindow.Checked(Globals.MainWindow.norange) == true)
                {
                    return true;
                }
                else
                {
                    if (Globals.MainWindow.Checked(Globals.MainWindow.walkaround) == true)
                    {                       
                        float dist_train = Math.Abs(monster.X - Convert.ToInt64(Globals.MainWindow.ReadText(Globals.MainWindow.x_setbox))) + Math.Abs(monster.Y - Convert.ToInt64(Globals.MainWindow.ReadText(Globals.MainWindow.y_setbox)));
                        if (dist_train <= Convert.ToInt64(Globals.MainWindow.ReadText(Globals.MainWindow.trainingrangebox)))
                        {
                            return true;
                        }
                    }
                    else if (Globals.MainWindow.Checked(Globals.MainWindow.walkpath) == true)
                    {
                        float dist_train = Math.Abs((monster.X - Character.X)) + Math.Abs((monster.Y - Character.Y));
                        if (dist_train <= Convert.ToInt64(Globals.MainWindow.trainingrangebox.Text))
                        {
                            return true;
                        }
                    }
                }               
                return false;
            }
            catch { return false; }
        }

        public static bool CheckStatus(Monster monster)
        {
            try
            {
                if (monster.Status == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch { return false; }
        }

        public static int CheckPriorityLevel(List<Monster> SpawnMob)
        {
            int prioritylevel = 0;
            foreach (Monster monster in SpawnMob)
            {
                if (monster.Priority > prioritylevel)
                    prioritylevel = monster.Priority;
            }
            return prioritylevel;
        }

        public static void Refresh(Packet packet)
        {
            byte flag1 = packet.ReadUInt8();
            byte flag2 = packet.ReadUInt8();
            if (flag1 == 2 && (flag2 == 0 || flag2 == 1))
            {
                LogicControl.Manager();
                /*if (Skills.skill_casted == true || PickupControl.picked == true || Training.normal_attack == true)
                {
                    Training.normal_attack = false;
                    Skills.skill_casted = false;
                    PickupControl.picking = false;
                    PickupControl.picked = false;
                }*/
            }    
            /*else if (flag1 == 2 && flag2 == 1)
            {
                Training.normal_attack = true;
            }*/         
            else if (flag1 == 3 && flag2 == 1)
            {
                //LogicControl.Manager();
            }
        }

        public static void MonsterAction(Packet packet)
        {   
            uint id = packet.ReadUInt32();
            string type = packet.ReadUInt8().ToString("X2") + packet.ReadUInt8().ToString("X2");
            if (type == "0002" || type == "0003")
            {
                if (id != Character.UniqueID)
                {                
                    foreach (Monster monster in Monster.SpawnMob)
                    {
                        if (id == monster.UniqueID)
                        {
                            if (id != Training.monster_id)
                            {
                                Monster.SpawnMob.Remove(monster);
                                Stuck.DeleteMob(id);                              
                                break;
                            }                               
                        }
                    }
                }
            }
        }

        public static void RecheckPriority()
        {
            foreach (Monster monster in Monster.SpawnMob.ToList())
            {
                System.Threading.Thread.Sleep(2);
                switch (monster.MobType)
                {
                    case 0:
                        monster.Priority = DataParser.NormalPriority;
                        break;
                    case 1:
                        monster.Priority = DataParser.ChampionPriority;
                        break;
                    case 3:
                        monster.Priority = DataParser.UniquePriority;
                        break;
                    case 4:
                        monster.Priority = DataParser.GiantPriority;
                        break;
                    case 5:
                        monster.Priority = DataParser.TitanPriority;
                        break;
                    case 6:
                        monster.Priority = DataParser.ElitePriority;
                        break;
                    case 7:
                        monster.Priority = DataParser.ElitePriority;
                        break;
                    case 16:
                        monster.Priority = DataParser.PartyPriority;
                        break;
                    case 17:
                        monster.Priority = DataParser.PartyChampionPriority;
                        break;
                    case 20:
                        monster.Priority = DataParser.PartyGiantPriority;
                        break; 
                }
            }
        }

        public static bool CheckAvoidMob(Monster monster)
        {
            /*//Snow slave      
            if (ID == 28811)
            {
                if (Globals.MainWindow.ignor_snow_slaves.Checked)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //Dimension Pillars
            else if (ID >= 36034 && ID <= 36040)
            {
                if (Program.f1.ignor_dimension_pillar.Checked)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }*/
            if (monster.Priority < 0)
            {
                return true;
            }
            else
            {
                foreach (string Type in Monster.AvoidMob)
                {
                    if (monster.Type == Type)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void NPCSelect(Packet packet)
        {
            Packet vienas = packet;
            if (Data.bot && Data.loop)
            {
                string type = Data.selectednpctype;
                uint id = Spawns.NPCID[Spawns.NPCType.IndexOf(type)];
                if (type.Contains("WAREHOUSE"))
                {
                    StorageControl.StorageManager(id);
                }
            }
            else
            {
                Proxy.ag_local_security.Send(vienas);
            }
        }

        public static void NPCDeselect(Packet packet)
        {
            if (packet.ReadUInt8() == 1)
            {
                if (Data.loopaction == "storage" || Data.loopaction == "blacksmith" || Data.loopaction == "stable" || Data.loopaction == "accessory" || Data.loopaction == "potion")
                {
                    Training.currentlyselected = 0;
                    LoopControl.WalkScript();
                }
            }
        }
    }
}
