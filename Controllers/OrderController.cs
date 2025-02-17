using CodWizardsMovieShop.Models.Db;
using CodWizardsMovieShop.Models.ViewModels;
using CodWizardsMovieShop.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Humanizer;

namespace CodWizardsMovieShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderServicescs _ios;
        private readonly ICustomerServices _ics;
        private readonly IMovieServices _ims;

        List<Movie> movielist = new List<Movie>();
        List<IndividualCartOrder> cvm = new List<IndividualCartOrder>();
        List<int> countCartItem = new List<int>();

        public OrderController(IOrderServicescs ios, ICustomerServices ics, IMovieServices ims)
        {
            _ios = ios;
            _ics = ics;
            _ims = ims;

        }


        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            if (id == null) return NotFound();

            var mov = _ims.GetMovie(id);
            if (HttpContext.Session.GetString("SessionMovieList") == null)
            {
                movielist.Add(mov);
                HttpContext.Session.SetString("SessionMovieList", JsonConvert.SerializeObject(movielist));
            }
            else
            {
                List<Movie> movielist1 = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));
                movielist1.Add(mov);

                HttpContext.Session.SetString("SessionMovieList", JsonConvert.SerializeObject(movielist1));
            }

            var cartList = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));
            var numberOfListItems = cartList.Count();

            HttpContext.Session.SetInt32("cartcountS", numberOfListItems);
            return Json(numberOfListItems);

        }

        [HttpPost]
        public IActionResult IncreaseItemInCart(int id)
        {
            if (id == null) return NotFound();

            var mov = _ims.GetMovie(id);

            List<Movie> movielist1 = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));
            movielist1.Add(mov);

            HttpContext.Session.SetString("SessionMovieList", JsonConvert.SerializeObject(movielist1));

            var cartList = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));
            var numberOfListItems = cartList.Count();
            var price = cartList.FirstOrDefault(x => x.Id == id).Price;
            var movnum = cartList.FindAll(x => x.Id == id).Count();
            var tot = cartList.Sum(x => x.Price);

            HttpContext.Session.SetInt32("cartcountS", numberOfListItems);

            var data = new { pris = price, numMov = movnum, total = tot, totMovNum = numberOfListItems };

            return Json(data);

        }


        [HttpPost]
        public IActionResult DecreaseItemInCart(int id)
        {
            if (id == null) return NotFound();

            List<Movie> movielist1 = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));
            var mov = _ims.GetMovie(id);
            double price = 0; int numberOfListItems = 0; int movnum = 0; double tot = 0;

            foreach (var item in movielist1)
            {
                if (item.Id == mov.Id) { movielist1.Remove(item); break; }

            }
            HttpContext.Session.SetString("SessionMovieList", JsonConvert.SerializeObject(movielist1));

            var cartList = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));
            if (cartList.Count > 0)
            {
                numberOfListItems = cartList.Count();
                movnum = cartList.FindAll(x => x.Id == id).Count();
                if (movnum > 0)
                {
                    price = cartList.FirstOrDefault(x => x.Id == id).Price;
                }
                tot = cartList.Sum(x => x.Price);
            }
            else
            {
                price = tot = 0;
                movnum = numberOfListItems = 0;
            }
            HttpContext.Session.SetInt32("cartcountS", numberOfListItems);

            var data = new { pris = price, numMov = movnum, total = tot, totMovNum = numberOfListItems };

            return Json(data);

        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            if (id == null) return NotFound();

            int numberOfListItems = 0; double tot = 0;
            List<Movie> movielist1 = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));
            if (movielist1.FindAll(x => x.Id == id).Count > 0)
            {

                movielist1.RemoveAll(m => m.Id == id);

            }
            HttpContext.Session.SetString("SessionMovieList", JsonConvert.SerializeObject(movielist1));
            var cartList = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));

            if (cartList.Count > 0)
            {

                numberOfListItems = cartList.Count();

                tot = cartList.Sum(x => x.Price);
            }
            else
            {
                tot = 0;
                numberOfListItems = 0;
            }
            HttpContext.Session.SetInt32("cartcountS", numberOfListItems);

            var data = new { total = tot, totMovNum = numberOfListItems };


            return Json(data);
        }

        public IActionResult CartDetails()
        {
            CartViewModel cvm1 = new CartViewModel();
            if (HttpContext.Session.GetString("SessionMovieList") == null)
            {
                return View();
            }
            else
            {
                var movielist1 = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));
                foreach (var item in movielist1)
                {
                    IndividualCartOrder im = new IndividualCartOrder();
                    if (!cvm1.ListMovie.Any(x => x.Movie.Id == item.Id))
                    {
                        im.Movie = item;
                        im.NumberofCopies = 1;

                        cvm1.ListMovie.Add(im);
                    }
                    else
                    {
                        cvm1.ListMovie.FirstOrDefault(x => x.Movie.Id == item.Id).NumberofCopies++;

                    }
                }
                HttpContext.Session.SetString("SessionCartList", JsonConvert.SerializeObject(cvm));
                return View(cvm1);
            }
        }

        public IActionResult DisplayLoginPartiaView()

        {
            return PartialView("_CustomerLoginPartialView");
        }

        public IActionResult DisplayRegistraionPartiaView()
        {
            return PartialView("_CustomerRegistrationPartialView");
        }

        public IActionResult CheckoutfromCart()
        {

            var movielist = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));
            if (movielist.Count > 0)
            {
                int cusid = (int)HttpContext.Session.GetInt32("SessionCusId");

                _ios.AddOrder(movielist, cusid);

            }

            //int orderid=_ios.GetAllOrders().Select(o=>o.Id).Max();
            return Json(new { success = true, redirectToUrl = Url.Action("OrderConfirmaion", "Order") });
        }


        public IActionResult OrderConfirmaion()
        {

            CartViewModel cvm1 = new CartViewModel();

            if (HttpContext.Session.GetString("SessionMovieList") == null)
            {
                return View();
            }
            else
            {
                var movielist1 = JsonConvert.DeserializeObject<List<Movie>>(HttpContext.Session.GetString("SessionMovieList"));
                foreach (var item in movielist1)
                {
                    IndividualCartOrder im = new IndividualCartOrder();
                    if (!cvm1.ListMovie.Any(x => x.Movie.Id == item.Id))
                    {
                        im.Movie = item;
                        im.NumberofCopies = 1;

                        cvm1.ListMovie.Add(im);
                    }
                    else
                    {
                        cvm1.ListMovie.FirstOrDefault(x => x.Movie.Id == item.Id).NumberofCopies++;

                    }
                }
            }
            HttpContext.Session.Remove("SessionMovieList");
            HttpContext.Session.Remove("cartcountS");
            return View(cvm1);
        }

       public IActionResult DisplayOrdersAdmin()
        {
            List<AdminOrderViewModel> aovm=new List<AdminOrderViewModel>();
            var list = _ios.GetAllOrdersListCOR().OrderByDescending(o=>o.OrderDate);
            foreach (var item in list)
            {
                AdminOrderViewModel avm = new AdminOrderViewModel();
                avm.OrderDate = item.OrderDate;
                avm.OrderId = item.Id;
                avm.OrderByCus = item.Customer.FristName + " " + item.Customer.LastName;
                avm.CustomerId = item.Customer.Id;
                avm.DeliveryAddress=$"{item.Customer.DeliveryAddress} \n {item.Customer.DeliveryZip}, {item.Customer.DeliveryCity}";
                foreach (var movl in item.OrderRows)
                {
                    IndividualCartOrder idc = new IndividualCartOrder();
                    if (!avm.InMvovList.Any(x => x.Movie.Id == movl.MovieId))
                    {
                        idc.Movie = _ims.GetMovie(movl.MovieId);
                        idc.NumberofCopies = 1;

                        avm.InMvovList.Add(idc);
                    }
                    else
                    {
                        avm.InMvovList.FirstOrDefault(x => x.Movie.Id == movl.MovieId).NumberofCopies++;
                    }
                }
                aovm.Add(avm);

            }

            return View(aovm);
        }

		public IActionResult DeleteOrder(int id)
		{
			if (id == null)
				return NotFound();
			var order = _ios.GetOrder(id);
			if (order == null)
				return NotFound();

            AdminOrderViewModel avm = new AdminOrderViewModel();
            avm.OrderDate = order.OrderDate;
            avm.OrderId = order.Id;
            avm.CustomerId = order.CustomerId;
            foreach (var movl in order.OrderRows)
            {
                IndividualCartOrder idc = new IndividualCartOrder();
                if (!avm.InMvovList.Any(x => x.Movie.Id == movl.MovieId))
                {
                    idc.Movie = _ims.GetMovie(movl.MovieId);
                    idc.NumberofCopies = 1;

                    avm.InMvovList.Add(idc);
                }
                else
                {
                    avm.InMvovList.FirstOrDefault(x => x.Movie.Id == movl.MovieId).NumberofCopies++;
                }
            }

            return View(avm);

		}
		
		public IActionResult DeleteOrderConfirmed(int id)
		{
			_ios.DeleteOrder(id);
            TempData["Message"] = $"Order_{id} is Removed succesfully";

            return Json(new { success = true, redirectToUrl = Url.Action("DisplayOrdersAdmin", "Order") });
		}

	}
}
