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
