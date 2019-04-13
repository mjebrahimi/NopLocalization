using System.Diagnostics;
using CacheManager.Core;
using Microsoft.AspNetCore.Mvc;
using NopLocalization.Sample.Models;
using System.Threading;
using System.Threading.Tasks;

namespace NopLocalization.Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext appDbContext;
        public HomeController(AppDbContext appDbContext, ICacheManager<string> cacheManagerString)
        {
            this.appDbContext = appDbContext;
            var x = appDbContext.Products.Find(1);
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
