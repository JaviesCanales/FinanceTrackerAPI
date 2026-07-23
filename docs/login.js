const API_URL = "https://financetrackerapi-production-6cf0.up.railway.app";
const loginForm = document.getElementById("login-form")
const registerForm = document.getElementById("register-form");
const users = []
registerForm.addEventListener("submit", (event) => {
    event.preventDefault();

    console.log("Form submitted!")

    const name = document.getElementById("name").value;
    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;
    const confirmPassword = document.getElementById("confirm-password").value;
    const user = {
        name,
        email,
        password
    };

    if (password !== confirmPassword) {
        alert("Password don't match");
        return;
    }

    fetch(`${API_URL}/api/user`, {
        method: "POST",
        headers: {"Content-type": "application/json" },
        body: JSON.stringify(user)
    })
    .then(response => response.json())
    .then(data => {
        console.log("Account created", data);
        registerForm.reset();
    })
    .catch(error => console.error("Error:", error));
});

