Drop table Audio
Drop table Audio_PlayList
Drop table PlayList

CREATE TABLE [dbo].[Audio] (
    [Id]     INT PRIMARY KEY IDENTITY,
    [Source] NVARCHAR (200) NOT NULL,
    [Title]  NVARCHAR (100) NOT NULL,
    [Author] NVARCHAR (100) NOT NULL,
    [Genre]  NVARCHAR (50)  NOT NULL,
    [Year]   NVARCHAR (20)  NOT NULL,
    [Mark]   INT           NOT NULL
);

CREATE TABLE [dbo].[Audio_PlayList] (
    [Audio]    INT NOT NULL,
    [PlayList] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Audio] ASC, [PlayList] ASC),
    CONSTRAINT [FK_Audio] FOREIGN KEY ([Audio]) REFERENCES [dbo].[Audio] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PlayList] FOREIGN KEY ([PlayList]) REFERENCES [dbo].[PlayList] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[PlayList] (
    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);