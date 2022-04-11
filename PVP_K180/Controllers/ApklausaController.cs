using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PVP_K180.Models;
using PVP_K180.ModelView;
using PVP_K180.Repos;
using System.Web.Mvc;

namespace PVP_K180.Controllers
{
    public class ApklausaController : Controller
    {
        Apklausos_Repos apklausa_Repos = new Apklausos_Repos();


        public ActionResult KurtiApklausa()
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
        public ActionResult KurtiApklausa(Apklausa apklausa)
        {
            if (apklausa.klausimai != null)
            {
                var count = 0;
                foreach (var item in apklausa.klausimai)
                {
                    if (item.klausimo_tekstas != null)
                    {
                        count++;
                    }
                }
                if (count <= 1)
                {
                    TempData["FailApklausa"] = "Apklausos klausimų turi būti daugiau nei vienas";
                    return View();
                }
            }
            else
            {
                TempData["FailApklausa"] = "Apklausos klausimų turi būti daugiau nei vienas";
                return View();
            }

            apklausa.sukurimo_data = DateTime.Now;
            apklausa.fk_Vartotojasid_Sukurejas = Convert.ToInt32(Session["UserID"]);
            apklausa.busena = 1;
            apklausa_Repos.Sukurti_Apklausa(apklausa);
            var last_id = apklausa_Repos.Gauti_Paskutini_Prideto_Index();

            foreach (var item in apklausa.klausimai)
            {
                item.fk_Apklausa_Apklausa = last_id;

                apklausa_Repos.Prideti_Klausima(item);
            }
            TempData["SuccsessApklausa"] = "Apklausa sėkmingai sukurtas";
            return View();
        }

        public ActionResult ZiuretiApklausas()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }

            List<Apklausa> apklausos = apklausa_Repos.GautiApklausas();
            return View(apklausos);

        }
    }
}