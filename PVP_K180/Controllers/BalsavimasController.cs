using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PVP_K180.Models;
using PVP_K180.ModelView;
using PVP_K180.Repos;

namespace PVP_K180.Controllers
{
    public class BalsavimasController : Controller
    {
        Balsavimas_Repos balsavimas_Repos = new Balsavimas_Repos();
 

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
                var count = 0;
                foreach (var item in balsavimas.balsavimo_variantai)
                {
                    if (item.balsavimo_variantas != null)
                    {
                        count++;
                    }
                }
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

            if(Session["Message"]!=null)
            {
                if(Convert.ToString(Session["Message"]) == "DeleteSucc")
                {
                    Response.Write("<script type='text/javascript' language='javascript'> alert('Balsavimas sėkmingai ištrintas')</script>");
                    Session.Remove("Message");
                }
                else
                {
                    Session.Remove("Message");
                    Response.Write("<script type='text/javascript' language='javascript'> alert('Balsavimas  neištrintas')</script>");
                }
            }

            List <Balsavimas> balsavimai = balsavimas_Repos.GautiBalsavimus();
            return View(balsavimai);

        }

        public ActionResult TrintiBalsavima(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }

            bool flag = balsavimas_Repos.Trinti_Balsavima(id);

            if(flag)
            {
                Session["Message"] = "DeleteSucc";
            }
            else
            {
                Session["Message"] = "DeleteFail";
            }

