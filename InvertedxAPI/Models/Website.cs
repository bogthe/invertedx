using Amazon.DynamoDBv2.DataModel;
namespace InvertedxAPI.Models
{
    [DynamoDBTable(DBRepository.TABLE_NAME)]
    public class Website
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public string Url { get; set; }
        public bool Processed { get; set; }
    }
}