var facilities = {'Wifi':'Stay connected with free, high-speed WiFi access.', 'Spa':'Relax and unwind with complimentary spa amenities.',
    'Gym':'Stay active at no extra cost in our state-of-the-art gym.','Scenic View':'Take in stunning vistas with picturesque views.',
    'AC':'Enjoy cool comfort with complimentary air conditioning.','Mini Restaurant':'Savor diverse cuisines at our complimentary dining spots.',
    'Parking' : 'Convenient access to secure parking facilities, typically located on-site or nearby, ensuring ease and safety for guests vehicles.',
    'Laundry service' : 'Conveniently handle your clothing needs with on-site or outsourced laundering options at the hotel.', 'TV' : 'Enjoy entertainment and relaxation with television access in guest rooms.',
    'Free breakfast' : 'Enjoy a complimentary morning meal provided by the hotel, enhancing convenience and value during your stay.'
}
var currentHotel = parseInt(localStorage.getItem('currentHotel'))

const fetch1 = fetch('http://localhost:5058/api/AdminHotel/GetHotel', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body:JSON.stringify(localStorage.getItem('currentHotel'))
});

const currentDate = new Date();
const checkInDate = `${currentDate.getFullYear()}-${(currentDate.getMonth() + 1).toString().padStart(2, '0')}-${currentDate.getDate().toString().padStart(2, '0')}`;
currentDate.setDate(currentDate.getDate() + 1);
const checkOutDate = `${currentDate.getFullYear()}-${(currentDate.getMonth() + 1).toString().padStart(2, '0')}-${currentDate.getDate().toString().padStart(2, '0')}`;
var bodyData = {
    hotelId : currentHotel,
    checkInDate: checkInDate,
    checkoutDate: checkOutDate
}

const fetch2 = fetch('http://localhost:5058/api/GuestBooking/GetRoomsByHotel', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json'},
    body:JSON.stringify(bodyData)
});

const fetch3 = fetch('http://localhost:5058/api/AdminHotel/GetAllRatingsForHotel', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body:JSON.stringify(currentHotel)
});

document.addEventListener('DOMContentLoaded', function() {
        checkUserLoggedInOrNot();
        Promise.all([fetch1, fetch2, fetch3])
        .then(async responses => {
            const dataPromises = responses.map(async res => {
              if (!res.ok) {
                if (res.status === 401) {
                    throw new Error('Unauthorized Access!');
                }
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
              }
              return res.json();
            });    
            return Promise.all(dataPromises);
        }).then(dataArray => {
            const [data1, data2, data3] = dataArray; // Destructure the array of responses
            displayBasicHotelDetails(data1, data2);
            imageLoad();
            displayRoomDetails(data2)
            displayAmenitiesInfo(data1.amenities);
            displayRestrictionsInfo(data1.restrictions)
            displayTestimonials(data3);
          }).catch(error => {
            if (error.message === 'Unauthorized Access!') {
                addAlert("Unauthorized Access!");
            } else {
                addAlert(error.message);
            }
       });
});


// ------------display hotel details----------------
var displayBasicHotelDetails = (data, imagesData) =>{
    var imagesList = imagesData[0].images.split(',')
    var starRatingHtml = ""; 
    for (let i = 0; i < data.rating; i++) {
        starRatingHtml+=`<span class="fa fa-star checked p-0.5"></span>`;
    }
    document.getElementById('displayHotelDetail').innerHTML = `
        <div class="flex flex-column">
            <div class="flex flex-row justify-between">
                <div>
                    <p class="heading">${data.name}</p>
                    <p class="address"><a href="#"><i class="bi bi-geo-alt-fill"></i>&nbsp;&nbsp;${data.address}</a></p>
                </div>
                <div>
                    ${starRatingHtml}
                    <br>
                    <p style="font-size: 15px; font-weight: bolder;">${data.rating}/5 Star Ratings</p>
                </div>
            </div>
            <div class="grid h-80 w-100 my-10 grid-cols-5 grid-rows-4 gap-2">
                <div class="col-span-2 row-span-4 rounded-xl bg-indigo-200 blurLoad" style="background-image:url(../Assets/Images/hotelImage1Small.jpg)"><img src="${imagesList[0]}" class="images" loading="lazy"/></div>
                <div class="col-span-2 row-span-2 rounded-xl bg-indigo-200 blurLoad" style="background-image:url(../Assets/Images/hotelImage2Small.jpg)"><img src="${imagesList[2]}" class="images" loading="lazy"/></div>
                <div class="col-span-1 row-span-2 rounded-xl bg-indigo-200 blurLoad" style="background-image:url(../Assets/Images/hotelImage3Small.jpg)"><img src="${imagesList[2]}" class="images" loading="lazy"/></div>
                <div class="col-span-1 row-span-2 rounded-xl bg-indigo-200 blurLoad" style="background-image:url(../Assets/Images/hotelImage4Small.jpg)"><img src="${imagesList[3]}" class="images" loading="lazy"/></div>
                <div class="col-span-1 row-span-2 rounded-xl bg-indigo-200 blurLoad" style="background-image:url(../Assets/Images/hotelImage5Small.jpg)"><img src="${imagesList[4]}" class="images" loading="lazy"/></div>
                <div class="col-span-1 row-span-2 rounded-xl bg-indigo-200 blurLoad" style="background-image:url(../Assets/Images/hotelBgSmall.jpg)"><img src="${imagesList[0]}" class="images" loading="lazy"/></div>
            </div>
        </div>    `;
}

