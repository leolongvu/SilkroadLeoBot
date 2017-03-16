using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LeoBot
{
    class Movement
    {
        public static bool TrainingRecording = false;
        public static double xcome = 0;
        public static double ycome = 0;

        public static void Move(Packet packet)
        {
            uint id = packet.ReadUInt32();
            if (id == Character.UniqueID)
            {
                if (packet.ReadUInt8() == 1)
                {
                    byte xsec = packet.ReadUInt8();
                    byte ysec = packet.ReadUInt8();
                    float xcoord = 0;
                    float zcoord = 0;
                    float ycoord = 0;
                    if (ysec == 0x80)
                    {
                        xcoord = packet.ReadUInt16() - packet.ReadUInt16();
                        zcoord = packet.ReadUInt16() - packet.ReadUInt16();
                        ycoord = packet.ReadUInt16() - packet.ReadUInt16();
                    }
                    else
                    {
                        xcoord = packet.ReadUInt16();
                        zcoord = packet.ReadUInt16();
                        ycoord = packet.ReadUInt16();
                    }
                    int real_xcoord = 0;
                    int real_ycoord = 0;
                    if (xcoord > 32768)
                    {
                        real_xcoord = (int)(65536 - xcoord);
                    }
                    else
                    {
                        real_xcoord = (int)xcoord;
                    }
                    if (ycoord > 32768)
                    {
                        real_ycoord = (int)(65536 - ycoord);
                    }
                    else
                    {
                        real_ycoord = (int)ycoord;
                    }
                    int x = Globals.CalculatePositionX(xsec, real_xcoord);
                    int y = Globals.CalculatePositionY(ysec, real_ycoord);
                    xcome = x;
                    ycome = y;

                    if (Skills.ghostwalking == true)
                    {
                        Skills.CastGhostWalk(Skills.ghostwalkid, xsec, ysec, (ushort)xcoord, (ushort)zcoord, (ushort)ycoord);
                        Skills.ghostwalking = false;
                    }
                    //Loop
                    //Training recording
                    if (Movement.TrainingRecording == true)
                    {
                        string text = "go(" + x + "," + y + ")";
                        Globals.MainWindow.AddText(Globals.MainWindow.trainbox, text);
                    }
                    if (Data.loopaction == "record")
                    {
                        string text = "go(" + x + "," + y + ")";
                        Globals.MainWindow.AddText(Globals.MainWindow.walkscript, text);
                    }

                    Character.X = x;
                    Character.Y = y;
                    string sector = (String.Format("{0:X2}{1:X2}", xsec, ysec));
                    int index = Character.locationsector.IndexOf(sector);
                    if (index != -1)
                    {
                        string location = Character.location[index];
                        PortConfigs.TrainWindow.Label(PortConfigs.TrainWindow.location, location);
                    }
                    
                    Globals.MainWindow.SetText(Globals.MainWindow.xlabel, Convert.ToString(Character.X));
                    Globals.MainWindow.SetText(Globals.MainWindow.ylabel, Convert.ToString(Character.Y));
                    
                    //Recalculate Mob Distance
                    foreach (Monster monster in Monster.SpawnMob)
                    {
                        System.Threading.Thread.Sleep(2);
                        {
                            monster.Distance = (int)(Math.Abs((monster.X - Character.X)) + Math.Abs((monster.Y - Character.Y)));
                        }
                    }
                    //Recalculate Mob Distance
                }
            }
            else if (id == Data.char_horseid)
            {
                if (packet.ReadUInt8() == 1)
                {
                    byte xsec = packet.ReadUInt8();
                    byte ysec = packet.ReadUInt8();
                    float xcoord = 0;
                    float zcoord = 0;
                    float ycoord = 0;
                    if (ysec == 0x80)
                    {
                        xcoord = packet.ReadUInt16() - packet.ReadUInt16();
                        zcoord = packet.ReadUInt16() - packet.ReadUInt16();
                        ycoord = packet.ReadUInt16() - packet.ReadUInt16();
                    }
                    else
                    {
                        xcoord = packet.ReadUInt16();
                        zcoord = packet.ReadUInt16();
                        ycoord = packet.ReadUInt16();
                    }
                    int real_xcoord = 0;
                    int real_ycoord = 0;
                    if (xcoord > 33000)
                    {
                        real_xcoord = (int)(65352 - xcoord);
                    }
                    else
                    {
                        real_xcoord = (int)xcoord;
                    }
                    if (ycoord > 33000)
                    {
                        real_ycoord = (int)(65352 - ycoord);
                    }
                    else
                    {
                        real_ycoord = (int)ycoord;
                    }
                    int x = Globals.CalculatePositionX(xsec, real_xcoord);
                    int y = Globals.CalculatePositionY(ysec, real_ycoord);

                    xcome = x;
                    ycome = y;

                    Character.X = x;
                    Character.Y = y;
                    Globals.MainWindow.SetText(Globals.MainWindow.xlabel, Convert.ToString(Character.X));
                    Globals.MainWindow.SetText(Globals.MainWindow.ylabel, Convert.ToString(Character.Y));

                    //Recalculate Mob Distance
                    foreach (Monster monster in Monster.SpawnMob)
                    {
                        System.Threading.Thread.Sleep(2);
                        {
                            monster.Distance = (int)(Math.Abs((monster.X - Character.X)) + Math.Abs((monster.Y - Character.Y)));
                        }
                    }
                    //Recalculate Mob Distance
                }
            }
            else
            {
                if (packet.ReadUInt8() == 0x01)
                {
                    foreach (Monster monster in Monster.SpawnMob)
                    {
                        if (id == monster.UniqueID)
                        {
                            byte xsec = packet.ReadUInt8();
                            byte ysec = packet.ReadUInt8();
                            float xcoord = 0;
                            float ycoord = 0;
                            if (ysec == 0x80)
                            {
                                xcoord = packet.ReadUInt16() - packet.ReadUInt16();
                                packet.ReadUInt16();
                                packet.ReadUInt16();
                                ycoord = packet.ReadUInt16() - packet.ReadUInt16();
                            }
                            else
                            {
                                xcoord = packet.ReadUInt16();
                                packet.ReadUInt16();
                                ycoord = packet.ReadUInt16();
                            }
                            int x = Globals.CalculatePositionX(xsec, xcoord);
                            int y = Globals.CalculatePositionY(ysec, ycoord);
                            int dist = Math.Abs((x - Character.X)) + Math.Abs((y - Character.Y));
                            monster.X = x;
                            monster.Y = y;
                            monster.Distance = dist;
                            break;
                        }
                    }
                }
            }
        }   
        
        public static byte stuck_count = 0;
        public static bool stuck = false;
        public static bool enablelogic = true;
        public static double xstuck = 0;
        public static double ystuck = 0;
        public static double xescape = 0;
        public static double yescape = 0;

        public static void Stuck(Packet packet)
        {
            if (packet.ReadUInt32() == Character.UniqueID)
            {
                stuck_count++;
                byte xsec = packet.ReadUInt8();
                byte ysec = packet.ReadUInt8();
                float xcoord = packet.ReadSingle();
                packet.ReadSingle();
                float ycoord = packet.ReadSingle();
                Character.X = Globals.CalculatePositionX(xsec, xcoord);
                Character.Y = Globals.CalculatePositionY(ysec, ycoord);

                xstuck = Character.X;
                ystuck = Character.Y;

                if (stuck_count > 2)
                {
                    stuck_count = 0;
                    if (Data.bot)
                    {
                        stuck = true;

                        Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTACTION);
                        NewPacket.WriteUInt8(2);
                        Proxy.ag_remote_security.Send(NewPacket);

                        /*if (Training.monster_selected)
                        {
                            LeoBot.Stuck.AddMob(Training.monster_id, 10);
                            Training.monster_selected = false;
                            Training.monster_id = 0;

                            Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTACTION);
                            NewPacket.WriteUInt8(2);
                            Proxy.ag_remote_security.Send(NewPacket);

                            enablelogic = false;
                            System.Threading.Thread n_t = new System.Threading.Thread(LogicControl.Manager);
                            n_t.Start();
                            enablelogic = true;
                        }*/
                    }
                }
            }
        }

        public static void StuckMove()
        {
            int dist = (int)(Math.Abs(xcome - xstuck) + Math.Abs(ycome - ystuck));

            double xp = (10 * xcome - (dist + 10) * xstuck) / (0 - dist);
            double yp = (10 * ycome - (dist + 10) * ystuck) / (0 - dist);

            BotAction.WalkTo(xp, yp);

            Timer Timer1 = new Timer();
            double time = (10 / Convert.ToInt64(Character.RunSpeed * 0.1f));
            Timer1.Elapsed += new ElapsedEventHandler(OnTick);
            Timer1.Interval = time * 1000 + 1;
            Timer1.Start();
            Timer1.AutoReset = false;
            Timer1.Enabled = true;

            xescape = xp - (yp - ystuck);
            yescape = (xp - xstuck) + yp;
        }

        public static void OnTick(object sender, ElapsedEventArgs e)
        {
            BotAction.WalkTo(xescape, yescape);

            Timer Timer2 = new Timer();
            double time = (10 / Convert.ToInt64(Character.RunSpeed * 0.1f));
            Timer2.Elapsed += new ElapsedEventHandler(OnTick1);
            Timer2.Interval = time * 1000 + 1;
            Timer2.Start();
            Timer2.AutoReset = false;
            Timer2.Enabled = true;
        }

        public static void OnTick1(object sender, ElapsedEventArgs e)
        {
            stuck = false;
            LogicControl.Manager();
        }
    }
}
