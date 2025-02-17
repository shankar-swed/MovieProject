using CodWizardsMovieShop.Models.Db;

namespace CodWizardsMovieShop.Models.ViewModels
{
   
        public class OrderViewModel
        {
            
            public int OrderId { get; set; }
            public DateTime OrderDate { get; set; }
            public decimal TotalCost { get; set; }
		    public List<Movie> Movies { get; set; }
        }

    
}
