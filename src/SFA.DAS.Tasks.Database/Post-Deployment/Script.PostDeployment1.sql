/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

------------------------------------------------
-- Populate monthly Employer Acccounts Reminder Tasks table --
------------------------------------------------

MERGE [Tasks].[MonthlyReminderTasks] AS target
USING (SELECT NEWID(), 'LevyDeclarationDue', 16, 19, 1) 
	AS source (Id, Type, StartDay, EndDay, ApplicableToApprenticeshipEmployerType)
ON (target.Type = source.Type)
WHEN MATCHED THEN
	UPDATE SET 
		StartDay = source.StartDay,
		EndDay = source.EndDay,
		ApplicableToApprenticeshipEmployerType = source.ApplicableToApprenticeshipEmployerType
WHEN NOT MATCHED THEN 
	INSERT (Id, Type, StartDay, EndDay, ApplicableToApprenticeshipEmployerType)
	VALUES (source.Id, source.Type, source.StartDay, source.EndDay, source.ApplicableToApprenticeshipEmployerType);


