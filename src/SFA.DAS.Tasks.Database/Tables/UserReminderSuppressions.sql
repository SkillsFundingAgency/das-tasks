CREATE TABLE [Tasks].[UserReminderSuppressions]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [UserId] VARCHAR(50) NOT NULL, 
    [EmployerAccountId] VARCHAR(50) NOT NULL, 
    [ReminderTaskType] VARCHAR(100) NOT NULL, 
    [CreatedDate] DATETIME NOT NULL
)
