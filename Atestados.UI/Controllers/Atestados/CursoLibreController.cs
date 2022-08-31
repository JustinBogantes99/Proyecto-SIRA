using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Atestados.UI.Controllers.Atestados
{
    public class CursoLibreController : Controller
    {
        // GET: CursoLibre
        public ActionResult Index()
        {
            return View();
        }

        // GET: CursoLibre/Crear
        public ActionResult Crear()
        {
            return View();
        }
    }
}