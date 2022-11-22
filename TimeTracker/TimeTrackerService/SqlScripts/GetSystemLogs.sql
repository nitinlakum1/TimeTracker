-- =============================================
-- Author      : Nitin Lakum
-- Create Date : 22/11/2022
-- Description : Get System Logs.
-- =============================================
CREATE PROCEDURE [dbo].[GetSystemLogs]
	@MacAddress NVARCHAR(50)
AS
BEGIN
BEGIN TRY

	SELECT TOP 1
		S.Id
		,S.LogTime
	FROM
		dbo.SystemLogs S
	JOIN dbo.Users U
		ON U.Id = S.UserId
	WHERE
		U.MacAddress = @MacAddress
	ORDER BY S.Id DESC

	RETURN 0
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
END