CREATE PROCEDURE [Tasks].[AddUserReminderSuppression]
	
	@userId VARCHAR(50),
	@employerAccountId VARCHAR(50),
	@reminderTaskType VARCHAR(100)
AS
	INSERT INTO UserReminderSuppressions (Id, UserId, EmployerAccountId, ReminderTaskType, CreatedDate) 
	VALUES (NEWID(), @userId, @employerAccountId, @reminderTaskType, GETDATE())
