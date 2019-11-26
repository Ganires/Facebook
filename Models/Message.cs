using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookService.Models
{
    public class Message
    {
        public string message { get; set; }
        public string From { get; set; }
        public string conversationId { get; set; }
    }
}
