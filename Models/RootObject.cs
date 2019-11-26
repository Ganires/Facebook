using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookService.Models
{
    public class RootObject
    {
        public string created_time { get; set; }
        public From from { get; set; }
        public string message { get; set; }
        public string id { get; set; }
    }
}
