# Stock Car API

The task is to build a web API in C# that allows dealers to manage their car stocks. The application should support the following functions:
Add/remove car
Get car
List cars and stock levels
Update car stock level
Search car by make and model


## Tech Stack

- .NET 10
- Dapper
- DotNetEnv
- FastEndpoints
- FastEndpoints.Swagger
- FastEndpoints.Security
- Microsoft.Data.Sqlite
- BCrypt.Net-Next

## Setup

### 1. Clone the repo

git clone
cd car-stock-api/CarStock

### 2. Create the .env file

Create a `.env` file in `car-stock-api/CarStock/` with:

    Jwt__SigningKey=Am3XVRA0ofC6sA5vfTBDWkZTI1I6S66iJyUEyveYzUg=
    ConnectionStrings__DefaultConnection=Data Source=carstock.db

### 3. Run

    cd car-stock-api/CarStock/
    dotnet run

Open <https://localhost:5000/swagger> to test with the Swagger UI.

## Dealer Accounts

| Name | Email | Password |
|---|---|---|
| Cars Melbourne | dealer@carsmelbourne.com.au | MelbournePassword123! |
| Cars Footscray | dealer@carsfootscray.com.au | FootscrayPassword123! |

## How to Test

1. Call POST /auth/login with a dealer account
2. Copy the token from the response (without the quotes)
3. Click Authorize in Swagger UI, paste token and click authorize
4. All car endpoints will now be authenticated as that dealer

## Endpoints

| Method | Route | Description |
|---|---|---|
| POST | /auth/login | Login and receive JWT |
| POST | /cars | Add a car |
| GET | /cars | List current dealer's cars |
| GET | /cars/{id} | Get one car |
| PUT | /cars/{id}/stock | Update stock |
| DELETE | /cars/{id} | Remove a car |
| GET | /cars/search | Search by make and/or model |

## Notes

- JWT tokens expire after 4 hours
- Passwords are hashed with BCrypt before storage
- Multiple dealers can consume this API. The dealers cannot access/modify another dealer’s cars or stock.
- The API receives and responds with JSON data
