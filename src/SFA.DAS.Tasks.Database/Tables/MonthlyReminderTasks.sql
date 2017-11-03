﻿CREATE TABLE [Tasks].[MonthlyReminderTasks]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Type] VARCHAR(100) NOT NULL, 
	[StartDay] TINYINT NOT NULL,
	[EndDay] TINYINT NOT NULL
)
