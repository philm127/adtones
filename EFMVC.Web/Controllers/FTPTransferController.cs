using EFMVC.Data;
using EFMVC.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Controllers
{
    public class FTPTransferController : Controller
    {
        // GET: FTPTransfer
        public ActionResult Index()
        {
            try
            {
                EFMVCDataContex db = new EFMVCDataContex();
                var advertData = db.Adverts.Where(s => s.UploadedToMediaServer == false).Select(s => s.AdvertId).ToList();
                foreach (var item in advertData)
                {
                    var returnValue = AdTransfer.CopyAdToOpeartorServer(item);
                }
                ViewBag.Window = "True";
                ViewBag.success = "sucess";
            }
            catch(Exception ex)
            {
                ViewBag.success = ex.Message.ToString();
            }                    
            return View();
        }
    }
}