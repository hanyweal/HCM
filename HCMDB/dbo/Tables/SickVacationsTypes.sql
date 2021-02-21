CREATE TABLE [dbo].[SickVacationsTypes] (
    [SickVacationTypeID]   INT            IDENTITY (1, 1) NOT NULL,
    [SickVacationTypeName] NVARCHAR (100) NULL,
    CONSTRAINT [PK_SickVacationsTypes] PRIMARY KEY CLUSTERED ([SickVacationTypeID] ASC)
);

