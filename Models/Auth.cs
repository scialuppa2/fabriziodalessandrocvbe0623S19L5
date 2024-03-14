﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace progetto_settimanaleS19L5.Models
{
    public class Auth
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}