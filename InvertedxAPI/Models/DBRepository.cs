using System;
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
    public class DBRepository : IAsyncRepository
    {
        public const string TABLE_NAME = "Websites";
        private readonly AmazonDynamoDBClient client;
        private readonly DynamoDBContext context;
        private readonly List<ScanCondition> scanConditions;
        private InvertedIndex<Website> index;

        public DBRepository(IOptions<AWSOptions> options)
        {
            index = new InvertedIndex<Website>();

            var credentials = new BasicAWSCredentials(options.Value.AwsAccessKey, options.Value.AwsSecretKey);
            client = new AmazonDynamoDBClient(credentials, RegionEndpoint.EUWest1);
            context = new DynamoDBContext(client);

            scanConditions = new List<ScanCondition>
            {
                new ScanCondition("Id", ScanOperator.IsNotNull)
            };

            Initialisation = CheckTable();
        }

        public Task<Website> this[string id] => context.LoadAsync<Website>(id);

        public InvertedIndex<Website> Index => index;

        public Task Initialisation { get; private set; }

        public Task<List<Website>> Collection => context.ScanAsync<Website>(scanConditions).GetRemainingAsync();

        public async Task<Website> AddWebsite(Website website)
        {
            if (string.IsNullOrEmpty(website.Id))
                website.Id = Guid.NewGuid().ToString();

            await context.SaveAsync<Website>(website);
            return website;
        }

        public Task DeleteWebsite(Website website) => context.DeleteAsync<Website>(website);

        public Task<Website> UpdateWebsite(Website website) => AddWebsite(website);

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
                        AttributeName = "Id",
                        KeyType = KeyType.HASH
                    }
                },
                attributeDefinitions: new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Id",
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