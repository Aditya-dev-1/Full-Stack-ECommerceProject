using System.Threading.Tasks;
using ECommerce.Core.Domain.IdentityEntities;
using ECommerce.Core.DTO;
using ECommerce.Core.Enums;
using ECommerce.Core.ServiceContracts;
using ECommerceApplication.Controllers;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.UI.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly SignInManager<ApplicationUser> _signInManager; 

        public AccountController(IAuthService authService, SignInManager<ApplicationUser> signInManager)
        {
            _authService = authService;
            _signInManager = signInManager;
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Register(RegisterRequestt registerRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage);
                return View(registerRequest);
            }

           IdentityResult result =  await _authService.Register(registerRequest);

            //if creation of user is successfull
            if (result.Succeeded)
            {
               ApplicationUser user =  registerRequest.ToApplicationUser();
                await _signInManager.PasswordSignInAsync(user.UserName, user.PasswordHash, isPersistent: false, lockoutOnFailure: false);
                return RedirectToAction(nameof(AccountController.ConfirmEmail), new {email = user.Email});
            }

            else
            {
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
                return View(registerRequest);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login(LoginRequestt loginRequest,string? ReturnUrl=null)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage);
                return View(loginRequest);
            }

            var result =  await _authService.Login(loginRequest);
            bool user = await _authService.IsUserRoleAdmin(UserTypeOptions.Admin.ToString(), loginRequest);

            
            //checking if sign in is successfull or not
            if(result.Succeeded)
            {
                //checking if the role of user is admin or not
                if(user)
                {
                    //if admin then redirecting him to admin area
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }

                //checking if return url is null or not
                if(ReturnUrl != null && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }
                return RedirectToAction(nameof(ProductsController.Index), "Products");
            }

            //cheking if user is allowed to login or not(if user does not verified his email id then he is not allowed to login first he has to confirm his identity) 
            if (result.IsNotAllowed)
            {
                ModelState.AddModelError("Login", "Login is Not Allowed");
                return View(loginRequest);
            }

            //if username or password is wrong

            ModelState.AddModelError("Login", "Invalid UserName or Password");
            return View(loginRequest);
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _authService.Logout();
            return RedirectToAction(nameof(ProductsController.Index), "Products");
        }


        [HttpGet]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token,string email)
        {
            EmailDTO emailConfirmationDTO = new EmailDTO()
            {
                Email = email
            };

            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                var result = await _authService.ConfirmEmail(uid, token);
                if (result.Succeeded)
                {
                    emailConfirmationDTO.IsEmailVerified = true;
                }
            }
            return View(emailConfirmationDTO);
        }

        [HttpPost]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailDTO emailConfirmationDTO)
        {
            var user = await _authService.GetUserByEmail(emailConfirmationDTO.Email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    emailConfirmationDTO.IsEmailVerified = true;
                    return View(emailConfirmationDTO);
                }
                else
                {
                    await _authService.GenerateEmailConfirmationToken(user);
                    emailConfirmationDTO.IsEmailSent = true;
                }
            }
            else
            {
                ModelState.AddModelError("","Something Went Wrong Try Again");
            }
            return View(emailConfirmationDTO);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ForgotPassword(EmailDTO emailDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage);
                return View(emailDTO);
            }
            var user = await _authService.GetUserByEmail(emailDTO.Email);
            if (user != null)
            {
                await _authService.GenerateForgotPasswordToken(user);
                emailDTO.IsEmailSent = true;
            }
            
            return View(emailDTO);
        }

        [HttpGet]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(string uid,string token)
        {
            ResetPasswordDTO resetPasswordDTO = new ResetPasswordDTO()
            {
                Uid = uid,
                Token = token
            };
            return View(resetPasswordDTO);
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage);
                return View(resetPasswordDTO);
            }

            resetPasswordDTO.Token = resetPasswordDTO.Token.Replace(' ', '+');
            var result = await _authService.ResetPassword(resetPasswordDTO);

            if (result.Succeeded)
            {
                resetPasswordDTO.IsPasswordChangedSuccessfully = true;
                return View(resetPasswordDTO);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(resetPasswordDTO);
        }



        [Route("[action]")] 
        //Broswer will make asynchronous request to check email eneterd by user is alerady exist or not. 
        
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
           bool result =  await _authService.IsEmailAlereadyRegistered(email);
            if (!result)
            {
                return Json(true); //email id is valid 
            }
            return Json(false); // email id is alerady in use
        }

        [Route("[action]")]
        //Broswer will make asynchronous request to check UserName eneterd by user is alerady exist or not. 
        public async Task<IActionResult> IsUserNameAleradyExist(string userName)
        {
            bool result = await _authService.IsUserNameAleradyExist(userName);
            if (!result)
            {
                return Json(true); //Username  is valid 
            }
            return Json(false); // UserName is alerady in use
        }

        
    }
}
