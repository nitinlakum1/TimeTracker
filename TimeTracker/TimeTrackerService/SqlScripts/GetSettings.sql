-- =============================================
-- Author      : Nitin Lakum
-- Create Date : 22/11/2022
-- Description : Get all active settings.
-- =============================================
CREATE PROCEDURE [dbo].[GetSettings]
AS
BEGIN
BEGIN TRY

	SELECT
		S.Id
		,S.[Key]
		,S.DisplayName
		,S.[Value]
	FROM
		dbo.Settings S
	WHERE
		S.IsActive = 1

	RETURN 0
END TRY
BEGIN CATCH
	RETURN @@ERROR
END CATCH
END