﻿using System;
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
        Apklausa apklausa = new Apklausa();
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

        [HttpGet]
        public ActionResult DalyvautiApklausoje()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ApklausosAtsakymai apklausosAtsakymai =  new ApklausosAtsakymai();
            int activeQuestioner = apklausa_Repos.Gauti_Aktyvia_Apklausa();
            apklausosAtsakymai.apklausa = apklausa_Repos.Gauti_Apklausa(activeQuestioner);
            apklausosAtsakymai.klausimai = apklausa_Repos.GautiKlausimus(activeQuestioner);
            var checkIfAnswered = apklausa_Repos.Patikrinti_Ar_Atsake(Convert.ToInt32(Session["UserID"]), activeQuestioner);
            List<string> atsakymai = new List<string>();
            
            if (checkIfAnswered)
            {
                TempData["Answered"] = true;
                
                foreach(var item in apklausosAtsakymai.klausimai)
                {
                    atsakymai.Add(apklausa_Repos.GautiAtsakyma(Convert.ToInt32(Session["UserID"]), item.id_Klausimas));
                }
            }
            else
            {
                TempData["Answered"] = false;
            }
            apklausosAtsakymai.gautiAtsakymai = atsakymai;
            return View(apklausosAtsakymai);
        }

        [HttpPost]
        public ActionResult DalyvautiApklausoje(ApklausosAtsakymai apklausosAtsakymai)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int activeQuestioner = apklausa_Repos.Gauti_Aktyvia_Apklausa();
            apklausosAtsakymai.klausimai = apklausa_Repos.GautiKlausimus(activeQuestioner);

            for(var i = 0; i < apklausosAtsakymai.atsakymai.Count;i++)
            {
                Atsakymas atsakymas = new Atsakymas();
                atsakymas.atsakymo_tekstas = apklausosAtsakymai.atsakymai[i];
                atsakymas.fk_Klausimasid_Klausimas = apklausosAtsakymai.klausimai[i].id_Klausimas;
                atsakymas.fk_Vartotojasid_Varotojas = Convert.ToInt32(Session["UserID"]);
                if (atsakymas.atsakymo_tekstas != "")
                {
                    apklausa_Repos.Prideti_Atsakyma(atsakymas);
                }
            }
            apklausa_Repos.Atzymeti_Dalyvavima(Convert.ToInt32(Session["UserID"]), activeQuestioner);
            TempData["SuccsessAtsakimas"] = "Sėkmingai atsakyta į apklausą";
            apklausa_Repos.AtnaujintiApklausuAtsakusiuSkaiciu(activeQuestioner);
            return RedirectToAction("DalyvautiApklausoje");
        }


        public ActionResult RedaguotiApklausa(int id)
        {
            Apklausos_Repos apklausos_Repos = new Apklausos_Repos();
            apklausa = apklausos_Repos.Gauti_Apklausa(id);
            return View(apklausa);
        }

        [HttpPost]
        public ActionResult RedaguotiApklausa(int id, Apklausa apklausa)
        {
            Apklausos_Repos apklausos_Repos = new Apklausos_Repos();
            apklausa.fk_Vartotojasid_Sukurejas = (int)Session["UserID"];
            apklausa.sukurimo_data = DateTime.Now;
            apklausa.id_Apklausa = id;
            bool flag = apklausos_Repos.Redaguoti_Apklausa(apklausa);
            if (flag)
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Apklausa sėkmingai redaguota!')</script>");
            }
            else
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Apklausa neredaguota!')</script>");

            }
            return View(apklausa);
        }



        public ActionResult TrintiApklausa(int id)
        {
            Apklausos_Repos apklausos_Repos = new Apklausos_Repos();
            bool flag = apklausos_Repos.Trinti_Apklausa(id);
            return RedirectToAction("ZiuretiApklausas");
        }

    }
}