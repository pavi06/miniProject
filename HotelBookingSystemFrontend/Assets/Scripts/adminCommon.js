var checkAdminLoggedInOrNot = () => {
    if (localStorage.getItem('isLoggedIn')) {
        document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('showNav'));
        document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('hide'));
    }
    else {
        document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('hide'));
        document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('showNav'));
    }
}

var adminLogOut = () => {
    localStorage.clear();
    document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('show'));
    document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('hide'));
    window.location.href = "../login.html";
}

function dropDown() {
    document.querySelectorAll('.sub-btn').forEach(function (subBtn) {
        subBtn.addEventListener('click', function () {
            var subMenu = this.nextElementSibling;
            subMenu.style.display = subMenu.style.display === 'block' ? 'none' : 'block';

            var dropdown = this.querySelector('.dropdown');
            dropdown.classList.toggle('rotate');
        });
    });

    document.querySelector('.menu-btn').addEventListener('click', function () {
        var sideBar = document.querySelector('.side-bar');
        sideBar.classList.add('active');
        this.style.visibility = 'hidden';
    });

    document.querySelector('.close-btn').addEventListener('click', function () {
        var sideBar = document.querySelector('.side-bar');
        sideBar.classList.remove('active');
        document.querySelector('.menu-btn').style.visibility = 'visible';
    });
}

// ---------preloader----------
window.addEventListener("load", () => {
    const loader = document.querySelector(".loader");
  
    loader.classList.add("loader--hidden");
  
    loader.addEventListener("transitionend", () => {
      document.body.removeChild(loader);
    });
});

function addAlert(message){
    document.getElementById("message").innerHTML = message;
    document.getElementById('popup').style.display = 'block';
}

function addSuccessAlert(message){
    document.getElementById("message-success").innerHTML = message;
    document.getElementById('popup-success').style.display = 'block';
}

function closeAlert(){
    document.getElementById('popup').style.display = 'none';
}

window.addEventListener('click', function (event) {
    var popup = document.getElementById('popup');
    if (event.target == popup) {
        popup.style.display = 'none';
    }
});