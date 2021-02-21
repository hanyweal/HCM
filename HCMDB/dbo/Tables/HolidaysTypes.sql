CREATE TABLE [dbo].[HolidaysTypes] (
    [HolidayTypeID]   INT           IDENTITY (1, 1) NOT NULL,
    [HolidayTypeName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_AnnualVacationsTypes] PRIMARY KEY CLUSTERED ([HolidayTypeID] ASC)
);

