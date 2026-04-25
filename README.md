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
- Retrieve transactions with filtered by type and category (GET)
- Retrieve a single transaction by ID (GET)
- Edit transactions by ID (PUT)
- Delete transactions by ID (DELETE)
- Data persists after API restart via MySQL database
- Data validation - required fields enforced, amount must be positive, type must be income or expense, not capital sensitive

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

## Transaction API Endpoints

### Get all or filtered Transactions 
- **Method:** GET
- **URL:** `/api/transactions`  
- **Response:** List of all transactions

- **URL:** `/api/transactions?category=entertainment`
- **Response:** List all entertainment transactions

- **URL:** `/api/transactions?type=expense`
- **Response:** List all expenses transactions


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

## User API Endpoints

User management endpoints with secure password hashing and DTO pattern to protect sensitive data.

### Get All Users
- **Method:** GET
- **URL:** `/api/user`
- **Response:** List of users (id, name, email — password never returned)

### Get User by ID
- **Method:** GET
- **URL:** `/api/user/{id}`
- **Response:** Single user or 404 if not found

### Register User
- **Method:** POST
- **URL:** `/api/user`
- **Body:**
```json
  {
    "name": "Javies",
    "email": "javies@email.com",
    "password": "password123"
  }
```
- **Response:** 201 Created with user data (no password)

### Edit User
- **Method:** PUT
- **URL:** `/api/user/{id}`
- **Body:** Any combination of name, email, password (all optional)
- **Response:** 204 No Content

### Delete User
- **Method:** DELETE
- **URL:** `/api/user/{id}`
- **Response:** 200 with deleted user or 404 if not found

## Python Reporting Script

A Python script that connects directly to the MySQL database and generates a spending summary grouped by category.

### Requirements
- Python 3
- mysql-connector-python

### Install dependency
```
pip3 install mysql-connector-python
```

### Run
```
python3 report.py
```

### Output
Displays total expenses, income, spendings per category and calculates net balance:
```
===== Expense Summary =====
Housing: $800.00
Entertainment: $35.00
Total Income: $1577.00
Total Expense: $835.00
Balance: $742.00
```

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
- Single user only — no multi-user support