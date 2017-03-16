using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LeoBot
{
    class LoadTXT
    {
        public static void LoadData()
        {
            Globals.MainWindow.UpdateLogs("Loading Silkroad data . . .");
            try
            {
                //Load mobs begin
                TextReader tr = new StreamReader(Environment.CurrentDirectory + @"\Data\Mob.txt");
                string input;
                string[] txt;
                while ((input = tr.ReadLine()) != null)
                {
                    if (input != "" && !input.StartsWith("//"))
                    {
                        txt = input.Split(',');
                        Mobs_Info.mobsidlist.Add(Globals.String_To_UInt32(txt[0]));
                        Mobs_Info.mobstypelist.Add(txt[1]);
                        Mobs_Info.mobsnamelist.Add(txt[2]);
                        Mobs_Info.mobshplist.Add(Convert.ToUInt32(txt[4]));
                        Mobs_Info.mobslevellist.Add(Convert.ToByte(txt[3]));
                    }
                }
                tr.Close();
                //Load mobs end
                //Load items begin
                tr = new StreamReader(Environment.CurrentDirectory + @"\Data\Item.txt");
                while ((input = tr.ReadLine()) != null)
                {
                    if (input != "" && !input.StartsWith("//"))
                    {
                        txt = input.Split(',');
                        Items_Info.itemsidlist.Add(Globals.String_To_UInt32(txt[0]));
                        Items_Info.itemstypelist.Add(txt[1]);
                        Items_Info.itemsnamelist.Add(txt[2]);
                        Items_Info.itemslevellist.Add(Convert.ToByte(txt[4]));
                        Items_Info.items_maxlist.Add(Convert.ToUInt16(txt[11]));
                        Items_Info.itemsdurabilitylist.Add(Convert.ToUInt32(txt[8]));

                        AdvanceItem Item = new AdvanceItem(txt[1], txt[2], Convert.ToByte(txt[4]), "Ignore");
                        AdvanceItem.Allitems.Add(Item);
                        if (txt[1].StartsWith("ITEM_ETC_GOLD"))
                            AdvanceItem.Gold.Add(Item);
                        else if ((txt[1].StartsWith("ITEM_CH_SWORD") || txt[1].StartsWith("ITEM_CH_BLADE") || txt[1].StartsWith("ITEM_CH_SPEAR") || txt[1].StartsWith("ITEM_CH_TBLADE") || txt[1].StartsWith("ITEM_CH_BOW") || txt[1].StartsWith("ITEM_CH_SHIELD") || txt[1].StartsWith("ITEM_EU_DAGGER") || txt[1].StartsWith("ITEM_EU_SWORD") || txt[1].StartsWith("ITEM_EU_TSWORD") || txt[1].StartsWith("ITEM_EU_AXE") || txt[1].StartsWith("ITEM_EU_CROSSBOW") || txt[1].StartsWith("ITEM_EU_DARKSTAFF") || txt[1].StartsWith("ITEM_EU_TSTAFF") || txt[1].StartsWith("ITEM_EU_HARP") || txt[1].StartsWith("ITEM_EU_STAFF") || txt[1].StartsWith("ITEM_EU_SHIELD")) && !(txt[1].Contains("RARE") || txt[1].Contains("BASIC") || txt[2].Contains("Tan khach")))
                            AdvanceItem.Weapon.Add(Item);
                        else if ((txt[1].StartsWith("ITEM_CH_M_HEAVY") || txt[1].StartsWith("ITEM_CH_M_LIGHT") || txt[1].StartsWith("ITEM_CH_M_CLOTHES") || txt[1].StartsWith("ITEM_CH_W_HEAVY") || txt[1].StartsWith("ITEM_CH_W_LIGHT") || txt[1].StartsWith("ITEM_CH_W_CLOTHES") || txt[1].StartsWith("ITEM_EU_M_HEAVY") || txt[1].StartsWith("ITEM_EU_M_LIGHT") || txt[1].StartsWith("ITEM_EU_M_CLOTHES") || txt[1].StartsWith("ITEM_EU_W_HEAVY") || txt[1].StartsWith("ITEM_EU_W_LIGHT") || txt[1].StartsWith("ITEM_EU_W_CLOTHES")) && !(txt[1].Contains("RARE") || txt[1].Contains("BASIC") || txt[2].Contains("Tan khach")))
                            AdvanceItem.Armor.Add(Item);
                        else if ((txt[1].StartsWith("ITEM_EU_RING") || txt[1].StartsWith("ITEM_EU_EARRING") || txt[1].StartsWith("ITEM_EU_NECKLACE") || txt[1].StartsWith("ITEM_CH_RING") || txt[1].StartsWith("ITEM_CH_EARRING") || txt[1].StartsWith("ITEM_CH_NECKLACE")) && !(txt[1].Contains("RARE") || txt[1].Contains("BASIC") || txt[2].Contains("Tan khach")))
                            AdvanceItem.Accessorise.Add(Item);
                        else if (txt[1].StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_WEAPON") || txt[1].StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_SHIELD") || txt[1].StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_ARMOR") || txt[1].StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_ACCESSARY"))
                            AdvanceItem.Elixir.Add(Item);
                        else if (txt[1].Contains("MAGICSTONE") || txt[1].StartsWith("ITEM_ETC_ARCHEMY_MAGICTABLET") || txt[1].StartsWith("ITEM_ETC_ARCHEMY_ATTRTABLET"))
                            AdvanceItem.Tablet.Add(Item);
                        else if (txt[1].StartsWith("ITEM_ETC_ARCHEMY_MATERIAL"))
                            AdvanceItem.Material.Add(Item);
                        else if (txt[1].StartsWith("ITEM_ETC_CURE_ALL") || txt[1].StartsWith("ITEM_ETC_ALL_SPOTION") || txt[1].StartsWith("ITEM_ETC_ALL_POTION") || txt[1].StartsWith("ITEM_ETC_MP_POTION") || txt[1].StartsWith("ITEM_ETC_HP_POTION"))
                            AdvanceItem.Potion.Add(Item);
                        else if (txt[1].StartsWith("ITEM_ETC_SCROLL_RETURN"))
                            AdvanceItem.Return.Add(Item);
                        else if (txt[1].Contains("ITEM_QSP") || txt[1].Contains("ITEM_QNO"))
                            AdvanceItem.Quest.Add(Item);
                        else if (txt[1].StartsWith("ITEM_ETC_AMMO_ARROW") || txt[1].StartsWith("ITEM_ETC_AMMO_BOLT"))
                            AdvanceItem.Arrow.Add(Item);
                        else if (txt[1].Contains("ITEM_MALL") || txt[1].Contains("ITEM_EVENT_CH") || txt[1].Contains("ITEM_EVENT_EU"))
                            AdvanceItem.Mall.Add(Item);
                    }
                }
                tr.Close();
                //Load items end
                //Load skill begin
                tr = new StreamReader(Environment.CurrentDirectory + @"\Data\Skill.txt");
                while ((input = tr.ReadLine()) != null)
                {
                    if (input != "" && !input.StartsWith("//"))
                    {
                        txt = input.Split(',');
                        Skills_Info.skillsidlist.Add(Globals.String_To_UInt32(txt[0]));
                        Skills_Info.skillstypelist.Add(txt[1]);
                        Skills_Info.skillsnamelist.Add(txt[2]);
                        Skills_Info.skillslevellist.Add(Convert.ToInt32(txt[3]));
                        Skills_Info.skillscasttimelist.Add(Convert.ToInt32(txt[6]));
                        Skills_Info.skillcooldownlist.Add(Convert.ToInt32(txt[7]));
                        Skills_Info.skillsmpreq.Add(Convert.ToInt32(txt[16]));
                        Skills_Info.skillsstatuslist.Add(0);
                        Skills_Info.skillbuffcheck.Add(Convert.ToByte(txt[17]));
                        Skills_Info.skillbufftime.Add(Convert.ToInt32(txt[8]));
                    }
                }
                tr.Close();
                //Load skill end
                //Load shop
                tr = new StreamReader(Environment.CurrentDirectory + @"\Data\Shop.txt");
                while ((input = tr.ReadLine()) != null)
                {
                    if (input != "" && !input.StartsWith("//"))
                    {
                        txt = input.Split(',');
                        string StoreName = txt[0];
                        for (int i = 0; i < Data.ShopTabData.Length; i++)
                        {
                            if (StoreName.StartsWith(Data.ShopTabData[i].StoreName))
                            {
                                for (int a = 0; a < Data.ShopTabData[i].Tab.Length; a++)
                                {
                                    if (Data.ShopTabData[i].Tab[a].TabName == StoreName)
                                    {
                                        Data.ShopTabData[i].Tab[a].ItemType[Convert.ToInt32(txt[2])] = txt[1];
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                tr.Close();
                //Load Shop End
                //Load Zone
                tr = new StreamReader(Environment.CurrentDirectory + @"\Data\Zone.txt");
                while ((input = tr.ReadLine()) != null)
                {
                    if (input != "" && !input.StartsWith("//"))
                    {
                        txt = input.Split(',');
                        Character.locationsector.Add(txt[0]);
                        Character.location.Add(txt[2]);
                    }
                }
                tr.Close();
                //End Load Zone
                //Load exp begin
                tr = new StreamReader(Environment.CurrentDirectory + @"\Data\EXP.txt");
                while ((input = tr.ReadLine()) != null)
                {
                    if (input != "" && !input.StartsWith("//"))
                    {
                        txt = input.Split(',');
                        Character.explist.Add(txt[0]);
                    }
                }
                tr.Close();
                //Load exp end
                //Load NPC 
                #region Truong An
                Spawns.NPCID.Add(240);
                Spawns.NPCType.Add("NPC_CH_POTION");
                Spawns.NPCID.Add(28);
                Spawns.NPCType.Add("NPC_CH_SMITH");
                Spawns.NPCID.Add(157);
                Spawns.NPCType.Add("NPC_CH_WAREHOUSE");
                Spawns.NPCID.Add(213);
                Spawns.NPCType.Add("NPC_CH_ACCESSORY");
                Spawns.NPCID.Add(191);
                Spawns.NPCType.Add("NPC_CH_HORSE");
                Spawns.NPCID.Add(48);
                Spawns.NPCType.Add("NPC_CH_ARMOR");
                Spawns.NPCID.Add(290);
                Spawns.NPCType.Add("NPC_CH_DOCTOR");
                #endregion

                #region Don Hoang
                Spawns.NPCID.Add(226);
                Spawns.NPCType.Add("NPC_WC_SMITH");
                Spawns.NPCID.Add(117);
                Spawns.NPCType.Add("NPC_WC_ARMOR");
                Spawns.NPCID.Add(47);
                Spawns.NPCType.Add("NPC_WC_ACCESSORY");
                Spawns.NPCID.Add(71);
                Spawns.NPCType.Add("NPC_WC_POTION");
                Spawns.NPCID.Add(27);
                Spawns.NPCType.Add("NPC_WC_HORSE");
                Spawns.NPCID.Add(267);
                Spawns.NPCType.Add("NPC_WC_GACHA_OPERATOR");
                Spawns.NPCID.Add(116);
                Spawns.NPCType.Add("NPC_WC_WAREHOUSE_M");
                Spawns.NPCID.Add(99);
                Spawns.NPCType.Add("NPC_WC_WAREHOUSE_W");
                #endregion

                #region Hoa Dien
                Spawns.NPCID.Add(541);
                Spawns.NPCType.Add("NPC_KT_ACCESSORY");
                Spawns.NPCID.Add(129);
                Spawns.NPCType.Add("NPC_KT_ARMOR");
                Spawns.NPCID.Add(22);
                Spawns.NPCType.Add("NPC_KT_SMITH");
                Spawns.NPCID.Add(105);
                Spawns.NPCType.Add("NPC_KT_POTION");
                Spawns.NPCID.Add(532);
                Spawns.NPCType.Add("NPC_KT_DESIGNER");
                Spawns.NPCID.Add(512);
                Spawns.NPCType.Add("NPC_KT_HORSE");
                Spawns.NPCID.Add(633);
                Spawns.NPCType.Add("NPC_KT_GACHA_OPERATOR");
                Spawns.NPCID.Add(495);
                Spawns.NPCType.Add("NPC_KT_WAREHOUSE");
                #endregion

                #region Samarkand
                Spawns.NPCID.Add(764);
                Spawns.NPCType.Add("NPC_CA_WAREHOUSE");
                Spawns.NPCID.Add(740);
                Spawns.NPCType.Add("NPC_CA_ACCESSORY");
                Spawns.NPCID.Add(714);
                Spawns.NPCType.Add("NPC_CA_POTION");
                Spawns.NPCID.Add(605);
                Spawns.NPCType.Add("NPC_CA_ARMOR");
                Spawns.NPCID.Add(573);
                Spawns.NPCType.Add("NPC_CA_SMITH");
                Spawns.NPCID.Add(796);
                Spawns.NPCType.Add("NPC_CA_HORSE");
                Spawns.NPCID.Add(781);
                Spawns.NPCType.Add("NPC_CA_SPECIAL");
                Spawns.NPCID.Add(832);
                Spawns.NPCType.Add("NPC_CH_GACHA_MACHINE");
                Spawns.NPCID.Add(786);
                Spawns.NPCType.Add("NPC_CA_MERCHANT");
                #endregion

                #region
                Spawns.NPCID.Add(308);
                Spawns.NPCType.Add("NPC_EU_SMITH");
                Spawns.NPCID.Add(516);
                Spawns.NPCType.Add("NPC_EU_POTION");
                Spawns.NPCID.Add(498);
                Spawns.NPCType.Add("NPC_EU_WAREHOUSE");
                Spawns.NPCID.Add(471);
                Spawns.NPCType.Add("NPC_EU_ACCESSORY");
                Spawns.NPCID.Add(552);
                Spawns.NPCType.Add("NPC_EU_SPECIAL");
                Spawns.NPCID.Add(558);
                Spawns.NPCType.Add("NPC_EU_GACHA_OPERATOR");
                Spawns.NPCID.Add(547);
                Spawns.NPCType.Add("NPC_EU_MERCHANT");
                Spawns.NPCID.Add(451);
                Spawns.NPCType.Add("NPC_EU_HORSE");
                #endregion
                //Load NPC End
                //Load Mob Name List
                tr = new StreamReader(Environment.CurrentDirectory + @"\Data\Advance Load Mob.txt");
                while ((input = tr.ReadLine()) != null)
                {
                    if (input != "" && !input.StartsWith("//"))
                    {
                        txt = input.Split(',');
                        if (txt[1].StartsWith("MOB"))
                        {
                            Monster.ModTypeList.Add(txt[1]);
                            Monster.ModNameList.Add(txt[2]);
                        }
                    }
                }
                tr.Close();
                //Load Mob Name End

                Globals.MainWindow.UpdateLogs("Successfully loaded data!");
            }
            catch (Exception)
            {
                Globals.MainWindow.UpdateLogs("Cannot load data files!");
            }
        }

        public static void CheckScripts()
        {
            byte error = 0;
            if (!File.Exists(Environment.CurrentDirectory + @"\Data\Scripts\ch_town.txt"))
            {
                error++;
            }
            if (!File.Exists(Environment.CurrentDirectory + @"\Data\Scripts\wc_town.txt"))
            {
                error++;
            }
            if (!File.Exists(Environment.CurrentDirectory + @"\Data\Scripts\kt_town.txt"))
            {
                error++;
            }
            if (!File.Exists(Environment.CurrentDirectory + @"\Data\Scripts\ca_town.txt"))
            {
                error++;
            }
            if (!File.Exists(Environment.CurrentDirectory + @"\Data\Scripts\eu_town.txt"))
            {
                error++;
            }
            if (error > 0)
            {
                Globals.MainWindow.UpdateLogs("Cannot Load Scripts!");
            }
        }
    }
}