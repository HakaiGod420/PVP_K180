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
    public class AdminController : Controller
    {
        Seniunija_Repos seniunija_Repos = new Seniunija_Repos();
        Vartotojas_Repos vartotojas_Repos = new Vartotojas_Repos();
        public ActionResult AtnaujintiInfoSeniunija()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }

            Seniunija seniunija = seniunija_Repos.Gauti_Seniunija();
            return View(seniunija);
        }

        [HttpPost]
        public ActionResult AtnaujintiInfoSeniunija(Seniunija seniunija)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }

            seniunija_Repos.AtnaujintiSeniunijosInfo(seniunija);
            Response.Write("<script type='text/javascript' language='javascript'> alert('Informacija yra atnaujinta')</script>");
            return View(seniunija);
        }

        public ActionResult VartotojuSarasas()
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }

            List<Vartotojas> vartotojai = vartotojas_Repos.GautiVartotojus();

            if(Convert.ToBoolean(Session["DeleteConfirmation"]))
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Vartotojas sėkmingai ištrintas')</script>");
            }

            return View(vartotojai);
        }

        public ActionResult TrintiVartotoja(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }

            vartotojas_Repos.TrintiVartotoja(id);
            Session["DeleteConfirmation"] = true;
            return RedirectToAction("VartotojuSarasas") ;
        }

    }
}