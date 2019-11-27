CREATE PROCEDURE [Tasks].[GetMonthlyReminderTasks]
	@applicableToApprenticeshipEmployerType smallint
AS
	SELECT * FROM [Tasks].[MonthlyReminderTasks] 
	WHERE StartDay <= DAY(GETDATE()) 
	AND EndDay >= DAY(GETDATE())
	AND @applicableToApprenticeshipEmployerType & ApplicableToApprenticeshipEmployerType <> 0 
