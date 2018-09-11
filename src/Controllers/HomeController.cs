using System.Web.Mvc;
using Agatha.Models;
using Agatha.Services;

namespace Agatha.Controllers
{
    public class HomeController : Controller
    {
        private IPaintingsService _paintingsService;

        public HomeController( IPaintingsService paintingsService )
            {
            _paintingsService = paintingsService;
            }

        public ActionResult Index()
            {
            ViewBag.PaintingService = _paintingsService;

            return View( new IndexModel(_paintingsService));
            }

        public ActionResult Works()
            {
            return View( new WorksModel(_paintingsService) );
            }

        public ActionResult About()
            {
            return View();
            }

        public ActionResult Contact()
            {
            return View();
            }
    }
}
