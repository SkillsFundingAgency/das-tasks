CREATE PROCEDURE [Tasks].[UpsertTask]
	@id uniqueidentifier,
	@type varchar(100),
	@employerAccountId varchar(50),
	@itemsDueCount int
AS
	MERGE [Tasks].[Tasks] AS [Target]
	USING (SELECT @id AS Id) AS [Source] 
	ON [Target].Id = [Source].Id
	WHEN MATCHED THEN  UPDATE SET [Target].ItemsDueCount = @itemsDueCount
	WHEN NOT MATCHED THEN INSERT (Id, Type, EmployerAccountId, ItemsDueCount) VALUES (NEWID(),@type, @employerAccountId, @itemsDueCount);
