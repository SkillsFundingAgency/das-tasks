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

if(not exists(select top 1 * from [Tasks].[MonthlyReminderTasks] WHERE [Type] = 'LevyDeclarationDue'))
begin
	INSERT INTO [Tasks].[MonthlyReminderTasks] (Id, Type, StartDay, EndDay) 
	VALUES (NEWID(), 'LevyDeclarationDue', 16, 19)
end