using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HONDA.Models
{
    public class CustomerModel
    {
        public string Customer_No { get; set; }
        public string Description { get; set; }
        public string Balance { get; set; }
        public string Blocked { get; set; }
        public string Sale_Person_Code { get; set; }
        public string FarmerLocation { get; set; }
        public string CustomerType { get; set; }
        public string CreditLimit { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Country_RegionCode { get; set; }
        public string PhoneNo { get; set; }
        public string Email_id { get; set; }
        public string LanguageCode { get; set; }
        public string VatRegistrationCode { get; set; }
        public string GLN { get; set; }
        public string GenBusPostingSetup { get; set; }
        public string VatBusPostingSetup { get; set; }
        public string CustomerPostingSetup { get; set; }
        public string CurrencyCode { get; set; }
        public string LocationCode { get; set; }
        public string RemainingCrLimit { get; set; }
    }
}