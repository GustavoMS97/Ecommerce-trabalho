using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrabalhoEcommerce;
using TrabalhoEcommerce.Models;

namespace TrabalhoEcommerce.Controllers
{
    public class ProdutosController : Controller
    {
        private Contexto db = new Contexto();

        // GET: Produtos
        public ActionResult Index()
        {
            return View(db.Produto.ToList());
        }

        // GET: Produtos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // GET: Produtos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Produtos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Codigo,Nome,Preco")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                db.Produto.Add(produto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(produto);
        }

        // GET: Produtos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produtos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Codigo,Nome,Preco")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(produto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(produto);
        }

        // GET: Produtos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Produto produto = db.Produto.Find(id);
            db.Produto.Remove(produto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddToCart(int id)
        {
            var CPF = Session["clienteCPF"];
            var cliente = db.Cliente.Find(CPF);
            var produto = db.Produto.Find(id);
            int qtd;
            try
            {
                var carrinho = db.Carrinho.Where(c => c.Cliente.CPF == CPF.ToString() && c.StatusCarrinho.ID == 1);
                if (carrinho != null && carrinho.Any())
                {
                    Carrinho carrinhoNovo = carrinho.First();
                    var carrinhoProduto = db.CarrinhoProduto.Where(c => c.Carrinho.ID == carrinhoNovo.ID && c.Produto.Codigo == id);
                    if (carrinhoProduto != null && carrinhoProduto.Any())
                    {
                        CarrinhoProduto cp = carrinhoProduto.First();
                        cp.Quantidade += 1;
                        qtd = cp.Quantidade;

                        var carrinhoProduto1 = db.CarrinhoProduto.Where(c => c.Carrinho.ID == carrinhoNovo.ID);
                        if (carrinhoProduto1 != null && carrinhoProduto1.Any())
                        {
                            List<CarrinhoProduto> cpList = carrinhoProduto1.ToList();
                            qtd = cpList.Count;
                        }
                    }
                    else
                    {
                        CarrinhoProduto cp = new CarrinhoProduto();
                        cp.Carrinho = carrinhoNovo;
                        cp.Produto = produto;
                        cp.Quantidade = 1;
                        qtd = int.Parse(Session["qtdCarrinho"].ToString());
                        Session["qtdCarrinho"] = qtd += 1;
                        db.CarrinhoProduto.Add(cp);
                    }
                    db.SaveChanges();

                }
                else
                {
                    Carrinho c = new Carrinho();
                    StatusCarrinho st = db.StatusCarrinho.Find(1);
                    c.StatusCarrinho = st;
                    c.Cliente = cliente;
                    db.Carrinho.Add(c);
                    db.SaveChanges();
                    var newCarrinho = db.Carrinho.Where(ca => ca.Cliente.CPF == CPF.ToString() && ca.StatusCarrinho.ID == 1).First();
                    CarrinhoProduto cp = new CarrinhoProduto();
                    cp.Carrinho = newCarrinho;
                    cp.Produto = produto;
                    cp.Quantidade = 1;
                    qtd = 1;
                    db.CarrinhoProduto.Add(cp);
                    db.SaveChanges();
                }


                Session["qtdCarrinho"] = qtd;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("Index", "Produtos");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
