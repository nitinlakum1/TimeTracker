/*** Added a preloaded Roles. ***/
SET IDENTITY_INSERT dbo.Roles ON;
INSERT INTO dbo.Roles (Id, [Name], DisplayName, Deleted) VALUES (1, 'Admin', 'Admin', 0)
INSERT INTO dbo.Roles (Id, [Name], DisplayName, Deleted) VALUES (2, 'Employee', 'Employee', 0)
SET IDENTITY_INSERT dbo.Roles OFF;

/*** Added a preloaded Settings. ***/
INSERT INTO dbo.Settings ([Key], DisplayName, [Value], IsActive)
VALUES ('SYSTEM_WIFI_NAME', 'System Wifi Name', 'WCT-GTPL,WCT', 1)

INSERT INTO dbo.Settings ([Key], DisplayName, [Value], IsActive)
VALUES ('SYSTEM_LOG_START_TIMING', 'System Log Start Timing', '8', 1)

INSERT INTO dbo.Settings ([Key], DisplayName, [Value], IsActive)
VALUES ('SYSTEM_LOG_END_TIMING', 'System Log End Timing', '20', 1)