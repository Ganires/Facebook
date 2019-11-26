using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookService.Models
{
    public class AppealToSend
    {
        [JsonProperty("Message")]
        public string Message { get; set; }
        [JsonProperty("PostId")]
        public string PostId { get; set; }
        [JsonProperty("NickName")]
        public string NickName { get; set; }
        [JsonProperty("MessageId")]
        public string MessageId { get; set; }
    }
}
