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
    public class Prisijungimo_Duomenys
    {
        [DisplayName("Slapyvardis")]
        [Required(ErrorMessage = "Privalo būti įvestas slapyvardis")]
        public string slapyvardis { get; set; }


        [DisplayName("Slaptažodis")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Privalo būti įvestas slaptažodis")]
        public string slaptazodis { get; set; }


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