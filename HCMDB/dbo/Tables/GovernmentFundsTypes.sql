CREATE TABLE [dbo].[GovernmentFundsTypes] (
    [GovernmentFundTypeID]   INT           IDENTITY (1, 1) NOT NULL,
    [GovernmentFundTypeName] NVARCHAR (50) NOT NULL,
    [CreatedDate]            DATETIME      CONSTRAINT [DF_GovernmentFundsTypes_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate]        DATETIME      NULL,
    [CreatedBy]              INT           NULL,
    [LastUpdatedBy]          INT           NULL,
    CONSTRAINT [PK_GovernmentFundsTypes] PRIMARY KEY CLUSTERED ([GovernmentFundTypeID] ASC)
);

