const SESSION_EXPIRATION_TIME = 5 * 60;
const CHECK_ACTIVITY_INTERVAL = 30;
let logoutTimer;


function startSession() {
    clearTimeout(logoutTimer);
    logoutTimer = setTimeout(this.logout, SESSION_EXPIRATION_TIME * 1000);
}

function resetSession() {
    clearTimeout(logoutTimer);
    startSession();
}

function logout() {
    localStorage.clear();
    document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('show'));
    document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('hide'));
}

document.addEventListener('mousemove', resetSession);
document.addEventListener('keypress', resetSession);

