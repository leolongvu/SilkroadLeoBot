using System;
using System.Collections.Generic;
using System.Text;

namespace LeoBot
{
    class LoginServerOpcodes
    {
        public enum SERVER_OPCODES : ushort
        {
            HANDSHAKE = 0x5000,
            BLOWFISH = 0x8000,
            AGENT_SERVER = 0x2001,
            SERVER_LIST = 0xA101,
            PATCH_INFO = 0x600D,
            GAME_LOGIN_REPLY = 0xA103,
            LOGIN_REPLY = 0xA102
        }

        public enum CLIENT_OPCODES : ushort
        {
            HANDSHAKE = 0x5000,
            HANDSHAKE_OK = 0x9000,
            ACCEPT = 0x6100,
            PING = 0x2002,
            GAME_LOGIN = 0x6103,
            REQUEST_SERVER_LIST = 0x6101,
            LOGIN = 0x6102
        }

    }

    class WorldServerOpcodes
    {
        public enum SERVER_OPCODES
        {
            SERVER_ITEMMODIFYBEGIN = 0x3038, //OK
            SERVER_ITEMMODIFY = 0x3039, //OK
            SERVER_SINGLEDESPAWN = 0x3016, //OK
            SERVER_CHARDATA = 0x3013, //OK
            SERVER_MOVE = 0xB021, //OK
            SERVER_GROUPSPAWNB = 0x3017, //OK
            SERVER_GROUPSPAWNEND = 0x3018, //OK
            SERVER_SINGLESPAWN = 0x3015, //OK
            SERVER_GROUPESPAWN = 0x3019, //OK
            SERVER_CHARACTERINFO = 0x303D, //OK
            SERVER_SPEEDUPDATE = 0x30D0, //??
            SERVER_CHARACTERLISTING = 0xB007, //OK
            SERVER_OBJECTDIE = 0x30BF, //OK
            SERVER_ANGLECHANGE = 0xB024, //OK
            SERVER_BUFFINFO = 0xB0BD, // OK
            SERVER_SKILLADD = 0xB070, // OK
            SERVER_SKILLCASTED = 0xB071, //OK
            SERVER_BUFFDELL = 0xB072, // OK
            SERVER_INVENTORYMOVEMENT = 0xB034, //OK
            SERVER_EXPSPUPDATE = 0x3056, //OK
            SERVER_LVLUP = 0x3054, //OK
            SERVER_STUCK = 0xB023, //OK
            SERVER_HPMPUPDATE = 0x3057,  //OK
            SERVER_OBJECTSELECT = 0xB045, //OK
            SERVER_CHAT = 0x3026, //OK
            SERVER_CHATCOUNT = 0xB025, //OK
            SERVER_NPCSELECT = 0xB046, //OK
            SERVER_NPCDESELECT = 0xB04B, //OK
            SERVER_CONFIRMSPAWN = 0x3020, //OK
            SERVER_INVENTORYUSE = 0xB04C, //OK
            SERVER_ITEMRELEASE = 0x304D, //OK
            SERVER_STUFFUPDATE = 0x304E, //OK
            SERVER_PARTYINVITATION = 0x3080, //OK
            SERVER_STORAGEITEMS = 0x3049, //OK
            SERVER_STORAGEGOLD = 0x3047, //OK
            SERVER_STORAGEOK = 0x3048, //OK
            SERVER_PARTYMATCHING = 0xB06C, //OK
            SERVER_ITEMFIXED = 0xB03E, //OK
            SERVER_OBJECTACTION = 0xB074, //OK
            SERVER_DURABILITYCHANGE = 0x3052, //OK
            SERVER_SKILLUPDATE = 0xB0A1, //OK
            SERVER_GUILDINFO = 0x3101, //OK
            SERVER_PETINFO = 0x30C8, //OK
            SERVER_HORSEACTION = 0xB0CB, //OK
            SERVER_PETSTATS = 0x30C9,
            SERVER_PARTYREMOVE = 0x3864, //OK
            SERVER_ACCEPTPARTY = 0x706D //OK
        }

        public enum CLIENT_OPCODES
        {
            CLIENT_DISCONNECT = 0x7005,//OK
            CLIENT_TELEPORT = 0x705A, //OK
            CLIENT_GETSTORAGEITEMS = 0x703C, //OK
            CLIENT_NPCDESELECT = 0x704B, //OK
            CLIENT_CHARACTERLISTING = 0x7007, //OK
            CLIENT_INVENTORYUSE = 0x704C, //OK
            CLIENT_SELECTCHARACTER = 0x7001, //OK
            CLIENT_OBJECTACTION = 0x7074, //OK
            CLIENT_NPCSELECT = 0x7046, //OK
            CLIENT_OBJECTSELECT = 0x7045, //OK
            CLIENT_INVENTORYMOVEMENT = 0x7034, //OK
            CLIENT_CONFIRMSPAWN = 0x34C5, //OK
            CLIENT_REPAIR = 0x703E, //OK
            CLIENT_KILLHORSE = 0x70C6, //OK
            CLIENT_PETACTION = 0x70C5, //OK
            CLIENT_SITDOWN = 0x704F, //OK
            CLIENT_CHAT = 0x7025, //OK
            CLIENT_DROPGOLD = 0x7034, //OK
            CLIENT_CREATEPARTY = 0x7069, // OK
            CLIENT_PARTY = 0x3080, //OK
            CLIENT_PARTYLEAVE = 0x7061, // OK
            CLIENT_MOVEMENT = 0x7021, //OK
            CLIENT_JOINPARTY = 0x706D, //OK
            CLIENT_ACCEPTDEAD = 0x3053, //OK
            CLIENT_ZERK = 0x70A7, // OK
            CLIENT_ACCEPTPARTYREQUEST = 0x306E //OK
        }
    }
}