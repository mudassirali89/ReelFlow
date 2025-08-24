using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReelFlow.DTO;
using ReelFlow.Models;
using ReelFLow.Context;
using ReelFLow.DTO;
using ReelFLow.Repository;
using Microsoft.AspNetCore.Authorization;


namespace ReelFLow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ICategoryRepository _categoryRepository;
        public MovieController(IMovieRepository _movieRepository,ICategoryRepository _categoryRepository)
        {
            this._movieRepository = _movieRepository;
            this._categoryRepository = _categoryRepository;
        }
        [HttpGet("movies")]
        public async Task<IActionResult> GetAllMoviesAsync()
        {
        var movies = await _movieRepository.GetAllMoviesAsync();
            if (movies == null || !movies.Any())
                return NotFound("No movies found");

            return Ok(movies);
        }

        [HttpGet("movies/{id}")]
        public async Task<IActionResult> GetMovieByIdAsync(int id)
        {
        var movie = await _movieRepository.GetMovieByIdAsync(id);
            if (movie == null)
                return NotFound($"No movie found with ID = {id}");

            return Ok(movie);
        }

        [HttpPost("add")]
        public async Task<IActionResult> CreateMovieAsync([FromBody] MovieDTO movieDTO)
        {
            var movieExists =await _movieRepository.MovieExistsAsync(movieDTO);
            if(movieExists!=null)
            {
                return BadRequest(new { message = "Movie already exists" });
            }
            Movie movie = new Movie
            {
                Title= movieDTO.Title,
                Description= movieDTO.Description,
                ReleaseDate= movieDTO.ReleaseDate,
                DurationMinutes= movieDTO.DurationMinutes,
                Genre= movieDTO.Genre,
                Director= movieDTO.Director,
                CategoryId=movieDTO.CategoryId
            };
            var result = await _movieRepository.AddMovieAsync(movie);
            return Ok(result);
        }

        [HttpPut("movies/{id}")]
        public async Task<IActionResult> UpdateMovieAsync([FromRoute] int id, [FromBody] UpdateMovieDTO updateMovieDTO)
        {
            var existingMovie = await _movieRepository.GetMovieByIdAsync(id);
            if (existingMovie == null)
                return BadRequest(new { message = "Movie does not exist" });

            existingMovie.Title = updateMovieDTO.Title;
            existingMovie.ReleaseDate = updateMovieDTO.ReleaseDate;
            existingMovie.Genre = updateMovieDTO.Genre;
            existingMovie.Description = updateMovieDTO.Description;
            existingMovie.DurationMinutes = updateMovieDTO.DurationMinutes;
            existingMovie.Director = updateMovieDTO.Director;
            existingMovie.CategoryId = updateMovieDTO.CategoryId;

            await _movieRepository.UpdateMovieAsync(existingMovie);
            return Ok(new { message = "Movie updated successfully" });
        }


        [HttpDelete("movies/{id}")]
        public async Task<IActionResult> DeleteMovieAsync(int id)
        {
            var result = await _movieRepository.DeleteMovieAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Movie not found." });
            }
            return Ok(new { message = "Movie deleted successfully." });
        }

        [HttpGet("movies/search")]
        public async Task<IActionResult> SearchMoviesAsync(
        [FromQuery] string? title,
        [FromQuery] string? description,
        [FromQuery] DateTime? releaseDate)
        {
            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(description) && !releaseDate.HasValue)
            {
                return BadRequest(new { message = "Please provide at least one search parameter." });
            }

            var results = await _movieRepository.SearchMoviesAsync(title, description, releaseDate);

            if (results == null || !results.Any())
            {
                return NotFound(new { message = "No movies matched the search criteria." });
            }

            return Ok(results);
        }

        [HttpGet("movies/filter/{categoryId}")]
        public async Task<IActionResult> FilterMoviesByCategoryAsync([FromRoute]int categoryId)
        {
            var movies = await _movieRepository.GetMoviesByCategoryAsync(categoryId);

            if (movies == null || !movies.Any())
                return NotFound(new { message = "No movies found for the specified category." });

            return Ok(movies);
        }

    }
}
