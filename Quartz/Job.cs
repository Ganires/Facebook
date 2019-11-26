using FacebookService.Facebook;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FacebookService.Quartz
{
    public class Job : IJob
    {
        private readonly IFacebookService _facebookService;
        public Job(IFacebookService facebookService)
        {
            _facebookService = facebookService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            ////Достаем все посты
            var getPostTask = _facebookService.GetPostAsync().Result;
            //// находим все коментарий к постам
            var getcommentTask = _facebookService.GetComments(getPostTask);
            Task.WaitAll(getcommentTask);
            var getAllConversations = _facebookService.GetConversationAsync().Result;
            var getNewmessages = _facebookService.GetMessageFromConversation(getAllConversations).Result;
        }
    }
}

