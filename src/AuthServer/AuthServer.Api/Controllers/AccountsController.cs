﻿using AuthServer.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AuthServer.Api.Models;
using AuthServer.Infrastructure.Constants;
using System.Threading.Tasks;

namespace AuthServer.Api.Controllers
{
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountsController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("api/[controller]")]
        public async Task<IActionResult> Register([FromBody]RegisterRequestViewModel model)
        {
            //var aVal = 0; var blowUp = 1 / aVal;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser { UserName = model.Email, Name = model.Name, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, Roles.Consumer);
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("userName", user.UserName));
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("name", user.Name));
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("email", user.Email));
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("role", Roles.Consumer));

            return Ok(new RegisterResponseViewModel(user));
        }
    }
}
