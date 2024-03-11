using Domain.Services;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.HumanResource;
using Azure;
using Domain.Models.Dtos.Auth.Response;
using Domain.Models.Dtos.Auth.Request;
using Domain.Models.ServiceResponses.User.Auth;
namespace Application.Services
{
    public class AuthenticationService// : IAuthenticationService
    {
        DataContext _context;
        public AuthenticationService(DataContext context)
        {
            _context = context;
        }

        //public async Task<SignUpServiceResponse> SignUpUser(SignUpRequest request)
        //{
        //    try
        //    {
        //        return null;
        //    }

        //    catch (Exception)
        //    {
        //       return new SignUpServiceResponse("InternalServerError");
        //    }
        //}

        //public async Task<SignInServiceResponse> SignInUser(SignInRequest request)
        //{
            
        //    try
        //    {
        //        return null;
        //    }
        //    catch (Exception)
        //    {
        //        return new SignInServiceResponse("InternalServerError"); 
        //    }
        //}
    }
}
