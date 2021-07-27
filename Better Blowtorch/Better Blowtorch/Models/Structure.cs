using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin132.Models
{
    public class Structure
    {
        public ushort StructureID { get; set; }
        public ushort RepairItemID { get; set; }
        public int Amount { get; set; }
        public float HealAmount { get; set; }
        public bool HasBypassPermission { get; set; }
        public string BypassPermission { get; set; }

        public Structure(ushort structureID, ushort repairItemID, int amount, float healAmount, bool hasBypassPermission, string bypassPermission)
        {
            StructureID = structureID;
            RepairItemID = repairItemID;
            Amount = amount;
            HealAmount = healAmount;
            HasBypassPermission = hasBypassPermission;
            BypassPermission = bypassPermission;
        }

        public Structure()
        {

        }
    }
}
