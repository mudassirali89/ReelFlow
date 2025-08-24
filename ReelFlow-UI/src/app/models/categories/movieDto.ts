export interface MovieDto {
    title: string;
    releaseDate: Date;
    genre: string;
    description: string;

    durationMinutes: number;
    director: string;
    categoryId: number;
}