            return RedirectToAction("ZiuretiBalsavimus");
        }

        public ActionResult RedaguotiBalsavima(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }

            Balsavimas balsavimas = balsavimas_Repos.Gauti_Balsavima(id);
            balsavimas.balsavimo_variantai = balsavimas_Repos.GautiVariantus(id);

            return View(balsavimas);
        }

        [HttpPost]
        public ActionResult RedaguotiBalsavima(int id,Balsavimas balsavimas)
        {
            balsavimas.id_Balsavimas = id;
            balsavimas_Repos.Atnaujinti_Balsavima(balsavimas);


            if(balsavimas.balsavimo_variantai != null)
            {
                var count = 0;
                foreach(var item in balsavimas.balsavimo_variantai)
                {
                    if (item.balsavimo_variantas != null)
                    {
                        count++;
                    }
                }
                if(count > 1)
                {
                    List<Balsavimo_Variantas> temp = balsavimas_Repos.GautiVariantus(id);

                    foreach(var item in temp)
                    {
                        balsavimas_Repos.Trinti_Variantus(item.id_Balsavimo_Variantas);
                    }

                    foreach(var item in balsavimas.balsavimo_variantai)
                    {
                        item.fk_Balsavimasid_Balsavimas = id;
                        balsavimas_Repos.Prideti_Varianta(item);
                    }
                }
                else
                {
                    Response.Write("<script type='text/javascript' language='javascript'> alert('Balsavime turi būti bent du pasirinkimai')</script>");
                    return View(balsavimas);
                }
            }
            else
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Balsavime turi būti bent du pasirinkimai')</script>");
                return View(balsavimas);
            }
            Response.Write("<script type='text/javascript' language='javascript'> alert('Balsavimas sėkmingai redaguotas')</script>");
            return View(balsavimas);
        }

        public ActionResult KeistiBalsavimoBusena(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }

            BalsavimoBusenaPerziura balsavimoBusenaPerziura = new BalsavimoBusenaPerziura();
            UzpildytiDuomenis(balsavimoBusenaPerziura);
            balsavimoBusenaPerziura.balsavimo_Busena = balsavimas_Repos.Gauti_Balsavima(id).busena;
            return View(balsavimoBusenaPerziura);
        }

        [HttpPost] 
        public ActionResult KeistiBalsavimoBusena(int id, BalsavimoBusenaPerziura  balsavimoBusenaPerziura)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }
            UzpildytiDuomenis(balsavimoBusenaPerziura);

            var count = balsavimas_Repos.GautiAktyviuSkaiciu();

            if(balsavimoBusenaPerziura.balsavimo_Busena == 2)
            {
                if(count >= 1)
                {
                    Response.Write("<script type='text/javascript' language='javascript'> alert('Aktyvus balsavimas gali būti tik vienas')</script>");
                    return View(balsavimoBusenaPerziura);
                }
            }

            balsavimas_Repos.Atnaujinti_Balsavimo_Busena(id, balsavimoBusenaPerziura.balsavimo_Busena);
            Response.Write("<script type='text/javascript' language='javascript'> alert('Balsavimo būsena pakeista')</script>");

            return View(balsavimoBusenaPerziura);
        }


        [HttpGet]
        public ActionResult Balsuoti()
        {
            int acviveVote = balsavimas_Repos.Gauti_Aktyvu_Balsavima();
            Balsavimas balsavimas = balsavimas_Repos.Gauti_Balsavima(acviveVote);
            balsavimas.balsavimo_variantai = balsavimas_Repos.GautiVariantus(balsavimas.id_Balsavimas);

            if(Session["UserID"] != null)
            {
                var check = balsavimas_Repos.PatikrintiPasirinkima(acviveVote, Convert.ToInt32(Session["UserID"]));

                if(check)
                {
                    TempData["Voted"] = true;
                    TempData["ChosenNumb"] = balsavimas_Repos.Gauti_Pasirinkta_Varianta(Convert.ToInt32(Session["UserID"]));
                }
                else
                {
                    TempData["Voted"] = false;
                }
            }

            return View(balsavimas);
        }

        public ActionResult RinktisVarianta(int id)
        {

            Varianto_Pasirinkimas varianto_Pasirinkimas = new Varianto_Pasirinkimas();
            varianto_Pasirinkimas.fk_Balsavimo_Variantasid_Balsavimo_Variantas = id;
            varianto_Pasirinkimas.fk_Vartotojasid_Vartotojas = Convert.ToInt32(Session["UserID"]);
            Balsavimo_Variantas variantas = balsavimas_Repos.GautiVarianta(id);
            varianto_Pasirinkimas.fk_Balsavimo_Id = variantas.fk_Balsavimasid_Balsavimas;
            balsavimas_Repos.Prideti_Pasirinkta_Varianta(varianto_Pasirinkimas);
            balsavimas_Repos.AtnaujintiBalsavimoSkaiciu(varianto_Pasirinkimas.fk_Balsavimo_Id);
            balsavimas_Repos.AtnaujintiVariantoSkaiciu(varianto_Pasirinkimas.fk_Balsavimo_Variantasid_Balsavimo_Variantas);
            TempData["SuccessVote"] = "Sėkmingai buvo užbalsuota";
            return RedirectToAction("Balsuoti");
        }


        public void UzpildytiDuomenis(BalsavimoBusenaPerziura balsavimoBusenaPerziura)
        {
            var busenos = balsavimas_Repos.GautiBusenas();
            IList<SelectListItem> busenosList = new List<SelectListItem>();

            foreach(var item in busenos)
            {
                busenosList.Add(new SelectListItem { Value = item.id_Balsavimo_busena.ToString(), Text = item.name });
            }
            balsavimoBusenaPerziura.Busenos = busenosList;
        }

        public ActionResult Rezultatai(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }

            Balsavimas balsavimas = balsavimas_Repos.Gauti_Balsavima(id);
            balsavimas.balsavimo_variantai = balsavimas_Repos.GautiVariantus(id);

            BalsavimuDiagrama diagrama = new BalsavimuDiagrama();

            foreach(var item in balsavimas.balsavimo_variantai)
            {
                var random = new Random(Guid.NewGuid().GetHashCode());
                var color = String.Format("#{0:X6}", random.Next(0x1000000)); // = "#A197B9"
                diagrama.labels.Add(item.balsavimo_variantas);
                diagrama.colors.Add(color);
                diagrama.values.Add(item.pasirinkusiu_skaicius);
            }

            balsavimas.diagramosData = diagrama;
            return View(balsavimas);
        }
    }
}