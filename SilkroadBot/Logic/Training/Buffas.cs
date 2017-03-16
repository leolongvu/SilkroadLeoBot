using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace LeoBot
{
    class Buffas
    {
        public static bool buff_waiting = false;
        public static bool changing_weapon = false;
        public static int min_mp_require = 200000;

        #region Buff Add
        public static void BuffAdd(Packet packet)
        {
            {
                uint charid = packet.ReadUInt32();
                if (charid == Character.UniqueID)
                {
                    uint id = packet.ReadUInt32();
                    for (int i = 0; i < Skill.Skills.Count; i++)
                    {
                        if (Skill.Skills[i].UniqueID == id)
                        {
                            uint temp = packet.ReadUInt32();
                            Skill.Skills[i].bufftemp = temp;
                            Skill.Skills[i].Status = 1;
                            Skill.Skills[i].BuffWaiting = 0;
                            if (Skill.Skills[i].Type.Contains("SKILL_CH_COLD_BINGBYEOK") || Skill.Skills[i].Type.Contains("SKILL_CH_FIRE_HWABYEOK"))
                            {
                                if (Skill.Skills[i].Type.Contains("SKILL_CH_COLD_BINGBYEOK"))
                                {
                                    Skills.buffcount1++;
                                    Skills.buff1 = id;
                                }
                                if (Skill.Skills[i].Type.Contains("SKILL_CH_FIRE_HWABYEOK"))
                                {
                                    Skills.buffcount2++;
                                    Skills.buff2 = id;
                                }
                            }
                            else
                            {
                                if (Skill.Skills[i].Bufftime < Skill.Skills[i].Cooldown)
                                {
                                    Skill.Skills[i].CooldownTime.Interval = Skill.Skills[i].Cooldown;
                                    Skill.Skills[i].CooldownTime.Elapsed += new ElapsedEventHandler((sender, e) => Buffs_Elapsed(sender, e, Skill.Skills[i]));
                                    Skill.Skills[i].CooldownTime.Start();
                                    Skill.Skills[i].CooldownTime.AutoReset = false;
                                    Skill.Skills[i].CooldownTime.Enabled = true;
                                }
                                else
                                {
                                    Skill.Skills[i].CooldownTime.Interval = Skill.Skills[i].Bufftime - 100;
                                    Skill.Skills[i].CooldownTime.Elapsed += new ElapsedEventHandler((sender, e) => Buffs_Elapsed(sender, e, Skill.Skills[i]));
                                    Skill.Skills[i].CooldownTime.Start();
                                    Skill.Skills[i].CooldownTime.AutoReset = false;
                                    Skill.Skills[i].CooldownTime.Enabled = true;
                                }
                            }
                            if (!((Skill.Skills[i].Type.Equals(Skill.Imbue[0].Type)) || (Skill.Skills[i].Type.Equals(Skill.PartyImbue[0].Type))))
                            {
                                System.Threading.Thread.Sleep(Skill.Skills[i].Casttime);
                                LogicControl.Manager();
                            }
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        static void Buffs_Elapsed(object sender, ElapsedEventArgs e, Skill skill)
        {
            skill.Status = 0;
            if (Globals.MainWindow.buff.Items.IndexOf(skill.Name) != -1)
            {
                buff_waiting = true;
            }
        }

        #region Buff Deletion
        public static void BuffDell(Packet packet)
        {
            {
                byte packet_type = packet.ReadUInt8();
                if (packet_type == 1)
                {
                    uint temp = packet.ReadUInt32();
                    foreach (Skill skill in Skill.Skills)
                    {
                        if (skill.bufftemp == temp)
                        {
                            if ((skill.Type.Equals(Skill.Imbue[0].Type) || (skill.Type.Equals(Skill.PartyImbue[0].Type))) && Training.monster_selected == true)
                            {
                                Skills.ImbueCast();
                            }
                            if (skill.Type.Contains("SKILL_CH_COLD_BINGBYEOK") == true || skill.Type.Contains("SKILL_CH_FIRE_HWABYEOK") == true)
                            {
                                if (skill.Type.Contains("SKILL_CH_COLD_BINGBYEOK"))
                                {
                                    Skills.buffcount1--;
                                    skill.Status = 0;
                                }
                                if (skill.Type.Contains("SKILL_CH_FIRE_HWABYEOK"))
                                {
                                    Skills.buffcount2--;
                                    skill.Status = 0;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        public static byte correct2nslot = 0;
        #region Buffer
        public static void BuffChecker()
        {
            if (Globals.MainWindow.secondbuff.Items.Count != 0)
            {
                for (int i = 0; i < Skill.Skills.Count; i++)
                {
                    //if (Skill.Skills[i].BuffWaiting == 1)
                    {
                        if (Skill.Skills[i].bufftype == 3) // At First We need to check 2nd weapon buffs :)
                        {
                            if (Skill.Skills[i].Status == 0)
                            {
                                //Checking If User Have 2nd Weapon !
                                if (Data.s_wep_name != "" && Data.s_wep_name != null)
                                {
                                    int index = Data.inventorytype.IndexOf(Data.s_wep_name);
                                    if (index == -1)
                                    {
                                        Data.s_wep_name = "";
                                        Globals.MainWindow.UpdateLogs("Could not find second weapon!");
                                        break;
                                    }
                                    else
                                    {
                                        if (Data.s_wep_name.Contains("SHIELD"))
                                            correct2nslot = 7;
                                        else correct2nslot = 6;
                                    }
                                }

                                //Trying To Cast Buff                          
                                if (Skill.Skills[i].MinMP <= Character.CurrentMP) //Checking if enough MP and it's possible to cast buff (cooldown).
                                {
                                    System.Threading.Thread.Sleep(1);
                                    int item_index = Data.inventorytype.IndexOf(Data.s_wep_name); //Getting Second Weapon Index
                                    if (Data.inventoryslot[item_index] != correct2nslot) //Checking If Second Weapon Is In Weapon Slot
                                    {
                                        System.Threading.Thread.Sleep(1);
                                        //Weapon Isn't In Weapon Slot, Changing The Weapon !!!
                                        changing_weapon = true;
                                        Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYMOVEMENT);
                                        NewPacket.WriteUInt8(0);
                                        NewPacket.WriteUInt8((byte)Data.inventoryslot[item_index]);
                                        if (Data.s_wep_name.Contains("SHIELD"))
                                            NewPacket.WriteUInt8(7);
                                        else
                                            NewPacket.WriteUInt8(6);
                                        NewPacket.WriteUInt16(0x0000);
                                        Proxy.ag_remote_security.Send(NewPacket);
                                        break;
                                    }
                                    else
                                    {
                                        //Weapon Is In Weapon Slot, Casting The Buff !!!
                                        Skills.CastSkill(Skill.Skills[i], 0);
                                        break;
                                    }
                                }
                                else
                                {
                                    if (Skill.Skills[i].MinMP > Character.CurrentMP)
                                    {
                                        min_mp_require = Skill.Skills[i].MinMP;
                                    }
                                }
                            }
                            if (i + 1 >= Skill.Skills.Count)
                            {
                                //---------------------------------------------------------------------------------------------------------------------------------------------------------
                                for (int a = 0; a < Skill.Skills.Count; a++)
                                {
                                    //if (Skill.Skills[a].BuffWaiting == 1)
                                    {
                                        if (Skill.Skills[a].bufftype == 1) // We need to check 1st weapon buffs :)
                                        {
                                            if (Skill.Skills[a].Status == 0)
                                            {
                                                //Checking If User Have 1st Weapon !
                                                if (Data.f_wep_name != "" && Data.f_wep_name != null)
                                                {
                                                    int index = Data.inventorytype.IndexOf(Data.f_wep_name);
                                                    if (index == -1)
                                                    {
                                                        Data.f_wep_name = "";
                                                        Globals.MainWindow.UpdateLogs("Could not find first weapon!");
                                                        break;
                                                    }
                                                }
                                                System.Threading.Thread.Sleep(2); //MicroSleep For Less CPU Usage

                                                //Trying To Cast Buff
                                                if (Skill.Skills[a].MinMP <= Character.CurrentMP) //Checking if enough MP and it's possible to cast buff (cooldown).
                                                {
                                                    int item_index = Data.inventorytype.IndexOf(Data.f_wep_name); //Getting First Weapon Index
                                                    if (Data.inventoryslot[item_index] != 6) //Checking If First Weapon Is In Weapon Slot
                                                    {
                                                        System.Threading.Thread.Sleep(1);
                                                        //Weapon Isn't In Weapon Slot, Changing The Weapon !!!
                                                        changing_weapon = true;
                                                        Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYMOVEMENT);
                                                        NewPacket.WriteUInt8(0);
                                                        NewPacket.WriteUInt8((byte)Data.inventoryslot[item_index]);
                                                        NewPacket.WriteUInt8(6);
                                                        NewPacket.WriteUInt16(0x0000);
                                                        Proxy.ag_remote_security.Send(NewPacket);
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        //Weapon Is In Weapon Slot, Casting The Buff !!!
                                                        Skills.CastSkill(Skill.Skills[a], 0);
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    if (Skill.Skills[a].MinMP > Character.CurrentMP)
                                                    {
                                                        min_mp_require = Skill.Skills[a].MinMP;
                                                    }
                                                }
                                            }

                                        }
                                    }
                                    if (a + 1 >= Skill.Skills.Count)
                                    {
                                        //---------------------------------------------------------------------------------------------------------------------------------------------------------
                                        buff_waiting = false;
                                        LogicControl.Manager();
                                        //---------------------------------------------------------------------------------------------------------------------------------------------------------
                                    }
                                    //---------------------------------------------------------------------------------------------------------------------------------------------------------
                                }
                            }
                        }
                    }
                }
            }
            else //No 2nd weapon buffs, just cast buff
            {
                for (int a = 0; a < Skill.Skills.Count; a++)
                {
                    //if (Skill.Skills[a].BuffWaiting == 1)
                    {
                        if (Skill.Skills[a].bufftype == 1)
                        {
                            //Trying To Cast Buff              
                            if (Skill.Skills[a].Status == 0 && Skill.Skills[a].MinMP <= Character.CurrentMP) //Checking if enough MP and it's possible to cast buff (cooldown).
                            {
                                Skills.CastSkill(Skill.Skills[a], 0);
                                break;
                            }
                            else
                            {
                                if (Skill.Skills[a].MinMP > Character.CurrentMP)
                                {
                                    min_mp_require = Skill.Skills[a].MinMP;
                                }
                            }
                        }
                    }
                    if (a + 1 >= Skill.Skills.Count)
                    {
                        //---------------------------------------------------------------------------------------------------------------------------------------------------------
                        buff_waiting = false;
                        LogicControl.Manager();
                        //---------------------------------------------------------------------------------------------------------------------------------------------------------
                    }
                }
            }
        }
        #endregion
    }
}
