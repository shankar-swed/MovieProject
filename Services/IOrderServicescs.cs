using CodWizardsMovieShop.Models.Db;
using CodWizardsMovieShop.Models.ViewModels;

namespace CodWizardsMovieShop.Services
{
	public interface IOrderServicescs
	{
		//for order
		public void AddOrder(List<Movie> movies,int cusId);
		public void DeleteOrder(int id);
		public Order GetOrder(int id);
		public bool OrderExists(int id);

		public List<Order> GetAllOrdersListCOR();

		public IEnumerable<OrderViewModel> GetOrdersByCustomerEmail(string email);



    }
}
