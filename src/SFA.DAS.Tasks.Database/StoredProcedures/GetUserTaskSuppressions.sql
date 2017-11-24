CREATE PROCEDURE [Tasks].[GetUserTaskSuppressions]
	@userId varchar(50),
	@accountId varchar(50)
AS
	SELECT ReminderTaskType FROM [Tasks].[UserReminderSuppressions]
	WHERE UserId = @userId AND
	AccountId = @accountId AND
	MONTH(CreatedDate) = MONTH(GETDATE()) -- Suppressions are only valid for the current month
