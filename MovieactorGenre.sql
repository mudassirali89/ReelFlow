create database ReelflowDB;
go

use ReelflowDB;
go

CREATE TABLE Genres (
    GenreId INT PRIMARY KEY IDENTITY(1,1),
    GenreName VARCHAR(100) NOT NULL
)
go


CREATE TABLE MovieActors (
    MovieId INT NOT NULL,
    ActorId INT NOT NULL,
    PRIMARY KEY (MovieId, ActorId),
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId),
    FOREIGN KEY (ActorId) REFERENCES Actors(ActorId)
)
go