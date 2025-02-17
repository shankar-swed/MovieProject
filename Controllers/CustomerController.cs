using CodWizardsMovieShop.Data;
using CodWizardsMovieShop.Models;
using CodWizardsMovieShop.Models.Db;
using CodWizardsMovieShop.Models.ViewModels;
using CodWizardsMovieShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace CodWizardsMovieShop.Controllers
{

	public class CustomerController : Controller
	{
		private readonly ICustomerServices _cid;
		private readonly IMovieServices _mid;
		private readonly IOrderServicescs _oid;

		public CustomerController(ICustomerServices cid, IMovieServices mid, IOrderServicescs oid)

		{
			_cid = cid;
			_mid = mid;
			_oid = oid;
		}

		public IActionResult List()
		{
			var movies = _mid.GetAllMovies();
			return View(movies);
		}
		public IActionResult MovieDetails(int id)
		{
			if (id == null)
				return NotFound();
			var movie = _mid.GetMovie(id);
			if (movie == null)
				return NotFound();


			return View(movie);
		}
		public IActionResult Index()
		{
			var viewModel = _mid.TopMovies();
			return View(viewModel);

		}
		[HttpGet]
		public IActionResult CreateCustomer()
		{
			TempData.Clear();
			return View();

		}
		[HttpPost]

		public IActionResult CreateCustomer(Customer customer)
		{
			if (ModelState.IsValid)
			{
				_cid.CreateCustomer(customer);
				string cusName = customer.FristName + " " + customer.LastName;
				int cusId = customer.Id;
				HttpContext.Session.SetString("SessionCustomerName", cusName);
				HttpContext.Session.SetInt32("SessionCusId", cusId);
				TempData["Message"] = $"{cusName} is registered succesfully";

			}

			if (TempData["formCart"] == null)
				return RedirectToAction("Index");
			else
				return RedirectToAction("CartDetails", "Order");
		}


		public IActionResult LoginCustomer()
		{
			//TempData["fromCart"] = null;
			TempData.Clear();
			return View();

		}
		[HttpPost]
		public IActionResult LoginCustomer(string email)
		{
			if (ModelState.IsValid)
			{
				var customer = _cid.GetCustomerFromEmail(email);
				if (customer != null)
				{
					string cusName = customer.FristName + " " + customer.LastName;
					int cusId = customer.Id;
					HttpContext.Session.SetString("SessionCustomerName", cusName);
					HttpContext.Session.SetInt32("SessionCusId", cusId);
					HttpContext.Session.SetString("SessionCustomerEmail", email);

				}


			}

			if (TempData["formCart"] == null)
				return RedirectToAction("Index");
			else
				return RedirectToAction("CartDetails", "Order");
		}

		public IActionResult LogoutCustomer()
		{


			HttpContext.Session.Remove("SessionCustomerName");
			HttpContext.Session.Remove("SessionCusId");
			TempData.Clear();
			return RedirectToAction("Index", "Customer");


		}

		[HttpPost]
		public JsonResult ValidateEmail(string email)
		{
			bool emailExists = _cid.CustomerExist(email);
			if (!emailExists)
					return Json(new { success = false, message = "Email is not registered." });
			
			return Json(new { success = true });
		}

		[HttpPost]
		public JsonResult ValidateRegisterEmail(string email)
		{
			//if (!ModelState.IsValid) { return Json(new { success = false, message = "Invalid email format." }); }

			
			bool emailExists = _cid.CustomerExist(email);
			if (emailExists)
				return Json(new { success = false, message = "Email already exists." });

			return Json(new { success = true });
		}

        [HttpGet]
        public IActionResult Orders(string email)
        {

			

			if (string.IsNullOrEmpty(email))
			{
                return BadRequest("Email is required.");

			}

			var customer = _cid.GetCustomerFromEmail(email);
            if (customer == null)
                return BadRequest("No Customer found.");

            var orders = _oid.GetOrdersByCustomerEmail(email)?.ToList() ?? new List<OrderViewModel>();

            var viewModel = new CustomerOrdersViewModel
            {
                CustomerName = $"{customer.FristName} {customer.LastName}",
                TotalOrders = orders.Count(),
                Orders = orders,
            };

            if (!orders.Any())
                ViewBag.Message = "No orders found for this customer.";

            return View(viewModel);
        }

    }
}
