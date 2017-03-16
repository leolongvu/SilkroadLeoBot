using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoBot
{
    class LoginHandler
    {
        public static void Handler(Packet packet)
        {
            switch (packet.Opcode)
            {
                case (ushort)LoginServerOpcodes.SERVER_OPCODES.SERVER_LIST:
                    Servers.GatewayRespond(packet);
                    break;
                case (ushort)0x2322:
                    Globals.MainWindow.Enable(Globals.MainWindow.capans);
                    Globals.MainWindow.Enable(Globals.MainWindow.sendcapans);
                    Globals.MainWindow.UpdateLogs("Received Image!");
                    Globals.Images.CreateImage(packet);
                    break;
                case (ushort)0xA323:
                    Globals.MainWindow.SetText(Globals.MainWindow.capans, "");
                    if (Globals.MainWindow.captcha.Dispatcher.CheckAccess())
                    {
                        Globals.MainWindow.captcha.Source = null;
                    }
                    else
                    {
                        Globals.MainWindow.captcha.Dispatcher.Invoke((Action)delegate()
                        {
                            Globals.MainWindow.captcha.Source = null;
                        });
                    }
                    Images.CaptchaResult(packet);
                    break;                                  
            }
        }
    }
}
