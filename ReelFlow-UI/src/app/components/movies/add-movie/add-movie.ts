// add-movie.ts
import { Component, EventEmitter, Output, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { MovieDto } from '../../../models/categories/movieDto';
import { MovieService } from '../../../services/movie-service';
import { CategoryDto } from '../../../models/categories/categoryDto';
import { MovieReadDto } from '../../../models/Movies/MovieReadDto';

@Component({
  selector: 'app-add-movie',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-movie.html',
  styleUrls: ['./add-movie.css']
})
export class AddMovie {
  @Input() showCreateForm: boolean = false;
  @Input() loading: boolean = false;
  @Input() categories: CategoryDto[] = [];
  @Output() movieCreated = new EventEmitter<MovieReadDto>(); 
  @Output() formClosed = new EventEmitter<void>();

  movieModel: MovieDto = {
    title: '',
    releaseDate: new Date(),
    genre: '',
    description: '',
    durationMinutes: 0,
    director: '',
    categoryId: 0
  };

  constructor(private movieService: MovieService) {}

  addMovie(form: NgForm) {
    if (form.valid) {
      this.movieService.createMovie(this.movieModel).subscribe({
        next: (newMovie: MovieReadDto) => { 
          this.movieCreated.emit(newMovie); 
          this.resetMovieModel();
          form.resetForm();
          alert('Movie created successfully!');
        },
        error: (err: any) => {
          console.error('Error creating movie:', err);
          alert(err.error?.message || 'Failed to create movie. Please try again.');
        }
      });
    }
  }

  onClose() {
    this.resetMovieModel();
    this.formClosed.emit();
  }

  resetMovieModel() {
    this.movieModel = {
      title: '',
      releaseDate: new Date(),
      genre: '',
      description: '',
      durationMinutes: 0,
      director: '',
      categoryId: 0
    };
  }
}