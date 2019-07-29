using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureForDevelopersCourse.Repository.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureForDevelopersCourse.Pages
{
    public class ServiceBusModel : PageModel
    {
        const string queueName = "orders";
        public string StatusMessage { get; set; }

        [BindProperty]
        public string MessageValue { get; set; }
        IAzureServiceBus ServiceBus;

        public ServiceBusModel(IAzureServiceBus serviceBus) {
            ServiceBus = serviceBus;
        }

        // public async Task OnGetAsync()
        // {

        // }

        public async Task<IActionResult> OnPostAsync() {
            if (ModelState.IsValid)
            {
                try {
                    await ServiceBus.SendMessageAsync("orders", MessageValue);
                    StatusMessage = "Message sent!";
                }
                catch (Exception exp) {
                    StatusMessage = exp.Message;
                }
            }

            return Page();            
        }

    }
}
