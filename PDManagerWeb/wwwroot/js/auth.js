const login = document.getElementById("login"),
    password = document.getElementById("password"),
    errMsg = document.getElementById("errMsg");
async function RegAuth(reg) {
    if (login.value === "" || password.value === "") {
        errMsg.innerText = "Логин и пароль не могут быть пустыми!";
        return;
    }
    const request = new Request(reg ? '/api/Accounts' : '/api/Accounts/Auth');
    const data = new FormData();
    data.append('login', login.value);
    data.append('password', password.value);
    const options = {
        method: "POST",
        body: data
    };
    const response = await fetch(request, options);
    if (!response.ok) {
        errMsg.innerText = "Ошибка связи с сервером!";
        return;
    }
    const json = await response.json();
    if (json.result === 0) {
        errMsg.innerText = json.message;
    }
    else {
        location.reload();
    }
}
document.getElementById("regBut").addEventListener('click', () => { RegAuth(true) });
document.getElementById("authBut").addEventListener('click', () => { RegAuth(false) });