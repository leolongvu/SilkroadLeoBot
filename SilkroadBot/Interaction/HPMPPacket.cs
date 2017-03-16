using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace LeoBot
{
    class HPMPPacket
    {
        public static Stopwatch Counter = new Stopwatch();
        public static byte bad_status = 0;
        public static byte pet_status = 0;
        public static byte horse_status = 0;
        public static bool frozen = false;

        public static void HPMPUpdate(Packet hp_packet)
        {
            {
                uint id = hp_packet.ReadUInt32();

                if (id == Character.UniqueID)
                {
                    hp_packet.ReadUInt8();
                    hp_packet.ReadUInt8(); // 0x00
                    byte type2 = hp_packet.ReadUInt8();
                    switch (type2)
                    {
                        case 0x01:
                            Character.CurrentHP = hp_packet.ReadUInt32();
                            break;
                        case 0x02:
                            Character.CurrentMP = hp_packet.ReadUInt32();
                            break;
                        case 0x03:
                            Character.CurrentHP = hp_packet.ReadUInt32();
                            Character.CurrentMP = hp_packet.ReadUInt32();
                            break;
                        case 0x04:
                            uint status = hp_packet.ReadUInt32();
                            if (status == 0)
                            {
                                bad_status = 0;
                                Globals.MainWindow.SetText(Globals.MainWindow.status, "Normal");
                                if (frozen == true)
                                {
                                    frozen = false;
                                    System.Threading.Thread.Sleep(500);
                                    Globals.MainWindow.UpdateLogs("Frozen!");
                                }
                            }
                            else 
                            {                          
                                if (status == 3 || status == 2)
                                {
                                    frozen = true;
                                }
                                bad_status = 1;
                                Globals.MainWindow.SetText(Globals.MainWindow.status, "Bad Status");
                            }
                            break;
                    }

                    Globals.MainWindow.UpdateBar();

                    if (Character.CurrentMP >= Buffas.min_mp_require)
                    {
                        Buffas.min_mp_require = 200000;
                        Buffas.buff_waiting = true;
                    }
                    if (Character.CurrentHP > 0)
                    {                       
                        Data.dead = false;
                    }
                    else
                    {
                        Data.dead = true;
                        Data.Statistic.died_count++;
                        /*if (Globals.MainWindow.alert_char_die.Checked)
                        {
                            Alert.StartAlert();
                        }*/
                        if (Globals.MainWindow.Checked(Globals.MainWindow.dead) == true)
                        {
                            //Return To Town     
                            Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_ACCEPTDEAD);
                            NewPacket.WriteUInt8(1);
                            Proxy.ag_remote_security.Send(NewPacket);
                            Data.returning = 1;
                        }
                    }
                    if (Globals.MainWindow.Checked(Globals.MainWindow.uniuse) == true && bad_status == 1)
                    {
                        Autopot.UseUni();
                    }                   
                    uint hp = Character.CurrentHP * 100 / Character.MaxHP;
                    uint mp = Character.CurrentMP * 100 / Character.MaxMP;

                    if (Globals.MainWindow.Checked(Globals.MainWindow.hpuse) == true)
                    {
                        if (hp < Convert.ToInt32(Globals.MainWindow.ReadText(Globals.MainWindow.hptext)))
                        {
                            Autopot.UseHP();
                        }
                    }
                    if (Globals.MainWindow.Checked(Globals.MainWindow.shpuse) == true)
                    {
                        if (hp < Convert.ToInt32(Globals.MainWindow.ReadText(Globals.MainWindow.shptext)))
                        {
                            Autopot.UseSHP();
                        }
                    }
                    if (Globals.MainWindow.Checked(Globals.MainWindow.mpuse) == true)
                    {
                        if (mp < Convert.ToInt32(Globals.MainWindow.ReadText(Globals.MainWindow.mptext)))
                        {
                            Autopot.UseMP();
                        }
                    }
                    if (Globals.MainWindow.Checked(Globals.MainWindow.smpuse) == true)
                    {
                        if (mp < Convert.ToInt32(Globals.MainWindow.ReadText(Globals.MainWindow.smptext)))
                        {
                            Autopot.UseSMP();
                        }
                    }
                    if (Globals.MainWindow.Checked(Globals.MainWindow.vigorhpuse) == true)
                    {
                        if (hp < Convert.ToInt32(Globals.MainWindow.ReadText(Globals.MainWindow.vigorhptext))) 
                        {
                            Autopot.UseVigor();
                        }
                    }
                    if (Globals.MainWindow.Checked(Globals.MainWindow.vigormpuse) == true)
                    {
                        if (mp < Convert.ToInt32(Globals.MainWindow.ReadText(Globals.MainWindow.vigormptext)))
                        {
                            Autopot.UseVigor();
                        }
                    }                 
                    try
                    {
                        if (Globals.MainWindow.Checked(Globals.MainWindow.skillhpuse) == true)
                        {
                            if (hp < Convert.ToInt32(Globals.MainWindow.ReadText(Globals.MainWindow.skillhptext)))
                            {
                                foreach (Skill skill in Skill.Skills)
                                {
                                    if (skill.Name == Globals.MainWindow.ReadText1(Globals.MainWindow.healskill))
                                    {
                                        Skills.CastSkill(skill, 0);
                                        break;
                                    }

                                }
                            }
                        }
                    }
                    catch 
                    {
                        Globals.MainWindow.UpdateLogs("Cannot find any skill to buff! Heal by skill disabled!");
                        Globals.MainWindow.UnCheck(Globals.MainWindow.skillhpuse);
                    }
                }
                else if (id == Data.char_attackpetid)
                {
                    hp_packet.ReadUInt8();
                    hp_packet.ReadUInt8();
                    byte type = hp_packet.ReadUInt8();
                    switch (type)
                    {
                        case 0x05:
                            Pets.CurrentHP = hp_packet.ReadUInt32();
                            uint hp = Pets.CurrentHP * 100 / Pets.MaxHP;
                            if (Globals.MainWindow.Checked(Globals.MainWindow.pethpuse) == true)
                            {
                                foreach (Pets CharPet in Pets.CharPets)
                                {
                                    if (CharPet.UniqueID == id)
                                    {
                                        if (hp < Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.pethptext))) 
                                        {
                                            Autopot.UsePetHP(CharPet.UniqueID);
                                        }
                                        break;
                                    }                            
                                }                            
                            }
                            break;
                        case 0x04:
                            if (hp_packet.ReadUInt32() == 0)
                            {
                                pet_status = 0;
                            }
                            else
                            {
                                pet_status = 1;
                            }
                            break;
                    }
                    foreach (Pets CharPet in Pets.CharPets)
                    {
                        if (CharPet.UniqueID == id)
                        {
                            if (Globals.MainWindow.Checked(Globals.MainWindow.unipet) == true && pet_status == 1)
                            {
                                Autopot.UsePetUni(CharPet.UniqueID);
                            }
                            break;
                        }
                    }                   
                }
                else if (id == Data.char_horseid)
                {
                    hp_packet.ReadUInt8();
                    hp_packet.ReadUInt8();
                    byte type = hp_packet.ReadUInt8();
                    switch (type)
                    {
                        case 0x05:
                            Pets.HorseCurrentHP = hp_packet.ReadUInt32();
                            uint hp = Pets.HorseCurrentHP * 100 / Pets.HorseMaxHP;
                            if (Globals.MainWindow.Checked(Globals.MainWindow.horsehpuse) == true)
                            {
                                foreach (Pets CharPet in Pets.CharPets)
                                {
                                    if (CharPet.UniqueID == id)
                                    {
                                        if (hp < Convert.ToUInt32(Globals.MainWindow.ReadText(Globals.MainWindow.horsehptext)))
                                        {
                                            Autopot.UsePetHP(CharPet.UniqueID);
                                        }
                                        break;
                                    }
                                }
                            }
                            break;
                        case 0x04:
                            if (hp_packet.ReadUInt32() == 0)
                            {
                                horse_status = 0;
                            }
                            else
                            {
                                horse_status = 1;
                            }
                            break;
                    }
                    foreach (Pets CharPet in Pets.CharPets)
                    {
                        if (CharPet.UniqueID == id)
                        {
                            if (Globals.MainWindow.Checked(Globals.MainWindow.unihorse) == true && horse_status == 1)
                            {
                                Autopot.UsePetUni(CharPet.UniqueID);
                            }
                            break;
                        }
                    }               
                }
                else
                {
                    hp_packet.ReadUInt8();
                    hp_packet.ReadUInt8();
                    byte type = hp_packet.ReadUInt8();
                    switch (type)
                    {
                        case 0x05:
                            uint hp = hp_packet.ReadUInt32();
                            PortConfigs.TrainWindow.Label(PortConfigs.TrainWindow.monsterHP, Convert.ToString(hp));
                            if (hp == 0)
                            {
                                foreach (Monster monster in Monster.SpawnMob)
                                {
                                    if (id == monster.UniqueID)
                                    {
                                        Monster.SpawnMob.Remove(monster);
                                        Stuck.DeleteMob(id);
                                        if (id == Training.monster_id)
                                        {
                                            Training.monster_selected = false;
                                            Training.monster_id = 0;
                                            //Select New Monster, cause selected just disapeared
                                            //If there is fire/cold wall, delete the buff
                                            if (Skills.buffcount1 > 0)
                                            {
                                                Packet NewPacket1 = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTACTION);
                                                NewPacket1.WriteUInt8(1);
                                                NewPacket1.WriteUInt8(5);
                                                NewPacket1.WriteUInt32(Skills.buff1);
                                                NewPacket1.WriteInt8(0);
                                                Proxy.ag_remote_security.Send(NewPacket1);
                                            }
                                            if (Skills.buffcount2 > 0)
                                            {
                                                Packet NewPacket1 = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTACTION);
                                                NewPacket1.WriteUInt8(1);
                                                NewPacket1.WriteUInt8(5);
                                                NewPacket1.WriteUInt32(Skills.buff2);
                                                NewPacket1.WriteInt8(0);
                                                Proxy.ag_remote_security.Send(NewPacket1);
                                            }
                                            LogicControl.Manager();
                                        }
                                        break;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        }
    }
}