using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LeoBot
{
    class Walking
    {
        public static bool walking_circle = false;
        public static bool walking_center = false;
        public static bool walking_path = false;

        public static int[] angle = new int[24] {0, 15, 30, 45, 60, 75, 90, 105, 120 , 135, 150, 165 , 180, 195, 210, 225, 240, 255 , 270, 285, 300 , 315, 330, 345};

        public static int i = 0;
        public static int j = 0;

        public static double nextx = 0;
        public static double nexty = 0;

        public static bool running = false;

        public static void WalkManager()
        {
            {
                if (Globals.MainWindow.Checked(Globals.MainWindow.walkcenter) == true)
                {
                    int trainx = Convert.ToInt32(Globals.MainWindow.ReadText(Globals.MainWindow.x_setbox));
                    int trainy = Convert.ToInt32(Globals.MainWindow.ReadText(Globals.MainWindow.y_setbox));

                    if (!walking_center)
                    {
                        walking_center = true;
                        BotAction.WalkTo(trainx, trainy);
                        LogicControl.Manager();
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                        LogicControl.Manager();
                    }
                }
                else if (Globals.MainWindow.Checked(Globals.MainWindow.walkaround) == true)
                {
                    if (walking_circle == true)
                    {
                        if (Walking.i == 24)
                        {
                            Walking.i = 0;
                        }  

                        running = true;
                        int runangle = 0;
                        try 
                        {
                             runangle = angle[i];
                        }
                        catch 
                        {
                            i = 0;
                            runangle = angle[i];
                        }

                        nextx = Convert.ToInt32(Globals.MainWindow.ReadText(Globals.MainWindow.x_setbox)) + Convert.ToInt32(Globals.MainWindow.ReadText(Globals.MainWindow.runningrangebox)) * Math.Cos(Math.PI * runangle / 180.0);
                        nexty = Convert.ToInt32(Globals.MainWindow.ReadText(Globals.MainWindow.y_setbox)) + Convert.ToInt32(Globals.MainWindow.ReadText(Globals.MainWindow.runningrangebox)) * Math.Sin(Math.PI * runangle / 180.0);                                
                        
                        int dist = (int)(Math.Abs((nextx - Character.X)) + Math.Abs((nexty - Character.Y)));

                        BotAction.WalkTo((nextx + Character.X) / 2, (nexty + Character.Y) / 2);
                        i++;                                              

                        double time = (dist / Convert.ToInt64(Character.RunSpeed * 0.12f));

                        Timer RandomTimer = new Timer();                       
                        RandomTimer.Elapsed += new ElapsedEventHandler(OnTick);
                        RandomTimer.Interval = time * 500 + 1;
                        RandomTimer.Start();
                        RandomTimer.AutoReset = false;
                        RandomTimer.Enabled = true;
                    }
                }
                else if (Globals.MainWindow.Checked(Globals.MainWindow.walkpath) == true)
                {
                    if (Globals.MainWindow.trainbox.Items.Count > 0)
                    {
                        if (walking_path == true)
                        {                           
                            string line = Globals.MainWindow.trainbox.Items[j].ToString();
                            if (line.StartsWith("go"))
                            {
                                string coodinates = line.Split('(')[1].Split(')')[0];
                                nextx = Convert.ToDouble(coodinates.Split(',')[0]);
                                nexty = Convert.ToDouble(coodinates.Split(',')[1]);
                                int dist = (int)(Math.Abs((nextx - Character.X)) + Math.Abs((nexty - Character.Y)));

                                BotAction.WalkTo((nextx + Character.X) / 2, (nexty + Character.Y) / 2);

                                double time = (dist / Convert.ToInt64(Character.RunSpeed * 0.12f));

                                Timer RandomTimer = new Timer();
                                RandomTimer.Elapsed += new ElapsedEventHandler(OnTick);
                                RandomTimer.Interval = time * 500 + 1;
                                RandomTimer.Start();
                                RandomTimer.AutoReset = false;
                                RandomTimer.Enabled = true;
                            }
                            j++;
                            if (j == Globals.MainWindow.trainbox.Items.Count)
                            {
                                j = 0;
                            }
                        }     
                    }
                    else
                    {
                        Globals.MainWindow.UpdateLogs("Cannot find any coordinates to walk to during training!");
                    }
                }
            }
        }

        public static void OnTick(object sender, ElapsedEventArgs e)
        {
            Training.SelectMonster();

            if (Training.monster_selected == false)
            {
                int dist = (int)(Math.Abs((nextx - Character.X)) + Math.Abs((nexty - Character.Y)));
                BotAction.WalkTo(nextx, nexty);
              
                double time = (dist / Convert.ToInt64(Character.RunSpeed * 0.12f));
                Timer RandomTimer = new Timer();
                RandomTimer.Elapsed += new ElapsedEventHandler(OnTickSecond);
                RandomTimer.Interval = time * 1000 + 1;
                RandomTimer.Start();
                RandomTimer.AutoReset = false;
                RandomTimer.Enabled = true;
            }  
            else
            {
                running = false;
            }
        }

        public static void OnTickSecond(object sender, ElapsedEventArgs e)
        {
            if (running == true)
            {
                running = false;
                LogicControl.Manager();
            }
        }
    }
}
