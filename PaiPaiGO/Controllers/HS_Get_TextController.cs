
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PaiPaiGO.Models;
using System.Diagnostics.Metrics;
using System.Drawing.Drawing2D;

namespace PaiPaiGO.Controllers
{
    public class HS_Get_TextController : Controller
    {

        private readonly PaiPaiGoContext _context;



        public HS_Get_TextController(PaiPaiGoContext context)
        {
            _context = context;
        }

        #region 接單頁面資料帶入
        public IActionResult GetOrder_Pai(int id)
        {
            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            ViewBag.PID = id;
            var PaiData = (from x in _context.Missions
                           where x.MissionId == id
                           select x).FirstOrDefault();



            if (PaiData != null)
            {
                // 使用 TempData 和 ViewBag 儲存資料
                TempData["PaiMissionId"] = PaiData.MissionId;
                TempData["PaiCategory"] = PaiData.Category;
                TempData["PaiTags"] = PaiData.Tags;
                TempData["PaiOrderMemberId"] = PaiData.OrderMemberId;
                TempData["PaiAcceptMemberId"] = PaiData.AcceptMemberId;
                TempData["PaiMissionName"] = PaiData.MissionName;
                TempData["PaiMissionAmount"] = Convert.ToInt32(PaiData.MissionAmount);
                TempData["PaiPostcode"] = PaiData.Postcode;
                TempData["PaiFormattedMissionAmount"] = PaiData.FormattedMissionAmount;
                TempData["PaiLocationCity"] = PaiData.LocationCity;
                TempData["PaiLocationDistrict"] = PaiData.LocationDistrict;
                TempData["PaiAddress"] = PaiData.Address;
                TempData["PaiDeliveryDate"] = PaiData.DeliveryDate;
                TempData["PaiDeliveryTime"] = PaiData.DeliveryTime.ToString();
                TempData["PaiDeadlineDate"] = PaiData.DeadlineDate;
                TempData["PaiDeadlineTime"] = PaiData.DeadlineTime.ToString();
                TempData["PaiMissionDescription"] = PaiData.MissionDescription;
                TempData["PaiDeliveryMethod"] = PaiData.DeliveryMethod;
                TempData["PaiExecutionLocation"] = PaiData.ExecutionLocation;
                TempData["PaiMissionStatus"] = PaiData.MissionStatus;
                TempData["PaiOrderTime"] = PaiData.OrderTime;
                TempData["PaiAcceptTime"] = PaiData.AcceptTime;
                TempData["rePaiAcceptTime"] = DateTime.Now.ToString("yyyy/M/d HH:mm");
                //圖片
                byte[] imageBytes = null;
                if (PaiData.ImagePath != null)
                {
                    imageBytes = PaiData.ImagePath;
                    HttpContext.Session.Set("Image", imageBytes);
                }
                byte[] storedImageBytes;
                HttpContext.Session.TryGetValue("Image", out storedImageBytes);
                if (storedImageBytes != null)
                {
                    string base64Image = Convert.ToBase64String(storedImageBytes);
                    ViewBag.ImageData = "data:image/png;base64," + base64Image;
                }

            }
            return View();
        }

