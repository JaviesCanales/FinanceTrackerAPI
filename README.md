# Finance Tracker API

A REST API built with C# and .NET that allows users to track income and expenses. Transactions are stored in a MySQL database and can be created, retrieved, and deleted through HTTP endpoints.

## Tech Stack

- C# / .NET 9
- MySQL
- Entity Framework Core (Pomelo)
- Postman (for testing)

## Features

- Add transactions (POST)
- Retrieve all transactions (GET)
- Retrieve a single transaction by ID (GET)
- Edit transactions by ID (PUT)
- Delete transactions by ID (DELETE)
- Data persists after API restart via MySQL database

## How to Run

### Prerequisites:
- .NET 9 SDK
- MySQL
- An IDE
- API Tester

1. Clone the repository
```
   git clone https://github.com/yourusername/FinanceTrackerAPI.git
   cd FinanceTrackerAPI
```

2. Set up the database
```sql
   CREATE DATABASE FinanceTracker;
   USE FinanceTracker;
   CREATE TABLE Transaction(Id INT AUTO_INCREMENT PRIMARY KEY, Description VARCHAR(255), Amount DOUBLE, Category VARCHAR(255), Type VARCHAR(255), Date DATETIME);
```

3. Configure the connection string in `appsettings.json`
```json
   "ConnectionStrings": {
     "DefaultConnection": "server=localhost;database=FinanceTracker;user=root;password=yourpassword;"
   }
```

4. Run the API
```
   dotnet run
```

5. Test endpoints using Postman at `http://localhost:5118/api/transactions`

## API Endpoints

### Get All Transactions
- **Method:** GET
- **URL:** `/api/transactions`
- **Response:** List of all transactions

### Get Transaction by ID
- **Method:** GET
- **URL:** `/api/transactions/{id}`
- **Response:** Single transaction matching the ID

### Add Transaction
- **Method:** POST
- **URL:** `/api/transactions`
- **Body:**
```json
  {
    "description": "Groceries",
    "amount": 50.00,
    "category": "Food",
    "type": "expense"
  }
```
- **Response:** 201 Created with the new transaction

### Edit Transaction
- **Method:** PUT
- **URL:** `/api/transactions/{id}`
- **Body:**
```json
  {
    "description": "Rent",
    "amount": 800.00,
    "category": "Housing",
    "type": "expense"
  }
```
- **Response:** 200 with updated transaction or 404 if not found

### Delete Transaction
- **Method:** DELETE
- **URL:** `/api/transactions/{id}`
- **Response:** 200 with deleted transaction or 404 if not found



## Bug Fixes

### Duplicate ID Bug (In-Memory Phase)
**Problem:** When using in-memory storage, deleting a transaction and adding a new one caused duplicate IDs because the ID was based on list count.

**Example:**
- Add transaction → id 1
- Add transaction → id 2
- Delete id 1 → list count is now 1
- Add new transaction → id 2 again (duplicate)

**Fix:** Replaced list count with a static `nextId` counter that incremented permanently. Later resolved completely by migrating to MySQL with `AUTO_INCREMENT`.

## Known Limitations

- No user authentication — any user can access and modify all transactions
- No data validation — invalid or missing fields are not rejected
- No spending calculations or budget summaries (planned for Python reporting script)
- Single user only — no multi-user support