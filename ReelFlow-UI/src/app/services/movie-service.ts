import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { MovieReadDto } from '../models/Movies/MovieReadDto';
import { MovieDto } from '../models/categories/movieDto';
import { Observable } from 'rxjs';
import { MovieUpdateDto } from '../models/Movies/MovieUpdateDto';

const apiUrl = 'http://localhost:5231/api/Movie';

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  private http = inject(HttpClient);
  getMovies():Observable<MovieReadDto[]>
  {
    return this.http.get<MovieReadDto[]>(apiUrl+'/movies');
  }
  

  createMovie(movie: MovieDto):Observable<MovieReadDto> {
    return this.http.post<MovieReadDto>(apiUrl+"/add", movie);
  }

  searchMovies(title?: string, description?: string, releaseDate?: string): Observable<MovieReadDto[]> {
  const params: any = {};
  if (title) params.title = title;
  if (description) params.description = description;
  if (releaseDate) params.releaseDate = releaseDate;

  return this.http.get<MovieReadDto[]>(`${apiUrl}/movies/search`, { params });
}

   getMoviesByCategory(categoryId: number) {
     return this.http.get<MovieReadDto[]>(`${apiUrl}/movies/filter/${categoryId}`);
  }

    // Update an existing product
  updateMovie(movieId: number, movie: MovieReadDto): Observable<any> {
    return this.http.put(`${apiUrl}/movies/${movieId}`, movie);
  }

deleteMovie(id: number): Observable<any> {
  return this.http.delete<any>(`${apiUrl}/movies/${id}`);
}
}