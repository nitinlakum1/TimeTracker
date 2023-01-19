-- =============================================
-- Author      : Nitin Lakum
-- Create Date : 12/01/2023
-- Description : Get last added service
-- =============================================
CREATE PROCEDURE [dbo].[GetUpdateService]
	@Name NVARCHAR(50)
AS
BEGIN
BEGIN TRY

	SELECT TOP 1
		Id
		,[Version]
	FROM
		dbo.UpdateServices
	WHERE
		[Name] = @Name
	ORDER BY Id DESC

	RETURN 0
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
END