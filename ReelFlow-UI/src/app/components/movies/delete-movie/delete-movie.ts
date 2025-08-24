import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MovieService } from '../../../services/movie-service';

@Component({
  selector: 'app-delete-movie',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './delete-movie.html'
})
export class DeleteMovieComponent {
  @Input() movieId!: number; // now passed as movie.movieId
  @Output() deleted = new EventEmitter<number>();

  constructor(private movieService: MovieService) {}

  onDelete() {
    if (confirm('Are you sure you want to delete this movie?')) {
      this.movieService.deleteMovie(this.movieId).subscribe({
        next: () => {
          alert('Movie deleted successfully');
          this.deleted.emit(this.movieId);
        },
        error: err => {
          console.error(err);
          alert('Failed to delete movie');
        }
      });
    }
  }
}
