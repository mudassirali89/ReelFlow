import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { CategoryDto } from '../../../models/categories/categoryDto';
import { CategoryService } from '../../../services/category-service';

@Component({
  selector: 'app-edit-category',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './edit-category.html',
  styleUrls: ['./edit-category.css']
})
export class EditCategory {
  @Input() showEditForm: boolean = false;
  @Input() loading: boolean = false;
  @Input() categoryModel: CategoryDto = { id: 0, name: '', description: '' };

  @Output() categoryUpdated = new EventEmitter<CategoryDto>();
  @Output() formClosed = new EventEmitter<void>();

  constructor(private categoryService: CategoryService) {}

  updateCategory(form: NgForm) {
    if (form.valid && this.categoryModel.id !== undefined) {
      this.loading = true;
      this.categoryService.updateCategory(this.categoryModel.id, this.categoryModel).subscribe({
        next: () => {
          this.categoryUpdated.emit(this.categoryModel);
          this.loading = false;
          this.onClose();
        },
        error: (err: any) => {
          console.error('Error updating category:', err);
          alert(err.error?.message || 'Failed to update category. Please try again.');
          this.loading = false;
        }
      });
    }
  }

  onClose() {
    this.formClosed.emit();
  }
}
