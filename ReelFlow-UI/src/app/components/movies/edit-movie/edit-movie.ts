import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { MovieService } from '../../../services/movie-service';
import { MovieReadDto } from '../../../models/Movies/MovieReadDto';
import { MovieCategoryDto } from '../../../models/Movies/MovieCategoryDto';
import { MovieCreateDto } from '../../../models/Movies/MovieCreateDto';

interface MovieUpdateDto extends MovieReadDto {
  movieId: number;
}
@Component({
  selector: 'app-edit-movie',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './edit-movie.html',
  styleUrls: ['./edit-movie.css']
})
export class EditMovie {
  @Input() categories: MovieCategoryDto[] = [];
  @Input() loading: boolean = false;
  @Input() showEditForm: boolean = false;
  @Input() selectedmovie: MovieReadDto | null = null;
  
  @Output() movieUpdated = new EventEmitter<MovieReadDto>();
  @Output() formClosed = new EventEmitter<void>();

  @Input() movieModel: MovieReadDto = {
    movieId: 0,
    title: '',
    releaseDate: new Date(),
    genre: '',
    description: '',
    durationMinutes: 0,
    director: '',
    categoryId: 0
};

constructor(private movieService: MovieService) {}

  onEditSubmit(form: NgForm) {
    if (form.valid && this.movieModel.movieId !== undefined) {
      this.loading = true;
      this.movieService.updateMovie(this.movieModel.movieId, this.movieModel).subscribe({
        next: () => {
          this.movieUpdated.emit(this.movieModel);
          this.loading = false;
          this.onCancel();
        },
        error: (err: any) => {
          console.error('Error updating movie:', err);
          alert(err.error?.message || 'Failed to update movie. Please try again.');
          this.loading = false;
        }
      });
    }
  }

  onCancel() {
    this.formClosed.emit();
  }
}