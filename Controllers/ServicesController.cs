using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Graduation.Controllers
{
    public class ServicesController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PointModel()
        {
            return View();
        }
        public IActionResult PhotoModel()
        {
            return View();
        }
        public IActionResult PhotoCad()
        {
            return View();
        }
        public IActionResult TryPointModel()
        {
            return View();
        }
        public IActionResult TryPointModelGeneric()
        {
            return View();
        }

        public IActionResult TryPhotoCad()
        {
            return View();
        }
        public IActionResult TryPhotoCadAuto()
        {
            return View();
        }
        public IActionResult TryPhotoModel()
        {
            return View();
        }
        public IActionResult TryPhotoModelGeneric()
        {
            return View();
        }

    }
}
