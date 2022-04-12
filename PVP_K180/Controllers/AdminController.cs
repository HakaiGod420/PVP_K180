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
        Role_Repos roles_Repos = new Role_Repos();
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

        public ActionResult KeistiBusena(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }

            VartotojoBusenosPerziura vartotojoBusenosPerziura = new VartotojoBusenosPerziura();
            UzpildytiBusenas(vartotojoBusenosPerziura);
            vartotojoBusenosPerziura.vartotojo_busena = vartotojas_Repos.Gauti_Vartotoja(id).vartotojo_busena;
            if(vartotojoBusenosPerziura.vartotojo_busena == null)
            {
                vartotojoBusenosPerziura.vartotojo_busena = 0;
            }
            return View(vartotojoBusenosPerziura);
        }

        [HttpPost]
        public ActionResult KeistiBusena(int id, VartotojoBusenosPerziura vartotojoBusenosPerziura)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }
            UzpildytiBusenas(vartotojoBusenosPerziura);

            if(vartotojoBusenosPerziura.vartotojo_busena == 0)
            {
                vartotojoBusenosPerziura.vartotojo_busena = null;
                vartotojas_Repos.AtnaujintiVartotojoBusena(id, vartotojoBusenosPerziura.vartotojo_busena);
                Response.Write("<script type='text/javascript' language='javascript'> alert('Vartotojo būsena pakeista')</script>");
                vartotojoBusenosPerziura.vartotojo_busena = 0;
                return View(vartotojoBusenosPerziura);
            }
            vartotojas_Repos.AtnaujintiVartotojoBusena(id, vartotojoBusenosPerziura.vartotojo_busena);
            Response.Write("<script type='text/javascript' language='javascript'> alert('Vartotojo būsena pakeista')</script>");

            return View(vartotojoBusenosPerziura);
        }


        public ActionResult KeistiRole(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }

            VartotojoRolesPerziura vartotojoRolesPerziura = new VartotojoRolesPerziura();
            UzpildytiRoles(vartotojoRolesPerziura);
            vartotojoRolesPerziura.vartotojo_role = vartotojas_Repos.Gauti_Vartotoja(id).role;
            return View(vartotojoRolesPerziura);
        }

        [HttpPost]
        public ActionResult KeistiRole(int id, VartotojoRolesPerziura vartotojoRolesPerziura)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }
            UzpildytiRoles(vartotojoRolesPerziura);
            vartotojas_Repos.AtnaujintiVartotojoRole(id, vartotojoRolesPerziura.vartotojo_role);
            Response.Write("<script type='text/javascript' language='javascript'> alert('Vartotojo rolė pakeista')</script>");

            return View(vartotojoRolesPerziura);
        }


        public void UzpildytiBusenas(VartotojoBusenosPerziura vartotojoBusenosPerziura)
        {
            var busenos = vartotojas_Repos.GautiBusenas();
            IList<SelectListItem> busenosList = new List<SelectListItem>();
            busenosList.Add(new SelectListItem { Value = "0", Text = "Nenurodyta" });
            foreach (var item in busenos)
            {
                busenosList.Add(new SelectListItem { Value = item.id_Vartotojo_Busena.ToString(), Text = item.name });
            }
            vartotojoBusenosPerziura.Busenos = busenosList;
        }

        public void UzpildytiRoles(VartotojoRolesPerziura vartotojoRolesPerziura)
        {
            var roles = roles_Repos.GautiRoles();
            IList<SelectListItem> rolesList = new List<SelectListItem>();
            foreach (var item in roles)
            {
                rolesList.Add(new SelectListItem { Value = item.id_Role.ToString(), Text = item.name });
            }
            vartotojoRolesPerziura.role = rolesList;
        }

    }
}