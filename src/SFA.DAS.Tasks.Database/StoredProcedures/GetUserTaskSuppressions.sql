CREATE PROCEDURE [Tasks].[GetUserTaskSuppressions]
	@userId varchar(50),
	@employerAccountId varchar(50)
AS
	SELECT ReminderTaskType FROM [Tasks].[UserReminderSuppressions]
	WHERE UserId = @userId AND
	EmployerAccountId = @employerAccountId AND
	MONTH(CreatedDate) = MONTH(GETDATE()) -- Suppressions are only valid for the current month
