CREATE TABLE [dbo].[TempNormalVacationsBalances] (
    [ID]                            INT             IDENTITY (1, 1) NOT NULL,
    [EmployeeCodeID]                INT             NOT NULL,
    [EmployeeCodeNo]                NVARCHAR (50)   NOT NULL,
    [EmployeeName]                  NVARCHAR (250)  NOT NULL,
    [RemainingOldBalance]           DECIMAL (18, 4) NOT NULL,
    [DeservedOldBalance]            DECIMAL (18, 4) NOT NULL,
    [ConsumedOldBalance]            DECIMAL (18, 4) NOT NULL,
    [DeservedCurrentBalance]        DECIMAL (18, 4) NOT NULL,
    [ConsumedCurrentBalance]        DECIMAL (18, 4) NOT NULL,
    [ExpiredCurrentBalance]         DECIMAL (18, 4) NOT NULL,
    [RemainingCurrentBalance]       DECIMAL (18, 4) NOT NULL,
    [InAdvanceBalance]              DECIMAL (18, 4) NOT NULL,
    [InAdvConsumed]                 DECIMAL (18, 4) NOT NULL,
    [TotalAvailableVacationBalance] DECIMAL (18, 4) NOT NULL,
    [TotalDeservedBalance]          DECIMAL (18, 4) NOT NULL,
    [TotalConsumedBalance]          DECIMAL (18, 4) NOT NULL,
    [TotalRemainingBalance]         DECIMAL (18, 4) NOT NULL,
    [CreatedDate]                   DATETIME        CONSTRAINT [DF_TempNormalVacationsBalances_CreatedDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_TempNormalVacationsBalances] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_TempNormalVacationsBalances]
    ON [dbo].[TempNormalVacationsBalances]([EmployeeCodeID] ASC);

