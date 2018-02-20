using System;
using System.ComponentModel.DataAnnotations;

namespace Bringly.AdminDomain
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Please enter username.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter password.")]
        public string UserPassword { get; set; }
    }
}
