using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.dtos.Account;
using backend.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var appUser = new AppUser
                {
                    UserName = dto.Username,
                    Email = dto.Email
                };

                if (string.IsNullOrEmpty(dto.Password))
                {
                    return BadRequest("Password is required.");
                }

                var createdUser = await _userManager.CreateAsync(appUser, dto.Password);

                if (!createdUser.Succeeded)
                {
                    return StatusCode(500, createdUser.Errors);
                }

                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                if (!roleResult.Succeeded)
                {
                    return StatusCode(500, roleResult.Errors);
                }

                return Ok("User Created");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}