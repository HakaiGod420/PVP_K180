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
    public class BalsavimasController : Controller
    {
        Balsavimas_Repos balsavimas_Repos = new Balsavimas_Repos();
        // GET: Balsavimas
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult KurtiBalsavima()
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
        public ActionResult KurtiBalsavima(Balsavimas balsavimas)
        {
            if(balsavimas.balsavimo_variantai != null)
            {
                var count = balsavimas.balsavimo_variantai.Count;
                if (count <= 1)
                {
                    Response.Write("<script type='text/javascript' language='javascript'> alert('Balsavimų variantų turi būti daugiau nei vienas')</script>");
                    return View();
                }
            }
            else
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Balsavimų variantų turi būti daugiau nei vienas')</script>");
                return View();
            }
            balsavimas.sukurimo_data = DateTime.Now;
            balsavimas.fk_Vartotojasid_Sukurejas = Convert.ToInt32(Session["UserID"]);
            balsavimas.busena = 1;
            balsavimas_Repos.Sukurti_Balsavima(balsavimas);
            var last_id = balsavimas_Repos.Gauti_Paskutini_Prideto_Index();

            foreach(var item in balsavimas.balsavimo_variantai)
            {
                item.fk_Balsavimasid_Balsavimas = last_id;

                balsavimas_Repos.Prideti_Varianta(item);
            }
            Response.Write("<script type='text/javascript' language='javascript'> alert('Balsavimas sėkmingai sukurtas')</script>");
            return View();
        }

        public ActionResult ZiuretiBalsavimus()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }

            List<Balsavimas> balsavimai = balsavimas_Repos.GautiBalsavimus();
            return View(balsavimai);

        }
    }
}