using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrabalhoEcommerce.Controllers
{
    public class LoginController : Controller
    {
        private Contexto db = new Contexto();
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Erro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logar(string email, string password)
        {
            try
            {
                var cliente = db.Cliente.Where(c => c.email == email && c.senha == password).First();
                cliente.ToString();
                Session["clienteCPF"] = cliente.CPF;
                Session["clienteNome"] = cliente.nome;
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                return RedirectToAction("Erro", "Login");
            }
        }

        public ActionResult Sair()
        {
            Session["clienteCPF"] = null;
            Session["clienteNome"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}