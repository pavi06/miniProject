var currentUrl = "";
var page = 1;
const itemsperpage = 5;
var bodyData;

var validateAndGet = () => {
    var locationInput = document.hotelForm.location.value;
    var checkInDateInput = document.hotelForm.checkInDate.value;
    var hotelDetail = {
        location:locationInput,
        date:checkInDateInput
    }
    bodyData = hotelDetail;
    if(validateLocation() && validateDate()){
        page=1;
        currentUrl = 'http://localhost:5058/api/GuestBooking/GetHotelsByLocationAndDate';
        document.getElementById("hotelsDisplay").innerHTML = "";
        getPaginatedDataWithBody(currentUrl,hotelDetail)
    }
    else{
        addAlert("Provide values properly! Make sure that every field is provided.");
        document.getElementById('alertModal').classList.add("active")
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
    bodyData=checkedBoxes;
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
    page=1;
    currentUrl = 'http://localhost:5058/api/GuestBooking/GetHotelsByFeatures';
    document.getElementById("hotelsDisplay").innerHTML = "";
    getPaginatedDataWithBody(currentUrl,checkedValues)
}

// ----------------------------------------
document.addEventListener('DOMContentLoaded', function() {
    checkUserLoggedInOrNot();
    page=1;
    currentUrl = 'http://localhost:5058/api/AdminHotel/GetAllHotels';
    document.getElementById("hotelsDisplay").innerHTML = "";
    getPaginatedDataWithoutBody(currentUrl)
});

// -------HOTEL REDIRECTION---------
var redirectToRooms = (id) => {
    // get hotel
    localStorage.setItem('currentHotel', id);
    window.location.href="./hotelRooms.html";
}

// displayHotelstemplate
var displayHotelsRetrieved = (data, boolValue) => {
    console.log(data)
    if(data.length === 0 && document.getElementById('hotelsDisplay').childNodes.length === 0){
        if( document.getElementById('loadBtn')){
            document.getElementById('loadBtn').classList.add('hide');
        }
        if(document.getElementById('loadBtnWithBody')){
            document.getElementById('loadBtnWithBody').classList.add('hide');
        }
        document.getElementById("hotelsDisplay").innerHTML = '<div class="m-auto p-3 ring-1 ring-orange-400 uppercase fw-bolder text-2xl text-center"><span style="color:#FFA456">No hotels</span> available!</div>';
        return;
    }
    else if(data.length === 0 && document.getElementById('hotelsDisplay').childNodes.length >0){
        addAlert("No more hotels available!");
    }
    var hotelsList = data.map((hotel) => {
        if(boolValue){
            document.getElementById('loadBtnWithBody').classList.remove('hide');
            document.getElementById('loadBtn').classList.add('hide');
        }
        else{
            document.getElementById('loadBtn').classList.remove('hide');
            document.getElementById('loadBtnWithBody').classList.add('hide');
        } 
        var starRatingHtml = ""; 
        for (let i = 0; i < hotel.rating; i++) {
            starRatingHtml+=`<span class="fa fa-star checked fa-lg p-0.5"></span>`;
        }
        var buttonHtml="";
        if(hotel.isAvailable){
            buttonHtml = ` <button type="button" class="buttonStyle" style="width:80%; padding:5px" id="availabilityBtn" onclick="redirectToRooms(${hotel.hotelId})"><span>Check Room Availability</span></button>`;
        }else{
            buttonHtml=`<button type="button" class="btn ring-1 ring-red-400 text-red-500" style="width:80%; padding:5px; cursor:auto;" id="availabilityBtn"><span>Not Availability right now!</span></button>`;
        }
        return `
        <div class="px-3 pb-5 mb-10 h-auto cardDesign">
              <div class="flex justify-between cardInside">                      
                  <div class="w-60 h-50 mt-4" style="object-fit: cover;">
                      <img src="https://drive.google.com/thumbnail?id=1GPAgDD_PBEnX0NrdGxyR6ZrOcbGpL3Lr" alt="HotelImage"/>
                  </div>
                  <div class="p-3">
                      <p class="hotelName">${hotel.name}</p>
                      <a href="#"><i class="bi bi-geo-alt-fill"></i>&nbsp;&nbsp;${hotel.address}</a>
                      <p class="description">Amenities:${hotel.amenities}  <br> Restriction : ${hotel.restrictions}</p>
                  </div>
                  <div class="flex flex-col justify-between mt-3" style="float:right">
                      <div class="reviewDiv">
                          <p><span class="review">Review Score</span></p>
                          <div>
                            ${starRatingHtml}
                          </div>
                          <p class="text-center fw-bolder"><a href="#">${hotel.ratingCount} reviews</a></p>
                      </div>
                     ${buttonHtml}
                  </div>
              </div>
        </div> 
    `}).join('');
    document.getElementById("hotelsDisplay").innerHTML += hotelsList;
}

var fetchHotelsByRatings = () => {
    page=1;
    currentUrl = 'http://localhost:5058/api/GuestBooking/GetHotelsByRating';
    document.getElementById("hotelsDisplay").innerHTML = "";
    getPaginatedDataWithoutBody(currentUrl)
}

//-------------Recommandation------------------
var displayRecommandation = (data) => {
    console.log(data)
    const recommModal = new bootstrap.Modal(document.getElementById('recommandationModal'));
    var recommHtml = data.map((recomm) =>`
        <div class="flex flex-row justify-between m-4" style="box-shadow: rgba(99, 99, 99, 0.2) 0px 2px 8px 0px;">
            <div class="flex flex-column justify-start fw-bolder text-xl text-left m-3">
                <div class="uppercase text-center" style="color:#FFA456"><span><a onclick="redirectToRooms(${recomm.hotelId})" style="cursor:pointer;">${recomm.hotelName}</a></span></div>
                <div>Location : <span>${recomm.address}</span></div>
                <div>RoomType : <span style="color:#FFA456">${recomm.roomType}</span></div>
                <div>Discount Percent : <span style="color:#00A36C">${recomm.discountPercent}%</span></div>
            </div>
            <div class="text-xl fw-bolder uppercase mr-3" style="color:#FFA456">${recomm.city}</div>
        </div>
    `).join('');
    document.getElementById('displayRecommandation').innerHTML = recommHtml;
    recommModal.show();
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
                if(data.length === 0){
                    document.getElementById("displayRecommandation").innerHTML = '<div class="m-auto p-3 ring-1 ring-orange-400 uppercase fw-bolder text-2xl text-center"><span style="color:#FFA456">No Recommandation!</span></div>';
                    return;
                }
                displayRecommandation(data);
        }).catch(error => {
            addAlert(error);
        });
    }else{
        addAlert("SignUp/Login to get recommandations for you!");
    }
}

