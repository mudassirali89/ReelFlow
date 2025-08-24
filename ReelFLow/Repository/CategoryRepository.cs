
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ReelFlow.DTO;
using ReelFlow.Models;
using ReelFLow.Context;

namespace ReelFLow.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(Category category)
        {
            var categoryMovies = await _context.Categories
           .Include(c => c.Movies)
           .FirstOrDefaultAsync(c => c.Id == category.Id);
            if (categoryMovies.Movies != null && categoryMovies.Movies.Any())
            {
                return false;
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
             return await _context.Categories.FindAsync(id);
        }

        // public Task<bool> SearchCategoriesAsync(string name)
        // {
        // }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<Category> CategoryExistsAsync(string categoryName)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(category => category.Name == categoryName);
            return category;
        }
    }
}
