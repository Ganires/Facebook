using System;
using System.Threading.Tasks;

namespace FaceBook
{
    class Program
    {
        static void Main(string[] args)
        {
            // to do создать метод для  лонг лив токен. 
            var accessToken = "";//User access Token
            var facebookClient = new FacebookClient();
            var facebookService = new FacebookService(facebookClient);
            var getAccountTask = facebookService.GetPostAsync(accessToken).Result;
            var getcommentTask = facebookService.GetComments(getAccountTask, accessToken).Result;
            foreach (var dicValue in getcommentTask)
            {
                var from = dicValue.Key;
                var message = dicValue.Value;
                System.Reflection.PropertyInfo piname = from.GetType().GetProperty("name");
                String name = (String)(piname.GetValue(from, null));
                System.Reflection.PropertyInfo piid = from.GetType().GetProperty("id");
                String id = (String)(piid.GetValue(from, null));

                Console.WriteLine("комментарий от "+ name.ToString() + " " +"Комментарий ID"+ id.ToString());
                System.Reflection.PropertyInfo pimessage = message.GetType().GetProperty("message");
                
                String comment = (String)(pimessage.GetValue(message, null));

                Console.WriteLine(" Kомментарий " + comment.ToString());


            }
  }
    }
}
