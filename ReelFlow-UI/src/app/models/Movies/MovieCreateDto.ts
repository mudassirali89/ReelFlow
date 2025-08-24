export interface MovieCreateDto{
  title: string;
  releaseDate: Date;
  genre: string;
  description: string;
  durationMinutes: number;
  director: string;
  categoryId: number;
}