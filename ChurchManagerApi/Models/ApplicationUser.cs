using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ChurchManagerApi.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public bool IsAccountOwner { get; set; }

        public bool ChangePasswordOnFirstLogin { get; set; }
    }
    
   
}