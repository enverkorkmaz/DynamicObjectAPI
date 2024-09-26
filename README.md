# Dynamic Object API

Dynamic Object API is a RESTful service that allows dynamic creation, retrieval, updating, and deletion (CRUD operations) of objects with flexible fields. The objects and their fields are defined by the user and can vary for each request, offering high flexibility in how data is stored and managed.

## Project Purpose

This project solves the need for flexible data structures where predefined schemas may not fit. By allowing dynamic object creation, it enables users to create objects with any set of fields on the fly, making it useful for applications requiring adaptable and dynamic data storage.

## Technologies Used

- .NET 8.0
- ASP.NET Core Web API
- Entity Framework Core (EF Core)
- FluentValidation
- AutoMapper
- SQL Server (for database)
- Swashbuckle (for Swagger API documentation)

## Features

- **Dynamic Object Creation:** You can define your own object types and fields.
- **CRUD Operations:** Full Create, Read, Update, and Delete (CRUD) operations are supported for dynamic objects.
- **Transaction Handling:** Ensures consistency with rollback on failures.
- **Validation:** Input validation using FluentValidation.
- **Swagger UI:** Interactive API documentation.

## Installation

### Prerequisites

- .NET 8 SDK
- SQL Server (or any compatible database)
- IDE (e.g., Visual Studio or VS Code)

### Steps

1. Clone the repository:
   ```bash
   git clone <repository-url>
Navigate to the project folder:

bash
Kodu kopyala
cd DynamicObejctAPI
Restore NuGet packages:

bash
Kodu kopyala
dotnet restore
Update the database connection string in appsettings.json:

json
Kodu kopyala
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=DynamicObjectDb;Trusted_Connection=True;TrustServerCertificate=true"
}
Apply migrations to set up the database:

bash
Kodu kopyala
dotnet ef database update
Run the application:

bash
Kodu kopyala
dotnet run
Swagger UI
Once the application is running, you can access the Swagger UI at:

bash
Kodu kopyala
https://localhost:7076/swagger
API Endpoints
GET /api/dynamicobjects - Retrieves all dynamic objects.
GET /api/dynamicobjects/{id} - Retrieves a specific dynamic object by ID.
POST /api/dynamicobjects - Creates a new dynamic object.
PUT /api/dynamicobjects/{id} - Updates an existing dynamic object by ID.
DELETE /api/dynamicobjects/{id} - Deletes an existing dynamic object by ID.
Example Request (Create Dynamic Object)
json
Kodu kopyala
{
  "objectType": "Order",
  "fields": {
    "orderId": "1234",
    "orderDate": "2024-09-26",
    "customerName": "John Doe",
    "product": {
      "productId": "5678",
      "productName": "Laptop",
      "quantity": 2
    }
  }
}
Running Tests
The project includes unit tests for core functionality. To run the tests, use the following command:

bash
Kodu kopyala
dotnet test
Contributing
Contributions are welcome! Please create an issue or submit a pull request for any improvements or bug fixes.

License
This project is licensed under the MIT License. See the LICENSE file for details.
