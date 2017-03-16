using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Xml;
using System.Timers;

namespace LeoBot
{
    class Proxy
    {
        public static Thread gw;
        public static Thread ag;

        public static TcpListener gw_local_server;
        public static TcpClient gw_local_client;
        public static TcpClient gw_remote_client;
        public static Security gw_local_security;
        public static Security gw_remote_security;
        public static NetworkStream gw_local_stream;
        public static NetworkStream gw_remote_stream;
        public static TransferBuffer gw_remote_recv_buffer;
        public static List<Packet> gw_remote_recv_packets;
        public static List<KeyValuePair<TransferBuffer, Packet>> gw_remote_send_buffers;
        public static TransferBuffer gw_local_recv_buffer;
        public static List<Packet> gw_local_recv_packets;
        public static List<KeyValuePair<TransferBuffer, Packet>> gw_local_send_buffers;

        public static TcpListener ag_local_server;
        public static TcpClient ag_local_client;
        public static TcpClient ag_remote_client;
        public static Security ag_local_security;
        public static Security ag_remote_security;
        public static NetworkStream ag_local_stream;
        public static NetworkStream ag_remote_stream;
        public static TransferBuffer ag_remote_recv_buffer;
        public static List<Packet> ag_remote_recv_packets;
        public static List<KeyValuePair<TransferBuffer, Packet>> ag_remote_send_buffers;
        public static TransferBuffer ag_local_recv_buffer;
        public static List<Packet> ag_local_recv_packets;
        public static List<KeyValuePair<TransferBuffer, Packet>> ag_local_send_buffers;

        public static string xfer_remote_ip;
        public static int xfer_remote_port;

        public static object exit_lock = new object();
        public static bool should_exit = false;
        public static uint agid = 0;

        public static bool connection = false;

        public static int port = 0;        

