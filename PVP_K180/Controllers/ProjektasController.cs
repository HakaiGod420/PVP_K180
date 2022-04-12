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
    public class ProjektasController : Controller
    {

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
        public ActionResult ProjektoKurimas(Projektas projektas)
        {
            Projektas_Repos projektas_Repos = new Projektas_Repos();
            projektas.fk_Vartotojasid_Vartotojas = (int)Session["UserID"];
            bool flag = projektas_Repos.Sukurti_Projekta(projektas);

            if (true)
            {
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
    }
}