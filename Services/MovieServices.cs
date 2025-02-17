﻿using CodWizardsMovieShop.Data;
using CodWizardsMovieShop.Models.Db;
using CodWizardsMovieShop.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CodWizardsMovieShop.Services
{
    public class MovieServices : IMovieServices
    {

        private readonly CWDBContext _db;

        public MovieServices(CWDBContext db)
        {
            _db = db;
        }
        public void CreateMovie(Movie movie)
        {
            if (movie != null) _db.Movies.Add(movie);
            _db.SaveChanges();
        }

        public void UpdateMovie(Movie movie)
        {
            _db.Movies.Update(movie);
            _db.SaveChanges();
        }
        public void DeleteMovie(int id)
        {
            var movie = _db.Movies.Find(id);
            if (movie != null)
            {
                _db.Movies.Remove(movie);
                _db.SaveChanges();
            }
        }

        public List<Movie> GetAllMovies()
        {
            return _db.Movies.ToList();
        }

        public Movie GetMovie(int id)
        {
            var movie = _db.Movies.FirstOrDefault(m => m.Id == id);
            return movie;
        }

        public bool MovieExists(int id)
        {
            return _db.Movies.Any(m => m.Id == id);
        }
        public CustomerViewModel TopMovies()
        {

            var viewModel = new CustomerViewModel
            {

                MostPopularMovie = _db.Movies
                    .OrderByDescending(m => m.OrderRows.Count)
                    .Take(5)
                    .ToList(),


                Top5NewestMovies = _db.Movies
                        .OrderByDescending(m => m.ReleaseYear)
                        .Take(5)
                        .ToList(),

                Top5CheapestMovies = _db.Movies
                        .OrderBy(m => m.Price)
                        .Take(5)
                        .ToList(),

                Top5OldestMovies = _db.Movies
                        .OrderBy(m => m.ReleaseYear)
                        .Take(5)
                        .ToList(),

                CustomerSpentMostMoney = _db.Customers
                      .Include(c => c.Orders)
                      .ThenInclude(o => o.OrderRows)
                      .ToList()
                      .Select(c => new
                      {
                          Customer = c,
                          TotalSpent = c.Orders.Sum(o => o.OrderRows.Sum(or => or.Price))
                      })
                      .OrderByDescending(c => c.TotalSpent)
                      .FirstOrDefault()?.Customer


            };
            return viewModel;
        }
    }   
}

