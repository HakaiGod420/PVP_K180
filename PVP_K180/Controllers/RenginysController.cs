using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PVP_K180.Repos;
using PVP_K180.Models;
using PVP_K180.ModelView;
using System.IO;

namespace PVP_K180.Controllers
{
    public class RenginysController : Controller
    {

        private Renginys_Repos renginys_Repos = new Renginys_Repos();
        Nuotrauka_Repos nuotrauka_Repos = new Nuotrauka_Repos();

        // GET: Renginys
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RenginioKurimas()
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
        public ActionResult RenginioKurimas(Renginio_duomenys renginio_Duomenys)
        {
            int counter = 0;
            List<string> posiblesExtensions = new List<string>() { ".jpg", ".png", ".JPG", ".PNG", ".jpeg", ".JPEG" };
            Renginys_Repos renginys_Repos = new Renginys_Repos();
            renginio_Duomenys.renginys.vartotojas_id = (int)Session["UserID"];
            renginio_Duomenys.renginys.paskelbimo_data = DateTime.Now;

            if (Convert.ToDouble(TempData["RenginysLang"]) == 0 || Convert.ToDouble(TempData["RenginysLong"]) == 0)
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Turite patvirtinti teisingą lokaciją')</script>");
                return View();
            }

            renginio_Duomenys.renginys.zemelapis_ilguma = (float)Convert.ToDouble(TempData["RenginysLang"]);
            renginio_Duomenys.renginys.zemelapis_platuma = (float)Convert.ToDouble(TempData["RenginysLong"]);

            bool flag = renginys_Repos.Sukurti_Rengini(renginio_Duomenys.renginys);
            if (flag)
            {
                int lastIndex = renginys_Repos.Gauti_Paskutini_Prideto_Index();

                foreach (HttpPostedFileBase file in renginio_Duomenys.nuotraukos)
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
                        nuotrauka_Repos.Prideti_Renginio_Nuotraukas(nuotrauka);

                    }
                }

