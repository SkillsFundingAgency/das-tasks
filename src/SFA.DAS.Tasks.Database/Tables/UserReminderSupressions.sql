CREATE TABLE [Tasks].[UserReminderSupressions]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [UserId] BIGINT NULL, 
    [AccountId] BIGINT NULL, 
    [ReminderTaskType] VARCHAR(100) NULL, 
    [CreatedDate] DATETIME NULL
)
