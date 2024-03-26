using Domain.Models.ServiceResponses.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.ApiServices
{
    public interface IUserService
    {
        public Task<GetUserDetailsServiceResponse> GetUserDetails(string email);
    }
}
