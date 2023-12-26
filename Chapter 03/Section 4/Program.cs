using System;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.Azure.Storage; // Namespace for CloudStorageAccount
using Microsoft.Azure.Storage.Queue; // Namespace for Queue storage types

namespace azurebookc3s4
{
    class Program
    {
        static void Main(string[] args)
        {
            // Parse the connection string, return reference to storage account.
            CloudStorageAccount StorageAct = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudQueueClient queueClient = StorageAct.CreateCloudQueueClient();

            // Get reference to a queue; Create the queue if it doesn't exist.
            CloudQueue jobqueue = queueClient.GetQueueReference("jobtodo");        
            jobqueue.CreateIfNotExists();

            // Get current timestamp (ts)
            String ts = (DateTime.Now).ToString("dddd, dd MMMM yyyy HH: mm:ss");

            // Create a new job message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage("job assigned at " + ts);
            jobqueue.AddMessage(message);

            // Get the next job message
            CloudQueueMessage retrievedMessage = jobqueue.GetMessage();

            //Display the job message retrived from the queue.
            string m = retrievedMessage.AsString;
            Console.WriteLine("Next job to do: " + m);

            //Process the job, and then delete the message
            jobqueue.DeleteMessage(retrievedMessage);
        }
    }
}