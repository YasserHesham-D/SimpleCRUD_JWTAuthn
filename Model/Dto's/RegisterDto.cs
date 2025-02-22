﻿using System.ComponentModel.DataAnnotations;

namespace SimpleCRUD_JWTAuthn.Model.Dto_s
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
