
# DynamicObjectAPI

DynamicObjectAPI is an API that allows you to dynamically manage `Customer`, `Invoice`, and `InvoiceLine` entities. The project supports the following operations:

- **Create**: Add new customers along with their associated invoices and invoice lines.
- **GetAll**: Retrieve all customers and their associated data.
- **Delete**: Remove a customer along with any related data.

## Available Endpoints

1. **Create**: `POST /api/dynamic/customers/create`
   - Creates a new customer, along with any provided invoices and invoice lines.
2. **GetAll**: `GET /api/dynamic/customers`
   - Retrieves all customer records, including their invoices and invoice lines.
3. **Delete**: `DELETE /api/dynamic/customers/{customerId}`
   - Deletes a specific customer and all related invoices and invoice lines.

## How to Use

### Create a Customer

To create a new customer along with associated invoices and invoice lines, make a `POST` request to `/api/dynamic/customers/create` with the following JSON format:

```json
{
  "Name": "John Doe",
  "Email": "john.doe@example.com",
  "Invoices": [
    {
      "InvoiceNumber": "INV-001",
      "Amount": 200.50,
      "InvoiceLines": [
        {
          "ItemName": "Product A",
          "Price": 100.00
        },
        {
          "ItemName": "Product B",
          "Price": 100.50
        }
      ]
    }
  ]
}
```

### Explanation:
- **Customer Details**: The "Name" and "Email" fields represent the customer's information.
- **Invoices**: The "Invoices" array can contain one or more invoices for the customer. Each invoice includes:
  - **InvoiceNumber**: A unique identifier for the invoice.
  - **Amount**: The total amount for the invoice.
  - **InvoiceLines**: A list of items or services within the invoice. Each line contains:
    - **ItemName**: The name of the product or service.
    - **Price**: The price for that item.

### Retrieve All Customers
To retrieve all customers along with their related invoices and invoice lines, make a `GET` request to `/api/dynamic/customers`.

### Delete a Customer
To delete a specific customer, including all associated invoices and invoice lines, make a `DELETE` request to `/api/dynamic/customers/{customerId}` where `{customerId}` is the ID of the customer you want to delete.

## Requirements
- **.NET Core SDK**
- A database connection configured in the `appsettings.json` file

## Getting Started
1. **Clone the Repository**: Clone this repository to your local machine.
2. **Configure the Database**: Update the `appsettings.json` file with your database connection details.
3. **Run Migrations**: Use `dotnet ef migrations add InitialCreate` and `dotnet ef database update` to set up the database schema.
4. **Run the Application**: Use `dotnet run` to start the API.

## Example Requests
- **Create**:
  - `POST /api/dynamic/create` with the example JSON above to add a customer with related data.
- **GetAll**:
  - `GET /api/dynamic/customers` to retrieve all customers.
- **Delete**:
  - `DELETE /api/dynamic/customers/1` to delete the customer with ID 1 and all related data.

## Notes
- This API allows nested data structures, making it flexible for managing complex customer relationships.
- Error handling ensures that if an issue occurs during any operation, appropriate messages are returned.

## License
This project is licensed under the MIT License - see the LICENSE file for details.
