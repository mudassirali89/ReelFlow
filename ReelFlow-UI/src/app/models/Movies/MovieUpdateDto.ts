export interface MovieUpdateDto {
  movieId: number;
  title: string;
  releaseDate: Date;
  genre: string;
  description: string;
  durationMinutes: number;
  director: string;
  categoryId: number;
}