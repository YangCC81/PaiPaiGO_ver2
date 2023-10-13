
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PaiPaiGO.Models;
using System.Diagnostics.Metrics;
using System.Drawing.Drawing2D;

namespace PaiPaiGo.Controllers
{
    public class HS_Get_TextController : Controller
    {

        private readonly PaiPaiGoContext _context;



        public HS_Get_TextController(PaiPaiGoContext context)
        {
            _context = context;
        }


        public IActionResult GetOrder_Pai(int id)
        {
            //int ID = 2023100801;
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
                TempData["rePaiAcceptTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                //if(PaiData.ImagePath != null)
                //{
                //    byte[] imageBytes = PaiData.ImagePath;
                //    string base64Image = Convert.ToBase64String(imageBytes);
                //    TempData["PaiImageData"] = base64Image;
                //    TempData.Keep();
                //}
                
            }
            return View();
        }

        public IActionResult GetOrder_Buy(int id)
        {
            //int ID = 2023100805;
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
                TempData["reBuyAcceptTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                //byte[] imageBytes = null;
                //TempData["BuyImageData"] = null;
                //if (BuyData.ImagePath != null)
                //{
                //    imageBytes = BuyData.ImagePath;
                //    string base64Image = Convert.ToBase64String(imageBytes);
                //    TempData["BuyImageData"] = base64Image;
                //    TempData.Keep();
                //}
            }
            return View();
        }


        private bool MissionExists(int id)
        {
            id = Convert.ToInt32(ViewBag.PID);
            return (_context.Missions?.Any(e => e.MissionId == id)).GetValueOrDefault();
        }


        public IActionResult Text_Pai(int id)
        {

            //int ID = 2023100801;
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
                TempData["rePaiAcceptTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                TempData["PaiImagePath"] = PaiData.ImagePath;
            }
            return View();
        }
        public IActionResult Text_Buy(int id)
        {
            //int ID = 2023100805;
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
                TempData["reBuyAcceptTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                TempData["BuyImagePath"] = BuyData.ImagePath;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetOrder_Pai(int MissionId, string AcceptMemberId, string MissionStatus, string AcceptTime)
        {
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
        {
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

    }
}
