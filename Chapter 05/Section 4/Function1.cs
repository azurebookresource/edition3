using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionAppC5S4
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            var TK = context.CallActivityAsync<string>("Function1_Hello", "Tokyo");
            var ST = context.CallActivityAsync<string>("Function1_Hello", "Seattle");
            var LD = context.CallActivityAsync<string>("Function1_Hello", "London");

            var myTasks = new List<Task> { TK, ST, LD };
            while (myTasks.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(myTasks);
                if (finishedTask == TK)  outputs.Add(TK.Result);
                if (finishedTask == ST)  outputs.Add(ST.Result);
                if (finishedTask == LD)  outputs.Add(LD.Result);
                myTasks.Remove(finishedTask);
            }

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName("Function1_Hello")]
        public static async Task <string> SayHello([ActivityTrigger] string name, ILogger log)
        {
            if (name.Equals("Tokyo"))   await Task.Delay(30000); // delay 30 seconds
            if (name.Equals("Seattle")) await Task.Delay(60000); // delay 60 seconds
            if (name.Equals("London")) await Task.Delay(30000); // delay 30 seconds
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }

        [FunctionName("Function1_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("Function1", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}