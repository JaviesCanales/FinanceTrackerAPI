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
for transaction in transactions:
    if transaction[3] in category_totals:
        category_totals[transaction[3]] += transaction[2]
    else:
        category_totals[transaction[3]] = transaction[2]
    
for i in category_totals:
    print(i, category_totals[i])