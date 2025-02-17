using CodWizardsMovieShop.Models.Db;

namespace CodWizardsMovieShop.Models.ViewModels
{
    public class CustomerOrdersViewModel
    {
        public string CustomerName { get; set; }
        public int TotalOrders { get; set; }
        public string Email { get; set; }
        public List<OrderViewModel> Orders { get; set; }
    }
}
