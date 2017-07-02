using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace InvertedxAPI.Models
{
    [DynamoDBTable(IndexRepository.TABLE_NAME)]
    public class IndexItem
    {
        [DynamoDBHashKey]
        public string Word { get; set; }
        public List<string> Urls{get;set;}
    }
}