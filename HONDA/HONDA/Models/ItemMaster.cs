using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HONDA.Models
{
    public class ItemMaster
    {

        public string Item_No { get; set; }
        public string Description { get; set; }
        public string Blocked { get; set; }
        public string Type { get; set; }
        public string BUOM { get; set; }
        public string ItemCategory { get; set; }
        public string SUOM { get; set; }
        public string ItemTrackingCode { get; set; }
        public string ExpirationCalculation { get; set; }
        public string ItemType { get; set; }
        public string Inventory { get; set; }
    }
}