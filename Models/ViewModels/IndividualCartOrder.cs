using CodWizardsMovieShop.Models.Db;

namespace CodWizardsMovieShop.Models.ViewModels
{
    public class IndividualCartOrder
    {
        public Movie Movie{ get; set; }
        public int NumberofCopies { get; set; }
    }
}
