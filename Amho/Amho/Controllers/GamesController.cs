using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Amho.Controllers
{
    public class GamesController : Controller
    {
        // GET: Games
        public ActionResult HeadOrTail()
        {
            return View();
        }

        public ActionResult GuessTheNumber()
        {
            return View();
        }
    }
}