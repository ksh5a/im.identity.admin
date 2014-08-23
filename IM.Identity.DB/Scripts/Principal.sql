IF NOT EXISTS 
    (SELECT name  
     FROM master.sys.server_principals
     WHERE name = 'imidentity')
BEGIN
    CREATE LOGIN metrics WITH PASSWORD = 'imidentitypassword'
END

GO
-- Creates a database user for the login created above.
CREATE USER imidentity FOR LOGIN imidentity;
exec sp_addrolemember 'db_owner', 'imidentity'
GO