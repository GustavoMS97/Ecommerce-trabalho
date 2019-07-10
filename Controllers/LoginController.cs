using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrabalhoEcommerce.Models;

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
                var carrinho = db.Carrinho.Where(c => c.Cliente.CPF == cliente.CPF && c.StatusCarrinho.ID == 1);
                if (carrinho != null && carrinho.Any())
                {
                    Carrinho carrinhoNovo = carrinho.First();
                    var carrinhoProduto = db.CarrinhoProduto.Where(c => c.Carrinho.ID == carrinhoNovo.ID);
                    if (carrinhoProduto != null && carrinhoProduto.Any())
                    {
                        List<CarrinhoProduto> cpList = carrinhoProduto.ToList();
                        int qtd = cpList.Count;
                        Session["qtdCarrinho"] = qtd;
                    }
                }
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