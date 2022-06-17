using PagedList;
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
        private const int pageSize = 5;
        private const int pageSizeView = 6;

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
            projekto_Duomenys.projektas.sukurimo_data = DateTime.Now;
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
                            TempData["Fail"] = "Įkeltas su netinkamu formatu";
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
                TempData["Succ"] = "Projektas sėkmingai sukurtas!";
                Response.Write("<script type='text/javascript' language='javascript'> alert('Projektas sėkmingai sukurtas!')</script>");
            }
            else
            {
                TempData["Fail"] = "Projektas nesukurtas!";

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
                TempData["Succ"] = "Projektas sėkmingai redaguotas!";
            }
            else
            {
                TempData["Fail"] = "Projektas neredaguotas!";
            }
            return View(projektas);
        }

        public ActionResult TrintiProjekta(int id)
        {
            Projektas_Repos projektas_Repos = new Projektas_Repos();
            bool flag = projektas_Repos.Trinti_Projekta(id);
            return RedirectToAction("GautiProjektus");
        }

        public ActionResult GautiProjektus(int? page)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (Session["Message"] != null)
            {
                if (Convert.ToBoolean(Session["Message"]))
                {
                    Response.Write("<script type='text/javascript' language='javascript'> alert('Nuotrauka pridėta sėkmingai!')</script>");
                    Session.Remove("Message");
                }
                else
                {
                    Response.Write("<script type='text/javascript' language='javascript'> alert('Nuotrauka pridėta nesėkmingai!')</script>");
                    Session.Remove("Message");
                }
            }

            Projektas_Repos projektas_Repos = new Projektas_Repos();
            List<Projektas> projektas = projektas_Repos.Gauti_Projektus();
            int pageNumber = (page ?? 1);
            return View(projektas.ToPagedList(pageNumber, pageSize));
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

        public ActionResult Projektai(int? page)
        {
            Projektas_Repos projektas_Repos = new Projektas_Repos();
            List<ProjektuPerziura> projektas = projektas_Repos.Gauti_Projektus_Atvaizdavimui();

            for(var i = 0; i < projektas.Count; i++)
            {
                projektas[i].diena = projektas[i].sukurimo_data.Day;
                projektas[i].menuo = menesiai[projektas[i].sukurimo_data.Month-1];
                List<Nuotrauka> nuotraukos = nuotrauka_Repos.Gauti_Projektu_Nuotraukas(projektas[i].id_Projektas);
                projektas[i].pradine_nuotrauka = nuotraukos[0].nuotraukos_nuoroda;
                if (projektas[i].aprasymas.Length > 150)
                {
                    projektas[i].aprasymas = projektas[i].aprasymas.Substring(0, 150);
                }
               
            }
            int pageNumber = (page ?? 1);
            return View(projektas.ToPagedList(pageNumber, pageSizeView));
        }

        public ActionResult DetaliInformacijaProjektas(int id)
        {
            Projektas projektas = projektas_Repos.Gauti_Projekta(id);
            projektas.gautosNuotraukos = nuotrauka_Repos.Gauti_Projektu_Nuotraukas(id);
            return View(projektas);
        }

        public ActionResult PridetiNuotraukuProjektui(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }

            Nuotrauku_Duomenys nuotrauku_Duomenys = new Nuotrauku_Duomenys();
            Session["ProjektoID"] = id;
            return View(nuotrauku_Duomenys);
        }

        [HttpPost]
        public ActionResult PridetiNuotraukuProjektui(Nuotrauku_Duomenys nuotrauku_Duomenys)
        {
            List<string> posiblesExtensions = new List<string>() { ".jpg", ".png", ".JPG", ".PNG", ".jpeg", ".JPEG" };
            nuotrauku_Duomenys.priskirtas_id = Convert.ToInt32(Session["ProjektoID"]);
            Session.Remove("ProjektoID");

            int photoCount = 0;
            foreach (HttpPostedFileBase file in nuotrauku_Duomenys.nuotraukos)
            {
                if (file != null)
                {
                    photoCount++;
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
                    nuotrauka.priskirtas_id = nuotrauku_Duomenys.priskirtas_id;
                    nuotrauka_Repos.Prideti_Projekto_Nuotraukas(nuotrauka);
                }
            }

            if (photoCount == 0)
            {
                Session["Message"] = false;
            }
            else
            {
                Session["Message"] = true;
            }

            return RedirectToAction("GautiProjektus");
        }

        public ActionResult GautiNuotraukasProjekto(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }

            Nuotrauka_Repos nuotrauka_Repos = new Nuotrauka_Repos();
            List<Nuotrauka> nuotraukos = nuotrauka_Repos.Gauti_Projektu_Nuotraukas(id);
            foreach (var item in nuotraukos)
            {
                var path = Path.Combine(("/Nuotraukos/") + item.nuotraukos_nuoroda);
                item.nuotraukos_nuoroda = path;
            }

            return View(nuotraukos);
        }

        public ActionResult TrintiNuotrauka(int id)
        {
            Nuotrauka_Repos nuotrauka_Repos = new Nuotrauka_Repos();
            Nuotrauka nuotrauka = nuotrauka_Repos.Gauti_Projekto_Nuotrauka(id);
            var ServerSavePath = Path.Combine(Server.MapPath("~/Nuotraukos/") + nuotrauka.nuotraukos_nuoroda);
            if (System.IO.File.Exists(ServerSavePath))
            {
                System.IO.File.Delete(ServerSavePath);
                bool flag = nuotrauka_Repos.Trinti_Nuotrauka(id);
            }
            return RedirectToAction("GautiNuotraukasProjekto", new { id = nuotrauka.priskirtas_id });
        }

        public ActionResult ProjektoInformacija(int id)
        {
            ProjektoPerziurosPuslapis perziura = new ProjektoPerziurosPuslapis();
            Session["ProjektoID"] = id;

            perziura.projektas = projektas_Repos.Gauti_Projekta(id);
            perziura.projektas.gautosNuotraukos = nuotrauka_Repos.Gauti_Projektu_Nuotraukas(perziura.projektas.id_Projektas);
            var perziuraList = projektas_Repos.Gauti_Projektus_Atvaizdavimui(true, id);
            perziura.perziura = perziuraList[0];
            return View(perziura);
        }
    }
}