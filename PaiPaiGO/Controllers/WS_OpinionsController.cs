using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaiPaiGO.Models;
using X.PagedList;
using System.Drawing;

namespace PaiPaiGo.Controllers
{
	public class WS_OpinionsController : Controller
	{
		private readonly PaiPaiGoContext _context;

		public WS_OpinionsController(PaiPaiGoContext context)
		{
			_context = context;
		}

		//寄信
		[HttpPost]
		public async Task<IActionResult> SendMail(string reportedMemberId, string subject, string content)
		{
			//string numstring = maxId.ToString();
			//int numLength = numstring.Length;
			//int zeronum = 3 - numLength;
			//for (int i = 0; i < zeronum; i++)
			//{
			//	numstring = "0" + numstring;
			//}
			// 使用 reportedMemberId 找到對應的 MemberEmail（使用LINQ或資料庫查詢）。
			var memberEmail = _context.Members
				.Where(m => m.MemberId == reportedMemberId)
				.Select(m => m.MemberEmail)
				.FirstOrDefault();

			if (memberEmail != null)
			{

				var emailSubject = subject;
				var emailMessage = content;
				var emailService = new EmailService();
				await emailService.SendEmailAsync(memberEmail, emailSubject, emailMessage);
				return Ok("已新增");
			}
			return NotFound("不成功");
		}

		public class EmailService
		{
			public async Task SendEmailAsync(string email, string subject, string message)
			{
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

		//新增警告
		[HttpPost]
		public IActionResult AddWarning(int missionId)
		{
			try
			{
				// 找對應的 WS_Opinion mission
				var opinion = _context.Opinions.FirstOrDefault(op => op.MissionId == missionId);

				if (opinion != null)
				{
					// 更新警告
					opinion.Warn = "警告";
					// 保存
					_context.SaveChanges();
					return Ok("警告已新增");
				}
				else
				{
					// 如果没有找到return找不道
					return NotFound("找不到對應的紀錄");
				}
			}
			catch (Exception ex)
			{
				// 異常處理
				return StatusCode(500, "伺服器錯誤：" + ex.Message);
			}
		}




		//改狀態
		[HttpPost]
		public ActionResult SaveOpinionStatusChanges(List<OpinionState> changes)
		{
			if (changes != null && changes.Any())
			{
				foreach (var change in changes)
				{
					var opinion = _context.Opinions.FirstOrDefault(o => Convert.ToInt32(o.Ratingnumber) == change.Ratingnumber);
					if (opinion != null)
					{
						opinion.State = change.State;
					}
				}
				_context.SaveChanges();
				// return成功的JSON回應
				return Json(new { success = true });
			}
			// return錯誤的JSON回應
			return Json(new { success = false });
		}






		// GET: WS_Opinions
		public async Task<IActionResult> AdmOpinion(string opinionStatusFilter, int? page)
        {            
			//layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            var paiPaiGoContext = _context.Opinions.Include(o => o.Mission).Include(o => o.ReportMember).Include(o => o.ReportedMember);

			//狀態選單用
			if (string.IsNullOrEmpty(opinionStatusFilter))
			{
				// 初始加載時，返回完整view(包含ViewBag)
				var allOpinions = await _context.Opinions.ToPagedListAsync(page ?? 1, 5);
				var selectList = _context.Opinions.Select(m => m.State).Distinct().ToList();
				ViewBag.OpinionStatusList = new SelectList(selectList, selectList);
				return View(allOpinions);
			}
			else
			{
				var filteredOpinions = await _context.Opinions
					.Where(m => m.State == opinionStatusFilter)
					.ToPagedListAsync(page ?? 1, 5);
				return PartialView("_OpinionListPartial", filteredOpinions);
			}

        }



        // GET: WS_Opinions/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{            //layout用
        //    ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
        //    ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
        //    if (id == null || _context.Opinions == null)
        //    {
        //        return NotFound();
        //    }

        //    var opinion = await _context.Opinions.FindAsync(id);
        //    if (opinion == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["MissionId"] = new SelectList(_context.Missions, "MissionId", "MissionId", opinion.MissionId);
        //    ViewData["ReportMemberId"] = new SelectList(_context.Members, "MemberId", "MemberId", opinion.ReportMemberId);
        //    ViewData["ReportedMemberId"] = new SelectList(_context.Members, "MemberId", "MemberId", opinion.ReportedMemberId);
        //    return View(opinion);
        //}



        private bool OpinionExists(string id)
        {            //layout用
            ViewBag.YU_ID = HttpContext.Session.GetString("MemberID");
            ViewBag.YU_Name = HttpContext.Session.GetString("MemberName");
            return (_context.Opinions?.Any(e => e.Type == id)).GetValueOrDefault();
        }
    }
}
