using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HONDA.CommanFilter;
using HONDA.TransferOrderList;
using HONDA.Database_Access_Layer;
using System.Configuration;
using HONDA.Models;
namespace HONDA.Controllers
{
    [SessionFilter]
    public class InventoryController : Controller
    {
        db db_layer = new db();
        string Uname = ConfigurationManager.AppSettings["Uname"].ToString();
        string Pwd = ConfigurationManager.AppSettings["Pwd"].ToString();
        // GET: Inventory
        public ActionResult TransferOrder()
        {
            return View();
        }

        public ActionResult TransferOrderList()
        {

          

            return View();

        }
      
    
         public JsonResult GetTransferOrderList()
        {
            List<TransferOrderListModal> TranOrderList = new List<TransferOrderListModal>();

            try
            {
                TransferOrderListPage_Service Tran_orderList = new TransferOrderListPage_Service();
                Tran_orderList.Credentials = new System.Net.NetworkCredential(Uname, Pwd);
                List<TransferOrderListPage_Filter> tran_orderlistf = new List<TransferOrderListPage_Filter>();
                TransferOrderListPage[] tran_list = Tran_orderList.ReadMultiple(tran_orderlistf.ToArray(), "", 1000);
                foreach (var item in tran_list)
                {
                    TransferOrderListModal Tran_order = new TransferOrderListModal();
                    Tran_order.No = item.No;
                    Tran_order.Receipt_Date = item.Receipt_Date.ToString(); ;
                    Tran_order.Shipment_Date = item.Shipment_Date.ToString();
                    Tran_order.Shipping_Advice = item.Shipping_Advice.ToString();
                    Tran_order.Shortcut_Dimension_Code = item.Shortcut_Dimension_1_Code;
                    Tran_order.Status = item.Status.ToString();

                    Tran_order.Transfer_from_Code = item.Transfer_from_Code;
                    Tran_order.Transfer_to_Code = item.Transfer_to_Code;
                    Tran_order.Direct_Transfer = item.Direct_Transfer.ToString();
                    Tran_order.In_Transit_Code = item.In_Transit_Code;
                    TranOrderList.Add(Tran_order);
                }
            }
            catch
            {

          
            }
            return Json(TranOrderList, JsonRequestBehavior.AllowGet);

        }
    }
}