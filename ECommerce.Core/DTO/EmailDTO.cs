using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Core.DTO
{
    public class EmailDTO
    {
        [Required(ErrorMessage = "Email can't be blank")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email should be valid")]
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsEmailSent { get; set; }
    }
}
