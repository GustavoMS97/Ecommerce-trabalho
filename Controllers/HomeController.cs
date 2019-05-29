using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrabalhoEcommerce.Models;

namespace TrabalhoEcommerce.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            using (var ctx = new Contexto())
            {
                List<Cliente> clientes = ctx.Cliente.ToList();
                ViewBag.Message = "Clientes cadastrados";
                ViewBag.clientes = clientes;
                ViewBag.clientesLength = clientes.Count;
            }

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}