        public IActionResult GetOrder_Buy(int id)
        {
            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");

            TempData["BID"] = id;
            var BuyData = (from x in _context.Missions
                           where x.MissionId == id
                           select x).FirstOrDefault();

            if (BuyData != null)
            {
                // 使用 TempData 和 ViewBag 儲存資料
                TempData["BuyMissionId"] = BuyData.MissionId;
                TempData["BuyCategory"] = BuyData.Category;
                TempData["BuyTags"] = BuyData.Tags;
                TempData["BuyOrderMemberId"] = BuyData.OrderMemberId;
                TempData["BuyAcceptMemberId"] = BuyData.AcceptMemberId;
                TempData["BuyMissionName"] = BuyData.MissionName;
                TempData["BuyMissionAmount"] = Convert.ToInt32(BuyData.MissionAmount);
                TempData["BuyPostcode"] = BuyData.Postcode;
                TempData["BuyFormattedMissionAmount"] = BuyData.FormattedMissionAmount;
                TempData["BuyLocationCity"] = BuyData.LocationCity;
                TempData["BuyLocationDistrict"] = BuyData.LocationDistrict;
                TempData["BuyAddress"] = BuyData.Address;
                TempData["BuyDeliveryDate"] = BuyData.DeliveryDate;
                TempData["BuyDeliveryTime"] = BuyData.DeliveryTime.ToString();
                TempData["BuyDeadlineDate"] = BuyData.DeadlineDate;
                TempData["BuyDeadlineTime"] = BuyData.DeadlineTime.ToString();
                TempData["BuyMissionDescription"] = BuyData.MissionDescription;
                TempData["BuyDeliveryMethod"] = BuyData.DeliveryMethod;
                TempData["BuyExecutionLocation"] = BuyData.ExecutionLocation;
                TempData["BuyMissionStatus"] = BuyData.MissionStatus;
                TempData["BuyOrderTime"] = BuyData.OrderTime;
                TempData["BuyAcceptTime"] = BuyData.AcceptTime;
                TempData["reBuyAcceptTime"] = DateTime.Now.ToString("yyyy/M/d HH:mm");
                //圖片
                byte[] BuyimageBytes = null;
                if (BuyData.ImagePath != null)
                {
                    BuyimageBytes = BuyData.ImagePath;
                    HttpContext.Session.Set("BuyImage", BuyimageBytes);
                }
                byte[] BuystoredImageBytes;
                HttpContext.Session.TryGetValue("BuyImage", out BuystoredImageBytes);
                if (BuystoredImageBytes != null)
                {
                    string base64Image = Convert.ToBase64String(BuystoredImageBytes);
                    ViewBag.BuyImageData = "data:image/png;base64," + base64Image;
                }

            }
            return View();
        }
        #endregion

        private bool MissionExists(int id)
        {
            id = Convert.ToInt32(ViewBag.PID);
            return (_context.Missions?.Any(e => e.MissionId == id)).GetValueOrDefault();
        }

        #region 詳細頁面資料帶入
        public IActionResult Text_Pai(int id)
        {
            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            ViewBag.PID = id;
            var PaiData = (from x in _context.Missions
                           where x.MissionId == id
                           select x).FirstOrDefault();


            if (PaiData != null)
            {
                // 使用 TempData 和 ViewBag 儲存資料
                TempData["PaiMissionId"] = PaiData.MissionId;
                TempData["PaiCategory"] = PaiData.Category;
                TempData["PaiTags"] = PaiData.Tags;
                TempData["PaiOrderMemberId"] = PaiData.OrderMemberId;
                TempData["PaiAcceptMemberId"] = PaiData.AcceptMemberId;
                TempData["PaiMissionName"] = PaiData.MissionName;
                TempData["PaiMissionAmount"] = Convert.ToInt32(PaiData.MissionAmount);
                TempData["PaiPostcode"] = PaiData.Postcode;
                TempData["PaiFormattedMissionAmount"] = PaiData.FormattedMissionAmount;
                TempData["PaiLocationCity"] = PaiData.LocationCity;
                TempData["PaiLocationDistrict"] = PaiData.LocationDistrict;
                TempData["PaiAddress"] = PaiData.Address;
                TempData["PaiDeliveryDate"] = PaiData.DeliveryDate;
                TempData["PaiDeliveryTime"] = PaiData.DeliveryTime.ToString();
                TempData["PaiDeadlineDate"] = PaiData.DeadlineDate;
                TempData["PaiDeadlineTime"] = PaiData.DeadlineTime.ToString();
                TempData["PaiMissionDescription"] = PaiData.MissionDescription;
                TempData["PaiDeliveryMethod"] = PaiData.DeliveryMethod;
                TempData["PaiExecutionLocation"] = PaiData.ExecutionLocation;
                TempData["PaiMissionStatus"] = PaiData.MissionStatus;
                TempData["PaiOrderTime"] = PaiData.OrderTime;
                TempData["PaiAcceptTime"] = PaiData.AcceptTime;
                TempData["rePaiAcceptTime"] = DateTime.Now.ToString("yyyy/M/d HH:mm");
                //圖片
                byte[] imageBytes = null;
                if (PaiData.ImagePath != null) {
                    imageBytes = PaiData.ImagePath;
                    HttpContext.Session.Set("Image", imageBytes);
                }
                byte[] storedImageBytes;
                HttpContext.Session.TryGetValue("Image", out storedImageBytes);
                if (storedImageBytes != null) {
                    string base64Image = Convert.ToBase64String(storedImageBytes);
                    ViewBag.ImageData = "data:image/png;base64," + base64Image;
                }
            }
            return View();
        }
        public IActionResult Text_Buy(int id)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            TempData["BID"] = id;
            var BuyData = (from x in _context.Missions
                           where x.MissionId == id
                           select x).FirstOrDefault();

