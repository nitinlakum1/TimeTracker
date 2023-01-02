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


--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-01 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-02 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-05 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-06 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-07 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-08 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-09 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-12 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-13 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-14 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-15 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-16 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-19 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-20 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-21 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-22 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-23 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-26 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-27 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-28 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-29 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2022-12-30 09:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 1, 'Service Start', '2023-01-02 09:00:00.000', GETDATE(), 'WCT-GTPL'

--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-01 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-02 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-05 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-06 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-07 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-08 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-09 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-12 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-13 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-14 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-15 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-16 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-19 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-20 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-21 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-22 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-23 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-26 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-27 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-28 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-29 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2022-12-30 17:00:00.000', GETDATE(), 'WCT-GTPL'
--INSERT INTO SystemLogs SELECT 3, 3, 'System Log Off', '2023-01-02 17:00:00.000', GETDATE(), 'WCT-GTPL'