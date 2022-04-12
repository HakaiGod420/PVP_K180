using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using PVP_K180.Models;

namespace PVP_K180.Models
{
    public class Vartotojas
    {
        [DisplayName("Vartotojo ID")]
        public int id_Vartotojas { get; set; }

        [DisplayName("Slapyvardis")]
        [Required(ErrorMessage = "Privalo būti įvestas slapyvardis")]
        public string slapyvardis { get; set; }

        [DisplayName("Slaptažodis")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Privalo būti įvestas slaptažodis")]
        public string slaptazodis { get; set; }
        [DisplayName("Pakartotinai įveskite tą patį slaptažodį")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Privalo būti įvestas pakartotinai slaptažodis")]
        public string re_slaptazodis { get; set; }
        [DisplayName("El. Paštas")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Privalo būti įvestas el. paštas")]
        public string el_pastas { get; set; }
        [DisplayName("Gimimo data")]
        [DisplayFormat(DataFormatString = "{0:yyyy-dd-MM}", ApplyFormatInEditMode = true)]
        public DateTime? gimimo_data { get; set; }

        [DisplayName("Profilio nuotrauka")]
        public string nuotrauka { get; set; }

        [DisplayName("Vardas")]
        public string vardas { get; set; }
        [DisplayName("Pavardė")]
        public string pavarde { get; set; }
        [DisplayName("Telefono numeris")]
        public string tel_nr { get; set; }

        [DisplayName("Profilio sukurimo data")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? sukurimo_data { get; set; }

        [DisplayName("Role")]
        public int role { get; set; }

        [DisplayName("Rolės informacija")]
        public Role roles_informacija { get; set; }

        [DisplayName("Vartotojo būsena")]
        public int? vartotojo_busena { get; set; }


        public void EncodePassword()
        {

            var data = Encoding.ASCII.GetBytes(this.slaptazodis);
            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(data);
            string result = Convert.ToBase64String(md5data);
            this.slaptazodis = result;
        }

    }
}