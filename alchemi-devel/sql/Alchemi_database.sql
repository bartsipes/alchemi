IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'Alchemi')
	DROP DATABASE [Alchemi]
GO

CREATE DATABASE [Alchemi]

GO
