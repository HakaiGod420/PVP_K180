using System;
using System.Collections.Generic;
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

        public ActionResult ProfilioPerziura()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Vartotojas vartotojas = vartotojas_Repos.Gauti_Vartotoja(Convert.ToInt32(Session["UserID"]));
            vartotojas.roles_informacija = role_Repos.Gauti_Role(vartotojas.role);
            return View(vartotojas);
        }
    }
}