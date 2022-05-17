using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PVP_K180.Models;
using PVP_K180.Repos;
using PVP_K180.ModelView;
using System.IO;

namespace PVP_K180.Controllers
{
    public class AtsitikimasController : Controller
    {
        Atsitikimas_Repos atsitikimas_Repos = new Atsitikimas_Repos();
        Nuotrauka_Repos nuotrauka_Repos = new Nuotrauka_Repos();

        public ActionResult SkelbtiAtsitikima()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Atsitikimas atsitikimas = new Atsitikimas();
            UzpildytiAtsitikimoTipus(atsitikimas);

            return View(atsitikimas);
        }

        [HttpPost]
        public ActionResult SkelbtiAtsitikima(Atsitikimas atsitikimas)
        {
            List<string> posiblesExtensions = new List<string>() { ".jpg", ".png", ".JPG", ".PNG", ".jpeg", ".JPEG" };

            UzpildytiAtsitikimoTipus(atsitikimas);
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if(atsitikimas.atsitikimo_tipas == 0)
            {
                TempData["AtsitikmasFail"] = "Turite Pasirinkti tipą";
                return View(atsitikimas);
            }

            if (Convert.ToDouble(TempData["AtsitikimasLang"]) == 0 || Convert.ToDouble(TempData["AtsitikimasLong"]) == 0)
            {
                TempData["AtsitikmasFail"] = "Turi būti pažymėta tiksli lokacija";
                return View(atsitikimas);
            }

            atsitikimas.paskelbimo_data = DateTime.Now;

            atsitikimas.fk_Vartotojasid_Pranesejas = Convert.ToInt32(Session["UserID"]);

            atsitikimas.zemelapio_ilguma = Convert.ToDouble(TempData["AtsitikimasLang"]);
            atsitikimas.zemelapio_platuma = Convert.ToDouble(TempData["AtsitikimasLong"]);
            atsitikimas.atsitikimo_busena = 1;

            atsitikimas_Repos.PridetiAtsitikima(atsitikimas);
            var lastIndex = atsitikimas_Repos.Gauti_Paskutini_Prideto_Index();

            TempData["AtsitikmasSuccsess"] = "Atsitikimas sėkmingai pridėtas";


            foreach (HttpPostedFileBase file in atsitikimas.nuotraukos)
            {
                if (file != null)
                {
                    var InputFileName = Path.GetFileName(file.FileName);

                    var extension = Path.GetExtension(file.FileName);
                    if (!posiblesExtensions.Contains(extension))
                    {
                        TempData["AtsitikmasFail"] = "Kai kurios nuotraukos įkeltus su netinkamu formatu";
                        continue;
                    }

                    var random = Guid.NewGuid() + InputFileName;
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Nuotraukos/") + random);


                    file.SaveAs(ServerSavePath);
                    Nuotrauka nuotrauka = new Nuotrauka();
                    nuotrauka.nuotraukos_nuoroda = random;
                    nuotrauka.priskirtas_id = lastIndex;
                    nuotrauka_Repos.Prideti_Atsitikimo_Nuotraukas(nuotrauka);

                }
            }

            return View(atsitikimas);
        }

        public void UzpildytiAtsitikimoTipus(Atsitikimas atsitikimas)
        {
            var atsitikimai = atsitikimas_Repos.GautiAtsitikimoTipus();
            IList<SelectListItem> tipaiList = new List<SelectListItem>();
            foreach (var item in atsitikimai)
            {
                tipaiList.Add(new SelectListItem { Value = item.id_Atsitikimas_Tipas.ToString(), Text = item.name });
            }
            atsitikimas.atsitikimo_tipai = tipaiList;
        }

        [HttpPost]
        public void IsaugotiLokacija(float x, float y)
        {
            TempData["AtsitikimasLang"] = x;
            TempData["AtsitikimasLong"] = y;
        }


