
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/08/2020 18:03:53
-- Generated from EDMX file: C:\Users\HP\source\repos\CollegeBuddy\WebApplication3\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [CollegeBuddy];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AdditionalSubjects_Details]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AdditionalSubjects] DROP CONSTRAINT [FK_AdditionalSubjects_Details];
GO
IF OBJECT_ID(N'[dbo].[FK_AdditionalSubjects_SubjectTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AdditionalSubjects] DROP CONSTRAINT [FK_AdditionalSubjects_SubjectTable];
GO
IF OBJECT_ID(N'[dbo].[FK_CommentTable_DashboardAnswer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CommentTable] DROP CONSTRAINT [FK_CommentTable_DashboardAnswer];
GO
IF OBJECT_ID(N'[dbo].[FK_DashboardAnswer_DashboardAnswer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DashboardAnswer] DROP CONSTRAINT [FK_DashboardAnswer_DashboardAnswer];
GO
IF OBJECT_ID(N'[dbo].[FK_DashboardAnswer_DashboardAnswer1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DashboardAnswer] DROP CONSTRAINT [FK_DashboardAnswer_DashboardAnswer1];
GO
IF OBJECT_ID(N'[dbo].[FK_LibraryTable_Details]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LibraryTable] DROP CONSTRAINT [FK_LibraryTable_Details];
GO
IF OBJECT_ID(N'[dbo].[FK_LibraryTable_PDFTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LibraryTable] DROP CONSTRAINT [FK_LibraryTable_PDFTable];
GO
IF OBJECT_ID(N'[dbo].[FK_PDFDetails_PDFTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PDFDetails] DROP CONSTRAINT [FK_PDFDetails_PDFTable];
GO
IF OBJECT_ID(N'[dbo].[FK_PDFTable_PDFTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PDFTable] DROP CONSTRAINT [FK_PDFTable_PDFTable];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AdditionalSubjects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AdditionalSubjects];
GO
IF OBJECT_ID(N'[dbo].[BranchTable]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BranchTable];
GO
IF OBJECT_ID(N'[dbo].[CommentTable]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CommentTable];
GO
IF OBJECT_ID(N'[dbo].[DashboardAnswer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DashboardAnswer];
GO
IF OBJECT_ID(N'[dbo].[DashboardQuestion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DashboardQuestion];
GO
IF OBJECT_ID(N'[dbo].[Details]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Details];
GO
IF OBJECT_ID(N'[dbo].[LibraryTable]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LibraryTable];
GO
IF OBJECT_ID(N'[dbo].[PDFDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PDFDetails];
GO
IF OBJECT_ID(N'[dbo].[PDFTable]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PDFTable];
GO
IF OBJECT_ID(N'[dbo].[SubjectTable]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubjectTable];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Details'
CREATE TABLE [dbo].[Details] (
    [ID] int  NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [College] varchar(50)  NOT NULL,
    [Branch] varchar(50)  NOT NULL,
    [Year] varchar(50)  NOT NULL,
    [MobileNo] nvarchar(50)  NOT NULL,
    [Image] nvarchar(max)  NULL,
    [Password] nvarchar(50)  NOT NULL,
    [AuthCode] nvarchar(10)  NULL,
    [IsVerified] bit  NULL,
    [datetime] datetime  NULL
);
GO

-- Creating table 'DashboardAnswers'
CREATE TABLE [dbo].[DashboardAnswers] (
    [AID] int  NOT NULL,
    [QID] int  NOT NULL,
    [Answer] nvarchar(max)  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Upvotes] int  NULL,
    [identify] int  NULL
);
GO

-- Creating table 'DashboardQuestions'
CREATE TABLE [dbo].[DashboardQuestions] (
    [QID] int  NOT NULL,
    [Question] nvarchar(max)  NOT NULL,
    [Datetime] datetime  NOT NULL,
    [ID] nvarchar(50)  NOT NULL,
    [Number] int  NULL,
    [CollegeName] nvarchar(50)  NULL
);
GO

-- Creating table 'AdditionalSubjects'
CREATE TABLE [dbo].[AdditionalSubjects] (
    [ID] int  NOT NULL,
    [Key] int  NOT NULL,
    [Seq] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'BranchTables'
CREATE TABLE [dbo].[BranchTables] (
    [BID] int IDENTITY(1,1) NOT NULL,
    [Branch] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'CommentTables'
CREATE TABLE [dbo].[CommentTables] (
    [AID] int  NOT NULL,
    [Comments] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [DateTime] datetime  NOT NULL,
    [Serial] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'LibraryTables'
CREATE TABLE [dbo].[LibraryTables] (
    [ID] int  NOT NULL,
    [Pkey] int  NOT NULL,
    [seq] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'PDFDetails'
CREATE TABLE [dbo].[PDFDetails] (
    [PKey] int IDENTITY(1,1) NOT NULL,
    [Review] nvarchar(50)  NOT NULL,
    [Rating] int  NOT NULL,
    [Downloads] int  NOT NULL,
    [User] int  NOT NULL,
    [Primarykey] int  NOT NULL
);
GO

-- Creating table 'PDFTables'
CREATE TABLE [dbo].[PDFTables] (
    [PKey] int IDENTITY(1,1) NOT NULL,
    [NameOfPDF] nvarchar(50)  NOT NULL,
    [Storage] nvarchar(50)  NOT NULL,
    [Type] nvarchar(50)  NOT NULL,
    [Key] int  NULL
);
GO

-- Creating table 'SubjectTables'
CREATE TABLE [dbo].[SubjectTables] (
    [Key] int IDENTITY(1,1) NOT NULL,
    [Year] int  NOT NULL,
    [BID] int  NOT NULL,
    [Subject] nvarchar(50)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Details'
ALTER TABLE [dbo].[Details]
ADD CONSTRAINT [PK_Details]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [AID] in table 'DashboardAnswers'
ALTER TABLE [dbo].[DashboardAnswers]
ADD CONSTRAINT [PK_DashboardAnswers]
    PRIMARY KEY CLUSTERED ([AID] ASC);
GO

-- Creating primary key on [QID] in table 'DashboardQuestions'
ALTER TABLE [dbo].[DashboardQuestions]
ADD CONSTRAINT [PK_DashboardQuestions]
    PRIMARY KEY CLUSTERED ([QID] ASC);
GO

-- Creating primary key on [Seq] in table 'AdditionalSubjects'
ALTER TABLE [dbo].[AdditionalSubjects]
ADD CONSTRAINT [PK_AdditionalSubjects]
    PRIMARY KEY CLUSTERED ([Seq] ASC);
GO

-- Creating primary key on [BID] in table 'BranchTables'
ALTER TABLE [dbo].[BranchTables]
ADD CONSTRAINT [PK_BranchTables]
    PRIMARY KEY CLUSTERED ([BID] ASC);
GO

-- Creating primary key on [Serial] in table 'CommentTables'
ALTER TABLE [dbo].[CommentTables]
ADD CONSTRAINT [PK_CommentTables]
    PRIMARY KEY CLUSTERED ([Serial] ASC);
GO

-- Creating primary key on [seq] in table 'LibraryTables'
ALTER TABLE [dbo].[LibraryTables]
ADD CONSTRAINT [PK_LibraryTables]
    PRIMARY KEY CLUSTERED ([seq] ASC);
GO

-- Creating primary key on [Primarykey] in table 'PDFDetails'
ALTER TABLE [dbo].[PDFDetails]
ADD CONSTRAINT [PK_PDFDetails]
    PRIMARY KEY CLUSTERED ([Primarykey] ASC);
GO

-- Creating primary key on [PKey] in table 'PDFTables'
ALTER TABLE [dbo].[PDFTables]
ADD CONSTRAINT [PK_PDFTables]
    PRIMARY KEY CLUSTERED ([PKey] ASC);
GO

-- Creating primary key on [Key] in table 'SubjectTables'
ALTER TABLE [dbo].[SubjectTables]
ADD CONSTRAINT [PK_SubjectTables]
    PRIMARY KEY CLUSTERED ([Key] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [QID] in table 'DashboardAnswers'
ALTER TABLE [dbo].[DashboardAnswers]
ADD CONSTRAINT [FK_DashboardAnswer_DashboardAnswer]
    FOREIGN KEY ([QID])
    REFERENCES [dbo].[DashboardQuestions]
        ([QID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DashboardAnswer_DashboardAnswer'
CREATE INDEX [IX_FK_DashboardAnswer_DashboardAnswer]
ON [dbo].[DashboardAnswers]
    ([QID]);
GO

-- Creating foreign key on [ID] in table 'AdditionalSubjects'
ALTER TABLE [dbo].[AdditionalSubjects]
ADD CONSTRAINT [FK_AdditionalSubjects_Details]
    FOREIGN KEY ([ID])
    REFERENCES [dbo].[Details]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AdditionalSubjects_Details'
CREATE INDEX [IX_FK_AdditionalSubjects_Details]
ON [dbo].[AdditionalSubjects]
    ([ID]);
GO

-- Creating foreign key on [Key] in table 'AdditionalSubjects'
ALTER TABLE [dbo].[AdditionalSubjects]
ADD CONSTRAINT [FK_AdditionalSubjects_SubjectTable]
    FOREIGN KEY ([Key])
    REFERENCES [dbo].[SubjectTables]
        ([Key])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AdditionalSubjects_SubjectTable'
CREATE INDEX [IX_FK_AdditionalSubjects_SubjectTable]
ON [dbo].[AdditionalSubjects]
    ([Key]);
GO

-- Creating foreign key on [AID] in table 'CommentTables'
ALTER TABLE [dbo].[CommentTables]
ADD CONSTRAINT [FK_CommentTable_DashboardAnswer]
    FOREIGN KEY ([AID])
    REFERENCES [dbo].[DashboardAnswers]
        ([AID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CommentTable_DashboardAnswer'
CREATE INDEX [IX_FK_CommentTable_DashboardAnswer]
ON [dbo].[CommentTables]
    ([AID]);
GO

-- Creating foreign key on [ID] in table 'LibraryTables'
ALTER TABLE [dbo].[LibraryTables]
ADD CONSTRAINT [FK_LibraryTable_Details]
    FOREIGN KEY ([ID])
    REFERENCES [dbo].[Details]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LibraryTable_Details'
CREATE INDEX [IX_FK_LibraryTable_Details]
ON [dbo].[LibraryTables]
    ([ID]);
GO

-- Creating foreign key on [Pkey] in table 'LibraryTables'
ALTER TABLE [dbo].[LibraryTables]
ADD CONSTRAINT [FK_LibraryTable_PDFTable]
    FOREIGN KEY ([Pkey])
    REFERENCES [dbo].[PDFTables]
        ([PKey])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LibraryTable_PDFTable'
CREATE INDEX [IX_FK_LibraryTable_PDFTable]
ON [dbo].[LibraryTables]
    ([Pkey]);
GO

-- Creating foreign key on [PKey] in table 'PDFDetails'
ALTER TABLE [dbo].[PDFDetails]
ADD CONSTRAINT [FK_PDFDetails_PDFTable]
    FOREIGN KEY ([PKey])
    REFERENCES [dbo].[PDFTables]
        ([PKey])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PDFDetails_PDFTable'
CREATE INDEX [IX_FK_PDFDetails_PDFTable]
ON [dbo].[PDFDetails]
    ([PKey]);
GO

-- Creating foreign key on [AID] in table 'DashboardAnswers'
ALTER TABLE [dbo].[DashboardAnswers]
ADD CONSTRAINT [FK_DashboardAnswer_DashboardAnswer1]
    FOREIGN KEY ([AID])
    REFERENCES [dbo].[DashboardAnswers]
        ([AID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Key] in table 'PDFTables'
ALTER TABLE [dbo].[PDFTables]
ADD CONSTRAINT [FK_PDFTable_PDFTable]
    FOREIGN KEY ([Key])
    REFERENCES [dbo].[SubjectTables]
        ([Key])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PDFTable_PDFTable'
CREATE INDEX [IX_FK_PDFTable_PDFTable]
ON [dbo].[PDFTables]
    ([Key]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------