using System;
using System.Security.Claims;
using ECommerce.Core.Domain.IdentityEntities;
using ECommerce.Core.DTO;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Core.ServiceContracts
{
    public interface IAuthService
    {
        /// <summary>
        /// Log in the user in application
        /// </summary>
        /// <param name="loginRequest">User details to Login</param>
        /// <returns>Returns SignInResult</returns>
        Task<SignInResult> Login(LoginRequestt loginRequest);

        Task<bool> Login(GoogleJsonWebSignature.Payload payload);

        /// <summary>
        /// Log out the user 
        /// </summary>
        /// <returns></returns>
        Task Logout();

        /// <summary>
        /// Register the user   
        /// </summary>
        /// <param name="registerRequest">User details to register</param>
        /// <returns>Returns IdenityResult object which tells user creation is successfull or not</returns>
        Task<IdentityResult> Register(RegisterRequestt registerRequest);

        Task<IdentityResult?> Register(GoogleJsonWebSignature.Payload payload);

        Task<IdentityResult> ConfirmEmail(string uid, string token);

        Task<IdentityResult> ResetPassword(ResetPasswordDTO resetPasswordDTO);

        Task GenerateEmailConfirmationToken(ApplicationUser user);

        Task GenerateForgotPasswordToken(ApplicationUser user);

        /// <summary>
        /// Checks if the email alerady exist or not 
        /// </summary>
        /// <param name="email">email to check</param>
        /// <returns>Returns true if exists; otherwise false</returns>
        Task<bool> IsEmailAlereadyRegistered(string email);

        /// <summary>
        /// Checks if the userName alerady exist or not
        /// </summary>
        /// <param name="userName">userName to check</param>
        /// <returns>Returns true if exists; otherwise false</returns>
        Task<bool> IsUserNameAleradyExist(string userName);

        /// <summary>
        /// Checks whether the user is in admin role or not
        /// </summary>
        /// <param name="roleName">The role name</param>
        /// <param name="loginRequest">The user to check</param>
        /// <returns>Returns true if role is admin; otherwise false</returns>

        Task<bool> IsUserRoleAdmin(string roleName,LoginRequestt loginRequest);

        Task<string?> GetUserId(ClaimsPrincipal user);

        Task<ApplicationUser?> GetUserByEmail(string email);
    }
}
