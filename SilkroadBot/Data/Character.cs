using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LeoBot
{
    class Character
    {
        public static bool haslist = false;
        public static void CharacterList(Packet packet)
        {
            haslist = true;
            Globals.MainWindow.Clear(Globals.MainWindow.charlist);
            List<string> characters = new List<string>();
            byte type = packet.ReadUInt8();
            if (type == 1 || type == 3 || type == 4 || type == 5)
            {
                if (packet.ReadUInt8() == 2)
                {
                    packet.ReadUInt16();
                }
            }
            else if (type == 2)
            {
                if (packet.ReadUInt8() == 1)
                {
                    byte charcount = packet.ReadUInt8();
                    for (int i = 0; i < charcount; i++)
                    {

                        #region Main
                        packet.ReadUInt32(); //Model
                        characters.Add(packet.ReadAscii()); // Name
                        Globals.MainWindow.AddText(Globals.MainWindow.charlist, characters[i]);
                        packet.ReadUInt8(); //Volume/Height
                        packet.ReadUInt8(); //Level
                        packet.ReadUInt64(); //Exp
                        packet.ReadUInt16(); //STR
                        packet.ReadUInt16(); //INT
                        packet.ReadUInt16(); //Stats points
                        packet.ReadUInt32(); //Hp
                        packet.ReadUInt32(); //Mp
                        #endregion

                        #region Deletion
                        byte char_delete = packet.ReadUInt8();
                        if (char_delete == 1)
                        {
                            packet.ReadUInt32();
                        }
                        packet.ReadUInt8();
                        if (packet.ReadUInt8() == 1)
                        {
                            packet.ReadAscii();
                        }
                        packet.ReadUInt8();
                        #endregion

                        #region Items
                        int itemscount = packet.ReadUInt8();
                        for (int a = 0; a < itemscount; a++)
                        {
                            packet.ReadUInt32(); //Item ID
                            packet.ReadUInt8(); //Plus Value
                        }
                        #endregion

                        #region Avatars
                        int avatarcount = packet.ReadUInt8(); //Avatar count
                        for (int a = 0; a < avatarcount; a++)
                        {
                            packet.ReadUInt32(); // Avatar ID
                            packet.ReadUInt8();
                        }
                        #endregion
                    }
                }   
                else
                {
                    packet.ReadUInt16();
                }
            }
            if (Globals.MainWindow.Checked(Globals.MainWindow.characterselect) == true)
            {
                if (Globals.MainWindow.ReadText(Globals.MainWindow.charnameselect) != "" || Globals.MainWindow.ReadText(Globals.MainWindow.charnameselect) != null)
                {
                    Packet NewPacket = new Packet((ushort)0x7001);
                    NewPacket.WriteAscii(Globals.MainWindow.ReadText(Globals.MainWindow.charnameselect));
                    Proxy.ag_remote_security.Send(NewPacket);
                }
                else
                {
                    Globals.MainWindow.UpdateLogs("Please fill in name!");
                }
            }  
        }

        #region Data

        public struct CAVE_
        {
            public bool char_incave;
            public byte xsector;
            public float zcoord;
            public float xcoord;
        }
        public static CAVE_ cave = new CAVE_();

        public static List<string> locationsector = new List<string>();
        public static List<string> location = new List<string>();

        public static List<string> explist = new List<string>();

        public static Packet CharPacket;
        public static uint RefObjID;
        public static byte Scale;
        public static byte Level;
        public static byte MaxLevel;
        public static ulong ExpMax;
        public static ulong ExpOffset;
        public static uint SExpOffset;
        public static ushort RemainStatPoint;
        public static uint GatheredExpPoint;
        public static uint CurrentHP;
        public static uint CurrentMP;
        public static uint MaxHP;
        public static uint MaxMP;
        public static byte AutoInvestExp;
        public static byte DailyPK;
        public static ushort TotalPK;
        public static uint PKPenaltyPoint;
        public static byte BerserkLevel;
        public static byte InventorySize;
        public static byte InventoryItemsCount;
        public static byte AvatarInventorySize;
        public static byte AvatarInventoryItemCount;
        public static byte MasteryFlag;
        public static byte SkillFlag;
        public static uint UniqueID;
        public static string PlayerName;
        public static uint AccountID;
        public static int X;
        public static int Y;
        public static float WalkSpeed;
        public static float RunSpeed;
        public static float BerserkSpeed;
        public static float Gold;
        public static float SP;
        public static ushort STR;
        public static ushort INT;
        public static byte data_loaded = 0;
        public static System.Timers.Timer time = new System.Timers.Timer();
        public static System.Timers.Timer time1 = new System.Timers.Timer();
        #endregion

        public static void CharID(Packet packet)
        {
            Character.UniqueID = packet.ReadUInt32();
            Globals.MainWindow.SetText(Globals.MainWindow.charID, String.Format("{0:X8}", Character.UniqueID));
        }

        public static void CharData(Packet packet)
        {
            try
            {
                Globals.MainWindow.UnEnable(Globals.MainWindow.characterselect);
                Globals.MainWindow.UnEnable(Globals.MainWindow.charnameselect);
                Globals.MainWindow.UnEnable(Globals.MainWindow.charlist);
                Globals.MainWindow.UnEnable(Globals.MainWindow.selectcharbutton);

                #region ResetData
                Data.inventorycount.Clear();
                Data.inventoryid.Clear();
                Data.inventoryslot.Clear();
                Data.inventorytype.Clear();
                Data.inventorydurability.Clear();
                Data.inventorylevel.Clear();
                Data.inventoryname.Clear();
                Data.maxdurability.Clear();

                Data.storageid.Clear();
                Data.storagetype.Clear();
                Data.storagecount.Clear();
                Data.storageslot.Clear();
                Data.storagedurability.Clear();
                Data.storagelevel.Clear();
                Data.storagename.Clear();

                Skill.Skills.Clear();

                Data.char_horseid = 0;
                Data.storageopened = 0;
                #endregion

                #region Main
                packet.ReadUInt32();
                Character.RefObjID = packet.ReadUInt32();
                Character.Scale = packet.ReadUInt8();
                Character.Level = packet.ReadUInt8();
                Character.MaxLevel = packet.ReadUInt8();
                int maxlvl = (int)Character.Level - 1;
                Character.ExpMax = Convert.ToUInt64(Character.explist[maxlvl]);
                Character.ExpOffset = packet.ReadUInt64();
                Character.SExpOffset = packet.ReadUInt32();
                Character.Gold = packet.ReadUInt64();
                Character.SP = packet.ReadUInt32();
                Character.RemainStatPoint = packet.ReadUInt16();
                Character.BerserkLevel = packet.ReadUInt8();
                Character.GatheredExpPoint = packet.ReadUInt32();
                Character.CurrentHP = packet.ReadUInt32();
                Character.CurrentMP = packet.ReadUInt32();
                Character.AutoInvestExp = packet.ReadUInt8();
                Character.DailyPK = packet.ReadUInt8();
                Character.TotalPK = packet.ReadUInt16();
                Character.PKPenaltyPoint = packet.ReadUInt32();
                packet.ReadUInt8();
                Globals.MainWindow.UpdateBar();
                Globals.MainWindow.SetText(Globals.MainWindow.level, Convert.ToString(Level));
                Globals.MainWindow.SetText(Globals.MainWindow.gold, Convert.ToString(Character.Gold));
                Globals.MainWindow.SetText(Globals.MainWindow.berserklevel, String.Format("{0}/5", Character.BerserkLevel));
                Globals.MainWindow.SetText(Globals.MainWindow.currentsp, Convert.ToString(Character.SP));
                packet.ReadUInt8();
                #endregion

                #region Items
                Character.InventorySize = packet.ReadUInt8();
                Character.InventoryItemsCount = packet.ReadUInt8();
                for (int i = 0; i < Character.InventoryItemsCount; i++)
                {
                    byte ItemSlot = packet.ReadUInt8();
                    uint ItemRentType = packet.ReadUInt32();
                    switch (ItemRentType)
                    {
                        case 1:
                            packet.ReadUInt16();
                            packet.ReadUInt32();
                            packet.ReadUInt32();
                            break;
                        case 2:
                            packet.ReadUInt16();
                            packet.ReadUInt16();
                            packet.ReadUInt32();
                            break;
                        case 3:
                            packet.ReadUInt16();
                            packet.ReadUInt32();
                            packet.ReadUInt32();
                            packet.ReadUInt16();
                            packet.ReadUInt32();
                            break;
                    }

                    uint ItemRefItemID = packet.ReadUInt32();
                    int index = Items_Info.itemsidlist.IndexOf(ItemRefItemID);
                    string type = Items_Info.itemstypelist[index];
                    string name = Items_Info.itemsnamelist[index];
                    Data.inventoryname.Add(name);
                    Data.inventoryslot.Add(ItemSlot);
                    Data.inventorytype.Add(type);
                    Data.inventorylevel.Add(Items_Info.itemslevellist[index]);
                    Data.maxdurability.Add(Items_Info.itemsdurabilitylist[index]);
                    Data.inventoryid.Add(ItemRefItemID);
                    if (type.StartsWith("ITEM_CH") || type.StartsWith("ITEM_EU") || type.StartsWith("ITEM_MALL_AVATAR") || type.StartsWith("ITEM_ETC_E060529_GOLDDRAGONFLAG") || type.StartsWith("ITEM_EVENT_CH") || type.StartsWith("ITEM_EVENT_EU") || type.StartsWith("ITEM_EVENT_AVATAR_W_NASRUN") || type.StartsWith("ITEM_EVENT_AVATAR_M_NASRUN"))
                    {
                        byte OptLevel = packet.ReadUInt8();
                        ulong Variance = packet.ReadUInt64();
                        uint Durability = packet.ReadUInt32();
                        Data.inventorydurability.Add(Durability);
                        byte MagParamNum = packet.ReadUInt8();
                        for (int k = 0; k < MagParamNum; k++)
                        {
                            uint MagParamType = packet.ReadUInt32();
                            uint MagParamValue = packet.ReadUInt32();
                        }
                        byte OptType = packet.ReadUInt8();
                        byte OptCount = packet.ReadUInt8(); //1 => Socket
                        for (int l = 0; l < OptCount; l++)
                        {
                            byte OptionSlot = packet.ReadUInt8();
                            uint OptionID = packet.ReadUInt32();
                            uint OptionnParam1 = packet.ReadUInt32(); //=> Reference to Socket
                        }
                        byte OptType1 = packet.ReadUInt8(); //2 => Advanced Elixir      
                        byte OptCount1 = packet.ReadUInt8();
                        for (int m = 0; m < OptCount1; m++)
                        {
                            byte OptionSlot1 = packet.ReadUInt8();
                            uint OptionID1 = packet.ReadUInt32();
                            uint OptionOptValue = packet.ReadUInt32(); //=> Advanced elixir in effect     
                        }
                        Data.inventorycount.Add(1);
                    }
                    else if (((Data.Types.attack_spawn_types.IndexOf(type)) != -1) || (Data.Types.attack_types.IndexOf(type) != -1))
                    {
                        byte Status = packet.ReadUInt8(); //1 =>  Unsumonned, 2 => Summonned, 3 => Alive, 4 => Dead                                     
                        if (Status == 2 || Status == 3 || Status == 4)
                        {
                            uint PetID = packet.ReadUInt32();
                            string PetName = packet.ReadAscii();
                            packet.ReadUInt8();
                        }
                        Data.inventorycount.Add(1);
                        Data.inventorydurability.Add(0);
                    }
                    else if (Data.Types.grabpet_spawn_types.IndexOf(type) != -1 || Data.Types.grab_types.IndexOf(type) != -1)
                    {
                        byte Status = packet.ReadUInt8(); //1 =>  Unsumonned, 2 => Summonned, 3 => Alive, 4 => Dead
                        if (Status == 2 || Status == 3 || Status == 4)
                        {
                            uint PetID = packet.ReadUInt32();
                            string PetName = packet.ReadAscii();
                            uint SecondsToRentEndTime = packet.ReadUInt32();
                            packet.ReadUInt8();
                        }
                        Data.inventorycount.Add(1);
                        Data.inventorydurability.Add(0);
                    }
                    else if (type == "ITEM_ETC_TRANS_MONSTER")
                    {
                        packet.ReadUInt32();
                        Data.inventorycount.Add(1);
                        Data.inventorydurability.Add(0);
                    }
                    else if (type.Contains("ITEM_ETC_ARCHEMY_ATTRSTONE") || type.Contains("ITEM_ETC_ARCHEMY_MAGICSTONE"))
                    {
                        ushort StoneStackCount = packet.ReadUInt16();
                        if (type.Contains("ITEM_ETC_ARCHEMY_MAGICSTONE_LUCK") == false)
                        {
                            byte AttributeAssimilationProbability = packet.ReadUInt8();
                        }
                        Data.inventorycount.Add(StoneStackCount);
                        Data.inventorydurability.Add(0);
                    }
                    else if (type.StartsWith("ITEM_MALL_MAGIC_CUBE"))
                    {
                        uint StoredItemCount = packet.ReadUInt32();
                        Data.inventorycount.Add(1);
                        Data.inventorydurability.Add(0);
                    }
                    else if (type.Contains("ITEM_MALL_GACHA_CARD_WIN") || type.Contains("ITEM_MALL_GACHA_CARD_LOSE"))
                    {
                        ushort CouponStackCount = packet.ReadUInt16();
                        byte CouponMagParamNum = packet.ReadUInt8();
                        for (int n = 0; n < CouponMagParamNum; n++)
                        {
                            ulong CouponMagParamNumValue = packet.ReadUInt64();
                        }
                        Data.inventorycount.Add(CouponStackCount);
                        Data.inventorydurability.Add(0);
                    }
                    else
                    {
                        ushort ItemStackCount = packet.ReadUInt16();
                        Data.inventorycount.Add(ItemStackCount);
                        Data.inventorydurability.Add(0);
                    }
                }
                ItemsCount.CountManager();

                Character.AvatarInventorySize = packet.ReadUInt8();
                Character.AvatarInventoryItemCount = packet.ReadUInt8();
                for (int i = 0; i < Character.AvatarInventoryItemCount; i++)
                {
                    byte AvatarSlot = packet.ReadUInt8(); //Slot
                    packet.ReadUInt32();
                    uint AvatarID = packet.ReadUInt32();
                    int index = Items_Info.itemsidlist.IndexOf(AvatarID);
                    string type = Items_Info.itemstypelist[index];
                    string name = Items_Info.itemsnamelist[index];
                    byte ItemPlus = packet.ReadUInt8();
                    packet.ReadUInt64();
                    packet.ReadUInt32();
                    byte AvatarMagParamNum = packet.ReadUInt8();
                    for (int o = 0; o < AvatarMagParamNum; o++)
                    {
                        packet.ReadUInt32();
                        packet.ReadUInt32();
                    }
                    packet.ReadUInt32();
                }
                #endregion

                #region MasteryFlag
                packet.ReadUInt8();
                Character.MasteryFlag = packet.ReadUInt8(); //0 = done, 1 = mastery
                while (MasteryFlag == 1)
                {
                    uint MasteryID = packet.ReadUInt32();
                    byte MasteryLevel = packet.ReadUInt8();

                    MasteryFlag = packet.ReadUInt8(); //0 = done, 1 = mastery
                }
                packet.ReadUInt8();

                Character.SkillFlag = packet.ReadUInt8(); //0 = done, 1 = mastery
                while (SkillFlag == 1)
                {
                    uint SkillID = packet.ReadUInt32();
                    byte SkillEnable = packet.ReadUInt8();
                    SkillFlag = packet.ReadUInt8(); //0 = done, 1 = mastery
                    int index = Skills_Info.skillsidlist.IndexOf(SkillID);
                    System.Timers.Timer Timer = new System.Timers.Timer();
                    Skill NewSkill = new Skill(SkillID, Skills_Info.skillsnamelist[index], Skills_Info.skillstypelist[index], Skills_Info.skillslevellist[index], 
                        Skills_Info.skillbuffcheck[index], 0, Skills_Info.skillscasttimelist[index],
                        Skills_Info.skillcooldownlist[index], Skills_Info.skillsmpreq[index], Skills_Info.skillbufftime[index], 0, Timer);

                    Skill.Skills.Add(NewSkill);                   
                }
                Globals.MainWindow.Updatehealskill();
                Globals.MainWindow.UpdateSkillList();
                #endregion

                #region SkippingQuestPart
                //Read every Byte the compare them to the UniqueID got from opcode 3020, if equal then stop, if not keep reading the ByteArray
                Packet charid = new Packet(3020);
                charid.WriteUInt32(Character.UniqueID);
                charid.Lock();
                byte idpart1 = charid.ReadUInt8();
                byte idpart2 = charid.ReadUInt8();
                byte idpart3 = charid.ReadUInt8();
                byte idpart4 = charid.ReadUInt8();
                while (true)
                {
                    if (packet.ReadUInt8() == idpart1)
                    {
                        if (packet.ReadUInt8() == idpart2)
                        {
                            if (packet.ReadUInt8() == idpart3)
                            {
                                if (packet.ReadUInt8() == idpart4)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region CharacterData
                //Unique ID here has been taken by the comparison #Skipping Quest Part
                byte xsec = packet.ReadUInt8();
                byte ysec = packet.ReadUInt8();                
                float xcoord = packet.ReadSingle();
                float zcoord = packet.ReadSingle();
                float ycoord = packet.ReadSingle();
                if (ysec == 0x80)
                {
                    cave.char_incave = true;
                    cave.xsector = xsec;
                    cave.zcoord = zcoord;
                    cave.xcoord = xcoord;
                }
                else
                {
                    cave.char_incave = false;
                }
                Character.X = Globals.CalculatePositionX(xsec, xcoord);
                Character.Y = Globals.CalculatePositionY(ysec, ycoord);
                string sector = (String.Format("{0:X2}{1:X2}", xsec, ysec));
                int indexx = Character.locationsector.IndexOf(sector);
                if (indexx != -1)
                {
                    string location = Character.location[indexx];
                    PortConfigs.TrainWindow.Label(PortConfigs.TrainWindow.location, location);
                }
                Globals.MainWindow.SetText(Globals.MainWindow.xlabel, Convert.ToString(Character.X));
                Globals.MainWindow.SetText(Globals.MainWindow.ylabel, Convert.ToString(Character.Y));
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
                Character.WalkSpeed = packet.ReadSingle();
                Character.RunSpeed = packet.ReadSingle();
                Character.BerserkSpeed = packet.ReadSingle();
                byte ActiveBuffCount = packet.ReadUInt8();
                for (int a = 0; a < ActiveBuffCount; a++)
                {
                    uint skillid = packet.ReadUInt32();
                    int buffindex = Skills_Info.skillsidlist.IndexOf(skillid);
                    string type = Skills_Info.skillstypelist[buffindex];
                    if (type.StartsWith("SKILL_EU_CLERIC_RECOVERYA_GROUP") || type.StartsWith("SKILL_EU_BARD_BATTLAA_GUARD") || type.StartsWith("SKILL_EU_BARD_DANCEA") || type.StartsWith("SKILL_EU_BARD_SPEEDUPA_HITRATE"))
                    {
                        packet.ReadUInt8();
                    }
                }
                Character.PlayerName = packet.ReadAscii();
                Globals.MainWindow.SetText(Globals.MainWindow.charname, Character.PlayerName);
                #endregion

                #region Job
                string JobTitle = packet.ReadAscii();
                byte JobType = packet.ReadUInt8(); //0 => None, 1 => Trader, 2 => Thief, 3 => Hunter
                byte JobLevel = packet.ReadUInt8();
                uint JobEXP = packet.ReadUInt8();
                uint JobContribution = packet.ReadUInt32();
                uint JobReward = packet.ReadUInt32();
                packet.ReadUInt8();
                packet.ReadUInt8();
                packet.ReadUInt8();
                packet.ReadUInt8();
                packet.ReadUInt8();
                packet.ReadUInt8();
                packet.ReadUInt8();
                packet.ReadUInt8();
                packet.ReadUInt8();
                packet.ReadUInt8();
                packet.ReadUInt8();
                packet.ReadUInt8();
                packet.ReadUInt8();
                packet.ReadUInt8();
                packet.ReadUInt8();
                Character.AccountID = packet.ReadUInt32();
                Globals.MainWindow.SetText(Globals.MainWindow.accID, String.Format("{0:X8}", Character.AccountID));
                Globals.MainWindow.UpdateInventory();
                #endregion

                Globals.MainWindow.SetText(Globals.MainWindow.status, "Normal");
                Globals.MainWindow.SetText(Globals.MainWindow.currentexp, Convert.ToString(Character.ExpOffset));
                Globals.MainWindow.SetText(Globals.MainWindow.exppercent, String.Format("{0}%", Character.ExpOffset * 100 / ExpMax));
                Globals.MainWindow.UpdateTray();
                Globals.MainWindow.Enable(Globals.MainWindow.startbot);  

                if (data_loaded == 0)
                {
                    data_loaded = 1;
                    Data.Statistic.sp_begin = (int)Character.SP;
                    Data.Statistic.gold_begin = (long)Character.Gold;
                    Configs.ReadConfigs();                 
                    System.Threading.Thread time_thread = new Thread(StartTimer);
                    time_thread.Start();
                    System.Threading.Thread time_thread1 = new Thread(StartTimer1);
                    time_thread1.Start();
                }

                for (int i = 0; i < Skill.Skills.Count; i++)
                {
                    Skill.Skills[i].index = i;
                }

                for (int i = 0; i < Globals.MainWindow.generalimbue.Items.Count; i++)
                {
                    foreach (Skill skill in Skill.Skills)
                    {
                        if (skill.Name == Globals.MainWindow.generalimbue.Items[i].ToString())
                        {
                            Skill.Imbue.Add(skill);
                            break;
                        }
                    }
                }

                for (int i = 0; i < Globals.MainWindow.partyimbue.Items.Count; i++)
                {
                    foreach (Skill skill in Skill.Skills)
                    {
                        if (skill.Name == Globals.MainWindow.partyimbue.Items[i].ToString())
                        {
                            Skill.PartyImbue.Add(skill);
                            break;
                        }
                    }
                }

                for (int i = 0; i < Globals.MainWindow.generalattack.Items.Count; i++)
                {
                    foreach (Skill skill in Skill.Skills)
                    {
                        if (skill.Name == Globals.MainWindow.generalattack.Items[i].ToString())
                        {
                            Skill.AttackSkills.Add(skill);
                            break;
                        }
                    }
                }

                for (int i = 0; i < Globals.MainWindow.partyattack.Items.Count; i++)
                {
                    foreach (Skill skill in Skill.Skills)
                    {
                        if (skill.Name == Globals.MainWindow.partyattack.Items[i].ToString())
                        {
                            Skill.PartyAttackSkills.Add(skill);
                            break;
                        }
                    }
                }

                for (int i = 0; i < Globals.MainWindow.buff.Items.Count; i++)
                {
                    foreach (Skill skill in Skill.Skills)
                    {
                        if (skill.Name == Globals.MainWindow.buff.Items[i].ToString())
                        {
                            skill.bufftype = 1;
                            skill.BuffWaiting = 1;
                            break;                            
                        }
                    }
                    Buffas.buff_waiting = true;
                }

                for (int i = 0; i < Globals.MainWindow.secondbuff.Items.Count; i++)
                {
                    foreach (Skill skill in Skill.Skills)
                    {
                        if (skill.Name == Globals.MainWindow.secondbuff.Items[i].ToString())
                        {
                            skill.bufftype = 3;
                            skill.BuffWaiting = 1;
                            break;
                        }
                    }
                    Buffas.buff_waiting = true;
                }

                if (Globals.MainWindow.Checked(Globals.MainWindow.autoparty) == true)
                {
                    Party.CreateParty();
                }

                if (Data.bot == true)
                {
                    StartLooping.Start();
                }
            }
            catch { }
        }

        static void StartTimer()
        {
            time.Elapsed += new System.Timers.ElapsedEventHandler(time_Elapsed);
            time.Interval += 10000;
            time.Start();
            time.Enabled = true;
        }

        static void StartTimer1()
        {
            time1.Elapsed += new System.Timers.ElapsedEventHandler(time_Elapsed1);
            time1.Interval += 1000;
            time1.Start();
            time1.Enabled = true;
        }

        static void time_Elapsed1(object sender, System.Timers.ElapsedEventArgs e)
        {
            Data.Statistic.second++;
            if (Data.Statistic.second == 60)
            {
                Data.Statistic.second = 0;
                Data.Statistic.minute++;
                if (Data.Statistic.minute == 60)
                {
                    Data.Statistic.minute = 0;
                    Data.Statistic.hour++;
                }
            }
            Globals.MainWindow.SetText(Globals.MainWindow.timecount, String.Format("{0} : {1} : {2}", Data.Statistic.hour, Data.Statistic.minute, Data.Statistic.second));        
        }

        static void time_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {         
            Data.Statistic.time_elapsed += 10;
            System.Threading.Thread.Sleep(10);
            Globals.MainWindow.SetText(Globals.MainWindow.gold_difference, Convert.ToString(((int)Character.Gold - Data.Statistic.gold_begin)));
            System.Threading.Thread.Sleep(10);
            int total_sp = (int)Character.SP - Data.Statistic.sp_begin;
            System.Threading.Thread.Sleep(10);
            float sp_minute = (float)Math.Round((float)(total_sp / Data.Statistic.time_elapsed * 60), 2);
            System.Threading.Thread.Sleep(10);
            float sp_hour = (float)Math.Round((float)(total_sp / Data.Statistic.time_elapsed * 3600), 2);
            System.Threading.Thread.Sleep(10);
            Globals.MainWindow.SetText(Globals.MainWindow.spgainedtotal, (Character.SP - Data.Statistic.sp_begin).ToString());
            System.Threading.Thread.Sleep(10);
            Globals.MainWindow.SetText(Globals.MainWindow.spgainedperhour, sp_hour.ToString());
            System.Threading.Thread.Sleep(10);
            Globals.MainWindow.SetText(Globals.MainWindow.spgainedpermin, sp_minute.ToString());
            System.Threading.Thread.Sleep(10);
            Globals.MainWindow.SetText(Globals.MainWindow.mobkilled, Data.Statistic.mob_killed.ToString());
            System.Threading.Thread.Sleep(10);
            Globals.MainWindow.SetText(Globals.MainWindow.diedcount, Data.Statistic.died_count.ToString());
            System.Threading.Thread.Sleep(10);
            Globals.MainWindow.SetText(Globals.MainWindow.returncount, Data.Statistic.return_count.ToString());
        }

        public static void UpdateInfo(Packet packet)
        {
            byte code = packet.ReadUInt8();
            switch (code)
            {
                case 1:

                    #region Gold Update
                    Character.Gold = packet.ReadUInt64();
                    Globals.MainWindow.SetText(Globals.MainWindow.gold, Convert.ToString(Convert.ToUInt64(Character.Gold)));
                    break;
                    #endregion

                case 2:

                    #region SP Update
                    Character.SP = packet.ReadUInt32();
                    Globals.MainWindow.SetText(Globals.MainWindow.currentsp, Convert.ToString(Character.SP));
                    break;
                    #endregion

                case 4:

                    #region Zerk Update
                    Character.BerserkLevel = packet.ReadUInt8();
                    Globals.MainWindow.SetText(Globals.MainWindow.berserklevel, String.Format("{0}/5", Character.BerserkLevel));
                    break;
                    #endregion

            }
        }

        public static void CharacterInfo(Packet packet)
        {
            {
                packet.ReadUInt64();
                packet.ReadUInt64();
                packet.ReadUInt16();
                packet.ReadUInt16();
                packet.ReadUInt16();
                packet.ReadUInt16();
                Character.MaxHP = packet.ReadUInt32();
                Character.MaxMP = packet.ReadUInt32();
                Character.STR = packet.ReadUInt16();
                Character.INT = packet.ReadUInt16();
                
                if (CurrentMP > MaxMP)
                {
                    CurrentMP = MaxMP;
                }
                if (CurrentHP > MaxHP)
                {
                    CurrentHP = MaxHP;
                }

                Data.returning = 0;
                Data.dead = false;
                Globals.MainWindow.UpdateBar();
            }
        }

        public static void SpeedUpdate(Packet packet)
        {
            uint ID = packet.ReadUInt32(); // Char ID
            if (Character.UniqueID == ID)
            {
                packet.ReadSingle(); // Walk Speed
                float Speed = packet.ReadSingle(); // Run Speed
                Character.RunSpeed = Speed;
            }             
        }

        public static void LevelUp(Packet packet)
        {

            if (packet.ReadUInt32() == Character.UniqueID)
            {
                Character.Level += 1;
                int maxlvl = (int)Character.Level - 1;
                Character.ExpMax = Convert.ToUInt64(Character.explist[maxlvl]);
            }
        }

        public static void ExpSpUpdate(Packet packet)
        {
            packet.ReadUInt32();
            ulong exp = packet.ReadUInt64();
            packet.ReadUInt64(); //SP XP
            if (Character.ExpOffset + exp >= Character.ExpMax)
            {
                int maxlvl = (int)Character.Level - 2;
                ulong exp_max = Convert.ToUInt64(Character.explist[maxlvl]);
                ulong new_exp = (Character.ExpOffset + exp) - exp_max;
                Character.ExpOffset = new_exp;
                Globals.MainWindow.SetText(Globals.MainWindow.currentexp, Convert.ToString(Character.ExpOffset));
                Globals.MainWindow.SetText(Globals.MainWindow.exppercent, String.Format("{0}%", Character.ExpOffset * 100 / ExpMax));
            }
            else
            {
                Character.ExpOffset = Character.ExpOffset + exp;
                Globals.MainWindow.SetText(Globals.MainWindow.currentexp, Convert.ToString(Character.ExpOffset));
                Globals.MainWindow.SetText(Globals.MainWindow.exppercent, String.Format("{0}%", Character.ExpOffset * 100 / ExpMax));
            }
            Data.Statistic.mob_killed++;
        }
    }
}
