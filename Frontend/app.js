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

        li.appendChild(deleteBtn);
        transactionList.appendChild(li);
    });
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