using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PVP_K180.Models;
using PVP_K180.Repos;
using PVP_K180.ModelView;

namespace PVP_K180.Controllers
{
    public class AtsitikimasController : Controller
    {
        Atsitikimas_Repos atsitikimas_Repos = new Atsitikimas_Repos();

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

            TempData["AtsitikmasSuccsess"] = "Atsitikimas sėkmingai pridėtas";
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


        public ActionResult AtsitikimuIstorija()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Atsitikimas> atsitikimai = atsitikimas_Repos.GautiAtsitikimus();
            return View(atsitikimai.Where(x=>x.fk_Vartotojasid_Pranesejas==Convert.ToInt32(Session["UserID"])).ToList());
        }
    }
}