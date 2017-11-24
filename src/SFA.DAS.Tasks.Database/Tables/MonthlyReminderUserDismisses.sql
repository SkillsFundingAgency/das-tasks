CREATE TABLE [Tasks].[MonthlyReminderUserDismisses]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [UserId] BIGINT NULL, 
    [AccountId] BIGINT NULL, 
    [MessageType] VARCHAR(100) NULL, 
    [CreatedDate] DATETIME NULL
)
