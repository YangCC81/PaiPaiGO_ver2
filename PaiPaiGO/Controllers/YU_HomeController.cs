using Microsoft.AspNetCore.Mvc;

namespace PaiPaiGO.Controllers {
    public class YU_HomeController : Controller {
        public IActionResult Index() {
            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            return View();
        }
    }
}
