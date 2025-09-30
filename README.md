Простое WinForms приложение (CRUD) с использованием Entity Framework Core и MSSQL Server.
Код создания базы данных:
CREATE TABLE [dbo].[People]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(200) NOT NULL, 
    [Age] INT NOT NULL, 
    [Birthdate] DATETIME NOT NULL
)