using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureForDevelopersCourse.Model;
using AzureForDevelopersCourse.Repository.Queues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Storage.Queue;

namespace AzureForDevelopersCourse.Pages
{
    public class QueuesStorageModel : PageModel
    {
        const string queueName = "orders";
        public IEnumerable<CloudQueueMessage> Messages { get; set; } = new List<CloudQueueMessage>() as IEnumerable<CloudQueueMessage>;
        public CloudQueueMessage Message { get; set; }

        [BindProperty]
        public string MessageValue { get; set; }
        IAzureQueuesStorage QueuesStorage;

        public QueuesStorageModel(IAzureQueuesStorage queuesStorage) {
            QueuesStorage = queuesStorage;
        }

        public async Task OnGetAsync()
        {
            Messages = await GetMessagesAsync();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await QueuesStorage.AddMessageAsync(queueName, MessageValue);
            return RedirectToPage("/Queues/QueuesStorage");
        }

        public async Task OnGetGetMessageAsync()
        {
            Message = await QueuesStorage.GetMessageAsync(queueName);
            if (Message != null) {
                await DeleteMessageAsync(Message);
            }
            Messages = await GetMessagesAsync();
        }
         private async Task<IEnumerable<CloudQueueMessage>> GetMessagesAsync() {
            return await QueuesStorage.PeekMessagesAsync(queueName, 10);
        }

        public async Task DeleteMessageAsync(CloudQueueMessage message)
        {
            await QueuesStorage.DeleteMessageAsync(queueName, message.Id, message.PopReceipt);
        }
    }
}
