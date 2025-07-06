create database ReelflowDB;
go

use ReelflowDB;
go

create table Movies(
MovieId int identity(1,1) primary key,
MovieName nvarchar(100) not null,
MovieInfo nvarchar(max),
ReleaseYear int,
Duration int,
FilePath nvarchar(500),
PosterPath nvarchar(500),
DirectorId int,
GenreId int
foreign key (DirectorId) references Directors(DirectorId),
foreign key (GenreId) references Genres(GenreId)
);
go

--Directors Table
CREATE TABLE Directors(
DirectorId int primary key identity(1,1),
DirectorName nvarchar(255) NOT NULL,
DirectorBio nvarchar(max)
);
GO
=======

CREATE TABLE Genres (
    GenreId INT PRIMARY KEY IDENTITY(1,1),
    GenreName VARCHAR(100) NOT NULL
);
go


CREATE TABLE MovieActors (
    MovieId INT NOT NULL,
    ActorId INT NOT NULL,
    PRIMARY KEY (MovieId, ActorId),
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId),
    FOREIGN KEY (ActorId) REFERENCES Actors(ActorId)
);
go

