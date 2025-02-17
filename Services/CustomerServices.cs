using CodWizardsMovieShop.Data;
using CodWizardsMovieShop.Models.Db;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CodWizardsMovieShop.Services
{
	public class CustomerServices : ICustomerServices
	{
		private readonly CWDBContext _db;

		public CustomerServices(CWDBContext db)
		{
			_db = db;
		}
		public void EditCustomer(Customer customer)
		{

			_db.Customers.Update(customer);
			_db.SaveChanges();

		}
		public void CreateCustomer(Customer customer)
		{
			if (customer != null)
			{
				_db.Customers.Add(customer);
				_db.SaveChanges();
			}
		}
		public void DeleteCustomer(int id)
		{
			var customer = _db.Customers.Find(id);
			if (customer != null)
			{
				_db.Customers.Remove(customer);
				_db.SaveChanges();
			}
		}

		public bool CustomerExists(int id)
		{
			return _db.Customers.Any(x => x.Id == id);
		}
		public bool CustomerExist(string email)
		{
			return _db.Customers.Any(x => x.Email==email);
		}


		public List<Customer> GetAllCustomers()
		{
			return _db.Customers.ToList();
		}

		public Customer GetCustomer(int id)
		{
			var customer = _db.Customers.FirstOrDefault(x => x.Id == id);
			return customer;
		}

		public Customer GetCustomerFromEmail(string email)
		{ 
			var customer = _db.Customers.FirstOrDefault(x=>x.Email == email);
			return customer;
		}
	}
}
