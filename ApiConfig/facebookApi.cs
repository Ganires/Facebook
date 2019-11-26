using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookService.ApiConfig
{
    public class FacebookApi 
    {
        private readonly IConfiguration _configuration;
        public FacebookApi(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    
    }
}
