function loadHotels (){
    fetch('http://localhost:5058/api/AdminHotel/GetAllHotelsWithoutLimit', {
        method: 'GET',
        headers:{'Content-Type':'application/json'}
    })
    .then(async(res) => {
        if (!res.ok) {
            if (res.status === 401) {
                throw new Error('Unauthorized Access!');
            }
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
        if(error.message === "Unauthorized Access!"){
            adminLogOut();
        }
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

async function updateHotel(){
    var data = document.getElementById('updateHotelForm').querySelectorAll('input , textarea');
    var attributeValuesPairs = {};
    var count=0;
    Array.from(data).forEach(ele => {
        if(ele.value){
            attributeValuesPairs[ele.name] = ele.value;
        }else{
            count+=1;
        }
    });
    if(count === data.length){
        addAlert("Please provide data for update!");
        return;
    }
    var updatedHotel = {
        hotelId : localStorage.getItem('currentHotel'),
        attributeValuesPair : attributeValuesPairs
    }
    var res = await checkTokenAboutToExpiry(JSON.parse(localStorage.getItem('loggedInUser')).accessToken);
    if (res === "Refresh") {
        await refreshToken();
    } else if (res === "Invalid accessToken!") {
        addAlert("Invalid AccessToken!");
        return;
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
            if (res.status === 401) {
                throw new Error('Unauthorized Access!');
            }
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    }).then(data => {
        addSuccessAlert('Hotel updated successfully!');
        document.querySelector('[data-bs-dismiss="modal"]').click();
        loadHotels();
    }).catch(error => {
        addAlert(error.message)
        if(error.message === "Unauthorized Access!"){
            adminLogOut();
        }
    });
}
// ----------------------------

//------------Update hotel status---------
var updateHotelStatus = async (hotelId) =>{
    var res = await checkTokenAboutToExpiry(JSON.parse(localStorage.getItem('loggedInUser')).accessToken);
    if (res === "Refresh") {
        await refreshToken();
    } else if (res === "Invalid accessToken!") {
        addAlert("Invalid AccessToken!");
        return;
    }
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
            if (res.status === 401) {
                throw new Error('Unauthorized Access!');
            }
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    })
    .then(data => {
        addSuccessAlert('Hotel status updated!');
        loadHotels();
    })
    .catch(error => {
        addAlert(error.message)
        if(error.message === "Unauthorized Access!"){
            adminLogOut();
        }
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
            if (res.status === 401) {
                throw new Error('Unauthorized Access!');
            }
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
        if(error.message === "Unauthorized Access!"){
            adminLogOut();
        }
    });
}

function openModalForAdd(id){
    localStorage.setItem('currentHotel', id);
    addRoomTypes('currentHotel')
    const addRoomModal = new bootstrap.Modal(document.getElementById('addRoomModal'));
    addRoomModal.show();
    document.getElementById('addRoomModal').addEventListener('hidden.bs.modal', function (e) {
        resetFormValues('AddRoomForm','input');
    });
}

async function AddRoom(){
    if(!(document.getElementById('roomTypesSelect').value && document.getElementById('roomImages').value)){
        addAlert("Please provide data to add room!");
        return;
    }
    var roomData = {
        hotelId : localStorage.getItem('currentHotel'),
        typeId : document.getElementById('roomTypesSelect').value,
        images : document.getElementById('roomImages').value
    };
    var res = await checkTokenAboutToExpiry(JSON.parse(localStorage.getItem('loggedInUser')).accessToken);
    if (res === "Refresh") {
        await refreshToken();
    } else if (res === "Invalid accessToken!") {
        addAlert("Invalid AccessToken!");
        return;
    }
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
            if (res.status === 401) {
                throw new Error('Unauthorized Access!');
            }
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        resetFormValues('AddRoomForm','input')
        return await res.json();
    }).then(data => {
        document.querySelector('[data-bs-dismiss="modal"]').click();
        addSuccessAlert('Room added successfully!');
    }).catch(error => {
        addAlert(error.message)
        if(error.message === "Unauthorized Access!"){
            adminLogOut();
        }
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
            if (res.status === 401) {
                throw new Error('Unauthorized Access!');
            }
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    }).then(data => {
        displayHotelsForAdmin(data);
    }).catch(error => {
        addAlert(error.message)
        if(error.message === "Unauthorized Access!"){
            adminLogOut();
        }
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




