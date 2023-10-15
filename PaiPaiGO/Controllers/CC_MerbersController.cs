using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text;
using System.Net.Mail;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using NuGet.Common;
using PaiPaiGO.Models;

namespace paipaigo1005.Controllers {
    public class CC_MembersController : Controller {
        private readonly PaiPaiGoContext _context;
        //內存緩存
        private readonly IMemoryCache _memoryCache;
        public CC_MembersController(PaiPaiGoContext context, IMemoryCache memoryCache) {
            _context = context;
            _memoryCache = memoryCache;
        }

        #region 忘記密碼
        [HttpGet]
        public IActionResult ForgotPassword() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email) {
            var user = _context.Members
            .FirstOrDefault(x => x.MemberEmail == email && x.MemberStatus == "正常");

            if (user == null) {
                ViewBag.Status = "空的";
                return View("ForgotPassword");
            }
            else {
                if (user.MemberStatus == "正常") {
                    // 生成重置密码令牌
                    var resetToken = GenerateResetToken(user);

                    // 發送電子郵件
                    var emailSubject = "paipaigo重設密碼驗證郵件";
                    var emailMessage = $"請點擊連結驗證您的Email地址：<a href='{Url.Action("SetPassword", "CC_Members", new { memberId = user.MemberId, token = GenerateToken(user.MemberId) }, protocol: HttpContext.Request.Scheme)}'>點擊重新設定密碼</a>";

                    var emailService = new EmailService();
                    await emailService.SendEmailAsync(user.MemberEmail, emailSubject, emailMessage);
                    ViewBag.Status = "正常";
                    return View("ForgotPassword");
                }
                else {
                    ViewBag.Status = "異常";
                    return View("ForgotPassword");
                }
            }
        }
        //生成token
        private string GenerateResetToken(Member user) {
            var Resettoken = Guid.NewGuid().ToString();
            //存在緩存裡
            _memoryCache.Set(Resettoken, user.MemberId, TimeSpan.FromHours(1));
            _memoryCache.Set("Setpass", user.MemberId, TimeSpan.FromHours(1));
            return Resettoken;
        }
        [HttpGet]
        public IActionResult SetPassword() {
            //搜尋緩存中有沒有token
            if (_memoryCache.TryGetValue("Setpass", out string Setpass)) {
                //緩存移除token
                _memoryCache.Remove("Setpass");
                var query = _context.Members
                        .FirstOrDefault(x => x.MemberId == Setpass);
                if (query != null) {
                    ViewBag.ID = query.MemberId;
                    ViewBag.name = query.MemberName;
                    ViewBag.email = query.MemberEmail;
                    ViewBag.phone = query.MemberPhoneNumber.Substring(0, 10); ;
                    //ViewBag.BankCode = query.BankCode;
                    //ViewBag.BankNum = query.BankNum;
                    ViewBag.Postcode = query.MemberPostcode;
                    ViewBag.MemberCity = query.MemberCity;
                    ViewBag.Township = query.MemberTownship;
                    ViewBag.Address = query.MemberAddress;
                    ViewBag.Status = query.MemberStatus;
                }
                return View();
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword([Bind("MemberId,MemberName,MemberPhoneNumber,MemberPostcode,MemberCity,MemberTownship,MemberAddress,MemberEmail,MemberStatus,MemberPassword,BankCode,BankNum")] Member member) {
            if (ModelState.IsValid) {
                ViewBag.Set = "更改成功";
                _context.Update(member);
                await _context.SaveChangesAsync();
                return View("SetPassword");
            }
            return View("SetPassword");
        }
        #endregion

        #region 註冊會員
        public IActionResult Resgister() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resgister([Bind("MemberId,MemberName,MemberPhoneNumber,MemberPostcode,MemberCity,MemberTownship,MemberAddress,MemberEmail,MemberStatus,MemberPassword,Gearing")] Member member) {

            var maxId = _context.Members
                    .AsEnumerable() // 轉為内存查詢以使用LINQ to Objects
                    .Select(item => int.TryParse(item.MemberId, out int id) ? id : 0)
                    .Max();
            maxId++;
            string numstring = maxId.ToString();
            int numLength = numstring.Length;
            int zeronum = 3 - numLength;
            for (int i = 0; i < zeronum; i++) {
                numstring = "0" + numstring;
            }
            member.MemberId = numstring;

            if (ModelState.IsValid) {
                _context.Add(member);
                await _context.SaveChangesAsync();

                // 發送電子郵件
                var emailSubject = "paipaigo會員驗證郵件";
                var emailMessage = $"請點擊連結驗證您的Email地址：<a href='{Url.Action("ConfirmEmail", "CC_Members", new { memberId = member.MemberId, token = GenerateToken(member.MemberId) }, protocol: HttpContext.Request.Scheme)}'>驗證並至登入頁面</a>";
                var emailService = new EmailService();
                await emailService.SendEmailAsync(member.MemberEmail, emailSubject, emailMessage);
                ViewBag.RegistrationSuccess = "Success";
                return View("Resgister");
            }
            return View(member);
        }
        //Email驗證Action
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string memberId, string token) {
            if (string.IsNullOrEmpty(memberId) || string.IsNullOrEmpty(token)) {
                return View("Error");
            }

            var user = await _context.Members.FirstOrDefaultAsync(m => m.MemberId == memberId);

            if (user == null) {
                return View("Error");
            }

            // 使用 ConfirmEmailAsync 方法驗證token是否正確
            var tokenIsValid = await ConfirmEmailAsync(user, token);

            if (tokenIsValid) {
                return View("Login");
            }
            else {
                return View("Error");
            }
        }
        private async Task<bool> ConfirmEmailAsync(Member member, string token) {
            var tokenIsValid = false;
            //搜尋緩存中有沒有token
            if (_memoryCache.TryGetValue(token, out string userId)) {
                //緩存移除token
                _memoryCache.Remove(token);
                // 更新用户信息
                member.MemberStatus = "正常";
                _context.Update(member);
                await _context.SaveChangesAsync();
                tokenIsValid = true;
            }

            return tokenIsValid;
        }
        //生成token
        private string GenerateToken(string userId) {
            var token = Guid.NewGuid().ToString();
            //存在緩存裡
            _memoryCache.Set(token, userId, TimeSpan.FromHours(1));
            return token;
        }
        //發送電子郵件
        public class EmailService {
            public async Task SendEmailAsync(string email, string subject, string message) {
                // 使用 Google Mail Server 發信
                string GoogleID = "paipaigo001@gmail.com"; //Google 發信帳號
                string TempPwd = "xsybeiwzmjmsdkpy"; //應用程式密碼
                string ReceiveMail = email; //接收信箱

                string SmtpServer = "smtp.gmail.com";
                int SmtpPort = 587;
                MailMessage mms = new MailMessage();
                mms.From = new MailAddress(GoogleID);
                mms.Subject = subject;
                mms.Body = message;
                mms.IsBodyHtml = true;
                mms.SubjectEncoding = Encoding.UTF8;
                mms.To.Add(new MailAddress(ReceiveMail));
                SmtpClient client = new SmtpClient(SmtpServer, SmtpPort);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(GoogleID, TempPwd);//寄信帳密 
                client.Send(mms); //寄出信件
            }
        }
        //驗證帳號重複
        public IActionResult CheckEmailAvailability(string email) {
            bool isAvailable = !_context.Members.Any(e => e.MemberEmail == email);
            return Json(new { isAvailable });
        }
        //驗證電話重複
        public IActionResult CheckphoneAvailability(string phone) {
            bool isAvailable = !_context.Members.Any(e => e.MemberPhoneNumber == phone);
            return Json(new { isAvailable });
        }
        #endregion

        #region 登入頁面
        [HttpGet]
        public ActionResult Login() {
            return View();
        }
        [HttpPost]
        //public ActionResult Login(Member model)
        public ActionResult Login(string username, string password) {
            var Status = _context.Members
            .FirstOrDefault(x => x.MemberEmail == username && x.MemberPassword == password);
            if (Status == null) {
                ViewBag.Status = "空的";
            }
            else {
                if (Status.MemberStatus != "正常") {
                    ViewBag.Status = "異常";
                }
            }
            // 驗證用戶名和密碼
            if (IsValidUser(username, password)) {

                HttpContext.Session.SetString("MemberID", Status.MemberId);
                //ViewBag.se = HttpContext.Session.GetString("MemberID");
                //return View("Index");
                return RedirectToAction("MemberProfile"); // 導向登入成功
            }
            else {
                return View("Login");
            }
        }
        // 檢查資料庫中的資料是否一致
        private bool IsValidUser(string username, string password) {

            var query = _context.Members
                        .FirstOrDefault(x => x.MemberEmail == username && x.MemberPassword == password && x.MemberStatus == "正常");


            return query != null;
        }
        #endregion

        #region 登出功能
        [HttpPost]
        public ActionResult Logout() {
            // 清除Session
            HttpContext.Session.Remove("MemberID");
            // 重新導向首頁
            return RedirectToAction("Login");
            //HTML內程式碼
            //< a href = "@Url.Action("Logout", "CC_Members")" > 登出 </ a >
        }
        #endregion

        #region 會員資訊
        [HttpGet]
        public ActionResult MemberProfile() {
            var SessioID = HttpContext.Session.GetString("MemberID");
            ViewBag.Change = TempData["Change"];
            if (SessioID != null) {
                var query = _context.Members
                        .FirstOrDefault(x => x.MemberId == SessioID);
                if (query != null) {
                    //var member =  _context.Members.FindAsync(query.MemberId);
                    //return View(member);
                    ViewBag.ID = query.MemberId;
                    ViewBag.name = query.MemberName;
                    ViewBag.email = query.MemberEmail;
                    ViewBag.phone = query.MemberPhoneNumber.Substring(0, 10);
                    //ViewBag.BankCode = query.BankCode;
                    //ViewBag.BankNum = query.BankNum;
                    ViewBag.Postcode = query.MemberPostcode;
                    ViewBag.MemberCity = query.MemberCity;
                    ViewBag.Township = query.MemberTownship;
                    ViewBag.Address = query.MemberAddress;
                    ViewBag.Status = query.MemberStatus;
                    ViewBag.password = query.MemberPassword;
                }
                return View();
                //return RedirectToAction("MemberProfile");
            }
            return View();
            //return RedirectToAction("MemberProfile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MemberProfile([Bind("MemberId,MemberName,MemberPhoneNumber,MemberPostcode,MemberCity,MemberTownship,MemberAddress,MemberEmail,MemberStatus,MemberPassword,BankCode,BankNum")] Member member) {
            if (ModelState.IsValid) {
                TempData["Change"] = "修改成功";
                _context.Update(member);
                await _context.SaveChangesAsync();
                return RedirectToAction("MemberProfile");
            }
            return View();
        }
        #endregion
    }
}

