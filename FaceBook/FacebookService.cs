using FacebookService.ApiConfig;
using FacebookService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FacebookService.Facebook
{
    public interface IFacebookService
    {
        Task<List<string>> GetPostAsync();
        Task<Dictionary<object, object>> GetComments(List<string> ListofPost);
        Task<List<string>> GetConversationAsync();
        Task<List<Message>> GetMessageFromConversation(List<string> ListOfConversation);
        Task PostOnWallAsync(string Converstationid, string Message);
    }
    public class FacebookApiService : IFacebookService
    {
        private readonly IFacebookHttpClient _facebookClient;
        public FacebookApiService(IFacebookHttpClient facebookClient)
        {
            _facebookClient = facebookClient;
            //accessToken = parseJsons.AccessToken.Value;
        }
     
        public async Task<Dictionary<object, object>> GetComments(List<string> listofPost)
        {

           Dictionary<object, object> CommentDic = new Dictionary<object, object>();
            foreach (string post in listofPost)
            {
                var result = await _facebookClient.GetAsync<dynamic>(
                post + "/comments", "fields=comment_count,from,message");
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
                    JToken jcommentcount = jObject["comment_count"];
                    JToken jid = jObject["id"];
                    if (jmessage != null && jmessage.ToString().Contains("#Senim109") && jcommentcount.ToString() =="0")
                    {
                        //Убираем # чтобы больше не отвечать на данный комментарий
                        string updatedComment  = jmessage.ToString().Replace('#', ' ');
                        updateComment.message = updatedComment;
                        var updateOnWallTask =  _facebookClient.PostAsync( jid + "/", new { updateComment.message });
                        //ответ на комментарий
                        from.messageId = jid.ToString();
                        rootobj.from = from;
                        rootobj.created_time = jcreated_time.ToString();
                        rootobj.message = jmessage.ToString();

                        var appealToSend = new AppealToSend();
                        appealToSend.NickName ="";
                        appealToSend.PostId = "";
                        appealToSend.Message = jmessage.ToString();
                        appealToSend.MessageId = jid.ToString();
                      
                        //string jsonString = JsonConvert.SerializeObject(appealToSend).ToString();
                        //var pageRequestJson = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        //var client = new HttpClient();
                        //var url = ReadJson.ReplaceJsonValue();
                        //dynamic parseJson = JsonConvert.DeserializeObject(url);
                        //var urlMainProject = parseJson.url.Value;

                        var responce = await _facebookClient.SendToOpenCity<JObject>(appealToSend);
                        var appealStringResult = JObject.Parse(result.ToString());
                        

                        MessageToSend.message = "Уважаемый,житель г.Караганды Ваше обращение было зарегистрировано под номером " + appealStringResult + " от " + System.DateTime.Now;
                        var updatecommentTask = _facebookClient.PostAsync( jid + "/comments", new { MessageToSend.message });

                        CommentDic.Add(from, rootobj);
                    }
                }
            }
            return await Task.FromResult(CommentDic);
        }

        public async Task<List<string>> GetPostAsync()
        {
            var result = await _facebookClient.GetAsync<dynamic>(
                 "me/posts");

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
        public async Task<List<string>> GetConversationAsync()
        {
            var result = await _facebookClient.GetAsync<dynamic>(
                 "me/conversations");
            var conversationData = (IEnumerable<object>)result["data"];
            var conversationIdList = new List<string>();
            foreach (var item in conversationData)
            {
                var item_To_String = item.ToString();
                JObject jObject = JObject.Parse(item_To_String);
                JToken jUser = jObject["id"];
                conversationIdList.Add(jUser.ToString());
            }

            return conversationIdList;
        }


        public async Task PostOnWallAsync(string converstationid, string message)
            => await _facebookClient.PostAsync( converstationid+"/messages", new { String.Empty });

        public async Task<List<Message>> GetMessageFromConversation(List<string> listOfConversation)
        {
            List<Message> messages = new List<Message>();
            foreach (string conversationId in listOfConversation)
            {
                Message mess = new Message();
                var result = await _facebookClient.GetAsync<dynamic>(
                      conversationId+ "/messages", "fields=message,from");
                var MessageData = (IEnumerable<object>)result["data"];
                //Возвражает скоп данные. Надо пройтись по каждому отдельно.
                foreach (var currentMessage in MessageData)
                {
                    var messageString = currentMessage.ToString();
                    JObject jObject = JObject.Parse(messageString);
                    JToken jmessage = jObject["message"];
                    JToken jfrom = jObject["from"];
                    var userCommentfrom = (string)jfrom["name"];
                    var MessageList = new List<string>();
                    if (userCommentfrom != "Senim 109")
                    {
                        mess.From = userCommentfrom;
                        mess.message = jmessage.ToString();
                        mess.conversationId = conversationId;
                        messages.Add(mess);

                        var appealToSend = new AppealToSend();
                        appealToSend.NickName = (string)jfrom["name"];
                        appealToSend.PostId = "";
                        appealToSend.Message = jmessage.ToString();
                        appealToSend.MessageId = conversationId.ToString();


                        string jsonString = JsonConvert.SerializeObject(appealToSend).ToString();
                        var pageRequestJson = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        var client = new HttpClient();
                        //var url = ReadJson.ReplaceJsonValue();
                       // dynamic parseJson = JsonConvert.DeserializeObject(url);
                        //var urlMainProject = parseJson.Url.Value;
                        //HttpResponseMessage responce = await client.PostAsync(urlMainProject, pageRequestJson);
                        //var AppealIdResult = responce.Content.ReadAsStringAsync().Result;
                       // string AppealIdString = "CC" + Regex.Replace(AppealIdResult, @"[^0-9.]", "");

                        MessageToSend.message = "Уважаемый(ая)," + (string)jfrom["name"] + " Ваше обращение было зарегистрировано под номером " 
                        + "" + " от " + System.DateTime.Now;
                        var postOnWallTask = _facebookClient.PostAsync(conversationId + "/messages", new { MessageToSend.message });

                    }
                    break;
                }
            }
            return await Task.FromResult(messages);
        }
    }
}
