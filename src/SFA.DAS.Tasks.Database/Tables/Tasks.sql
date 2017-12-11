CREATE TABLE [Tasks].[Tasks]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Type] VARCHAR(100) NOT NULL, 
    [EmployerAccountId] VARCHAR(50) NOT NULL, 
    [ItemsDueCount] INT NOT NULL
)
GO

CREATE INDEX [IX_Tasks_EmployerAccountId] ON [Tasks].[Tasks] ([EmployerAccountId])
