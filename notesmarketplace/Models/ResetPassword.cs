using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace notesmarketplace.Models
{
    public class ResetPassword
    {
        public String OldPassword { get; set; }
        public String NewPassword { get; set; }
        [Compare("NewPassword",ErrorMessage ="Password is not matched")]
        public String ConfirmNewPassword { get; set; }
    }
}