var displayRoomDetails = (data) =>{
    document.getElementById('displayRoomTypes').innerHTML = "";
    data.forEach(roomType => {
        var discountAmount = (roomType.discount/100)*roomType.amount;
        var finalAmount = roomType.amount - discountAmount;
        var roomsAvailableHtml = ""
        if(roomType.noOfRoomsAvailable === 0){
            roomsAvailableHtml = ` <p class="roomCount text-red font-bold roomsCount"><span class="bg-red-200">No Rooms Available!</span></p>`;
        }else{
            if(roomType.noOfRoomsAvailable <=5){
                var classColor = "bg-red-200";
            }
            else{
                var classColor = "bg-green-200";
            }
            roomsAvailableHtml = ` <p class="roomCount font-bold roomsCount"><span class="${classColor}">Only ${roomType.noOfRoomsAvailable} rooms left</span></p>`;
        }
        var personIcon = "";
        for (let i = 0; i < roomType.occupancy; i++) {
            personIcon+=`<i class="bi bi-person-fill icon"></i>`;
        }
        var roomTypeHtml =`
            <div  class=" w-75 h-auto px-5 py-3 mb-5 flex justify-between shadow-lg mx-auto roomTypes" style="border-radius:20px;">
                <div>
                    <button type="button" onclick="openRoomTypeModalForDetails('${roomType.images}','${roomType.amenities}')" class="subHeading">${roomType.roomType}</button>
                    ${roomsAvailableHtml}
                    <p>Amenities : ${roomType.amenities}</p>
                </div>
                <div class="px-3">
                    <p class="subHeading">Guests</p>
                    ${personIcon}
                    <!-- <i class="bi bi-person-fill icon"></i><i class="bi bi-person-fill icon"></i> -->
                </div>
                <div class="mx-5">
                    <p class="subHeading">Price</p>
                    <p><span style="text-decoration: line-through; color: red; font-size: 28px;line-height: 20px;">&#x20B9;${roomType.amount}</span><br><span style="color: green; font-size: 20px; font-weight: bolder; line-height: 18px;">- &#x20B9;${discountAmount}</span><br>&#x20B9;${finalAmount} per night</p>
                    <button type="button" class="mt-3 buttonStyle addToCartBtn" data-roomtype-id="${roomType.roomTypeId}"><span>Book now</span></button>
                </div>
            </div>
        `;
        var dataDiv = document.createElement('div'); 
        dataDiv.style.alignItems='center';
        dataDiv.innerHTML = roomTypeHtml;
        document.getElementById('displayRoomTypes').appendChild(dataDiv);
    }); 
    displayRoomTypes.querySelectorAll('.addToCartBtn').forEach(button => {
        var roomTypeId = button.getAttribute('data-roomtype-id');
        var roomType = data.find(rt => rt.roomTypeId === parseInt(roomTypeId));
        if (roomType.noOfRoomsAvailable === 0) {
            button.disabled = true;
            button.style.cursor = 'auto';
            button.classList.add('bg-red-400');
        }
        button.addEventListener('click', function(event) {
            if(!localStorage.getItem('isLoggedIn')){
                localStorage.setItem('redirectUrl', window.location.pathname)
                addAlert("Oops... Login to book rooms now!");
            }
            else{
                var clickedRoomTypeId = event.currentTarget.getAttribute('data-roomtype-id');
                var roomTypeToAdd = data.find(rt => rt.roomTypeId === parseInt(clickedRoomTypeId));
                addToBookingCart(roomTypeToAdd);
            }            
        });
    });      
}

var displayAmenitiesInfo = (data) =>{
    var amenitiesHTML = "";
    let amenitiesList = data.split(',').map(item => item.trim());
    amenitiesList.forEach(amenity =>{
        amenitiesHTML+=`
            <div class="facilityDiv text-wrap">
                <div class="content">
                    <p class="p-3 leading-8 text-justify text-wrap"><q>${facilities[amenity]}</q></p>
                </div> 
                <div class="text-xl font-bold text-center bg-orange-300 banner"><p>${amenity}</p></div>      
            </div>
        `;
    })
    document.getElementById('displayAmenities').innerHTML = amenitiesHTML;
}

