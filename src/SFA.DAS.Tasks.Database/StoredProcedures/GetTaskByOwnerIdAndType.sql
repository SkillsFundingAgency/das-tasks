CREATE PROCEDURE [Tasks].[GetTaskByOwnerIdAndType]
	@ownerId varchar(50),
	@type varchar(100)
AS
	SELECT TOP(1) * FROM [Tasks].Tasks 
	WHERE OwnerId = @ownerId
	AND Type = @type

