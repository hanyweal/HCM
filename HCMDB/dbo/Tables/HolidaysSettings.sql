CREATE TABLE [dbo].[HolidaysSettings] (
    [HolidaySettingID]        INT      IDENTITY (1, 1) NOT NULL,
    [HolidayTypeID]           INT      NOT NULL,
    [MaturityYearID]          INT      NOT NULL,
    [HolidaySettingStartDate] DATE     NOT NULL,
    [HolidaySettingEndDate]   DATE     NOT NULL,
    [CreatedBy]               INT      NOT NULL,
    [CreatedDate]             DATETIME NOT NULL,
    [LastUpdatedBy]           INT      NULL,
    [LastUpdatedDate]         DATETIME NULL,
    CONSTRAINT [PK_HolidaysSettings] PRIMARY KEY CLUSTERED ([HolidaySettingID] ASC),
    CONSTRAINT [FK_HolidaysSettings_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_HolidaysSettings_EmployeesCodes1] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_HolidaysSettings_HolidaysTypes] FOREIGN KEY ([HolidayTypeID]) REFERENCES [dbo].[HolidaysTypes] ([HolidayTypeID]),
    CONSTRAINT [FK_HolidaysSettings_MaturityYearsBalances] FOREIGN KEY ([MaturityYearID]) REFERENCES [dbo].[MaturityYearsBalances] ([MaturityYearID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NonClusteredIndex-HolidayTypeMaturityYear]
    ON [dbo].[HolidaysSettings]([HolidayTypeID] ASC, [MaturityYearID] ASC);

