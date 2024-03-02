using Domain.Dtos.Auth.Request;
using Domain.ServiceResponse.User.Auth;
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
using Domain.Dtos.Auth.Response;
namespace Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        DataContext _context;
        public AuthenticationService(DataContext context)
        {
            _context = context;
        }

        public async Task<SignUpServiceResponse> SignUpUser(SignUpRequest request)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Email == request.Email.ToLower()))
                {
                    return new SignUpServiceResponse("UserAlredyExists");
                    
                }

                await _context.Users.AddAsync(new User()
                {
                    Name = request.Name,
                    Email = request.Email.ToLower(),
                    Password = request.Password,
                });

                await _context.SaveChangesAsync();

                return new SignUpServiceResponse("Success");
            }

            catch (Exception)
            {
               return new SignUpServiceResponse("InternalServerError");
            }
        }

        public async Task<SignInServiceResponse> SignInUser(SignInRequest request)
        {
            
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email.ToLower());

                if (user == null || user.Password != request.Password)
                {
                    return new SignInServiceResponse("InvalidUserData");
                }

                 return new SignInServiceResponse("Success")
                {
                    UserData = new SignInResponse()
                    {
                        Name = user.Name,
                        Email = user.Email,
                        Token = "token"
                    }
                };

            }
            catch (Exception)
            {
                return new SignInServiceResponse("InternalServerError"); 
            }
        }
    }
}
