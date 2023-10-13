using Microsoft.AspNetCore.Mvc;

namespace PaiPaiGO.Controllers
{
    public class HS_ProblemController : Controller
    {
        public IActionResult CommonProblem()
        {
            return View();
        }
        public IActionResult Explain()
        {
            return View();
        }
    }
}
