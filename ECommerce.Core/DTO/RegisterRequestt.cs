using System;
using System.ComponentModel.DataAnnotations;
using ECommerce.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Core.DTO
{
    public class RegisterRequestt
    {

        [Required(ErrorMessage ="UserName can't be blank")]
        [Length(5,10,ErrorMessage = "User Name should be between 5 to 10 characters")]
        [RegularExpression("^[a-zA-Z0-9_]*$",ErrorMessage ="UserName should only contains digits , alphabets and underscore")]
        [Remote(action: "IsUserNameAleradyExist", controller:"Account",ErrorMessage ="UserName is already taken")]
        public string? UserName { get; set; }


        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }


        [Required(ErrorMessage = "Confirm Password can't be blank")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password and Confirm Password do not match")]
        public string? ConfirmPassword { get; set; }


        [Required(ErrorMessage = "Email can't be blank")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Email should be valid")]
        [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email id is already in use")]
        public string? Email { get; set; }


        [Required(ErrorMessage = "Phone Number can't be blank")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits.")]
        [RegularExpression("^[0-9]*$",ErrorMessage ="Phone Number should contains only digits")]
        public string? PhoneNumber { get; set; }

        public ApplicationUser ToApplicationUser()
        {
            return new ApplicationUser()
            {
                UserName = UserName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                PasswordHash = Password
            };
        }

    }
}
