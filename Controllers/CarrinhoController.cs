using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrabalhoEcommerce.Models;
using System.Data;

namespace TrabalhoEcommerce.Controllers
{
    public class CarrinhoController : Controller
    {
        private Contexto db = new Contexto();

        public ActionResult Index()
        {
            var CPF = Session["clienteCPF"];
            var cliente = db.Cliente.Find(CPF);
            double total = 0;
            if (cliente == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var carrinho = db.Carrinho.Where(c => c.Cliente.CPF == CPF.ToString() && c.StatusCarrinho.ID == 1);
            if (carrinho != null && carrinho.Any())
            {
                Carrinho c = carrinho.First();
                Session["carrinhoId"] = c.ID;
                var query = db.CarrinhoProduto.Join(db.Produto,
                                cp => cp.Produto.Codigo,
                                p => p.Codigo,
                                (cp, p) => new { CarrinhoProduto = cp, Produto = p })
                                .Where(a => a.CarrinhoProduto.Carrinho.ID == c.ID)
                                .Join(db.Carrinho,
                                cp => cp.CarrinhoProduto.Carrinho.ID,
                                car => car.ID,
                                (cp, p) => new { CarrinhoProduto = cp, Produto = p }).ToList();
                List<CarrinhoProduto> lCp = new List<CarrinhoProduto>(0);
                for (int i = 0; i < query.Count; i++)
                {
                    lCp.Add(query.ElementAt(i).CarrinhoProduto.CarrinhoProduto);
                    total += query.ElementAt(i).CarrinhoProduto.CarrinhoProduto.Produto.Preco *
                        Convert.ToDouble(query.ElementAt(i).CarrinhoProduto.CarrinhoProduto.Quantidade);

                }
                ViewBag.valorFinal = total;
                return View(lCp);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult changeQtd(int value, int id)
        {
            var carrinhoProduto = db.CarrinhoProduto.Find(id);
            if (carrinhoProduto != null)
            {
                if (value > 0 ||
                    (carrinhoProduto.Quantidade >= 2))
                {
                    carrinhoProduto.Quantidade += value;
                }
                else
                {
                    db.CarrinhoProduto.Remove(carrinhoProduto);
                    var qtd = int.Parse(Session["qtdCarrinho"].ToString());
                    Session["qtdCarrinho"] = qtd -= 1;
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Carrinho");
        }

        [HttpPost]
        public ActionResult checkout(int carrinhoId)
        {
            //TODO: Implementar a finalizacao do carriho.
            return RedirectToAction("Index", "Carrinho");
        }
    }
}