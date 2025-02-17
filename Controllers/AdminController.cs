using CodWizardsMovieShop.Data;
using CodWizardsMovieShop.Models;
using CodWizardsMovieShop.Models.Db;
using CodWizardsMovieShop.Models.ViewModels;
using CodWizardsMovieShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodWizardsMovieShop.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICustomerServices _cid;
        private readonly IMovieServices _mid;
        private readonly CWDBContext _db;

        public AdminController(ICustomerServices cid, IMovieServices mid, CWDBContext db)
        {
            _cid = cid;
            _mid = mid;
            _db = db;
        }

        public IActionResult Index()
        {
            MovieCustVm obj = new MovieCustVm();
            var cutomers =_cid.GetAllCustomers().OrderBy(c=>c.FristName).ThenBy(c=>c.LastName).ToList();
            obj.Customers = cutomers;

            List<Movie> movies = _mid.GetAllMovies().OrderByDescending(m=>m.ReleaseYear).ThenBy(m=>m.Title).ToList();
            obj.Movies = movies;

            return View(obj);
        }


        [HttpGet]
        public IActionResult CreateMovie()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateMovie(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _mid.CreateMovie(movie);
				TempData["Message"] = $"{movie.Title} is Added succesfully";
				return RedirectToAction("Index"); // INDEX FOR NOW, CHANGE LATER TO MOVIE LIST MAYBE
            }

            return View();
        }

        [HttpGet]
        public IActionResult EditMovie(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var movie = _mid.GetMovie(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        [HttpPost]
        public IActionResult EditMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _mid.UpdateMovie(movie);
				TempData["Message"] = $"{movie.Title} is Updated succesfully";

				return RedirectToAction("Index");
            }
            return View(movie);
        }

        public IActionResult CustomerDetail(int id)
        {
            if (id == null)
                return NotFound();
            var customer = _cid.GetCustomer(id);
            if (customer == null)
                return NotFound();
            return View(customer);
        }

        [HttpGet]
        public IActionResult EditCustomer(int id)
        {
            if (id == null)
                return NotFound();
            var customer = _cid.GetCustomer(id);
            if (customer == null)
                return NotFound();
            return View(customer);
        }
        [HttpPost]
        public IActionResult EditCustomer(int id, Customer customer)
        {
            if (ModelState.IsValid)
            {
                _cid.EditCustomer(customer);
				TempData["Message"] = $"{customer.FristName} is Updated succesfully";

				return RedirectToAction("Index"); // CHANGE LATER
            }
            return View(customer);
        
        }
        [HttpGet]
        public IActionResult DeleteCustomer(int id)
        {
            if (id == null)
                return NotFound();
            var customer = _cid.GetCustomer(id);
            if (customer == null)
                return NotFound();
            return View(customer);
            
        }
        [HttpPost]
        public IActionResult DeleteCustomerConfirmed(int id)
        {
            _cid.DeleteCustomer(id);
			TempData["Message"] = $"Customer_{id} is Removed succesfully";

			return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult DeleteMovie(int id)
        {
            if (id == null)
                return NotFound();
            var movie = _mid.GetMovie(id);
            if (movie == null)
                return NotFound();
            return View(movie);
        }
        [HttpPost]
        public IActionResult DeleteMovieConfirmed(int id)
        {
            _mid.DeleteMovie(id);
			TempData["Message"] = $"Movie_{id} is Removed succesfully";

			return RedirectToAction("Index");
        }
        
       public IActionResult DisplayCustomers()
        {
            var customers = _cid.GetAllCustomers();
            return View(customers);
        }

        public IActionResult AdminMoviesList()
        {
            var list = _mid.GetAllMovies();
            return View(list);
        }



    }
}
