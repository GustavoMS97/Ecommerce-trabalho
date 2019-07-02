using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrabalhoEcommerce.Models;

namespace TrabalhoEcommerce.Controllers
{
    public class VendaController : Controller
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
                if(Session["tipoPagamento"] == null)
                {
                    Session["tipoPagamento"] = 0;
                }else
                {
                    int tipoPagamento = (int)Session["tipoPagamento"];
                    if (tipoPagamento == 0)
                    {

                    }else
                    {
                        var chars = "0123456789";
                        var stringChars = new char[48];
                        var random = new Random();

                        for (int i = 0; i < stringChars.Length; i++)
                        {
                            stringChars[i] = chars[random.Next(chars.Length)];
                        }

                        var finalString = new String(stringChars);
                        ViewBag.numeroBoleto = finalString;
                    }
                }
                ViewBag.valorFinal = total;
                return View(lCp);
            }

            //Buscar tipos de pagamento.

            return RedirectToAction("Index", "Home");
        }

        public ActionResult TrocaTipoPagamento(int tipoPagamento)
        {
            Session["tipoPagamento"] = tipoPagamento;
            return RedirectToAction("Index", "Venda");
        }
    }
}