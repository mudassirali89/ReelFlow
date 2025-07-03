CREATE DATABASE IF NOT EXISTS ReelFlowDB;
GO
USE ReelFlowDB;
GO
CREATE TABLE Directors(
DirectorId int primary key identity(1,1),
DirectorName nvarchar(255) NOT NULL,
DirectorBio nvarchar(max)
);
GO