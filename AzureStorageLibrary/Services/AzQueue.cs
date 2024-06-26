﻿using Azure.Storage.Queues.Models;
using Azure.Storage.Queues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace AzureStorageLibrary.Services
{
    public class AzQueue
    {
        private readonly QueueClient _queueClient;
        public AzQueue(string queueName)
        {
            _queueClient = new QueueClient(ConnectionStrings.AzureStorageConnectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        public async Task SendMessageAsync(string message)
        {
            await _queueClient.SendMessageAsync(message);
        }

        public async Task<QueueMessage> RetrieveNextMessageAsync()
        {
            QueueProperties properties = await _queueClient.GetPropertiesAsync();
            if (properties.ApproximateMessagesCount > 0)
            {
                var queueMessages = await _queueClient.ReceiveMessagesAsync(1, TimeSpan.FromMinutes(1));

                var messages = queueMessages.Value;
                if (messages != null)
                {
                    if (messages.Any())
                    {
                        return messages[0];
                    }
                }
            }
            return null!;
        }

        public async Task DeleteMessage(string messageId, string popReceipt)
        {
            await _queueClient.DeleteMessageAsync(messageId, popReceipt);
        }


        public async Task<List<QueueMessage>> GetAllMessagesFromQueueAsync()
        {

            QueueProperties properties = await _queueClient.GetPropertiesAsync();
            if (properties.ApproximateMessagesCount > 0)
            {
                var queueMessages = await _queueClient.ReceiveMessagesAsync(properties.ApproximateMessagesCount,TimeSpan.FromSeconds(30));

                var messages = queueMessages.Value;
                if (messages != null)
                {
                    if (messages.Any())
                    {
                        return messages.ToList();
                    }
                }
            }
            return null!;
        }
    }
}
