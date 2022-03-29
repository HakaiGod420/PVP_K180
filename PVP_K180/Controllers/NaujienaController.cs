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
    public class NaujienaController : Controller
    {
        Naujiena naujiena = new Naujiena();
        Nuotrauka_Repos nuotrauka_Repos = new Nuotrauka_Repos();


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
        public ActionResult NaujienosKurimas(Naujienos_duomenys naujienos_Duomenys)
        {
            List<string> posiblesExtensions = new List<string>() { ".jpg", ".png", ".JPG", ".PNG", ".jpeg", ".JPEG" };
            Naujiena_Repos naujiena_Repos = new Naujiena_Repos();
            naujienos_Duomenys.naujiena.naujienos_kurejo_ID = (int)Session["UserID"];
            naujienos_Duomenys.naujiena.naujienos_sukurimo_data = DateTime.Now;
            bool flag = naujiena_Repos.Sukurti_Naujiena(naujienos_Duomenys.naujiena);
            if (flag)
            {
                int lastIndex = naujiena_Repos.Gauti_Paskutini_Prideto_Index();

                foreach(HttpPostedFileBase file in naujienos_Duomenys.nuotraukos)
                {
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);

                        var extension = Path.GetExtension(file.FileName);
                        if (!posiblesExtensions.Contains(extension))
                        {
                            Response.Write("<script type='text/javascript' language='javascript'> alert('Įkeltas su netinkamu formatu')</script>");
                            return View();
                        }

                        var random = Guid.NewGuid() + InputFileName;
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Nuotraukos/") + random);


                        file.SaveAs(ServerSavePath);
                        Nuotrauka nuotrauka = new Nuotrauka();
                        nuotrauka.nuotraukos_nuoroda = random;
                        nuotrauka.priskirtas_id = lastIndex;
                        nuotrauka_Repos.Prideti_Naujienos_Nuotraukas(nuotrauka);

                    }
                }

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
            if (Session["Message"] != null)
            {
                if (Convert.ToBoolean(Session["Message"]))
                {
                    Response.Write("<script type='text/javascript' language='javascript'> alert('Nuotrauka pridėta sėkmingai!')</script>");
                    Session.Remove("Message");
                }
                else
                {
                    Response.Write("<script type='text/javascript' language='javascript'> alert('Nuotrauka pridėta nesėkmingai!')</script>");
                    Session.Remove("Message");      
                }
            }
            Naujiena_Repos naujiena_Repos = new Naujiena_Repos();
            List<Naujiena> naujienos = naujiena_Repos.Gauti_Naujienas();
            return View(naujienos);
        }

        public ActionResult GautiNuotraukas(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }

            Nuotrauka_Repos nuotrauka_Repos = new Nuotrauka_Repos();
            List<Nuotrauka> nuotraukos = nuotrauka_Repos.Gauti_Nuotraukas(id);
            foreach (var item in nuotraukos)
            {
                var path = Path.Combine(("/Nuotraukos/") + item.nuotraukos_nuoroda);
                item.nuotraukos_nuoroda = path;
            }

            return View(nuotraukos);

        }


        public ActionResult TrintiNaujiena(int id)
        {
            Naujiena_Repos naujiena_Repos = new Naujiena_Repos();
            bool flag = naujiena_Repos.Trinti_Naujiena(id);
            return RedirectToAction("GautiNaujienas");
        }

        public ActionResult TrintiNuotrauka(int id)
        {
            Nuotrauka_Repos nuotrauka_Repos = new Nuotrauka_Repos();
            Nuotrauka nuotrauka = nuotrauka_Repos.Gauti_Nuotrauka(id);
            var ServerSavePath = Path.Combine(Server.MapPath("~/Nuotraukos/") + nuotrauka.nuotraukos_nuoroda);
            if (System.IO.File.Exists(ServerSavePath))
            {
                System.IO.File.Delete(ServerSavePath);
                bool flag = nuotrauka_Repos.Trinti_Nuotrauka(id);
            }
            return RedirectToAction("GautiNuotraukas", new { id = nuotrauka.priskirtas_id });
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

        public ActionResult PridetiNuotrauku(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }

            Nuotrauku_Duomenys nuotrauku_Duomenys = new Nuotrauku_Duomenys();
            Session["NaujienosID"] = id;
            return View(nuotrauku_Duomenys);
        }

        [HttpPost]
        public ActionResult PridetiNuotrauku(Nuotrauku_Duomenys nuotrauku_Duomenys)
        {
            List<string> posiblesExtensions = new List<string>() {".jpg", ".png", ".JPG", ".PNG", ".jpeg", ".JPEG"};
            nuotrauku_Duomenys.priskirtas_id = Convert.ToInt32(Session["NaujienosID"]);
            Session.Remove("NaujienosID");

            int photoCount = 0;
            foreach (HttpPostedFileBase file in nuotrauku_Duomenys.nuotraukos)
            {
                if (file != null)
                {
                    photoCount++;
                    var InputFileName = Path.GetFileName(file.FileName);
                    var extension = Path.GetExtension(file.FileName);

                    if(!posiblesExtensions.Contains(extension))
                    {
                        Response.Write("<script type='text/javascript' language='javascript'> alert('Įkeltas su netinkamu formatu')</script>");
                        return View();
                    }

                    var random = Guid.NewGuid() + InputFileName;
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Nuotraukos/") + random);


                    file.SaveAs(ServerSavePath);
                    Nuotrauka nuotrauka = new Nuotrauka();
                    nuotrauka.nuotraukos_nuoroda = random;
                    nuotrauka.priskirtas_id = nuotrauku_Duomenys.priskirtas_id;
                    nuotrauka_Repos.Prideti_Naujienos_Nuotraukas(nuotrauka);


                }
            }

            if(photoCount == 0)
            {
                Session["Message"] = false;
            }
            else
            {
                Session["Message"] = true;
            }


            return RedirectToAction("GautiNaujienas");

        }
    }
}