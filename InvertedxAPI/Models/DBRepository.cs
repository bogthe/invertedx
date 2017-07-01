using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using InvertedxAPI.Collections;
using InvertedxAPI.Configuration;
using Microsoft.Extensions.Options;

namespace InvertedxAPI.Models
{
    public class DBRepository : IRepository
    {
        private const string TABLE_NAME = "Websites";
        private readonly AWSOptions options;
        private readonly AmazonDynamoDBClient client;
        private DynamoDBContext context;

        public DBRepository(IOptions<AWSOptions> options)
        {
            this.options = options.Value;
            client = CreateNewClient();
            InitializeTable();
        }

        public Website this[int id] => GetItem(id).Result;

        public IEnumerable<Website> WebsiteCollection => throw new NotImplementedException();

        public InvertedIndex<Website> Index => throw new NotImplementedException();

        public Website AddWebsiteSource(Website website)
        {
            SaveItem(website);
            return website;
        }

        public void DeleteWebsiteSource(int id)
        {
            throw new NotImplementedException();
        }

        public Website UpdateWebsiteSource(Website website)
        {
            throw new NotImplementedException();
        }

        private async void SaveItem(Website website)
        {
            await client.PutItemAsync(
                TABLE_NAME,
                item: new Dictionary<string, AttributeValue>()
                {
                    {"WebId", new AttributeValue{S = website.Id.ToString()}},
                    {"Url", new AttributeValue{S = website.Url}},
                    {"Processed", new AttributeValue{S = website.Processed.ToString()}}
                }
            );
        }

        private async Task<Website> GetItem(int id)
        {
            var result = await client.GetItemAsync(
                TABLE_NAME,
                key: new Dictionary<string, AttributeValue>
                {
                    {"WebId", new AttributeValue{S = id.ToString()}}
                }
            );

            Website website = new Website()
            {
                Id = Convert.ToInt32(result.Item["WebId"].S),
                Url = result.Item["Url"].S,
                Processed = Convert.ToBoolean(result.Item["Processed"].S)
            };

            return website;
        }

        private AmazonDynamoDBClient CreateNewClient()
        {
            var credentials = new BasicAWSCredentials(
                options.AwsAccessKey, options.AwsSecretKey
            );

            return new AmazonDynamoDBClient(credentials, RegionEndpoint.EUWest1);
        }

        private async void InitializeTable()
        {
            var tableList = await client.ListTablesAsync();
            if (!tableList.TableNames.Contains(TABLE_NAME))
                await CreateTable();

            await WaitForTableToBeActive();
        }

        private async Task CreateTable()
        {
            await client.CreateTableAsync(
                TABLE_NAME,
                keySchema: new List<KeySchemaElement>
                {
                    new KeySchemaElement("WebId", KeyType.HASH)
                },
                attributeDefinitions: new List<AttributeDefinition>
                {
                    new AttributeDefinition("WebId", ScalarAttributeType.S)
                },
                provisionedThroughput: new ProvisionedThroughput
                {
                    ReadCapacityUnits = 1,
                    WriteCapacityUnits = 1
                }
            );
        }

        private async Task WaitForTableToBeActive()
        {
            bool isTableActive = false;
            while (!isTableActive)
            {
                var tableStatus = await client.DescribeTableAsync(TABLE_NAME);
                isTableActive = tableStatus.Table.TableStatus == "ACTIVE";
                Thread.Sleep(1000);
            }
        }
    }
}