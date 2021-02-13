using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HONDA.CommanFilter;
using HONDA.Database_Access_Layer;
using HONDA.CustomerCard;
using HONDA.PostCode;
using HONDA.CurrenciesList;
using HONDA.LanguageCode;
using HONDA.SalesPersonList;
using HONDA.VatBusinessPostingSetup;
using HONDA.Location_List;
using HONDA.GenBusinessPosting;
using HONDA.CustomerPostingGroupSetUp;
using HONDA.Models;
using HONDA.Item_List;
using System.Configuration;
using HONDA.Customer_List;

namespace HONDA.Controllers
{
    [SessionFilter]
    public class MasterController : Controller
    {
        db db_layer = new db();
        string Uname = ConfigurationManager.AppSettings["Uname"].ToString();
        string Pwd = ConfigurationManager.AppSettings["Pwd"].ToString();
        // GET: Customer
        public ActionResult Create()
        {
            try
            {

             
         //Post code
            PostCodePage_Service Pcode = new PostCodePage_Service();
            Pcode.Credentials = new System.Net.NetworkCredential(Uname, Pwd);
                List<PostCodePage_Filter> pclinef = new List<PostCodePage_Filter>();
            PostCodePage[] pclist = Pcode.ReadMultiple(pclinef.ToArray(), "", 1000);
            ViewBag.PostCode = new SelectList(pclist, "Code", "Code");
         //Currencies list
            CurrenciesListPage_Service Currencies_list  = new CurrenciesListPage_Service();
            Currencies_list.Credentials = new System.Net.NetworkCredential(Uname, Pwd);
                List<CurrenciesListPage_Filter> Cllinef = new List<CurrenciesListPage_Filter>();
            CurrenciesListPage[] cllist = Currencies_list.ReadMultiple(Cllinef.ToArray(), "", 1000);
            ViewBag.Currencies_Code = new SelectList(cllist, "Code", "Code");
         //Language code
            LanguagePage_Service Language_code = new LanguagePage_Service();
            Language_code.Credentials = new System.Net.NetworkCredential(Uname, Pwd);
                List<LanguagePage_Filter> lclinef = new List<LanguagePage_Filter>();
            LanguagePage[] lclist = Language_code.ReadMultiple(lclinef.ToArray(), "", 1000);
            ViewBag.Language_code = new SelectList(lclist, "Code", "Code");
         //Sales_Person list 
            Sales_Person_List_Page_Service Sales_PersonList = new Sales_Person_List_Page_Service();
            Sales_PersonList.Credentials = new System.Net.NetworkCredential(Uname, Pwd);
                List<Sales_Person_List_Page_Filter> sp_listf = new List<Sales_Person_List_Page_Filter>();
            Sales_Person_List_Page[] sp_list = Sales_PersonList.ReadMultiple(sp_listf.ToArray(), "", 1000);
            ViewBag.Sales_Person_Code = new SelectList(sp_list, "Code", "Name");
         //Vat Business Posting
            VatBusinessPostingGroup_Service VatBusPosting = new VatBusinessPostingGroup_Service();
            VatBusPosting.Credentials = new System.Net.NetworkCredential(Uname, Pwd);
                List<VatBusinessPostingGroup_Filter> vtbus_listf = new List<VatBusinessPostingGroup_Filter>();
            VatBusinessPostingGroup[] vtbuslist = VatBusPosting.ReadMultiple(vtbus_listf.ToArray(), "", 1000);
            ViewBag.VatBusinessPosting = new SelectList(vtbuslist, "Code", "Description");
         //Gen Bus Posting
            GenBusinessPostingGroup_Service GenBusPosting = new GenBusinessPostingGroup_Service();
            GenBusPosting.Credentials = new System.Net.NetworkCredential(Uname, Pwd);
                List<GenBusinessPostingGroup_Filter> l_listf = new List<GenBusinessPostingGroup_Filter>();
            GenBusinessPostingGroup[] llist = GenBusPosting.ReadMultiple(l_listf.ToArray(), "", 1000);
            ViewBag.GenBusinessPosting = new SelectList(llist, "Code", "Description");
         //Customer Posting 
            CustomerPostingGroup_Service cust_post_group = new CustomerPostingGroup_Service();
            cust_post_group.Credentials = new System.Net.NetworkCredential(Uname, Pwd);
                List<CustomerPostingGroup_Filter> cp_linef = new List<CustomerPostingGroup_Filter>();
            CustomerPostingGroup[] cp_list = cust_post_group.ReadMultiple(cp_linef.ToArray(), "", 1000);
            ViewBag.CustomerPosting_SetUp = new SelectList(cp_list, "Code", "Description");
         //Location Code
            Location_List_Page_Service locationCode = new Location_List_Page_Service();
            locationCode.Credentials = new System.Net.NetworkCredential(Uname, Pwd);
                List<Location_List_Page_Filter> Lc_listf = new List<Location_List_Page_Filter>();
            Location_List_Page[] lc_list = locationCode.ReadMultiple(Lc_listf.ToArray(), "", 1000);
            ViewBag.LocationCode = new SelectList(lc_list, "Code", "Name");

            }
            catch
            {

            }

            return View();
            
        }


        public ActionResult CustomerApproval(){

            return View();
        }

        public ActionResult CustomerList()
        {
            return View();
        }

