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
            InitializeContext();
        }

        public Website this[int id] => new Website() { Id = 0, Url = $"{options.AwsAccessKey} / {options.AwsSecretKey}" };

        public IEnumerable<Website> WebsiteCollection => throw new NotImplementedException();

        public InvertedIndex<Website> Index => throw new NotImplementedException();

        public Website AddWebsiteSource(Website website)
        {
            throw new NotImplementedException();
        }

        public void DeleteWebsiteSource(int id)
        {
            throw new NotImplementedException();
        }

        public Website UpdateWebsiteSource(Website website)
        {
            throw new NotImplementedException();
        }

        private AmazonDynamoDBClient CreateNewClient()
        {
            var credentials = new BasicAWSCredentials(
                options.AwsAccessKey, options.AwsSecretKey
            );

            return new AmazonDynamoDBClient(credentials, RegionEndpoint.EUWest1);
        }

        private async void InitializeContext()
        {
            var tableList = await client.ListTablesAsync();
            if (!tableList.TableNames.Contains(TABLE_NAME))
                await CreateTable();

            await WaitForTableToBeActive();
            context = new DynamoDBContext(client);
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
                Thread.Sleep(200);
            }
        }
    }
}