import { ChangeDetectorRef, Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MovieCategoryDto } from '../../../models/Movies/MovieCategoryDto';
import { MovieService } from '../../../services/movie-service';
import { MovieReadDto } from '../../../models/Movies/MovieReadDto';
import { DeleteMovieComponent } from '../delete-movie/delete-movie';
import { MovieDto } from '../../../models/categories/movieDto';

@Component({
  selector: 'app-movie-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './movie-list.component.html',
  styleUrls: ['./movie-list.component.css']
})
export class MovieListComponent {
  @Input() movies: MovieReadDto[] = [];
  @Input() categories: MovieCategoryDto[] = [];
  @Input() loading: boolean = false;
  @Output() updateMovie = new EventEmitter<MovieReadDto>();


  constructor(private movieService: MovieService, private cdr: ChangeDetectorRef) {
  }

  getCategoryName(categoryId: number): string {
    const category = this.categories.find(c => c.id === categoryId);
    return category ? category.name : 'Unknown';
  }
    onUpdate(movie: MovieReadDto): void{
      this.updateMovie.emit(movie);
    }

  onDelete(movieId: number): void {
    console.log('Attempting to delete movieId:', movieId);

    if (!movieId) {
      console.error('Invalid movieId:', movieId);
      alert('Invalid movie ID');
      return;
    }

    if (confirm('Are you sure you want to delete this movie?')) {
      this.movieService.deleteMovie(movieId).subscribe({
        next: (res) => {
          console.log('Delete API success:', res);
          alert('Movie deleted successfully');

          // Remove movie from list
          this.movies = this.movies.filter(m => m.movieId != movieId);
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error('Delete API error:', err);
          alert('Failed to delete movie');
        }
      });
    }
  }
}
