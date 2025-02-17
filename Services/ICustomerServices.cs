using CodWizardsMovieShop.Models.Db;

namespace CodWizardsMovieShop.Services
{
    public interface ICustomerServices
    {
        public void EditCustomer(Customer customer);
        public void CreateCustomer(Customer customer);
        public void DeleteCustomer(int id);
        public List<Customer> GetAllCustomers();        
        public Customer GetCustomer(int id);
        public bool CustomerExists(int id);
        public bool CustomerExist(string email);
        public Customer GetCustomerFromEmail(string email);


	}
}