var displayRestrictionsInfo = (data) => {
    var restrictionsHtml ="";
    let restrictionsList = data.split(',').map(item => item.trim());
    restrictionsList.forEach(restriction =>{
        restrictionsHtml+=`
            <span class="inline-flex items-center rounded-md  px-2 py-3 mx-3 my-5 text-xl font-bold text-base ring-1 ring-inset ring-orange-400">${restriction}</span>
        `;
    })
    document.getElementById('restrictions').innerHTML = restrictionsHtml;
}

var displayTestimonials = (data) =>{
    var testimonialsHtml = "";
    data.forEach(review => {
        var starRatingHtml = ""; 
        for (let i = 0; i < review.ratingPoints; i++) {
            starRatingHtml+=`<span class="fa fa-star checked p-0.5"></span>`;
        }
        testimonialsHtml+=`
            <div class="card">
                    <i class="ribbon">
                        ${starRatingHtml}
                    </i>
                    <div class="cardContent">
                      <p class="cardBody">
                        <q>${review.feedback}</q>
                      </p> 
                        <h4 style="margin-top: 5%;font-weight: bolder;color: #FFA456;">@${review.guestName}, ${review.ratingProvidedDate.substring(0, 10)}</h4>  
                    </div>
            </div>
        `;
    });
    document.getElementById('displayTestimonials').innerHTML=testimonialsHtml;
}

var addToBookingCart = (roomType) =>{
    var cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
    if(cartItems.length === 0){
        document.querySelector('.bookRooms').classList.add('hide');
    } 
    else{
        document.querySelector('.bookRooms').classList.add('show');
    }
    const roomtypeExitsOrNot = cartItems.find(item => item.roomTypeId === roomType.roomTypeId);
    if (roomtypeExitsOrNot) {
        roomtypeExitsOrNot.quantity++;
        if(roomtypeExitsOrNot.noOfRoomsAvailable<roomtypeExitsOrNot.quantity){
            addAlert("No more rooms available!");
        }
        roomtypeExitsOrNot.quantity--;
    } else {
        cartItems.push({ ...roomType, quantity: 1 });
        addSuccessAlert('Room added successfully!')
        document.querySelector('.bookRooms').classList.add('show');
    }
    localStorage.setItem('cartItems', JSON.stringify(cartItems));
}
// -----------------------------------------------------


var validateAndGetRooms = () =>{
    var checkIn = new Date(document.dateForm.checkInDate.value); 
    var checkOut = new Date(document.dateForm.checkOutDate.value);
    if(validateDate('checkInDate') && validateDate('checkOutDate')){        
        const checkInDate = `${checkIn.getFullYear()}-${(checkIn.getMonth() + 1).toString().padStart(2, '0')}-${checkIn.getDate().toString().padStart(2, '0')}`;
        const checkOutDate = `${checkOut.getFullYear()}-${(checkOut.getMonth() + 1).toString().padStart(2, '0')}-${checkOut.getDate().toString().padStart(2, '0')}`;
        var bodyData = {
            hotelId : currentHotel,
            checkInDate: checkInDate,
            checkoutDate: checkOutDate
        }
        fetch('http://localhost:5058/api/GuestBooking/GetRoomsByHotel', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json'},
            body:JSON.stringify(bodyData)
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
            displayRoomDetails(data);
        }).catch(error => {
            if (error.message === 'Unauthorized Access!') {
                addAlert("Unauthorized Access!");
            } else {
                addAlert(error.message);
            }
       }); 
    }
    else{
        addAlert("Provide checkIn and checkOut date properly!");
        return;
    }
   
}


var openRoomTypeModalForDetails = (images, facilities) =>{
 var imageList = images.split(',').slice(0, 4);
 var facilitiesList = facilities.split(',')
 const addRoomDetailsModal = new bootstrap.Modal(document.getElementById('roomTypeDetailsModal'));
 var imageDiv = document.getElementById('imagesGrid');
 var html="";
 imageList.forEach(image =>{
    html+=`
        <div class="col-span-2 rounded-xl bg-indigo-200 w-120 h-30" style="object-fit: cover;"><img src="${image}" class="images"/></div>
    `;
 })
 imageDiv.innerHTML=html;
 var facilitiesDiv = document.getElementById('facilitiesModalDiv');
 var facilitiesHtml="";
 facilitiesList.forEach(facility =>{
    facilitiesHtml+=`
        <div class="flex-none w-35 px-3 m-3 ring-1 ring-orange-400"><i class="bi bi-dot"></i>${facility}</div>
    `;
 })
 facilitiesDiv.innerHTML=facilitiesHtml;
 addRoomDetailsModal.show();
}


//-------------------image load--------
var imageLoad = () =>{
    const divs = document.querySelectorAll(".blurLoad")
    divs.forEach(div=>{
        const img = div.querySelector("img")
        function loaded(){
            div.classList.add('loaded')
        }
        if(img.complete){
            loaded()
        }else{
            img.addEventListener("load",loaded)
        }
    })
}

