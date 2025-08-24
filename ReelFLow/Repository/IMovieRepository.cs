

using ReelFlow.DTO;
using ReelFlow.Models;
using ReelFLow.DTO;

namespace ReelFLow.Repository
{
    public interface IMovieRepository
    {

         Task<IEnumerable<Movie>> GetAllMoviesAsync();
         Task<Movie?> GetMovieByIdAsync(int id);
       
      //  Task<bool> AddMovieAsync(MovieDTO movieDTO);

        
        Task<Movie> AddMovieAsync(Movie movie);

        Task<bool> DeleteMovieAsync(int id);
        Task<bool> UpdateMovieAsync(Movie movie);
        Task<Movie>MovieExistsAsync(MovieDTO movieDTO);
        Task<IEnumerable<Movie>> GetMoviesByCategoryAsync(int categoryId);

        Task<IEnumerable<Movie>> SearchMoviesAsync(string? title, string? description, DateTime? releaseDate);


    }
}
