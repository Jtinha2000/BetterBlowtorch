using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Provider;
using SDG.Framework;
using Rocket.Core;
using SDG.Unturned;
using Rocket.API.Collections;
using Rocket.Unturned.Events;
using System.Collections;
using UnityEngine;
using Plugin132.Models;

namespace Plugin132
{
    public class Main : RocketPlugin<Config>
    {
        public static Main Instance { get; set; }
        protected override void Load()
        {
            Instance = this;
            StructureManager.OnRepairRequested += new RepairStructureRequestHandler(this.StructureManager_OnRepairRequested);
            BarricadeManager.OnRepairRequested += BarricadeManager_OnRepairRequested;
            VehicleManager.onRepairVehicleRequested += new RepairVehicleRequestHandler(this.VehicleManager_OnRepairRequested);
        }
        private void VehicleManager_OnRepairRequested(Steamworks.CSteamID player, InteractableVehicle interactableVehicle, ref ushort TotalPendingHealing, ref bool ShouldAllow)
        {
            int Index = 0;
            if (Main.Instance.Configuration.Instance.Vehicle.Exists(x => x.VehicleID == interactableVehicle.id))
            {
                Index = Main.Instance.Configuration.Instance.Vehicle.FindIndex(x => x.VehicleID == interactableVehicle.id);
            }
            else
            {
                if (Main.Instance.Configuration.Instance.AllowRepairNonConfiguratedStructures == true)
                {
                    ShouldAllow = true;
                    return;
                }
                UnturnedChat.Say(player, Main.Instance.Translate("StructureNotRegister"));
                ShouldAllow = false;
            }
            UnturnedPlayer Player = UnturnedPlayer.FromCSteamID(player);
            if (Player.HasPermission(Main.Instance.Configuration.Instance.GlobalBypassPermission) || Main.Instance.Configuration.Instance.VehicleIdBypass.Contains(interactableVehicle.id))
            {
                UnturnedChat.Say(player, Main.Instance.Translate("BypassPermission"));
                TotalPendingHealing = (ushort)Main.Instance.Configuration.Instance.Structure[Index].HealAmount;
                ShouldAllow = true;
                return;
            }
            else if (Main.Instance.Configuration.Instance.Structure[Index].HasBypassPermission == true && Player.HasPermission(Main.Instance.Configuration.Instance.Structure[Index].BypassPermission))
            {
                UnturnedChat.Say(player, Main.Instance.Translate("BypassPermission"));
                TotalPendingHealing = (ushort)Main.Instance.Configuration.Instance.Structure[Index].HealAmount;
                ShouldAllow = true;
                return;
            }
            var a = Player.Inventory.search(Main.Instance.Configuration.Instance.Structure[Index].RepairItemID, true, true);
            if (a.Count == 0 || a == null)
            {
                UnturnedChat.Say(player, Main.Instance.Translate("Donthasitem"));
                ShouldAllow = false;
                return;
            }
            if (a.Count < Main.Instance.Configuration.Instance.Structure[Index].Amount)
            {
                UnturnedChat.Say(player, Main.Instance.Translate("DonthasAmount"));
                return;
            }
            for (int timer = 0; timer < Main.Instance.Configuration.Instance.Structure[Index].Amount; timer++)
            {
                Player.Inventory.removeItem(a[timer].page, Player.Inventory.getIndex(a[timer].page, a[timer].jar.x, a[timer].jar.y));
            }
            TotalPendingHealing = (ushort)Main.Instance.Configuration.Instance.Structure[Index].HealAmount;
            ShouldAllow = true;
            return;
        }
        private void StructureManager_OnRepairRequested(Steamworks.CSteamID player, Transform Transformer, ref float TotalPendingHealing, ref bool ShouldAllow)
        {
            int Index = 0;
            if (!StructureManager.tryGetInfo(Transformer, out byte X, out byte Y, out ushort index1, out StructureRegion structureRegion))
            {
                return;
            }
            var StructureID = structureRegion.structures[index1].structure.asset.id;
            if (Main.Instance.Configuration.Instance.Structure.Exists(x => x.StructureID == StructureID))
            {
                Index = Main.Instance.Configuration.Instance.Structure.FindIndex(x => x.StructureID == StructureID);
            }
            else
            {
                if (Main.Instance.Configuration.Instance.AllowRepairNonConfiguratedStructures == true)
                {
                    ShouldAllow = true;
                    return;
                }
                UnturnedChat.Say(player, Main.Instance.Translate("StructureNotRegister"));
                ShouldAllow = false;
            }
            UnturnedPlayer Player = UnturnedPlayer.FromCSteamID(player);
            if (Player.HasPermission(Main.Instance.Configuration.Instance.GlobalBypassPermission) || Main.Instance.Configuration.Instance.StructureIdBypass.Contains(StructureID))
            {
                UnturnedChat.Say(player, Main.Instance.Translate("BypassPermission"));
                TotalPendingHealing = Main.Instance.Configuration.Instance.Structure[Index].HealAmount;
                ShouldAllow = true;
                return;
            }
            else if (Main.Instance.Configuration.Instance.Structure[Index].HasBypassPermission == true && Player.HasPermission(Main.Instance.Configuration.Instance.Structure[Index].BypassPermission))
            {
                UnturnedChat.Say(player, Main.Instance.Translate("BypassPermission"));
                TotalPendingHealing = Main.Instance.Configuration.Instance.Structure[Index].HealAmount;
                ShouldAllow = true;
                return;
            }
            var a = Player.Inventory.search(Main.Instance.Configuration.Instance.Structure[Index].RepairItemID, true, true);
            if (a.Count == 0 || a == null)
            {
                UnturnedChat.Say(player, Main.Instance.Translate("Donthasitem"));
                ShouldAllow = false;
                return;
            }
            if (a.Count < Main.Instance.Configuration.Instance.Structure[Index].Amount)
            {
                UnturnedChat.Say(player, Main.Instance.Translate("DonthasAmount"));
                return;
            }
            for (int timer = 0; timer < Main.Instance.Configuration.Instance.Structure[Index].Amount; timer++)
            {
                Player.Inventory.removeItem(a[timer].page, Player.Inventory.getIndex(a[timer].page, a[timer].jar.x, a[timer].jar.y));
            }
            TotalPendingHealing = Main.Instance.Configuration.Instance.Structure[Index].HealAmount;
            ShouldAllow = true;
            return;
        }
        private void BarricadeManager_OnRepairRequested(Steamworks.CSteamID player, Transform Transformer, ref float TotalPendingHealing, ref bool ShouldAllow)
        {
            int Index = 0;
            if (!BarricadeManager.tryGetInfo(Transformer, out byte X, out byte Y, out ushort plant, out ushort index1, out BarricadeRegion structureRegion))
            {
                return;
            }
            var StructureID = structureRegion.barricades[index1].barricade.asset.id;
            if (Main.Instance.Configuration.Instance.Structure.Exists(x => x.StructureID == StructureID))
            {
                Index = Main.Instance.Configuration.Instance.Structure.FindIndex(x => x.StructureID == StructureID);
            }
            else
            {
                if (Main.Instance.Configuration.Instance.AllowRepairNonConfiguratedStructures == true)
                {
                    ShouldAllow = true;
                    return;
                }
                UnturnedChat.Say(player, Main.Instance.Translate("StructureNotRegister"));
                ShouldAllow = false;
            }
            UnturnedPlayer Player = UnturnedPlayer.FromCSteamID(player);
            if (Player.HasPermission(Main.Instance.Configuration.Instance.GlobalBypassPermission) || Main.Instance.Configuration.Instance.StructureIdBypass.Contains(StructureID))
            {
                UnturnedChat.Say(player, Main.Instance.Translate("BypassPermission"));
                TotalPendingHealing = Main.Instance.Configuration.Instance.Structure[Index].HealAmount;
                ShouldAllow = true;
                return;
            }
            else if (Main.Instance.Configuration.Instance.Structure[Index].HasBypassPermission == true && Player.HasPermission(Main.Instance.Configuration.Instance.Structure[Index].BypassPermission))
            {
                UnturnedChat.Say(player, Main.Instance.Translate("BypassPermission"));
                TotalPendingHealing = Main.Instance.Configuration.Instance.Structure[Index].HealAmount;
                ShouldAllow = true;
                return;
            }
            var a = Player.Inventory.search(Main.Instance.Configuration.Instance.Structure[Index].RepairItemID, true, true);
            if (a.Count == 0 || a == null)
            {
                UnturnedChat.Say(player, Main.Instance.Translate("Donthasitem"));
                ShouldAllow = false;
                return;
            }
            if (a.Count < Main.Instance.Configuration.Instance.Structure[Index].Amount)
            {
                UnturnedChat.Say(player, Main.Instance.Translate("DonthasAmount"));
                return;
            }
            for (int timer = 0; timer < Main.Instance.Configuration.Instance.Structure[Index].Amount; timer++)
            {
                Player.Inventory.removeItem(a[timer].page, Player.Inventory.getIndex(a[timer].page, a[timer].jar.x, a[timer].jar.y));
            }
            TotalPendingHealing = Main.Instance.Configuration.Instance.Structure[Index].HealAmount;
            ShouldAllow = true;
            return;
        }

        public override TranslationList DefaultTranslations => new TranslationList
        {
            {"BypassPermission", "You have the bypass permission!" },
            {"StructureNotRegister", "You cant repair that!" },
            {"Donthasitem", "You don't has the nescessary item" },
            {"DonthasAmount", "You don't has the nescessary amount of those item" },
        };
        protected override void Unload()
        {
            BarricadeManager.OnRepairRequested -= BarricadeManager_OnRepairRequested;
        }
    }
}
