# UserTasksAPI

## Building a User’s and Tasks API

**API Development**
I created a new ASP.NET Core Web API project using .NET 8. I started with .NET 7 but had to make the change to 8 due to Jwt and .NET imcompatibilities between the versions. 

*User Model*:
- User: ID, Username, Email, Password

*Task Model*: 
- ID, Title, Description, Assignee (UserID), DueDate

**Authentication**
- Implemented JWT (JSON Web Tokens) authentication.
- Generate and validate JWT tokens for users.
![Screenshot 2024-01-26 112957](https://github.com/G0rd0n1/UserTasksAPI/assets/107427229/e4c7bb98-4ca6-4c78-b337-421babe84cb7)
![Screenshot 2024-01-26 112927](https://github.com/G0rd0n1/UserTasksAPI/assets/107427229/09c40fae-db51-4974-9c39-54beefdb68ef)
![Screenshot 2024-01-26 113136](https://github.com/G0rd0n1/UserTasksAPI/assets/107427229/55823df1-4e41-4b8d-b40c-71812b4a1651)

  
**Database Interaction**
![image](https://github.com/G0rd0n1/UserTasksAPI/assets/107427229/4b2a218c-4a6c-49fa-9e6a-10e4d1b5f09e)

- I used Dapper for database interaction
- Set up a local database (SQL Server on SSMS) for storing users and tasks information.
- Implemented repository patterns to interact with the database for CRUD operations.

**Filtering**
Filter Tasks by ID or BY UserName

**Testing**
- Wrote unit tests for the API controllers and repository methods making use of NUnit and Moq for writing the tests.
![image](https://github.com/G0rd0n1/UserTasksAPI/assets/107427229/31725ff1-29a7-4dbf-aa4b-2373d89b9424)

  
**Swagger Documentation**
