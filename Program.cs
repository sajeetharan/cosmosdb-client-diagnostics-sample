using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Cosmos;

namespace ClientDiagnostics
{
    class Program
    {
        // The Azure Cosmos DB endpoint for running this sample.
        private static readonly string EndpointUri = ConfigurationManager.AppSettings["EndPointUri"];

        // The primary key for the Azure Cosmos account.
        private static readonly string PrimaryKey = ConfigurationManager.AppSettings["PrimaryKey"];

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database database;

        // The container we will create.
        private Container container;

        // The name of the database and container we will create
        private string databaseId = "cosmicworks";
        private string containerId = "products";

        // <Main>
        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Beginning operations...\n");
                Program p = new Program();
                await p.GetStartedDemoAsync();

            }
            catch (CosmosException cosmosException)
            {
                Console.WriteLine("Cosmos Exception with Status {0} : {1}\n", cosmosException.StatusCode, cosmosException);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit.");
                Console.ReadKey();
            }
        }
        // </Main>

        // <GetStartedDemoAsync>
        /// <summary>
        /// Entry point to call methods that operate on Azure Cosmos DB resources in this sample
        /// </summary>
        public async Task GetStartedDemoAsync()
        {
            // Create a new instance of the Cosmos Client
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            await this.QueryItemsAsync();
        }
 


        // <QueryItemsAsync>
        /// <summary>
        /// Run a query (using Azure Cosmos DB SQL syntax) against the container
        /// </summary>
        private async Task QueryItemsAsync()
        {
            var sqlQueryText = "SELECT * FROM products p";

            Console.WriteLine("Running query: {0}\n", sqlQueryText);
            this.container = cosmosClient.GetContainer(databaseId,containerId);
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            using FeedIterator<Product> queryResultSetIterator = this.container.GetItemQueryIterator<Product>(queryDefinition);

            List<Product> products = new List<Product>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Product> response = await queryResultSetIterator.ReadNextAsync();

                string diagnostics = response.Diagnostics.ToString();

                foreach (Product prod in response)
                {
                    products.Add(prod);
                    Console.WriteLine("\tRead {0}\n", prod);
                }
            }
        }
    }
}
