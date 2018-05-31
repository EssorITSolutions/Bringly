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
    public class UserRole
    {
        public Guid RoleGuid { get; set; }
        [Required(ErrorMessage = "Please enter role")]
        public string RoleName { get; set; }
        public Guid CreatedByGuid { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
