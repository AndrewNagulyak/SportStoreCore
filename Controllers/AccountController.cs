using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportStoreCore.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace SportStoreCore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userMgr,
            SignInManager<IdentityUser> signinMgr)
        {
            userManager = userMgr;
            signInManager = signinMgr;

        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginModel {ReturnUrl = returnUrl});
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel details)
        {

            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByNameAsync(details.login) ??
                                    await userManager.FindByEmailAsync(details.login);
                if (user != null)
                {
                    SignInResult result = await signInManager.PasswordSignInAsync(user, details.password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(details.ReturnUrl ?? "/");
                    }
                }
            }

            ModelState.AddModelError(nameof(LoginModel.login),
                "Invalid user or password");

            return View(details);
        }
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLogin()
        {

            string redirectUrl = Url.Action("GoogleResponse", "Account", new { ReturnUrl = ViewBag.returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(("Google"), redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
        {
            ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, false);
            if (result.Succeeded)
            {
                return Redirect(returnUrl);

            }
            else
            {
                IdentityUser user = new IdentityUser
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName =
                        info.Principal.FindFirst(ClaimTypes.Email).Value
                };
                IdentityResult identResult = await userManager.CreateAsync(user);
                if (identResult.Succeeded)
                {
                    identResult = await userManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {
                        await signInManager.SignInAsync(user, false);
                        return Redirect(returnUrl);
                    }
                }

                return AccessDenied(returnUrl);
            }
        }


        [AllowAnonymous]
        public IActionResult Registration(string returnUrl)
        {
            return View(new RegistrationModel(){ReturnUrl = returnUrl});
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            return RedirectToAction("Logout");
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registration(RegistrationModel details)
        {
            await signInManager.SignOutAsync();
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByEmailAsync(details.email);
                if (user == null)
                {
                    user = await userManager.FindByNameAsync(details.login);
                    if (user == null)
                    {
                        IdentityUser user1 = new IdentityUser() {UserName = details.login, Email = details.email,};
                        
                       await userManager.CreateAsync(user1,details.password);
                       
                       await signInManager.SignInAsync(user1,false);

                    }
                    else
                    {
                        ModelState.AddModelError("", "User already exist");

                    }
                }
                else
                {
                    ModelState.AddModelError("","Mail already exist");
                }
               
            }
            return Redirect(details.ReturnUrl??"/");
        }

    public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

    }
}