/*** Added a preloaded Roles. ***/
SET IDENTITY_INSERT dbo.Roles ON;
INSERT INTO dbo.Roles (Id, [Name], DisplayName, Deleted) VALUES (1, 'Admin', 'Admin', 0)
INSERT INTO dbo.Roles (Id, [Name], DisplayName, Deleted) VALUES (2, 'Employee', 'Employee', 0)
INSERT INTO dbo.Roles (Id, [Name], DisplayName, Deleted) VALUES (3, 'HR', 'HR', 0)
SET IDENTITY_INSERT dbo.Roles OFF;


/*** Added a preloaded User. ***/
SET IDENTITY_INSERT dbo.[Users] ON;
INSERT INTO [dbo].[Users]([Id],[RoleId],[Username],[FullName],[Email],[Designation],[Education],[Experience]
           ,[ContactNo],[Gender],[Password],[Address],[DOB],[JoiningDate],[CreateAt],[BankName],[AccountNo]
           ,[IFSC],[MacAddress],[Url])
VALUES(1,1,'Admin','WhiteCore Technology','info@whitecoregroup.com','N/A','N/A','N/A','8905560307',1,'XVsc0JBTHsUQc5++Fe1auA=='
		,'428, WhiteCore Technology LLP, Avadh viceroy, Sarthana Jakat Naka, Nana Varachha, Surat, Gujarat-395006',null
		,GETDATE(),GETDATE(),'N/A','N/A','N/A','N/A','')
SET IDENTITY_INSERT dbo.[Users] OFF;


/*** Added a preloaded Settings. ***/
IF NOT EXISTS(SELECT 1 FROM dbo.Settings WHERE [Key]='SYSTEM_WIFI_NAME')
BEGIN
	INSERT INTO dbo.Settings ([Key], DisplayName, [Value], IsActive)
	VALUES ('SYSTEM_WIFI_NAME', 'System Wifi Name', 'WCT-GTPL,WCT', 1)
END

IF NOT EXISTS(SELECT 1 FROM dbo.Settings WHERE [Key]='SYSTEM_LOG_START_TIMING')
BEGIN
	INSERT INTO dbo.Settings ([Key], DisplayName, [Value], IsActive)
	VALUES ('SYSTEM_LOG_START_TIMING', 'System Log Start Timing', '08:00', 1)
END

IF NOT EXISTS(SELECT 1 FROM dbo.Settings WHERE [Key]='SYSTEM_LOG_END_TIMING')
BEGIN
	INSERT INTO dbo.Settings ([Key], DisplayName, [Value], IsActive)
	VALUES ('SYSTEM_LOG_END_TIMING', 'System Log End Timing', '20:00', 1)
END

IF NOT EXISTS(SELECT 1 FROM dbo.Settings WHERE [Key]='COMPANY_HOLIDAYS')
BEGIN
	INSERT INTO dbo.Settings ([Key], DisplayName, [Value], IsActive)
	VALUES ('COMPANY_HOLIDAYS', 'Company Holidays', '26-11-2022', 1)
END