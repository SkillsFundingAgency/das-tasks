CREATE PROCEDURE [Tasks].[GetTasksByOwnerId]
	@ownerId varchar(50)	
AS
	SELECT * FROM [Tasks].Tasks 
	WHERE OwnerId = @ownerId