        public JsonResult GetCustomerList()
        {
            List<CustomerModel> CustomerList = new List<CustomerModel>();
            try
            {
                Customer_List_Page_Service co = new Customer_List_Page_Service();
                co.Credentials = new System.Net.NetworkCredential(Uname, Pwd);
                List<Customer_List_Page_Filter> custList1 = new List<Customer_List_Page_Filter>();
                Customer_List_Page_Filter custList = new Customer_List_Page_Filter();
                //custList.Field = Customer_List_Page_Fields.;
                //custList.Criteria = "HHP";
                //custList1.Add(custList);

                Customer_List_Page_Filter custList2 = new Customer_List_Page_Filter();
                //custList2.Field = Customer_List_Page_Fields.Global_Dimension_2_Code;
                //custList2.Criteria = "CHD ZKR";
                //custList1.Add(custList2);

                Customer_List_Page[] customer = co.ReadMultiple(custList1.ToArray(), "", 10000);

                foreach (var obj in customer)
                {
                    CustomerModel cm = new CustomerModel();
                    cm.Customer_No = obj.No;
                    cm.Description = obj.Name;
                    cm.Balance = obj.Balance_LCY.ToString();
                    cm.Blocked = obj.Blocked.ToString();
                    cm.Sale_Person_Code = Convert.ToString(obj.Salesperson_Code) != null ? Convert.ToString(obj.Salesperson_Code) : "";
                    cm.FarmerLocation = Convert.ToString(obj.Farmer_Location) != null ? Convert.ToString(obj.Farmer_Location) : "";
                    cm.CustomerType = Convert.ToString(obj.Customer_Type) != null ? Convert.ToString(obj.Customer_Type) : "";
                    cm.CreditLimit = Convert.ToString(obj.Credit_Limit_LCY) != null ? Convert.ToString(obj.Credit_Limit_LCY) : "";
                    cm.Address = Convert.ToString(obj.Address) != null ? Convert.ToString(obj.Address) : "";
                    cm.Address2 = Convert.ToString(obj.Address_2) != null ? Convert.ToString(obj.Address_2) : "";
                    cm.City = Convert.ToString(obj.City) != null ? Convert.ToString(obj.City) : "";
                    cm.PostCode = Convert.ToString(obj.Post_Code) != null ? Convert.ToString(obj.Post_Code) : "";
                    cm.Country_RegionCode = Convert.ToString(obj.Country_Region_Code) != null ? Convert.ToString(obj.Country_Region_Code) : "";
                    cm.PhoneNo = Convert.ToString(obj.Phone_No) != null ? Convert.ToString(obj.Phone_No) : "";
                    cm.Email_id = Convert.ToString(obj.E_Mail) != null ? Convert.ToString(obj.E_Mail) : "";
                    cm.LanguageCode = Convert.ToString(obj.Language_Code) != null ? Convert.ToString(obj.Language_Code) : "";
                    cm.VatRegistrationCode = Convert.ToString(obj.VAT_Registration_No) != null ? Convert.ToString(obj.VAT_Registration_No) : "";
                    cm.GLN = Convert.ToString(obj.GLN) != null ? Convert.ToString(obj.GLN) : "";
                    cm.GenBusPostingSetup = Convert.ToString(obj.Gen_Bus_Posting_Group) != null ? Convert.ToString(obj.Gen_Bus_Posting_Group) : "";
                    cm.VatBusPostingSetup = Convert.ToString(obj.VAT_Bus_Posting_Group) != null ? Convert.ToString(obj.VAT_Bus_Posting_Group) : "";
                    cm.CustomerPostingSetup = Convert.ToString(obj.Customer_Posting_Group) != null ? Convert.ToString(obj.Customer_Posting_Group) : "";
                    cm.CurrencyCode = Convert.ToString(obj.Currency_Code) != null ? Convert.ToString(obj.Currency_Code) : "";
                    cm.LocationCode = Convert.ToString(obj.Location_Code) != null ? Convert.ToString(obj.Location_Code) : "";
                    cm.RemainingCrLimit = Convert.ToString(obj.Remaining_Credit_Limit) != null ? Convert.ToString(obj.Remaining_Credit_Limit) : "";
                    CustomerList.Add(cm);
                }
            }
            catch { }
            return Json(CustomerList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemMaster()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetItemList()
        {
            List<ItemMaster> Itemlist = new List<ItemMaster>();
            try
            {
                Item_List_Page_Service io = new Item_List_Page_Service();
                io.Credentials = new System.Net.NetworkCredential(Uname, Pwd);
                List<Item_List_Page_Filter> ilist1 = new List<Item_List_Page_Filter>();
                Item_List_Page_Filter ilist = new Item_List_Page_Filter();
                ilist.Field = Item_List_Page_Fields.Item_Category_Code;
                ilist.Criteria = "FEED";
                ilist1.Add(ilist);

                Item_List_Page[] item = io.ReadMultiple(ilist1.ToArray(), "", 10000);

                foreach (var obj in item)
                {
                    ItemMaster im = new ItemMaster();
                    im.Item_No = obj.No;
                    im.Description = obj.Search_Description;
                    im.Blocked = obj.Blocked.ToString();
                    im.Type = obj.Type.ToString();
                    im.BUOM = obj.Base_Unit_of_Measure.ToString();
                    im.ItemCategory = obj.Item_Category_Code.ToString();
                    im.SUOM = obj.Sales_Unit_of_Measure.ToString();
                    im.ItemTrackingCode = "";
                    im.ExpirationCalculation = "";
                    im.ItemType = "";
                    im.Inventory = obj.Inventory.ToString();
                    Itemlist.Add(im);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(Itemlist, JsonRequestBehavior.AllowGet);
        }

    }
}