CREATE PROCEDURE [Tasks].[GetTasksByEmployerAccountId]
	@employerAccountId varchar(50)	
AS
	SELECT * FROM [Tasks].Tasks 
	WHERE EmployerAccountId = @employerAccountId

