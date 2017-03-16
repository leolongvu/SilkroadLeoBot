using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;

namespace LeoBot
{
    class Configs
    {
        public static byte config = 0;
        public static void SaveInfo()
        {
            if (!Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\LoginConfigs"))
            {
                Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + @"\LoginConfigs");
            }
            if (Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\LoginConfigs"))
            {
                TextWriter config_writer = new StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory + @"\LoginConfigs\" + "LoadInfo" + config + ".ini");
                //AutoLogin
                if (Data.sro_path != "")
                {
                    config_writer.WriteLine("[SRO Path]");
                    config_writer.WriteLine(Data.sro_path);
                }
                config_writer.WriteLine("[AutoLogin]");
                config_writer.WriteLine("username=" + Globals.MainWindow.ReadText(Globals.MainWindow.username));
                config_writer.WriteLine("autologincheck=" + Globals.MainWindow.Checked(Globals.MainWindow.autologin));
                config_writer.WriteLine("characterselect=" + Globals.MainWindow.Checked(Globals.MainWindow.characterselect));
                config_writer.WriteLine("charactername=" + Globals.MainWindow.ReadText(Globals.MainWindow.charnameselect));
                //AutoLogin
                config_writer.Close();
            }                
        }

        public static void LoadInfo()
        {
            if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\LoginConfigs\" + "LoadInfo" + config + ".ini"))
            {
                TextReader config_reader = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + @"\LoginConfigs\" + "LoadInfo" + config + ".ini");
                string input;
                while ((input = config_reader.ReadLine()) != null)
                {
                    switch (input)
                    {
                        case "[SRO Path]":
                            Data.sro_path = config_reader.ReadLine();
                            break;
                        case "[AutoLogin]":
                            Globals.MainWindow.SetText(Globals.MainWindow.username, config_reader.ReadLine().Split('=')[1]);
                            Globals.MainWindow.SetCheck(Globals.MainWindow.autologin, config_reader.ReadLine().Split('=')[1]);
                            Globals.MainWindow.SetCheck(Globals.MainWindow.characterselect, config_reader.ReadLine().Split('=')[1]);
                            Globals.MainWindow.SetText(Globals.MainWindow.charnameselect, config_reader.ReadLine().Split('=')[1]);
                            break;
                    }
                }
                config_reader.Close();
            }
            else
            {
                Globals.MainWindow.UpdateLogs("Previous login information cannot be found!");
            }
        }

        public static void WriteConfigs()
        {
            if (((Globals.MainWindow.ReadText(Globals.MainWindow.charname)) != "") && ((Globals.MainWindow.ReadText(Globals.MainWindow.charname)) != null))
            {
                if (!Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\Configs\" + Character.PlayerName + @"\"))
                {
                    Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + @"\Configs\" + Character.PlayerName + @"\");
                }
                if (Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\Configs\" + Character.PlayerName + @"\"))
                {
                    TextWriter config_writer = new StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory + @"\Configs\" + Character.PlayerName + @"\" + "Configs" + ".ini");                 
                    //Training Setting
                    config_writer.WriteLine("[Hunt]");
                    config_writer.WriteLine("huntx=" + Globals.MainWindow.ReadText(Globals.MainWindow.x_setbox));
                    config_writer.WriteLine("hunty=" + Globals.MainWindow.ReadText(Globals.MainWindow.y_setbox));
                    config_writer.WriteLine("huntr=" + Globals.MainWindow.ReadText(Globals.MainWindow.trainingrangebox));
                    config_writer.WriteLine("runr=" + Globals.MainWindow.ReadText(Globals.MainWindow.runningrangebox));
                    config_writer.WriteLine("usecirclewalk=" + Globals.MainWindow.Checked(Globals.MainWindow.walkaround));
                    config_writer.WriteLine("usepathwalk=" + Globals.MainWindow.Checked(Globals.MainWindow.walkpath));
                    config_writer.WriteLine("coordinatecounts=" + Globals.MainWindow.trainbox.Items.Count);
                    int coordinatecount = Globals.MainWindow.trainbox.Items.Count;
                    for (int i = 0; i < coordinatecount; i++)
                    {
                        config_writer.WriteLine(Globals.MainWindow.trainbox.Items[i].ToString());
                    }
                    config_writer.WriteLine("pathwalkfile=" + Globals.MainWindow.ReadText(Globals.MainWindow.trainrecordpath));
                    config_writer.WriteLine("walktocenter=" + Globals.MainWindow.Checked(Globals.MainWindow.walkcenter));
                    config_writer.WriteLine("attackwithpet=" + Globals.MainWindow.Checked(Globals.MainWindow.petattack));
                    config_writer.WriteLine("protectpet=" + Globals.MainWindow.Checked(Globals.MainWindow.protectpet));
                    config_writer.WriteLine("norange=" + Globals.MainWindow.Checked(Globals.MainWindow.norange));
                    config_writer.WriteLine("[Berserk]");
                    config_writer.WriteLine("9");
                    for (int a = 0; a < 9; a++)
                    {
                        config_writer.WriteLine(Globals.MainWindow.berserk.Items[a].ToString());
                    }
                    config_writer.WriteLine("[Priority]");
                    config_writer.WriteLine("9");
                    for (int a = 0; a < 9; a++)
                    {
                        config_writer.WriteLine(Globals.MainWindow.priority.Items[a].ToString());
                    }
                    config_writer.WriteLine("[Avoidmob]");
                    config_writer.WriteLine(Globals.MainWindow.advancemob.Items.Count);
                    for (int i = 0; i < Globals.MainWindow.advancemob.Items.Count; i++)
                    {
                        config_writer.WriteLine(Globals.MainWindow.advancemob.Items[i].ToString());
                    }
                    //Training Setting End
                    //Skill Setting
                    config_writer.WriteLine("[Generalskill]");
                    config_writer.WriteLine("imbue_count=" + Globals.MainWindow.generalimbue.Items.Count);
                    for (int i = 0; i < Globals.MainWindow.generalimbue.Items.Count; i++)
                    {
                        config_writer.WriteLine("imbuegeneral_name=" + Globals.MainWindow.generalimbue.Items[i].ToString());
                    }
                    config_writer.WriteLine("skills_general_count=" + Globals.MainWindow.generalattack.Items.Count);
                    for (int i = 0; i < Globals.MainWindow.generalattack.Items.Count; i++)
                    {
                        config_writer.WriteLine("skills_general=" + Globals.MainWindow.generalattack.Items[i].ToString());
                    }
                    config_writer.WriteLine("buffs_general_counts=" + Globals.MainWindow.buff.Items.Count);
                    for (int i = 0; i < Globals.MainWindow.buff.Items.Count; i++)
                    {
                        config_writer.WriteLine("buffs_general=" + Globals.MainWindow.buff.Items[i].ToString());
                    }
                    config_writer.WriteLine("[Partyskill]");
                    config_writer.WriteLine("imbue_count=" + Globals.MainWindow.partyimbue.Items.Count);
                    for (int i = 0; i < Globals.MainWindow.partyimbue.Items.Count; i++)
                    {
                        config_writer.WriteLine("imbueparty_name=" + Globals.MainWindow.partyimbue.Items[i].ToString());
                    }
                    config_writer.WriteLine("skills_party_count=" + Globals.MainWindow.partyattack.Items.Count);
                    for (int i = 0; i < Globals.MainWindow.partyattack.Items.Count; i++)
                    {
                        config_writer.WriteLine("skills_party=" + Globals.MainWindow.partyattack.Items[i].ToString());
                    }
                    config_writer.WriteLine("[TwoWeaponBuff]");
                    config_writer.WriteLine("buffcount=" + Globals.MainWindow.secondbuff.Items.Count);
                    for (int i = 0; i < Globals.MainWindow.secondbuff.Items.Count; i++)
                    {
                        config_writer.WriteLine("buff=" + Globals.MainWindow.secondbuff.Items[i].ToString());
                    }
                    config_writer.WriteLine("wep2count=" + Globals.MainWindow.secondweapon.Items.Count);
                    for (int i = 0; i < Globals.MainWindow.secondweapon.Items.Count; i++)
                    {
                        config_writer.WriteLine("2ndweapon=" + Globals.MainWindow.secondweapon.Items[i].ToString());
                    }
                    config_writer.WriteLine("wep1count=" + Globals.MainWindow.firstweapon.Items.Count);
                    for (int i = 0; i < Globals.MainWindow.firstweapon.Items.Count; i++)
                    {
                        config_writer.WriteLine("1stweapon=" + Globals.MainWindow.firstweapon.Items[i].ToString());
                    }
                    config_writer.WriteLine("[GhostWalk]");
                    config_writer.WriteLine("walk_count=" + Globals.MainWindow.ghostwalk.Items.Count);
                    for (int i = 0; i < Globals.MainWindow.ghostwalk.Items.Count; i++)
                    {
                        config_writer.WriteLine("ghostwalkskill=" + Globals.MainWindow.ghostwalk.Items[i].ToString());
                    }
                    config_writer.WriteLine("useghostwalk=" + Globals.MainWindow.Checked(Globals.MainWindow.useghostwalk));
                    config_writer.WriteLine("distance=" + Globals.MainWindow.ReadText(Globals.MainWindow.ghostwalkdistance));
                    //Skill Setting End
                    //Auto Potion
                    config_writer.WriteLine("[AutoPotion]");
                    config_writer.WriteLine("autohp=" + Globals.MainWindow.Checked(Globals.MainWindow.hpuse));
                    config_writer.WriteLine("autohptext=" + Globals.MainWindow.ReadText(Globals.MainWindow.hptext));
                    config_writer.WriteLine("automp=" + Globals.MainWindow.Checked(Globals.MainWindow.mpuse));
                    config_writer.WriteLine("automptext=" + Globals.MainWindow.ReadText(Globals.MainWindow.mptext));
                    config_writer.WriteLine("autovigorhp=" + Globals.MainWindow.Checked(Globals.MainWindow.vigorhpuse));
                    config_writer.WriteLine("autovigorhptext=" + Globals.MainWindow.ReadText(Globals.MainWindow.vigorhptext));
                    config_writer.WriteLine("autovigormp=" + Globals.MainWindow.Checked(Globals.MainWindow.vigormpuse));
                    config_writer.WriteLine("autovigormptext=" + Globals.MainWindow.ReadText(Globals.MainWindow.vigormptext));
                    config_writer.WriteLine("auto25%hp=" + Globals.MainWindow.Checked(Globals.MainWindow.shpuse));
                    config_writer.WriteLine("auto25%hptext=" + Globals.MainWindow.ReadText(Globals.MainWindow.shptext));
                    config_writer.WriteLine("auto25%mp=" + Globals.MainWindow.Checked(Globals.MainWindow.smpuse));
                    config_writer.WriteLine("auto25%mptext=" + Globals.MainWindow.ReadText(Globals.MainWindow.smptext));
                    config_writer.WriteLine("skillhp=" + Globals.MainWindow.Checked(Globals.MainWindow.skillhpuse));
                    config_writer.WriteLine("skill=" + Globals.MainWindow.healskill.SelectedIndex);
                    config_writer.WriteLine("skillhptext=" + Globals.MainWindow.ReadText(Globals.MainWindow.skillhptext));
                    config_writer.WriteLine("unipill=" + Globals.MainWindow.Checked(Globals.MainWindow.uniuse));
                    config_writer.WriteLine("[Backtown]");
                    config_writer.WriteLine("dead=" + Globals.MainWindow.Checked(Globals.MainWindow.dead));
                    config_writer.WriteLine("noarrows=" + Globals.MainWindow.Checked(Globals.MainWindow.lowarrow));
                    config_writer.WriteLine("nobolts=" + Globals.MainWindow.Checked(Globals.MainWindow.lowbolt));
                    config_writer.WriteLine("inventoryfull=" + Globals.MainWindow.Checked(Globals.MainWindow.inventoryfull));
                    config_writer.WriteLine("weapondurability=" + Globals.MainWindow.Checked(Globals.MainWindow.lowdurability));
                    config_writer.WriteLine("lowhp=" + Globals.MainWindow.Checked(Globals.MainWindow.lowhp));
                    config_writer.WriteLine("lowhptext=" + Globals.MainWindow.ReadText(Globals.MainWindow.lowhptext));
                    config_writer.WriteLine("lowmp=" + Globals.MainWindow.Checked(Globals.MainWindow.lowmp));
                    config_writer.WriteLine("lowmptext=" + Globals.MainWindow.ReadText(Globals.MainWindow.lowmptext));
                    config_writer.WriteLine("[Petautoheal]");
                    config_writer.WriteLine("hppet=" + Globals.MainWindow.Checked(Globals.MainWindow.pethpuse));
                    config_writer.WriteLine("hppetset=" + Globals.MainWindow.ReadText(Globals.MainWindow.pethptext));
                    config_writer.WriteLine("hgppet=" + Globals.MainWindow.Checked(Globals.MainWindow.pethgpuse));
                    config_writer.WriteLine("hgppettext=" + Globals.MainWindow.ReadText(Globals.MainWindow.pethgptext));
                    config_writer.WriteLine("unipet=" + Globals.MainWindow.Checked(Globals.MainWindow.unipet));
                    config_writer.WriteLine("horsehp=" + Globals.MainWindow.Checked(Globals.MainWindow.horsehpuse));
                    config_writer.WriteLine("horsehptext=" + Globals.MainWindow.ReadText(Globals.MainWindow.horsehptext));
                    config_writer.WriteLine("horsebad=" + Globals.MainWindow.Checked(Globals.MainWindow.unihorse));
                    //Auto Potion End
                    //Pick up Setting
                    config_writer.WriteLine("[PickSetting]");
                    config_writer.WriteLine("pet_check=" + Globals.MainWindow.Checked(Globals.MainWindow.petpick));
                    config_writer.WriteLine("pickother=" + Globals.MainWindow.Checked(Globals.MainWindow.pickall));
                    config_writer.WriteLine("pick_choice1=" + Globals.MainWindow.Checked(Globals.MainWindow.nopick));
                    config_writer.WriteLine("pick_choice2=" + Globals.MainWindow.Checked(Globals.MainWindow.normalpick));
                    config_writer.WriteLine("pick_choice3=" + Globals.MainWindow.Checked(Globals.MainWindow.advancepick));
                    config_writer.WriteLine("[NormalPick]");
                    config_writer.WriteLine("gold=" + Globals.MainWindow.gold_drop.SelectedIndex);
                    config_writer.WriteLine("weapon=" + Globals.MainWindow.wep_drop.SelectedIndex);
                    config_writer.WriteLine("armor=" + Globals.MainWindow.armor_drop.SelectedIndex);
                    config_writer.WriteLine("accessory=" + Globals.MainWindow.acc_drop.SelectedIndex);
                    config_writer.WriteLine("weapon_e=" + Globals.MainWindow.wepe_drop.SelectedIndex);
                    config_writer.WriteLine("shield_e=" + Globals.MainWindow.shielde_drop.SelectedIndex);
                    config_writer.WriteLine("protector_e=" + Globals.MainWindow.prote_drop.SelectedIndex);
                    config_writer.WriteLine("accessory_e=" + Globals.MainWindow.acce_drop.SelectedIndex);
                    config_writer.WriteLine("hp_pot=" + Globals.MainWindow.hp_drop.SelectedIndex);
                    config_writer.WriteLine("mp_pot=" + Globals.MainWindow.mp_drop.SelectedIndex);
                    config_writer.WriteLine("vigor_pot=" + Globals.MainWindow.vigorp_drop.SelectedIndex);
                    config_writer.WriteLine("vigor_grain=" + Globals.MainWindow.vigorg_drop.SelectedIndex);
                    config_writer.WriteLine("uni_pill=" + Globals.MainWindow.uni_drop.SelectedIndex);
                    config_writer.WriteLine("return=" + Globals.MainWindow.return_drop.SelectedIndex);
                    config_writer.WriteLine("arrow=" + Globals.MainWindow.arrow_drop.SelectedIndex);
                    config_writer.WriteLine("bolt=" + Globals.MainWindow.bolt_drop.SelectedIndex);
                    config_writer.WriteLine("material=" + Globals.MainWindow.materials_drop.SelectedIndex);
                    config_writer.WriteLine("tablets=" + Globals.MainWindow.tablets_drop.SelectedIndex);
                    config_writer.WriteLine("[AdvancePick]");
                    foreach (AdvanceItem item in AdvanceItem.Allitems)
                        config_writer.WriteLine(item.Action);
                    //Pick up Setting End
                    //Loop Setting
                    config_writer.WriteLine("[Loop]");
                    config_writer.WriteLine("hppotiontype=" + Globals.MainWindow.hp_buy.SelectedIndex);
                    config_writer.WriteLine("hpbuytext=" + Globals.MainWindow.ReadText(Globals.MainWindow.hp_count));
                    config_writer.WriteLine("mppotiontype=" + Globals.MainWindow.mp_buy.SelectedIndex);
                    config_writer.WriteLine("mpbuytext=" + Globals.MainWindow.ReadText(Globals.MainWindow.mp_count));
                    config_writer.WriteLine("unipotiontype=" + Globals.MainWindow.uni_buy.SelectedIndex);
                    config_writer.WriteLine("unibuytext=" + Globals.MainWindow.ReadText(Globals.MainWindow.uni_count));
                    config_writer.WriteLine("scrolltype=" + Globals.MainWindow.return_buy.SelectedIndex);
                    config_writer.WriteLine("scrolltext=" + Globals.MainWindow.ReadText(Globals.MainWindow.return_count));
                    config_writer.WriteLine("speedtype=" + Globals.MainWindow.speed_buy.SelectedIndex);
                    config_writer.WriteLine("speedtext=" + Globals.MainWindow.ReadText(Globals.MainWindow.speed_count));
                    config_writer.WriteLine("horsetype=" + Globals.MainWindow.horse_buy.SelectedIndex);
                    config_writer.WriteLine("horsetext=" + Globals.MainWindow.ReadText(Globals.MainWindow.horse_count));
                    config_writer.WriteLine("recorverytype=" + Globals.MainWindow.php_buy.SelectedIndex);
                    config_writer.WriteLine("recoverytext=" + Globals.MainWindow.ReadText(Globals.MainWindow.php_count));
                    config_writer.WriteLine("petuni=" + Globals.MainWindow.pet_status_buy.SelectedIndex);
                    config_writer.WriteLine("petunicount=" + Globals.MainWindow.ReadText(Globals.MainWindow.pet_status_count));
                    config_writer.WriteLine("hgpbuytext=" + Globals.MainWindow.ReadText(Globals.MainWindow.hgp_count));
                    config_writer.WriteLine("arrowstext=" + Globals.MainWindow.ReadText(Globals.MainWindow.arrows_count));
                    config_writer.WriteLine("boltstext=" + Globals.MainWindow.ReadText(Globals.MainWindow.bolts_count));
                    config_writer.WriteLine("revivecount=" + Globals.MainWindow.ReadText(Globals.MainWindow.revive_count));
                    config_writer.WriteLine("loopoff=" + Globals.MainWindow.Checked(Globals.MainWindow.loop_off));
                    config_writer.WriteLine("repair=" + Globals.MainWindow.Checked(Globals.MainWindow.repairall));
                    config_writer.WriteLine("script=" + Data.walkscriptpath);
                    config_writer.WriteLine("coorcount=" + Globals.MainWindow.walkscript.Items.Count);
                    for (int i = 0; i < Globals.MainWindow.walkscript.Items.Count; i++)
                    {
                        config_writer.WriteLine(Globals.MainWindow.walkscript.Items[i].ToString());
                    }
                    //Loop Setting End
                    //Party
                    config_writer.WriteLine("[Party]");
                    config_writer.WriteLine("partyname=" + Globals.MainWindow.ReadText(Globals.MainWindow.partyname));
                    config_writer.WriteLine("minlevel=" + Globals.MainWindow.ReadText(Globals.MainWindow.minlevel));
                    config_writer.WriteLine("maxlevel=" + Globals.MainWindow.ReadText(Globals.MainWindow.maxlevel));
                    config_writer.WriteLine("itemdis=" + Globals.MainWindow.Checked(Globals.MainWindow.itemdis));
                    config_writer.WriteLine("noitemdis=" + Globals.MainWindow.Checked(Globals.MainWindow.noitemdis));
                    config_writer.WriteLine("expdis=" + Globals.MainWindow.Checked(Globals.MainWindow.expdis));
                    config_writer.WriteLine("noexpdis=" + Globals.MainWindow.Checked(Globals.MainWindow.noexpdis));
                    config_writer.WriteLine("allowother=" + Globals.MainWindow.Checked(Globals.MainWindow.allowinvite));
                    config_writer.WriteLine("allowrequest=" + Globals.MainWindow.Checked(Globals.MainWindow.allowparty));
                    config_writer.WriteLine("autoparty=" + Globals.MainWindow.Checked(Globals.MainWindow.autoparty));
                    //End Party
                    config_writer.Close();
                }
                else
                {
                    Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + @"\Configs");
                    WriteConfigs();
                }
            }
        }

        public static void ReadConfigs()
        {
            {
                if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\Configs\" + Character.PlayerName + @"\" + "Configs" + ".ini"))
                {
                    TextReader config_reader = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + @"\Configs\" + Character.PlayerName + @"\" + "Configs" + ".ini");
                    string input;
                    while ((input = config_reader.ReadLine()) != null)
                    {
                        switch (input)
                        {
                            case "[AutoLogin]":
                                Globals.MainWindow.SetText(Globals.MainWindow.username, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.autologin, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.characterselect, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.charnameselect, config_reader.ReadLine().Split('=')[1]);
                            break;
                            case "[Hunt]":
                                Globals.MainWindow.SetText(Globals.MainWindow.x_setbox, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.y_setbox, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.trainingrangebox, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.runningrangebox, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.walkaround, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.walkpath, config_reader.ReadLine().Split('=')[1]);
                                int coordinatescount = Convert.ToInt32(config_reader.ReadLine().Split('=')[1]);
                                for (int i = 0; i < coordinatescount; i++)
                                {
                                    Globals.MainWindow.AddText(Globals.MainWindow.trainbox, config_reader.ReadLine().Split('=')[1]);
                                }
                                Globals.MainWindow.SetText(Globals.MainWindow.trainrecordpath, config_reader.ReadLine().Split('=')[1]);                                                  
                                Globals.MainWindow.SetCheck(Globals.MainWindow.walkcenter, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.petattack, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.protectpet, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.norange, config_reader.ReadLine().Split('=')[1]);                  
                                break;
                            case "[Berserk]":
                                int berserkcount = Convert.ToInt32(config_reader.ReadLine());
                                Globals.MainWindow.Clear(Globals.MainWindow.berserk);
                                for (int i = 0; i < berserkcount; i++)
                                {
                                    Globals.MainWindow.AddText(Globals.MainWindow.berserk, config_reader.ReadLine());
                                }
                                break;
                            case "[Priority]":
                                int prioritycount = Convert.ToInt32(config_reader.ReadLine());
                                Globals.MainWindow.Clear(Globals.MainWindow.priority);
                                for (int i = 0; i < prioritycount; i++)
                                {
                                    Globals.MainWindow.AddText(Globals.MainWindow.priority, config_reader.ReadLine());
                                }
                                break;
                            case "[Avoidmob]":
                                Monster.AvoidMob.Clear();
                                Globals.MainWindow.Clear(Globals.MainWindow.advancemob);
                                int avoidmobcount = Convert.ToInt32(config_reader.ReadLine());
                                for (int i = 0; i < avoidmobcount; i++)
                                {
                                    string input1 = config_reader.ReadLine();
                                    Globals.MainWindow.AddText(Globals.MainWindow.advancemob, input1);
                                    Monster.AvoidMob.Add(Monster.ModTypeList[Monster.ModNameList.IndexOf(input1)]);
                                }
                                break;
                            case "[Generalskill]":
                                Globals.MainWindow.Clear(Globals.MainWindow.generalimbue);
                                int imbue_count = Convert.ToInt32(config_reader.ReadLine().Split('=')[1]);
                                for (int i = 0; i < imbue_count; i++)
                                {
                                    Globals.MainWindow.AddText(Globals.MainWindow.generalimbue, config_reader.ReadLine().Split('=')[1]);
                                }
                                Globals.MainWindow.Clear(Globals.MainWindow.generalattack);
                                int skill_count = Convert.ToInt32(config_reader.ReadLine().Split('=')[1]);
                                for (int i = 0; i < skill_count; i++)
                                {
                                    Globals.MainWindow.AddText(Globals.MainWindow.generalattack, config_reader.ReadLine().Split('=')[1]);
                                }
                                int buff_count = Convert.ToInt32(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.Clear(Globals.MainWindow.buff);
                                for (int i = 0; i < buff_count; i++)
                                {
                                    Globals.MainWindow.AddText(Globals.MainWindow.buff, config_reader.ReadLine().Split('=')[1]);
                                }
                                break;
                            case "[Partyskill]":
                                Globals.MainWindow.Clear(Globals.MainWindow.partyimbue);
                                int imbue_count1 = Convert.ToInt32(config_reader.ReadLine().Split('=')[1]);
                                for (int i = 0; i < imbue_count1; i++)
                                {
                                    Globals.MainWindow.AddText(Globals.MainWindow.partyimbue, config_reader.ReadLine().Split('=')[1]);
                                }
                                Globals.MainWindow.Clear(Globals.MainWindow.partyattack);
                                int skill_count1 = Convert.ToInt32(config_reader.ReadLine().Split('=')[1]);
                                for (int i = 0; i < skill_count1; i++)
                                {
                                    Globals.MainWindow.AddText(Globals.MainWindow.partyattack, config_reader.ReadLine().Split('=')[1]);
                                }
                                break;
                            case "[TwoWeaponBuff]":
                                Globals.MainWindow.Clear(Globals.MainWindow.secondbuff);
                                int buff_count2 = Convert.ToInt32(config_reader.ReadLine().Split('=')[1]);
                                for (int i = 0; i < buff_count2; i++)
                                {
                                    Globals.MainWindow.AddText(Globals.MainWindow.secondbuff, config_reader.ReadLine().Split('=')[1]);
                                }
                                Globals.MainWindow.Clear(Globals.MainWindow.secondweapon);
                                int sndweapon_count = Convert.ToInt32(config_reader.ReadLine().Split('=')[1]);
                                for (int i = 0; i < sndweapon_count; i++)
                                {
                                    string swepname = config_reader.ReadLine().Split('=')[1];
                                    Globals.MainWindow.AddText(Globals.MainWindow.secondweapon, swepname);
                                    Data.s_wep_name = Data.inventorytype[Data.inventoryname.IndexOf(swepname)];
                                }
                                int fwep_count = Convert.ToInt32(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.Clear(Globals.MainWindow.firstweapon);
                                for (int i = 0; i < fwep_count; i++)
                                {
                                    string fwepname = config_reader.ReadLine().Split('=')[1];
                                    Globals.MainWindow.AddText(Globals.MainWindow.firstweapon, fwepname);
                                    Data.f_wep_name = Data.inventorytype[Data.inventoryname.IndexOf(fwepname)];
                                }
                                break;
                            case "[GhostWalk]":
                                Globals.MainWindow.Clear(Globals.MainWindow.ghostwalk);
                                int ghostcount = Convert.ToInt32(config_reader.ReadLine().Split('=')[1]);
                                for (int i = 0; i < ghostcount; i++)
                                {
                                    Globals.MainWindow.AddText(Globals.MainWindow.ghostwalk, config_reader.ReadLine().Split('=')[1]);
                                }
                                Globals.MainWindow.SetCheck(Globals.MainWindow.useghostwalk, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.ghostwalkdistance, config_reader.ReadLine().Split('=')[1]);
                                if (Globals.MainWindow.Checked(Globals.MainWindow.useghostwalk) == true)
                                {
                                    MainWindow.enable = false;
                                    Globals.MainWindow.UnEnable(Globals.MainWindow.addghostwalk);                                    
                                }
                                break;
                            case "[AutoPotion]":
                                Globals.MainWindow.SetCheck(Globals.MainWindow.hpuse, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.hptext, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.mpuse, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.mptext, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.vigorhpuse, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.vigorhptext, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.vigormpuse, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.vigormptext, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.shpuse, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.shptext, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.smpuse, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.smptext, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.skillhpuse, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.Index(Globals.MainWindow.healskill, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.SetText(Globals.MainWindow.skillhptext, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.uniuse, config_reader.ReadLine().Split('=')[1]);
                                break;
                            case "[Petautoheal]":
                                Globals.MainWindow.SetCheck(Globals.MainWindow.pethpuse, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.pethptext, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.pethgpuse, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.pethgptext, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.unipet, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.horsehpuse, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.horsehptext, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.unihorse, config_reader.ReadLine().Split('=')[1]);
                                break;
                            case "[Backtown]":
                                Globals.MainWindow.SetCheck(Globals.MainWindow.dead, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.lowarrow, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.lowbolt, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.inventoryfull, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.lowdurability, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.lowhp, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.lowhptext, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.lowmp, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.lowmptext, config_reader.ReadLine().Split('=')[1]);
                                break;
                            case "[PickSetting]":
                                Globals.MainWindow.SetCheck(Globals.MainWindow.petpick, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.pickall, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.nopick, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.normalpick, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.advancepick, config_reader.ReadLine().Split('=')[1]);
                                break;
                            case "[NormalPick]":
                                Globals.MainWindow.Index(Globals.MainWindow.gold_drop, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.Index(Globals.MainWindow.wep_drop, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.Index(Globals.MainWindow.armor_drop, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.Index(Globals.MainWindow.acc_drop, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.Index(Globals.MainWindow.wepe_drop, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.Index(Globals.MainWindow.shielde_drop, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.Index(Globals.MainWindow.prote_drop, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.Index(Globals.MainWindow.acce_drop, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.Index(Globals.MainWindow.hp_drop, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.Index(Globals.MainWindow.mp_drop, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.Index(Globals.MainWindow.vigorp_drop, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.Index(Globals.MainWindow.vigorg_drop, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.Index(Globals.MainWindow.uni_drop, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.Index(Globals.MainWindow.return_drop, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.Index(Globals.MainWindow.arrow_drop, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.Index(Globals.MainWindow.bolt_drop, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.Index(Globals.MainWindow.materials_drop, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.Index(Globals.MainWindow.tablets_drop, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                break;
                            case "[AdvancePick]":
                                foreach (AdvanceItem item in AdvanceItem.Allitems)
                                {
                                    item.Action = config_reader.ReadLine();
                                    foreach (AdvanceItem itemgold in AdvanceItem.Gold)
                                    {
                                        if (item.AdvanceType == itemgold.AdvanceType)
                                            itemgold.Action = item.Action;
                                    }
                                    foreach (AdvanceItem itemweapon in AdvanceItem.Weapon)
                                    {
                                        if (item.AdvanceType == itemweapon.AdvanceType)
                                            itemweapon.Action = item.Action;
                                    }
                                    foreach (AdvanceItem itemarmor in AdvanceItem.Armor)
                                    {
                                        if (item.AdvanceType == itemarmor.AdvanceType)
                                            itemarmor.Action = item.Action;
                                    }
                                    foreach (AdvanceItem itemacces in AdvanceItem.Accessorise)
                                    {
                                        if (item.AdvanceType == itemacces.AdvanceType)
                                            itemacces.Action = item.Action;
                                    }
                                    foreach (AdvanceItem itemelixir in AdvanceItem.Elixir)
                                    {
                                        if (item.AdvanceType == itemelixir.AdvanceType)
                                            itemelixir.Action = item.Action;
                                    }
                                    foreach (AdvanceItem itemtablet in AdvanceItem.Tablet)
                                    {
                                        if (item.AdvanceType == itemtablet.AdvanceType)
                                            itemtablet.Action = item.Action;
                                    }
                                    foreach (AdvanceItem itemmaterial in AdvanceItem.Material)
                                    {
                                        if (item.AdvanceType == itemmaterial.AdvanceType)
                                            itemmaterial.Action = item.Action;
                                    }
                                    foreach (AdvanceItem itempotion in AdvanceItem.Potion)
                                    {
                                        if (item.AdvanceType == itempotion.AdvanceType)
                                            itempotion.Action = item.Action;
                                    }
                                    foreach (AdvanceItem itemreturn in AdvanceItem.Return)
                                    {
                                        if (item.AdvanceType == itemreturn.AdvanceType)
                                            itemreturn.Action = item.Action;
                                    }
                                    foreach (AdvanceItem itemquest in AdvanceItem.Quest)
                                    {
                                        if (item.AdvanceType == itemquest.AdvanceType)
                                            itemquest.Action = item.Action;
                                    }
                                    foreach (AdvanceItem itemmall in AdvanceItem.Mall)
                                    {
                                        if (item.AdvanceType == itemmall.AdvanceType)
                                            itemmall.Action = item.Action;
                                    }
                                    foreach (AdvanceItem itemarrow in AdvanceItem.Arrow)
                                    {
                                        if (item.AdvanceType == itemarrow.AdvanceType)
                                            itemarrow.Action = item.Action;
                                    }
                                }
                                break;
                            case "[Loop]":
                                Globals.MainWindow.Index(Globals.MainWindow.hp_buy, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.SetText(Globals.MainWindow.hp_count, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.Index(Globals.MainWindow.mp_buy, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.SetText(Globals.MainWindow.mp_count, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.Index(Globals.MainWindow.uni_buy, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.SetText(Globals.MainWindow.uni_count, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.Index(Globals.MainWindow.return_buy, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.SetText(Globals.MainWindow.return_count, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.Index(Globals.MainWindow.speed_buy, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.SetText(Globals.MainWindow.speed_count, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.Index(Globals.MainWindow.horse_buy, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.SetText(Globals.MainWindow.horse_count, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.Index(Globals.MainWindow.php_buy, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.SetText(Globals.MainWindow.php_count, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.Index(Globals.MainWindow.pet_status_buy, Convert.ToInt32(config_reader.ReadLine().Split('=')[1]));
                                Globals.MainWindow.SetText(Globals.MainWindow.pet_status_count, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.hgp_count, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.arrows_count, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.bolts_count, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.revive_count, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.loop_off, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.repairall, config_reader.ReadLine().Split('=')[1]);
                                string path = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.SetText(Globals.MainWindow.walkscriptpath, path);
                                Data.walkscriptpath = path;
                                int count = Convert.ToInt32(config_reader.ReadLine().Split('=')[1]);
                                for (int i = 0; i < count; i++)
                                {
                                    Globals.MainWindow.AddText(Globals.MainWindow.walkscript, config_reader.ReadLine().ToString());
                                }
                                break;
                            case "[Party]":
                                Globals.MainWindow.SetText(Globals.MainWindow.partyname, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.minlevel, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetText(Globals.MainWindow.maxlevel, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.itemdis, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.noitemdis, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.expdis, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.noexpdis, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.allowinvite, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.allowparty, config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.SetCheck(Globals.MainWindow.autoparty, config_reader.ReadLine().Split('=')[1]);
                                break;
                        }
                    }
                    config_reader.Close();
                    if (Data.f_wep_name != "" && Data.f_wep_name != null)
                    {
                        int index = Data.inventorytype.IndexOf(Data.f_wep_name);
                        if (index == -1)
                        {
                            Data.f_wep_name = "";
                        }
                    }
                    if (Data.s_wep_name != "" && Data.s_wep_name != null)
                    {
                        int index = Data.inventorytype.IndexOf(Data.s_wep_name);
                        if (index == -1)
                        {
                            Data.s_wep_name = "";
                        }
                    }                   
                }               
            }
        }
    }
}