        static void GatewayRemoteThread()
        {
            {
                while (true)
                {
                    lock (exit_lock)
                    {
                        if (should_exit)
                        {
                            break;
                        }
                    }

                    if (gw_remote_stream.DataAvailable)
                    {
                        gw_remote_recv_buffer.Offset = 0;
                        gw_remote_recv_buffer.Size = gw_remote_stream.Read(gw_remote_recv_buffer.Buffer, 0, gw_remote_recv_buffer.Buffer.Length);
                        gw_remote_security.Recv(gw_remote_recv_buffer);
                    }

                    gw_remote_recv_packets = gw_remote_security.TransferIncoming();
                    if (gw_remote_recv_packets != null)
                    {
                        foreach (Packet packet in gw_remote_recv_packets)
                        {
                            byte[] packet_bytes = packet.GetBytes();
                            //Globals.MainWindow.UpdateAnalyzerLine(String.Format("[S->P][{0:X4}][{1} bytes]{2}{3}{4}{5}{6}", packet.Opcode, packet_bytes.Length, packet.Encrypted ? "[Encrypted]" : "", packet.Massive ? "[Massive]" : "", Environment.NewLine, Utility.HexDump(packet_bytes), Environment.NewLine));

                            //WritePacket(packet, packet_bytes, "S->C");

                            // Do not pass through these packets.
                            if (packet.Opcode == 0x5000 || packet.Opcode == 0x9000)
                            {
                                continue;
                            }

                            LoginHandler.Handler(packet);

                            if (packet.Opcode == 0xA102)
                            {
                                byte result = packet.ReadUInt8();
                                if (result == 1)
                                {
                                    Globals.MainWindow.UnEnable(Globals.MainWindow.username);
                                    Globals.MainWindow.UnEnable(Globals.MainWindow.password);
                                    Globals.MainWindow.UnEnable(Globals.MainWindow.autologin);
                                    Globals.MainWindow.UnEnable(Globals.MainWindow.serverlist);

                                    uint id = packet.ReadUInt32();
                                    agid = id;
                                    string ip = packet.ReadAscii();
                                    ushort port = packet.ReadUInt16();

                                    xfer_remote_ip = ip;
                                    xfer_remote_port = port;

                                    Packet new_packet = new Packet(0xA102, true);
                                    new_packet.WriteUInt8(result);
                                    new_packet.WriteUInt32(id);
                                    new_packet.WriteAscii("127.0.0.1");
                                    new_packet.WriteUInt16(port);

                                    gw_local_security.Send(new_packet);
                                    ag = new Thread(AgentThread);
                                    ag.Start();
                                    ag.IsBackground = true;
                                    ag.Join();                                    

                                    continue;
                                }
                                else
                                {
                                    byte subcode = packet.ReadUInt8();
                                    switch (subcode)
                                    {
                                        case 1:
                                            uint maxTry = packet.ReadUInt32();
                                            uint curTry = packet.ReadUInt32();
                                            Globals.MainWindow.UpdateLogs(string.Format("Wrong ID/PW. You have {0} attempts left!", (maxTry - curTry)));
                                            Globals.MainWindow.UnCheck(Globals.MainWindow.autologin);
                                            break;
                                        case 2:
                                            if (packet.ReadUInt8() == 1)
                                            {
                                                string reason = packet.ReadAscii();
                                                string date = packet.ReadUInt16() + "." + packet.ReadUInt16() + "." + packet.ReadUInt16() + " " + packet.ReadUInt16() + ":" + packet.ReadUInt16();
                                                Globals.MainWindow.UpdateLogs("Your account has been banned! Reason: " + reason + ". Till: " + date);
                                            }
                                            Globals.MainWindow.UnCheck(Globals.MainWindow.autologin);
                                            break;
                                        case 3: // User Already Connected
                                            Globals.MainWindow.UpdateLogs("User already connected!");
                                            System.Threading.Thread.Sleep(5000);
                                            Globals.MainWindow.UnCheck(Globals.MainWindow.autologin);
                                            break;
                                        case 5: // Server Full
                                            Globals.MainWindow.UpdateLogs("The server is full!");
                                            Packet NewPacket = new Packet((ushort)LoginServerOpcodes.CLIENT_OPCODES.LOGIN, true);
                                            NewPacket.WriteUInt8(22);
                                            NewPacket.WriteAscii(Globals.MainWindow.ReadText(Globals.MainWindow.username));
                                            NewPacket.WriteAscii(Globals.MainWindow.ReadText(Globals.MainWindow.password));
                                            NewPacket.WriteUInt16(Data.ServerID[Data.ServerName.IndexOf(Globals.MainWindow.ReadText1(Globals.MainWindow.serverlist))]);
                                            System.Threading.Thread.Sleep(500);
                                            Proxy.gw_remote_security.Send(NewPacket);
                                            break;
                                        default:
                                            Globals.MainWindow.UpdateLogs("Unknown login error code: " + subcode);
                                            Globals.MainWindow.UnCheck(Globals.MainWindow.autologin);
                                            System.Threading.Thread.Sleep(5000);
                                            break;
                                    }
                                }                               
                            }
                            gw_local_security.Send(packet);
                        }
                    }

                    gw_remote_send_buffers = gw_remote_security.TransferOutgoing();
                    if (gw_remote_send_buffers != null)
                    {
                        foreach (var kvp in gw_remote_send_buffers)
                        {
                            //Packet packet = kvp.Value;
                            TransferBuffer buffer = kvp.Key;
                            //byte[] packet_bytes = packet.GetBytes();
                            //Globals.MainWindow.UpdateAnalyzerLine(String.Format("[P->S][{0:X4}][{1} bytes]{2}{3}{4}{5}{6}", packet.Opcode, packet_bytes.Length, packet.Encrypted ? "[Encrypted]" : "", packet.Massive ? "[Massive]" : "", Environment.NewLine, Utility.HexDump(packet_bytes), Environment.NewLine));
                            //WritePacket(packet, packet_bytes, "C->S");
                            gw_remote_stream.Write(buffer.Buffer, 0, buffer.Size);
                        }
                    }
                    Thread.Sleep(1);
                }
            }
        }

