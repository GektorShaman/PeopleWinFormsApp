������� WinForms ���������� (CRUD) � �������������� Entity Framework Core � MS SQL Server.
��� �������� ���� ������:
CREATE TABLE [dbo].[People]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(200) NOT NULL, 
    [Age] INT NOT NULL, 
    [Birthdate] DATETIME NOT NULL
)