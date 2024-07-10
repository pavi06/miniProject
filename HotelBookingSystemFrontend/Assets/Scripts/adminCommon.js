var checkAdminLoggedInOrNot = () =>{
  if( localStorage.getItem('isLoggedIn')){
      document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('showNav'));        
      document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('hide'));
  }
  else{
      document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('hide'));
      document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('showNav')); 
  }
}

var logOut = () =>{
    localStorage.clear();
    document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('show'));
    document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('hide'));
    window.location.href="../login.html";
}
