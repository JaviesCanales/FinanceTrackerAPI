const API_URL = "https://financetrackerapi-production-6cf0.up.railway.app";
const loginForm = document.getElementById("login-form")
const registerForm = document.getElementById("register-form");
const users = []
registerForm.addEventListener("submit", (event) => {
    event.preventDefault();

    console.log("Form submitted!")

    const name = document.getElementById("name").value;
    const email = document.getElementById("register-email").value;
    const password = document.getElementById("register-password").value;
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
        const message = document.getElementById("message");
        if (data.id) {
            message.textContent = "Account created succesfully!";
            message.style.color = "green";
            message.style.display = "block";
            registerForm.reset();
        } else {
            message.textContent = "Registration failed. Email may already be in use";
            message.style.color = "red";
            message.style.display = "block";
        }
    })
    .catch(error => console.error("Error:", error));
});

loginForm.addEventListener("submit", (event) => {
    event.preventDefault();

    console.log("Log in")

    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;

    fetch(`${API_URL}/api/login`, {
        method: "POST",
        headers: {"Content-type": "application/json" },
        body: JSON.stringify({email, password})
    })
    .then(response => response.json())
    .then(data => {
        if (data.token) {
            localStorage.setItem("token", data.token);
            window.location.href = "index.html";
        } else {
            alert("Invalid email or password")
        }
    })
    .catch(error => console.error("Error", error));
});

document.getElementById("show-register").addEventListener("click", (e) => {
    e.preventDefault();
    document.getElementById("login").style.display = "none";
    document.getElementById("register").style.display = "block";
});

document.getElementById("show-login").addEventListener("click", (e) => {
    e.preventDefault();
    document.getElementById("register").style.display = "none";
    document.getElementById("login").style.display = "block";
})

