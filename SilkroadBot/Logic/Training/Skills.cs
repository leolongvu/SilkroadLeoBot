using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
namespace LeoBot
{
    class Skills
    {
        public static byte buffcount1 = 0;
        public static byte buffcount2 = 0;
        public static uint buff1 = 0;
        public static uint buff2 = 0;

        public static void SkillAdd(Packet packet)
        {
            byte result = packet.ReadUInt8();

            if (result == 0x01)
            {
                byte type = packet.ReadUInt8();
                if (type == 0x00)
                {
                    //Buff Add!
                }
                else if (type == 0x02)
                {
                    if (packet.ReadUInt8() == 0x30)
                    {
                        uint skill_id = packet.ReadUInt32();
                        uint attacker_id = packet.ReadUInt32();
                        if (attacker_id == Character.UniqueID)
                        {
                            //Skill casted !
                            packet.ReadUInt32();
                            packet.ReadUInt32();
                            Checker(skill_id);

                            if (Data.bot)
                            {
                                if (Training.monster_selected)
                                {
                                    Movement.stuck_count = 0;
                                }
                            }
                        }
                        else
                        {
                            packet.ReadUInt32();
                            uint obj_id = packet.ReadUInt32();
                            if ((obj_id == Character.UniqueID) || (obj_id == Data.char_attackpetid && Globals.MainWindow.Checked(Globals.MainWindow.protectpet) == true))
                            {
                                if (Data.bot && !Data.loop)
                                {
                                    if (Training.monster_selected == false && Buffas.buff_waiting == false)
                                    {
                                        foreach (Monster monster in Monster.SpawnMob)
                                        {
                                            if (monster.UniqueID == attacker_id)
                                            {
                                                if (Walking.running == true)
                                                {
                                                    Walking.running = false;
                                                }

                                                Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTSELECT);
                                                NewPacket.WriteUInt32(attacker_id);
                                                Proxy.ag_remote_security.Send(NewPacket);

                                                Training.monster_id = attacker_id;
                                                Training.monster_selected = true;
                                                Training.monster_type = monster.MobType;
                                                Training.monster_type_string = monster.MobTypename;
                                                Training.monster_name = monster.AdvanceName;
                                                Training.distance = monster.Distance;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (attacker_id != Training.monster_id)
                                        {
                                            foreach (Monster monster in Monster.SpawnMob)
                                            {
                                                if ((monster.UniqueID == attacker_id) && (monster.Priority < 9))
                                                {
                                                    monster.Status = 0;
                                                    monster.Priority = 9;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (result == 0x02)
            {
                byte type = packet.ReadUInt8();
                switch (type)
                {
                    case 16:
                        if (Training.monster_selected && Training.monster_id != 0)
                        {
                            Stuck.AddMob(Training.monster_id, 10);

                            Training.monster_selected = false;
                            Training.monster_id = 0;

                            LogicControl.Manager();
                        }
                        break;
                    case 5:
                        //LogicControl.Manager();
                        //Skill errors
                        break;
                    case 6:
                        //LogicControl.Manager();
                        //Skill errors
                        break;
                }
            }
        }

        public static void Checker(uint id)
        {
            for (int i = 0; i < Skill.Skills.Count; i++)
            {
                if (Skill.Skills[i].UniqueID == id)
                {
                    Skill.Skills[i].Status = 1;
                    int interval = Skill.Skills[i].Cooldown - 100;
                    if (interval <= 0)
                    {
                        interval = 1;
                    }
                    Skill.Skills[i].CooldownTime.Interval = interval;
                    Skill.Skills[i].CooldownTime.Elapsed += new ElapsedEventHandler((sender, e) => Skills_Elapsed(sender, e, Skill.Skills[i]));
                    Skill.Skills[i].CooldownTime.Start();
                    Skill.Skills[i].CooldownTime.AutoReset = false;
                    Skill.Skills[i].CooldownTime.Enabled = true;
                    break;
                }
            }
        }

        static void Skills_Elapsed(object sender, ElapsedEventArgs e, Skill skill)
        {
            skill.Status = 0;

            if (Training.normal_attack == true && Training.monster_selected == true)
            {
                Training.normal_attack = false;
                //Skills.skill_casting = false;
                //Skills.skill_casted = false;

                /*Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTACTION);
                NewPacket.WriteUInt8(2);
                Proxy.ag_remote_security.Send(NewPacket);

                LogicControl.Manager();*/
            }
        }

        public static void ImbueCast()
        {
            if (Training.monster_type == 0 || Training.monster_type == 1 || Training.monster_type == 4 || Globals.MainWindow.partyimbue.Items.Count == 0)
            {
                if (Globals.MainWindow.generalimbue.Items.Count != 0)
                {
                    Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTACTION);
                    NewPacket.WriteUInt8(1);
                    NewPacket.WriteUInt8(4);
                    NewPacket.WriteUInt32(Skill.Imbue[0].UniqueID);
                    NewPacket.WriteUInt8(0);
                    Proxy.ag_remote_security.Send(NewPacket);
                }
            }
            else
            {
                Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTACTION);
                NewPacket.WriteUInt8(1);
                NewPacket.WriteUInt8(4);
                NewPacket.WriteUInt32(Skill.PartyImbue[0].UniqueID);
                NewPacket.WriteUInt8(0);
                Proxy.ag_remote_security.Send(NewPacket);
            }
        }

        public static void CheckSkills()
        {
            {
                if (Data.bot && Training.monster_selected == true)
                {
                    if (Globals.MainWindow.secondbuff.Items.Count == 0)
                    {
                        if (Training.monster_type == 0 || Training.monster_type == 1 || Training.monster_type == 4 || Globals.MainWindow.partyattack.Items.Count == 0)
                        {
                            if (Globals.MainWindow.generalattack.Items.Count == 0)
                            {
                                ImbueCast();
                                Training.Attack(Training.monster_id);                               
                            }
                            else
                            {
                                for (int i = 0; i < Globals.MainWindow.generalattack.Items.Count; i++)
                                {
                                    if (Skill.Skills[Skill.AttackSkills[i].index].Status == 0 && Skill.AttackSkills[i].MinMP <= Character.CurrentMP)
                                    {
                                        ImbueCast();
                                        CastSkill(Skill.AttackSkills[i], Training.monster_id);
                                        break;                                            
                                    }
                                    if (i + 1 == Globals.MainWindow.generalattack.Items.Count) /*&& (Globals.MainWindow.no_normal_attack.unchecked)*/
                                    {
                                        ImbueCast();
                                        Training.Attack(Training.monster_id);
                                        //No more skills, normal attack
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < Globals.MainWindow.partyattack.Items.Count; i++)
                            {
                                if (Skill.Skills[Skill.PartyAttackSkills[i].index].Status == 0 && Skill.PartyAttackSkills[i].MinMP <= Character.CurrentMP)
                                {
                                    ImbueCast();
                                    CastSkill(Skill.PartyAttackSkills[i], Training.monster_id);
                                    break;
                                }
                                if (i + 1 == Globals.MainWindow.partyattack.Items.Count) /*&& (Globals.MainWindow.no_normal_attack.unchecked)*/
                                {
                                    ImbueCast();
                                    Training.Attack(Training.monster_id);
                                    //No more skills, normal attack
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Training.monster_type == 0 || Training.monster_type == 1 || Training.monster_type == 4 || Globals.MainWindow.partyattack.Items.Count == 0)
                        {
                            if (Globals.MainWindow.generalattack.Items.Count == 0)
                            {
                                if (Data.f_wep_name != "" && Data.f_wep_name != null)
                                {
                                    int index = Data.inventorytype.IndexOf(Data.f_wep_name);
                                    if (index == -1)
                                    {
                                        Data.f_wep_name = "";
                                    }
                                }
                                System.Threading.Thread.Sleep(1);
                                if (Data.f_wep_name != "")
                                {
                                    int index1 = Data.inventorytype.IndexOf(Data.f_wep_name);
                                    if (Data.inventoryslot[index1] != 6)
                                    {
                                        System.Threading.Thread.Sleep(1);
                                        Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYMOVEMENT);
                                        NewPacket.WriteUInt8(0);
                                        NewPacket.WriteUInt8((byte)Data.inventoryslot[index1]);
                                        NewPacket.WriteUInt8(6);
                                        NewPacket.WriteUInt16(0x0000);
                                        Proxy.ag_remote_security.Send(NewPacket);
                                    }
                                    else
                                    {
                                        ImbueCast();
                                        Training.Attack(Training.monster_id);
                                    }
                                }
                                else
                                {
                                    Data.bot = false;
                                    Data.loop = false;
                                    Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                    Globals.MainWindow.UpdateLogs("Bot Stops! Cannot Find 1st Weapon");
                                }
                            }
                            else
                            {
                                for (int i = 0; i < Globals.MainWindow.generalattack.Items.Count; i++)
                                {
                                    if (Data.f_wep_name != "" && Data.f_wep_name != null)
                                    {
                                        int index = Data.inventorytype.IndexOf(Data.f_wep_name);
                                        if (index == -1)
                                        {
                                            Data.f_wep_name = "";
                                        }
                                    }
                                    System.Threading.Thread.Sleep(1);
                                    if (Data.f_wep_name != "")
                                    {
                                        if (Skill.Skills[Skill.AttackSkills[i].index].Status == 0 && Skill.AttackSkills[i].MinMP <= Character.CurrentMP)
                                        {
                                            System.Threading.Thread.Sleep(1);
                                            int index1 = Data.inventorytype.IndexOf(Data.f_wep_name);
                                            if (Data.inventoryslot[index1] != 6)
                                            {
                                                System.Threading.Thread.Sleep(1);
                                                Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYMOVEMENT);
                                                NewPacket.WriteUInt8(0);
                                                NewPacket.WriteUInt8((byte)Data.inventoryslot[index1]);
                                                NewPacket.WriteUInt8(6);
                                                NewPacket.WriteUInt16(0x0000);
                                                Proxy.ag_remote_security.Send(NewPacket);
                                                break;
                                            }
                                            else
                                            {
                                                ImbueCast();
                                                CastSkill(Skill.AttackSkills[i], Training.monster_id);
                                                break;
                                            }
                                        }
                                        if (i + 1 == Globals.MainWindow.generalattack.Items.Count) /*&& (Globals.MainWindow.no_normal_attack.unchecked)*/
                                        {
                                            int index1 = Data.inventorytype.IndexOf(Data.f_wep_name);
                                            if (Data.inventoryslot[index1] != 6)
                                            {
                                                System.Threading.Thread.Sleep(1);
                                                Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYMOVEMENT);
                                                NewPacket.WriteUInt8(0);
                                                NewPacket.WriteUInt8((byte)Data.inventoryslot[index1]);
                                                NewPacket.WriteUInt8(6);
                                                NewPacket.WriteUInt16(0x0000);
                                                Proxy.ag_remote_security.Send(NewPacket);
                                            }
                                            else
                                            {
                                                ImbueCast();
                                                Training.Attack(Training.monster_id);
                                                //No more skills, normal attack
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Data.bot = false;
                                        Data.loop = false;
                                        Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                                        Globals.MainWindow.UpdateLogs("Bot Stops! Cannot Find 1st Weapon");
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < Globals.MainWindow.partyattack.Items.Count; i++)
                            {
                                if (Data.f_wep_name != "" && Data.f_wep_name != null)
                                {
                                    int index = Data.inventorytype.IndexOf(Data.f_wep_name);
                                    if (index == -1)
                                    {
                                        Data.f_wep_name = "";
                                    }
                                }
                                System.Threading.Thread.Sleep(1);
                                if (Data.f_wep_name != "")
                                {
                                    if (Skill.Skills[Skill.PartyAttackSkills[i].index].Status == 0 && Skill.PartyAttackSkills[i].MinMP <= Character.CurrentMP)
                                    {
                                        System.Threading.Thread.Sleep(1);
                                        int index1 = Data.inventorytype.IndexOf(Data.f_wep_name);
                                        if (Data.inventoryslot[index1] != 6)
                                        {
                                            System.Threading.Thread.Sleep(1);
                                            Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYMOVEMENT);
                                            NewPacket.WriteUInt8(0);
                                            NewPacket.WriteUInt8((byte)Data.inventoryslot[index1]);
                                            NewPacket.WriteUInt8(6);
                                            NewPacket.WriteUInt16(0x0000);
                                            Proxy.ag_remote_security.Send(NewPacket);
                                            break;
                                        }
                                        else
                                        {
                                            ImbueCast();
                                            CastSkill(Skill.PartyAttackSkills[i], Training.monster_id);
                                            break;
                                        }
                                    }
                                    if (i + 1 == Globals.MainWindow.partyattack.Items.Count) /*&& (Globals.MainWindow.no_normal_attack.unchecked)*/
                                    {
                                        System.Threading.Thread.Sleep(1);
                                        int index1 = Data.inventorytype.IndexOf(Data.f_wep_name);
                                        if (Data.inventoryslot[index1] != 6)
                                        {
                                            System.Threading.Thread.Sleep(1);
                                            Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYMOVEMENT);
                                            NewPacket.WriteUInt8(0);
                                            NewPacket.WriteUInt8((byte)Data.inventoryslot[index1]);
                                            NewPacket.WriteUInt8(6);
                                            NewPacket.WriteUInt16(0x0000);
                                            Proxy.ag_remote_security.Send(NewPacket);
                                        }
                                        else
                                        {
                                            ImbueCast();
                                            Training.Attack(Training.monster_id);
                                            //No more skills, normal attack
                                        }
                                    }
                                }
                                else
                                {
                                    Data.bot = false;
                                    Data.loop = false;
                                    Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");;
                                    Globals.MainWindow.UpdateLogs("Bot Stops! Cannot Find 1st Weapon");
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        #region CastSkill
        //public static bool skill_casting = false;
        //public static bool skill_casted = false;
        public static void CastSkill(Skill skill, uint mob_id)
        {
            if (skill.SType == 1)
            {
                Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTACTION);
                NewPacket.WriteUInt8(1);
                NewPacket.WriteUInt8(4);
                NewPacket.WriteUInt32(skill.UniqueID);
                NewPacket.WriteUInt8(1);
                NewPacket.WriteUInt32(mob_id);
                Proxy.ag_remote_security.Send(NewPacket);
            }
            else if (skill.SType == 0)
            {
                Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTACTION);
                NewPacket.WriteUInt8(1);
                NewPacket.WriteUInt8(4);
                NewPacket.WriteUInt32(skill.UniqueID);
                NewPacket.WriteInt8(0);
                Proxy.ag_remote_security.Send(NewPacket);
            } 
        }
        #endregion

        #region SkillUpdate
        public static void SkillUpdate(Packet packet)
        {
            if (packet.ReadUInt8() == 0x01)
            {
                int count = 0;
                uint new_skill_id = packet.ReadUInt32();
                int index = Skills_Info.skillsidlist.IndexOf(new_skill_id);
                string new_skill_name = Skills_Info.skillsnamelist[index];              
                foreach (Skill skill in Skill.Skills)
                {
                    if (skill.Name == new_skill_name)
                    {
                        count++;
                        Skill.Skills.Remove(skill);
                        System.Timers.Timer Timer = new System.Timers.Timer();
                        Skill NewSkill = new Skill(new_skill_id, new_skill_name, Skills_Info.skillstypelist[index], Skills_Info.skillslevellist[index],
                            Skills_Info.skillbuffcheck[index], 0, Skills_Info.skillscasttimelist[index],
                            Skills_Info.skillcooldownlist[index], Skills_Info.skillsmpreq[index], Skills_Info.skillbufftime[index], 0, Timer);
                        Skill.Skills.Add(NewSkill);
                        break;
                    }
                }
                if (count == 0)
                {
                    System.Timers.Timer Timer = new System.Timers.Timer();
                    Skill NewSkill = new Skill(new_skill_id, new_skill_name, Skills_Info.skillstypelist[index], Skills_Info.skillslevellist[index],
                        Skills_Info.skillbuffcheck[index], 0, Skills_Info.skillscasttimelist[index],
                        Skills_Info.skillcooldownlist[index], Skills_Info.skillsmpreq[index], Skills_Info.skillbufftime[index], 0, Timer);
                    Skill.Skills.Add(NewSkill);
                }
            }
            Globals.MainWindow.Updatehealskill();
            Globals.MainWindow.UpdateSkillList();
        }
        #endregion

        public static void CastGhostWalk(uint id, byte xsector, byte ysector, ushort xcoord, ushort zcoord, ushort ycoord)
        {
            Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTACTION);
            NewPacket.WriteUInt8(1);
            NewPacket.WriteUInt8(4);
            NewPacket.WriteUInt32(id);
            NewPacket.WriteUInt8(2);
            NewPacket.WriteUInt8(xsector);
            NewPacket.WriteUInt8(ysector);
            NewPacket.WriteUInt16(xcoord);
            NewPacket.WriteUInt16(0);
            NewPacket.WriteUInt16(zcoord);
            NewPacket.WriteUInt16(0);
            NewPacket.WriteUInt16(ycoord);
            NewPacket.WriteUInt16(0);
            Proxy.ag_remote_security.Send(NewPacket);                       
        }

        public static uint ghostwalkid = 0;
        public static bool ghostwalking = false;

        public static void GhostWalk(int distance, double X, double Y)
        {
            if (Globals.MainWindow.Checked(Globals.MainWindow.useghostwalk) == true)
            {
                if (distance > Convert.ToInt32(Globals.MainWindow.ReadText(Globals.MainWindow.ghostwalkdistance)))
                {
                    foreach (Skill skill in Skill.Skills)
                    {
                        if (skill.Name == Globals.MainWindow.ghostwalk.Items[0].ToString() && skill.Status == 0)
                        {
                            ghostwalkid = skill.UniqueID;
                            ghostwalking = true;
                        }
                    }                    
                }
            }            
        }
    }
}