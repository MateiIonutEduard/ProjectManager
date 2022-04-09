# ProjectManager
This application is made to schedule some projects according to the AGILE methodology.<br>
Project is built with ASP.NET Core API, Angular and PostgreSQL.

## Project Setup
Before running the application, you need to download <br>PostgreSQL, install it from: https://www.postgresql.org/<br>
Then create the database from the included migration <br>scheme, as shown below.<br><br><br>
<b>Package Manager Console</b>
```powershell
PM> Update-Database
PM> Remove-Migration
```
<br/>
<b>If you use CLI, open up terminal</b>:

```shell

> dotnet ef database update
> dotnet ef migrations remove

```

## Scheduled Tasks
![status](https://img.shields.io/badge/unittest-passed-green.svg)&nbsp;&nbsp;Create database, entity mapping<br>
![status](https://img.shields.io/badge/unittest-passed-green.svg)&nbsp;&nbsp;Create Account, Profile API<br>
![status](https://img.shields.io/badge/unittest-running-blue.svg)&nbsp;&nbsp;Create Project API, Meeting<br>
![status](https://img.shields.io/badge/unittest-failure-red.svg)&nbsp;&nbsp;Add Member Invitation, Milestones<br>
![status](https://img.shields.io/badge/unittest-running-blue.svg)&nbsp;&nbsp;Create Phase Short Cycle to Milestone<br>
![status](https://img.shields.io/badge/unittest-passed-green.svg)&nbsp;&nbsp;Creates Task list in Phase Short Cycle<br>
![status](https://img.shields.io/badge/unittest-running-blue.svg)&nbsp;&nbsp;Add Issue list attached to Task list<br>
![status](https://img.shields.io/badge/unittest-passed-green.svg)&nbsp;&nbsp;Create Notification API<br>
