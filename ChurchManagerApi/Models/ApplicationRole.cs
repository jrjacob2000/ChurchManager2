using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChurchManagerApi.Models
{
    public class ApplicationRole : IdentityRole
    {

        public string Description { get; set; }


        [Required]
        public bool IsDefaultRole { get; set; }
    }

    
}