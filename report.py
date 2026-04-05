import mysql.connector

conn = mysql.connector.connect(
    host="localhost",
    user="root",
    password="",
    database="FinanceTracker"
)

cursor = conn.cursor()

cursor.execute("SELECT Date, Id, Amount, Category, Type FROM Transaction")

transactions = cursor.fetchall()

category_totals = {}
expense = 0
income = 0
for transaction in transactions:
    if transaction[4].lower() == "expense":
        if transaction[3] in category_totals:
            category_totals[transaction[3]] += transaction[2]
        else:
            category_totals[transaction[3]] = transaction[2]

        expense += transaction[2]

    if transaction[4].lower() == "income":
        income += transaction[2]

print("===== Expense Summary =====")
for i in category_totals:
    print(f"{i}: ${category_totals[i]:.2f}")
print(f"\nTotal Income: ${income:.2f}")
print(f"Total Expense: ${expense:.2f}")


balance = income - expense
print(f"Balance: {balance:.2f}")