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

function dropDown () {
    document.querySelectorAll('.sub-btn').forEach(function(subBtn) {
      subBtn.addEventListener('click', function() {
          // Toggle visibility of next .sub-menu element
          var subMenu = this.nextElementSibling;
          subMenu.style.display = subMenu.style.display === 'block' ? 'none' : 'block';
  
          // Toggle 'rotate' class on .dropdown element within clicked .sub-btn
          var dropdown = this.querySelector('.dropdown');
          dropdown.classList.toggle('rotate');
      });
  });
  
  // Add event listener for click on .menu-btn element
  document.querySelector('.menu-btn').addEventListener('click', function() {
      // Add 'active' class to .side-bar element
      var sideBar = document.querySelector('.side-bar');
      sideBar.classList.add('active');
  
      // Hide .menu-btn by setting its visibility to 'hidden'
      this.style.visibility = 'hidden';
  });
  
  // Add event listener for click on .close-btn element
  document.querySelector('.close-btn').addEventListener('click', function() {
      // Remove 'active' class from .side-bar element
      var sideBar = document.querySelector('.side-bar');
      sideBar.classList.remove('active');
  
      // Show .menu-btn by setting its visibility to 'visible'
      document.querySelector('.menu-btn').style.visibility = 'visible';
  });
}


document.addEventListener('DOMContentLoaded', function(){
    checkAdminLoggedInOrNot();
    dropDown();
    fetch('http://localhost:5058/api/AdminHotel/GetAppDetails', {
        method: 'GET',
        headers:{
            'Content-Type':'application/json',
            'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`
        }
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    })
    .then(data => {
        document.getElementById('detailsDiv').innerHTML=`
            <div class="flex flex-column mx-auto p-3" style="width: 80%;">
                    <div class="my-2 fw-bolder flex flex-row justify-between">
                        <p>Hotels Available</p><span class="fw-normal">${data.totalNoOfHotelsAvailable} hotels</span>
                    </div>
                    <div class="my-2 fw-bolder flex flex-row justify-between"><p>Employees</p><span class="fw-normal">${data.totalNoOfEmployees} employees</span></div>
                    <div class="my-2 fw-bolder flex flex-row justify-between"><p>Users</p><span class="fw-normal">${data.totalNoOfUsers} users</span></div>              
                    <h2 class="uppercase m-auto fw-bolder text-orange-300 text-2xl mb-5 mt-2">Analytics</h2>
                    <div>
                        <div class="my-2 fw-bolder flex flex-row justify-between"><p>Average Usage Time</p><span class="fw-normal">${data.averageUsageTime}hrs</span></div>
                        <div class="my-2 fw-bolder flex flex-row justify-between"><p>Average bookings per month</p><span class="fw-normal">${data.averageBookingPerMonth}bookings</span></div>
                    </div>              
            </div>
        `;
    })
    .catch(error => {
        addAlert(error.message)
    });
})

var logOut = () =>{
    localStorage.clear();
    document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('show'));
    document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('hide'));
    window.location.href="../login.html";
}