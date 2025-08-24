using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReelFlow.DTO;
using ReelFlow.Models;
using ReelFLow.Repository;
using Microsoft.AspNetCore.Authorization;


namespace ReelFLow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }
        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return Ok(categories);

        }

        [HttpGet("categories/{id}")]
        public async Task<IActionResult> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { Message = "Category not found." });
            }
            return Ok(category);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNewCategoryAsync([FromBody] CategoryDTO categoryDTO)
        {
            var categoryExists = await _categoryRepository.CategoryExistsAsync(categoryDTO.Name ?? string.Empty);
            if (categoryExists != null)
            {
                return BadRequest(new { Message = "Category already exists" });
            }
            Category newCategory = new Category
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description
            };
            var result = await _categoryRepository.CreateCategoryAsync(newCategory);
        
            return Ok(result);
        }


        [HttpPut("categories/{id}")]
        public async Task<IActionResult> UpdateCategoryAsync(int id, [FromBody] CategoryDTO categoryDto)
        {
            var existingCategory = await _categoryRepository.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound(new { Message = "Category not found." });
            }

            existingCategory.Name = categoryDto.Name;
            existingCategory.Description = categoryDto.Description;

            await _categoryRepository.UpdateCategoryAsync(existingCategory);
            return Ok(new { Message = "Category updated successfully." });
        }


        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var existingCategory = await _categoryRepository.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound(new { Message = "Category not found." });
            }

            bool isDeleted = await _categoryRepository.DeleteCategoryAsync(existingCategory);

            if (!isDeleted)
            {
                return BadRequest(new { Message = "Cannot delete category because it has associated movies." });
            }

            return Ok(new { Message = "Category deleted successfully." });
        }

        //[HttpGet("categories/search")]
        //public async Task<IActionResult> SearchCategoriesAsync(string name)
        //{

        //}

    }
}
