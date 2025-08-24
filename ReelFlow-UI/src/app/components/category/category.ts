import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CategoryDto } from '../../models/categories/categoryDto';
import { MovieDto } from '../../models/categories/movieDto';
import { AuthService } from '../../services/auth/authservice';
import { MovieService } from '../../services/movie-service';
import { CategoryService } from '../../services/category-service';
import { AddCategory } from "./add-category/add-category";
import { CategoryList } from './category-list/category-list';
import { EditCategory } from "./edit-category/edit-category";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-category',
  imports: [AddCategory, CategoryList, EditCategory, CommonModule],
  templateUrl: './category.html',
  styleUrl: './category.css'
})
export class Category implements OnInit {
  constructor(
    private categoryService: CategoryService,
    private movieService: MovieService,
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) { 
    this.showCreateForm = false;
    this.showEditForm = false;
  }

  showCreateForm: boolean = false;
  showEditForm: boolean = false;
  categories: CategoryDto[] = [];
  movies: MovieDto[] = [];
  error: string = '';
  loading: boolean = false;
  editCategoryData: CategoryDto = { id: 0, name: '', description: '' };

  ngOnInit() {
    if (this.isAuthenticated()) {
      this.loadCategories();
      this.loadMovies();
    }
  }

  showAddCategoryForm() {
    this.showCreateForm = true;
  }

  isAuthenticated() {
    return this.authService.isAuthenticated();
  }

  loadMovies() {
    this.movieService.getMovies().subscribe({
      next: (movies: MovieDto[]) => {
        console.log('Movies loaded:', movies);
        this.movies = movies;
      },
      error: (err: any) => {
        console.error('Error loading movies:', err);
      }
    });
  }

  loadCategories() {
    this.loading = true;
    this.categoryService.getCategories().subscribe({
      next: (categories: CategoryDto[]) => {
        console.log('Categories loaded:', categories);
        this.categories = categories;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        console.error('Error loading categories:', err);
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  onCategoryCreated(newCategory: CategoryDto) {
    this.categories.push(newCategory);
    this.loadCategories();
    this.showCreateForm = false;
    this.loading = false;
  }

  onCreateFormClosed() {
    this.showCreateForm = false;
  }

  handleDeleteCategory(id: number) {
    this.categories = this.categories.filter(c => c.id !== id);
  }

  handleUpdateCategory(category: CategoryDto) {
    this.editCategoryData = { ...category };
    this.showEditForm = true;
  }

  onCategoryUpdated(updated: CategoryDto) 
  {  
    alert(updated.name+' Category updated successfully!');
    this.showEditForm = false;
  }

  onEditFormClosed() {
    this.loadMovies();
    this.loadCategories();
    this.showEditForm = false;
    this.cdr.detectChanges();
  }
}