        static void GatewayLocalThread()
        {
            try
            {
                while (true)
                {
                    lock (exit_lock)
                    {
                        if (should_exit)
                        {
                            break;
                        }
                    }

                    if (gw_local_stream.DataAvailable)
                    {
                        gw_local_recv_buffer.Offset = 0;
                        gw_local_recv_buffer.Size = gw_local_stream.Read(gw_local_recv_buffer.Buffer, 0, gw_local_recv_buffer.Buffer.Length);
                        gw_local_security.Recv(gw_local_recv_buffer);
                    }

                    gw_local_recv_packets = gw_local_security.TransferIncoming();
                    if (gw_local_recv_packets != null)
                    {
                        foreach (Packet packet in gw_local_recv_packets)
                        {
                            //byte[] packet_bytes = packet.GetBytes();
                            //Globals.MainWindow.UpdateAnalyzerLine(String.Format("[C->P][{0:X4}][{1} bytes]{2}{3}{4}{5}{6}", packet.Opcode, packet_bytes.Length, packet.Encrypted ? "[Encrypted]" : "", packet.Massive ? "[Massive]" : "", Environment.NewLine, Utility.HexDump(packet_bytes), Environment.NewLine));

                            // Do not pass through these packets.
                            if (packet.Opcode == 0x5000 || packet.Opcode == 0x9000 || packet.Opcode == 0x2001)
                            {
                                continue;
                            }                           

                            gw_remote_security.Send(packet);
                        }
                    }

                    gw_local_send_buffers = gw_local_security.TransferOutgoing();
                    if (gw_local_send_buffers != null)
                    {
                        foreach (var kvp in gw_local_send_buffers)
                        {
                            ///Packet packet = kvp.Value;
                            TransferBuffer buffer = kvp.Key;
                            //byte[] packet_bytes = packet.GetBytes();
                            //Console.WriteLine("[P->C][{0:X4}][{1} bytes]{2}{3}{4}{5}{6}", packet.Opcode, packet_bytes.Length, packet.Encrypted ? "[Encrypted]" : "", packet.Massive ? "[Massive]" : "", Environment.NewLine, Utility.HexDump(packet_bytes), Environment.NewLine);
                            gw_local_stream.Write(buffer.Buffer, 0, buffer.Size);
                        }
                    }
                    Thread.Sleep(1);
                }
            }
            catch  { }                            
        }

        public static void GatewayThread()
        {
            try
            {                
                gw_local_security = new Security();
                gw_local_security.GenerateSecurity(true, true, true);

                gw_remote_security = new Security();

                gw_remote_recv_buffer = new TransferBuffer(8192, 0, 0);
                gw_local_recv_buffer = new TransferBuffer(8192, 0, 0);

                gw_local_server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                gw_local_server.Start();

                Globals.MainWindow.UpdateLogs("Waiting for a connection . . . ");

                gw_local_client = gw_local_server.AcceptTcpClient();
                gw_remote_client = new TcpClient();

                Globals.MainWindow.UpdateLogs("A connection has been made!");

                gw_local_server.Stop();

                Globals.MainWindow.UpdateLogs("Connecting to Silkroad . . . ");

                gw_remote_client.Connect("gw.cdtl.ongame.vn", 15779);

                //Globals.MainWindow.UpdateLogs("The connection has been made!");
                connection = true;

                gw_local_stream = gw_local_client.GetStream();
                gw_remote_stream = gw_remote_client.GetStream();

                should_exit = false;

                Thread remote_thread = new Thread(GatewayRemoteThread);
                remote_thread.Start();
                remote_thread.IsBackground = true;
                Thread local_thread = new Thread(GatewayLocalThread);
                local_thread.Start();
                local_thread.IsBackground = true;
                remote_thread.Join();
                local_thread.Join();                
            }
            catch
            {
                Globals.MainWindow.UpdateLogs("Unable to connect to the server! Maybe the server is repaired?");
                gw = new Thread(GatewayThread);
                gw.Start();
                gw.IsBackground = true;
            }
        }

