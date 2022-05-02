using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PVP_K180.Repos;
using PVP_K180.Models;

namespace PVP_K180.Controllers
{
    public class RenginysController : Controller
    {
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
        public ActionResult RenginioKurimas(Renginys renginys)
        {
            Renginys_Repos renginys_Repos = new Renginys_Repos();
            renginys.vartotojas_id = (int)Session["UserID"];
            renginys.paskelbimo_data = DateTime.Now;
            bool flag = renginys_Repos.Sukurti_Rengini(renginys);
            if (flag)
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Renginys sėkmingai sukurtas!')</script>");
            }
            else
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Renginys nesukurtas!')</script>");
            }
            return View();
        }

        public ActionResult GautiRenginius(Renginys rasti_rengini)
        {
            
            Renginys_Repos renginys_Repos = new Renginys_Repos();
            List<Renginys> renginys = renginys_Repos.RastiRenginius(rasti_rengini);
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
    }
}