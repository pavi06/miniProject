// const hamBurger = document.querySelector(".toggle-btn");

// hamBurger.addEventListener("click", function () {
//   document.querySelector("#sidebar").classList.toggle("expand");
// });

// var toggleDropdown = (id) => {
//     var dropdown = document.getElementById(id);
//     dropdown.classList.toggle('active');
// }

function loadHotels (){
    fetch('http://localhost:5058/api/AdminHotel/GetAllHotels', {
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
        console.log(data);
        displayHotelsForAdmin(data);
    })
    .catch(error => {
        alert(error);
        console.error(error);
    });
}

var displayHotelsForAdmin = (data) => {
    // var hotelsList ="";
    data.forEach(hotel => {
        var roomTypes = "";
        hotel.roomTypes.forEach(type => {
            roomTypes+=type.type;
            roomTypes+=","
        });
        console.log(roomTypes);
        var reviewsCount = hotel.ratings.length; 
        var roomTypesObject = [...hotel.roomTypes];
        console.log("Room Types")
        console.log(roomTypesObject)
        var hotelsList = data.map(hotel => {
            console.log(hotel);
            return `
            <div class="px-3 pb-5 mb-10 h-auto mx-auto cardDesign" style="width: 80%;">
                <div class="flex flex-row justify-between">                      
                    <div class="w-60 h-50 mt-4" style="object-fit: cover;">
                        <img src="./Assets/Images/hotelImage5.jpg" alt="Image gallery"/>
                    </div>
                    <div class="p-3">
                        <p class="hotelName">${hotel.name}</p>
                        <a href="#"><i class="bi bi-geo-alt-fill"></i>&nbsp;&nbsp;${hotel.address}</a>
                        <p class="description">Amenities:${hotel.amenities}  <br> Restriction : ${hotel.restrictions}<br>RoomTypes : ${roomTypes}<br>Total No. of Rooms Available : ${hotel.totalNoOfRooms}</p>
                    </div>
                    <div class="flex flex-col justify-between mt-3" style="float:right">
                        <div>
                            <p class="description"><span class="review">Review Score</span></p>
                            <p class="description" style="line-height: 10px;">${hotel.rating}</p>
                            <p class="description"><a href="#">${reviewsCount} reviews</a></p>
                        </div>
                    </div>
                </div>
                <div class="flex flex-row px-3 mt-3 justify-center">
                    <button type="button" class="buttonStyle mr-5" style="width:20%; padding:5px" id="editHotelBtn" onclick="openModalForEdit(${hotel.hotelId})"><span>Edit Hotel</span></button>
                    <button type="button" class="buttonStyle" style="width:20%; padding:5px" id="addRoomTypeBtn" onclick="routeRoomType(${hotel})"><span>Add RoomType</span></button>
                    <button type="button" class="buttonStyle mr-5" style="width:20%; padding:5px" id="addRoomBtn" onclick="openModalForAdd(${hotel.hotelId},${roomTypesObject})"><span>Add Room</span></button>
                </div>
            </div>
            `;
        });
        document.getElementById("displayAllHotels").innerHTML = hotelsList.join('');
    });
}

var routeRoomType = (hotel) =>{
    localStorage.setItem('currentActiveHotel', hotel)
    window.location.href="./AddRoomType.html";
}

function openModalForEdit(id){
    const editModal = new bootstrap.Modal(document.getElementById('editHotelModal'));
    localStorage.setItem('currentHotel', id);
    console.log(id);
    editModal.show();
}

function openModalForAdd(id, roomTypes){
    localStorage.setItem('currentHotel', id);
    var roomSelect = document.getElementById('roomTypesSelect');
    roomSelect.innerHTML = '';
    if(roomTypes){
        roomTypes.forEach(ele => {
            roomSelect.innerHTML += `<option value="${ele.roomTypeId}">${ele.type}</option>`;
        });
    }else{
        alert("Add roomTypes first to add the room!");
        return;
    }
    const addRoomModal = new bootstrap.Modal(document.getElementById('addRoomModal'));
    localStorage.setItem('currentHotel', id);
    addRoomModal.show();
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
        console.log(data);
        alert("data updated successfully");
        document.querySelector('[data-bs-dismiss="modal"]').click();
        //update the hotel itself,
        loadHotels();
    }).catch(error => {
        alert(error);
        console.error(error);
    });
}

function addRoom(){
    var roomData = {
        hotelId : localStorage.getItem('currentHotel'),
        typeId : document.getElementById('roomTypesSelect').value,
        images : document.getElementById('roomImages').value
    };
    console.log(roomData);
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
        return await res.json();
    }).then(data => {
        console.log(data);
        alert("room added successfully");
        document.querySelector('[data-bs-dismiss="modal"]').click();
    }).catch(error => {
        alert(error);
        console.error(error);
    });
}

document.addEventListener('DOMContentLoaded', function() {
    loadHotels();
})
