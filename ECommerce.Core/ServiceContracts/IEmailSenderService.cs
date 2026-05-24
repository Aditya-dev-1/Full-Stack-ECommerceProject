using System;
using ECommerce.Core.Domain.IdentityEntities;

namespace ECommerce.Core.ServiceContracts
{
    public interface IEmailSenderService
    {
        Task SendEmailConfirmation(ApplicationUser user, string token);

        Task SendForgotPasswordEmail(ApplicationUser user, string token);
    }
}
