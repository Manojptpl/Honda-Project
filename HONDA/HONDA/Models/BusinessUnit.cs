using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HONDA.Models
{
    public class BusinessUnit
    {
        public int ID { get; set; }
        public string CompanyName  { get; set; }
        public string CompanyID { get; set; }
        public string CompanyShortName { get; set; }
        public string IncorporationDate { get; set; }
        public string PANNumber { get; set; }
        public string TANNumber { get; set; }
        public string Address { get; set; }      
        public string CountryID { get; set; }
        public string Countryname { get; set; }
        public string ZoneID { get; set; }
        public string ResionName { get; set; }
        public string State_ProvinceID { get; set; }
        public string Dzongkhagname { get; set; }
        public string CityID { get; set; }
        public string Locationname { get; set; }
        public string PostalCode { get; set; }
        public string CompanyEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactPerson { get; set; }
        public string ContactEmail { get; set; }
        public string MobileNumber { get; set; }
        public string Website { get; set; }
        public string FinancialYearFrom { get; set; }
        public string FinancialYearTo { get; set; }
        public string CalendarYearFrom { get; set; }
        public string CalendarYearTo { get; set; }
         
    }
}