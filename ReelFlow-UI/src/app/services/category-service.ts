import { inject, Injectable } from '@angular/core';
import { CategoryDto } from '../models/categories/categoryDto';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

const apiUrl = 'http://localhost:5231/api/Category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private http = inject(HttpClient);
  getCategories():Observable<CategoryDto[]> {
    return this.http.get<CategoryDto[]>(apiUrl+'/categories');
  }

  createCategory(category: CategoryDto): Observable<CategoryDto> {
    return this.http.post<CategoryDto>(apiUrl+"/add", category);
  }
  deleteCategory(id?: number): Observable<any> {
    return this.http.delete(`${apiUrl}/categories/${id}`);
  }
  updateCategory(id: number, category: CategoryDto): Observable<any> {
  return this.http.put(`${apiUrl}/categories/${id}`, category);
}

}
