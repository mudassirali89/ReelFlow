
using Microsoft.EntityFrameworkCore;
using ReelFlow.DTO;
using ReelFlow.Models;
using ReelFLow.Context;
using ReelFLow.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ReelFLow.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext _context;


        public MovieRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
        return await _context.Movies.Include(m => m.Category).ToListAsync();

        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
        return await _context.Movies.Include(m => m.Category).FirstOrDefaultAsync(m => m.MovieId == id);

        }

        public async Task<Movie> AddMovieAsync(Movie movie)
        {
            
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
            return movie;
        }

        public async Task<bool> UpdateMovieAsync(Movie movie)
        {
            _context.Entry(movie).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return false;
            }
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Movie>MovieExistsAsync(MovieDTO movieDTO)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(movie =>
            movie.Title == movieDTO.Title &&
            movie.ReleaseDate.Date == movieDTO.ReleaseDate.Date &&
            movie.Director == movieDTO.Director);
            return movie;
        }
        public async Task<IEnumerable<Movie>> GetMoviesByCategoryAsync(int categoryId)
        {
            return await _context.Movies
                .Where(m => m.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> SearchMoviesAsync(string? title, string? description, DateTime? releaseDate)
        {
            var query = _context.Movies.Include(m => m.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(m => m.Title.Contains(title));

            if (!string.IsNullOrWhiteSpace(description))
                query = query.Where(m => m.Description.Contains(description));

            if (releaseDate.HasValue)
                query = query.Where(m => m.ReleaseDate.Date == releaseDate.Value.Date);

            return await query.ToListAsync();
        }



    }

}
