import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-search-movie',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './search-movie.html',
  styleUrls: ['./search-movie.css']
})
export class SearchMovie {
  title: string = '';
  description: string = '';
  releaseDate: string = '';

  @Output() search = new EventEmitter<{ title: string, description: string, releaseDate: string }>();
  @Output() reset = new EventEmitter<void>();
  @Output() close = new EventEmitter<void>();

  onSearchClick() {
    this.search.emit({
      title: this.title,
      description: this.description,
      releaseDate: this.releaseDate
    });
  }

  onResetClick() {
    this.title = '';
    this.description = '';
    this.releaseDate = '';
    this.reset.emit();
  }

  onValueChange(event: Event) {
    const isAllEmpty = !this.title.trim() && !this.description.trim() && !this.releaseDate.trim();

  if (isAllEmpty) {
    this.reset.emit();
  } else {
    this.onSearchClick();
  }
  }

  onClose(){
    this.close.emit();
    this.reset.emit();
  }
}
