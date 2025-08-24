import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common'; // ✅ Import here
import { CategoryDto } from '../../../models/categories/categoryDto';
import { CategoryService } from '../../../services/category-service';

@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [CommonModule], // ✅ Add CommonModule so *ngFor, *ngIf work
  templateUrl: './category-list.html',
  styleUrls: ['./category-list.css']
})
export class CategoryList {
  constructor(private categoryService: CategoryService) {}
  @Input() categories: CategoryDto[] = [];
  @Input() loading:boolean=false;
  @Output() deleteCategory = new EventEmitter<number>();
  @Output() updateCategory = new EventEmitter<CategoryDto>();
  

  onDelete(id?: number) {
    if (confirm(`Are you sure you want to delete this category?`)) {
      this.categoryService.deleteCategory(id).subscribe({
        next: () => {
          this.deleteCategory.emit(id);
          alert('Category deleted successfully!');
        },
        error: (err: any) => {
          console.error('Error deleting category:', err);
          alert(err.error?.message || 'Failed to delete category. Please try again.');
        }
      });
    }
  }

  onUpdate(category: CategoryDto) {
    this.updateCategory.emit(category);
  }
}
