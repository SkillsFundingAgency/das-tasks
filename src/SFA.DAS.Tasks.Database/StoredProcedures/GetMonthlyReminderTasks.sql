CREATE PROCEDURE [Tasks].[GetMonthlyReminderTasks]	
AS
	SELECT * FROM [Tasks].[MonthlyReminderTasks] 
	WHERE StartDay <= DAY(GETDATE()) AND
	EndDay >= DAY(GETDATE())

