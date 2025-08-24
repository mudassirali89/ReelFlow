using System.ComponentModel.DataAnnotations;

namespace ReelFLow.DTO
{
    public class UpdateMovieDTO
    {
        //[Required(ErrorMessage = "Movie ID is required.")]
        //public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string Title { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [StringLength(100, ErrorMessage = "Genre cannot exceed 100 characters.")]
        public string Genre { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes.")]
        public int DurationMinutes { get; set; }

        [StringLength(255, ErrorMessage = "Director name cannot exceed 255 characters.")]
        public string Director { get; set; }

    public int CategoryId { get; set; }
    }
}
