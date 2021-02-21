CREATE TABLE [dbo].[VacationsTypes] (
    [VacationTypeID]   INT           NOT NULL,
    [VacationTypeName] NVARCHAR (50) NOT NULL,
    [IsForFemaleOnly]  BIT           NULL,
    CONSTRAINT [PK_VacationsTypes] PRIMARY KEY CLUSTERED ([VacationTypeID] ASC)
);

