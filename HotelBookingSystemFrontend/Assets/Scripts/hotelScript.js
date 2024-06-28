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

var validateAndGet = () => {
    console.log("Inside validate")
    var locationInput = document.hotelForm.location.value;
    console.log("Hotel value")
    console.log(locationInput)
    var checkInDateInput = document.hotelForm.checkInDate.value;
    console.log("Date value")
    console.log(checkInDateInput)
    var hotelDetail = {
        location:locationInput,
        date:checkInDateInput
    }
    if(validateLocation() && validateDate()){
        console.log("Inside api");
        fetch('http://localhost:5058/api/GuestBooking/GetHotelsByLocationAndDate', {
            method: 'POST',
            headers: {'Content-Type' : 'application/json'},
            body:JSON.stringify(hotelDetail)
            }).then((res) => {
                if (!res.ok) {
                    const errorResponse = res.json();
                    throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
                }
                return res.json();
            }).then(data => {
                console.log(data);
                displayHotelsRetrieved(data);
            }).catch(error => {
                alert(error);
                console.error(error);
        });
    }
    else{
        alert("Provide values properly!");
    }
}

// --------------------HOTELS BY FEATURES--------------------------------
//method to toggle the dropdown used with filters ui
var toggleDropdown = (id) => {
    var dropdown = document.getElementById(id);
    dropdown.classList.toggle('active');
}

var getCheckedCheckboxes = (groupId) => {
    var checkedBoxes = [];
    var groupCheckboxes = document.querySelectorAll(groupId + '[type="checkbox"]');
    groupCheckboxes.forEach(function(cb) {
      if (cb.checked) {
        checkedBoxes.push(cb.value);
      }
    });
    return checkedBoxes;
}

var refreshCheckBoxValues = (id)=>{
    checkedValues = getCheckedCheckboxes(`#${id}`);
    fetchDataFromServer(checkedValues);
}

// get hotels by features
var fetchDataFromServer = (checkedValues) => {
    fetch('http://localhost:5058/api/GuestBooking/GetHotelsByFeatures', {
        method: 'POST',
        headers: {'Content-Type':'application/json'},
        body:JSON.stringify(checkedValues),
    }).then(async(res) => {
            if (!res.ok) {
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            return await res.json();
    }).then(data => {
            console.log(data);
            displayHotelsRetrieved(data);
    }).catch(error => {
            alert(error);
            console.error(error);
    });
}

// ----------------------------------------

document.addEventListener('DOMContentLoaded', function() {
    fetch('http://localhost:5058/api/AdminHotel/GetAllHotels', {
        method: 'GET',
        }).then(async(res) => {
            if (!res.ok) {
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            return await res.json();
        }).then(data => {
            displayHotelsRetrieved(data);
        }).catch(error => {
            alert(error);
            console.error(error);
       });
});


// -------HOTEL REDIRECTION---------
var redirectToRooms = (id) => {
    // get hotel
    localStorage.setItem('currentHotelId', id);
    window.location.href="./hotelRooms.html";
}


// displayHotelstemplate
var displayHotelsRetrieved = (data) => {
    var hotelsList ="";
    data.forEach(hotel => {
        hotelsList+=`
            <div class="px-3 pb-5 mb-10 h-auto cardDesign">
                <div class="flex flex-row justify-between">                      
                    <div class="w-60 h-50 mt-4" style="object-fit: cover;">
                        <img src="../Assets/Images/hotelImage5.jpg" alt="HotelImage"/>
                    </div>
                    <div class="p-3">
                        <p class="hotelName">${hotel.name}</p>
                        <a href="#"><i class="bi bi-geo-alt-fill"></i>&nbsp;&nbsp;${hotel.address}</a>
                        <p class="description">Amenities:${hotel.amenities}  <br> Restriction : ${hotel.restrictions}</p>
                    </div>
                    <div class="flex flex-col justify-between mt-3" style="float:right">
                        <div>
                            <p><span class="review">Review Score</span></p>
                            <div>
                                <span class="fa fa-star checked"></span>
                                <span class="fa fa-star checked"></span>
                                <span class="fa fa-star checked"></span>
                                <span class="fa fa-star checked"></span>
                                <span class="fa fa-star"></span>
                            </div>
                            <p><a href="#">10 reviews</a></p>
                        </div>
                        <button type="button" class="buttonStyle" style="width:80%; padding:5px" id="availabilityBtn" onclick="redirectToRooms(${hotel.hotelId})"><span>Check Room Availability</span></button>
                    </div>
                </div>
            </div> 
        `;
    });
    document.getElementById("hotelsDisplay").innerHTML = hotelsList;
}