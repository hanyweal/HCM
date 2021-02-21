CREATE TABLE [dbo].[NormalVacationsTypes] (
    [NormalVacationTypeID]   INT           IDENTITY (1, 1) NOT NULL,
    [NormalVacationTypeName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_NormalVacationsTypes] PRIMARY KEY CLUSTERED ([NormalVacationTypeID] ASC)
);

