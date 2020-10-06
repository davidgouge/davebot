using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DaveBot
{
    public static class DaveBot
    {
        [FunctionName("DaveBot")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            
            if(requestBody.Contains("challenge"))
            {
                var challengeObj = JObject.Parse(requestBody);
                return new OkObjectResult(new {challenge = challengeObj["challenge"]});
            }

            var message = JsonConvert.DeserializeObject<Message>(requestBody);

            return new OkResult();
        }
    }

    public class Message
    {
        public Event Event {get;set;}
    }

    public class Event
    {
        public string User {get;set;}
        public string Type {get;set;}
        public Block[] Blocks {get;set;}
    }

    public class Block
    {
        public Element[] Elements {get;set;}
        
    }

    public class Element
    {
        public Element[] Elements {get;set;}
        public string Type {get;set;}
        public string Text {get;set;}
        [JsonProperty("user_id")]
        public string UserId {get;set;}
    }
}
