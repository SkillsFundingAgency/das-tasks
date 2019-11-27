CREATE TABLE [Tasks].[MonthlyReminderTasks]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Type] VARCHAR(100) NOT NULL, 
	[StartDay] TINYINT NOT NULL,  -- Day of the month to start showing task
	[EndDay] TINYINT NOT NULL,     -- Day of the month to stop showing task
	[ApplicableToApprenticeshipEmployerType] SMALLINT NOT NULL DEFAULT (-1)
)
