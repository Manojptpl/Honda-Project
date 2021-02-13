using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HONDA.Models
{
    public class StoreLocationMapping
    {
        public int ID { get; set; }

        public int locationid { get; set; }
        public int storeid { get; set; }
        public string storename { get; set; }
        public string BUName { get; set; }
        public string StoreLocation { get; set; }

        public int bunameid { get; set; }
        public double storelatitude { get; set; }
        public double storelongitude { get; set; }
        public double radius { get; set; }
        public string status { get; set; }
     //   public string effectivestartdate { get; set; }
      //  public string effectiveenddate { get; set; }
      //  public string creationdate { get; set; }
        public string createdby { get; set; }
     //   public string lastupdatedate { get; set; }
        public string lastupdatedby { get; set; }
      //  public int lastupdatelogin { get; set; }
    }
}