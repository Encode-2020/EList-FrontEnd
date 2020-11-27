﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EList_Frontend.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } 
        [Required(ErrorMessage = "Password is required")]
        [UIHint("password")]
        public string Password { get; set; } 
    }
}
