
# DynamicObjectAPI

## Overview

DynamicObjectAPI is a flexible API project that allows users to manage dynamic objects. The system provides CRUD (Create, Read, Update, Delete) operations on dynamic objects, which are defined at runtime. It leverages ASP.NET Core with Entity Framework for database operations, and follows SOLID principles for maintainable and scalable code.

This API can be used for scenarios where the structure of data is not predetermined and needs to be managed dynamically.

## Features

- Dynamic object modeling and management.
- CRUD operations (Create, Read, Update, Delete).
- Validation using FluentValidation.
- Clean architecture adhering to SOLID principles.
- Unit tests to ensure the integrity of the system.
- No hard-coded object structures – everything is defined dynamically.

## Requirements

To run this project, ensure that you have the following tools installed:

- .NET 8 SDK
- SQL Server or any compatible database management system

## Installation

### 1. Clone the Repository

To clone the project to your local machine:

```bash
git clone <repository-url>
cd DynamicObjectAPI
```

### 2. Install Dependencies

Restore the required NuGet packages by running:

```bash
dotnet restore
```

### 3. Database Setup

Open the `appsettings.json` file and configure your database connection string. For example, if you're using SQL Server, you may have the following configuration:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=DynamicObjectDB;Trusted_Connection=True;"
}
```

### 4. Apply Migrations

To set up the database schema, apply the existing migrations:

```bash
dotnet ef database update
```

### 5. Run the Application

To run the API, use the following command:

```bash
dotnet run
```

The API will be available at `https://localhost:5001`. You can now start interacting with it using any HTTP client (like Postman or cURL).

## Usage

DynamicObjectAPI allows users to define object types and their fields dynamically. You can create, update, retrieve, and delete dynamic objects using the provided API endpoints.

### Example API Request

#### Creating a Dynamic Object

To create a new dynamic object (for example, an order), you can send a POST request to `/api/dynamicobjects` with the following payload:

```http
POST /api/dynamicobjects
Content-Type: application/json
{
  "objectType": "Order",
  "fields": {
    "orderId": "123",
    "orderDate": "2024-09-26",
    "customerName": "Enver",
    "product": "Laptop"
  }
}
```

### API Endpoints

Here are the main endpoints for interacting with the dynamic objects:

- **POST** `/api/dynamicobjects` – Creates a new dynamic object.
  - **Request Body Example**:
    ```json
    {
      "objectType": "Order",
      "fields": {
        "orderId": "123",
        "orderDate": "2024-09-26",
        "customerName": "Enver",
        "product": "Laptop"
      }
    }
    ```
  - Response: Returns the created dynamic object.

- **GET** `/api/dynamicobjects/{id}` – Retrieves a dynamic object by its ID.
  - Response: Returns the dynamic object details.

- **PUT** `/api/dynamicobjects/{id}` – Updates an existing dynamic object.
  - **Request Body Example**:
    ```json
    {
      "fields": {
        "customerName": "Enver Updated",
        "product": "Smartphone"
      }
    }
    ```
  - Response: Returns the updated dynamic object.

- **DELETE** `/api/dynamicobjects/{id}` – Deletes a dynamic object by its ID.

### Error Handling

The API returns standard HTTP status codes to indicate success or failure:

- `200 OK` – The request was successful.
- `400 Bad Request` – There was a problem with the request (e.g., validation errors).
- `404 Not Found` – The requested object was not found.
- `500 Internal Server Error` – An unexpected error occurred on the server.

### Validation

The API uses FluentValidation to validate incoming requests. For example, when creating or updating an object, required fields will be validated, and if any field is missing or invalid, a `400 Bad Request` will be returned.

## Testing

The project includes unit tests to validate the correctness of the system. The tests are located in the `DynamicObjectAPI.Tests` project. To run the tests, use the following command:

```bash
dotnet test
```

Unit tests ensure that core functionalities like CRUD operations and object validation work as expected.

## Technologies Used

- **ASP.NET Core (.NET 8)** – Web API framework.
- **Entity Framework Core** – Object-relational mapping (ORM) for database interactions.
- **FluentValidation** – Library for validating incoming request data.
- **XUnit** – Unit testing framework.

## Future Enhancements

- **Authentication and Authorization**: Add token-based authentication for securing the API.
- **Pagination**: Implement pagination for retrieving large datasets.
- **Detailed Logging**: Integrate a logging framework like Serilog for better debugging and monitoring.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
