im.identity.admin
=================

Identity Administrator is an user management application based on Microsoft Identity 2.0 framework. It provides easy administration to speed up ASP.NET projects that needs a way to manage users and user roles. 

It can be used toghether with an already existing project (and its own database) in case you have to just run the script that will add the necessary tables, or can be the starting point of a new one.

## Features

- One time super administrator installation 
- Users and roles management
- Email confirmation and user password setup
- Two factor authentication

## Installation

- If you have full control on your SQL Server and you want to start from scratch.

Run [IM.Identity.DB.publish.sql] (https://github.com/RazvanPredescu/im.identity.admin/blob/master/IM.Identity.DB.publish.sql) script to create the default database. You have to adjust the lines that specify database location:

```sql
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER12\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER12\MSSQL\DATA\"
```

- If you do not have the rights to create databases from scripts, create database manually and run [IM.Identity.Tables.publish.sql] (https://github.com/RazvanPredescu/im.identity.admin/blob/master/IM.Identity.Tables.publish.sql)
script to create the required tables.

In both above scenarios, you have to create your own database user (principal) that will have access to the database. This user and his corresponding password
will be used in connection string in Web.config that application will use to access the database.

- Publish website to IIS.

- Configure website as it is suggested in the configuration section below. 
WARNING: You should setup your email server credentials before moving to the last step because no user account will be valid without email confirmation (not even the super administrator one). In case the email isn't received, installation procedure can be run again after the email (configuration) problem was solved.

- Access http://www.yourwebsitehere.com/install one time install page to create Super Administrator account. 

## Configuration

For a functional administration site, few things have to be configured.

### AppSettings.config

1. Provide a valid SMTP or Microsoft SendGrid credentials
2. Provide a valid SMS service credentials (optional)

```asp

  <!-- ============================================= -->
  <!-- Mail -->
  <!-- ============================================= -->

  <!-- SendGrid-->
  <add key="SendGridAccount" value="account" />
  <add key="SendGridPassword" value="password" />

  <!-- SMTP-->
  <add key="SmtpServer" value="smtp.gmail.com" />
  <add key="SmtpUserName" value="username" />
  <add key="SmtpPassword" value="password" />
  <add key="SmtpPort" value="587" />
  <add key="SmtpHtmlBody" value="True" />

  <!-- ============================================= -->
  <!-- Sms -->
  <!-- ============================================= -->

  <!-- Twilio-->
  <add key="TwilioSid" value="sid" />
  <add key="TwilioToken" value="token" />
  <add key="TwilioFromPhone" value="+206-555-1234" />
```

### Web.config

In Web.config several settings must be specified:

<b>Connection string:</b>
```asp
  <connectionStrings>
    <add name="DefaultConnection" connectionString="Data Source=.;Database=IM.Identity.Test.DB; User ID=youdatabaseuser; Password=yourdatabasepassword;Pooling=True" providerName="System.Data.SqlClient" />
  </connectionStrings>
 ```
 
<b>Application settings:</b>
```asp	
<appSettings file="AppSettings.config">
	<!-- Website where users will be redirected after they set-up their password (login page) -->
	<add key="UserWebsiteUrl" value="http://www.yourwebsitehere.com/" />
	
	<!-- Email that will appear to users as sender of confirmation emails -->
	<add key="MailAdmin" value="administrator@yourwebsitehere.com" />
	
	<!-- Current email service (Smtp by default). Supported services are: Smtp, SendGrid -->
	<add key="EmailService" value="Smtp" />
	
	<!-- Current Sms service. Supported sms services are: Twilio -->
	<add key="SmsService" value="Twilio" />
	
	<!--Number of access attempts allowed before a user is locked out (if lockout is enabled) -->
	<add key="LockoutMaxFailedAttempts" value="5" />
	
	<!-- Default amount of time (in minutes) that a user is locked out for after MaxFailedAccessAttemptsBeforeLockout is reached -->
	<add key="DefaultAccountLockoutTimeSpan" value="5" />
</appSettings>
 ```
 
## License
[MS-PL License](https://github.com/RazvanPredescu/im.identity.admin/blob/master/LICENSE.md)

## Reference

Some of the implementation source code was inspired from the articles originally written by Rick Anderson

- [Create a secure ASP.NET MVC 5 web app with log in, email confirmation and password reset](http://www.asp.net/mvc/overview/security/create-an-aspnet-mvc-5-web-app-with-email-confirmation-and-password-reset)
- [ASP.NET MVC 5 app with SMS and email Two-Factor Authentication](http://www.asp.net/mvc/overview/security/aspnet-mvc-5-app-with-sms-and-email-two-factor-authentication)
