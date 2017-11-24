CREATE PROCEDURE [Tasks].[AddUserMonthlyReminderDismiss]
	
	@userId bigint NOT NULL,
	@accountId bigint NOT NULL,
	@reminderTaskType VARCHAR(100) NOT NULL
AS
	INSERT INTO MonthlyReminderUserDismisses (UserId, AccountId, ReminderTaskType, CreatedDate) 
	VALUES (@userId, @accountId, @reminderTaskType, GETDATE())
