using PVP_K180.Models;
using PVP_K180.ModelView;
using PVP_K180.Repos;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PVP_K180.Controllers
{
    public class ProjektasController : Controller
    {
        private Projektas_Repos projektas_Repos = new Projektas_Repos();

        // GET: Projektas
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProjektoKurimas()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ProjektoKurimas(Projektas projektas)
        {
            Projektas_Repos projektas_Repos = new Projektas_Repos();
            projektas.fk_Vartotojasid_Vartotojas = (int)Session["UserID"];
            bool flag = projektas_Repos.Sukurti_Projekta(projektas);

            if (true)
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Projektas sėkmingai sukurtas!')</script>");
            }
            else
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Projektas nesukurtas!')</script>");
            }

            return View();
        }

        public ActionResult RedaguotiProjekta(int id)
        {
            Projektas_Repos projektas_Repos = new Projektas_Repos();
            Projektas projektas = projektas_Repos.Gauti_Projekta(id);
            return View(projektas);
        }

        [HttpPost]
        public ActionResult RedaguotiProjekta(int id, Projektas projektas)
        {
            projektas.id_Projektas = id;
            Projektas_Repos projektas_Repos = new Projektas_Repos();
            projektas.fk_Vartotojasid_Vartotojas = (int)Session["UserID"];
            bool flag = projektas_Repos.Redaguoti_Projekta(projektas);
            if (flag)
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Projektas sėkmingai redaguotas!')</script>");
            }
            else
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Projektas neredaguotas!')</script>");
            }
            return View(projektas);
        }

        public ActionResult TrintiProjekta(int id)
        {
            Projektas_Repos projektas_Repos = new Projektas_Repos();
            bool flag = projektas_Repos.Trinti_Projekta(id);
            return RedirectToAction("GautiProjektus");
        }

        public ActionResult GautiProjektus()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }

            Projektas_Repos projektas_Repos = new Projektas_Repos();
            List<Projektas> projektas = projektas_Repos.Gauti_Projektus();
            return View(projektas);
        }

        public ActionResult Komentarai(int id)
        {
            KomentaroLangasProjektas komentaruLangas = new KomentaroLangasProjektas();
            komentaruLangas.parasytiKomentarai = projektas_Repos.Gauti_Projekto_Komentarus(id);
            TempData["ProjektoID"] = id;
            return View(komentaruLangas);
        }

        [HttpPost]
        public ActionResult Komentarai(int id, KomentaroLangasProjektas komentaras)
        {
            komentaras.rasomasKomentaras.fk_Vartotojasid_Vartotojas = Convert.ToInt32(Session["UserID"]);
            komentaras.rasomasKomentaras.parasymo_data = DateTime.Now;
            komentaras.rasomasKomentaras.priskirtas_id = id;
            projektas_Repos.Rasyti_Komentara(komentaras.rasomasKomentaras);
            TempData["SuccsessComment"] = "Komentaras sėkmingai parašytas";
            return RedirectToAction("Komentarai", new { id = id });
        }

        public ActionResult TrintiKomentara(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            bool check = projektas_Repos.PatikrintiArGalimaTrinti(Convert.ToInt32(Session["UserID"]), id);
            if (check)
            {
                projektas_Repos.Trinti_Komentara(id);
                TempData["DeleteSucc"] = "Komentaras ištrintas sėkmingai";
            }
            else
            {
                TempData["DeleteFail"] = "Komentaro nepavyko ištrinti";
            }

            return RedirectToAction("Komentarai", new { id = Convert.ToInt32(TempData["ProjektoID"]) });
        }
    }
}