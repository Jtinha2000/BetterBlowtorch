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
    public class Config : IRocketPluginConfiguration
    {
        public bool AllowRepairNonConfiguratedStructures { get; set; }
        public List<Models.Structure> Structure = new List<Models.Structure>();
        public List<Models.Vehicles> Vehicle = new List<Models.Vehicles>();
        public string GlobalBypassPermission { get; set; }
        public List<ushort> StructureIdBypass = new List<ushort>();
        public List<ushort> VehicleIdBypass = new List<ushort>();
        public void LoadDefaults()
        {
            VehicleIdBypass.Add(103);
            GlobalBypassPermission = "Sim";
            AllowRepairNonConfiguratedStructures = true;
            StructureIdBypass.Add(1090);
            Vehicle.Add(new Models.Vehicles(109, 39, 1, 1, true, "sim"));
            Structure.Add(new Models.Structure(30, 41, 1, 1, true, "Nao"));
        }
    }
}
