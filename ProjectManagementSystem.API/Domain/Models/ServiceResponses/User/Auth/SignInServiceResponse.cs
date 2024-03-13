﻿using Domain.Models.Dtos.Auth.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.User.Auth
{
    public class SignInServiceResponse(string message)
    {
        public string Message { get; set; } = message.ToString();
        public string Token { get; set; }
        public string RefrshToken { get; set; }
    }
    public static class SignInServiceResponseMessages
    {
        public const string Success = "Success";
        public const string InvalidUserCredentials = "InvalidUserCredentials";
        public const string InternalError = "InternalError";
    }
}