using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PVP_K180.Models;
using PVP_K180.ModelView;
using PVP_K180.Repos;
using System.IO;
using System.Net;
using EASendMail;
using SmtpClient = EASendMail.SmtpClient;

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

            if(Convert.ToDouble(TempData["SeniunijaLang"]) == 0 || Convert.ToDouble(TempData["SeniunijaLong"]) == 0)
            {
                Response.Write("<script type='text/javascript' language='javascript'> alert('Turite patvirtinti teisingą lokaciją')</script>");
                return View(seniunija);
            }

            seniunija.zemelapis_ilguma = (float)Convert.ToDouble(TempData["SeniunijaLang"]);
            seniunija.zemelapis_platuma = (float)Convert.ToDouble(TempData["SeniunijaLong"]);
            seniunija_Repos.AtnaujintiSeniunijosInfo(seniunija);
            Response.Write("<script type='text/javascript' language='javascript'> alert('Informacija yra atnaujinta')</script>");
            return View(seniunija);
        }

        [HttpPost]
        public void IsaugotiLokacija(float x, float y)
        {
            TempData["SeniunijaLang"] = x;
            TempData["SeniunijaLong"] = y;
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

        public ActionResult SiustiNaujienlaiski()
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
        public ActionResult SiustiNaujienlaiski(EmaiModel model)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!Session["Role"].Equals("Administratorius"))
            {
                return RedirectToAction("Index", "Home");
            }

            if(!ModelState.IsValid)
            {
                return View();
            }

            List<string> emails = vartotojas_Repos.GautiEmailSub();

            try
            {
                SmtpMail oMail = new SmtpMail("TryIt");

                if(model.Attachment.Length > 0)
                {
                    foreach(var item in model.Attachment)
                    {
                        string fileName = Path.GetFileName(item.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Nuotraukos/") + fileName);
                        item.SaveAs(ServerSavePath);
                        oMail.AddAttachment(ServerSavePath);
                    }
                }

                // Set sender email address, please change it to yours
                oMail.From = "noreply@k180.vhost.lt";

                // Set email subject
                oMail.Subject = model.Subject;

                // Set email body
                oMail.HtmlBody = model.Body;

                // Your SMTP server address
                SmtpServer oServer = new SmtpServer("mail.k180.vhost.lt");

                // User and password for ESMTP authentication, if your server doesn't require
                // User authentication, please remove the following codes.
                oServer.User = "noreply@k180.vhost.lt";
                oServer.Password = "1721420858!";

                // Set 465 SMTP port
                oServer.Port = 465;

                // Enable SSL connection
                oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;


                SmtpClient oSmtp = new SmtpClient();
                

                foreach(var item in emails)
                {
                    oMail.To.Add(item);
                    
                }

                oSmtp.SendMail(oServer, oMail);
                if (model.Attachment.Length > 0)
                {
                    foreach (var item in model.Attachment)
                    {
                        string fileName = Path.GetFileName(item.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Nuotraukos/") + fileName);
                        item.SaveAs(ServerSavePath);
                        if (System.IO.File.Exists(ServerSavePath))
                        {
                            System.IO.File.Delete(ServerSavePath);
                        }
                    }
                }

                TempData["Succsess"] = "Laiškai sėkmingai išiųsti " + emails.Count() + " vartotojams";
            }
            catch (Exception ep)
            {
                throw ep;
            }
            return View();
        }

    }
}