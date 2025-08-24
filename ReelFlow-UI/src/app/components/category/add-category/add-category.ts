import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Input } from '@angular/core';
import { CategoryDto } from '../../../models/categories/categoryDto';
import { CategoryService } from '../../../services/category-service';
@Component({
  selector: 'app-add-category',
  imports: [CommonModule,FormsModule],
  templateUrl: './add-category.html',
  styleUrl: './add-category.css'
})
export class AddCategory {
  @Input() showCreateForm: boolean = false;
  @Input() loading:boolean=false;
  @Output() categoryCreated = new EventEmitter<CategoryDto>();
  @Output() formClosed = new EventEmitter<void>();
  constructor(private categoryService: CategoryService) {}
  categoryModel:CategoryDto={
    name: '',
    description: ''
  }
  createCategory(form:NgForm)
  {
    if (form.valid) {
      console.log(form.value);
      this.categoryService.createCategory(this.categoryModel).subscribe({
        next: (newCategory: CategoryDto) => {
          this.categoryCreated.emit(newCategory);
          this.resetCategoryModel();
          form.resetForm();
          alert('Category created successfully!');
        },
        error: (err: any) => {
          console.error('Error creating category:', err);
          alert(err.error?.message || 'Failed to create category. Please try again.');
        }
      });
    }
  }
  onClose() {
    this.resetCategoryModel();
    this.formClosed.emit();
  }
  resetCategoryModel() {
    this.categoryModel = {
      name: '',
      description: ''
    };
  }
}
