using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureForDevelopersCourse.Model;
using AzureForDevelopersCourse.Repository.TableStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureForDevelopersCourse.Pages
{
    public class TableStorageModel : PageModel
    {
        const string tableName = "Customers";
        const string routeName = "/Tables/TableStorage";
        public List<Customer> Customers { get; set; } = new List<Customer>();

        [BindProperty]
        public Customer InsertedCustomer { get; set; }
        IAzureTableStorage<Customer> TableStorage;
        public TableStorageModel(IAzureTableStorage<Customer> tableStorage) {
            TableStorage = tableStorage;
        }
        public async Task OnGetAsync()
        {
            Customers = await this.TableStorage.GetItems(tableName);
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            this.InsertedCustomer.RowKey = Guid.NewGuid().ToString();
            await TableStorage.Insert(tableName, this.InsertedCustomer);
            return RedirectToPage(routeName);
        }

        public async Task<IActionResult> OnGetDeleteCustomerAsync(string partitionKey, string rowKey)
        {
            await TableStorage.Delete(tableName, partitionKey, rowKey);
            return RedirectToPage(routeName);
        }
    }
}
