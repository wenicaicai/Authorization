﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BasicAuthentication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthorizationService _authorizationService;

        public HomeController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        [Authorize(Policy = "Claim.Mile")]
        public IActionResult SecretPolicy()
        {
            return View("Secret");
        }

        //[Authorize(Roles = "Claim.Mile")]
        [Authorize(Roles = "Admin")]
        public IActionResult SecretRoles()
        {
            return View("Secret");
        }

        public IActionResult Authenticate()
        {
            //1.
            var grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "BWhite"),
                new Claim(ClaimTypes.Email,"BWhite@cmail.com"),
                new Claim(ClaimTypes.DateOfBirth,"11/11/2000"),
                new Claim(ClaimTypes.Role,"Manager"),
                new Claim("Grandma.Says","BWhite is a good boy.")
            };

            //2.
            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"BWhite Yang"),
                new Claim("DrivingLicense","A+")
            };

            var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");

            //这个是什么用途
            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, licenseIdentity });
            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DoStuff()
        {
            var builder = new AuthorizationPolicyBuilder("Schema");
            var customPolicy = builder.RequireClaim("Hello").Build();

            var authResult = await _authorizationService.AuthorizeAsync(User, customPolicy);
            if (authResult.Succeeded)
            { 
            
            }
            return View("Index");
        }

    }
}