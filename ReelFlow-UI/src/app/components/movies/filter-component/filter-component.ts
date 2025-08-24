import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CategoryDto } from '../../../models/categories/categoryDto';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-filter-component',
  imports: [CommonModule],
  templateUrl: './filter-component.html',
  styleUrls: ['./filter-component.css']
})
export class FilterComponent {
  @Input() categories: CategoryDto[] = [];
  @Output() categorySelected = new EventEmitter<number | null | undefined>();

  onFilterCategory(categoryId: number | null | undefined) {
    this.categorySelected.emit(categoryId);
  }
}
