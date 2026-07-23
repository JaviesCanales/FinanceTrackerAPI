const API_URL = "https://financetrackerapi-production-6cf0.up.railway.app/api/transactions";
const form = document.getElementById("transaction-form");
const transactions = []
const transactionList = document.getElementById("transaction-list");
form.addEventListener("submit", (event) => {
    event.preventDefault();

    console.log("Form submitted!");

    const description = document.getElementById("description").value;
    const amount = Number(document.getElementById("amount").value);
    const category = capitalize(document.getElementById("category").value);
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
        const date = new Date (t.date).toLocaleDateString('en-US', {
        month: "short",
        day: "numeric",
        year: "numeric"
    });
        const li = document.createElement("li");
        li.textContent = `${capitalize(t.description)} - $${t.amount} - ${capitalize(t.category)} (${t.type}) - ${date}`;
        
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

        li.appendChild(editBtn); 
        li.appendChild(deleteBtn);
        transactionList.appendChild(li);
    });
    let income = 0;
    let expense = 0;
    let categoryTotal = {};
    transactions.forEach((t) => {
        if (t.type.toLowerCase() === "income") {
            income += t.amount;
        }
        if (t.type.toLowerCase() === "expense") {
            expense += t.amount;
          
            if (!categoryTotal[capitalize(t.category)]) {
                categoryTotal[capitalize(t.category)] = 0;
            }
            categoryTotal[capitalize(t.category)] += t.amount;
        }
    });

    const balance = income - expense;
    if (window.categoryChart) {
        window.categoryChart.destroy();
    }
    const ctx = document.getElementById("category-chart").getContext("2d");
    window.categoryChart = new Chart(ctx, {
        type: "doughnut",
        data: {
            labels: Object.keys(categoryTotal),
            datasets: [{
                data: Object.values(categoryTotal),
                backgroundColor: [
                    "#2980b9",
                    "#27ae60",
                    "#e74c3c",
                    "#f39c12",
                    "#8e44ad",
                    "#16a085",
                    "#d35400"
                ]
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: "bottom"
                },
                tooltip: {
                    callbacks: {
                        label: function(context) {
                            return ` ${context.label}: $${context.parsed.toFixed(2)}`;
                        }
                    }
                }
            }
        }
    });
    const categoryList = document.getElementById("category-list");
    categoryList.innerHTML = "";
    
    const total = Object.values(categoryTotal).reduce((a,b) => a + b, 0);

    Object.entries(categoryTotal).forEach(([category, amount]) => {
        const li = document.createElement("li");
        const percentage = ((amount / total) * 100).toFixed(1);
        li.textContent = `${category}: $${amount.toFixed(2)} (${percentage}%)`;
        categoryList.appendChild(li);
    });
    document.getElementById("total-income").textContent = `Income: $${income.toFixed(2)}`;
    document.getElementById("total-expense").textContent = `Expense: $${expense.toFixed(2)}`;
    document.getElementById("total-balance").textContent = `Balance: $${balance.toFixed(2)}`;

    const balanceEl = document.getElementById("total-balance");
    balanceEl.style.color = balance < 0 ? "#c0392b" : "#1a3c5e";
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

function capitalize(str) {
    return str[0].toUpperCase() + str.slice(1);
}



loadTransactions();