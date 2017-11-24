CREATE PROCEDURE [Tasks].[AddUserReminderSupression]
	
	@userId VARCHAR(50),
	@accountId VARCHAR(50),
	@reminderTaskType VARCHAR(100)
AS
	INSERT INTO UserReminderSupressions (Id, UserId, AccountId, ReminderTaskType, CreatedDate) 
	VALUES (NEWID(), @userId, @accountId, @reminderTaskType, GETDATE())
