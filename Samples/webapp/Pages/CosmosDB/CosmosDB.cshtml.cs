using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AzureForDevelopersCourse.Model;
using AzureForDevelopersCourse.Repository.BlobStorage;
using AzureForDevelopersCourse.Repository.CosmosDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureForDevelopersCourse.Pages
{
    public class CosmosDBModel : PageModel
    {
        const string collectionName = "MoviesCollection";
        const string routeName = "/CosmosDB/CosmosDB";

        [BindProperty]
        public Movie Movie { get; set; }
        public List<Movie> Movies { get; set; } = new List<Movie>();

        ICosmosDBRepository<Movie> cosmosDbRepository;
        public CosmosDBModel(ICosmosDBRepository<Movie> cosmosDbRepository) {
            this.cosmosDbRepository = cosmosDbRepository;
        }
        public async Task OnGetAsync()
        {
            Movies = await cosmosDbRepository.GetItems();
        }

       public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) {
                return Page();
            }

            Movie.id = Guid.NewGuid().ToString();
            Movie.Distributor = new Distributor
            {
                Name = "Out of this World Movies",
                Address = new Address { State = "CA",  City = "Los Angelos", Zip = 90001 }
            };

            await cosmosDbRepository.AddItemAsync(Movie);
            return RedirectToPage(routeName);
        }

        public async Task<IActionResult> OnGetDeleteMovieAsync(string id)
        {
            await cosmosDbRepository.DeleteItemAsync(id);
            return RedirectToPage(routeName);
        }
    }
}
