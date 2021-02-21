CREATE TABLE [dbo].[VacationsActions] (
    [VacationActionID]   INT           NOT NULL,
    [VacationActionName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_VacationsActions] PRIMARY KEY CLUSTERED ([VacationActionID] ASC)
);