        public ActionResult AtsitikimuIstorija(int? busena,int? tipas, DateTime? nuo, DateTime? iki)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Atsitikimas> atsitikimai = atsitikimas_Repos.GautiAtsitikimus();
            TempData["AdminMenu"] = false;
            if(busena != null)
            {
                atsitikimai = atsitikimai.Where(x => x.atsitikimo_busena == Convert.ToInt32(busena)).ToList();
            }
            if(tipas != null)
            {
                atsitikimai = atsitikimai.Where(x => x.atsitikimo_tipas == Convert.ToInt32(tipas)).ToList();
            }
            if (nuo != null)
            {
                var data = (DateTime)nuo;
                atsitikimai = atsitikimai.Where(x => x.paskelbimo_data.Date >= (DateTime)data.Date).ToList();
            }

            if (iki != null)
            {
                var data = (DateTime)iki;
                atsitikimai = atsitikimai.Where(x => x.paskelbimo_data.Date <= (DateTime)data.Date).ToList();
            }
            return View(atsitikimai.Where(x=>x.fk_Vartotojasid_Pranesejas==Convert.ToInt32(Session["UserID"])).ToList());
        }

        public ActionResult AtsitikimuSarasas(int? busena, int? tipas, DateTime? nuo, DateTime? iki)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<Atsitikimas> atsitikimai = atsitikimas_Repos.GautiAtsitikimus();
            TempData["AdminMenu"] = true;
            if (busena != null)
            {
                atsitikimai = atsitikimai.Where(x => x.atsitikimo_busena == Convert.ToInt32(busena)).ToList();
            }
            if (tipas != null)
            {
                atsitikimai = atsitikimai.Where(x => x.atsitikimo_tipas == Convert.ToInt32(tipas)).ToList();
            }
            if (nuo != null)
            {
                var data = (DateTime)nuo;
                atsitikimai = atsitikimai.Where(x => x.paskelbimo_data.Date >= (DateTime)data.Date).ToList();
            }

            if (iki != null)
            {
                var data = (DateTime)iki;
                atsitikimai = atsitikimai.Where(x => x.paskelbimo_data.Date <= (DateTime)data.Date).ToList();
            }
            return View(atsitikimai);
        }

        public ActionResult DetaliInformacija(int id)
        {
            Atsitikimas atsitikimas = atsitikimas_Repos.Gauti_Atsitikima(id);
            atsitikimas.gautosNuotraukos = nuotrauka_Repos.Gauti__Atsitikimu_Nuotraukas(id);
            return View(atsitikimas);
        }

        public ActionResult KeistiAtsitikimoBusena(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }
            TempData["idAtsitikimas"] = id;
            AtsitikimuKeitimasPerziura atsitikimas = atsitikimas_Repos.Gauti_Atsitikimo_Perziura(id);
            UzpildytiAtsitikimoBusenas(atsitikimas);
            
            return View(atsitikimas);
        }

        [HttpPost]
        public ActionResult KeistiAtsitikimoBusena(AtsitikimuKeitimasPerziura atsitikimas)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }
            atsitikimas.tvirtintojas = Convert.ToInt32(Session["UserID"]);
            atsitikimas.id_Atstikimas = Convert.ToInt32(TempData["idAtsitikimas"]);
            atsitikimas_Repos.Atnaujinti_Atsitikimo_Busena(atsitikimas);
            UzpildytiAtsitikimoBusenas(atsitikimas);
            TempData["SuccsessAtstikimas"] = "Atsitikimas sėkmingai atnaujintas";   
            return View(atsitikimas);
        }

        public void UzpildytiAtsitikimoBusenas(AtsitikimuKeitimasPerziura atsitikimas)
        {
            var atsitikimai = atsitikimas_Repos.GautiAtsitikimoBusenas();
            IList<SelectListItem> busenosList = new List<SelectListItem>();
            foreach (var item in atsitikimai)
            {
                busenosList.Add(new SelectListItem { Value = item.id_Atsitikimas_Busena.ToString(), Text = item.name });
            }
            atsitikimas.atsitikimo_busenos = busenosList;
        }

    }
}