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
    public class NaujienaController : Controller
    {
        Naujiena naujiena = new Naujiena();


        // GET: Naujiena
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NaujienosKurimas()
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
        public ActionResult NaujienosKurimas(Naujiena naujiena)
        {
            Naujiena_Repos naujiena_Repos = new Naujiena_Repos();
            naujiena.naujienos_kurejo_ID = (int)Session["UserID"];
            naujiena.naujienos_sukurimo_data = DateTime.Now;
            bool flag = naujiena_Repos.Sukurti_Naujiena(naujiena);
            if (flag)
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Naujiena sėkmingai sukurta!')</script>");
            }
            else
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Naujiena nesukurta!')</script>");

            }
            return View();
        }

        public ActionResult GautiNaujienas()
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }
            Naujiena_Repos naujiena_Repos = new Naujiena_Repos();
            List<Naujiena> naujienos = naujiena_Repos.Gauti_Naujienas();
            return View(naujienos);
        }


        public ActionResult TrintiNaujiena(int id)
        {
            Naujiena_Repos naujiena_Repos = new Naujiena_Repos();
            bool flag = naujiena_Repos.Trinti_Naujiena(id);
            return RedirectToAction("GautiNaujienas");
        }

        public ActionResult RedaguotiNaujiena(int id)
        {
            Naujiena_Repos naujiena_Repos = new Naujiena_Repos();
            naujiena = naujiena_Repos.Gauti_Naujiena(id);
            return View(naujiena);
        }

        [HttpPost]
        public ActionResult RedaguotiNaujiena(Naujiena naujiena)
        {
            Naujiena_Repos naujiena_Repos = new Naujiena_Repos();
            naujiena.naujienos_kurejo_ID = (int)Session["UserID"];
            naujiena.naujienos_sukurimo_data = DateTime.Now;
            bool flag = naujiena_Repos.Redaguoti_Naujiena(naujiena);
            if (flag)
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Naujiena sėkmingai redaguota!')</script>");
            }
            else
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Naujiena neredaguota!')</script>");

            }
            return View(naujiena);
        }
    }
}