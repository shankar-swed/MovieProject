using CodWizardsMovieShop.Models.Db;

namespace CodWizardsMovieShop.Models.ViewModels
{
    public class CartViewModel
    {
        public List<IndividualCartOrder>ListMovie { get; set; } = new List<IndividualCartOrder>();
        public Customer Customer { get; set; }

       
    }
}
