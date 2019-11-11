using System;
using System.Collections.Generic;
using System.Text;

namespace FaceBook
{
    public class Account
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Locale { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
    }
    public class Posts
    { 
        public int Id { get; set; }
    }
    public class From
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class RootObject
    {
        public string created_time { get; set; }
        public From from { get; set; }
        public string message { get; set; }
        public string id { get; set; }
    }
}
