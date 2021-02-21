CREATE TABLE [dbo].[SenioritysPointsDeclarations] (
    [SeniorityPointDeclarationID] INT            IDENTITY (1, 1) NOT NULL,
    [Months]                      INT            NOT NULL,
    [Points]                      DECIMAL (5, 2) NOT NULL,
    CONSTRAINT [PK_SenioritysPointsDeclarations] PRIMARY KEY CLUSTERED ([SeniorityPointDeclarationID] ASC)
);

