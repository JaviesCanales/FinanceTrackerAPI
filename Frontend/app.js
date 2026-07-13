const API_URL = "http://localhost:5118/api/transactions"
const form = document.getElementById("transaction-form");
const transactions = []
const transactionList = document.getElementById("transaction-list");
form.addEventListener("submit", (event) => {
    event.preventDefault();

    console.log("Form submitted!");

    const description = document.getElementById("description").value;
    const amount = Number(document.getElementById("amount").value);
    const category = document.getElementById("category").value;
    const type = document.getElementById("type").value;
    const transaction = {
        description,
        amount,
        category,
        type
    };

    fetch(API_URL, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(transaction)
    })
    .then(response => response.json())
    .then(data => {
        console.log("Transaction added", data);
        loadTransactions();
        form.reset();
    })
    .catch(error => console.error("Error:", error));
});


function renderTransactions() {
    transactionList.innerHTML = "";
    
    transactions.forEach((t) => {
        const li = document.createElement("li");
        li.textContent = `${t.description} - $${t.amount} - ${t.category} (${t.type})`;
        
        const deleteBtn = document.createElement("button");
        deleteBtn.textContent = "Delete";

        deleteBtn.addEventListener("click", () => {
            const confirmed = confirm("Are you sure you want to delete this transaction?");
            if (!confirmed) {
                return;
            }

            fetch(`${API_URL}/${t.id}`, {
                method: "DELETE"
            })
            .then(() => {
                loadTransactions();
            })
            .catch(error => console.error("Error:", error));
        });
        const editBtn = document.createElement("button");
        editBtn.textContent = "Edit";
        editBtn.addEventListener("click", () => {
            li.innerHTML = `
            <input id="edit-desc" value="${t.description}" />
            <input id="edit-amount" type="number" value="${t.amount}" />
            <input id="edit-category" value="${t.category}" />
            <select id="edit-type">
                <option value="income" ${t.type.toLowerCase() === 'income' ? 'selected' : ''}>Income</option>
                <option value="expense" ${t.type.toLowerCase() === 'expense' ? 'selected' : ''}>Expense</option>
            </select>
            <button id="save-btn">Save</button>
            <button id="cancel-btn">Cancel</button>
            `;
            document.getElementById("save-btn").addEventListener("click", () => {
                const updated = {
                    description: document.getElementById("edit-desc").value,
                    amount: Number(document.getElementById("edit-amount").value),
                    category: document.getElementById("edit-category").value,
                    type: document.getElementById("edit-type").value
                };
                fetch(`${API_URL}/${t.id}`, {
                    method: "PUT",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(updated)
                })
                .then(() => loadTransactions())
                .catch(error => console.error("Error", error));
            });
            
            document.getElementById("cancel-btn").addEventListener("click", () => {
                loadTransactions();
            });
        });

        li.appendChild(deleteBtn);
        li.appendChild(editBtn); 
        transactionList.appendChild(li);
    });
    let income = 0;
    let expense = 0;
    let categoryTotal = {};
    transactions.forEach((t) => {
        if (t.type.toLowerCase() === "income"){
            income += t.amount;
        }
        else{
            expense += t.amount;
        }

        if (!categoryTotal[t.category]) {
            categoryTotal[t.category] = 0;
        }
        categoryTotal[t.category] += t.amount;
    });

    const balance = income - expense;
    const categoryList = document.getElementById("category-list");
    categoryList.innerHTML = "";
    Object.entries(categoryTotal).forEach(([category, total]) => {
        const li = document.createElement("li");
        li.textContent = `${category}: $${total.toFixed(2)}`;
        categoryList.appendChild(li);
    });

    document.getElementById("total-income").textContent = `Income: $${income.toFixed(2)}`;
    document.getElementById("total-expense").textContent = `Expense: $${expense.toFixed(2)}`;
    document.getElementById("total-balance").textContent = `Balance: $${balance.toFixed(2)}`;
}

function loadTransactions() {
    fetch (API_URL)
    .then(response => response.json())
    .then(data => {
        transactions.length = 0;
        data.forEach(t=> transactions.push(t));
        renderTransactions(); 
    });
}



loadTransactions();