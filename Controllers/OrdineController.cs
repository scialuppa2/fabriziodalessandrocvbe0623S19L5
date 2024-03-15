using progetto_settimanaleS19L5.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace progetto_settimanaleS19L5.Controllers
{
    [Authorize]
    public class OrdineController : Controller
    {
        readonly ModelDbContext db = new ModelDbContext();

        public ActionResult ListaOrdine()
        {
            int? userId = (int?)Session["UserId"];

            if (userId == 0)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }

            if (userId.HasValue)
            {
                if (User.IsInRole("Admin"))
                {
                    var ordineAdmin = db.Ordine.Include(o => o.Utente);
                    return View(ordineAdmin.ToList());
                }
                else
                {
                    var ordineUtente = db.Ordine.Include(o => o.Utente).Where(o => o.IdUtente == userId.Value).ToList();
                    return View(ordineUtente);
                }
            }
            return View(new List<Ordine>());
        }

        public ActionResult ModificaOrdine(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordine ordine = db.Ordine.Find(id);
            if (ordine == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUtente = new SelectList(db.Utente, "IdUtente", "Nome", ordine.IdUtente);
            return View(ordine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificaOrdine(Ordine ordine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListaOrdine");
            }
            ViewBag.IdUtente = new SelectList(db.Utente, "IdUtente", "Nome", ordine.IdUtente);
            return View(ordine);
        }

        public ActionResult EliminaOrdine(int id)
        {
            Ordine ordine = db.Ordine.Find(id);
            db.Ordine.Remove(ordine);
            db.SaveChanges();
            return RedirectToAction("ListaOrdine");
        }


        public ActionResult ControlloOrdine()
        {
            return View();
        }

        [HttpPost]
        public JsonResult OrdiniEvasi()
        {
            List<Ordine> ordiniEvasi = db.Ordine.Where(o => o.Evaso).ToList();

            int totaleOrdiniEvasi = ordiniEvasi.Count();

            return Json(totaleOrdiniEvasi);
        }

        [HttpPost]
        public JsonResult OrdineByData(DateTime inputVal)
        {
            DateTime tomorrow = inputVal.AddDays(1);

            List<Ordine> ordine = db.Ordine.Where(a => a.Evaso == true && a.DataOrdine >= inputVal && a.DataOrdine < tomorrow).ToList();

            decimal TotaleIncasso = ordine.Sum(o => o.Importo);

            return Json(TotaleIncasso);
        }



    }
}