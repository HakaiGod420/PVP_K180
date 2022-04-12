using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PVP_K180.ModelView
{
    public class Slaptazodzio_Atnaujinimo_Duomenys
    {
        public int id_Vartotojas { get; set; }
        [DisplayName("Senas slaptažodis")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Privalo būti įvestas senas slaptažodis")]
        public string senas_slaptazodis { get; set; }

        [DisplayName("Naujas slaptažodis")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Privalo būti įvestas naujas slaptažodis")]
        public string naujas_slaptazodis { get; set; }

        public string EncodePassword(string slaptazodis)
        {

            var data = Encoding.ASCII.GetBytes(slaptazodis);
            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(data);
            string result = Convert.ToBase64String(md5data);
            slaptazodis = result;
            return slaptazodis;
        }
    }
}