CREATE PROCEDURE [Tasks].[GetTasksByOwnerId]
	@ownerId varchar(100)	
AS
	SELECT * FROM [Tasks].Tasks 
	WHERE OwnerId = @ownerId

