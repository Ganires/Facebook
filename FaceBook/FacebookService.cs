using FaceBook;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public interface IFacebookService
{
    Task<List<string>> GetPostAsync(string accessToken);
    Task<Dictionary<object, object>> GetComments(List<string> listofPost, string accessToken);
}

public class FacebookService : IFacebookService
{
    private readonly IFacebookClient _facebookClient;

    public FacebookService(IFacebookClient facebookClient)
    {
        _facebookClient = facebookClient;
    }

    public async Task<Dictionary<object, object>> GetComments(List<string> listofPost, string accessToken)
    {

        Dictionary<object, object> CommentDic = new Dictionary<object, object>(); 
        foreach (string post in listofPost)
        {
            var result = await _facebookClient.GetAsync<dynamic>(
            accessToken, post + "/comments", "");
            var PostData = (IEnumerable<object>)result["data"];
            foreach (var item in PostData)
            {
                From from = new From();
                RootObject rootobj = new RootObject();
                var item_To_String = item.ToString();
                JObject jObject = JObject.Parse(item_To_String);
                JToken jUser = jObject["from"];
                JToken jmessage = jObject["message"];
                JToken jcreated_time = jObject["created_time"];
                if (jUser != null && jmessage != null)
                {
                    from.id = (string)jUser["id"];
                    from.name = (string)jUser["name"];
                    rootobj.from = from;
                    rootobj.created_time = jcreated_time.ToString();
                    // rootobj.id = (string)jmessage["id"];
                    rootobj.message = jmessage.ToString();
                    CommentDic.Add(from, rootobj);
                }
            }

        }
        return await Task.FromResult(CommentDic);
    }

    public async Task<List<string>> GetPostAsync(string accessToken)
    {
        var result = await _facebookClient.GetAsync<dynamic>(
            accessToken, "me/posts", "");
        var PostData = (IEnumerable<object>)result["data"];
        var PostIdList = new List<string>();
        foreach (var item in PostData)
        {
            var location = item.ToString();
            var splitString = location.Split("id");
            string id = Regex.Replace(splitString[1], @"[^0-9_]", "");
            PostIdList.Add(id);
        }

        return PostIdList;
    }

}