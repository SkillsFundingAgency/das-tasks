CREATE TABLE [Tasks].[UserReminderSupressions]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [UserId] VARCHAR(50) NULL, 
    [AccountId] VARCHAR(50) NULL, 
    [ReminderTaskType] VARCHAR(100) NULL, 
    [CreatedDate] DATETIME NULL
)
