IF DB_ID(N'RealEstate') IS NULL
BEGIN
  CREATE DATABASE RealEstate;
END
GO
USE RealEstateDB;
GO

IF OBJECT_ID('dbo.PropertyTrace', 'U') IS NOT NULL DROP TABLE dbo.PropertyTrace;
IF OBJECT_ID('dbo.PropertyImage', 'U') IS NOT NULL DROP TABLE dbo.PropertyImage;
IF OBJECT_ID('dbo.Property', 'U') IS NOT NULL DROP TABLE dbo.Property;
IF OBJECT_ID('dbo.Owner', 'U') IS NOT NULL DROP TABLE dbo.Owner;
GO

CREATE TABLE dbo.Owner(
  IdOwner     UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Owner PRIMARY KEY,
  Name        NVARCHAR(150)    NOT NULL,
  Address     NVARCHAR(200)    NULL,
  Photo       NVARCHAR(500)    NULL,
  Birthday    DATE             NULL
);
GO

CREATE TABLE dbo.Property(
  IdProperty   UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Property PRIMARY KEY,
  Name         NVARCHAR(150)    NOT NULL,
  Address      NVARCHAR(200)    NOT NULL,
  Price        DECIMAL(18,2)    NOT NULL,
  CodeInternal NVARCHAR(50)     NOT NULL,
  [Year]       SMALLINT         NULL,
  IdOwner      UNIQUEIDENTIFIER NOT NULL,
  CreatedAt    DATETIME2(0)     NOT NULL CONSTRAINT DF_Property_CreatedAt DEFAULT(SYSDATETIME()),
  UpdatedAt    DATETIME2(0)     NOT NULL CONSTRAINT DF_Property_UpdatedAt DEFAULT(SYSDATETIME())
);
GO

ALTER TABLE dbo.Property WITH CHECK
ADD CONSTRAINT FK_Property_Owner
FOREIGN KEY(IdOwner) REFERENCES dbo.Owner(IdOwner);

ALTER TABLE dbo.Property WITH CHECK
ADD CONSTRAINT CK_Property_Price_Positive CHECK (Price > 0);

CREATE UNIQUE INDEX UX_Property_CodeInternal ON dbo.Property(CodeInternal);
CREATE INDEX IX_Property_Price      ON dbo.Property(Price);
CREATE INDEX IX_Property_Year       ON dbo.Property([Year]);
CREATE INDEX IX_Property_CreatedAt  ON dbo.Property(CreatedAt);
GO

CREATE TABLE dbo.PropertyImage(
  IdPropertyImage UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_PropertyImage PRIMARY KEY,
  IdProperty      UNIQUEIDENTIFIER NOT NULL,
  [File]          NVARCHAR(500)    NOT NULL,
  Enabled         BIT              NOT NULL    CONSTRAINT DF_PropertyImage_Enabled DEFAULT(1),
  CreatedAt       DATETIME2(0)     NOT NULL    CONSTRAINT DF_PropertyImage_CreatedAt DEFAULT(SYSDATETIME())
);
GO

ALTER TABLE dbo.PropertyImage WITH CHECK
ADD CONSTRAINT FK_PropertyImage_Property
FOREIGN KEY(IdProperty) REFERENCES dbo.Property(IdProperty);

CREATE INDEX IX_PropertyImage_Property_Enabled ON dbo.PropertyImage(IdProperty, Enabled);
GO

CREATE TABLE dbo.PropertyTrace(
  IdPropertyTrace UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_PropertyTrace PRIMARY KEY,
  IdProperty      UNIQUEIDENTIFIER NOT NULL,
  DateSale        DATE             NOT NULL,
  [Name]          NVARCHAR(150)    NOT NULL,
  [Value]         DECIMAL(18,2)    NOT NULL,
  Tax             DECIMAL(18,2)    NOT NULL
);
GO

ALTER TABLE dbo.PropertyTrace WITH CHECK
ADD CONSTRAINT FK_PropertyTrace_Property
FOREIGN KEY(IdProperty) REFERENCES dbo.Property(IdProperty);

CREATE INDEX IX_PropertyTrace_Property_DateSale ON dbo.PropertyTrace(IdProperty, DateSale);
GO