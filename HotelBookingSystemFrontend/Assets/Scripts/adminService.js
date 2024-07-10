var functionAddValidEffects = (element) => {
    var name = element.name;
    element.classList.remove("is-invalid");
    element.classList.add("is-valid");
    document.getElementById(`${name}Valid`).innerHTML="valid input!";
    document.getElementById(`${name}Invalid`).innerHTML="";
    return true;
}

var functionAddInValidEffects = (element) => {
    var name = element.name;
    element.classList.remove("is-valid");
    element.classList.add("is-invalid");
    document.getElementById(`${name}Valid`).innerHTML="";
    document.getElementById(`${name}Invalid`).innerHTML=`Please provide the valid ${name}!`;
    return true;
}

// validation
// [A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}
function validateEmail(){
    var element = document.getElementById('email');
    if(element.value){
        return functionAddValidEffects(element, 'email');
    }
    else{
        return functionAddInValidEffects(element, 'email');
    }
}
var validatePassword = () => {
    var regexExpression = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[@$&*_])(?=.*[0-9]).{6,}$/;
    var element = document.getElementById('password');
    if(element.value && element.value.match(regexExpression)){
        return functionAddValidEffects(element);
    }
    else{
        return functionAddInValidEffects(element);
    }
}

var validateAddress=(id)=>{
    var element = document.getElementById(id);
    var addressRegex = /^[a-zA-Z0-9\s,'-]*$/;
    if(element.value && element.value.match(addressRegex)){
        return functionAddValidEffects(element);
    }
    else{
        return functionAddInValidEffects(element);
    }
}

var validate = (id) =>{
    var element = document.getElementById(id);
    var regString = /[a-zA-Z]/g
    if(element.value && element.value.match(regString)){
        return functionAddValidEffects(element);
    }
    else{
        return functionAddInValidEffects(element);
    }
}

var validateDate = () =>{
    var dateElement = document.getElementById('checkInDate');
    console.log(dateElement)
    var today = new Date();
    let formattedDate = `${today.getFullYear()}-${(today.getMonth() + 1).toString().padStart(2, '0')}-${today.getDate().toString().padStart(2, '0')}`;
    console.log(formattedDate)
    if(dateElement.value && Date.parse(dateElement.value) >= Date.parse(formattedDate)) {
        functionAddValidEffects(dateElement);
        return true;
    } else {
        functionAddInValidEffects(dateElement);
        return false;
    }
}

var validateNumber = (id) =>{
    var data = document.getElementById(`${id}`);
    var reg = /^\d+$/;
    if(data.value.match(reg)){
        return functionAddValidEffects(data);
    }
    else{
        return functionAddInValidEffects(data);
    }
}

var validateDecimalNumber = (id) =>{
    var data = document.getElementById(`${id}`);
    var pattern = /^[+]?\d+(\.\d*)?$/;
    if(data.value.match(pattern)){
        return functionAddValidEffects(data);
    }
    else{
        return functionAddInValidEffects(data);
    }
}

var validateLocation = () =>{
    var locationElement = document.getElementById('location');
    console.log(locationElement)
    var reg=/^[a-zA-Z]+$/;
    if(locationElement.value && locationElement.value.match(reg)){
        functionAddValidEffects(locationElement);
        return true;
    }
    else{
        functionAddInValidEffects(locationElement);
        return false;
    }
}

function validatePhone(){
    var element = document.registrationForm.phoneNumber;
    var regPhone =  /^[+]{1}(?:[0-9\-\\(\\)\\/.]\s?){6,15}[0-9]{1}$/;
    if(element.value && element.value.match(regPhone)){
        return functionAddValidEffects(element);
    }
    else{
        return functionAddInValidEffects(element);
    }
}

function loadHotels (){
    fetch('http://localhost:5058/api/AdminHotel/GetAllHotelsWithoutLimit', {
        method: 'GET',
        headers:{'Content-Type':'application/json'}
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    })
    .then(data => {
        addCityDatalist(data)
        displayHotelsForAdmin(data);
    })
    .catch(error => {
        addAlert(error.message);
    });
}

var displayHotelsForAdmin = (data) => {
    var hotelsList = data.map((hotel) => {
        var starRatingHtml = ""; 
        for (let i = 0; i < hotel.rating; i++) {
            starRatingHtml+=`<span class="fa fa-star fa-lg checked p-0.5"></span>`;
        }
        return `
        <div class="px-3 pb-5 mb-10 h-auto mx-auto cardDesign loadOnScroll" style="width: 80%;">
            <div class="hotelsFlex">                      
                <div class="mt-4 imageDiv">
                    <img src="https://drive.google.com/thumbnail?id=1gizbhn8kXaSVBIIVE8sdIonEmk0e01tN" alt="Image gallery"/>
                </div>
                <div class="p-3">
                    <p class="hotelName">${hotel.name}</p>
                    <a href="#"><i class="bi bi-geo-alt-fill"></i>&nbsp;&nbsp;${hotel.address}</a>
                    <p class="description">Amenities: ${hotel.amenities}<br>Restriction: ${hotel.restrictions}<br>RoomTypes: ${Object.values(hotel.roomTypes)}<br>Total No. of Rooms Available: ${hotel.totalNoOfRooms}</p>
                </div>
                <div class="flex flex-col justify-between mt-3" style="float:right">
                    <div>
                        <p class="description"><span class="review">Review Score</span></p>
                         ${starRatingHtml}
                        <p class="description text-center fw-bolder"><a href="#">${hotel.ratingCount} reviews</a></p>
                    </div>
                </div>
            </div>
            <div class="flex flex-row flex-wrap px-3 mt-3 justify-center">
                <button type="button" class="buttonStyle mr-5 px-2" id="editHotelBtn" onclick="openModalForEdit(${hotel.hotelId})"><span>Edit Hotel</span></button>
                <button type="button" class="buttonStyle mr-5 px-2" id="addRoomTypeBtn" onclick="routeRoomType(${hotel.hotelId})"><span>Add RoomType</span></button>
                <button type="button" class="buttonStyle mr-5 px-2" id="addRoomBtn" onclick="openModalForAdd(${hotel.hotelId})"><span>Add Room</span></button>
            </div>
            <div class="flex flex-row flex-wrap px-3 mt-3 justify-center">
                <button type="button" class="buttonStyle mr-5 px-2" id="addRoomBtn" onclick="updateHotelStatus(${hotel.hotelId})"><span>Update status</span></button>
                <button type="button" class="buttonStyle mr-5 px-2" id="addRoomBtn" onclick="routeAllRoomTypes(${hotel.hotelId})"><span>RoomTypes</span></button>
            </div>
        </div>
        `;
    });
    document.getElementById("displayAllHotels").innerHTML = hotelsList.join('');
    lazyload();
}

var routeRoomType = (hotelId) =>{
    localStorage.setItem('currentHotel', hotelId)    
    window.location.href="./AddRoomType.html";
}

// -----------update hotel---------
function openModalForEdit(id){
    localStorage.setItem('currentHotel',id)
    const editModal = new bootstrap.Modal(document.getElementById('editHotelModal'));
    editModal.show();
    document.getElementById('editHotelModal').addEventListener('hidden.bs.modal', function () {
        resetFormValues('updateHotelForm','input, textarea');
    });
}

function updateHotel(){
    var data = document.getElementById('updateHotelForm').querySelectorAll('input , textarea');
    var attributeValuesPairs = {};
    Array.from(data).forEach(ele => {
        if(ele.value){
            attributeValuesPairs[ele.name] = ele.value;
        }
    });
    var updatedHotel = {
        hotelId : localStorage.getItem('currentHotel'),
        attributeValuesPair : attributeValuesPairs
    }
    fetch('http://localhost:5058/api/AdminHotel/UpdateHotel',{
        method:'PUT',
        headers:{
            'Authorization':`Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`,
            'Content-Type' : 'application/json'
        },
        body:JSON.stringify(updatedHotel)
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    }).then(data => {
        addSuccessAlert('Hotel updated successfully!')
        document.querySelector('[data-bs-dismiss="modal"]').click();
        loadHotels();
    }).catch(error => {
        addAlert(error.message)
    });
}
// ----------------------------

//------------Update hotel status---------
var updateHotelStatus = (hotelId) =>{
    fetch('http://localhost:5058/api/AdminHotel/UpdateHotelAvailabilityStatus', {
        method: 'PUT',
        headers:{
            'Content-Type':'application/json',
            'Authorization':`Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`,
        },
        body:JSON.stringify(hotelId)
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    })
    .then(data => {
        addSuccessAlert('Hotel status updated!')
        loadHotels();
    })
    .catch(error => {
        addAlert(error.message)
    });
}


//---------Add Room---------
var addRoomTypes = (itemName) =>{
    var hotelId = localStorage.getItem(`${itemName}`);
    fetch('http://localhost:5058/api/AdminHotel/GetHotel',{
        method:'POST',
        headers:{
            'Content-Type' : 'application/json'
        },
        body:JSON.stringify(hotelId)
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    })
    .then(data => {
        var roomTypesHtml ="";
        Object.entries(data.roomTypes).forEach(([key, value]) => {
            roomTypesHtml += `
                <option value="${key}">${value}</option>
            `;
        });
        document.getElementById('roomTypesSelect').innerHTML=roomTypesHtml;
    })
    .catch(error => {
        addAlert(error.message)
    });
}

function openModalForAdd(id){
    localStorage.setItem('currentHotel', id);
    addRoomTypes('currentHotel')
    const addRoomModal = new bootstrap.Modal(document.getElementById('addRoomModal'));
    addRoomModal.show();
    document.getElementById('addRoomModal').addEventListener('hidden.bs.modal', function (e) {
        resetFormValues('AddRoomForm');
    });
}

function AddRoom(){
    var roomData = {
        hotelId : localStorage.getItem('currentHotel'),
        typeId : document.getElementById('roomTypesSelect').value,
        images : document.getElementById('roomImages').value
    };
    fetch('http://localhost:5058/api/AdminRoom/RegisterRoomForHotel',{
        method:'POST',
        headers:{
            'Content-Type':'application/json',
            'Authorization' : `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`
        },
        body:JSON.stringify(roomData)
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        resetFormValues('AddRoomForm','input')
        return await res.json();
    }).then(data => {
        addSuccessAlert('Room added successfully!')
        document.querySelector('[data-bs-dismiss="modal"]').click();
    }).catch(error => {
        addAlert(error.message)
    });
}
//----------------------------


// --------------Route roomtypes----------
var routeAllRoomTypes = (hotelId) =>{
    localStorage.setItem('currentHotel', hotelId)    
    window.location.href="./RoomTypes.html";
}
// ---------------------------

var filterHotels = () =>{
    var location = document.getElementById('filterValue').value;
    if(!location){
        addAlert("Provide the valid location!");
        return;
    }
    if(location === 'All'){
        loadHotels();
        return;
    }
    fetch('http://localhost:5058/api/AdminHotel/GetAllHotelsByLocation',{
        method:'POST',
        headers:{
            'Content-Type':'application/json',
        },
        body:JSON.stringify(location)
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    }).then(data => {
        displayHotelsForAdmin(data);
    }).catch(error => {
        addAlert(error.message)
    });
}

document.addEventListener('DOMContentLoaded', function() {
    checkAdminLoggedInOrNot();
    loadHotels();
    dropDown();
})

// ------lazy loading
var lazyload = () => {
    const items = document.querySelectorAll('.loadOnScroll');

    const observer = new IntersectionObserver(entries => {
        entries.forEach(entry => {
            entry.target.classList.toggle('show', entry.isIntersecting);
        });
    }, {
        threshold: 0.2,
        rootMargin: '20px'
    });

    items.forEach(item => {
        observer.observe(item);
    });
};

// --------add cities to the datalist----------
var addCityDatalist = (data) =>{
    var dataList = document.getElementById('datalistCityOptions');
    dataList.innerHTML = '';
    var uniqueCities = [...new Set(data.map(hotel => hotel.city))];
    dataList.innerHTML = '<option value="All">';
    dataList.innerHTML += uniqueCities.map(city => `<option value="${city}">`).join('');
}

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
  
  var addAlert = (message) =>{
    if(document.getElementById('alertModal')){
        document.getElementById('alertMessage').innerHTML = message;
        const modal = new bootstrap.Modal(document.getElementById('alertModal'));
        modal.show();
        return;
    }
    const alert = document.createElement('div')
    alert.innerHTML = `
         <div class="modal" id="alertModal">
            <div class="modal-dialog">
                <div class="modal-content" style="border-radius:25px">
                <div class="modal-header bg-red-400" style="border-bottom:none;height:15px;">
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
    if(document.getElementById('loadBtn')){
        document.getElementById('loadBtn').classList.add('hide');
    }
  }
  
var addSuccessAlert = (message) =>{
    if(document.getElementById('successAlertModal')){
        document.getElementById('successAlertModal').innerHTML = message;
        const modal = new bootstrap.Modal(document.getElementById('successAlertModal'));
        modal.show();
        return;
    }
    const alert = document.createElement('div')
    alert.innerHTML = `
         <div class="modal" id="successAlertModal">
            <div class="modal-dialog">
                <div class="modal-content" style="border-radius:25px">
                <div class="modal-header bg-green-400" style="border-bottom:none;height:15px;">
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
    if(message.includes('No hotels are available!') || message === "No more hotels available!"){
        if( document.getElementById('loadBtn')){
            document.getElementById('loadBtn').classList.add('hide');
        }
        if(document.getElementById('loadBtnWithBody')){
            document.getElementById('loadBtnWithBody').classList.add('hide');
        }
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
  
  document.querySelector('.menu-btn').addEventListener('click', function() {
      var sideBar = document.querySelector('.side-bar');
      sideBar.classList.add('active');
      this.style.visibility = 'hidden';
  });
  
  document.querySelector('.close-btn').addEventListener('click', function() {
      var sideBar = document.querySelector('.side-bar');
      sideBar.classList.remove('active');
      document.querySelector('.menu-btn').style.visibility = 'visible';
  });
}

var logOut = () =>{
    localStorage.clear();
    document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('show'));
    document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('hide'));
    window.location.href="../login.html";
}

var resetFormValues = (formName, formTypes) => {
    document.getElementById(formName).reset();
    const formInputs = document.getElementById(formName).querySelectorAll(formTypes);
    formInputs.forEach(input => {
    //removing the classlist added and empty small element
    input.classList.remove('is-valid', 'is-invalid');
    document.getElementById(`${input.name}Valid`).innerHTML="";
    document.getElementById(`${input.name}Invalid`).innerHTML="";
    });
}
