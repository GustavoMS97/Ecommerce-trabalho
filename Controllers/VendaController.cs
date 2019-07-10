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
        private List<VendaProduto> listaVendaProduto;

        public ActionResult List()
        {
            var CPF = Session["clienteCPF"];
            var cliente = db.Cliente.Find(CPF);
            Dictionary<int, double> totalPorVenda = new Dictionary<int, double>(0);

            var l = db.VendaProduto.Join(db.Venda,
                                vp => vp.Venda.ID,
                                v => v.ID,
                                (vp, v) => new { VendaProduto = vp, Venda = v })
                                .Where(a => a.Venda.Cliente.CPF == CPF.ToString())

                                .Join(db.Produto,
                                vp => vp.VendaProduto.Produto.Codigo,
                                v => v.Codigo,
                                (v, fp) => new { Venda = v, FormaPagamento = fp })
                                .Where(a => 1 == 1)

                                .Join(db.FormaPagamento,
                                v => v.Venda.Venda.FormaPagamento.ID,
                                fp => fp.ID,
                                (v, fp) => new { Venda = v, FormaPagamento = fp })
                                .Where(a => 1 == 1)

                                .Join(db.Cartao,
                                v => v.Venda.Venda.Venda.FormaPagamento.Cartao.ID,
                                c => c.ID,
                                (v, fp) => new { Venda = v, FormaPagamento = fp })
                                .Where(a => 1 == 1)

                                .Join(db.StatusPagamento,
                                v => v.Venda.Venda.Venda.Venda.StatusPagamento.ID,
                                sp => sp.ID,
                                (v, fp) => new { Venda = v, FormaPagamento = fp }).ToList();

            if (l != null && l.Any())
            {
                var lista = l.ToList();
                List<VendaProduto> vendas = new List<VendaProduto>(0);
                for (int i = 0; i < lista.Count; i++)
                {
                    if (!this.containsVendaProduto(lista.ElementAt(i).Venda.Venda.Venda.Venda.VendaProduto.Venda.ID, vendas))
                    {
                        vendas.Add(lista.ElementAt(i).Venda.Venda.Venda.Venda.VendaProduto);
                    }
                }
                listaVendaProduto = new List<VendaProduto>(vendas);
                for (int i = 0; i < vendas.Count; i++)
                {
                    if (totalPorVenda.TryGetValue(vendas.ElementAt(i).Venda.ID, out double saida))
                    {
                        totalPorVenda[vendas.ElementAt(i).Venda.ID] += vendas.ElementAt(i).Produto.Preco * vendas.ElementAt(i).Quantidade;
                    }
                    else
                    {
                        totalPorVenda[vendas.ElementAt(i).Venda.ID] = vendas.ElementAt(i).Produto.Preco * vendas.ElementAt(i).Quantidade;
                    }
                }
                ViewBag.totalPorVenda = totalPorVenda;
                return View(vendas);
            }
            return RedirectToAction("Index", "Home");
        }

        private bool containsVendaProduto(int id, List<VendaProduto> lista)
        {
            bool response = false;
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista.ElementAt(i).Venda.ID == id)
                {
                    response = true;
                    break;
                }
            }
            return response;
        }

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
                if (Session["tipoPagamento"] == null)
                {
                    Session["tipoPagamento"] = 0;
                }
                else
                {
                    int tipoPagamento = (int)Session["tipoPagamento"];
                    if (tipoPagamento == 0)
                    {

                    }
                    else
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
                var cartoes = db.Cartao.Where(cartao => cartao.Cliente.CPF == (string)CPF);
                List<Cartao> cartoesList = new List<Cartao>(0);
                if (cartoes != null && cartoes.Any())
                {
                    cartoesList = cartoes.ToList();
                }
                Session["cartoes"] = cartoesList;
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

        [HttpPost]
        public ActionResult checkout(string cartaoId, string boleto, string remessa)
        {
            //FINALIZAR O CARRINHO
            var CPF = Session["clienteCPF"];
            var cliente = db.Cliente.Find(CPF);
            double total = 0;
            List<VendaProduto> lCp = new List<VendaProduto>(0);
            if (cliente == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var carrinho = db.Carrinho.Where(c => c.Cliente.CPF == CPF.ToString() && c.StatusCarrinho.ID == 1);
            if (carrinho != null && carrinho.Any())
            {
                Carrinho c = carrinho.First();
                StatusCarrinho st = db.StatusCarrinho.Find(3);
                c.StatusCarrinho = st;

                var query = db.CarrinhoProduto.Join(db.Produto,
                               cp => cp.Produto.Codigo,
                               p => p.Codigo,
                               (cp, p) => new { CarrinhoProduto = cp, Produto = p })
                               .Where(a => a.CarrinhoProduto.Carrinho.ID == c.ID)
                               .Join(db.Carrinho,
                               cp => cp.CarrinhoProduto.Carrinho.ID,
                               car => car.ID,
                               (cp, p) => new { CarrinhoProduto = cp, Produto = p }).ToList();
                for (int i = 0; i < query.Count; i++)
                {
                    VendaProduto vp = new VendaProduto();
                    vp.Produto = query.ElementAt(i).CarrinhoProduto.Produto;
                    vp.Quantidade = query.ElementAt(i).CarrinhoProduto.CarrinhoProduto.Quantidade;
                    lCp.Add(vp);
                    total += query.ElementAt(i).CarrinhoProduto.CarrinhoProduto.Produto.Preco *
                        Convert.ToDouble(query.ElementAt(i).CarrinhoProduto.CarrinhoProduto.Quantidade);

                }
            }

            db.SaveChanges();
            //GERAR A VENDA
            Venda v = new Venda();
            v.Cliente = cliente;
            v.DataCompra = DateTime.Now;
            v.Entregue = false;
            v.Desconto = 0;
            v.Remessa = remessa;
            StatusPagamento stP = db.StatusPagamento.Find(1);
            v.StatusPagamento = stP;
            //VALIDAR O TIPO PARA DEFINIR O PAGAMENTO
            FormaPagamento f = new FormaPagamento();
            if (boleto == null && cartaoId != null && cartaoId.Trim().Length > 0)
            {
                var cartaoIdInt = int.Parse(cartaoId);
                var res = db.FormaPagamento.Where(forma => forma.Cartao.ID == cartaoIdInt);
                if (res != null && res.Any())
                {
                    f = res.First();
                }
                else
                {
                    Cartao c = db.Cartao.Find(cartaoIdInt);
                    f.Cartao = c;
                    db.FormaPagamento.Add(f);
                    db.SaveChanges();
                    res = db.FormaPagamento.Where(forma => forma.Cartao.ID == cartaoIdInt);
                    if (res != null && res.Any())
                    {
                        f = res.First();
                    }
                }
            }
            else
            {
                Boleto b = new Boleto();
                b.Codigo = boleto;
                b.DataEmissao = DateTime.Now;
                DateTime vencimento = DateTime.Now;
                vencimento = vencimento.AddDays(30);
                b.DataVencimento = vencimento;
                b.Valor = total;
                db.Boleto.Add(b);
                db.SaveChanges();
                Boleto bol = db.Boleto.Find(boleto);
                var res = db.FormaPagamento.Where(forma => forma.Boleto.Codigo == boleto);
                if (res != null && res.Any())
                {
                    f = res.First();
                }
            }
            v.FormaPagamento = f;
            db.Venda.Add(v);
            db.SaveChanges();
            //DEFINIR OS PRODUTOS NA VENDA.
            var vendaObj = db.Venda.Where(c => c.Cliente.CPF == CPF.ToString() && c.StatusPagamento.ID == 1);
            if (vendaObj != null && vendaObj.Any())
            {
                Venda Venda = vendaObj.First();
                for (int i = 0; i < lCp.Count; i++)
                {
                    lCp.ElementAt(i).Venda = Venda;
                    db.VendaProduto.Add(lCp.ElementAt(i));
                }
                db.SaveChanges();
                Session["qtdCarrinho"] = null;
                Session["carrinhoId"] = null;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("List", "Venda");
        }

        [HttpPost]
        public ActionResult retornar(int id)
        {
            var CPF = Session["clienteCPF"];
            var cliente = db.Cliente.Find(CPF);
            var l = db.VendaProduto.Join(db.Venda,
                                vp => vp.Venda.ID,
                                v => v.ID,
                                (vp, v) => new { VendaProduto = vp, Venda = v })
                                .Where(a => a.Venda.Cliente.CPF == CPF.ToString())

                                .Join(db.Produto,
                                vp => vp.VendaProduto.Produto.Codigo,
                                v => v.Codigo,
                                (v, fp) => new { Venda = v, FormaPagamento = fp })
                                .Where(a => 1 == 1)

                                .Join(db.FormaPagamento,
                                v => v.Venda.Venda.FormaPagamento.ID,
                                fp => fp.ID,
                                (v, fp) => new { Venda = v, FormaPagamento = fp })
                                .Where(a => 1 == 1)

                                .Join(db.Cartao,
                                v => v.Venda.Venda.Venda.FormaPagamento.Cartao.ID,
                                c => c.ID,
                                (v, fp) => new { Venda = v, FormaPagamento = fp })
                                .Where(a => 1 == 1)

                                .Join(db.StatusPagamento,
                                v => v.Venda.Venda.Venda.Venda.StatusPagamento.ID,
                                sp => sp.ID,
                                (v, fp) => new { Venda = v, FormaPagamento = fp }).ToList();

            if (l != null && l.Any())
            {
                var lista = l.ToList();
                listaVendaProduto = new List<VendaProduto>(0);
                for (int i = 0; i < lista.Count; i++)
                {
                    if (!this.containsVendaProduto(lista.ElementAt(i).Venda.Venda.Venda.Venda.VendaProduto.Venda.ID, listaVendaProduto))
                    {
                        listaVendaProduto.Add(lista.ElementAt(i).Venda.Venda.Venda.Venda.VendaProduto);
                    }
                }
            }
            var idToSearch = 0;
            for (int i = 0; i < listaVendaProduto.Count; i++)
            {
                if (listaVendaProduto.ElementAt(i).ID == id)
                {
                    idToSearch = listaVendaProduto.ElementAt(i).Venda.ID;
                    break;
                }
            }
            if (idToSearch != 0)
            {
                Venda venda = db.Venda.Find(idToSearch);
                StatusPagamento st = db.StatusPagamento.Find(5);
                venda.StatusPagamento = st;
                db.SaveChanges();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("List", "Venda");
        }
    }
}