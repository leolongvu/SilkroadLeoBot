using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace LeoBot
{
    class LoopControl
    {
        public static TextReader read;
        public static byte repeat_walk = 0;
        public struct Coordinates_
        {
            public int x;
            public int y;
        }
        public static Coordinates_ Coordinates = new Coordinates_();
        public static int i = 0;
        public static int count = 0;

        public static void WalkScript()
        {
            {
                if (Data.bot)
                {
                    if (i < count)
                    {
                        Data.loop = true;
                        i++;
                        try
                        {
                            string action = read.ReadLine();
                            if (action.StartsWith("go"))
                            {
                                Data.loopaction = "go";
                                Coordinates.x = Convert.ToInt32(action.Split('(')[1].Split(')')[0].Split(',')[0]);
                                Coordinates.y = Convert.ToInt32(action.Split('(')[1].Split(')')[0].Split(',')[1]);
                                int dist = (int)(Math.Abs(Convert.ToInt32(action.Split('(')[1].Split(')')[0].Split(',')[0]) - Character.X) + Math.Abs(Convert.ToInt32(action.Split('(')[1].Split(')')[0].Split(',')[1]) - Character.Y));
                                BotAction.WalkTo(Convert.ToInt32(action.Split('(')[1].Split(')')[0].Split(',')[0]), Convert.ToInt32(action.Split('(')[1].Split(')')[0].Split(',')[1]));
                                double time = 0;
                                if (Data.char_horseid == 0)
                                {
                                    time = (dist / Convert.ToInt64(Character.RunSpeed * 0.08));
                                }
                                else
                                {
                                    time = (dist / Convert.ToInt64(Data.char_horsespeed * 0.08));
                                }
                                Timer repeat = new Timer();
                                repeat.Elapsed += new ElapsedEventHandler(repeat_Elapsed);
                                repeat.Interval = time * 1000 + 1;
                                repeat.Start();
                                repeat.AutoReset = false;
                                repeat.Enabled = true;
                            }
                            if (action.StartsWith("talk"))
                            {
                                if (action.Split('(')[1].Split(')')[0] == "Storage")
                                {
                                    Data.loopaction = "storage";
                                    StorageControl.OpenStorage();
                                }
                                if (action.Split('(')[1].Split(')')[0] == "Sell")
                                {
                                    Data.loopaction = "blacksmith";
                                    SellControl.OpenShop();
                                }
                                if (action.Split('(')[1].Split(')')[0] == "Stable")
                                {
                                    Data.loopaction = "stable";
                                    BuyControl.OpenShop();
                                }
                                if (action.Split('(')[1].Split(')')[0] == "Grocery")
                                {
                                    Data.loopaction = "accessory";
                                    BuyControl.OpenShop();
                                }
                                if (action.Split('(')[1].Split(')')[0] == "Potion")
                                {
                                    Data.loopaction = "potion";
                                    BuyControl.OpenShop();
                                }
                            }
                            if (action.StartsWith("delay"))
                            {
                                Timer timer = new Timer();
                                timer.Elapsed += new ElapsedEventHandler(OnTick);
                                timer.Interval = Convert.ToInt32(action.Split('(')[1].Split(')')[0]) + 1;
                                timer.Start();
                                timer.AutoReset = false;
                                timer.Enabled = true;
                            }
                            if (action.StartsWith("teleport"))
                            {
                                string[] tmp = action.Split(',');
                                uint id = Spawns.NPCID[Spawns.NPCType.IndexOf(Mobs_Info.mobstypelist[Mobs_Info.mobsidlist.IndexOf(Convert.ToUInt32(tmp[1]))])];
                                Teleport.Tele(id, Convert.ToByte(tmp[2]), Convert.ToUInt32(tmp[3]));
                            }
                            if (action.StartsWith("set"))
                            {
                                Globals.MainWindow.SetText(Globals.MainWindow.x_setbox, action.Split('(')[1].Split(')')[0].Split(',')[0]);
                                Globals.MainWindow.SetText(Globals.MainWindow.y_setbox, action.Split('(')[1].Split(')')[0].Split(',')[1]);
                            }
                        }
                        catch { }
                        if (i == count)
                        {
                            Data.loopaction = null;
                            Data.loop = false;
                            i = 0;
                            read.Close();
                            if (Data.loopend == 0)
                            {                               
                                InventoryControl.MergeItems();
                                System.Threading.Thread.Sleep(5000);

                                Globals.MainWindow.UpdateLogs("Townloop Ended");                               
                                if (Data.char_horseid == 0)
                                {
                                    Data.loopaction = "mounthorse";
                                    BotAction.MountHorse();
                                }
                                else
                                {
                                    Data.loopend = 1;
                                    StartLooping.LoadTrainScript();
                                }
                            }
                            else
                            {
                                Globals.MainWindow.UpdateLogs("Walkscript Ended");
                                Globals.MainWindow.SetText(Globals.MainWindow.x_setbox, Character.X.ToString());
                                Globals.MainWindow.SetText(Globals.MainWindow.y_setbox, Character.Y.ToString());
                                StartLooping.Start();
                            }
                        }
                    }
                }                   
            }
        }

        static void repeat_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (i < count)
            {
                LoopControl.WalkScript();                
            }                                 
        }

        public static void OnTick(object sender, ElapsedEventArgs e)
        {
            if (Data.bot == true)
            {
                LoopControl.WalkScript();
            }
        }
    }
}