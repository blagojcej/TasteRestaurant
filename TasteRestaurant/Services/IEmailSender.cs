﻿using System.Threading.Tasks;

namespace TasteRestaurant.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
