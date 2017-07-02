using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;

namespace InvertedxAPI.Models
{
    public class DBThrottler<T> : IDisposable
    {
        private DynamoDBContext context;
        private readonly int maxBatchWrite;
        private List<BatchWrite<T>> items;
        
        private int currentItemSlot;
        private int itemsInBatch;
        private int sleepTimeMs;

        private bool disposingItems;

        public DBThrottler(DynamoDBContext dynamoContext, int maxBatchNo, int sleepTimeMs)
        {
            
            context = dynamoContext;
            maxBatchWrite = maxBatchNo;
            this.sleepTimeMs = sleepTimeMs;

            items = new List<BatchWrite<T>>();
            items.Add(context.CreateBatchWrite<T>());
        }

        public void AddItem(T item)
        {
            items[currentItemSlot].AddPutItem(item);
            itemsInBatch++;

            if (itemsInBatch >= maxBatchWrite)
            {
                items.Add(context.CreateBatchWrite<T>());
                itemsInBatch = 0;
                currentItemSlot++;
            }
        }

        public async Task SaveBatches()
        {
            Console.WriteLine($"Bathces: {items.Count}");
            foreach(var batchWrite in items)
            {
                Console.WriteLine("Saving batch...");
                await batchWrite.ExecuteAsync();
                if(sleepTimeMs > 0)
                    Thread.Sleep(sleepTimeMs);
            }
        }   

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if(!disposingItems)
            {
                disposingItems = true;
                if(isDisposing)
                {
                    items = null;
                    context = null;
                }
            }
        }
    }
}