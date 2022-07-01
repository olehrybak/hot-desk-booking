# Hot desk booking system
API is done using ASP.NET 6 with Entity Framework. For the database I used MS SQL Server 2019. Authentication was done using JWT tokens. Also, for Unit tests Moq library was used.

# Project Testing
To start the project we need to setup MS SQL Server and enter proper connection string in the appsettings.json. Then we can automatically setup the database using entity framework by simply executing the following command:
```
dotnet ef database update
```
After that, we can test the API using swagger:
https://localhost:PORT/swagger/index.html (PORT is 7011 by default)

# REST API Description

## User Endpoints

<img width="1422" alt="Screenshot 2022-07-01 at 13 54 19" src="https://user-images.githubusercontent.com/100802720/176889622-2c6077be-beba-4c4c-b0be-3ac19950832c.png">

Both user endpoints can be accessed by anonymous user. After registration with your own credentials, you can login. If login was successful, it returns code 200 and JWT token. Swagger allows us to authorize by pressing "Authorize" button on top and pass there JWT token obtained from login. After that, we can access all other features of the API, which require authorization.

NOTE: There are 2 roles in the API: "User" and "Admin". By default every user is "User" after registration. It can be changed only in database.

## Location Endpoints

<img width="1422" alt="Screenshot 2022-07-01 at 14 00 10" src="https://user-images.githubusercontent.com/100802720/176890672-5109366d-49a8-4289-a1dd-a833cd56ed78.png">

The endpoints here are pretty obvious, we can get all locations with GET, add a new one with POST and DELETE location by id. Only GET is allowed for normal users, while other two require "Admin" role.

## Desk Endpoints

<img width="1422" alt="Screenshot 2022-07-01 at 14 05 25" src="https://user-images.githubusercontent.com/100802720/176891136-f60cf81a-df78-459c-9043-b00694bb76f1.png">

The first GET returns all Desks, POST adds a new one. PUT request is used to toggle availability of the desk (i.e. IsAvailable = !IsAvailable). GET: /api/Desk/{id} returns a list of reservations for the specific table without showing IDs of the users, who made the reservations. GET: /api/Desk/filter?location={id} returns a list of desks in the specific location. POST, DELETE and PUT endpoints require "Admin" role.

## Reservation Endpoints

<img width="1422" alt="Screenshot 2022-07-01 at 14 11 58" src="https://user-images.githubusercontent.com/100802720/176891986-21581a82-54c1-4458-8ab8-f074e802617a.png">

GET returns all reservations details including IDs of the users. POST allows user to create a new reservation. PUT allows user to change the reservation desk and time (if they are available). GET request requires "Admin" role.

NOTE: StartDay in PUT and POST requests should be in format "mm/dd/yyyy". StartDay and EndDay are returned in the format "mm/dd/yyyy hh:mm:ss". 
      While creating a reservation, user sets only the day, while time automatically sets to 08:00, which is considered a start of a working day.
