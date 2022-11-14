﻿using Microsoft.AspNetCore.Identity;

namespace SkillSystem.IdentityServer4.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}