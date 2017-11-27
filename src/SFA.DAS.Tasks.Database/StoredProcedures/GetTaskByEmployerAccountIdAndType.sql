CREATE PROCEDURE [Tasks].[GetTaskByEmployerAccountIdAndType]
	@employerAccountId varchar(50),
	@type varchar(100)
AS
	SELECT TOP(1) * FROM [Tasks].Tasks 
	WHERE EmployerAccountId = @employerAccountId
	AND Type = @type

