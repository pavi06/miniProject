const hamBurger = document.querySelector(".toggle-btn");

// hamBurger.addEventListener("click", function () {
//   document.querySelector("#sidebar").classList.toggle("expand");
// });

var toggleDropdown = (id) => {
    var dropdown = document.getElementById(id);
    dropdown.classList.toggle('active');
}

var loadHotels = () => {
    fetch('http://localhost:5058/api/AdminHotel/GetAllHotels', {
        method: 'GET',
    }).then(async(res) => {
            if (!res.ok) {
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            return await res.json();
    }).then(data => {
            console.log(data);
            displayHotelsForAdmin(data);
    }).catch(error => {
            alert(error);
            console.error(error);
    });
}

loadHotels();


var displayHotelsForAdmin = (data) => {
    var hotelsList ="";
    data.forEach(hotel => {
        hotelsList+=`
            <div class="px-3 pb-5 mb-10 h-auto mx-auto cardDesign" style="width: 80%;">
                <div class="flex flex-row justify-between">                      
                    <div class="w-60 h-50 mt-4" style="object-fit: cover;">
                        <img src="./Assets/Images/hotelImage5.jpg" alt="Image gallery"/>
                    </div>
                    <div class="p-3">
                        <p class="hotelName">${hotel.name}</p>
                        <a href="#"><i class="bi bi-geo-alt-fill"></i>&nbsp;&nbsp;${hotel.address}</a>
                        <p class="description">Amenities:${hotel.amenities}  <br> Restriction : ${hotel.restrictions}<br>RoomTypes : ${hotel.roomTypes}<br>Total No. of Rooms Available : ${hotel.roomsCount}</p>
                    </div>
                    <div class="flex flex-col justify-between mt-3" style="float:right">
                        <div>
                            <p class="description"><span class="review">Review Score</span></p>
                            <p class="description" style="line-height: 10px;">${hotel.ratings}</p>
                            <p class="description"><a href="#">${hotel.reviews} reviews</a></p>
                        </div>
                    </div>
                </div>
                <div class="flex flex-row px-3 mt-3 justify-center">
                    <button type="button" class="buttonStyle mr-5" style="width:20%; padding:5px" id="editHotelBtn" onclick="openModalForEdit(${hotel.hotelId})"><span>Edit Hotel</span></button>
                    <button type="button" class="buttonStyle" style="width:20%; padding:5px" id="addRoomTypeBtn" onclick="routeRoomType(${hotel.hotelId})"><span>Add RoomType</span></button>
                    <button type="button" class="buttonStyle mr-5" style="width:20%; padding:5px" id="editHotelBtn" onclick="openModalForEdit(${hotel.hotelId})"><span>Add Room</span></button>
                </div>
            </div>
        `;
    });
    console.log(document.getElementById("displayAllHotels"));
    document.getElementById("displayAllHotels").innerHTML = hotelsList;
}

var routeRoomType = (id) =>{
    window.location.href="./AddRoomType.html";
}

function openModalForEdit(id){
    const editModal = new bootstrap.Modal(document.getElementById('editHotelModal'));
    localStorage.setItem('currentHotel', id);
    console.log(id);
    editModal.show();
}

function openModalForAddRoom(){
    const addRoomModal = new bootstrap.Modal(document.getElementById('addRoomModal'));
    addRoomModal.show();
}

function updateHotel(){
    var data = document.getElementById('updateHotelForm').querySelectorAll('input');
    console.log(localStorage.getItem('currentHotel'))
    console.log(data);
    var attributePairList = {};
    Array.from(data).forEach(ele => {
        console.log(ele.name)
        console.log(ele.value)
        if(ele.value){
            attributePairList[ele.name] = ele.value;
        }
    });
    var updatedHotel = {
        "hotelId" : localStorage.getItem('currentHotel'),
        "attributePairs" : attributePairList
    }
    console.log(updateHotel);
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
    }).catch(error => {
        alert(error);
        console.error(error);
    });
}
