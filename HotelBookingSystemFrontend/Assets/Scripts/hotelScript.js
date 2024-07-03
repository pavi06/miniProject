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
    var locationInput = document.hotelForm.location.value;
    var checkInDateInput = document.hotelForm.checkInDate.value;
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
                if(data.length === 0){
                    document.getElementById("hotelsDisplay").innerHTML = '<div class="m-auto p-3 ring-1 ring-orange-400 uppercase fw-bolder text-2xl text-center"><span style="color:#FFA456">No hotels</span> available!</div>';
                }
                else{
                    displayHotelsRetrieved(data);
                }
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
    if(groupId === '#ratings'){
        fetchHotelsByRatings();
        return;
    }
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
    if(id === 'ratings'){
        return;
    }
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
            if(data.length === 0){
                document.getElementById("hotelsDisplay").innerHTML = '<div class="m-auto p-3 ring-1 ring-orange-400 uppercase fw-bolder text-2xl text-center"><span style="color:#FFA456">No hotels</span> available!</div>';
                return;
            }
            displayHotelsRetrieved(data);
    }).catch(error => {
            alert(error);
            console.error(error);
    });
}

// ----------------------------------------

document.addEventListener('DOMContentLoaded', function() {
    checkUserLoggedInOrNot();
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
    localStorage.setItem('currentHotel', id);
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

var fetchHotelsByRatings = () => {
    fetch('http://localhost:5058/api/GuestBooking/GetHotelsByRating', {
        method: 'GET',
        headers: {'Content-Type':'application/json'},
    }).then(async(res) => {
            if (!res.ok) {
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            return await res.json();
    }).then(data => {
            if(data.length === 0){
                document.getElementById("hotelsDisplay").innerHTML = '<div class="m-auto p-3 ring-1 ring-orange-400 uppercase fw-bolder text-2xl text-center"><span style="color:#FFA456">No hotels</span> available!</div>';
                return;
            }
            displayHotelsRetrieved(data);
    }).catch(error => {
            alert(error);
            console.error(error);
    });
}

var displayRecommandation = (data) => {
    const recommModal = new bootstrap.Modal(document.getElementById('recommandationModal'));
    var recommHtml = "";
    data.forEach(recomm => {
        recommHtml +=`
        <div class="flex flex-row justify-between m-4" style="box-shadow: rgba(99, 99, 99, 0.2) 0px 2px 8px 0px;">
            <div class="flex flex-column justify-start fw-bolder text-xl text-left m-3">
                <div class="uppercase text-center" style="color:#FFA456"><span>${recomm.hotelName}</span></div>
                <div>Location : <span>${recomm.address}</span></div>
                <div>RoomType : <span style="color:#FFA456">${recomm.roomType}</span></div>
                <div>Discount Percent : <span style="color:#00A36C">${recomm.discountPercent}%</span></div>
            </div>
            <div class="text-xl fw-bolder uppercase mr-3" style="color:#FFA456">${recomm.city}</div>
        </div>
        `;
    document.getElementById('displayRecommandation').innerHTML = recommHtml;
    recommModal.show();
});
}


var getRecommandation  = () =>{
    if(localStorage.getItem('isLoggedIn')){
        fetch('http://localhost:5058/api/GuestBooking/GetRecommandation', {
            method: 'GET',
            headers: {
                'Content-Type':'application/json',
                'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`
            }
        }).then(async(res) => {
                if (!res.ok) {
                    const errorResponse = await res.json();
                    throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
                }
                return await res.json();
        }).then(data => {
                console.log(data);
                if(data.length === 0){
                    document.getElementById("displayRecommandation").innerHTML = '<div class="m-auto p-3 ring-1 ring-orange-400 uppercase fw-bolder text-2xl text-center"><span style="color:#FFA456">No Recommandation!</span></div>';
                    return;
                }
                displayRecommandation(data);
        }).catch(error => {
                alert(error);
                console.error(error);
        });
    }else{
        alert("SignUp/Login to get recommandations for you!");
    }
}