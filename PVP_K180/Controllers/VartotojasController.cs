using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PVP_K180.Models;
using PVP_K180.ModelView;
using PVP_K180.Repos;

namespace PVP_K180.Controllers
{
    public class VartotojasController : Controller
    {
        Role_Repos role_Repos = new Role_Repos();
        Vartotojas_Repos vartotojas_Repos = new Vartotojas_Repos();
        // GET: Vartotojo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProfilioValdymas()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult AdminValdymas()
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

        public ActionResult ProfilioPerziura(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Vartotojas vartotojas = vartotojas_Repos.Gauti_Vartotoja(id);
            vartotojas.roles_informacija = role_Repos.Gauti_Role(vartotojas.role);
            vartotojas.nuotrauka = Path.Combine(("/Nuotraukos/") + vartotojas.nuotrauka);
            return View(vartotojas);
        }

        public ActionResult RedaguotiDuomenys()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Atnaujinti_email()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Vartotojas vartotojas = vartotojas_Repos.Gauti_Vartotoja(Convert.ToInt32(Session["UserID"]));
            Pasto_duomenys pasto_Duomenys = new Pasto_duomenys();
            pasto_Duomenys.el_pastas = vartotojas.el_pastas;
            return View(pasto_Duomenys);
        }

        [HttpPost]
        public ActionResult Atnaujinti_email(Pasto_duomenys pasto_Duomenys)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            pasto_Duomenys.id_vartotojas = Convert.ToInt32(Session["UserID"]);

