﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.Core.Dtos.Request
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Username or email is required")]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Mot de passe
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
