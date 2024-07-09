var displayUser = (data) => {
    var usersHtml = "";
    data.forEach(user => {
        usersHtml+=`
            <div class="px-3 pb-3 mb-10 h-auto mx-auto cardDesign">
                <div class="flex flex-row justify-between">                      
                    <div class="p-3 mx-auto">
                        <p class="uppercase fw-bolder mb-3 text-center text-2xl" style="color: #FFA456;">User</p>
                        <div class="grid grid-cols-2">
                            <p class="uppercase fw-bolder text-lg">Guest Id</p>
                            <p class="lowercase fw-normal px-3 text-lg">${user.guestId}</p>
                        </div>
                        <div class="grid grid-cols-2">
                            <p class="uppercase fw-bolder text-lg">Email</p>
                            <p class="lowercase fw-normal px-3 text-lg">${user.email}</p>
                        </div>
                        <div class="grid grid-cols-2">
                            <p class="uppercase fw-bolder text-lg">PhoneNo.</p>
                            <p class="lowercase fw-normal px-3 text-lg">${user.phoneNumber}</p>
                        </div>
                        <div class="grid grid-cols-2">
                            <p class="uppercase fw-bolder text-lg">Role</p>
                            <p class="lowercase fw-normal px-3 text-lg">${user.role}</p>
                        </div>
                    </div>
                </div>
                <div class="flex flex-row flex-wrap px-3 mt-3 justify-center align-middle">
                    <button type="button" class="buttonStyle mr-5" id="activateUserBtn" onclick="updateUserStatus(${user.guestId},'Active')"><span>Activate User</span></button>
                    <button type="button" class="buttonStyle" id="disableUserBtn" onclick="updateUserStatus(${user.guestId}, 'Disabled')"><span>Disable User</span></button>
                </div>
            </div>
        `;
    });
    document.getElementById('displayUsers').innerHTML = usersHtml;
}

var updateUserStatus = (guestId, status) => {
    fetch('http://localhost:5058/api/AdminBasic/UpdateUserStatus',{
        method:'PUT',
        headers:{
            'Content-Type' : 'application/json',
            'Authorization':`Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`,
        },
        body: JSON.stringify({
            guestId:guestId,
            status : status
        })
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    })
    .then(data => {
        addSuccessAlert(`User ${status} successfullly`)
        fetchUsersForActivation();
    })
    .catch(error => {
        addAlert(error.message)
    });
}

var fetchUsersForActivation = () =>{
    fetch('http://localhost:5058/api/AdminBasic/GetAllUsersForActivaion',{
        method:'GET',
        headers:{
            'Content-Type' : 'application/json',
            'Authorization':`Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`,
        },
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    })
    .then(data => {
        displayUser(data);
    })
    .catch(error => {
        addAlert(error.message)
    });
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

  var checkAdminLoggedInOrNot = () =>{
    if( localStorage.getItem('isLoggedIn')){
        document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('show'));        
        document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('hide'));
    }
    else{
        document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('hide'));
        document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('show')); 
    }
  }
  

document.addEventListener('DOMContentLoaded', function(){
    checkAdminLoggedInOrNot();
    dropDown();
    fetchUsersForActivation();
})

var addSuccessAlert = (message) =>{
    if(document.getElementById('successAlertModal')){
        document.getElementById('successAlertMessage').innerHTML = message;
        const modal = new bootstrap.Modal(document.getElementById('successAlertModal'));
        modal.show();
        return;
    }
    const alert = document.createElement('div')
    alert.innerHTML = `
         <div class="modal" id="successAlertModal">
            <div class="modal-dialog">
                <div class="modal-content">
                <div class="modal-header bg-green-400">
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <img class="flex mx-auto" src="../../Assets/Images/success.png" style="width:40%; height:40%;"/>
                <h5 class="text-2xl mt-0" style="font-weight:bolder;text-transform:uppercase;text-align:center;color:green;">SUCCESS</h5>
                <div class="modal-body text-center">
                    <p class="text-xl text-black" id="successAlertMessage">${message}</p>
                </div>
                <button type="button" class="btn uppercase w-25 text-center mx-auto my-3 bg-green-400  fw-bolder alertBtn" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    `;
    document.body.insertAdjacentElement('beforeend', alert);
    const modal = new bootstrap.Modal(document.getElementById('successAlertModal'));
    modal.show();
}

var addAlert = (message) =>{
    if(document.getElementById('alertModal')){
        document.getElementById('alertMessage').innerHTML = message;
        const modal = new bootstrap.Modal(document.getElementById('alertModal'));
        modal.show();
        return;
    }
    const alert = document.createElement('div')
    alert.innerHTML = `
         <div class="modal" id="alertModal" style="border-radius:50px">
            <div class="modal-dialog">
                <div class="modal-content">
                <div class="modal-header bg-red-400">
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <img class="flex mx-auto" src="../../Assets/Images/error.png" style="width:40%; height:40%;"/>
                <h5 class="text-2xl mt-0" style="font-weight:bolder;text-transform:uppercase;text-align:center;color:red;">Oops!</h5>
                <div class="modal-body text-center">
                    <p class="text-xl text-black" id="alertMessage">${message}</p>
                </div>
                <button type="button" class="btn uppercase w-25 text-center mx-auto my-3 bg-red-400  fw-bolder alertBtn" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    `;
    document.body.insertAdjacentElement('beforeend', alert);
    const modal = new bootstrap.Modal(document.getElementById('alertModal'));
    modal.show();
}

var logOut = () =>{
    localStorage.clear();
    document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('show'));
    document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('hide'));
    window.location.href="../login.html";
}