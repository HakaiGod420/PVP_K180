using PVP_K180.Models;
using PVP_K180.ModelView;
using PVP_K180.Repos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace PVP_K180.Controllers
{
    public class ProjektasController : Controller
    {
        private Projektas_Repos projektas_Repos = new Projektas_Repos();
        private Nuotrauka_Repos nuotrauka_Repos = new Nuotrauka_Repos();
        string[] menesiai = new string[12] { "SAU", "VAS", "KOV", "BAL", "GEG", "BIR", "LIE", "LAP", "RUG", "SPA", "LAP", "GRU" };

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
        public ActionResult ProjektoKurimas(Projekto_duomenys projekto_Duomenys)
        {
            List<string> posiblesExtensions = new List<string>() { ".jpg", ".png", ".JPG", ".PNG", ".jpeg", ".JPEG" };
            Projektas_Repos projektas_Repos = new Projektas_Repos();
            projekto_Duomenys.projektas.fk_Vartotojasid_Vartotojas = (int)Session["UserID"];
            bool flag = projektas_Repos.Sukurti_Projekta(projekto_Duomenys.projektas);
            if (flag)
            {
                int lastIndex = projektas_Repos.Gauti_Paskutini_Prideto_Index();

                foreach (HttpPostedFileBase file in projekto_Duomenys.nuotraukos)
                {
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);

                        var extension = Path.GetExtension(file.FileName);
                        if (!posiblesExtensions.Contains(extension))
                        {
                            Response.Write("<script type='text/javascript' language='javascript'> alert('Įkeltas su netinkamu formatu')</script>");
                            return View();
                        }

                        var random = Guid.NewGuid() + InputFileName;
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Nuotraukos/") + random);


                        file.SaveAs(ServerSavePath);
                        Nuotrauka nuotrauka = new Nuotrauka();
                        nuotrauka.nuotraukos_nuoroda = random;
                        nuotrauka.priskirtas_id = lastIndex;
                        nuotrauka_Repos.Prideti_Projekto_Nuotraukas(nuotrauka);

                    }
                }

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

        public ActionResult Projektai()
        {
            Projektas_Repos projektas_Repos = new Projektas_Repos();
            List<ProjektuPerziura> projektas = projektas_Repos.Gauti_Projektus_Atvaizdavimui();

            for(var i = 0; i < projektas.Count; i++)
            {
                projektas[i].diena = projektas[i].sukurimo_data.Day;
                projektas[i].menuo = menesiai[projektas[i].sukurimo_data.Month-1];
                List<Nuotrauka> nuotraukos = nuotrauka_Repos.Gauti_Projektu_Nuotraukas(projektas[i].id_Projektas);
                projektas[i].pradine_nuotrauka = nuotraukos[0].nuotraukos_nuoroda;
            }
            return View(projektas);
        }

        public ActionResult DetaliInformacijaProjektas(int id)
        {
            Projektas projektas = projektas_Repos.Gauti_Projekta(id);
            projektas.gautosNuotraukos = nuotrauka_Repos.Gauti_Projektu_Nuotraukas(id);
            return View(projektas);
        }
    }
}