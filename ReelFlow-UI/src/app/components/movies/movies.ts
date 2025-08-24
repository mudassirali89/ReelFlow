import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CategoryDto } from '../../models/categories/categoryDto';
import { MovieDto } from '../../models/categories/movieDto';
import { MovieReadDto } from '../../models/Movies/MovieReadDto';
import { AuthService } from '../../services/auth/authservice';
import { MovieService } from '../../services/movie-service';
import { CategoryService } from '../../services/category-service';
import { AddMovie } from './add-movie/add-movie';
import { MovieListComponent } from './movie-list.component/movie-list.component';
import { MovieCategoryDto } from '../../models/Movies/MovieCategoryDto';
import { FilterComponent } from './filter-component/filter-component';
import { SearchMovie } from './search-movie/search-movie';
import { EditMovie } from "./edit-movie/edit-movie";

@Component({
  selector: 'app-movies',
  imports: [CommonModule, FormsModule, AddMovie, MovieListComponent, FilterComponent, SearchMovie, EditMovie],
  templateUrl: './movies.html',
  styleUrls: ['./movies.css']
})
export class Movies implements OnInit {
  constructor(
    private categoryService: CategoryService,
    private movieService: MovieService,
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  showCreateForm: boolean = false;
  showEditForm:boolean=false;
  editMovieData:MovieReadDto={title:'',description:'',releaseDate:new Date(),durationMinutes:0,director:'',genre:'',movieId:0,categoryId:0};
  showSearchForm = false;
  categories: CategoryDto[] = [];
  movieCategories: MovieCategoryDto[] = [];
  movies: MovieReadDto[] = [];
  loading: boolean = false;

  // Search fields
  searchTitle: string = '';
  searchDescription: string = '';
  searchReleaseDate: string = '';

  ngOnInit() {
    if (this.authService.isAuthenticated()) {
      this.loadMovies();
      this.loadCategories();
    }
  }

  showAddMovieForm() {
    this.showCreateForm = true;
  }
 toggleSearchForm() {
    this.showSearchForm = !this.showSearchForm;
  }
  loadMovies() {
    this.loading = true;
    this.movieService.getMovies().subscribe({
      next: (movies: MovieReadDto[]) => {
        this.movies = movies;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        console.error('Error loading movies:', err);
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  loadCategories() {
    this.categoryService.getCategories().subscribe({
      next: (categories: any[]) => {
        console.log('Categories API Response:', categories);
        this.categories = categories.map(cat => ({
          id: cat.id,
          name: cat.name,
          description: cat.description
        }));

        this.movieCategories = categories.map(cat => ({
          id: cat.id,
          name: cat.name
        }));
        this.cdr.detectChanges();
      },
      error: (err: any) => console.error('Error loading categories:', err)
    });
  }

  onMovieCreated(newMovie: MovieReadDto) {
    this.movies.push(newMovie);
    this.cdr.detectChanges();
    this.showCreateForm = false;
    this.loading = false;
  }

  onCreateFormClosed() {
    this.showCreateForm = false;
  }

  // Search logic
onSearchMovies(criteria: { title: string; description: string; releaseDate: string }) {
    if (!criteria.title && !criteria.description && !criteria.releaseDate) {
      alert('Please provide at least one search parameter.');
      return;
    }

    this.loading = true;
    this.movieService
      .searchMovies(criteria.title, criteria.description, criteria.releaseDate)
      .subscribe({

        next: (results: MovieReadDto[]) => {
          this.movies = results;
          this.loading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          this.loading = false;
          if (err.status === 404) {
            this.movies = [];
          } else {
            console.error('Search error:', err);
          }
          this.cdr.detectChanges();
        }
      });
  }

  onResetMovies() {
    this.searchTitle = '';
    this.searchDescription = '';
    this.searchReleaseDate = '';
    this.loadMovies();
  }
  
  onMovieDeleted(deletedId: number) {
    this.movies = this.movies.filter(m => m.movieId !== deletedId);
  }
  onFilterCategory(categoryId: number | null | undefined) 
  {
    if (categoryId === null || categoryId === undefined) {
      this.loadMovies();
      return;
    }

    this.loading = true;
    this.movieService.getMoviesByCategory(categoryId).subscribe({
      next: (filteredMovies: MovieReadDto[]) => {
        this.movies = filteredMovies;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error filtering movies:', err);
        this.movies = [];
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  onEditFormClosed()
  {
    this.loadMovies();
    this.loadCategories();
    this.showEditForm = false;
    this.cdr.detectChanges();
  }
  onMovieUpdated(updatedMovie: MovieReadDto) 
  {
    alert(updatedMovie.title+' Movie updated successfully!');
    this.showEditForm = false;
  }

  handleUpdateMovie(updateMovie:MovieReadDto)
  {
    this.editMovieData = { ...updateMovie };
    this.showEditForm=true;
  }

  
}