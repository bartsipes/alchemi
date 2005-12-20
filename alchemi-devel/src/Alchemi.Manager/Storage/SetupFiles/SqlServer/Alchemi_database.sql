IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'Alchemi')
	DROP DATABASE [Alchemi];

CREATE DATABASE [Alchemi];
