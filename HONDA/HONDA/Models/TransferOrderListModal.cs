using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HONDA.Models
{
    public class TransferOrderListModal
    {
        public string No { get; set; }
        public string Transfer_from_Code { get; set; }
        public string Transfer_to_Code { get; set; }
        public string In_Transit_Code { get; set; }
        public string Status { get; set; }
        public string Direct_Transfer { get; set; }
        public string Shortcut_Dimension_Code { get; set; }
        public string Shipment_Date { get; set; }
        public string Shipping_Advice { get; set; }
        public string Receipt_Date { get; set; }

    }
}