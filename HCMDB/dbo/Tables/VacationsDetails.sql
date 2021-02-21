CREATE TABLE [dbo].[VacationsDetails] (
    [VacationDetailID] INT            IDENTITY (1, 1) NOT NULL,
    [VacationActionID] INT            NOT NULL,
    [FromDate]         DATE           NOT NULL,
    [ToDate]           DATE           NOT NULL,
    [MainframeNo]      NVARCHAR (50)  NULL,
    [IsApproved]       BIT            NOT NULL,
    [Notes]            NVARCHAR (MAX) NULL,
    [VacationID]       INT            NOT NULL,
    [ApprovedBy]       INT            NULL,
    [ApprovedDate]     DATETIME       NULL,
    [CreatedBy]        INT            NOT NULL,
    [CreatedDate]      DATETIME       NOT NULL,
    [LastUpdatedBy]    INT            NULL,
    [LastUpdatedDate]  DATETIME       NULL,
    CONSTRAINT [PK_VacationsDetails] PRIMARY KEY CLUSTERED ([VacationDetailID] ASC),
    CONSTRAINT [FK_VacationsDetails_EmployeesCodes] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]) ON DELETE CASCADE,
    CONSTRAINT [FK_VacationsDetails_EmployeesCodes1] FOREIGN KEY ([ApprovedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_VacationsDetails_EmployeesCodes2] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_VacationsDetails_Vacations] FOREIGN KEY ([VacationID]) REFERENCES [dbo].[Vacations] ([VacationID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_VacationsDetails_VacationsActions] FOREIGN KEY ([VacationActionID]) REFERENCES [dbo].[VacationsActions] ([VacationActionID]) ON DELETE CASCADE ON UPDATE CASCADE
);

