using System;
using System.Security.Claims;
using ECommerce.Core.Domain.IdentityEntities;
using ECommerce.Core.DTO;
using ECommerce.Core.Enums;
using ECommerce.Core.ServiceContracts;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Services.Helpers;

namespace ECommerce.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmailSenderService _emailSenderService;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IEmailSenderService emailSenderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSenderService = emailSenderService;
        }

        public async Task<string?> GetUserId(ClaimsPrincipal user)
        {
            if (user == null)
                return null;

           return  _userManager.GetUserId(user);
        }

        public async Task<ApplicationUser?> GetUserByEmail(string email)
        {
            if(email == null) return null;

            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> IsEmailAlereadyRegistered(string email)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
            if (user== null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsUserNameAleradyExist(string userName)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return false;
            return true;
        }

        public async Task<bool> IsUserRoleAdmin(string roleName, LoginRequestt loginRequest)
        {
            if(loginRequest == null)
            {
                throw new ArgumentNullException(nameof(loginRequest));
            }

            if(roleName == null)
                throw new ArgumentException(nameof(roleName));

            if(loginRequest.UserName == null && loginRequest.Password==null) 
                throw new ArgumentException(nameof(loginRequest.Password),nameof(loginRequest.UserName));


            ApplicationUser? user = await _userManager.FindByNameAsync(loginRequest.UserName);

            if (user != null)
            {
                //checking user role 
                if(await _userManager.IsInRoleAsync(user, roleName))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<SignInResult> Login(LoginRequestt loginRequest)
        {
            if(loginRequest  == null) 
                throw new ArgumentNullException(nameof(loginRequest));

            if(loginRequest.Password == null || loginRequest.UserName==null)
                throw new ArgumentException(nameof(loginRequest.Password),nameof(loginRequest.UserName));

            ValidationHelper.ModelValidator(loginRequest);

            //sign in the user 
           SignInResult result =  await _signInManager.PasswordSignInAsync(loginRequest.UserName, loginRequest.Password, isPersistent: false, lockoutOnFailure: false);

            return result;
        }

        public async Task<bool> Login(GoogleJsonWebSignature.Payload payload)
        {
            if(payload == null)
                throw new ArgumentNullException(nameof(payload));

            //sign in the user 
          

           ApplicationUser? user =  await _userManager.FindByEmailAsync(payload.Email);
            if (user != null)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                //If sign in is successfull return true otherwise false
                return _signInManager.Context.User.Identity.IsAuthenticated == true;
            }

            //If sign in is successfull return true otherwise false
            return _signInManager.Context.User.Identity.IsAuthenticated == true;

        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> Register(RegisterRequestt registerRequest)
        {
            if(registerRequest == null)
                throw new ArgumentNullException(nameof(registerRequest));
            if (registerRequest.Password == null || registerRequest.Email == null)
                throw new ArgumentException(nameof(registerRequest.Password), nameof(registerRequest.Email));

            ValidationHelper.ModelValidator(registerRequest);

            ApplicationUser user = registerRequest.ToApplicationUser();

            //Creating user
            IdentityResult result =  await _userManager.CreateAsync(user, registerRequest.Password);

            if (result.Succeeded)
            {
                //checking if user role is created or not in ASPNetRoles Table
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.User.ToString()) is null)
                    {
                        ApplicationRole role = new ApplicationRole()
                        {
                            Name = UserTypeOptions.User.ToString()
                        };

                        //Creating role in ASPNetRoles Table as User role is not exist
                        await _roleManager.CreateAsync(role);

                        // inserting user and it's role in ASPNetUserRoles Table
                        await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());

                        //generating emailConfirmationToken 
                        await GenerateEmailConfirmationToken(user);
                        return result;
                    }
                    else
                    {
                        // inserting user and it's role in ASPNetUserRoles Table
                        await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());

                        //generating emailConfirmationToken 
                        await GenerateEmailConfirmationToken(user);
                    }
            }
            return result;
        }

        public async Task<IdentityResult?> Register(GoogleJsonWebSignature.Payload payload)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));

            ApplicationUser? user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                ApplicationUser newUser = new ApplicationUser()
                {
                    Email = payload.Email,
                    UserName = payload.Name,
                    EmailConfirmed = payload.EmailVerified
                };

                IdentityResult result = await _userManager.CreateAsync(newUser);

                if (result.Succeeded)
                {
                    //checking if user role is created or not in ASPNetRoles Table

                    if (await _roleManager.FindByNameAsync(UserTypeOptions.User.ToString()) is null)
                    {
                        ApplicationRole role = new ApplicationRole()
                        {
                            Name = UserTypeOptions.User.ToString()
                        };

                        //Creating role in ASPNetRoles Table as User role is not exist
                        await _roleManager.CreateAsync(role);

                        // inserting user and it's role in ASPNetUserRoles Table
                        await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());

                        return result;
                    }
                    else
                    {
                        // inserting user and it's role in ASPNetUserRoles Table
                        await _userManager.AddToRoleAsync(newUser, UserTypeOptions.User.ToString());
                    }
                }

                return result;
            }
            return null;
        }

        public async Task GenerateEmailConfirmationToken(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            if (token != null)
            {
                // sending email verification link to user's email id 
                await _emailSenderService.SendEmailConfirmation(user, token);
            }
        }

        public async Task GenerateForgotPasswordToken(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (token != null)
            {
                // sending reset-password link to user's email id 
                await _emailSenderService.SendForgotPasswordEmail(user, token);
            }
        }

        public async Task<IdentityResult> ConfirmEmail(string uid,string token)
        {
            return await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            return await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(resetPasswordDTO.Uid), resetPasswordDTO.Token,resetPasswordDTO.Password);
        }
    }
}
