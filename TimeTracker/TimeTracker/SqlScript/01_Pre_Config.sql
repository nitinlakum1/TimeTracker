/*** Added a preloaded Roles. ***/
SET IDENTITY_INSERT dbo.Roles ON;
INSERT INTO dbo.Roles (Id, [Name], DisplayName, Deleted) VALUES (1, 'Admin', 'Admin', 0)
INSERT INTO dbo.Roles (Id, [Name], DisplayName, Deleted) VALUES (2, 'Employee', 'Employee', 0)
SET IDENTITY_INSERT dbo.Roles OFF;

/*** Added a preloaded Settings. ***/
IF NOT EXISTS(SELECT 1 FROM dbo.Settings WHERE [Key]='SYSTEM_WIFI_NAME')
BEGIN
	INSERT INTO dbo.Settings ([Key], DisplayName, [Value], IsActive)
	VALUES ('SYSTEM_WIFI_NAME', 'System Wifi Name', 'WCT-GTPL,WCT', 1)
END

IF NOT EXISTS(SELECT 1 FROM dbo.Settings WHERE [Key]='SYSTEM_LOG_START_TIMING')
BEGIN
	INSERT INTO dbo.Settings ([Key], DisplayName, [Value], IsActive)
	VALUES ('SYSTEM_LOG_START_TIMING', 'System Log Start Timing', '8:0', 1)
END

IF NOT EXISTS(SELECT 1 FROM dbo.Settings WHERE [Key]='SYSTEM_LOG_END_TIMING')
BEGIN
	INSERT INTO dbo.Settings ([Key], DisplayName, [Value], IsActive)
	VALUES ('SYSTEM_LOG_END_TIMING', 'System Log End Timing', '20:0', 1)
END

IF NOT EXISTS(SELECT 1 FROM dbo.Settings WHERE [Key]='COMPANY_HOLIDAYS')
BEGIN
	INSERT INTO dbo.Settings ([Key], DisplayName, [Value], IsActive)
	VALUES ('COMPANY_HOLIDAYS', 'Company Holidays', '26-11-2022', 1)
END