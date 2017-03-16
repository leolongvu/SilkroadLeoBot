using System;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Threading;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Data;
using System.Windows.Controls;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using System.Xml.Serialization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Interop;
using System.Timers;

namespace LeoBot
{
    /// <summary>
    /// InterAction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        static extern uint WriteProcessMemory(IntPtr hProcess, uint lpBaseAddress, byte[] lpBuffer, int nSize, uint lpNumberOfBytesWritten);
        [DllImport("kernel32.dll")]
        static extern uint VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateMutex(IntPtr lpMutexAttributes, bool bInitialOwner, string lpName);
        public static IntPtr Handle;
        public static IntPtr SROHandle;      

        public MainWindow()
        {
            InitializeComponent();

            Globals.MainWindow = this;
            Data.InitializeTypes();
            Data.LoadShopTabData();
            Load();
            LoadTXT.LoadData();
            Proxy.gw = new Thread(Proxy.GatewayThread);
            Proxy.gw.Start();
            Proxy.gw.IsBackground = true;
            Configs.LoadInfo();
        }
         
        #region Metro Style

        private void PART_TITLEBAR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void PART_CLOSE_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PortConfigs.TrainWindow.Close();
        }

        private void PART_MINIMIZE_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
        #endregion

        #region Invoke Methods

        private delegate void UpdateLogsLine(string Text);
        public void UpdateLogs(string log)
        {
            string currentTime = "[" + DateTime.Now.ToLongTimeString() + "] ";
            UpdateLogsLine InvokeWriteLine = new UpdateLogsLine(this.logs.AppendText);
            this.logs.Dispatcher.BeginInvoke(InvokeWriteLine, currentTime + log + "\r\n");
        }

        public void SetText(TextBox textbox, string text)
        {
            if (textbox.Dispatcher.CheckAccess())
            {
                textbox.Text = text;
            }
            else
            {
                textbox.Dispatcher.Invoke((Action)delegate()
                {
                    SetText(textbox, text);
                });               
            }
        }

        public void SetCheck(CheckBox check, string text)
        {
            if (check.Dispatcher.CheckAccess())
            {
                check.IsChecked = Convert.ToBoolean(text);
            }
            else
            {
                check.Dispatcher.Invoke((Action)delegate()
                {
                    SetCheck(check, text);
                });
            }
        }

        public void SetCheck(RadioButton check, string text)
        {
            if (check.Dispatcher.CheckAccess())
            {
                check.IsChecked = Convert.ToBoolean(text);
            }
            else
            {
                check.Dispatcher.Invoke((Action)delegate()
                {
                    SetCheck(check, text);
                });
            }
        }

        public void SetText(Label label, string text)
        {
            if (label.Dispatcher.CheckAccess())
            {
                label.Content = text;
            }
            else
            {
                label.Dispatcher.Invoke((Action)delegate()
                {
                    SetText(label, text);
                });
            }
        }

        public void AddText(ListBox list, string text)
        {
            if (list.Dispatcher.CheckAccess())
            {
                list.Items.Add(text);
            }
            else
            {
                list.Dispatcher.Invoke((Action)delegate()
                {
                    AddText(list, text);
                });
            }
        }

        public void Clear(ListBox list)
        {
            if (list.Dispatcher.CheckAccess())
            {
                list.Items.Clear();
            }
            else
            {
                list.Dispatcher.Invoke((Action)delegate()
                {
                    Clear(list);
                });
            }
        }

        public void Index(ComboBox combobox, int index)
        {
            if (combobox.Dispatcher.CheckAccess())
            {
                combobox.SelectedIndex = index;
            }
            else
            {
                combobox.Dispatcher.Invoke((Action)delegate()
                {
                    Index(combobox, index);
                });
            }
        }

        public void Content(Button button, string text)
        {
            if (button.Dispatcher.CheckAccess())
            {
                button.Content = text;
            }
            else
            {
                button.Dispatcher.Invoke((Action)delegate()
                {
                    Content(button, text);
                });
            }
        }

        public void Enable(Button button)
        {
            if (button.Dispatcher.CheckAccess())
            {
                button.IsEnabled = true;
            }
            else
            {
                button.Dispatcher.Invoke((Action)delegate()
                {
                    Enable(button);
                });
            }
        }

        public void Enable(ListBox button)
        {
            if (button.Dispatcher.CheckAccess())
            {
                button.IsEnabled = true;
            }
            else
            {
                button.Dispatcher.Invoke((Action)delegate()
                {
                    Enable(button);
                });
            }
        }

        public bool CheckEnable(Button button)
        {
            bool check = false;
            if (button.Dispatcher.CheckAccess())
            {
                if (button.IsEnabled == true)
                {
                    check = true;
                }
                return check;
            }
            else
            {
                return (bool)button.Dispatcher.Invoke(
                  new Func<bool>(() => CheckEnable(button))
                );
            }
        }

        public void Enable(CheckBox button)
        {
            if (button.Dispatcher.CheckAccess())
            {
                button.IsEnabled = true;
            }
            else
            {
                button.Dispatcher.Invoke((Action)delegate()
                {
                    Enable(button);
                });
            }
        }

        public void Enable(PasswordBox button)
        {
            if (button.Dispatcher.CheckAccess())
            {
                button.IsEnabled = true;
            }
            else
            {
                button.Dispatcher.Invoke((Action)delegate()
                {
                    Enable(button);
                });
            }
        }

        public void Enable(ComboBox button)
        {
            if (button.Dispatcher.CheckAccess())
            {
                button.IsEnabled = true;
            }
            else
            {
                button.Dispatcher.Invoke((Action)delegate()
                {
                    Enable(button);
                });
            }
        }

        public void UnCheck(CheckBox button)
        {
            if (button.Dispatcher.CheckAccess())
            {
                button.IsChecked = false;
            }
            else
            {
                button.Dispatcher.Invoke((Action)delegate()
                {
                    UnCheck(button);
                });
            }
        }

        public void Enable(TextBox textbox)
        {
            if (textbox.Dispatcher.CheckAccess())
            {
                textbox.IsEnabled = true;
            }
            else
            {
                textbox.Dispatcher.Invoke((Action)delegate()
                {
                    Enable(textbox);
                });
            }
        }

        public void UnEnable(TextBox textbox)
        {
            if (textbox.Dispatcher.CheckAccess())
            {
                textbox.IsEnabled = false;
            }
            else
            {
                textbox.Dispatcher.Invoke((Action)delegate()
                {
                    UnEnable(textbox);
                });
            }
        }

        public void UnEnable(Button textbox)
        {
            if (textbox.Dispatcher.CheckAccess())
            {
                textbox.IsEnabled = false;
            }
            else
            {
                textbox.Dispatcher.Invoke((Action)delegate()
                {
                    UnEnable(textbox);
                });
            }
        }

        public void UnEnable(CheckBox textbox)
        {
            if (textbox.Dispatcher.CheckAccess())
            {
                textbox.IsEnabled = false;
            }
            else
            {
                textbox.Dispatcher.Invoke((Action)delegate()
                {
                    UnEnable(textbox);
                });
            }
        }

        public void UnEnable(ListBox textbox)
        {
            if (textbox.Dispatcher.CheckAccess())
            {
                textbox.IsEnabled = false;
            }
            else
            {
                textbox.Dispatcher.Invoke((Action)delegate()
                {
                    UnEnable(textbox);
                });
            }
        }

        public void UnEnable(ComboBox textbox)
        {
            if (textbox.Dispatcher.CheckAccess())
            {
                textbox.IsEnabled = false;
            }
            else
            {
                textbox.Dispatcher.Invoke((Action)delegate()
                {
                    UnEnable(textbox);
                });
            }
        }

        public void UnEnable(PasswordBox textbox)
        {
            if (textbox.Dispatcher.CheckAccess())
            {
                textbox.IsEnabled = false;
            }
            else
            {
                textbox.Dispatcher.Invoke((Action)delegate()
                {
                    UnEnable(textbox);
                });
            }
        }

        public void UpdatePriorityList()
        {
            if (!priority.Dispatcher.CheckAccess())
            {
                priority.Dispatcher.Invoke((Action)delegate()
                {
                    UpdatePriorityList();
                });
            }
            else
            {
                priority.Items.Clear();
                priority.Items.Add(String.Format("Normal: {0}", DataParser.NormalPriority));
                priority.Items.Add(String.Format("Champion: {0}", DataParser.ChampionPriority));
                priority.Items.Add(String.Format("Giant: {0}", DataParser.GiantPriority));
                priority.Items.Add(String.Format("Party: {0}", DataParser.PartyPriority));
                priority.Items.Add(String.Format("Party Champion: {0}", DataParser.PartyChampionPriority));
                priority.Items.Add(String.Format("Party Giant: {0}", DataParser.PartyGiantPriority));
                priority.Items.Add(String.Format("Titan: {0}", DataParser.TitanPriority));
                priority.Items.Add(String.Format("Elite: {0}", DataParser.ElitePriority));
                priority.Items.Add(String.Format("Unique: {0}", DataParser.UniquePriority));
            }
        }

        public void UpdateTray()
        {
            if (this.Dispatcher.CheckAccess())
            {
                this.Title = "LeoBot - " + Character.PlayerName;
            }
            else
            {
                this.Dispatcher.Invoke((Action)delegate()
                {
                    UpdateTray();
                });  
            }
        }

        public void UpdateBar()
        {
            if (!this.HP_Bar.Dispatcher.CheckAccess())
            {
                this.HP_Bar.Dispatcher.Invoke((Action)delegate()
                {
                    UpdateBar();
                });
            }
            else
            {
                this.HP_Bar.Maximum = (int)Character.MaxHP;
                this.HP_Bar.Value = (int)Character.CurrentHP;
                this.HP_Bar.UpdateLayout();
                this.MP_Bar.Maximum = (int)Character.MaxMP;
                this.MP_Bar.Value = (int)Character.CurrentMP;
                this.MP_Bar.UpdateLayout();
                if (Pets.MaxHP != 0)
                {
                    this.PetHP_Bar.Maximum = (int)Pets.MaxHP;
                    this.PetHP_Bar.Value = (int)Pets.CurrentHP;
                    this.PetHP_Bar.UpdateLayout();
                    this.PetHGP_Bar.Maximum = 10000;
                    this.PetHGP_Bar.Value = Pets.CurrentHGP;
                    this.PetHGP_Bar.UpdateLayout();
                }
                if (Pets.HorseMaxHP != 0)
                {
                    this.HorseHP_Bar.Maximum = (int)Pets.HorseMaxHP;
                    this.HorseHP_Bar.Value = (int)Pets.HorseCurrentHP;
                    this.HorseHP_Bar.UpdateLayout();
                }                              
            }
        }

        public void UpdateSkillList()
        {
            for (int i = 0; i < Skill.Skills.Count - 1; i++)
            {
                for (int j = i + 1; j < Skill.Skills.Count; j++)
                {
                    if (Skill.Skills[i].UniqueID == Skill.Skills[j].UniqueID)
                    {
                        Skill.Skills.RemoveAt(j);
                    }
                }
            }
            if (!this.skilllist.Dispatcher.CheckAccess())
            {
                this.skilllist.Dispatcher.Invoke((Action)delegate()
                {
                    UpdateSkillList();
                });
            }
            else
            {
                skilllist.Items.Clear();
                foreach (Skill skill in Skill.Skills)
                {
                    skilllist.Items.Add(new SkillList() { Level = skill.Level, Name = skill.Name, ID = String.Format("{0:X8}", skill.UniqueID) , Bufftype = skill.bufftype, Buffwait = skill.Status});                    
                }       
            }
        }

        public void UpdateInventory()
        {
            if (!this.inventory.Dispatcher.CheckAccess())
            {
                this.inventory.Dispatcher.Invoke((Action)delegate()
                {
                    UpdateInventory();
                });
            }
            else
            {
                inventory.Items.Clear();
                for (int i = 0; i < Data.inventoryslot.Count; i++ )
                {
                    inventory.Items.Add(new ItemList() { Slot = Data.inventoryslot[i], Quanlity = Data.inventorycount[i], Name = Data.inventoryname[i], Level = Data.inventorylevel[i], Durability = Data.inventorydurability[i] });
                }
            }
        }

        public string ReadText(ComboBox combobox)
        {
            if (combobox.Dispatcher.CheckAccess())
            {
                string text = combobox.SelectedValue.ToString().Split(':')[1].Trim();
                return text;
            }
            else
            {
                return (string)combobox.Dispatcher.Invoke(
                  new Func<String>(() => ReadText(combobox))
                );
            }
        }

        public string ReadText(Button combobox)
        {
            if (combobox.Dispatcher.CheckAccess())
            {
                string text = combobox.Content.ToString();
                return text;
            }
            else
            {
                return (string)combobox.Dispatcher.Invoke(
                  new Func<String>(() => ReadText(combobox))
                );
            }
        }

        public string ReadText1(ComboBox combobox)
        {
            if (combobox.Dispatcher.CheckAccess())
            {
                string text = combobox.SelectedValue.ToString();
                return text;
            }
            else
            {
                return (string)combobox.Dispatcher.Invoke(
                  new Func<String>(() => ReadText1(combobox))
                );
            }
        }

        public string ReadText(TextBox textbox)
        {
            if (textbox.Dispatcher.CheckAccess())
            {
                string text = textbox.Text.ToString();
                return text;
            }
            else
            {
                return (string)textbox.Dispatcher.Invoke(
                  new Func<String>(() => ReadText(textbox))
                );
            }
        }

        public string ReadText(Label textbox)
        {
            if (textbox.Dispatcher.CheckAccess())
            {
                string text = textbox.Content.ToString();
                return text;
            }
            else
            {
                return (string)textbox.Dispatcher.Invoke(
                  new Func<String>(() => ReadText(textbox))
                );
            }
        }

        public string ReadText(PasswordBox textbox)
        {
            if (textbox.Dispatcher.CheckAccess())
            {
                string text = textbox.Password.ToString();
                return text;
            }
            else
            {
                return (string)textbox.Dispatcher.Invoke(
                  new Func<String>(() => ReadText(textbox))
                );
            }
        }

        public bool? Checked(RadioButton check)
        {
            if (check.Dispatcher.CheckAccess())
            {
                bool? check1 = check.IsChecked;
                return check1;
            }
            else
            {
                return (bool?)check.Dispatcher.Invoke(
                  new Func<bool?>(() => Checked(check))
                );
            }
        }

        public bool? Checked(CheckBox check)
        {
            if (check.Dispatcher.CheckAccess())
            {
                bool? check1 = check.IsChecked;
                return check1;
            }
            else
            {
                return (bool?)check.Dispatcher.Invoke(
                  new Func<bool?>(() => Checked(check))
                );
            }
        }
        #endregion

        #region Windows Interface

        private void Load()
        {
            walkaround.IsChecked = true;
            nopick.IsChecked = true;
            startbot.IsEnabled = false;

            UpdatePriorityList();

            capans.IsEnabled = false;
            sendcapans.IsEnabled = false;

            berserk.Items.Add("Normal: No");
            berserk.Items.Add("Champion: No");
            berserk.Items.Add("Giant: No");
            berserk.Items.Add("Party: No");
            berserk.Items.Add("Party Champion: No");
            berserk.Items.Add("Party Giant: No");
            berserk.Items.Add("Titan: No");
            berserk.Items.Add("Elite: No");
            berserk.Items.Add("Unique: No");

            spgainedtotal.Text = "0";
            spgainedpermin.Text = "0";
            spgainedperhour.Text = "0";
            mobkilled.Text = "0";
            diedcount.Text = "0";
            returncount.Text = "0";

            partyname.Text = "LeoBot Auto Party!";
            minlevel.Text = "1";
            maxlevel.Text = "90";
            noitemdis.IsChecked = true;
            noexpdis.IsChecked = true;
            allowinvite.IsChecked = true;
        }
    
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        void GridViewColumnHeaderClickedHandler(object sender,
                                            RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked =
                  e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    string header = headerClicked.Column.Header as string;
                    Sort(skilllist, header, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header 
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }


                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        void GridViewColumnHeaderClickedHandler1(object sender,
                                            RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked =
                  e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    string header = headerClicked.Column.Header as string;
                    Sort(advanceitem, header, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header 
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }


                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        void GridViewColumnHeaderClickedHandler2(object sender,
                                            RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked =
                  e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    string header = headerClicked.Column.Header as string;
                    Sort(inventory, header, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header 
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }


                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void Sort(ListView list, string sortBy, ListSortDirection direction)
        {
            list.Items.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            list.Items.SortDescriptions.Add(sd);
            list.Items.Refresh();
        }
        #endregion

        private void AddImbue(object sender, RoutedEventArgs e)
        {
            if ((skilllist.SelectedIndex != -1) && (generalimbue.Items.Count == 0))
            {
                SkillList skill = (SkillList)skilllist.SelectedItems[0];
                generalimbue.Items.Add(skill.Name);
                foreach (Skill addskill in Skill.Skills)
                {
                    if (addskill.Name == skill.Name)
                    {
                        Skill.Imbue.Add(addskill);
                        break;
                    }
                }
            }
        }

        private void AddGhostWalk(object sender, RoutedEventArgs e)
        {
            if ((skilllist.SelectedIndex != -1) && (ghostwalk.Items.Count == 0))
            {
                SkillList skill = (SkillList)skilllist.SelectedItems[0];
                ghostwalk.Items.Add(skill.Name);
            }
        }

        public static bool enable = true;
        private void DeleteGhostWalk(object sender, MouseButtonEventArgs e)
        {
            if ((ghostwalk.SelectedIndex != -1) && (enable == true))
            {
                ghostwalk.Items.Remove(ghostwalk.SelectedItem);
            }
        }

        private void AddPartyImbue(object sender, RoutedEventArgs e)
        {
            if ((skilllist.SelectedIndex != -1) && (partyimbue.Items.Count == 0))
            {
                SkillList skill = (SkillList)skilllist.SelectedItems[0];
                partyimbue.Items.Add(skill.Name);
                foreach (Skill addskill in Skill.Skills)
                {
                    if (addskill.Name == skill.Name)
                    {
                        Skill.PartyImbue.Add(addskill);
                        break;
                    }
                }
            }
        }

        private void DeleteImbue(object sender, MouseButtonEventArgs e)
        {
            if (generalimbue.SelectedIndex != -1)
            {
                generalimbue.Items.Remove(generalimbue.SelectedItem);
                Skill.Imbue.RemoveAt(0);
            }
        }

        private void DeletePartyImbue(object sender, MouseButtonEventArgs e)
        {
            if (partyimbue.SelectedIndex != -1)
            {
                partyimbue.Items.Remove(partyimbue.SelectedItem);
                Skill.PartyImbue.RemoveAt(0);
            }
        }

        private void AddSkill(object sender, RoutedEventArgs e)
        {
            if (skilllist.SelectedIndex != -1)
            {
                SkillList skill = (SkillList)skilllist.SelectedItems[0];
                generalattack.Items.Add(skill.Name);
                foreach (Skill addskill in Skill.Skills)
                {
                    if (addskill.Name == skill.Name)
                    {
                        Skill.AttackSkills.Add(addskill);
                        break;
                    }
                }
            }
        }

        private void AddPartySkill(object sender, RoutedEventArgs e)
        {
            if (skilllist.SelectedIndex != -1)
            {
                SkillList skill = (SkillList)skilllist.SelectedItems[0];
                partyattack.Items.Add(skill.Name);
                foreach (Skill addskill in Skill.Skills)
                {
                    if (addskill.Name == skill.Name)
                    {
                        Skill.PartyAttackSkills.Add(addskill);
                        break;
                    }
                }
            }
        }

        private void Swap(List<Skill> list, int indexA, int indexB)
        {
            var Temp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = Temp;
        }

        private void SkillUp(object sender, RoutedEventArgs e)
        {
            if (generalattack.SelectedIndex != -1)
            {
                int index = generalattack.SelectedIndex;
                string Name = generalattack.SelectedItem.ToString();
                if (index != 0)
                {
                    generalattack.Items.Insert(index - 1, Name);
                    generalattack.Items.RemoveAt(index + 1);
                    Swap(Skill.AttackSkills, index, index - 1);
                }                
            }            
        }

        private void PartySkillUp(object sender, RoutedEventArgs e)
        {
            if (partyattack.SelectedIndex != -1)
            {
                int index = partyattack.SelectedIndex;
                string Name = partyattack.SelectedItem.ToString();
                if (index != 0)
                {
                    partyattack.Items.Insert(index - 1, Name);
                    partyattack.Items.RemoveAt(index + 1);
                    Swap(Skill.PartyAttackSkills, index, index - 1);
                }
            }
        }

        private void SkillDown(object sender, RoutedEventArgs e)
        {
            if (generalattack.SelectedIndex != -1)
            {
                int index = generalattack.SelectedIndex;
                string Name = generalattack.SelectedItem.ToString();
                if (index != generalattack.Items.Count - 1)
                {
                    generalattack.Items.Insert(index + 2, Name);
                    generalattack.Items.RemoveAt(index);
                    Swap(Skill.AttackSkills, index , index + 1);
                }                
            }
        }

        private void PartySkillDown(object sender, RoutedEventArgs e)
        {
            if (partyattack.SelectedIndex != -1)
            {
                int index = partyattack.SelectedIndex;
                string Name = partyattack.SelectedItem.ToString();
                if (index != partyattack.Items.Count - 1)
                {
                    partyattack.Items.Insert(index + 2, Name);
                    partyattack.Items.RemoveAt(index);
                    Swap(Skill.PartyAttackSkills, index, index + 1);
                }
            }
        }

        private void DeleteSkill(object sender, MouseButtonEventArgs e)
        {
            if (generalattack.SelectedIndex != -1)
            {
                Skill.AttackSkills.RemoveAt(generalattack.SelectedIndex);
                generalattack.Items.Remove(generalattack.SelectedItem);
            }
        }

        private void DeletePartySkill(object sender, MouseButtonEventArgs e)
        {
            if (partyattack.SelectedIndex != -1)
            {
                Skill.AttackSkills.RemoveAt(partyattack.SelectedIndex);
                partyattack.Items.Remove(partyattack.SelectedItem);
            }
        }

        private void AddBuff(object sender, RoutedEventArgs e)
        {
            if (skilllist.SelectedIndex != -1)
            {
                SkillList skill = (SkillList)skilllist.SelectedItems[0];
                buff.Items.Add(skill.Name);
                int index = Skill.Skills.FindIndex(buffskill => buffskill.Name.Equals(skill.Name, StringComparison.Ordinal));
                if (Skill.Skills[index].BuffWaiting == 0)
                {
                    Skill.Skills[index].bufftype = 1;
                    Skill.Skills[index].BuffWaiting = 1;
                    Buffas.buff_waiting = true;
                }
            }
        }

        private void DeleteBuff(object sender, MouseButtonEventArgs e)
        {
            if (buff.SelectedIndex != -1)
            {
                foreach (Skill skill in Skill.Skills)
                {
                    if (skill.Name == buff.SelectedItem.ToString())
                    {
                        if (skill.BuffWaiting == 1)
                        {
                            skill.BuffWaiting = 0;
                        }
                        break;
                    }
                }                
                buff.Items.Remove(buff.SelectedItem);               
            }
        }

        private void BuffUp(object sender, RoutedEventArgs e)
        {
            if (buff.SelectedIndex != -1)
            {
                int index = buff.SelectedIndex;
                string Name = buff.SelectedItem.ToString();
                if (index != 0)
                {
                    buff.Items.Insert(index - 1, Name);
                    buff.Items.RemoveAt(index + 1);
                }
            }
        }

        private void BuffDown(object sender, RoutedEventArgs e)
        {
            if (buff.SelectedIndex != -1)
            {
                int index = buff.SelectedIndex;
                string Name = buff.SelectedItem.ToString();
                if (index != buff.Items.Count - 1)
                {
                    buff.Items.Insert(index + 2, Name);
                    buff.Items.RemoveAt(index);
                }
            }
        }

        private void AddSecondBuff(object sender, RoutedEventArgs e)
        {
            if (skilllist.SelectedIndex != -1)
            {
                SkillList skill = (SkillList)skilllist.SelectedItems[0];
                secondbuff.Items.Add(skill.Name);
                int index = Skill.Skills.FindIndex(buffskill => buffskill.Name.Equals(skill.Name, StringComparison.Ordinal));
                if (Skill.Skills[index].BuffWaiting == 0)
                {
                    Skill.Skills[index].bufftype = 3;
                    Skill.Skills[index].BuffWaiting = 1;
                    Buffas.buff_waiting = true;
                }
            }
        }

        private void DeleteSecondBuff(object sender, MouseButtonEventArgs e)
        {
            if (secondbuff.SelectedIndex != -1)
            {
                foreach (Skill skill in Skill.Skills)
                {
                    if (skill.Name == secondbuff.SelectedItem.ToString())
                    {
                        if (skill.BuffWaiting == 1)
                        {
                            skill.BuffWaiting = 0;
                        }
                        break;
                    }
                }                
                secondbuff.Items.Remove(buff.SelectedItem);
            }
        }

        private void BuffSecondUp(object sender, RoutedEventArgs e)
        {
            if (secondbuff.SelectedIndex != -1)
            {
                int index = secondbuff.SelectedIndex;
                string Name = secondbuff.SelectedItem.ToString();
                if (index != 0)
                {
                    secondbuff.Items.Insert(index - 1, Name);
                    secondbuff.Items.RemoveAt(index + 1);
                }
            }
        }

        private void BuffSecondDown(object sender, RoutedEventArgs e)
        {
            if (secondbuff.SelectedIndex != -1)
            {
                int index = secondbuff.SelectedIndex;
                string Name = secondbuff.SelectedItem.ToString();
                if (index != secondbuff.Items.Count - 1)
                {
                    secondbuff.Items.Insert(index + 2, Name);
                    secondbuff.Items.RemoveAt(index);
                }
            }
        }

        private void berserkyes(object sender, RoutedEventArgs e)
        {
            if (berserk.SelectedIndex != -1)
            {
                int index = berserk.SelectedIndex;
                string line = berserk.SelectedItem.ToString();
                string[] split = line.Split(':');
                split[1] = ": Yes";
                string newline = split[0] + split[1];
                berserk.Items.RemoveAt(index);
                berserk.Items.Insert(index, newline);
            }
        }

        private void berserkno(object sender, RoutedEventArgs e)
        {
            if (berserk.SelectedIndex != -1)
            {
                int index = berserk.SelectedIndex;
                string line = berserk.SelectedItem.ToString();
                string[] split = line.Split(':');
                split[1] = ": No";
                string newline = split[0] + split[1];
                berserk.Items.RemoveAt(index);
                berserk.Items.Insert(index, newline);
            }
        }

        private void priorityup(object sender, RoutedEventArgs e)
        {
            string[] split;
            if (priority.SelectedIndex != -1)
            {
                string line = priority.SelectedItems[0].ToString();
                split = line.Split(':');
                switch (split[0])
                {
                    case "Normal":
                        if (DataParser.NormalPriority < 8)
                            DataParser.NormalPriority++;
                        else
                            DataParser.NormalPriority = 8;
                        break;
                    case "Champion":
                        if (DataParser.ChampionPriority < 8)
                            DataParser.ChampionPriority++;
                        else
                            DataParser.ChampionPriority = 8;
                        break;
                    case "Giant":
                        if (DataParser.GiantPriority < 8)
                            DataParser.GiantPriority++;
                        else
                            DataParser.GiantPriority = 8;
                        break;
                    case "Party":
                        if (DataParser.PartyPriority < 8)
                            DataParser.PartyPriority++;
                        else
                            DataParser.PartyPriority = 8;
                        break;
                    case "Party Champion":
                        if (DataParser.PartyChampionPriority < 8)
                            DataParser.PartyChampionPriority++;
                        else
                            DataParser.PartyChampionPriority = 8;
                        break;
                    case "Party Giant":
                        if (DataParser.PartyGiantPriority < 8)
                            DataParser.PartyGiantPriority++;
                        else
                            DataParser.PartyGiantPriority = 8;
                        break;
                    case "Titan":
                        if (DataParser.TitanPriority < 8)
                            DataParser.TitanPriority++;
                        else
                            DataParser.TitanPriority = 8;
                        break;
                    case "Elite":
                        if (DataParser.ElitePriority < 8)
                            DataParser.ElitePriority++;
                        else
                            DataParser.ElitePriority = 8;
                        break;
                    case "Unique":
                        if (DataParser.UniquePriority < 8)
                            DataParser.UniquePriority++;
                        else
                            DataParser.UniquePriority = 8;
                        break;
                }
                MonsterControl.RecheckPriority();
                Globals.MainWindow.UpdatePriorityList();
            }
        }

        private void prioritydown(object sender, RoutedEventArgs e)
        {
            string[] split;
            if (priority.SelectedIndex != -1)
            {
                string line = priority.SelectedItems[0].ToString();
                split = line.Split(':');
                switch (split[0])
                {
                    case "Normal":
                        if (DataParser.NormalPriority > -1)
                            DataParser.NormalPriority--;
                        else
                            DataParser.NormalPriority = -1;
                        break;
                    case "Champion":
                        if (DataParser.ChampionPriority > -1)
                            DataParser.ChampionPriority--;
                        else
                            DataParser.ChampionPriority = -1;
                        break;
                    case "Giant":
                        if (DataParser.GiantPriority > -1)
                            DataParser.GiantPriority--;
                        else
                            DataParser.GiantPriority = -1;
                        break;
                    case "Party":
                        if (DataParser.PartyPriority > -1)
                            DataParser.PartyPriority--;
                        else
                            DataParser.PartyPriority = -1;
                        break;
                    case "Party Champion":
                        if (DataParser.PartyChampionPriority > -1)
                            DataParser.PartyChampionPriority--;
                        else
                            DataParser.PartyChampionPriority = -1;
                        break;
                    case "Party Giant":
                        if (DataParser.PartyGiantPriority > -1)
                            DataParser.PartyGiantPriority--;
                        else
                            DataParser.PartyGiantPriority = -1;
                        break;
                    case "Titan":
                        if (DataParser.TitanPriority > -1)
                            DataParser.TitanPriority--;
                        else
                            DataParser.TitanPriority = -1;
                        break;
                    case "Elite":
                        if (DataParser.ElitePriority > -1)
                            DataParser.ElitePriority--;
                        else
                            DataParser.ElitePriority = -1;
                        break;
                    case "Unique":
                        if (DataParser.UniquePriority > -1)
                            DataParser.UniquePriority--;
                        else
                            DataParser.UniquePriority = -1;
                        break;
                }
                MonsterControl.RecheckPriority();
                Globals.MainWindow.UpdatePriorityList();
            }
        }

        private void advancesearchclick(object sender, RoutedEventArgs e)
        {
            advancemobname.Items.Clear();
            if ((advancemobsearch.Text != "") && (advancemobsearch.Text != null))
            {
                foreach (string Name in Monster.ModNameList)
                {
                    string nameup = Name.ToUpper();
                    if (nameup.Contains(advancemobsearch.Text.ToUpper()))
                    {
                        advancemobname.Items.Add(Name);
                    }
                }
            }
        }

        private void advancemobadd(object sender, RoutedEventArgs e)
        {
            if (advancemobname.SelectedIndex != -1)
            {              
                for (int i = 0; i < Monster.ModNameList.Count; i++)
                {
                    if (Monster.ModNameList[i] == advancemobname.SelectedItem.ToString())
                    {
                        Monster.AvoidMob.Add(Monster.ModTypeList[i]);
                        break;
                    }
                }
                advancemob.Items.Add(advancemobname.SelectedItem.ToString());
            }
        }

        private void advancemobremove(object sender, RoutedEventArgs e)
        {
            if (advancemob.SelectedIndex != -1)
            {
                for (int i = 0; i < Monster.ModNameList.Count; i++)
                {
                    if (Monster.ModNameList[i] == advancemob.SelectedItem.ToString())
                    {
                        Monster.AvoidMob.Remove(Monster.ModTypeList[i]);
                        break;
                    }
                }
                advancemob.Items.Remove(advancemob.SelectedItem);
            }
        }

        private void StartBot()
        {
            if (Data.bot == false)
            {
                Packet NewPacket = new Packet((ushort)0x3026);
                NewPacket.WriteUInt8(7);
                string name = "You just started the bot for " + Character.PlayerName + "! Enjoy botting!";
                NewPacket.WriteUInt16(name.Length);
                NewPacket.WriteUInt8Array(Globals.StringToByteArray(Globals.StringToHex(name)));
                Proxy.ag_local_security.Send(NewPacket);
                Data.bot = true;
                StartLooping.CheckStart();
                Content(startbot, "Stop Bot");
            }           
        }

        private void StopBot()
        {
            Packet NewPacket = new Packet((ushort)0x3026);
            NewPacket.WriteUInt8(7);
            NewPacket.WriteUInt16(12);
            NewPacket.WriteUInt8Array(Globals.StringToByteArray(Globals.StringToHex("Bot stopped!")));
            Proxy.ag_local_security.Send(NewPacket);
            Data.bot = false;
            Data.loop = false;
            LoopControl.i = 0;
            Data.returning = 0;
            Content(startbot, "Start Bot");
        }

        private void BotCheck(object sender, RoutedEventArgs e)
        {
            if ((Globals.MainWindow.x_setbox.Text != "") || (Globals.MainWindow.x_setbox.Text != null))
            {
                if (startbot.Content.ToString() == "Start Bot")
                {
                    StartBot();
                }
                else
                {
                    StopBot();
                }
            }
            else
            {
                Globals.MainWindow.UpdateLogs("Please Set Training Coordinates in the Fight Tab!");
            }
        }

        private Process Started;
        private void StartSRO_Click(object sender, RoutedEventArgs e)
        {
            if (Data.sro_path != "")
            {
                CreateMutex(IntPtr.Zero, false, "Silkroad Online Launcher");
                CreateMutex(IntPtr.Zero, false, "Ready");
                uint count = 0;                
                Process SilkProcess = new Process();
                SilkProcess.StartInfo.FileName = Data.sro_path;
                SilkProcess.StartInfo.Arguments = "0/22 0 0";
                Started = Process.Start(SilkProcess.StartInfo);
                Handle = OpenProcess((uint)(0x000F0000L | 0x00100000L | 0xFFF), 0, Started.Id);               
                uint ConnectionStack = VirtualAllocEx(Handle, IntPtr.Zero, 8, 0x1000, 0x4);
                byte[] ConnectionStackArray = BitConverter.GetBytes(ConnectionStack);
                if (Proxy.port == 20001)
                {
                    byte[] Connection = {
                                            0x02,0x00,
                                            0x4E, 0x21, // PORT (20001)
                                            0x7F,0x00,0x00,0x01 // IP (127.0.0.1)
                                        };
                    uint Codecave = VirtualAllocEx(Handle, IntPtr.Zero, 16, 0x1000, 0x4);
                    byte[] CodecaveArray = BitConverter.GetBytes(Codecave - 0x004B08A1 - 5);
                    byte[] CodeCaveFunc = {
                                                0xBF,ConnectionStackArray[0],ConnectionStackArray[1],ConnectionStackArray[2],ConnectionStackArray[3],
                                                0x8B,0x4E,0x04,
                                                0x6A,0x10,
                                                0x68,0xA6,0x08,0x4B,0x00,
                                                0xC3
                                          };
                    byte[] JMPCodeCave = { 0xE9, CodecaveArray[0], CodecaveArray[1], CodecaveArray[2], CodecaveArray[3] };
                    WriteProcessMemory(Handle, ConnectionStack, Connection, Connection.Length, count);
                    WriteProcessMemory(Handle, Codecave, CodeCaveFunc, CodeCaveFunc.Length, count);
                    WriteProcessMemory(Handle, 0x004B08A1, JMPCodeCave, JMPCodeCave.Length, count);
                }
                else if (Proxy.port == 20002)
                {
                    byte[] Connection = {
                                            0x02,0x00,
                                            0x4E, 0x22, // PORT (20002)
                                            0x7F,0x00,0x00,0x01 // IP (127.0.0.1)
                                        };
                    uint Codecave = VirtualAllocEx(Handle, IntPtr.Zero, 16, 0x1000, 0x4);
                    byte[] CodecaveArray = BitConverter.GetBytes(Codecave - 0x004B08A1 - 5);
                    byte[] CodeCaveFunc = {
                                                0xBF,ConnectionStackArray[0],ConnectionStackArray[1],ConnectionStackArray[2],ConnectionStackArray[3],
                                                0x8B,0x4E,0x04,
                                                0x6A,0x10,
                                                0x68,0xA6,0x08,0x4B,0x00,
                                                0xC3
                                          };
                    byte[] JMPCodeCave = { 0xE9, CodecaveArray[0], CodecaveArray[1], CodecaveArray[2], CodecaveArray[3] };
                    WriteProcessMemory(Handle, ConnectionStack, Connection, Connection.Length, count);
                    WriteProcessMemory(Handle, Codecave, CodeCaveFunc, CodeCaveFunc.Length, count);
                    WriteProcessMemory(Handle, 0x004B08A1, JMPCodeCave, JMPCodeCave.Length, count);
                }
                else if (Proxy.port == 20003)
                {
                    byte[] Connection = {
                                            0x02,0x00,
                                            0x4E, 0x23, // PORT (20003)
                                            0x7F,0x00,0x00,0x01 // IP (127.0.0.1)
                                        };
                    uint Codecave = VirtualAllocEx(Handle, IntPtr.Zero, 16, 0x1000, 0x4);
                    byte[] CodecaveArray = BitConverter.GetBytes(Codecave - 0x004B08A1 - 5);
                    byte[] CodeCaveFunc = {
                                                0xBF,ConnectionStackArray[0],ConnectionStackArray[1],ConnectionStackArray[2],ConnectionStackArray[3],
                                                0x8B,0x4E,0x04,
                                                0x6A,0x10,
                                                0x68,0xA6,0x08,0x4B,0x00,
                                                0xC3
                                          };
                    byte[] JMPCodeCave = { 0xE9, CodecaveArray[0], CodecaveArray[1], CodecaveArray[2], CodecaveArray[3] };
                    WriteProcessMemory(Handle, ConnectionStack, Connection, Connection.Length, count);
                    WriteProcessMemory(Handle, Codecave, CodeCaveFunc, CodeCaveFunc.Length, count);
                    WriteProcessMemory(Handle, 0x004B08A1, JMPCodeCave, JMPCodeCave.Length, count);
                }
                else if (Proxy.port == 20004)
                {
                    byte[] Connection = {
                                            0x02,0x00,
                                            0x4E, 0x24, // PORT (20004)
                                            0x7F,0x00,0x00,0x01 // IP (127.0.0.1)
                                        };
                    uint Codecave = VirtualAllocEx(Handle, IntPtr.Zero, 16, 0x1000, 0x4);
                    byte[] CodecaveArray = BitConverter.GetBytes(Codecave - 0x004B08A1 - 5);
                    byte[] CodeCaveFunc = {
                                                0xBF,ConnectionStackArray[0],ConnectionStackArray[1],ConnectionStackArray[2],ConnectionStackArray[3],
                                                0x8B,0x4E,0x04,
                                                0x6A,0x10,
                                                0x68,0xA6,0x08,0x4B,0x00,
                                                0xC3
                                          };
                    byte[] JMPCodeCave = { 0xE9, CodecaveArray[0], CodecaveArray[1], CodecaveArray[2], CodecaveArray[3] };
                    WriteProcessMemory(Handle, ConnectionStack, Connection, Connection.Length, count);
                    WriteProcessMemory(Handle, Codecave, CodeCaveFunc, CodeCaveFunc.Length, count);
                    WriteProcessMemory(Handle, 0x004B08A1, JMPCodeCave, JMPCodeCave.Length, count);
                }
                else if (Proxy.port == 20005)
                {
                    byte[] Connection = {
                                            0x02,0x00,
                                            0x4E, 0x25, // PORT (20005)
                                            0x7F,0x00,0x00,0x01 // IP (127.0.0.1)
                                        };
                    uint Codecave = VirtualAllocEx(Handle, IntPtr.Zero, 16, 0x1000, 0x4);
                    byte[] CodecaveArray = BitConverter.GetBytes(Codecave - 0x004B08A1 - 5);
                    byte[] CodeCaveFunc = {
                                                0xBF,ConnectionStackArray[0],ConnectionStackArray[1],ConnectionStackArray[2],ConnectionStackArray[3],
                                                0x8B,0x4E,0x04,
                                                0x6A,0x10,
                                                0x68,0xA6,0x08,0x4B,0x00,
                                                0xC3
                                          };
                    byte[] JMPCodeCave = { 0xE9, CodecaveArray[0], CodecaveArray[1], CodecaveArray[2], CodecaveArray[3] };
                    WriteProcessMemory(Handle, ConnectionStack, Connection, Connection.Length, count);
                    WriteProcessMemory(Handle, Codecave, CodeCaveFunc, CodeCaveFunc.Length, count);
                    WriteProcessMemory(Handle, 0x004B08A1, JMPCodeCave, JMPCodeCave.Length, count);
                }
                else if (Proxy.port == 20006)
                {
                    byte[] Connection = {
                                            0x02,0x00,
                                            0x4E, 0x26, // PORT (20006)
                                            0x7F,0x00,0x00,0x01 // IP (127.0.0.1)
                                        };
                    uint Codecave = VirtualAllocEx(Handle, IntPtr.Zero, 16, 0x1000, 0x4);
                    byte[] CodecaveArray = BitConverter.GetBytes(Codecave - 0x004B08A1 - 5);
                    byte[] CodeCaveFunc = {
                                                0xBF,ConnectionStackArray[0],ConnectionStackArray[1],ConnectionStackArray[2],ConnectionStackArray[3],
                                                0x8B,0x4E,0x04,
                                                0x6A,0x10,
                                                0x68,0xA6,0x08,0x4B,0x00,
                                                0xC3
                                          };
                    byte[] JMPCodeCave = { 0xE9, CodecaveArray[0], CodecaveArray[1], CodecaveArray[2], CodecaveArray[3] };
                    WriteProcessMemory(Handle, ConnectionStack, Connection, Connection.Length, count);
                    WriteProcessMemory(Handle, Codecave, CodeCaveFunc, CodeCaveFunc.Length, count);
                    WriteProcessMemory(Handle, 0x004B08A1, JMPCodeCave, JMPCodeCave.Length, count);
                }
                else if (Proxy.port == 20007)
                {
                    byte[] Connection = {
                                            0x02,0x00,
                                            0x4E, 0x27, // PORT (20007)
                                            0x7F,0x00,0x00,0x01 // IP (127.0.0.1)
                                        };
                    uint Codecave = VirtualAllocEx(Handle, IntPtr.Zero, 16, 0x1000, 0x4);
                    byte[] CodecaveArray = BitConverter.GetBytes(Codecave - 0x004B08A1 - 5);
                    byte[] CodeCaveFunc = {
                                                0xBF,ConnectionStackArray[0],ConnectionStackArray[1],ConnectionStackArray[2],ConnectionStackArray[3],
                                                0x8B,0x4E,0x04,
                                                0x6A,0x10,
                                                0x68,0xA6,0x08,0x4B,0x00,
                                                0xC3
                                          };
                    byte[] JMPCodeCave = { 0xE9, CodecaveArray[0], CodecaveArray[1], CodecaveArray[2], CodecaveArray[3] };
                    WriteProcessMemory(Handle, ConnectionStack, Connection, Connection.Length, count);
                    WriteProcessMemory(Handle, Codecave, CodeCaveFunc, CodeCaveFunc.Length, count);
                    WriteProcessMemory(Handle, 0x004B08A1, JMPCodeCave, JMPCodeCave.Length, count);
                }
                else if (Proxy.port == 20008)
                {
                    byte[] Connection = {
                                            0x02,0x00,
                                            0x4E, 0x28, // PORT (20008)
                                            0x7F,0x00,0x00,0x01 // IP (127.0.0.1)
                                        };
                    uint Codecave = VirtualAllocEx(Handle, IntPtr.Zero, 16, 0x1000, 0x4);
                    byte[] CodecaveArray = BitConverter.GetBytes(Codecave - 0x004B08A1 - 5);
                    byte[] CodeCaveFunc = {
                                                0xBF,ConnectionStackArray[0],ConnectionStackArray[1],ConnectionStackArray[2],ConnectionStackArray[3],
                                                0x8B,0x4E,0x04,
                                                0x6A,0x10,
                                                0x68,0xA6,0x08,0x4B,0x00,
                                                0xC3
                                          };
                    byte[] JMPCodeCave = { 0xE9, CodecaveArray[0], CodecaveArray[1], CodecaveArray[2], CodecaveArray[3] };
                    WriteProcessMemory(Handle, ConnectionStack, Connection, Connection.Length, count);
                    WriteProcessMemory(Handle, Codecave, CodeCaveFunc, CodeCaveFunc.Length, count);
                    WriteProcessMemory(Handle, 0x004B08A1, JMPCodeCave, JMPCodeCave.Length, count);
                }
                else if (Proxy.port == 20009)
                {
                    byte[] Connection = {
                                            0x02,0x00,
                                            0x4E, 0x29, // PORT (20009)
                                            0x7F,0x00,0x00,0x01 // IP (127.0.0.1)
                                        };
                    uint Codecave = VirtualAllocEx(Handle, IntPtr.Zero, 16, 0x1000, 0x4);
                    byte[] CodecaveArray = BitConverter.GetBytes(Codecave - 0x004B08A1 - 5);
                    byte[] CodeCaveFunc = {
                                                0xBF,ConnectionStackArray[0],ConnectionStackArray[1],ConnectionStackArray[2],ConnectionStackArray[3],
                                                0x8B,0x4E,0x04,
                                                0x6A,0x10,
                                                0x68,0xA6,0x08,0x4B,0x00,
                                                0xC3
                                          };
                    byte[] JMPCodeCave = { 0xE9, CodecaveArray[0], CodecaveArray[1], CodecaveArray[2], CodecaveArray[3] };
                    WriteProcessMemory(Handle, ConnectionStack, Connection, Connection.Length, count);
                    WriteProcessMemory(Handle, Codecave, CodeCaveFunc, CodeCaveFunc.Length, count);
                    WriteProcessMemory(Handle, 0x004B08A1, JMPCodeCave, JMPCodeCave.Length, count);
                }
                else if (Proxy.port == 20010)
                {
                    byte[] Connection = {
                                            0x02,0x00,
                                            0x4E, 0x2A, // PORT (20010)
                                            0x7F,0x00,0x00,0x01 // IP (127.0.0.1)
                                        };
                    uint Codecave = VirtualAllocEx(Handle, IntPtr.Zero, 16, 0x1000, 0x4);
                    byte[] CodecaveArray = BitConverter.GetBytes(Codecave - 0x004B08A1 - 5);
                    byte[] CodeCaveFunc = {
                                                0xBF,ConnectionStackArray[0],ConnectionStackArray[1],ConnectionStackArray[2],ConnectionStackArray[3],
                                                0x8B,0x4E,0x04,
                                                0x6A,0x10,
                                                0x68,0xA6,0x08,0x4B,0x00,
                                                0xC3
                                          };
                    byte[] JMPCodeCave = { 0xE9, CodecaveArray[0], CodecaveArray[1], CodecaveArray[2], CodecaveArray[3] };
                    WriteProcessMemory(Handle, ConnectionStack, Connection, Connection.Length, count);
                    WriteProcessMemory(Handle, Codecave, CodeCaveFunc, CodeCaveFunc.Length, count);
                    WriteProcessMemory(Handle, 0x004B08A1, JMPCodeCave, JMPCodeCave.Length, count);
                }        

                Configs.SaveInfo();                               
            }
            else
            {
                Globals.MainWindow.UpdateLogs("Please choose Silkroad folder!");
            }
        }

        public static bool ApplicationIsActivated()
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;
            }

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            return activeProcId == procId;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);


        public void Checkstarthotkey()
        {
            SROHandle = Started.MainWindowHandle;
            if (CheckEnable(startbot) == true)
            {
                if (ApplicationIsActivated() == true || GetForegroundWindow() == SROHandle)
                {
                    StartBot();
                }
            }
        }

        public void Checkstophotkey()
        {
            SROHandle = Started.MainWindowHandle;
            if (ReadText(startbot) == "Stop Bot")
            {
                if (ApplicationIsActivated() == true || GetForegroundWindow() == SROHandle)
                {
                    StopBot();
                }
            }
        }


        private void SelectSRO_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fdialog = new Microsoft.Win32.OpenFileDialog();
            fdialog.Filter = "sro_client.exe | *.exe";
            fdialog.ShowDialog();
            if (File.Exists(fdialog.FileName) && fdialog.FileName.ToUpper().Contains("SRO_CLIENT.EXE"))
            {
                Data.sro_path = fdialog.FileName;
                //Globals.Configs.WriteBotConfigs();
            }
        }

        private void SetCoordinate(object sender, RoutedEventArgs e)
        {
            this.x_setbox.Text = xlabel.Content.ToString();
            this.y_setbox.Text = ylabel.Content.ToString();
        }

        public void Checkcoordinate()
        {
            SetText(x_setbox, ReadText(xlabel));
            SetText(y_setbox, ReadText(ylabel));
            Packet NewPacket = new Packet((ushort)0x3026);
            NewPacket.WriteUInt8(7);
            string name = "Set coordinate at " + ReadText(xlabel) + ", " + ReadText(ylabel) + " using hot key!";
            NewPacket.WriteUInt16(name.Length);
            NewPacket.WriteUInt8Array(Globals.StringToByteArray(Globals.StringToHex(name)));
            Proxy.ag_local_security.Send(NewPacket);
        }

        private void AdvanceListCat(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = advanceitemselect.SelectedItem as ComboBoxItem;

            switch (item.Content.ToString())
            {
                case "Gold":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Gold)
                    {
                        advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                    }
                    break;
                case "Weapons":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Weapon)
                    {
                        advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                    }
                    break;
                case "Armors":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Armor)
                    {
                        advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                    }
                    break;
                case "Accessorises":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Accessorise)
                    {
                        advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                    }
                    break;
                case "Elixirs":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Elixir)
                    {
                        advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                    }
                    break;
                case "Arrows/Bolts":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Arrow)
                    {
                        advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                    }
                    break;
                case "Return Scrolls":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Return)
                    {
                        advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                    }
                    break;
                case "Potions":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Potion)
                    {
                        advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                    }
                    break;
                case "Tablets/Jades":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Tablet)
                    {
                        advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                    }
                    break;
                case "Materials":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Material)
                    {
                        advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                    }
                    break;
                case "Malls":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Mall)
                    {
                        advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                    }
                    break;
                case "Quests":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Quest)
                    {
                        advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                    }
                    break;
                case "All Items":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Allitems)
                    {
                        advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                    }
                    break;
            }
        }

        private void pickselect(object sender, RoutedEventArgs e)
        {
            if (advanceitem.SelectedItems.Count > 0)
            {
                ComboBoxItem item = advanceitemselect.SelectedItem as ComboBoxItem;
                IList<AdvanceList> selecting = advanceitem.SelectedItems.Cast<AdvanceList>().ToList();

                switch (item.Content.ToString())
                {
                    case "Gold":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Gold)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Just Pick";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Weapons":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Weapon)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Just Pick";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Armors":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Armor)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Just Pick";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Accessorises":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Accessorise)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Just Pick";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Elixirs":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Elixir)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Just Pick";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Arrows/Bolts":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Arrow)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Just Pick";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Return Scrolls":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Return)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Just Pick";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Potions":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Potion)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Just Pick";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Tablets/Jades":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Tablet)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Just Pick";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Materials":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Material)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Just Pick";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Malls":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Mall)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Just Pick";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Quests":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Quest)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Just Pick";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "All Items":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Allitems)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Just Pick";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                }
            }
        }

        private void sellselect(object sender, RoutedEventArgs e)
        {
            if (advanceitem.SelectedItems.Count > 0)
            {
                ComboBoxItem item = advanceitemselect.SelectedItem as ComboBoxItem;
                IList<AdvanceList> selecting = advanceitem.SelectedItems.Cast<AdvanceList>().ToList();

                switch (item.Content.ToString())
                {
                    case "Gold":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Gold)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Sell";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Weapons":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Weapon)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Sell";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Armors":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Armor)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Sell";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Accessorises":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Accessorise)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Sell";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Elixirs":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Elixir)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Sell";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Arrows/Bolts":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Arrow)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Sell";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Return Scrolls":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Return)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Sell";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Potions":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Potion)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Sell";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Tablets/Jades":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Tablet)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Sell";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Materials":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Material)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Sell";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Malls":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Mall)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Sell";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Quests":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Quest)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Sell";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "All Items":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Allitems)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Sell";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                }
            }
        }

        private void storeselect(object sender, RoutedEventArgs e)
        {
            if (advanceitem.SelectedItems.Count > 0)
            {
                ComboBoxItem item = advanceitemselect.SelectedItem as ComboBoxItem;
                IList<AdvanceList> selecting = advanceitem.SelectedItems.Cast<AdvanceList>().ToList();

                switch (item.Content.ToString())
                {
                    case "Gold":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Gold)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Store";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Weapons":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Weapon)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Store";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Armors":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Armor)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Store";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Accessorises":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Accessorise)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Store";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Elixirs":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Elixir)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Store";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Arrows/Bolts":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Arrow)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Store";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Return Scrolls":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Return)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Store";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Potions":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Potion)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Store";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Tablets/Jades":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Tablet)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Store";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Materials":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Material)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Store";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Malls":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Mall)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Store";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Quests":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Quest)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Store";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "All Items":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Allitems)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Store";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                }
            }
        }

        private void ignoreselect(object sender, RoutedEventArgs e)
        {
            if (advanceitem.SelectedItems.Count > 0)
            {
                ComboBoxItem item = advanceitemselect.SelectedItem as ComboBoxItem;
                IList<AdvanceList> selecting = advanceitem.SelectedItems.Cast<AdvanceList>().ToList();

                switch (item.Content.ToString())
                {
                    case "Gold":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Gold)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Ignore";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Weapons":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Weapon)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Ignore";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Armors":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Armor)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Ignore";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Accessorises":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Accessorise)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Ignore";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Elixirs":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Elixir)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Ignore";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Arrows/Bolts":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Arrow)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Ignore";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Return Scrolls":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Return)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Ignore";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Potions":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Potion)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Ignore";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Tablets/Jades":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Tablet)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Ignore";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Materials":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Material)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Ignore";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Malls":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Mall)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Ignore";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "Quests":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Quest)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Ignore";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                    case "All Items":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Allitems)
                        {
                            foreach (AdvanceList select in selecting)
                            {
                                if (select.Name == AItem.AdvanceName)
                                {
                                    AItem.Action = "Ignore";
                                    break;
                                }
                            }
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }
                        break;
                }
            }
        }

        private void namesearch(object sender, RoutedEventArgs e)
        {
            ComboBoxItem item = advanceitemselect.SelectedItem as ComboBoxItem;

            switch (item.Content.ToString())
            {
                case "Gold":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Gold)
                    {
                        if (AItem.AdvanceName.ToUpper().Contains(advanceitemnamesearch.Text.ToString().ToUpper()))
                        {
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }                        
                    }
                    break;
                case "Weapons":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Weapon)
                    {
                        if (AItem.AdvanceName.ToUpper().Contains(advanceitemnamesearch.Text.ToString().ToUpper()))
                        {
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }       
                    }
                    break;
                case "Armors":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Armor)
                    {
                        if (AItem.AdvanceName.ToUpper().Contains(advanceitemnamesearch.Text.ToString().ToUpper()))
                        {
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }       
                    }
                    break;
                case "Accessorises":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Accessorise)
                    {
                        if (AItem.AdvanceName.ToUpper().Contains(advanceitemnamesearch.Text.ToString().ToUpper()))
                        {
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }       
                    }
                    break;
                case "Elixirs":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Elixir)
                    {
                        if (AItem.AdvanceName.ToUpper().Contains(advanceitemnamesearch.Text.ToString().ToUpper()))
                        {
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }       
                    }
                    break;
                case "Arrows/Bolts":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Arrow)
                    {
                        if (AItem.AdvanceName.ToUpper().Contains(advanceitemnamesearch.Text.ToString().ToUpper()))
                        {
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }       
                    }
                    break;
                case "Return Scrolls":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Return)
                    {
                        if (AItem.AdvanceName.ToUpper().Contains(advanceitemnamesearch.Text.ToString().ToUpper()))
                        {
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }       
                    }
                    break;
                case "Potions":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Potion)
                    {
                        if (AItem.AdvanceName.ToUpper().Contains(advanceitemnamesearch.Text.ToString().ToUpper()))
                        {
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }       
                    }
                    break;
                case "Tablets/Jades":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Tablet)
                    {
                        if (AItem.AdvanceName.ToUpper().Contains(advanceitemnamesearch.Text.ToString().ToUpper()))
                        {
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }       
                    }
                    break;
                case "Materials":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Material)
                    {
                        if (AItem.AdvanceName.ToUpper().Contains(advanceitemnamesearch.Text.ToString().ToUpper()))
                        {
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }       
                    }
                    break;
                case "Malls":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Mall)
                    {
                        if (AItem.AdvanceName.ToUpper().Contains(advanceitemnamesearch.Text.ToString().ToUpper()))
                        {
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }       
                    }
                    break;
                case "Quests":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Quest)
                    {
                        if (AItem.AdvanceName.ToUpper().Contains(advanceitemnamesearch.Text.ToString().ToUpper()))
                        {
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }       
                    }
                    break;
                case "All Items":
                    advanceitem.Items.Clear();
                    foreach (AdvanceItem AItem in AdvanceItem.Allitems)
                    {
                        if (AItem.AdvanceName.ToUpper().Contains(advanceitemnamesearch.Text.ToString().ToUpper()))
                        {
                            advanceitem.Items.Add(new AdvanceList() {Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                        }       
                    }
                    break;
            }
        }

        private void levelsearch(object sender, RoutedEventArgs e)
        {
            try
            {
                ComboBoxItem item = advanceitemselect.SelectedItem as ComboBoxItem;

                switch (item.Content.ToString())
                {
                    case "Gold":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Gold)
                        {
                            if (AItem.AdvanceLevel == Convert.ToInt32(searchitembox.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Weapons":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Weapon)
                        {
                            if (AItem.AdvanceLevel == Convert.ToInt32(searchitembox.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Armors":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Armor)
                        {
                            if (AItem.AdvanceLevel == Convert.ToInt32(searchitembox.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Accessorises":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Accessorise)
                        {
                            if (AItem.AdvanceLevel == Convert.ToInt32(searchitembox.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Elixirs":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Elixir)
                        {
                            if (AItem.AdvanceLevel == Convert.ToInt32(searchitembox.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Arrows/Bolts":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Arrow)
                        {
                            if (AItem.AdvanceLevel == Convert.ToInt32(searchitembox.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Return Scrolls":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Return)
                        {
                            if (AItem.AdvanceLevel == Convert.ToInt32(searchitembox.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Potions":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Potion)
                        {
                            if (AItem.AdvanceLevel == Convert.ToInt32(searchitembox.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Tablets/Jades":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Tablet)
                        {
                            if (AItem.AdvanceLevel == Convert.ToInt32(searchitembox.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Materials":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Material)
                        {
                            if (AItem.AdvanceLevel == Convert.ToInt32(searchitembox.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Malls":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Mall)
                        {
                            if (AItem.AdvanceLevel == Convert.ToInt32(searchitembox.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Quests":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Quest)
                        {
                            if (AItem.AdvanceLevel == Convert.ToInt32(searchitembox.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "All Items":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Allitems)
                        {
                            if (AItem.AdvanceLevel == Convert.ToInt32(searchitembox.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                }
            }
            catch { }
        }

        private void actionsearch(object sender, RoutedEventArgs e)
        {
            try
            {
                ComboBoxItem item = advanceitemselect.SelectedItem as ComboBoxItem;

                switch (item.Content.ToString())
                {
                    case "Gold":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Gold)
                        {
                            if (AItem.Action.ToUpper() == (searchitembox.Text.ToString().ToUpper()))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Weapons":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Weapon)
                        {
                            if (AItem.Action.ToUpper() == (searchitembox.Text.ToString().ToUpper()))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Armors":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Armor)
                        {
                            if (AItem.Action.ToUpper() == (searchitembox.Text.ToString().ToUpper()))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Accessorises":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Accessorise)
                        {
                            if (AItem.Action.ToUpper() == (searchitembox.Text.ToString().ToUpper()))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Elixirs":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Elixir)
                        {
                            if (AItem.Action.ToUpper() == (searchitembox.Text.ToString().ToUpper()))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Arrows/Bolts":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Arrow)
                        {
                            if (AItem.Action.ToUpper() == (searchitembox.Text.ToString().ToUpper()))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Return Scrolls":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Return)
                        {
                            if (AItem.Action.ToUpper() == (searchitembox.Text.ToString().ToUpper()))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Potions":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Potion)
                        {
                            if (AItem.Action.ToUpper() == (searchitembox.Text.ToString().ToUpper()))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Tablets/Jades":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Tablet)
                        {
                            if (AItem.Action.ToUpper() == (searchitembox.Text.ToString().ToUpper()))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Materials":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Material)
                        {
                            if (AItem.Action.ToUpper() == (searchitembox.Text.ToString().ToUpper()))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Malls":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Mall)
                        {
                            if (AItem.Action.ToUpper() == (searchitembox.Text.ToString().ToUpper()))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Quests":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Quest)
                        {
                            if (AItem.Action.ToUpper() == (searchitembox.Text.ToString().ToUpper()))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "All Items":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Allitems)
                        {
                            if (AItem.Action.ToUpper() == (searchitembox.Text.ToString().ToUpper()))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                }
            }
            catch { }
        }

        private void levelrangesearch(object sender, RoutedEventArgs e)
        {
            try
            {
                ComboBoxItem item = advanceitemselect.SelectedItem as ComboBoxItem;

                switch (item.Content.ToString())
                {
                    case "Gold":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Gold)
                        {
                            if (AItem.AdvanceLevel >= Convert.ToInt32(min.Text) &&  AItem.AdvanceLevel <= Convert.ToInt32(max.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Weapons":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Weapon)
                        {
                            if (AItem.AdvanceLevel >= Convert.ToInt32(min.Text) && AItem.AdvanceLevel <= Convert.ToInt32(max.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Armors":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Armor)
                        {
                            if (AItem.AdvanceLevel >= Convert.ToInt32(min.Text) && AItem.AdvanceLevel <= Convert.ToInt32(max.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Accessorises":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Accessorise)
                        {
                            if (AItem.AdvanceLevel >= Convert.ToInt32(min.Text) && AItem.AdvanceLevel <= Convert.ToInt32(max.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Elixirs":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Elixir)
                        {
                            if (AItem.AdvanceLevel >= Convert.ToInt32(min.Text) && AItem.AdvanceLevel <= Convert.ToInt32(max.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Arrows/Bolts":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Arrow)
                        {
                            if (AItem.AdvanceLevel >= Convert.ToInt32(min.Text) && AItem.AdvanceLevel <= Convert.ToInt32(max.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Return Scrolls":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Return)
                        {
                            if (AItem.AdvanceLevel >= Convert.ToInt32(min.Text) && AItem.AdvanceLevel <= Convert.ToInt32(max.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Potions":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Potion)
                        {
                            if (AItem.AdvanceLevel >= Convert.ToInt32(min.Text) && AItem.AdvanceLevel <= Convert.ToInt32(max.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Tablets/Jades":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Tablet)
                        {
                            if (AItem.AdvanceLevel >= Convert.ToInt32(min.Text) && AItem.AdvanceLevel <= Convert.ToInt32(max.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Materials":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Material)
                        {
                            if (AItem.AdvanceLevel >= Convert.ToInt32(min.Text) && AItem.AdvanceLevel <= Convert.ToInt32(max.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Malls":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Mall)
                        {
                            if (AItem.AdvanceLevel >= Convert.ToInt32(min.Text) && AItem.AdvanceLevel <= Convert.ToInt32(max.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "Quests":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Quest)
                        {
                            if (AItem.AdvanceLevel >= Convert.ToInt32(min.Text) && AItem.AdvanceLevel <= Convert.ToInt32(max.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                    case "All Items":
                        advanceitem.Items.Clear();
                        foreach (AdvanceItem AItem in AdvanceItem.Allitems)
                        {
                            if (AItem.AdvanceLevel >= Convert.ToInt32(min.Text) && AItem.AdvanceLevel <= Convert.ToInt32(max.Text))
                            {
                                advanceitem.Items.Add(new AdvanceList() { Name = AItem.AdvanceName, Level = AItem.AdvanceLevel, Action = AItem.Action });
                            }
                        }
                        break;
                }
            }
            catch { }
        }

        private void Selectfirstitem(object sender, RoutedEventArgs e)
        {
            if (inventory.SelectedIndex != -1)
            {
                ItemList item = (ItemList)inventory.SelectedItem;
                Data.f_wep_name = Data.inventorytype[Data.inventoryname.IndexOf(item.Name)];
                firstweapon.Items.Clear();
                firstweapon.Items.Add(item.Name);
            }
        }

        private void Selectseconditem(object sender, RoutedEventArgs e)
        {
            if (inventory.SelectedIndex != -1)
            {
                ItemList item = (ItemList)inventory.SelectedItem;
                Data.s_wep_name = Data.inventorytype[Data.inventoryname.IndexOf(item.Name)];
                secondweapon.Items.Clear();
                secondweapon.Items.Add(item.Name);
            }
        }

        private void Dropselect(object sender, RoutedEventArgs e)
        {
            if (inventory.SelectedIndex != -1)
            {
                ItemList item = (ItemList)inventory.SelectedItem;
                Packet NewPacket = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYMOVEMENT);
                NewPacket.WriteUInt8(7); // Drop Item
                NewPacket.WriteUInt8(item.Slot);
                Proxy.ag_remote_security.Send(NewPacket);
            }
        }

        private void hptextchange(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Convert.ToDouble(hptext.Text) < 0 || Convert.ToDouble(hptext.Text) > 100)
                {
                    hptext.Text = "50";
                }
            }
            catch { hptext.Text = "50"; }            
        }

        private void pethptextchange(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Convert.ToDouble(pethptext.Text) < 0 || Convert.ToDouble(pethptext.Text) > 100)
                {
                    pethptext.Text = "50";
                }
            }
            catch { pethptext.Text = "50"; }
        }

        private void pethgptextchange(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Convert.ToDouble(pethgptext.Text) < 0 || Convert.ToDouble(pethgptext.Text) > 100)
                {
                    pethgptext.Text = "50";
                }
            }
            catch { pethgptext.Text = "50"; }
        }

        private void horsehptextchange(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Convert.ToDouble(horsehptext.Text) < 0 || Convert.ToDouble(horsehptext.Text) > 100)
                {
                    horsehptext.Text = "50";
                }
            }
            catch { horsehptext.Text = "50"; }
        }

        private void mptextchange(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(mptext.Text) < 0 || Convert.ToInt32(mptext.Text) > 100)
                {
                    mptext.Text = "50";
                }
            }
            catch { mptext.Text = "50"; }
        }

        private void vigorhptextchange(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(vigorhptext.Text) < 0 || Convert.ToInt32(vigorhptext.Text) > 100)
                {
                    vigorhptext.Text = "50";
                }           
            }
            catch { vigorhptext.Text = "50"; }
        }

        private void vigormptextchange(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(vigormptext.Text) < 0 || Convert.ToInt32(vigormptext.Text) > 100)
                {
                    vigormptext.Text = "50";
                }
            }
            catch { vigormptext.Text = "50"; }
        }

        private void shptextchange(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(shptext.Text) < 0 || Convert.ToInt32(shptext.Text) > 100)
                {
                    shptext.Text = "50";
                }
            }
            catch { shptext.Text = "50"; }
        }

        private void smptextchange(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(smptext.Text) < 0 || Convert.ToInt32(smptext.Text) > 100)
                {
                    smptext.Text = "50";
                }
            }
            catch { smptext.Text = "50"; }
        }

        private void skillhptextchange(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(skillhptext.Text) < 0 || Convert.ToInt32(skillhptext.Text) > 100)
                {
                    skillhptext.Text = "50";
                }
            }
            catch { skillhptext.Text = "50"; }
        }

        public void Updatehealskill()
        {           
            if (healskill.Dispatcher.CheckAccess() == true)
            {
                healskill.Items.Clear();
                foreach (Skill skill in Skill.Skills)
                {
                    if (skill.Type.Contains("SELFHEAL"))
                    {
                        healskill.Items.Add(skill.Name);
                    }
                }
            }
            else
            {
                healskill.Dispatcher.Invoke((Action)delegate()
                {
                    Updatehealskill();
                });
            }
        }

        public void Updateserverlist()
        {
            if (serverlist.Dispatcher.CheckAccess() == true)
            {
                serverlist.Items.Clear();
                foreach (string server in Data.ServerName)
                {
                    serverlist.Items.Add(server);
                }
                serverlist.SelectedIndex = 0;
            }
            else
            {
                serverlist.Dispatcher.Invoke((Action)delegate()
                {
                    Updateserverlist();
                });
            }
        }

        public void Selectscript(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fDialog = new Microsoft.Win32.OpenFileDialog();
            fDialog.Title = "Select WalkScript";
            fDialog.Filter = "Text File|*.txt";
            fDialog.InitialDirectory = Environment.CurrentDirectory;
            bool? result = fDialog.ShowDialog();
            if (result == true)
            {
                string tmppath = fDialog.FileName.ToString();
                Data.walkscriptpath = tmppath;
                walkscriptpath.Text = tmppath;
                walkscript.Items.Clear();
                var lineCount = File.ReadAllLines(tmppath).Length;
                TextReader tr = new StreamReader(tmppath);
                for (int i = 0; i < lineCount; i++)
                {                    
                    string input = tr.ReadLine();
                    walkscript.Items.Add(input);
                }
                tr.Close();    
            }
            else
            {
                MessageBox.Show("Unable To Load Walk Script!");
            }
        }

        private void Recordscript(object sender, RoutedEventArgs e)
        {
            Data.loopaction = "record";
            scriptrecord.IsEnabled = false;
            scriptstop.IsEnabled = true;
            scriptclear.IsEnabled = false;
            scriptsave.IsEnabled = false;
        }

        private void Stoprecord(object sender, RoutedEventArgs e)
        {
            Data.loopaction = null;
            scriptrecord.IsEnabled = true;
            scriptstop.IsEnabled = false;
            scriptclear.IsEnabled = true;
            scriptsave.IsEnabled = true;
        }

        private void Saverecord(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog save = new Microsoft.Win32.SaveFileDialog();
            save.Title = "Save WalkScript";
            save.Filter = "Text File|*.txt";
            save.InitialDirectory = Environment.CurrentDirectory;
            if (save.ShowDialog() == true)
            {
                string tmppath = save.FileName.ToString();
                TextWriter tw = new StreamWriter(tmppath);
                for (int i = 0; i < Globals.MainWindow.walkscript.Items.Count; i++)
                {
                    tw.WriteLine(Globals.MainWindow.walkscript.Items[i].ToString());
                }
                tw.Close();
            }
            else
            {
                MessageBox.Show("Unable To Save!");
            }
        }

        private void Recordtrainrecord(object sender, RoutedEventArgs e)
        {
            Movement.TrainingRecording = true;
            stop.IsEnabled = true;
            clear.IsEnabled = false;
            load.IsEnabled = false;
            load.IsEnabled = false;
        }

        private void Stoptrainrecord(object sender, RoutedEventArgs e)
        {
            Movement.TrainingRecording = false;
            stop.IsEnabled = false;
            clear.IsEnabled = true;
            load.IsEnabled = true;
            load.IsEnabled = true;
        }

        private void Cleartrainrecord(object sender, RoutedEventArgs e)
        {
            trainbox.Items.Clear();
        }

        private void Loadtrainrecord(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fDialog = new Microsoft.Win32.OpenFileDialog();
            fDialog.Title = "Select TrainScript";
            fDialog.Filter = "Text File|*.txt";
            fDialog.InitialDirectory = Environment.CurrentDirectory;
            bool? result = fDialog.ShowDialog();
            if (result == true)
            {
                string tmppath = fDialog.FileName.ToString();
                trainrecordpath.Text = tmppath;
                trainbox.Items.Clear();
                var lineCount = File.ReadAllLines(tmppath).Length;
                TextReader tr = new StreamReader(tmppath);
                for (int i = 0; i < lineCount; i++)
                {
                    string input = tr.ReadLine();
                    trainbox.Items.Add(input);
                }
                tr.Close();
            }
            else
            {
                MessageBox.Show("Unable To Load Train Script!");
            }
        }

        private void Savetrainrecord(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog save = new Microsoft.Win32.SaveFileDialog();
            save.Title = "Save TrainScript";
            save.Filter = "Text File|*.txt";
            save.InitialDirectory = Environment.CurrentDirectory;
            if (save.ShowDialog() == true)
            {
                string tmppath = save.FileName.ToString();
                trainrecordpath.Text = tmppath;
                TextWriter tw = new StreamWriter(tmppath);
                for (int i = 0; i < Globals.MainWindow.trainbox.Items.Count; i++)
                {
                    tw.WriteLine(Globals.MainWindow.trainbox.Items[i].ToString());
                }
                tw.Close();
            }
            else
            {
                MessageBox.Show("Unable To Save Train Script!");
            }
        }

        private void Clearrecord(object sender, RoutedEventArgs e)
        {
            walkscript.Items.Clear();
        }

        private void Saveconfig(object sender, RoutedEventArgs e)
        {
            Configs.WriteConfigs();
        }

        private void Loadconfig(object sender, RoutedEventArgs e)
        {
            Configs.ReadConfigs();
        }

        private void PartyCheck(object sender, RoutedEventArgs e)
        {
            if (autoparty.IsChecked == true)
            {
                if (Data.hasparty == false)
                {
                    Party.CreateParty();
                }
            }
        }

        #region Client Manager      

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        private const int SW_SHOWMAXIMIZED = 3;
        private const UInt32 WM_CLOSE = 0x0010;

        [DllImport("user32.dll")]
        private static extern int SetWindowText(IntPtr hWnd, string windowName);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool EndTask(IntPtr hWnd, bool fShutDown, bool fForce);

        [DllImport("User32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string strClassName, string strWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, [Out] StringBuilder lParam);


        private void Getprocess(object sender, RoutedEventArgs e)
        {
            try 
            {
                processlist.Items.Clear();
                Windows.Wins.Clear();
                Process[] processes = Process.GetProcessesByName("sro_client");
                {
                    foreach (Process process in processes)
                    {
                        // Search through all top level windows
                        // Compare process IDs with the saved ID of our started process
                        // If found, restore it
                        IntPtr h = IntPtr.Zero;
                        IntPtr hw = IntPtr.Zero;
                        int tid;
                        int pid;
                        // Loop through all top-level windows
                        do
                        {
                            pid = 0;
                            // get the window handle
                            h = FindWindowEx(IntPtr.Zero, h, null, null);
                            // get the process id (and thread id)
                            tid = GetWindowThreadProcessId(h, out pid);
                            // test for a match of our saved pID
                            if (pid == process.Id)
                            {
                                hw = h;
                            }
                        }
                        while (!h.Equals(IntPtr.Zero));
                        // Allocate correct string length first
                        int length = (int)SendMessage(hw, 0x000E, IntPtr.Zero, IntPtr.Zero);
                        StringBuilder sb = new StringBuilder(length + 1);
                        SendMessage(hw, 0x000D, (IntPtr)sb.Capacity, sb);
                        Windows win = new Windows(process.Id, hw, sb.ToString());
                        Windows.Wins.Add(win);
                    }
                }

                foreach (Windows win in Windows.Wins)
                {
                    processlist.Items.Add(win.Tiltle);
                }    
            }
            catch 
            {
                UpdateLogs("Error refreshing!");
            }              
        }

        private void Hideprocess(object sender, RoutedEventArgs e)
        {
            if (processlist.SelectedIndex != -1)
            {
                ShowWindow(Windows.Wins[processlist.SelectedIndex].HWnd, SW_HIDE);
            }
        }

        private void Showprocess(object sender, RoutedEventArgs e)
        {
            if (processlist.SelectedIndex != -1)
            {
                ShowWindow(Windows.Wins[processlist.SelectedIndex].HWnd, SW_SHOW);
            }
        }

        private void Killprocess(object sender, RoutedEventArgs e)
        {
            if (processlist.SelectedIndex != -1)
            {
                EndTask(Windows.Wins[processlist.SelectedIndex].HWnd, false, true);
                Windows.Wins.RemoveAt(processlist.SelectedIndex);
                processlist.Items.Clear();
                foreach (Windows win in Windows.Wins.ToList())
                {
                    processlist.Items.Add(win.Tiltle);
                }
            }
        }

        private void Renameprocess(object sender, RoutedEventArgs e)
        {
            if (processlist.SelectedIndex != -1 && processname.Text != "")
            {
                SetWindowText(Windows.Wins[processlist.SelectedIndex].HWnd, processname.Text);
                Windows.Wins[processlist.SelectedIndex].Tiltle = processname.Text;
                processlist.Items.Clear();
                foreach (Windows win in Windows.Wins.ToList())
                {
                    processlist.Items.Add(win.Tiltle);
                }
            }
        }

        #endregion 

        private void CaptchaAns(object sender, RoutedEventArgs e)
        {
            Packet NewPacket = new Packet((ushort)0x6323);
            NewPacket.WriteAscii(capans.Text);
            Proxy.gw_remote_security.Send(NewPacket);
        }

        private void AutoLoginCheck(object sender, RoutedEventArgs e)
        {
            try
            {
                if (autologin.IsChecked == true && Proxy.connection == true)
                {
                    Packet NewPacket = new Packet((ushort)LoginServerOpcodes.CLIENT_OPCODES.LOGIN, true);
                    NewPacket.WriteUInt8(22);
                    NewPacket.WriteAscii(Globals.MainWindow.ReadText(Globals.MainWindow.username));
                    NewPacket.WriteAscii(Globals.MainWindow.ReadText(Globals.MainWindow.password));
                    NewPacket.WriteUInt16(Data.ServerID[Data.ServerName.IndexOf(Globals.MainWindow.ReadText1(Globals.MainWindow.serverlist))]);
                    System.Threading.Thread.Sleep(300);
                    Proxy.gw_remote_security.Send(NewPacket);
                }
            }
            catch (Exception)
            {
                Globals.MainWindow.UpdateLogs("Please enter username and password first!");
                autologin.IsChecked = false;
            }
        }

        private void AutoSelectCheck(object sender, RoutedEventArgs e)
        {
            if (characterselect.IsChecked == true && Character.haslist == true)
            {
                if (charnameselect.Text != "" || charnameselect.Text != null)
                {
                    Packet NewPacket = new Packet((ushort)0x7001);
                    NewPacket.WriteAscii(charnameselect.Text);
                    Proxy.ag_remote_security.Send(NewPacket);
                }
                else
                {
                    Globals.MainWindow.UpdateLogs("Please fill in name!");
                }
            }
        }

        private void SelectCharacter(object sender, RoutedEventArgs e)
        {
            if (charlist.SelectedIndex != -1)
            {
                Packet NewPacket = new Packet((ushort)0x7001);
                NewPacket.WriteAscii(charlist.SelectedItem.ToString());
                Proxy.ag_remote_security.Send(NewPacket);
            }
        }

        private void GhostWalkCheck(object sender, RoutedEventArgs e)
        {
            if (useghostwalk.IsChecked == true)
            {
                enable = false;
                addghostwalk.IsEnabled = false;
            }
            else
            {
                enable = true;
                addghostwalk.IsEnabled = true;
            }
        }

        private void DistanceCheck(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Convert.ToDouble(ghostwalkdistance.Text) <= 0) 
                {
                    ghostwalkdistance.Text = "40";
                }
            }
            catch
            {
                ghostwalkdistance.Text = "40";
            }
        }

        private void Trainingbar(object sender, RoutedEventArgs e)
        {
            if (PortConfigs.TrainWindow.IsVisible == false)
            {
                PortConfigs.TrainWindow.Show();
            }
            else
            {
                PortConfigs.TrainWindow.Hide();
            }
        }

        System.Timers.Timer CloseTimer;

        private void Checkcloseclient(object sender, RoutedEventArgs e)
        {
            if (closeclient.IsChecked == true)
            {
                try
                {
                    CloseTimer.Stop();
                    CloseTimer.Dispose();
                }
                catch { }
                CloseTimer = new System.Timers.Timer();
                CloseTimer.Elapsed += new ElapsedEventHandler(CloseTick);
                CloseTimer.Interval = 60000;
                CloseTimer.Start();
                CloseTimer.Enabled = true;
            }
            else
            {
                try
                {
                    CloseTimer.Stop();
                    CloseTimer.Dispose();
                }
                catch { }
            }
        }

        private void CloseTick(object sender, ElapsedEventArgs e)
        {
            Globals.MainWindow.SetText(timeclose, Convert.ToString(Convert.ToInt32(Globals.MainWindow.ReadText(timeclose)) - 1));
            if (Globals.MainWindow.ReadText(timeclose) == "0")
            {
                Started.Kill();
            }
        }

        private void TimeCheck(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(timeclose.Text) < 0)
                {
                    timeclose.Text = "180";
                }
            }
            catch
            {
                timeclose.Text = "180";
            }
        }

        public void Refresh(object sender, RoutedEventArgs e)
        {
            UpdateSkillList();
        }
    }

    class Windows
    {
        public int Id { get; set; }
        public IntPtr HWnd { get; set; }
        public string Tiltle { get; set; }
        public Windows(int id, IntPtr hwnd, string tiltle)
        {
            this.Id = id;
            this.HWnd = hwnd;
            this.Tiltle = tiltle;
        }

        public static List<Windows> Wins = new List<Windows>();
    }
}
