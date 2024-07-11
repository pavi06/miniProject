// var checkAdminLoggedInOrNot = () =>{
//     if( localStorage.getItem('isLoggedIn')){
//         document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('showNav'));        
//         document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('hide'));
//     }
//     else{
//         document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('hide'));
//         document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('showNav')); 
//     }
// }

// function dropDown () {
//     document.querySelectorAll('.sub-btn').forEach(function(subBtn) {
//       subBtn.addEventListener('click', function() {
//           // Toggle visibility of next .sub-menu element
//           var subMenu = this.nextElementSibling;
//           subMenu.style.display = subMenu.style.display === 'block' ? 'none' : 'block';
  
//           // Toggle 'rotate' class on .dropdown element within clicked .sub-btn
//           var dropdown = this.querySelector('.dropdown');
//           dropdown.classList.toggle('rotate');
//       });
//   });
  
//   // Add event listener for click on .menu-btn element
//   document.querySelector('.menu-btn').addEventListener('click', function() {
//       // Add 'active' class to .side-bar element
//       var sideBar = document.querySelector('.side-bar');
//       sideBar.classList.add('active');
  
//       // Hide .menu-btn by setting its visibility to 'hidden'
//       this.style.visibility = 'hidden';
//   });
  
//   // Add event listener for click on .close-btn element
//   document.querySelector('.close-btn').addEventListener('click', function() {
//       // Remove 'active' class from .side-bar element
//       var sideBar = document.querySelector('.side-bar');
//       sideBar.classList.remove('active');
  
//       // Show .menu-btn by setting its visibility to 'visible'
//       document.querySelector('.menu-btn').style.visibility = 'visible';
//   });
// }

var validateAndAddHotel = () =>{
    if(!localStorage.getItem('isLoggedIn')){
        addAlert("Login to continue!");
        window.location.href='../login.html';
        return;
    }
    if(validate('hotelName') && validateAddress('address') && validate('city') && validateNumber('roomsCount')
    && validate('amenities') && validate('restriction')){
        fetch('http://localhost:5058/api/AdminHotel/RegisterHotel',
            {
                method:'POST',
                headers:{
                    'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`,
                    'Content-Type' : 'application/json',                
                },
                body:JSON.stringify({
                    name: document.getElementById('hotelName').value,
                    address: document.getElementById('address').value,
                    city: document.getElementById('city').value,
                    totalNoOfRooms: document.getElementById('roomsCount').value,
                    amenities: document.getElementById('amenities').value,
                    restrictions: document.getElementById('restriction').value
                })
            }
        )
        .then(async (res) => {
            if (!res.ok) {
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            resetFormValues('AddHotelForm','input, textarea');
            return await res.json();
        })
        .then( data => {
            addSuccessAlert("Hotel Registered Successfully!")
        }).catch( error => {
            addAlert(error.message)
            resetFormValues('AddHotelForm','input, textarea');
            }
        );
    }else{
        addAlert("Provide all values properly to register!");
    }
}

document.addEventListener('DOMContentLoaded',function(){
    dropDown();
    checkAdminLoggedInOrNot();
})
