
using ReelFlow.DTO;
using ReelFlow.Models;

namespace ReelFLow.Repository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category> CreateCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(Category category);
        // Task<bool> SearchCategoriesAsync(string name);
        Task<Category>CategoryExistsAsync(string categoryName);
    }
}
