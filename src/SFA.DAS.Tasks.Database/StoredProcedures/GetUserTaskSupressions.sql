CREATE PROCEDURE [Tasks].[GetUserTaskSupressions]
	@userId varchar(50),
	@accountId varchar(50)
AS
	SELECT ReminderTaskType FROM [Tasks].[UserReminderSupressions]
	WHERE UserId = @userId AND
	AccountId = @accountId AND
	MONTH(CreatedDate) = MONTH(GETDATE()) -- supressions are only valid for the current month
