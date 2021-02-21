CREATE TABLE [dbo].[HolidaysAttendance] (
    [HolidayAttendanceID] INT      IDENTITY (1, 1) NOT NULL,
    [HolidaySettingID]    INT      NOT NULL,
    [CreatedBy]           INT      NOT NULL,
    [CreatedDate]         DATETIME NOT NULL,
    [LastUpdatedBy]       INT      NULL,
    [LastUpdatedDate]     DATETIME NULL,
    [OrganizationID]      INT      NULL,
    CONSTRAINT [PK_HolidaysAttendance] PRIMARY KEY CLUSTERED ([HolidayAttendanceID] ASC),
    CONSTRAINT [FK_HolidaysAttendance_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_HolidaysAttendance_EmployeesCodes1] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_HolidaysAttendance_HolidaysSettings] FOREIGN KEY ([HolidaySettingID]) REFERENCES [dbo].[HolidaysSettings] ([HolidaySettingID]),
    CONSTRAINT [FK_HolidaysAttendance_OrganizationsStructures] FOREIGN KEY ([OrganizationID]) REFERENCES [dbo].[OrganizationsStructures] ([OrganizationID])
);

