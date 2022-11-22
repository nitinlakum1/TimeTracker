﻿-- =============================================
-- Author      : Nitin Lakum
-- Create Date : 18/11/2022
-- Description : Insert system logs.
-- =============================================
CREATE PROCEDURE [dbo].[AddSystemLog]
	@MacAddress NVARCHAR(50)
	,@LogType INT
	,@Description NVARCHAR(50)
	,@LogTime DATETIME
AS
BEGIN
BEGIN TRY

	DECLARE @UserId INT = (SELECT TOP 1 U.Id FROM dbo.Users U WHERE U.MacAddress = @MacAddress)

	INSERT INTO [dbo].SystemLogs
	(
		UserId
		,LogType
		,[Description]
		,LogTime
	)
	VALUES
	(
		@UserId
		,@LogType
		,@Description
		,@LogTime
	)

	RETURN 0
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
END