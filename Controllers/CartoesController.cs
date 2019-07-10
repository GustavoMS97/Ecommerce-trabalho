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
    public class CartoesController : Controller
    {
        private Contexto db = new Contexto();

        // GET: Cartoes
        public ActionResult Index()
        {
            var CPF = Session["clienteCPF"];
            try
            {
                var cartoes = db.Cartao.Where(c => c.Cliente.CPF == CPF.ToString()).ToList();
                return View(cartoes);
            }
            catch(Exception e)
            {
                return View();
            }
        }

        // GET: Cartoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cartao cartao = db.Cartao.Find(id);
            if (cartao == null)
            {
                return HttpNotFound();
            }
            return View(cartao);
        }

        // GET: Cartoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cartoes/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Numero,Ag,DataVencimentoStr")] Cartao cartao)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.ParseExact(cartao.DataVencimentoStr, "yyyy-MM",
                                       System.Globalization.CultureInfo.InvariantCulture);
                var CPF = Session["clienteCPF"];
                var cliente = db.Cliente.Where(c => c.CPF == CPF.ToString()).First();
                cartao.Cliente = cliente;
                cartao.DataVencimento = dt;
                db.Cartao.Add(cartao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cartao);
        }

        // GET: Cartoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cartao cartao = db.Cartao.Find(id);
            if (cartao == null)
            {
                return HttpNotFound();
            }
            return View(cartao);
        }

        // POST: Cartoes/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Numero,Ccv,Cc,Ag,DataVencimento")] Cartao cartao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cartao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cartao);
        }

        // GET: Cartoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cartao cartao = db.Cartao.Find(id);
            if (cartao == null)
            {
                return HttpNotFound();
            }
            return View(cartao);
        }

        // POST: Cartoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cartao cartao = db.Cartao.Find(id);
            db.Cartao.Remove(cartao);
            db.SaveChanges();
            return RedirectToAction("Index");
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
