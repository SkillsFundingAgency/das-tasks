CREATE PROCEDURE [Tasks].[AddUserReminderSupression]
	
	@userId bigint NOT NULL,
	@accountId bigint NOT NULL,
	@reminderTaskType VARCHAR(100) NOT NULL
AS
	INSERT INTO UserReminderSupressions (UserId, AccountId, ReminderTaskType, CreatedDate) 
	VALUES (@userId, @accountId, @reminderTaskType, GETDATE())
