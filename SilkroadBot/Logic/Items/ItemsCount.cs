using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeoBot
{
    class ItemsCount
    {
        public static void CountManager()
        {
            HPMPUNIVIGOR_Pots();
            Arrows_Bolts();
            HorseETC();
            Return();
            Speed();
            Petheal();
            InventorySlots();
        }

        public static void Petheal()
        {
            uint grass = 0;
            uint uni = 0;
            for (int i = 0; i < Data.inventoryid.Count; i++)
            {
                string name = Data.inventorytype[i];
                if (name.StartsWith("ITEM_COS_P_REVIVAL"))
                {
                    grass = grass + Convert.ToUInt32(Data.inventorycount[i]);
                }
                if (name.StartsWith("ITEM_COS_P_CURE_ALL"))
                {
                    uni = uni + Convert.ToUInt32(Data.inventorycount[i]);
                }
            }
            Data.itemscount.glass = grass;
            Data.itemscount.petuni = uni;
        }

        public static void HPMPUNIVIGOR_Pots()
        {
            uint hp = 0;
            uint mp = 0;
            uint uni = 0;
            uint vigor = 0;
            for (int i = 0; i < Data.inventoryid.Count; i++)
            {
                string name = Data.inventorytype[i];
                if (name.StartsWith("ITEM_ETC_HP_POTION"))
                {
                    hp = hp + Convert.ToUInt32(Data.inventorycount[i]);
                }
                if (name.StartsWith("ITEM_ETC_MP_POTION"))
                {
                    mp = mp + Convert.ToUInt32(Data.inventorycount[i]);
                }
                if (name.StartsWith("ITEM_ETC_CURE_ALL"))
                {
                    uni = uni + Convert.ToUInt32(Data.inventorycount[i]);
                }
                if (name.StartsWith("ITEM_ETC_ALL_POTION"))
                {
                    vigor = vigor + Convert.ToUInt32(Data.inventorycount[i]);
                }
            }
            Data.itemscount.hp_pots = hp;
            Data.itemscount.mp_pots = mp;
            Data.itemscount.uni_pills = uni;
            Data.itemscount.vigor = vigor;
            if (Globals.MainWindow.Checked(Globals.MainWindow.lowhp) == true && !Data.loop)
            {
                if (Convert.ToInt32(Globals.MainWindow.ReadText(Globals.MainWindow.lowhptext)) >= hp)
                {
                    Globals.MainWindow.UpdateLogs("Returning To Town: Low HP Potions");
                    BotAction.UseReturn();
                }                
            }
            if (Globals.MainWindow.Checked(Globals.MainWindow.lowmp) == true && !Data.loop)
            {
                if (Convert.ToInt32(Globals.MainWindow.ReadText(Globals.MainWindow.lowmptext)) >= mp)
                {
                    Globals.MainWindow.UpdateLogs("Returning To Town: Low MP Potions");
                    BotAction.UseReturn();
                }               
            }
        }

        public static void Arrows_Bolts()
        {
            int arrows = 0;
            int bolts = 0;
            for (int i = 0; i < Data.inventoryid.Count; i++)
            {
                string name = Data.inventorytype[i];
                if (name == "ITEM_ETC_AMMO_ARROW_01" || name == "ITEM_ETC_AMMO_ARROW_01_DEF" || name == "ITEM_MALL_QUIVER")
                {
                    arrows = arrows + Convert.ToInt32(Data.inventorycount[i]);
                }
                if (name == "ITEM_ETC_AMMO_BOLT_01" || name == "ITEM_ETC_AMMO_BOLT_01_DEF" || name == "ITEM_MALL_BOLT")
                {
                    bolts = bolts + Convert.ToInt32(Data.inventorycount[i]);
                }
            }
            Data.itemscount.arrows = (uint)arrows;
            Data.itemscount.bolts = (uint)bolts;
            if (Globals.MainWindow.Checked(Globals.MainWindow.lowarrow) == true && !Data.loop)
            {
                if (Convert.ToInt32(Globals.MainWindow.ReadText(Globals.MainWindow.lowarrowtext)) >= arrows)
                {
                    Globals.MainWindow.UpdateLogs("Returning To Town: Low Arrows");
                    BotAction.UseReturn();
                }           
            }
            if (Globals.MainWindow.Checked(Globals.MainWindow.lowbolt) == true && !Data.loop)
            {
                if (Convert.ToInt32(Globals.MainWindow.ReadText(Globals.MainWindow.lowbolttext)) >= bolts)
                {
                    Globals.MainWindow.UpdateLogs("Returning To Town: Low Bolts");
                    BotAction.UseReturn();
                }
            }
        }

        public static void HorseETC()
        {
            int pet_hp = 0;
            int horse = 0;
            int hgp = 0;
            for (int i = 0; i < Data.inventoryid.Count; i++)
            {
                string name = Data.inventorytype[i];
                if (name.StartsWith("ITEM_ETC_COS_HP_POTION"))
                {
                    pet_hp = pet_hp + Convert.ToInt32(Data.inventorycount[i]);
                }
                if (name.StartsWith("ITEM_COS_C_HORSE") || name == "ITEM_COS_C_DHORSE1")
                {
                    horse = horse + Convert.ToInt32(Data.inventorycount[i]);
                }
                if (name == "ITEM_COS_P_HGP_POTION_01")
                {
                    hgp = hgp + Convert.ToInt32(Data.inventorycount[i]);
                }
            }
            Data.itemscount.pet_hp = (uint)pet_hp;
            Data.itemscount.horse = (uint)horse;
            Data.itemscount.hgp = (uint)hgp;
        }

        public static void Speed()
        {
            int speed = 0;
            for (int i = 0; i < Data.inventoryid.Count; i++)
            {
                string name = Data.inventorytype[i];
                if (name.StartsWith("ITEM_ETC_ARCHEMY_POTION_SPEED"))
                {
                    speed = speed + Convert.ToInt32(Data.inventorycount[i]);
                }
            }
            Data.itemscount.speed_pots = (uint)speed;
        }

        public static void Return()
        {
            int return_s = 0;
            for (int i = 0; i < Data.inventoryid.Count; i++)
            {
                string name = Data.inventorytype[i];
                if (name == "ITEM_ETC_SCROLL_RETURN_NEWBIE_01" || name == "ITEM_ETC_SCROLL_RETURN_03" || name == "ITEM_ETC_SCROLL_RETURN_02" || name == "ITEM_ETC_SCROLL_RETURN_01")
                {
                    return_s = return_s + Convert.ToInt32(Data.inventorycount[i]);
                }
            }
            Data.itemscount.return_scrool = (uint)return_s;
        }

        public static void InventorySlots()
        {
            byte items_count = 0;
            for (int i = 0; i < Data.inventoryslot.Count; i++)
            {
                if (Data.inventoryslot[i] >= 13)
                {
                    items_count++;
                }
            }
            Data.itemscount.items_count = items_count;
            if (items_count == Character.InventorySize - 13 && Globals.MainWindow.Checked(Globals.MainWindow.inventoryfull) == true && !Data.loop)
            {
                Globals.MainWindow.UpdateLogs("Returning To Town: Inventory Full");
                BotAction.UseReturn();
            }
        }
    }
}