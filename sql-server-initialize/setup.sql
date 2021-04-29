USE [master]
GO

IF NOT EXISTS(SELECT * FROM sys.databases WHERE [name]='OrdersDemo')
	CREATE DATABASE [OrdersDemo]
GO

USE [OrdersDemo]
GO

IF OBJECT_ID('Customer') IS NOT NULL
	DROP TABLE [Customer]
GO

CREATE TABLE [Customer](
  [CustomerId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED,
  [FirstName] VARCHAR(50) NOT NULL,
  [LastName] VARCHAR(50) NOT NULL
)
GO

IF EXISTS (SELECT * from [master]..syslogins WHERE [name] = 'OrdersApp')
	DROP LOGIN [OrdersApp]
GO

CREATE LOGIN [OrdersApp] WITH PASSWORD='Password@12345'
GO

IF EXISTS (SELECT * FROM sysusers WHERE [name] = 'OrdersApp')
	DROP USER [OrdersApp]
GO

CREATE USER [OrdersApp] FOR LOGIN [OrdersApp]
GO

ALTER ROLE db_datareader
  ADD MEMBER OrdersApp
GO

ALTER ROLE db_datawriter
  ADD MEMBER OrdersApp
GO
