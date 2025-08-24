using Microsoft.AspNetCore.Mvc;
using Moq;
using ReelFlow.DTO;
using ReelFlow.Models;
using ReelFLow.Controllers;
using ReelFLow.DTO;
using ReelFLow.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelFlowTests
{
    [TestFixture]
    public class MovieControllerTests
    {
        private Mock<IMovieRepository> _mockMovieRepo;
        private Mock<ICategoryRepository> _mockCategoryRepo;
        private MovieController _controller;

        [SetUp]
        public void Setup()
        {
            _mockMovieRepo = new Mock<IMovieRepository>();
            _mockCategoryRepo = new Mock<ICategoryRepository>();
            _controller = new MovieController(_mockMovieRepo.Object, _mockCategoryRepo.Object);
        }

        [Test]
        public async Task GetAllMoviesAsync_ReturnsOkResult_WithMovieList()
        {
            
            var movies = new List<Movie>
            {
                new Movie { MovieId = 1, Title = "Movie 1", Director = "Director A" },
                new Movie { MovieId = 2, Title = "Movie 2", Director = "Director B" }
            };

            _mockMovieRepo.Setup(repo => repo.GetAllMoviesAsync())
                          .ReturnsAsync(movies);

            
            var result = await _controller.GetAllMoviesAsync();

            
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(movies));
        }

        [Test]
        public async Task GetMovieByIdAsync_ValidId_ReturnsMovie()
        {
            
            var movie = new Movie { MovieId = 1, Title = "Test Movie", Director = "Test Director" };

            _mockMovieRepo.Setup(repo => repo.GetMovieByIdAsync(1))
                          .ReturnsAsync(movie);

            
            var result = await _controller.GetMovieByIdAsync(1);

            
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(movie));
        }

        [Test]
        public async Task GetMovieByIdAsync_InvalidId_ReturnsNotFound()
        {
           
            _mockMovieRepo.Setup(repo => repo.GetMovieByIdAsync(999))
                          .ReturnsAsync((Movie)null);

           
            var result = await _controller.GetMovieByIdAsync(999);

            
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task SearchMoviesAsync_WithTitleOnly_ReturnsMatchingMovies()
        {
            
            var title = "Avengers";
            var movies = new List<Movie>
            {
                new Movie { MovieId = 1, Title = "Avengers: Endgame", Director = "Russo Brothers" }
            };

            _mockMovieRepo.Setup(repo => repo.SearchMoviesAsync(title, null, null))
                          .ReturnsAsync(movies);

            
            var result = await _controller.SearchMoviesAsync(title, null, null);

            
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(movies));
        }

        [Test]
        public async Task SearchMoviesAsync_WithNoParams_ReturnsBadRequest()
        {
            
            var result = await _controller.SearchMoviesAsync(null, null, null);

           
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task SearchMoviesAsync_NoMatchFound_ReturnsNotFound()
        {
            
            _mockMovieRepo.Setup(repo => repo.SearchMoviesAsync("Unknown", null, null))
                          .ReturnsAsync(new List<Movie>());

            
            var result = await _controller.SearchMoviesAsync("Unknown", null, null);

            
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task CreateMovieAsync_WhenMovieDoesNotExist_ReturnsOkResult()
        {
            var movieDTO = new MovieDTO
            {
                Title = "Test Movie",
                Description = "Test Description",
                ReleaseDate = new DateTime(2023, 1, 1),
                DurationMinutes = 120,
                Genre = "Action",
                Director = "John Doe",
                CategoryId = 1
            };

            _mockMovieRepo.Setup(x => x.MovieExistsAsync(movieDTO)).ReturnsAsync((Movie)null);
            _mockMovieRepo.Setup(x => x.AddMovieAsync(It.IsAny<Movie>())).ReturnsAsync(new Movie
            {
                MovieId = 1,
                Title = movieDTO.Title,
                Description = movieDTO.Description,
                ReleaseDate = movieDTO.ReleaseDate,
                DurationMinutes = movieDTO.DurationMinutes,
                Genre = movieDTO.Genre,
                Director = movieDTO.Director,
                CategoryId = movieDTO.CategoryId
            });

            var result = await _controller.CreateMovieAsync(movieDTO);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Movie added successfully", okResult.Value);
        }

        [Test]
        public async Task CreateMovieAsync_WhenMovieExists_ReturnsBadRequest()
        {
            var movieDTO = new MovieDTO
            {
                Title = "Test Movie",
                Description = "Test Description",
                ReleaseDate = new DateTime(2023, 1, 1),
                DurationMinutes = 120,
                Genre = "Action",
                Director = "John Doe",
                CategoryId = 1
            };

            _mockMovieRepo.Setup(x => x.MovieExistsAsync(movieDTO)).ReturnsAsync(new Movie());

            var result = await _controller.CreateMovieAsync(movieDTO);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Movie already exists", badRequestResult.Value);
        }

        [Test]
        public async Task UpdateMovieAsync_WhenMovieExists_UpdatesAndReturnsOkResult()
        {
            var movieId = 1;
            var updateMovieDTO = new UpdateMovieDTO
            {
                Title = "Updated Movie",
                Description = "Updated Description",
                ReleaseDate = new DateTime(2023, 1, 1),
                DurationMinutes = 150,
                Genre = "Drama",
                Director = "Jane Doe",
                CategoryId = 2
            };

            var existingMovie = new Movie
            {
                MovieId = movieId,
                Title = "Old Movie",
                Description = "Old Description",
                ReleaseDate = new DateTime(2022, 1, 1),
                DurationMinutes = 120,
                Genre = "Action",
                Director = "John Doe",
                CategoryId = 1
            };

            _mockMovieRepo.Setup(x => x.GetMovieByIdAsync(movieId)).ReturnsAsync(existingMovie);
            _mockMovieRepo.Setup(x => x.UpdateMovieAsync(It.IsAny<Movie>())).ReturnsAsync(true);

            var result = await _controller.UpdateMovieAsync(movieId, updateMovieDTO);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Movie updated successfully", okResult.Value);
            Assert.AreEqual(updateMovieDTO.Title, existingMovie.Title);
            Assert.AreEqual(updateMovieDTO.Description, existingMovie.Description);
            Assert.AreEqual(updateMovieDTO.ReleaseDate, existingMovie.ReleaseDate);
            Assert.AreEqual(updateMovieDTO.DurationMinutes, existingMovie.DurationMinutes);
            Assert.AreEqual(updateMovieDTO.Genre, existingMovie.Genre);
            Assert.AreEqual(updateMovieDTO.Director, existingMovie.Director);
            Assert.AreEqual(updateMovieDTO.CategoryId, existingMovie.CategoryId);
        }

        [Test]
        public async Task UpdateMovieAsync_WhenMovieDoesNotExist_ReturnsBadRequest()
        {
            var movieId = 1;
            var updateMovieDTO = new UpdateMovieDTO
            {
                Title = "Updated Movie",
                Description = "Updated Description",
                ReleaseDate = new DateTime(2023, 1, 1),
                DurationMinutes = 150,
                Genre = "Drama",
                Director = "Jane Doe",
                CategoryId = 2
            };

            _mockMovieRepo.Setup(x => x.GetMovieByIdAsync(movieId)).ReturnsAsync((Movie)null);

            var result = await _controller.UpdateMovieAsync(movieId, updateMovieDTO);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Movie does not exist", badRequestResult.Value);
        }


        [Test]
        public async Task DeleteMovieAsync_ShouldReturnMovieNotFound_IfMovieDoesNotExist()
        {
            int movieId = 999;
            _mockMovieRepo.Setup(repo => repo.DeleteMovieAsync(movieId)).ReturnsAsync(false);
            IActionResult result = await _controller.DeleteMovieAsync(movieId);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            NotFoundObjectResult notFoundResult = (NotFoundObjectResult)result;
            Assert.AreEqual("Movie not found.", notFoundResult.Value);
        }

        [Test]
        public async Task DeleteMovieAsync_ShouldReturnOk_IfMovieIsDeletedSuccessfully()
        {
            int movieId = 1;
            _mockMovieRepo.Setup(repo => repo.DeleteMovieAsync(movieId)).ReturnsAsync(true);
            IActionResult result = await _controller.DeleteMovieAsync(movieId);
            Assert.IsInstanceOf<OkObjectResult>(result);
            OkObjectResult okResult = (OkObjectResult)result;
            Assert.AreEqual("Movie deleted successfully.", okResult.Value);
        }

        [Test]
        public async Task FilterMoviesByCategoryAsync_ValidCategoryId_ReturnsOkResultWithMovies()
        {
            var categoryId = 1;
            var movies = new List<Movie>
            {
                new Movie { Id = 1, Title = "Movie 1", CategoryId = categoryId },
                new Movie { Id = 2, Title = "Movie 2", CategoryId = categoryId }
            };

            _mockMovieRepo.Setup(repo => repo.GetMoviesByCategoryAsync(categoryId))
                          .ReturnsAsync(movies);

            var result = await _controller.FilterMoviesByCategoryAsync(categoryId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            var returnedMovies = okResult.Value as IEnumerable<Movie>;

            Assert.That(returnedMovies, Is.EqualTo(movies));
        }

        [Test]
        public async Task FilterMoviesByCategoryAsync_NoMoviesFound_ReturnsNotFoundResult()
        {
            var categoryId = 1;

            _mockMovieRepo.Setup(repo => repo.GetMoviesByCategoryAsync(categoryId))
                          .ReturnsAsync(new List<Movie>());

            var result = await _controller.FilterMoviesByCategoryAsync(categoryId);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.That(notFoundResult.Value, Is.EqualTo("No movies found for the specified category."));
        }


    }
}
