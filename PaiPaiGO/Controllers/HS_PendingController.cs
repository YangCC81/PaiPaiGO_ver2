using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PaiPaiGo.Controllers;
using PaiPaiGO.Models;
using System;
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
using static PaiPaiGO.Models.PendingVal;

namespace PaiPaiGO.Controllers
{
    public class HS_PendingController : Controller
    {

        private readonly PaiPaiGoContext _context;

        public HS_PendingController(PaiPaiGoContext context)
        {
            _context = context;
        }
        #region 排隊頁面輸入傳遞
        [HttpGet]
        public IActionResult PendingOrder_Pai()
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            return View();
        }

        [HttpPost]
        public IActionResult PendingOrder_Pai(PendingVal.PaiVal model)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            // 使用 TempData 儲存表單數據
            TempData["Orderclass"] = model.Orderclass;
            TempData["QueueLabel"] = model.QueueLabel;
            TempData["PaiName"] = model.PaiName;
            TempData["Amount"] = model.Amount;
            TempData["City"] = model.City;
            TempData["Location"] = model.Location;
            TempData["Postcode"] = model.Postcode;
            TempData["Address"] = model.Address;
            TempData["QueueDate"] = model.QueueTime;
            TempData["Deadline"] = model.Deadline.ToString();
            TempData["TaskContent"] = model.TaskContent;
            TempData["TotalAmount"] = (int.Parse(model.Amount) + 50).ToString();

            byte[]? imageBytes = null;

            if (!string.IsNullOrEmpty(model.Image) && model.Image.Contains(","))
            {
                imageBytes = Convert.FromBase64String(model.Image.Split(",")[1]);
                HttpContext.Session.Set("Image", imageBytes);
            }



            string QueueTimeStr = model.QueueTime.ToString();
            DateTime QueueTime;

            if (DateTime.TryParse(QueueTimeStr, out QueueTime))
            {
                string time24Hour = QueueTime.TimeOfDay.ToString(@"hh\:mm");
                TempData["QueueTime"] = time24Hour;
            }
            string DeadlineStr = model.Deadline.ToString();
            DateTime DeadlineTime;

            if (DateTime.TryParse(DeadlineStr, out DeadlineTime))
            {
                string time24Hour = DeadlineTime.TimeOfDay.ToString(@"hh\:mm");
                TempData["DeadlineTime"] = time24Hour;
            }
            TempData.Keep();

