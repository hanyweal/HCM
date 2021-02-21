CREATE TABLE [dbo].[TransfersTypes] (
    [TransferTypeID]   INT           IDENTITY (1, 1) NOT NULL,
    [TransferTypeName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_TransfersTypes] PRIMARY KEY CLUSTERED ([TransferTypeID] ASC)
);

