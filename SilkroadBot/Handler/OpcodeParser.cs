using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Threading;

namespace LeoBot
{
    //packet parser - opcode base
    class OpcodeParser
    {
        public static void Handler(Packet packet)
        {
            switch (packet.Opcode)
            {
                #region Login
                case (ushort)0xA103:
                    Servers.AgentRespond(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_CHARACTERLISTING:
                    Character.CharacterList(packet);
                    break;
                #endregion
                
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_CONFIRMSPAWN:
                    Character.CharID(packet);
                    Character.CharData(Character.CharPacket);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_CHARDATA:
                    Character.CharPacket = packet;
                    break;               
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_CHARACTERINFO:
                    Character.CharacterInfo(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_STUFFUPDATE:
                    Character.UpdateInfo(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_LVLUP:
                    Character.LevelUp(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_EXPSPUPDATE:
                    Character.ExpSpUpdate(packet);
                    break;

                #region Pets
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_PETINFO:
                    PetsData.PetInfo(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_PETSTATS:
                    PetsData.PetStats(packet);
                    break;
                #endregion

                #region Spawns
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_SINGLESPAWN:
                    Spawn.SingleSpawn(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_SINGLEDESPAWN:
                    Spawn.SingleDeSpawn(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_GROUPSPAWNB:
                    GroupSpawns.GroupeSpawnB(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_GROUPESPAWN:
                    GroupSpawns.Manager(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_GROUPSPAWNEND:
                    GroupSpawns.GroupSpawned();
                    break;
                #endregion

                #region Training
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_OBJECTDIE:
                    MonsterControl.MonsterAction(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_NPCSELECT:
                    MonsterControl.NPCSelect(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_NPCDESELECT:
                    MonsterControl.NPCDeselect(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_OBJECTSELECT:
                    Training.Selected(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_OBJECTACTION:
                    MonsterControl.Refresh(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_HPMPUPDATE:
                    HPMPPacket.HPMPUpdate(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_HORSEACTION:
                    PetsData.HorseAction(packet);
                    break;
                #endregion

                #region Storage
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_STORAGEITEMS:
                    StorageControl.ParseStorageItems(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_STORAGEGOLD:
                    StorageControl.StorageGold(packet);
                    break;
                #endregion

                #region Movement
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_MOVE:
                    Movement.Move(packet);
                    break;
               case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_STUCK:
                    Movement.Stuck(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_SPEEDUPDATE:
                    Character.SpeedUpdate(packet);
                    break;
                #endregion

                #region Items
                /*case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_ITEMFIXED:
                    InventoryControl.ItemFixed(packet);
                    break;*/
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_DURABILITYCHANGE:
                    InventoryControl.Durability(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_INVENTORYMOVEMENT:
                    InventoryControl.Inventory_Update1(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_INVENTORYUSE:
                    InventoryControl.Inventory_Update(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_ITEMMODIFY:
                    InventoryControl.Inventory_Update2(packet);
                    break;
                case(ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_ITEMRELEASE:
                    PickupControl.Itemfree(packet);
                    break;
                #endregion

                #region Skills
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_SKILLADD:
                    Skills.SkillAdd(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_SKILLUPDATE:
                    Skills.SkillUpdate(packet);
                    break;
                #endregion

                #region Buffs
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_BUFFINFO:
                    Buffas.BuffAdd(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_BUFFDELL:
                    Buffas.BuffDell(packet);
                    break;
                #endregion

                #region Party
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_ACCEPTPARTY:
                    Party.AcceptParty(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_PARTYREMOVE:
                    Party.ReformParty(packet);
                    break;
                #endregion
            }
        }
    }
}