            vartotojas_Repos.AtnaujintiEmail(pasto_Duomenys);
            TempData["Succ"] = "Paštas sėkmingai pakeistas!";
            return View(pasto_Duomenys);
        }

        public ActionResult Atnaujinti_Slaptazodi()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Atnaujinti_Slaptazodi(Slaptazodzio_Atnaujinimo_Duomenys pass_Data)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Vartotojas vartotojas = vartotojas_Repos.Gauti_Vartotoja(Convert.ToInt32(Session["UserID"]));

            var decodedOldPass = pass_Data.EncodePassword(pass_Data.senas_slaptazodis);

            if(decodedOldPass == vartotojas.slaptazodis)
            {
                pass_Data.id_Vartotojas = Convert.ToInt32(Session["UserID"]);
                pass_Data.naujas_slaptazodis = pass_Data.EncodePassword(pass_Data.naujas_slaptazodis);
                vartotojas_Repos.AtnaujintiPass(pass_Data);
                TempData["Succ"] = "Slaptažodis sėkmingai pakeistas!";
                return View(pass_Data);
            }
            else
            {
                TempData["Senas slaptažodis netinka!"] = "Slaptažodis sėkmingai pakeistas!";
                return View(pass_Data);
            }

            return View(pass_Data);
        }

        public ActionResult Atnaujinti_Pagrindinius_Duomenis()
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Vartotojas vartotojas = vartotojas_Repos.Gauti_Vartotoja(Convert.ToInt32(Session["UserID"]));
            Atnaujinami_Vartotojo_Duomenys atnaujinami_Vartotojo_Duomenys = new Atnaujinami_Vartotojo_Duomenys();
            atnaujinami_Vartotojo_Duomenys.gimimo_data = vartotojas.gimimo_data;
            atnaujinami_Vartotojo_Duomenys.pavarde = vartotojas.pavarde;
            atnaujinami_Vartotojo_Duomenys.vardas = vartotojas.vardas;
            atnaujinami_Vartotojo_Duomenys.tel_nr = vartotojas.tel_nr;
            return View(atnaujinami_Vartotojo_Duomenys);
        }

        [HttpPost]
        public ActionResult Atnaujinti_Pagrindinius_Duomenis(Atnaujinami_Vartotojo_Duomenys atnaujinami_Vartotojo_Duomenys)
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            atnaujinami_Vartotojo_Duomenys.id_Vartotojas = Convert.ToInt32(Session["UserID"]);

            vartotojas_Repos.AtnaujintiPagrDuomenis(atnaujinami_Vartotojo_Duomenys);

            TempData["Succ"] = "Duomenys sėkmingai pakeisti!";

            return View(atnaujinami_Vartotojo_Duomenys);
        }

        public ActionResult KeistiProfilioNuotrauka()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult KeistiProfilioNuotrauka(Nuotrauku_Duomenys nuotrauku_Duomenys)
        {
            List<string> posiblesExtensions = new List<string>() { ".jpg", ".png", ".JPG", ".PNG", ".jpeg", ".JPEG" };
            nuotrauku_Duomenys.priskirtas_id = Convert.ToInt32(Session["UserID"]);

            int photoCount = 0;
            if(nuotrauku_Duomenys.nuotraukos.Count() > 1)
            {
                TempData["Fail"] = "Gali būti tik viena pridėta nuotrauka";
            }
            foreach (HttpPostedFileBase file in nuotrauku_Duomenys.nuotraukos)
            {
                if (file != null)
                {
                    photoCount++;
                    var InputFileName = Path.GetFileName(file.FileName);
                    var extension = Path.GetExtension(file.FileName);

                    if (!posiblesExtensions.Contains(extension))
                    {
                        TempData["Fail"] = "Įkeltas su netinkamu formatu";
                        return View();
                    }

                    Vartotojas vartotojas = vartotojas_Repos.Gauti_Vartotoja(Convert.ToInt32(Session["UserID"]));

                    var ServerSavePathToDelete = Path.Combine(Server.MapPath("~/Nuotraukos/") + vartotojas.nuotrauka);
                    if (System.IO.File.Exists(ServerSavePathToDelete))
                    {
                        if (vartotojas.nuotrauka != "profile.jpg")
                        {
                            System.IO.File.Delete(ServerSavePathToDelete);
                        }
                    }

                    var random = Guid.NewGuid() + InputFileName;
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Nuotraukos/") + random);


                    file.SaveAs(ServerSavePath);
                    Nuotrauka nuotrauka = new Nuotrauka();
                    nuotrauka.nuotraukos_nuoroda = random;
                    nuotrauka.priskirtas_id = nuotrauku_Duomenys.priskirtas_id;
                    vartotojas_Repos.AtnaujintiNuotrauka(nuotrauka);
                }
            }

            if (photoCount == 0)
            {
                TempData["Fail"] = "Turi būti pasirinkta viena nuotrauka";
                return View();
            }
            else
            {
                TempData["Succ"] = "Profilio nuotrauka sėkmingai atnaujinta";
            }
            return View();

        }

        public ActionResult AktyvuotiNaujienlaiski()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int aktyvacija = vartotojas_Repos.Gauti_Naujienlaiskio_Prenumerata(Convert.ToInt32(Session["UserID"]));

            if(aktyvacija == 1)
            {
                TempData["Aktyvacija"] = true;
            }
            else
            {
                TempData["Aktyvacija"] = false;
            }

            return View();
        }

        public ActionResult AktyvuotiPrenumerata()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int id = Convert.ToInt32(Session["UserID"]);

            vartotojas_Repos.AtnaujintiNaujienlaiskioPrenumerata(id, 1);

            TempData["SuccsessSubs"] = "Sėkmingai užprenumeruotas naujienlaiškis";

            return RedirectToAction("AktyvuotiNaujienlaiski");
        }
        public ActionResult SalintiPrenumerata()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int id = Convert.ToInt32(Session["UserID"]);

            vartotojas_Repos.AtnaujintiNaujienlaiskioPrenumerata(id, 0);
            TempData["SuccsessSubs"] = "Sėkmingai atšaukta naujienlaiškio prenumerata";
            return RedirectToAction("AktyvuotiNaujienlaiski");
        }
    }
}