//-------------------Pagination-------------------
// getting paginated data
var getPaginatedDataWithoutBody = (url) =>{
    const skip =  (page - 1) * itemsperpage;
    fetch(`${url}?limit=${itemsperpage}&skip=${skip}`, {
        method: 'GET',
        headers: {'Content-Type':'application/json'},
    }).then(async(res) => {
            if (!res.ok) {
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            return await res.json();
    }).then(data => {
        displayHotelsRetrieved(data,false);
    }).catch(error => {
        addAlert(error.message);
    });
}

var getPaginatedDataWithBody = (url,bodyData) =>{
    const skip =  (page - 1) * itemsperpage;
    console.log(bodyData)
    fetch(`${url}?limit=${itemsperpage}&skip=${skip}`, {
        method: 'POST',
        headers: {'Content-Type':'application/json'},
        body:JSON.stringify(bodyData),
    }).then(async(res) => {
            if (!res.ok) {
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            return await res.json();
    }).then(data => {
        console.log(data)
        displayHotelsRetrieved(data, true);
    }).catch(error => {
        console.log(error);
        addAlert(error.message);
    });

}

var loadMoreDataForWithoutBody = () =>{
    page++;
    getPaginatedDataWithoutBody(currentUrl);
}

var loadMoreDataForWithBody = () =>{
    console.log("--inside--")
    page++;
    getPaginatedDataWithBody(currentUrl,bodyData);
}
//---------------------------------------------

