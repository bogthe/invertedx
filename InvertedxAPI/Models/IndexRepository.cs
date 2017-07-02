using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using InvertedxAPI.Collections;
using InvertedxAPI.Configuration;
using Microsoft.Extensions.Options;

namespace InvertedxAPI.Models
{
    public class IndexRepository : IIndexRepository
    {
        public const string TABLE_NAME = "InvertedIndex";
        private InvertedIndex<string> index;
        private readonly AmazonDynamoDBClient client;
        private readonly DynamoDBContext context;
        private readonly DBThrottler<IndexItem> throttler;

        public IndexRepository(IOptions<AWSOptions> options)
        {
            index = new InvertedIndex<string>();
            client = new AmazonDynamoDBClient(
                options.Value.AwsAccessKey, options.Value.AwsSecretKey, RegionEndpoint.EUWest1
            );
            context = new DynamoDBContext(client);
            throttler = new DBThrottler<IndexItem>(context, 10, 500);
            
            Initialisation = AsyncInitialise();
        }

        public Task Initialisation { get; private set; }

        public InvertedIndex<string> Index => index;

        public async Task UpdateRepo()
        {
            var keysEnum = index.GetKeys();
            string key = string.Empty;

            while (keysEnum.MoveNext())
            {
                key = keysEnum.Current;
                if(string.IsNullOrEmpty(key))
                    continue;

                IndexItem item = new IndexItem()
                {
                    Word = key,
                    Urls = new List<string>()
                };

                foreach(string url in index[key])
                {
                    item.Urls.Add(url);
                }

                throttler.AddItem(item);
            }

           await throttler.SaveBatches();
        }

        private async Task AsyncInitialise()
        {
            await CheckTable();
            await ConstructInMemoryIndex();
        }

        private async Task ConstructInMemoryIndex()
        {
            foreach (IndexItem item in await GetAllItems())
            {
                if (!index.ContainsKey(item.Word))
                    index.Add(item.Word, new HashSet<string>());

                foreach (string url in item.Urls)
                {
                    index[item.Word].Add(url);
                }
            }
        }

        private Task<List<IndexItem>> GetAllItems()
        {
            var conditions = new List<ScanCondition>()
            {
                new ScanCondition("Word", ScanOperator.IsNotNull)
            };
            return context.ScanAsync<IndexItem>(conditions).GetRemainingAsync();
        }

        private async Task CheckTable()
        {
            var tableList = await client.ListTablesAsync();
            if (!tableList.TableNames.Contains(TABLE_NAME))
                await CreateTable();
            else
                await WaitForActiveTable();
        }

        private async Task CreateTable()
        {
            await client.CreateTableAsync(
                TABLE_NAME,
                keySchema: new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Word",
                        KeyType = KeyType.HASH
                    }
                },
                attributeDefinitions: new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Word",
                        AttributeType = ScalarAttributeType.S
                    }
                },
                provisionedThroughput: new ProvisionedThroughput(1, 1)
            );

            await WaitForActiveTable();
        }

        private async Task WaitForActiveTable()
        {
            var result = await client.DescribeTableAsync(TABLE_NAME);
            var isActive = result.Table.TableStatus == "ACTIVE";
            if (!isActive)
            {
                Thread.Sleep(1000);
                await WaitForActiveTable();
            }
        }
    }
}