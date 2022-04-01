using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PVP_K180.Repos;
using PVP_K180.Models;
using PVP_K180.ModelView;

namespace PVP_K180.Controllers
{
    public class HomeController : Controller
    {
        Vartotojas_Repos vartotojas_Repos = new Vartotojas_Repos();
        Role_Repos role_Repos = new Role_Repos();
        public ActionResult Index()
        {
            if (Session["Message"] != null)
            {
                if (Session["Message"].Equals("Registracija"))
                {
                    Response.Write("<script type='text/javascript' language='javascript'> alert('Paskyra sėkmingai buvo sukurta')</script>");
                }
                else if (Session["Message"].Equals("Prisijungimas"))
                {
                    Response.Write("<script type='text/javascript' language='javascript'> alert('Sėkmingai prisijungta prie paskyros')</script>");
                }
                else if (Session["Message"].Equals("Atsijungti"))
                {
                    Response.Write("<script type='text/javascript' language='javascript'> alert('Sėkmingai atsijungta nuo paskyros')</script>");
                    Session.Clear();
                }

                Session.Remove("Message");
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Registracija()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index");
            }
            Vartotojas vartotojas = new Vartotojas();
            return View(vartotojas);
        }

        [HttpPost]
        public ActionResult Registracija(Vartotojas vartotojas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    vartotojas.sukurimo_data = DateTime.Now;
                    vartotojas.role = 2;

                    if (vartotojas.slaptazodis != vartotojas.re_slaptazodis)
                    {
                        Response.Write("<script type='text/javascript' language='javascript'> alert('Slaptažodžiai nesutampa')</script>");
                        return View();
                    }
                    vartotojas.EncodePassword();
                    vartotojas.nuotrauka = "profile.jpg";
                    vartotojas_Repos.Registruoti_Vartotoja(vartotojas);


                }
            }
            catch (Exception ex)
            {

            }
            Session["Message"] = "Registracija";
            return RedirectToAction("Index");
        }

        public ActionResult Prisijungti()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Prisijungti(Prisijungimo_Duomenys vartotojas_prisijungimo_duomenys)
        {
            vartotojas_prisijungimo_duomenys.EncodePassword();
            var patikrintiDuomenys = vartotojas_Repos.Patikrinti_Duomenys(vartotojas_prisijungimo_duomenys);

            if (patikrintiDuomenys == true)
            {
                int id = vartotojas_Repos.Gauti_Vartotojo_ID(vartotojas_prisijungimo_duomenys);
                if (id != -1)
                {
                    Vartotojas vartotojas = vartotojas_Repos.Gauti_Vartotoja(id);
                    Session["UserID"] = id;
                    Session["UserName"] = vartotojas.slapyvardis;
                    Session["Role"] = role_Repos.Gauti_Role(vartotojas.role).name;

                    Session["Message"] = "Prisijungimas";
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Notification = "Neteisingi prisijungimo duomenys";
                    return View();
                }
            }
            else
            {
                ViewBag.Notification = "Neteisingi prisijungimo duomenys";
                return View();
            }
        }

        public ActionResult Atsijungti()
        {
            Session["Message"] = "Atsijungti";
            return RedirectToAction("Index");
        }
    }
}