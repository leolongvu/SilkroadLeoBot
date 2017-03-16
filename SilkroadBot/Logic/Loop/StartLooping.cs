using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace LeoBot
{
    class StartLooping
    {
        public static string type = null;
        public static void Start()
        {
            if (Data.bot)
            {
                type = Location.FindTown();
                switch (type)
                {
                    case null:
                        Data.loopend = 0;
                        Data.loop = false;
                        Data.bot = false;
                        Globals.MainWindow.UpdateLogs("Train coordinates are not correct! Please set coordinates before training.");
                        Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                        break;
                    case "train":                      
                        if (Data.char_horseid == 0)
                        {
                            Data.loopend = 0;
                            Data.loop = false;
                            Data.bot = true;
                            PickupControl.there_is_pickable = true;
                            Buffas.buff_waiting = true;

                            Globals.MainWindow.UpdateLogs("Start Botting!");
                            Globals.MainWindow.Content(Globals.MainWindow.startbot, "Stop Bot");

                            if (Globals.MainWindow.Checked(Globals.MainWindow.autoparty) == true)
                            {
                                Party.CreateParty();
                            }

                            LogicControl.Manager();
                        }
                        else
                        {
                            Data.loopaction = "dismounthorse";
                            Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_KILLHORSE);
                            NewPacket.WriteUInt32(Data.char_horseid);
                            Proxy.ag_remote_security.Send(NewPacket);
                        }
                        break;
                    case "ch":
                        if (Globals.MainWindow.Checked(Globals.MainWindow.loop_off) == true)
                        {
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
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
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
                            LoopControl.read = new StreamReader(@"Data/Scripts/ch_town.txt");
                            LoopControl.count = File.ReadAllLines(@"Data/Scripts/ch_town.txt").Length;
                            Data.loopend = 0;
                            Data.loop = false;
                            LoopControl.WalkScript();
                        }
                        break;   
                    case "wc":
                        if (Globals.MainWindow.Checked(Globals.MainWindow.loop_off) == true)
                        {
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
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
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
                            LoopControl.read = new StreamReader(@"Data/Scripts/wc_town.txt");
                            LoopControl.count = File.ReadAllLines(@"Data/Scripts/wc_town.txt").Length;
                            Data.loopend = 0;
                            Data.loop = false;
                            LoopControl.WalkScript();
                        }
                        break;
                    case "kt":
                        if (Globals.MainWindow.Checked(Globals.MainWindow.loop_off) == true)
                        {
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
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
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
                            LoopControl.read = new StreamReader(@"Data/Scripts/kt_town.txt");
                            LoopControl.count = File.ReadAllLines(@"Data/Scripts/kt_town.txt").Length;
                            Data.loopend = 0;
                            Data.loop = false;
                            LoopControl.WalkScript();
                        }
                        break;
                    case "ca":
                        if (Globals.MainWindow.Checked(Globals.MainWindow.loop_off) == true)
                        {
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
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
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
                            LoopControl.read = new StreamReader(@"Data/Scripts/ca_town.txt");
                            LoopControl.count = File.ReadAllLines(@"Data/Scripts/ca_town.txt").Length;
                            Data.loopend = 0;
                            Data.loop = false;
                            LoopControl.WalkScript();
                        }
                        break;
                    case "eu":
                        if (Globals.MainWindow.Checked(Globals.MainWindow.loop_off) == true)
                        {
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
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
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
                            LoopControl.read = new StreamReader(@"Data/Scripts/eu_town.txt");
                            LoopControl.count = File.ReadAllLines(@"Data/Scripts/eu_town.txt").Length;
                            Data.loopend = 0;
                            Data.loop = false;
                            LoopControl.WalkScript();
                        }
                        break;
                }
            }
        }

        public static void LoadTrainScript()
        {
            if (Data.bot)
            {
                if (Data.walkscriptpath != null | Data.walkscriptpath != "")
                {
                    if (File.Exists(Data.walkscriptpath))
                    {
                        try
                        {
                            LoopControl.read.Close();
                        }
                        catch { }
                        LoopControl.read = new StreamReader(Data.walkscriptpath);
                        LoopControl.count = File.ReadAllLines(Data.walkscriptpath).Length;
                        LoopControl.WalkScript();
                    }
                    else
                    {
                        Data.bot = false;
                        Data.loop = false;
                        Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                        Globals.MainWindow.UpdateLogs("Cannot Find WalkScript!");
                    }
                }
                else
                {
                    Data.bot = false;
                    Data.loop = false;
                    Globals.MainWindow.Content(Globals.MainWindow.startbot, "Start Bot");
                    Globals.MainWindow.UpdateLogs("Cannot Find WalkScript!");
                }
            }
        }

        public static void CheckStart()
        {
            if ((Globals.MainWindow.ReadText(Globals.MainWindow.x_setbox) != "") || (Globals.MainWindow.ReadText(Globals.MainWindow.x_setbox) != null))
            {
                Data.bot = true;
                StartLooping.Start();
            }
            else
            {
                Globals.MainWindow.UpdateLogs("Please Set Training Coordinates in the Fight Tab!");
            }
        }
    }
}
