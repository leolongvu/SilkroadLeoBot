using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace LeoBot
{
    class Mobs_Info
    {
        public static List<uint> mobsidlist = new List<uint>();
        public static List<string> mobstypelist = new List<string>();
        public static List<string> mobsnamelist = new List<string>();
        public static List<byte> mobslevellist = new List<byte>();
        public static List<uint> mobshplist = new List<uint>();
    }
    class Skills_Info
    {
        public static List<uint> skillsidlist = new List<uint>();
        public static List<string> skillstypelist = new List<string>();
        public static List<string> skillsnamelist = new List<string>();
        public static List<int> skillslevellist = new List<int>();
        public static List<int> skillsmpreq = new List<int>();
        public static List<int> skillsstatuslist = new List<int>();
        public static List<int> skillscasttimelist = new List<int>();
        public static List<int> skillcooldownlist = new List<int>();
        public static List<byte> skillbuffcheck = new List<byte>();
        public static List<int> skillbufftime = new List<int>();
    }
    class Items_Info
    {
        public static List<uint> itemsidlist = new List<uint>();
        public static List<string> itemstypelist = new List<string>();
        public static List<string> itemsnamelist = new List<string>();
        public static List<byte> itemslevellist = new List<byte>();
        public static List<ushort> items_maxlist = new List<ushort>();
        public static List<uint> itemsdurabilitylist = new List<uint>();
    }

    class Data
    {
        public static string selectednpctype = null;
        public static string sro_path = "";
        public static int mode = 0;
        public static string walkscriptpath = null;
        public static bool hasparty = false;

        public static List<string> ServerName = new List<string>();
        public static List<ushort> ServerID = new List<ushort>();

        public struct Types_
        {
            public List<string> grab_types;
            public List<string> grabpet_spawn_types;
            public List<string> attack_types;
            public List<string> attack_spawn_types;
        }
        public static Types_ Types = new Types_();

        public struct Statistc_
        {
            public float time_elapsed;
            public int mob_killed;
            public int sp_begin;
            public long gold_begin;
            public int return_count;
            public int died_count;

            public byte second;
            public byte minute;
            public byte hour;
        }
        public static Statistc_ Statistic = new Statistc_();  

        public static void InitializeTypes()
        {
            //Grab Pets
            Types.grab_types = new List<string>();
            Types.grab_types.Add("COS_P_SPOT_RABBIT");
            Types.grab_types.Add("COS_P_RABBIT");
            Types.grab_types.Add("COS_P_GGLIDER");
            Types.grab_types.Add("COS_P_MYOWON");
            Types.grab_types.Add("COS_P_SEOWON");
            Types.grab_types.Add("COS_P_RACCOONDOG");
            Types.grab_types.Add("COS_P_CAT");
            Types.grab_types.Add("COS_P_BROWNIE");
            Types.grab_types.Add("COS_P_PINKPIG");
            Types.grab_types.Add("COS_P_GOLDPIG");
            Types.grab_types.Add("COS_P_FOX");
            //Grab Pets

            //Attack Pets
            Types.attack_types = new List<string>();
            Types.attack_types.Add("COS_P_BEAR");
            Types.attack_types.Add("COS_P_FOX");
            Types.attack_types.Add("COS_P_PENGUIN");
            Types.attack_types.Add("COS_P_WOLF_WHITE_SMALL");
            Types.attack_types.Add("COS_P_WOLF_WHITE");
            Types.attack_types.Add("COS_P_WOLF");
            Types.attack_types.Add("COS_P_RAVEN");
            Types.attack_types.Add("COS_P_KANGAROO");
            //Attack Pets

            //Attack Pets Item
            Types.attack_spawn_types = new List<string>();
            Types.attack_spawn_types.Add("ITEM_COS_P_FOX_SCROLL");
            Types.attack_spawn_types.Add("ITEM_COS_P_KANGAROO_SCROLL");
            Types.attack_spawn_types.Add("ITEM_COS_P_RAVEN_SCROLL");
            Types.attack_spawn_types.Add("ITEM_COS_P_BEAR_SCROLL");
            Types.attack_spawn_types.Add("ITEM_COS_P_FLUTE");
            Types.attack_spawn_types.Add("ITEM_COS_P_FLUTE_SILK");
            Types.attack_spawn_types.Add("ITEM_COS_P_FLUTE_WHITE");
            Types.attack_spawn_types.Add("ITEM_COS_P_FLUTE_WHITE_SMALL");
            Types.attack_spawn_types.Add("ITEM_COS_P_PENGUIN_SCROLL");
            //Attack Pets Item

            //Grab Pets Item
            Types.grabpet_spawn_types = new List<string>();
            Types.grabpet_spawn_types.Add("ITEM_COS_P_SPOT_RABBIT_SCROLL");
            Types.grabpet_spawn_types.Add("ITEM_COS_P_RABBIT_SCROLL");
            Types.grabpet_spawn_types.Add("ITEM_COS_P_RABBIT_SCROLL_SILK");
            Types.grabpet_spawn_types.Add("ITEM_COS_P_GGLIDER_SCROLL");
            Types.grabpet_spawn_types.Add("ITEM_COS_P_MYOWON_SCROLL");
            Types.grabpet_spawn_types.Add("ITEM_COS_P_SEOWON_SCROLL");
            Types.grabpet_spawn_types.Add("ITEM_COS_P_RACCOONDOG_SCROLL");
            Types.grabpet_spawn_types.Add("ITEM_COS_P_BROWNIE_SCROLL");
            Types.grabpet_spawn_types.Add("ITEM_COS_P_CAT_SCROLL");
            Types.grabpet_spawn_types.Add("ITEM_COS_P_PINKPIG_SCROLL");
            Types.grabpet_spawn_types.Add("ITEM_COS_P_GOLDPIG_SCROLL");
            Types.grabpet_spawn_types.Add("ITEM_COS_P_GOLDPIG_SCROLL_SILK");
            //Grab Pets Item
        }

        public static uint char_horseid = 0;
        public static float char_horsespeed = 0;
        public static uint char_attackpetid = 0;
        public static string char_attackpetname = "";
        public static uint char_grabpetid = 0;
        public static string char_grabpetname = "";

        public static List<byte> inventoryslot = new List<byte>();
        public static List<uint> inventoryid = new List<uint>();
        public static List<string> inventorytype = new List<string>();
        public static List<ushort> inventorycount = new List<ushort>();
        public static List<uint> inventorydurability = new List<uint>();
        public static List<byte> inventorylevel = new List<byte>();
        public static List<string> inventoryname = new List<string>();
        public static List<uint> maxdurability = new List<uint>();

        public static List<byte> storageslot = new List<byte>();
        public static List<uint> storageid = new List<uint>();
        public static List<string> storagetype = new List<string>();
        public static List<ushort> storagecount = new List<ushort>();
        public static List<uint> storagedurability = new List<uint>();
        public static List<byte> storagelevel = new List<byte>();
        public static List<string> storagename = new List<string>();
        public static List<uint> strmaxdurability = new List<uint>();

        public static ulong storagegold = 0;
        public static int storageopened = 0;

        public static int groupespawncount = 0;
        public static int groupespawninfo = 0;
        public static int groupespawned = 0;

        public static string f_wep_name = null;
        public static string s_wep_name = null;

        #region Logic
        public static bool bot = false;
        public static byte loopend = 0;
        public static bool loop = false;
        public static string loopaction = null;
        #endregion

        #region WorldInfo
        public static uint teleportdata = 0;
        public static byte returning = 0;
        public static bool dead = false;
        #endregion

        public enum enumMobType : byte
        {
            Normal = 0x00,
            Champion = 0x01,
            Unique = 0x03,
            Giant = 0x04,
            Titan = 0x05,
            Elite1 = 0x06,
            Elite2 = 0x07,
            Party = 0x10, //?
            PartyChampion = 0x11, //?
            PartyGiant = 0x14 //?
        }

        public struct ShopTabData_
        {
            public string StoreName;

            public struct Tab_
            {
                public string TabName;
                public string[] ItemType;
            }
            public Tab_[] Tab;
        }
        public static ShopTabData_[] ShopTabData;

        #region Inventory
        public struct ItemsCount_
        {
            public uint hp_pots;
            public uint mp_pots;
            public uint arrows;
            public uint bolts;
            public uint uni_pills;
            public uint pet_hp;
            public uint horse;
            public uint hgp;
            public uint speed_pots;
            public uint return_scrool;
            public byte items_count;
            public uint vigor;
            public uint glass;
            public uint petuni;
        }
        public static ItemsCount_ itemscount = new ItemsCount_();
        #endregion

        public static void LoadShopTabData()
        {
            ShopTabData = new ShopTabData_[25];

            #region Jangan
            ShopTabData[0].StoreName = "STORE_CH_ACCESSORY";
            ShopTabData[0].Tab = new ShopTabData_.Tab_[3];
            ShopTabData[0].Tab[0].TabName = "STORE_CH_ACCESSORY_TAB1";
            ShopTabData[0].Tab[1].TabName = "STORE_CH_ACCESSORY_TAB2";
            ShopTabData[0].Tab[2].TabName = "STORE_CH_ACCESSORY_TAB3";

            ShopTabData[1].StoreName = "STORE_CH_ARMOR";
            ShopTabData[1].Tab = new ShopTabData_.Tab_[6];
            ShopTabData[1].Tab[0].TabName = "STORE_CH_ARMOR_TAB1";
            ShopTabData[1].Tab[1].TabName = "STORE_CH_ARMOR_TAB2";
            ShopTabData[1].Tab[2].TabName = "STORE_CH_ARMOR_TAB3";
            ShopTabData[1].Tab[3].TabName = "STORE_CH_ARMOR_TAB4";
            ShopTabData[1].Tab[4].TabName = "STORE_CH_ARMOR_TAB5";
            ShopTabData[1].Tab[5].TabName = "STORE_CH_ARMOR_TAB6";

            ShopTabData[2].StoreName = "STORE_CH_POTION";
            ShopTabData[2].Tab = new ShopTabData_.Tab_[1];
            ShopTabData[2].Tab[0].TabName = "STORE_CH_POTION_TAB1";

            ShopTabData[3].StoreName = "STORE_CH_SMITH";
            ShopTabData[3].Tab = new ShopTabData_.Tab_[3];
            ShopTabData[3].Tab[0].TabName = "STORE_CH_SMITH_TAB1";
            ShopTabData[3].Tab[1].TabName = "STORE_CH_SMITH_TAB2";
            ShopTabData[3].Tab[2].TabName = "STORE_CH_SMITH_TAB3";

            ShopTabData[4].StoreName = "STORE_CH_HORSE";
            ShopTabData[4].Tab = new ShopTabData_.Tab_[4];
            ShopTabData[4].Tab[0].TabName = "STORE_CH_HORSE_TAB1";
            ShopTabData[4].Tab[1].TabName = "STORE_CH_HORSE_TAB2";
            ShopTabData[4].Tab[2].TabName = "STORE_CH_HORSE_TAB3";
            ShopTabData[4].Tab[3].TabName = "STORE_CH_HORSE_TAB4";
            #endregion

            #region Donwhang
            ShopTabData[5].StoreName = "STORE_WC_ACCESSORY";
            ShopTabData[5].Tab = new ShopTabData_.Tab_[3];
            ShopTabData[5].Tab[0].TabName = "STORE_WC_ACCESSORY_TAB1";
            ShopTabData[5].Tab[1].TabName = "STORE_WC_ACCESSORY_TAB2";
            ShopTabData[5].Tab[2].TabName = "STORE_WC_ACCESSORY_TAB3";

            ShopTabData[6].StoreName = "STORE_WC_ARMOR";
            ShopTabData[6].Tab = new ShopTabData_.Tab_[6];
            ShopTabData[6].Tab[0].TabName = "STORE_WC_ARMOR_TAB1";
            ShopTabData[6].Tab[1].TabName = "STORE_WC_ARMOR_TAB2";
            ShopTabData[6].Tab[2].TabName = "STORE_WC_ARMOR_TAB3";
            ShopTabData[6].Tab[3].TabName = "STORE_WC_ARMOR_TAB4";
            ShopTabData[6].Tab[4].TabName = "STORE_WC_ARMOR_TAB5";
            ShopTabData[6].Tab[5].TabName = "STORE_WC_ARMOR_TAB6";

            ShopTabData[7].StoreName = "STORE_WC_POTION";
            ShopTabData[7].Tab = new ShopTabData_.Tab_[1];
            ShopTabData[7].Tab[0].TabName = "STORE_WC_POTION_TAB1";

            ShopTabData[8].StoreName = "STORE_WC_SMITH";
            ShopTabData[8].Tab = new ShopTabData_.Tab_[3];
            ShopTabData[8].Tab[0].TabName = "STORE_WC_SMITH_TAB1";
            ShopTabData[8].Tab[1].TabName = "STORE_WC_SMITH_TAB2";
            ShopTabData[8].Tab[2].TabName = "STORE_WC_SMITH_TAB3";

            ShopTabData[9].StoreName = "STORE_WC_HORSE";
            ShopTabData[9].Tab = new ShopTabData_.Tab_[4];
            ShopTabData[9].Tab[0].TabName = "STORE_WC_HORSE_TAB1";
            ShopTabData[9].Tab[1].TabName = "STORE_WC_HORSE_TAB2";
            ShopTabData[9].Tab[2].TabName = "STORE_WC_HORSE_TAB3";
            ShopTabData[9].Tab[3].TabName = "STORE_WC_HORSE_TAB4";
            #endregion

            #region Constantinopole
            ShopTabData[10].StoreName = "STORE_EU_ACCESSORY";
            ShopTabData[10].Tab = new ShopTabData_.Tab_[3];
            ShopTabData[10].Tab[0].TabName = "STORE_EU_ACCESSORY_TAB1";
            ShopTabData[10].Tab[1].TabName = "STORE_EU_ACCESSORY_TAB2";
            ShopTabData[10].Tab[2].TabName = "STORE_EU_ACCESSORY_TAB3";

            ShopTabData[11].StoreName = "STORE_EU_ARMOR";
            ShopTabData[11].Tab = new ShopTabData_.Tab_[6];
            ShopTabData[11].Tab[0].TabName = "STORE_EU_ARMOR_TAB1";
            ShopTabData[11].Tab[1].TabName = "STORE_EU_ARMOR_TAB2";
            ShopTabData[11].Tab[2].TabName = "STORE_EU_ARMOR_TAB3";
            ShopTabData[11].Tab[3].TabName = "STORE_EU_ARMOR_TAB4";
            ShopTabData[11].Tab[4].TabName = "STORE_EU_ARMOR_TAB5";
            ShopTabData[11].Tab[5].TabName = "STORE_EU_ARMOR_TAB6";

            ShopTabData[12].StoreName = "STORE_EU_POTION";
            ShopTabData[12].Tab = new ShopTabData_.Tab_[1];
            ShopTabData[12].Tab[0].TabName = "STORE_EU_POTION_TAB1";

            ShopTabData[13].StoreName = "STORE_EU_SMITH";
            ShopTabData[13].Tab = new ShopTabData_.Tab_[3];
            ShopTabData[13].Tab[0].TabName = "STORE_EU_SMITH_TAB1";
            ShopTabData[13].Tab[1].TabName = "STORE_EU_SMITH_TAB2";
            ShopTabData[13].Tab[2].TabName = "STORE_EU_SMITH_TAB3";

            ShopTabData[14].StoreName = "STORE_EU_HORSE";
            ShopTabData[14].Tab = new ShopTabData_.Tab_[4];
            ShopTabData[14].Tab[0].TabName = "STORE_EU_HORSE_TAB1";
            ShopTabData[14].Tab[1].TabName = "STORE_EU_HORSE_TAB2";
            ShopTabData[14].Tab[2].TabName = "STORE_EU_HORSE_TAB3";
            ShopTabData[14].Tab[3].TabName = "STORE_EU_HORSE_TAB4";
            #endregion

            #region Samarkand
            ShopTabData[15].StoreName = "STORE_CA_ACCESSORY";
            ShopTabData[15].Tab = new ShopTabData_.Tab_[3];
            ShopTabData[15].Tab[0].TabName = "STORE_CA_ACCESSORY_TAB1";
            ShopTabData[15].Tab[1].TabName = "STORE_CA_ACCESSORY_TAB2";
            ShopTabData[15].Tab[2].TabName = "STORE_CA_ACCESSORY_TAB3";

            ShopTabData[16].StoreName = "STORE_CA_ARMOR";
            ShopTabData[16].Tab = new ShopTabData_.Tab_[6];
            ShopTabData[16].Tab[0].TabName = "STORE_CA_ARMOR_TAB1";
            ShopTabData[16].Tab[1].TabName = "STORE_CA_ARMOR_TAB2";
            ShopTabData[16].Tab[2].TabName = "STORE_CA_ARMOR_TAB3";
            ShopTabData[16].Tab[3].TabName = "STORE_CA_ARMOR_TAB4";
            ShopTabData[16].Tab[4].TabName = "STORE_CA_ARMOR_TAB5";
            ShopTabData[16].Tab[5].TabName = "STORE_CA_ARMOR_TAB6";

            ShopTabData[17].StoreName = "STORE_CA_POTION";
            ShopTabData[17].Tab = new ShopTabData_.Tab_[1];
            ShopTabData[17].Tab[0].TabName = "STORE_CA_POTION_TAB1";

            ShopTabData[18].StoreName = "STORE_CA_SMITH";
            ShopTabData[18].Tab = new ShopTabData_.Tab_[3];
            ShopTabData[18].Tab[0].TabName = "STORE_CA_SMITH_TAB1";
            ShopTabData[18].Tab[1].TabName = "STORE_CA_SMITH_TAB2";
            ShopTabData[18].Tab[2].TabName = "STORE_CA_SMITH_TAB3";

            ShopTabData[19].StoreName = "STORE_CA_HORSE";
            ShopTabData[19].Tab = new ShopTabData_.Tab_[4];
            ShopTabData[19].Tab[0].TabName = "STORE_CA_HORSE_TAB1";
            ShopTabData[19].Tab[1].TabName = "STORE_CA_HORSE_TAB2";
            ShopTabData[19].Tab[2].TabName = "STORE_CA_HORSE_TAB3";
            ShopTabData[19].Tab[3].TabName = "STORE_CA_HORSE_TAB4";
            #endregion

            #region Hotan
            ShopTabData[20].StoreName = "STORE_KT_ACCESSORY";
            ShopTabData[20].Tab = new ShopTabData_.Tab_[6];
            ShopTabData[20].Tab[0].TabName = "STORE_KT_ACCESSORY_EU_TAB1";
            ShopTabData[20].Tab[1].TabName = "STORE_KT_ACCESSORY_EU_TAB2";
            ShopTabData[20].Tab[2].TabName = "STORE_KT_ACCESSORY_EU_TAB3";
            ShopTabData[20].Tab[3].TabName = "STORE_KT_ACCESSORY_TAB1";
            ShopTabData[20].Tab[4].TabName = "STORE_KT_ACCESSORY_TAB2";
            ShopTabData[20].Tab[5].TabName = "STORE_KT_ACCESSORY_TAB3";

            ShopTabData[21].StoreName = "STORE_KT_ARMOR";
            ShopTabData[21].Tab = new ShopTabData_.Tab_[12];
            ShopTabData[21].Tab[0].TabName = "STORE_KT_ARMOR_EU_TAB1";
            ShopTabData[21].Tab[1].TabName = "STORE_KT_ARMOR_EU_TAB2";
            ShopTabData[21].Tab[2].TabName = "STORE_KT_ARMOR_EU_TAB3";
            ShopTabData[21].Tab[3].TabName = "STORE_KT_ARMOR_EU_TAB4";
            ShopTabData[21].Tab[4].TabName = "STORE_KT_ARMOR_EU_TAB5";
            ShopTabData[21].Tab[5].TabName = "STORE_KT_ARMOR_EU_TAB6";
            ShopTabData[21].Tab[6].TabName = "STORE_KT_ARMOR_TAB1";
            ShopTabData[21].Tab[7].TabName = "STORE_KT_ARMOR_TAB2";
            ShopTabData[21].Tab[8].TabName = "STORE_KT_ARMOR_TAB3";
            ShopTabData[21].Tab[9].TabName = "STORE_KT_ARMOR_TAB4";
            ShopTabData[21].Tab[10].TabName = "STORE_KT_ARMOR_TAB5";
            ShopTabData[21].Tab[11].TabName = "STORE_KT_ARMOR_TAB6";

            ShopTabData[22].StoreName = "STORE_KT_POTION";
            ShopTabData[22].Tab = new ShopTabData_.Tab_[1];
            ShopTabData[22].Tab[0].TabName = "STORE_KT_POTION_TAB1";

            ShopTabData[23].StoreName = "STORE_KT_SMITH";
            ShopTabData[23].Tab = new ShopTabData_.Tab_[6];
            ShopTabData[23].Tab[0].TabName = "STORE_KT_SMITH_EU_TAB1";
            ShopTabData[23].Tab[1].TabName = "STORE_KT_SMITH_EU_TAB2";
            ShopTabData[23].Tab[2].TabName = "STORE_KT_SMITH_EU_TAB3";
            ShopTabData[23].Tab[3].TabName = "STORE_KT_SMITH_TAB1";
            ShopTabData[23].Tab[4].TabName = "STORE_KT_SMITH_TAB2";
            ShopTabData[23].Tab[5].TabName = "STORE_KT_SMITH_TAB3";

            ShopTabData[24].StoreName = "STORE_KT_HORSE";
            ShopTabData[24].Tab = new ShopTabData_.Tab_[4];
            ShopTabData[24].Tab[0].TabName = "STORE_KT_HORSE_TAB1";
            ShopTabData[24].Tab[1].TabName = "STORE_KT_HORSE_TAB2";
            ShopTabData[24].Tab[2].TabName = "STORE_KT_HORSE_TAB3";
            ShopTabData[24].Tab[3].TabName = "STORE_KT_HORSE_TAB4";
            #endregion

            #region Initialize Slots
            for (int i = 0; i < ShopTabData.Length; i++)
            {
                for (int a = 0; a < ShopTabData[i].Tab.Length; a++)
                {
                    ShopTabData[i].Tab[a].ItemType = new string[100];
                }
            }
            #endregion

        }
    }

    class Monster
    {
        public uint UniqueID;
        public string Type;
        public byte MobType;
        public string MobTypename;
        public string Name;
        public uint Level;
        public float X;
        public float Y;
        public int Distance;
        public int Priority;
        public byte Status;
        public string AdvanceName;

        public Monster(uint UniqueID, string Type, byte MobType, string MobTypename, string Name, uint Level, int Priority, byte Status, string AdvanceName)
        {
            this.UniqueID = UniqueID;
            this.Type = Type;
            this.MobType = MobType;
            this.MobTypename = MobTypename;
            this.Name = Name;
            this.Level = Level;
            this.Priority = Priority;
            this.Status = Status;
            this.AdvanceName = AdvanceName;
        }

        public static List<Monster> SpawnMob = new List<Monster>();
        public static List<string> ModNameList = new List<string>();
        public static List<string> ModTypeList = new List<string>();
        public static List<string> AvoidMob = new List<string>();
    }

    class Item
    {
        public uint UniqueID;
        public byte Status;
        public string Type;
        public string Name;
        public Item(uint UniqueID, byte Status, string Type, string Name)
        {
            this.UniqueID = UniqueID;
            this.Status = Status;
            this.Type = Type;
            this.Name = Name;
        }

        public static List<Item> SpawnItem = new List<Item>();  
        public static List<Item> PickableItem = new List<Item>();
    }

    class ItemList
    {
        public byte Slot { get; set; }
        public ushort Quanlity { get; set; }
        public string Name { get; set; }
        public byte Level { get; set; }
        public uint Durability { get; set; }
    }

    class AdvanceItem
    {
        public string AdvanceType;
        public string AdvanceName;
        public byte AdvanceLevel;
        public string Action;

        public AdvanceItem(string Type, string Name, byte Level, string Action)
        {
            this.AdvanceType = Type;
            this.AdvanceName = Name;
            this.AdvanceLevel = Level;
            this.Action = Action;
        }

        public static List<AdvanceItem> Allitems = new List<AdvanceItem>();
        public static List<AdvanceItem> Gold = new List<AdvanceItem>();
        public static List<AdvanceItem> Weapon = new List<AdvanceItem>();
        public static List<AdvanceItem> Armor = new List<AdvanceItem>();
        public static List<AdvanceItem> Accessorise = new List<AdvanceItem>();
        public static List<AdvanceItem> Elixir = new List<AdvanceItem>();
        public static List<AdvanceItem> Tablet = new List<AdvanceItem>();
        public static List<AdvanceItem> Material = new List<AdvanceItem>();
        public static List<AdvanceItem> Potion = new List<AdvanceItem>();
        public static List<AdvanceItem> Return = new List<AdvanceItem>();
        public static List<AdvanceItem> Quest = new List<AdvanceItem>();
        public static List<AdvanceItem> Arrow = new List<AdvanceItem>();
        public static List<AdvanceItem> Mall = new List<AdvanceItem>();
    }

    class AdvanceList
    {
        public string Name { get; set; }
        public byte Level { get; set; }
        public string Action { get; set; }
    }

    class OtherCharacter
    {
        public uint UniqueID;
        public string CharName;
        public string GuildName;
        public Buffs_[] Buffs;
        public Items_[] Items;
        public byte Alive;
        public struct Buffs_
        {
            public string Name;
            public uint TempID;
        }
        public struct Items_
        {
            public uint ID;
            public string Name;
        }
        
        public OtherCharacter(uint UniqueID, string CharName, string GuildName, Buffs_[] Buffs, Items_[] Items, byte Alive)
        {
            this.UniqueID = UniqueID;
            this.CharName = CharName;
            this.GuildName = GuildName;
            this.Buffs = Buffs;
            this.Items = Items;
            this.Alive = Alive;
        }

        public static List<OtherCharacter> Characters = new List<OtherCharacter>();
    }

    class Skill
    {
        public uint UniqueID;
        public string Name;
        public string Type;
        public int Level;
        public byte SType;
        public byte Status;
        public int Casttime;
        public int Cooldown;
        public int MinMP;
        public int Bufftime;
        public byte BuffWaiting;

        public uint bufftemp;
        public byte bufftype;
        public int index;

        public Timer CooldownTime;

        public Skill(uint UniqueID, string Name, string Type, int Level, byte SType, byte Status, int Casttime, int Cooldown, int MinMP, int Bufftime, byte BuffWaiting, Timer CooldownTime)
        {
            this.UniqueID = UniqueID;
            this.Name = Name;
            this.Type = Type;
            this.Level = Level;
            this.SType = SType;
            this.Status = Status;
            this.Casttime = Casttime;
            this.Cooldown = Cooldown;
            this.MinMP = MinMP;
            this.Bufftime = Bufftime;
            this.BuffWaiting = BuffWaiting;
            this.CooldownTime = CooldownTime;
            this.CooldownTime = new Timer();
        }

        public static List<Skill> Skills = new List<Skill>();
        public static List<Skill> AttackSkills = new List<Skill>();
        public static List<Skill> PartyAttackSkills = new List<Skill>();
        public static List<Skill> Imbue = new List<Skill>();
        public static List<Skill> PartyImbue = new List<Skill>();
    }

    class SkillList
    {
        public int Level { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
        public byte Bufftype { get; set; }
        public byte Buffwait { get; set; }
    }

    class Spawns
    {
        //NPC
        public static List<uint> NPCID = new List<uint>();
        public static List<string> NPCType = new List<string>();
        //NPC
        public struct Pets_
        {
            public uint id;
            public string name;
            public string type;
            public int x;
            public int y;
            public float speed;
        }
        public static Pets_[] pets = new Pets_[1000];
    }

    class Pets
    {
        public struct Inventory_
        {
            public string name;
            public uint id;
            public string type;
            public byte slot;
            public ushort count;
            public uint durability;
            public uint maxdurability;
            public byte level;
        }

        public uint UniqueID;
        public string Name;
        public ushort HGP;
        public float Speed;
        public Inventory_[] Inventory;

        public Pets(uint UniqueID)
        {
            this.UniqueID = UniqueID;
        }

        public static List<Pets> CharPets = new List<Pets>();

        public static uint HorseMaxHP;
        public static uint HorseCurrentHP;
        public static uint MaxHP;
        public static uint CurrentHP;
        public static uint CurrentHGP;
    }
}
