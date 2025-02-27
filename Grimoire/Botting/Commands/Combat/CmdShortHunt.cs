﻿using Grimoire.Botting.Commands.Map;
using Grimoire.Game;
using Grimoire.Tools;
using System.Threading.Tasks;

namespace Grimoire.Botting.Commands.Combat
{
    class CmdShortHunt : IBotCommand
    {
        public string Map { get; set; }
        public string Cell { get; set; }
        public string Pad { get; set; }
        public string Monster { get; set; }
        public string MonId { get; set; }
        public string ItemName { get; set; }
        public ItemType ItemType { get; set; }
        public string Quantity { get; set; }
        public bool IsGetDrops { get; set; } = false;
        public int AfterKills { get; set; } = 1;
        public string KillPriority { get; set; } = "";
        public bool AntiCounter { get; set; } = false;
        public string QuestId { get; set; }
        public int DelayAfterKill { get; set; } = 500;
        public bool BlankFirst { get; set; }

        public async Task Execute(IBotEngine instance)
        {
            if (ItemType == ItemType.Items)
            {
                if (Player.Inventory.ContainsItem(ItemName, Quantity)) return;
            }
            else
            {
                if (Player.TempInventory.ContainsItem(ItemName, Quantity)) return;
            }

            if (!Player.Map.Equals(Map.Split('-')[0]))
            {
                CmdJoin join = new CmdJoin
                {
                    Map = this.Map,
                    Cell = this.Cell,
                    Pad = this.Pad
                };

                if (BlankFirst)
                {
                    string[] safeCell = ClientConfig.GetValue(ClientConfig.C_SAFE_CELL).Split(',');
                    Player.MoveToCell(safeCell[0], safeCell[1]);
                    await instance.WaitUntil(() => Player.CurrentState != Player.State.InCombat);
                    await Task.Delay(1000);
                }

                await join.Execute(instance);
            }
            else
            {
                Player.MoveToCell(Cell, Pad);
            }

            await Task.Delay(1000);

            CmdKillFor killFor = new CmdKillFor
            {
                Monster = this.Monster,
                ItemName = this.ItemName,
                ItemType = this.ItemType,
                Quantity = this.Quantity,
                IsGetDrops = this.IsGetDrops,
                AfterKills = this.AfterKills,
                QuestId = this.QuestId,
                DelayAfterKill = this.DelayAfterKill,
                KillPriority = this.KillPriority,
                AntiCounter = this.AntiCounter
            };

            await killFor.Execute(instance);
        }

        public override string ToString()
        {
            string itemType = ItemType == ItemType.Items ? "Items" : "Temps";
            return $"Hunt {itemType} {Quantity}x {ItemName}";
        }

    }
}