            return RedirectToAction("CheckOrder_Pai");
        }
      
        public IActionResult CheckOrder_Pai()
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            // 使用 ViewBag 讀取 TempData
            ViewBag.Orderclass = TempData["Orderclass"] as string;
            ViewBag.QueueLabel = TempData["QueueLabel"] as string;
            ViewBag.PaiName = TempData["PaiName"] as string;
            ViewBag.Amount = TempData["Amount"] as string;
            ViewBag.City = TempData["City"] as string;
            ViewBag.Location = TempData["Location"] as string;
            ViewBag.Address = TempData["Address"];
            //ViewBag.QueueTime = TempData["QueueDate"];
            ViewBag.DeadlineDate = TempData["DeadlineDate"];
            // ViewBag.Deadline = TempData["Deadline"];
            ViewBag.TaskContent = TempData["TaskContent"] as string;
            ViewBag.TotalAmount = TempData["TotalAmount"] as string;
          
            byte[] storedImageBytes;
            HttpContext.Session.TryGetValue("Image", out storedImageBytes);
            if (storedImageBytes != null)
            {
                ViewBag.ImageData = Convert.ToBase64String(storedImageBytes);
            }

            // 將base64String 傳遞到View中
            ViewBag.ImageBytes = storedImageBytes;

            TempData.Keep();

            // 轉換和填充數據
            int maxMissionId = _context.Missions.Max(m => m.MissionId);
            int newMissionId = maxMissionId + 1;
            ViewBag.MissionId = newMissionId;

            // 特殊處理：將 "排隊" 和 "購買" 轉換為 1 和 2
            int category = ViewBag.Orderclass == "排隊" ? 1 : (ViewBag.Orderclass == "購買" ? 2 : 0);
            ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", category);
            TempData["Category"] = category; 
            Console.WriteLine(TempData["Category"]);

            return View();
        }
        #endregion


        #region 購買頁面輸入傳遞
        [HttpGet]
        public IActionResult PendingOrder_Buy()
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            return View();
        }

        [HttpPost]
        public IActionResult PendingOrder_Buy(PendingVal.BuyVal model)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            // 在這裡，您可以對 model 進行處理，例如保存到數據庫或進行其他業務邏輯

            // 使用 TempData 儲存表單數據
            TempData["BuyOrderclass"] = model.BuyOrderclass;
            TempData["BuyLabel"] = model.BuyLabel;
            TempData["BuyName"] = model.BuyName;
            TempData["BuyLocation"] = model.BuyLocation;
            TempData["BuyAmount"] = model.BuyAmount;
            TempData["BuyDelivery"] = model.BuyDelivery;
            TempData["BuyDeadline"] = model.BuyDeadline;
            TempData["BuyCity"] = model.BuyCity;
            TempData["BuyDistrict"] = model.BuyDistrict;
            TempData["BuyPostcode"] = model.BuyPostcode;
            TempData["BuyAddress"] = model.BuyAddress;
            TempData["BuyDeliveryMethod"] = model.BuyDeliveryMethod;
            TempData["BuyTaskContent"] = model.BuyTaskContent;
            TempData["BuyTotalAmount"] = (int.Parse(model.BuyAmount) + 50).ToString();
            byte[]? BuyimageBytes = null;

            if (!string.IsNullOrEmpty(model.BuyImage) && model.BuyImage.Contains(","))
            {
                BuyimageBytes = Convert.FromBase64String(model.BuyImage.Split(",")[1]);
                HttpContext.Session.Set("BuyImage", BuyimageBytes);
            }

            string BuyDeliveryStr = model.BuyDelivery.ToString();
            DateTime BuyDeliverytime;

            if (DateTime.TryParse(BuyDeliveryStr, out BuyDeliverytime))
            {
                string time24Hour = BuyDeliverytime.TimeOfDay.ToString(@"hh\:mm");
                TempData["BuyDeliveryTime"] = time24Hour;
            }
            string BuyDeadlineStr = model.BuyDeadline.ToString();
            DateTime BuyDeadlineTime;

            if (DateTime.TryParse(BuyDeadlineStr, out BuyDeadlineTime))
            {
                string time24Hour = BuyDeadlineTime.TimeOfDay.ToString(@"hh\:mm");
                TempData["BuyDeadlineTime"] = time24Hour;
            }

            TempData.Keep();

            // 保存完成後重定向到其他視圖或控制器操作
            return RedirectToAction("CheckOrder_Buy");
        }

        public IActionResult CheckOrder_Buy()
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            // 使用 ViewBag 讀取 TempData
            ViewBag.Orderclass = TempData["BuyOrderclass"] as string;
            ViewBag.BuyLabel = TempData["BuyLabel"] as string;
            ViewBag.BuyName = TempData["BuyName"] as string;
            ViewBag.BuyLocation = TempData["BuyLocation"] as string;
            ViewBag.BuyAmount = TempData["BuyAmount"] as string;
            //ViewBag.BuyDelivery = TempData["BuyDelivery"];
            //ViewBag.BuyDeadline = TempData["BuyDeadline"];
            ViewBag.BuyCity = TempData["BuyCity"] as string;
            ViewBag.BuyDistrict = TempData["BuyDistrict"] as string;
            ViewBag.BuyAddress = TempData["BuyAddress"] as string;
            ViewBag.BuyDeliveryMethod = TempData["BuyDeliveryMethod"] as string;
            ViewBag.BuyTaskContent = TempData["BuyTaskContent"] as string;
            ViewBag.BuyTotalAmount = TempData["BuyTotalAmount"] as string;

            byte[] BuystoredImageBytes;
            HttpContext.Session.TryGetValue("BuyImage", out BuystoredImageBytes);
            if (BuystoredImageBytes != null)
            {
                ViewBag.BuyImageData = Convert.ToBase64String(BuystoredImageBytes);
            }

            // 將base64String 傳遞到View中
            ViewBag.BuyImageBytes = BuystoredImageBytes;

            // 保留 TempData 以便後續使用
            TempData.Keep();


            // 轉換和填充數據
            int maxMissionId = _context.Missions.Max(m => m.MissionId);
            int newMissionId = maxMissionId + 1;
            ViewBag.MissionId = newMissionId;

            // 特殊處理：將 "排隊" 和 "購買" 轉換為 1 和 2
            //int category = ViewBag.Orderclass == "排隊" ? 1 : (ViewBag.Orderclass == "購買" ? 2 : 0);
            //ViewBag.Category = category;
            int category = ViewBag.Orderclass == "排隊" ? 1 : (ViewBag.Orderclass == "購買" ? 2 : 0);
            ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", category);
            TempData["BuyCategory"] = category; // 如果你還想在其他地方使用這個值


            return View();
        }
        #endregion

        #region 新增至SQL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PaiToDatabase([Bind("MissionId,Category,Tags,OrderMemberId,AcceptMemberId,MissionName,MissionAmount,Postcode,FormattedMissionAmount,LocationCity,LocationDistrict,Address,DeliveryDate,DeliveryTime,DeadlineDate,DeadlineTime,MissionDescription,DeliveryMethod,ExecutionLocation,MissionStatus,OrderTime,AcceptTime,ImagePath")] Mission mission)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            byte[] storedImageBytes;
            HttpContext.Session.TryGetValue("Image", out storedImageBytes);
            if (storedImageBytes != null)
            {
                ViewBag.Img = storedImageBytes;
            }

            // 獲取今天日期的字符串形式（yyyyMMdd）
            string dateStr = DateTime.Now.ToString("yyyyMMdd");
            // 從資料庫中獲取最大的任務ID
            int maxMissionId = _context.Missions.Max(m => m.MissionId);
            string maxMissionIdStr = maxMissionId.ToString();
            int newMissionNumber;
            string newMissionDateStr;
            if (maxMissionIdStr.StartsWith(dateStr))
            {
                // 如果最大任務ID的日期與今天相同，取出數字部分，加1
                newMissionDateStr = dateStr;
                string numberPart = maxMissionIdStr.Substring(8);  
                newMissionNumber = int.Parse(numberPart) + 1;
            }
            else
            {
                // 如果最大任務ID的日期與今天不同，則用今天的日期和數字1作為新的任務ID的一部分
                newMissionDateStr = dateStr;
                newMissionNumber = 1;
            }
            // 生成新的任務ID
            int newMissionId = int.Parse(newMissionDateStr + newMissionNumber.ToString("D2")); 
            ViewBag.MissionId = newMissionId;
            mission.MissionId = newMissionId;


            int selectedCategoryId = mission.Category;

            var existingCategory = await _context.Categories.FindAsync(selectedCategoryId);
            if (existingCategory != null)
            {
                mission.CategoryNavigation = existingCategory;
                // 移除與 'CategoryNavigation' 相關的 ModelState 錯誤（如果有）
                ModelState.Remove("CategoryNavigation");
            }
            else
            {
                Console.WriteLine("Category with ID 1 not found.");
            }

            // 重新檢查 ModelState 是否有效
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            // 如果現在 ModelState 是有效的，則進行保存
            if (ModelState.IsValid)
            {
                _context.Add(mission);
                await _context.SaveChangesAsync();
                return RedirectToAction("YH_CasePage", "YH_CasePages");
            }

            ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", mission.Category);
            return View(mission);
        }
        #endregion
    }
}

