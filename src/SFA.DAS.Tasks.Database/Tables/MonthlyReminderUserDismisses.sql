CREATE TABLE [Tasks].[MonthlyReminderUserDismisses]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [UserId] BIGINT NULL, 
    [AccountId] BIGINT NULL, 
    [ReminderTaskType] VARCHAR(100) NULL, 
    [CreatedDate] DATETIME NULL
)