            if (BuyData != null)
            {
                // 使用 TempData 和 ViewBag 儲存資料
                TempData["BuyMissionId"] = BuyData.MissionId;
                TempData["BuyCategory"] = BuyData.Category;
                TempData["BuyTags"] = BuyData.Tags;
                TempData["BuyOrderMemberId"] = BuyData.OrderMemberId;
                TempData["BuyAcceptMemberId"] = BuyData.AcceptMemberId;
                TempData["BuyMissionName"] = BuyData.MissionName;
                TempData["BuyMissionAmount"] = Convert.ToInt32(BuyData.MissionAmount);
                TempData["BuyPostcode"] = BuyData.Postcode;
                TempData["BuyFormattedMissionAmount"] = BuyData.FormattedMissionAmount;
                TempData["BuyLocationCity"] = BuyData.LocationCity;
                TempData["BuyLocationDistrict"] = BuyData.LocationDistrict;
                TempData["BuyAddress"] = BuyData.Address;
                TempData["BuyDeliveryDate"] = BuyData.DeliveryDate;
                TempData["BuyDeliveryTime"] = BuyData.DeliveryTime.ToString();
                TempData["BuyDeadlineDate"] = BuyData.DeadlineDate;
                TempData["BuyDeadlineTime"] = BuyData.DeadlineTime.ToString();
                TempData["BuyMissionDescription"] = BuyData.MissionDescription;
                TempData["BuyDeliveryMethod"] = BuyData.DeliveryMethod;
                TempData["BuyExecutionLocation"] = BuyData.ExecutionLocation;
                TempData["BuyMissionStatus"] = BuyData.MissionStatus;
                TempData["BuyOrderTime"] = BuyData.OrderTime;
                TempData["BuyAcceptTime"] = BuyData.AcceptTime;
                TempData["reBuyAcceptTime"] = DateTime.Now.ToString("yyyy/M/d HH:mm");
                //圖片
                byte[] BuyimageBytes = null;
                if (BuyData.ImagePath != null) {
                    BuyimageBytes = BuyData.ImagePath;
                    HttpContext.Session.Set("BuyImage", BuyimageBytes);
                }
                byte[] BuystoredImageBytes;
                HttpContext.Session.TryGetValue("BuyImage", out BuystoredImageBytes);
                if (BuystoredImageBytes != null) {
                    string base64Image = Convert.ToBase64String(BuystoredImageBytes);
                    ViewBag.BuyImageData = "data:image/png;base64," + base64Image;
                }
            }
            return View();
        }
        #endregion

        #region 狀態資料更新
        [HttpPost]
        public async Task<IActionResult> GetOrder_Pai(int MissionId, string AcceptMemberId, string MissionStatus, string AcceptTime)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            var mission = await _context.Missions.FindAsync(MissionId);
            if (mission == null)
            {
                return Json(new { success = false });
            }

            mission.AcceptMemberId = AcceptMemberId;
            mission.MissionStatus = MissionStatus;
            mission.AcceptTime = AcceptTime;

            try
            {
                _context.Update(mission);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetOrder_Buy(int MissionId, string AcceptMemberId, string MissionStatus, string AcceptTime)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            var mission = await _context.Missions.FindAsync(MissionId);
            if (mission == null)
            {
                return Json(new { success = false });
            }

            mission.AcceptMemberId = AcceptMemberId;
            mission.MissionStatus = MissionStatus;
            mission.AcceptTime = AcceptTime;

            try
            {
                _context.Update(mission);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false });
            }
        }
        #endregion
    }
}