        static void AgentRemoteThread()
        {
            try
            {
                while (true)
                {
                    lock (exit_lock)
                    {
                        if (should_exit)
                        {
                            break;
                        }
                    }

                    if (ag_remote_stream.DataAvailable)
                    {
                        ag_remote_recv_buffer.Offset = 0;
                        ag_remote_recv_buffer.Size = ag_remote_stream.Read(ag_remote_recv_buffer.Buffer, 0, ag_remote_recv_buffer.Buffer.Length);
                        ag_remote_security.Recv(ag_remote_recv_buffer);
                    }

                    ag_remote_recv_packets = ag_remote_security.TransferIncoming();
                    if (ag_remote_recv_packets != null)
                    {
                        foreach (Packet packet in ag_remote_recv_packets)
                        {
                            //byte[] packet_bytes = packet.GetBytes();
                            //Globals.MainWindow1.UpdateLogs(String.Format("[S->P][{0:X4}][{1} bytes]{2}{3}{4}{5}{6}", packet.Opcode, packet_bytes.Length, packet.Encrypted ? "[Encrypted]" : "", packet.Massive ? "[Massive]" : "", Environment.NewLine, Utility.HexDump(packet_bytes), Environment.NewLine));
                            //WritePacket(packet, packet_bytes, "S->C");
                            // Do not pass through these packets.
                            if (packet.Opcode == 0x5000 || packet.Opcode == 0x9000)
                            {
                                continue;
                            }

                            OpcodeParser.Handler(packet);

                            if (packet.Opcode != ((ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_INVENTORYMOVEMENT) && packet.Opcode != (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_NPCSELECT)
                                ag_local_security.Send(packet);
                        }
                    }

                    ag_remote_send_buffers = ag_remote_security.TransferOutgoing();
                    if (ag_remote_send_buffers != null)
                    {
                        foreach (var kvp in ag_remote_send_buffers)
                        {
                            //Packet packet = kvp.Value;
                            TransferBuffer buffer = kvp.Key;
                            //byte[] packet_bytes = packet.GetBytes();
                            //if (packet.Opcode != (0x2002))                            
                            //Globals.MainWindow1.UpdateLogs(String.Format("[P->S][{0:X4}][{1} bytes]{2}{3}{4}{5}{6}", packet.Opcode, packet_bytes.Length, packet.Encrypted ? "[Encrypted]" : "", packet.Massive ? "[Massive]" : "", Environment.NewLine, Utility.HexDump(packet_bytes), Environment.NewLine));                           
                            ag_remote_stream.Write(buffer.Buffer, 0, buffer.Size);                         
                            //WritePacket(packet, packet_bytes, "C->S");                           
                        }
                    }
                    Thread.Sleep(1);
                }
            }
            catch { }
        }

        static void AgentLocalThread()
        {
            try
            {
                while (true)
                {
                    lock (exit_lock)
                    {
                        if (should_exit)
                        {
                            break;
                        }
                    }

                    if (ag_local_stream.DataAvailable)
                    {
                        ag_local_recv_buffer.Offset = 0;
                        ag_local_recv_buffer.Size = ag_local_stream.Read(ag_local_recv_buffer.Buffer, 0, ag_local_recv_buffer.Buffer.Length);
                        ag_local_security.Recv(ag_local_recv_buffer);
                    }

                    ag_local_recv_packets = ag_local_security.TransferIncoming();
                    if (ag_local_recv_packets != null)
                    {
                        foreach (Packet packet in ag_local_recv_packets)
                        {
                            //byte[] packet_bytes = packet.GetBytes();
                            //if (packet.Opcode != (0x2002))
                            //MainWindow.DebugWindow.UpdateAnalyzerLine(String.Format("[C->P][{0:X4}][{1} bytes]{2}{3}{4}{5}{6}", packet.Opcode, packet_bytes.Length, packet.Encrypted ? "[Encrypted]" : "", packet.Massive ? "[Massive]" : "", Environment.NewLine, Utility.HexDump(packet_bytes), Environment.NewLine));

                            // Do not pass through these packets.
                            if (packet.Opcode == 0x5000 || packet.Opcode == 0x9000 || packet.Opcode == 0x2001)
                            {
                                continue;
                            }

                            if (packet.Opcode == 0x6103 && packet.GetBytes().Length != 33)
                            {
                                Packet NewPacket = new Packet((ushort)LoginServerOpcodes.CLIENT_OPCODES.GAME_LOGIN, true);
                                NewPacket.WriteUInt32(agid);
                                NewPacket.WriteAscii(Globals.MainWindow.ReadText(Globals.MainWindow.username).ToLower());
                                NewPacket.WriteAscii(Globals.MainWindow.ReadText(Globals.MainWindow.password).ToLower());
                                NewPacket.WriteUInt8(22);
                                NewPacket.WriteUInt8(0);
                                NewPacket.WriteUInt8(0);
                                NewPacket.WriteUInt8(0);
                                NewPacket.WriteUInt8(0);
                                NewPacket.WriteUInt8(0);
                                NewPacket.WriteUInt8(0);
                                ag_remote_security.Send(NewPacket);

                                continue;
                            }

                            ag_remote_security.Send(packet);
                        }
                    }

                    ag_local_send_buffers = ag_local_security.TransferOutgoing();
                    if (ag_local_send_buffers != null)
                    {
                        foreach (var kvp in ag_local_send_buffers)
                        {
                            //Packet packet = kvp.Value;
                            TransferBuffer buffer = kvp.Key;
                            //byte[] packet_bytes = packet.GetBytes();
                            //Globals.MainWindow.UpdateAnalyzerLine(String.Format("[P->C][{0:X4}][{1} bytes]{2}{3}{4}{5}{6}", packet.Opcode, packet_bytes.Length, packet.Encrypted ? "[Encrypted]" : "", packet.Massive ? "[Massive]" : "", Environment.NewLine, Utility.HexDump(packet_bytes), Environment.NewLine));
                            ag_local_stream.Write(buffer.Buffer, 0, buffer.Size);
                        }
                    }
                    Thread.Sleep(1);
                }
            }
            catch 
            {
                ExitThread();
            }
        }

        public static void AgentThread()
        {
            {
                ag_local_security = new Security();
                ag_local_security.GenerateSecurity(true, true, true);

                ag_remote_security = new Security();

                ag_remote_recv_buffer = new TransferBuffer(8192, 0, 0);
                ag_local_recv_buffer = new TransferBuffer(8192, 0, 0);

                ag_local_server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                ag_local_server.Start();

                //Globals.MainWindow.UpdateLogs("Waiting for a connection . . . ");

                ag_local_client = ag_local_server.AcceptTcpClient();
                ag_remote_client = new TcpClient();

                //Globals.MainWindow.UpdateLogs("A connection has been made!");

                ag_local_server.Stop();

                Globals.MainWindow.UpdateLogs(String.Format("Connecting to {0}:{1}", xfer_remote_ip, xfer_remote_port));

                ag_remote_client.Connect(xfer_remote_ip, xfer_remote_port);

                Globals.MainWindow.UpdateLogs("The connection has been made!");

                ag_local_stream = ag_local_client.GetStream();
                ag_remote_stream = ag_remote_client.GetStream();

                Thread remote_thread = new Thread(AgentRemoteThread);
                remote_thread.Start();
                remote_thread.IsBackground = true;
                Thread local_thread = new Thread(AgentLocalThread);
                local_thread.Start();
                local_thread.IsBackground = true;
                remote_thread.Join();
                local_thread.Join();
            }
        }

        public static void ExitThread()
        {
            lock (exit_lock)
            {
                should_exit = true;
            }

            gw = new Thread(GatewayThread);
            gw.Start();
            gw.IsBackground = true;

            Globals.MainWindow.Enable(Globals.MainWindow.autologin);
            Globals.MainWindow.Enable(Globals.MainWindow.username);
            Globals.MainWindow.Enable(Globals.MainWindow.password);
            Globals.MainWindow.Enable(Globals.MainWindow.serverlist);
            Globals.MainWindow.Enable(Globals.MainWindow.charname);
            Globals.MainWindow.Enable(Globals.MainWindow.charnameselect);
            Globals.MainWindow.Enable(Globals.MainWindow.capans);
            Globals.MainWindow.Enable(Globals.MainWindow.sendcapans);
            Globals.MainWindow.Enable(Globals.MainWindow.selectcharbutton);
            Globals.MainWindow.Enable(Globals.MainWindow.charlist);

            Globals.MainWindow.UnEnable(Globals.MainWindow.startbot);
        }
    }
}
