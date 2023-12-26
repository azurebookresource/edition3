using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using Microsoft.Azure.Cosmos;

namespace ConsoleAppC4S4 // replace it with your Project name
{
    class Program
    {
        private static readonly string EndpointUri = ConfigurationManager.AppSettings["EndPointUri"];
        private static readonly string PrimaryKey = ConfigurationManager.AppSettings["PrimaryKey"];

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        private Database database;
        private Container container;

        // The name of the database and container we will create
        private string databaseId = "Registrations"; // Replace it with your database id; See Section 4.3
        private string containerId = "Students";    // replace it with your container id; See Section 4.3

        public static async Task Main(string[] args)
        {
            try
            {
                Program p = new Program();
                await p.GetStartedDemoAsync();
            }
            catch (CosmosException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
            finally
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }

        public async Task GetStartedDemoAsync()
        {
            // Create a new instance of the Cosmos Client
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions() { ApplicationName = "CosmosDBDotnetQuickstart" });
            await this.CreateDatabaseAsync();
            await this.CreateContainerAsync();
            await this.AddItemsToContainerAsync();
            await this.QueryItemsAsync();
        }

        private async Task CreateDatabaseAsync()
        {
            // Create a new database
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        }

        private async Task CreateContainerAsync()
        {
            // Create a new container
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/LastName", 400);
        }

        private async Task AddItemsToContainerAsync()
        {
            // Create a Student object
            Student s = new Student
            {
                Id = "Duke001",
                LastName = "Duke",
                Address = new Address { State = "IL", County = "Cook", City = "Chicago" }
            };

            try
            {
                // Read the item to see if it exists.  
                ItemResponse<Student> NewItemResponse = await this.container.ReadItemAsync<Student>(s.Id, new PartitionKey(s.LastName));
                Console.WriteLine("Item in database with id: {0} already exists\n", NewItemResponse.Resource.Id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container representing the new item.
                ItemResponse<Student> NewItemResponse = await this.container.CreateItemAsync<Student>(s, new PartitionKey(s.LastName));
                Console.WriteLine("Created item in database with id: {0}.\n", NewItemResponse.Resource.Id);
            }
        }

        private async Task QueryItemsAsync()
        {
            var sqlQueryText = "SELECT * FROM c WHERE c.LastName = 'Duke'";

            Console.WriteLine("Running query to retrieve all students with last name Duke:");

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Student> queryResultSetIterator = this.container.GetItemQueryIterator<Student>(queryDefinition);

            List<Student> result = new List<Student>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Student> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Student s in currentResultSet)
                {
                    result.Add(s);
                    Console.WriteLine("ID: " + s.Id + "; City: " + s.Address.City);
                }
            }
        }
    }
}