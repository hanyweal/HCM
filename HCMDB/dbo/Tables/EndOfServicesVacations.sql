CREATE TABLE [dbo].[EndOfServicesVacations] (
    [EndOfServiceVacationID] INT      IDENTITY (1, 1) NOT NULL,
    [EndOfServiceID]         INT      NOT NULL,
    [VacationTypeID]         INT      NOT NULL,
    [VacationStartDate]      DATE     NOT NULL,
    [VacationEndDate]        DATE     NOT NULL,
    [CreatedBy]              INT      NULL,
    [CreatedDate]            DATETIME NOT NULL,
    [LastUpdatedBy]          INT      NULL,
    [LastUpdatedDate]        DATETIME NULL,
    CONSTRAINT [PK_EndOfServicesVacations] PRIMARY KEY CLUSTERED ([EndOfServiceVacationID] ASC),
    CONSTRAINT [FK_EndOfServicesVacations_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_EndOfServicesVacations_EmployeesCodes1] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_EndOfServicesVacations_EndOfServices] FOREIGN KEY ([EndOfServiceID]) REFERENCES [dbo].[EndOfServices] ([EndOfServiceID]),
    CONSTRAINT [FK_EndOfServicesVacations_VacationsTypes] FOREIGN KEY ([VacationTypeID]) REFERENCES [dbo].[VacationsTypes] ([VacationTypeID])
);

