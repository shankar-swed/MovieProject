using CodWizardsMovieShop.Models.Db;

namespace CodWizardsMovieShop.Models.ViewModels
{
    public class MovieCustVm
    {
        public List<Movie> Movies { get; set; } = new List<Movie>();

        public List<Customer> Customers { get; set; } = new List<Customer>();


    }
}
