using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;

namespace LeoBot
{
    class LogicControl
    {
        public static void Manager()
        {
            {
                if (Data.bot && (Movement.enablelogic && (Data.dead == false)))
                {
                    if (Globals.MainWindow.Checked(Globals.MainWindow.nopick) == false)
                    {
                        if (Globals.MainWindow.Checked(Globals.MainWindow.normalpick) == true)
                        {
                            if (Movement.stuck == true)
                            {
                                Movement.StuckMove();
                            }
                            else
                            {
                                if (Globals.MainWindow.Checked(Globals.MainWindow.petpick) == true && Data.char_grabpetid != 0)
                                {
                                    PickupControl.NormalFilter();
                                }
                                if (Globals.MainWindow.Checked(Globals.MainWindow.petpick) == false && PickupControl.there_is_pickable == true)
                                {
                                    PickupControl.NormalFilter();
                                }
                                else
                                {
                                    if (Buffas.buff_waiting == true)
                                    {
                                        Buffas.BuffChecker();
                                    }
                                    else
                                    {
                                        if (Training.monster_selected == false)
                                        {
                                            Training.SelectMonster();
                                        }
                                        else
                                        {
                                            Skills.CheckSkills();
                                        }   
                                    }                                                                                                                                                                      
                                }
                            }                            
                        }
                        else if (Globals.MainWindow.Checked(Globals.MainWindow.advancepick) == true)
                        {
                            if (Movement.stuck == true)
                            {
                                Movement.StuckMove();
                            }
                            else
                            {
                                if (Globals.MainWindow.Checked(Globals.MainWindow.petpick) == true && Data.char_grabpetid != 0)
                                {
                                    PickupControl.AdvanceFilter();
                                }
                                if (!Globals.MainWindow.Checked(Globals.MainWindow.petpick) == true && PickupControl.there_is_pickable == true)
                                {
                                    PickupControl.AdvanceFilter();
                                }
                                else
                                {
                                    if (Buffas.buff_waiting == true)
                                    {
                                        Buffas.BuffChecker();
                                    }
                                    else
                                    {
                                        if (Training.monster_selected == false)
                                        {
                                            Training.SelectMonster();
                                        }
                                        else
                                        {
                                            Skills.CheckSkills();
                                        }
                                    }                      
                                }
                            }                           
                        }
                    }                    
                    else
                    {
                        if (Movement.stuck == true)
                        {
                            Movement.StuckMove();
                        }
                        else
                        {
                            if (Buffas.buff_waiting == true)
                            {
                                Buffas.BuffChecker();
                            }
                            else
                            {
                                if (Training.monster_selected == false)
                                {
                                    Training.SelectMonster();
                                }
                                else
                                {
                                    Skills.CheckSkills();
                                }
                            }                           
                        }                        
                    }
                }
            }
        }
    }
}