                Response.Write("<script type='text/javascript' language='javascript'> alert('Renginys sėkmingai sukurtas!')</script>");
            }
            else
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Renginys nesukurtas!')</script>");

            }
            return View();
        }

        public ActionResult GautiRenginius(int? busena, DateTime? nuo, DateTime? iki)
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

            List<Renginys> renginys = renginys_Repos.Gauti_Renginius();
            if (busena != null)
            {
                renginys = renginys.Where(x => x.renginio_busena == busena).ToList();
            }
            if (nuo != null)
            {
                var data = (DateTime)nuo;
                renginys = renginys.Where(x => x.pabaigos_data.Date >= (DateTime)data.Date).ToList();
            }

            if (iki != null)
            {
                var data = (DateTime)iki;
                renginys = renginys.Where(x => x.pabaigos_data.Date <= (DateTime)data.Date).ToList();
            }
            return View(renginys);
        }

        public ActionResult RedaguotiRengini(int id)
        {
            Renginys_Repos renginys_Repos = new Renginys_Repos();
            Renginys renginys = renginys_Repos.Gauti_Rengini(id);
            return View(renginys);
        }

        [HttpPost]
        public ActionResult RedaguotiRengini(int id, Renginys renginys)
        {
            renginys.id_Renginys = id;
            Renginys_Repos renginys_Repos = new Renginys_Repos();
            renginys.vartotojas_id = (int)Session["UserID"];
            renginys.paskelbimo_data = DateTime.Now;
            Renginys renginys2 = renginys_Repos.Gauti_Rengini(id);
            renginys.zemelapis_ilguma = renginys2.zemelapis_ilguma;
            renginys.zemelapis_platuma = renginys2.zemelapis_platuma;

            if (Convert.ToDouble(TempData["RenginysLang"]) == 0 || Convert.ToDouble(TempData["RenginysLong"]) == 0)
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Turite patvirtinti teisingą lokaciją')</script>");
                return View(renginys);
            }

            renginys.zemelapis_ilguma = (float)Convert.ToDouble(TempData["RenginysLang"]);
            renginys.zemelapis_platuma = (float)Convert.ToDouble(TempData["RenginysLong"]);

            bool flag = renginys_Repos.Redaguoti_Rengini(renginys);
            if (flag)
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Projektas sėkmingai redaguotas!')</script>");
            }
            else
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Projektas neredaguotas!')</script>");
            }
            return View(renginys);
        }


        public ActionResult Komentarai(int id)
        {
            KomentaroLangasRenginys komentaruLangas = new KomentaroLangasRenginys();
            komentaruLangas.parasytiKomentarai = renginys_Repos.Gauti_Renginio_Komentarus(id);
            Session["RenginioID"] = id;
            ViewBag.currentReng = id;
            komentaruLangas.parasytiKomentarai = komentaruLangas.parasytiKomentarai.OrderByDescending(x => x.parasymo_data).ToList();
            return View(komentaruLangas);
        }

        [HttpPost]
        public ActionResult Komentarai(int id, KomentaroLangasRenginys komentaras)
        {
            komentaras.rasomasKomentaras.fk_Vartotojasid_Vartotojas = Convert.ToInt32(Session["UserID"]);
            komentaras.rasomasKomentaras.parasymo_data = DateTime.Now;
            komentaras.rasomasKomentaras.priskirtas_id = id;
            renginys_Repos.Rasyti_Komentara(komentaras.rasomasKomentaras);
            TempData["SuccsessComment"] = "Komentaras sėkmingai parašytas";
            return RedirectToAction("Komentarai", new { id = id });
        }

        public ActionResult TrintiKomentara(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            bool check = renginys_Repos.PatikrintiArGalimaTrinti(Convert.ToInt32(Session["UserID"]), id);
            if (check)
            {
                renginys_Repos.Trinti_Komentara(id);
                TempData["DeleteSucc"] = "Komentaras ištrintas sėkmingai";
            }
            else
            {
                TempData["DeleteFail"] = "Komentaro nepavyko ištrinti";
            }

            return RedirectToAction("Komentarai", new { id = Convert.ToInt32(Session["RenginioID"]) });
        }

        [HttpPost]
        public void IsaugotiLokacija(float x, float y)
        {
            TempData["RenginysLang"] = x;
            TempData["RenginysLong"] = y;
        }

        public ActionResult Renginiai()
        {
            List<Renginys> renginiai = renginys_Repos.Gauti_Renginius();

            for(var i = 0; i<renginiai.Count;i++)
            {
                var length = renginiai[i].aprasymas.Length;

                if(length > 200)
                {
                    renginiai[i].aprasymas = renginiai[i].aprasymas.Substring(0, 200);
                }
                renginiai[i].gautosNuotraukos = nuotrauka_Repos.Gauti__Renginiu_Nuotraukas(renginiai[i].id_Renginys);
            }

     
            return View(renginiai);
        }

        public ActionResult DetaliInformacijaRenginys(int id)
        {
            Renginys renginys = renginys_Repos.Gauti_Rengini(id);
            renginys.gautosNuotraukos = nuotrauka_Repos.Gauti__Renginiu_Nuotraukas(id);
            return View(renginys);
        }

        public ActionResult KeistiRenginioBusena(int id)
        {
            Renginys_Repos renginiai_Repos = new Renginys_Repos();
            Renginys renginys = renginiai_Repos.Gauti_Rengini(id);
            return View(new RenginioBusenaPerziura
            {
                renginio_Busena = renginys.renginio_busena,
                Busenos = renginys_Repos.GautiBusenas().Select(b => new SelectListItem
                {
                    Text = b.name,
                    Value = b.id_Renginio_busena.ToString()
                })
            });
        }

        [HttpPost]
        public ActionResult KeistiRenginioBusena(int id, RenginioBusenaPerziura renginysBusenaPerziura)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }

           

            renginys_Repos.Atnaujinti_Renginio_Busena(id, renginysBusenaPerziura.renginio_Busena);
            Response.Write("<script type='text/javascript' language='javascript'> alert('Renginio būsena pakeista')</script>");

            return RedirectToAction("GautiRenginius", "Renginys");
        }

        public ActionResult PerziuretiRengini(int id)
        {
            Renginys renginys = renginys_Repos.Gauti_Rengini(id);
            renginys.gautosNuotraukos = nuotrauka_Repos.Gauti__Renginiu_Nuotraukas(id);

            return View(renginys);
        }

        public ActionResult PridetiNuotraukuRenginiui(int id)
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
            Session["RenginioID"] = id;
            return View(nuotrauku_Duomenys);
        }

        [HttpPost]
        public ActionResult PridetiNuotraukuRenginiui(Nuotrauku_Duomenys nuotrauku_Duomenys)
        {
            List<string> posiblesExtensions = new List<string>() { ".jpg", ".png", ".JPG", ".PNG", ".jpeg", ".JPEG" };
            nuotrauku_Duomenys.priskirtas_id = Convert.ToInt32(Session["RenginioID"]);
            Session.Remove("RenginioID");

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
                    nuotrauka_Repos.Prideti_Renginio_Nuotraukas(nuotrauka);
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

            return RedirectToAction("GautiRenginius");
        }

        public ActionResult GautiNuotraukasRenginio(int id)
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
            List<Nuotrauka> nuotraukos = nuotrauka_Repos.Gauti__Renginiu_Nuotraukas(id);
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
            Nuotrauka nuotrauka = nuotrauka_Repos.Gauti_Renginio_Nuotrauka(id);
            var ServerSavePath = Path.Combine(Server.MapPath("~/Nuotraukos/") + nuotrauka.nuotraukos_nuoroda);
            if (System.IO.File.Exists(ServerSavePath))
            {
                System.IO.File.Delete(ServerSavePath);
                bool flag = nuotrauka_Repos.Trinti_Nuotrauka(id);
            }
            return RedirectToAction("GautiNuotraukasRenginio", new { id = nuotrauka.priskirtas_id });
        }
    }
}