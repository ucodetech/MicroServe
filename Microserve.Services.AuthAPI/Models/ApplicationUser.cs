﻿using Microsoft.AspNetCore.Identity;

namespace Microserve.Services.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set;}
    }
}
