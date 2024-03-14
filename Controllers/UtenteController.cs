using progetto_settimanaleS19L5.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace progetto_settimanaleS19L5.Controllers
{

    [Authorize]
    public class UtenteController : Controller
    {
        readonly ModelDbContext db = new ModelDbContext();

        public ActionResult ListaUtente()
        {
            var utente = db.Utente.Where(u => u.Ruolo.Role != "Admin").ToList();
            return View(utente.ToList());
        }

        public ActionResult DettaglioUtente(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utente utente = db.Utente.Find(id);
            if (utente == null)
            {
                return HttpNotFound();
            }
            return View(utente);
        }

        public ActionResult ModificaUtente(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utente utente = db.Utente.Find(id);
            if (utente == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdRuolo = new SelectList(db.Ruolo, "IdRuolo", "Role", utente.IdRuolo);
            return View(utente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificaUtente(Utente utente)
        {
            if (ModelState.IsValid)
            {
                var utenteOriginale = db.Utente.Find(utente.IdUtente);

                if (utenteOriginale == null)
                {
                    return HttpNotFound();
                }

                utenteOriginale.Nome = utente.Nome;
                utenteOriginale.Cognome = utente.Cognome;
                utenteOriginale.Provincia = utente.Provincia;
                utenteOriginale.Citta = utente.Citta;
                utenteOriginale.Indirizzo = utente.Indirizzo;

                db.SaveChanges();

                return RedirectToAction("ListaUtente");
            }

            return View(utente);
        }


        public ActionResult EliminaUtente(int id)
        {
            Utente utente = db.Utente.Find(id);
            db.Utente.Remove(utente);
            db.SaveChanges();
            return RedirectToAction("ListaUtente");
        }
    }
}