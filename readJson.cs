using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookService
{
    public static  class ReadJson
    {
        public static string ReplaceJsonValue()
        {
            string filepath = @"..\FacebookService\appsettings.json";
            string result = string.Empty;
            //using (StreamReader r = new StreamReader(filepath))
            //{
            //    var json = r.ReadToEnd();
            //    var jobj = JObject.Parse(json);
            //    result = jobj.ToString();
            //}
            return result;
        }
    }

}
