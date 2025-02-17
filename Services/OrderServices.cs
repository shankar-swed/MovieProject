using CodWizardsMovieShop.Data;
using CodWizardsMovieShop.Models.Db;
using CodWizardsMovieShop.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CodWizardsMovieShop.Services
{
	public class OrderServices : IOrderServicescs
	{
		private readonly CWDBContext _db;

		public OrderServices(CWDBContext db)
		{
			_db = db;
		}

		public void AddOrder(List<Movie> movies, int cusId )
		{
			//if (order != null) _db.Orders.Add(order);
			//_db.SaveChanges();
			Order newOrder = new Order()
			{ 
				CustomerId = cusId,
				OrderDate = DateTime.Now
			};
			foreach (Movie movie in movies)
			{
				OrderRow row = new OrderRow()
				{
					MovieId = movie.Id,
					Price = movie.Price,
				};
				newOrder.OrderRows.Add(row);
			}
			_db.Orders.Add(newOrder);
			_db.SaveChanges();
		}


		public void DeleteOrder(int id)
		{
			var order = _db.Orders.Include(or => or.OrderRows).FirstOrDefault(o=> o.Id == id);
			
				_db.Orders.Remove(order);
				_db.SaveChanges();
			
		}		
		

		public Order GetOrder(int id)
		{
			var order = _db.Orders.Include(or=>or.OrderRows).ThenInclude(m=>m.Movie).FirstOrDefault(o => o.Id == id);
			return order;
		}

		public bool OrderExists(int id)
		{
			return _db.Orders.Any(m => m.Id == id);
		}
				

		public List<Order> GetAllOrdersListCOR()
		{
			var order=_db.Orders.Include(c=>c.Customer).Include(or=>or.OrderRows).ToList();
			return order;
		}

	

        public IEnumerable<OrderViewModel> GetOrdersByCustomerEmail(string email)
        {
            var orders = _db.Orders
                .Include(o => o.OrderRows)
                .ThenInclude(or => or.Movie)
                .Where(o => o.Customer.Email == email)
                .Select(o => new OrderViewModel
                {
                    OrderId = o.Id,
                    OrderDate = o.OrderDate,
                    TotalCost = (decimal)o.OrderRows.Sum(or => or.Price), 
                    Movies = o.OrderRows.Select(or => new Movie
                    {
                        Title = or.Movie.Title,
                        Director = or.Movie.Director,
                        ReleaseYear = or.Movie.ReleaseYear,
                        Price = or.Price
                    }).ToList()
                }).ToList();

            return orders;
        }


    }

}
