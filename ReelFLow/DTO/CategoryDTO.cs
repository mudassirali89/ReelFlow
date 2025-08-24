using System.ComponentModel.DataAnnotations;

namespace ReelFlow.DTO
{
    public class CategoryDTO
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]

        public string Description { get; set; }
    }
}