using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PVP_K180.ModelView
{
    public class EmaiModel
    {
        [Display(Name = "Tema")]
        [Required(ErrorMessage = "Privalo būti įvesta tema")]
        public string Subject { get; set; }
        [Display(Name = "Turinys")]
        [Required(ErrorMessage = "Privalo būti įvestas tūrinys")]
        [AllowHtml]
        public string Body { get; set; }

        [Display(Name = "Pridėti failai")]
        public HttpPostedFileBase[] Attachment { get; set; }
    }
}