CREATE TABLE [dbo].[Users] (
    [Id]            INT          NOT NULL,
    [userID]        VARCHAR (32) NOT NULL,
    [userPass]      VARCHAR (64) NOT NULL,
    [userFirstName] VARCHAR (32) NOT NULL,
    [userLastName]  VARCHAR (32) NOT NULL,
    [userDOB]       DATE         NOT NULL,
    [userPosition]  VARCHAR (15) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

