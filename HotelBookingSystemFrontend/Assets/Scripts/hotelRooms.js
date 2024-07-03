var facilities = {'Wifi':'Stay connected with free, high-speed WiFi access.', 'Spa':'Relax and unwind with complimentary spa amenities.',
    'Gym':'Stay active at no extra cost in our state-of-the-art gym.','Scenic View':'Take in stunning vistas with picturesque views.',
    'AC':'Enjoy cool comfort with complimentary air conditioning.','Mini Restaurant':'Savor diverse cuisines at our complimentary dining spots.',
    'Parking' : 'pppppppp','Laundry service' : 'lllllllll', 'TV' : 'tvvvvvvvvvvv','Free breakfast' : 'bffffffff'
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
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
              }
              return res.json();
            });
    
            return Promise.all(dataPromises);
        }).then(dataArray => {
            const [data1, data2, data3] = dataArray; // Destructure the array of responses
            displayBasicHotelDetails(data1);
            displayRoomDetails(data2)
            displayAmenitiesInfo(data1.amenities);
            displayRestrictionsInfo(data1.restrictions)
            displayTestimonials(data3);
          }).catch(error => {
            alert(error);
            console.error(error);
       });
});

var displayBasicHotelDetails = (data) =>{
    document.getElementById('displayHotelDetail').innerHTML = `
        <div class="flex flex-column">
            <div class="flex flex-row justify-between">
                <div>
                    <p class="heading">${data.name}</p>
                    <p class="address"><a href="#"><i class="bi bi-geo-alt-fill"></i>&nbsp;&nbsp;${data.address}</a></p>
                </div>
                <div>
                    ${data.ratingCount}
                    <br>
                    <p style="font-size: 15px; font-weight: bolder;">${data.rating}/5 Star Ratings</p>
                </div>
            </div>
            <div class="grid h-80 w-100 my-10 grid-cols-5 grid-rows-4 gap-2">
                <div class="col-span-2 row-span-4 rounded-xl bg-indigo-200"><img src="../Assets/Images/hotelImage1.jpg" class="images"/></div>
                <div class="col-span-2 row-span-2 rounded-xl bg-indigo-200"><img src="../Assets/Images/hotelImage2.jpg" class="images"/></div>
                <div class="col-span-1 row-span-2 rounded-xl bg-indigo-200"><img src="../Assets/Images/hotelImage3.jpg" class="images"/></div>
                <div class="col-span-1 row-span-2 rounded-xl bg-indigo-200"><img src="../Assets/Images/hotelImage4.jpg" class="images"/></div>
                <div class="col-span-1 row-span-2 rounded-xl bg-indigo-200"><img src="../Assets/Images/hotelImage5.jpg" class="images"/></div>
                <div class="col-span-1 row-span-2 rounded-xl bg-indigo-200"><img src="../Assets/Images/hotelBg.jpg" class="images"/></div>
            </div>
        </div>    `;
}

var displayRoomDetails = (data) =>{
    document.getElementById('displayRoomTypes').innerHTML = "";
    data.forEach(roomType => {
        var discountAmount = (roomType.discount/100)*roomType.amount;
        var finalAmount = roomType.amount - discountAmount;
        var roomTypeHtml =`
            <div  class=" w-75 h-auto px-5 py-3 mb-5 flex flex-row justify-between shadow-lg" style="border-radius:20px;">
                <div>
                    <button type="button" data-bs-toggle="modal" data-bs-target="#roomTypeDetailsModal" class="subHeading">${roomType.roomType}</button>
                    <p class="roomCount text-red-600 font-bold"><span class="bg-red-200">Only ${roomType.noOfRoomsAvailable} rooms left</span></p>
                    <p>Amenities : ${roomType.amenities}</p>
                </div>
                <div class="px-3">
                    <p class="subHeading">Guests</p>
                    ${roomType.occupancy}
                    <!-- <i class="bi bi-person-fill icon"></i><i class="bi bi-person-fill icon"></i> -->
                </div>
                <div class="mx-5">
                    <p class="subHeading">Price</p>
                    <p>&#x20B9;${roomType.amount} per night</p>
                    <p><span style="text-decoration: line-through; color: red; font-size: 28px;line-height: 20px;">&#x20B9;${roomType.amount}</span><br><span style="color: green; font-size: 20px; font-weight: bolder; line-height: 18px;">&#x20B9;-${discountAmount}</span><br>&#x20B9;${finalAmount} per night</p>
                    <button type="button" class="mt-3 buttonStyle addToCartBtn" data-roomtype-id="${roomType.roomTypeId}"><span>Book now</span></button>
                </div>
            </div>
        `;
        var dataDiv = document.createElement('div'); 
        dataDiv.innerHTML = roomTypeHtml;
        document.getElementById('displayRoomTypes').appendChild(dataDiv);
    }); 
    displayRoomTypes.querySelectorAll('.addToCartBtn').forEach(button => {
        var roomTypeId = button.getAttribute('data-roomtype-id');
        var roomType = data.find(rt => rt.roomTypeId === parseInt(roomTypeId));
        if (roomType.noOfRoomsAvailable === 0) {
            button.disabled = true;
            button.style.cursor = 'auto';
        }
        button.addEventListener('click', function(event) {
            if(!localStorage.getItem('isLoggedIn')){
                localStorage.setItem('redirectUrl', window.location.pathname)
                alert("Oops... Login to book rooms now!");
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
    console.log("displaying amenities")
    var amenitiesHTML = "";
    let amenitiesList = data.split(',').map(item => item.trim());
    console.log(amenitiesList)
    amenitiesList.forEach(amenity =>{
        amenitiesHTML+=`
            <div class="facilityDiv">
                <div class="content">
                    <p class="p-3 leading-8 text-justify"><q>${facilities[amenity]}</q></p>
                </div> 
                <div class="text-xl font-bold text-center bg-orange-300 banner"><p>${amenity}</p></div>      
            </div>
        `;
    })
    console.log(document.getElementById('displayAmenities'));
    document.getElementById('displayAmenities').innerHTML = amenitiesHTML;
}

var displayRestrictionsInfo = (data) => {
    console.log(data)
    console.log("displaying restrictions")
    var restrictionsHtml ="";
    let restrictionsList = data.split(',').map(item => item.trim());
    restrictionsList.forEach(restriction =>{
        restrictionsHtml+=`
            <span class="inline-flex items-center rounded-md  px-2 py-3 mx-3 my-5 text-xl font-bold text-base ring-1 ring-inset ring-orange-400">${restriction}</span>
        `;
    })
    console.log(document.getElementById('restrictions'));
    document.getElementById('restrictions').innerHTML = restrictionsHtml;
}

var displayTestimonials = (data) =>{
    console.log(data)
    console.log("displaying testimonials")
    var testimonialsHtml = "";
    data.forEach(review => {
        testimonialsHtml+=`
            <div class="card">
                    <i class="ribbon">
                        ${review.ratingPoints}
                    </i>
                    <div class="cardContent">
                      <p class="cardBody">
                        <q>${review.feedback}</q>
                      </p> 
                        <h4 style="margin-top: 5%;font-weight: bolder;color: #FFA456;">@${review.guestName}</h4> 
                       <h4 style="margin-top: 5%;font-weight: bolder;color: #FFA456;">${review.ratingProvidedDate}</h4>  
                    </div>
            </div>
        `;
    });
    console.log(document.getElementById('displayTestimonials'));
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
        console.log("inside if")
        roomtypeExitsOrNot.quantity++;
        if(roomtypeExitsOrNot.noOfRoomsAvailable<roomtypeExitsOrNot.quantity){
            alert("No more rooms available!");
        }
        roomtypeExitsOrNot.quantity--;
    } else {
        console.log("inside else")
        cartItems.push({ ...roomType, quantity: 1 });
        alert("Item added successfully!")
        document.querySelector('.bookRooms').classList.add('show');
    }
    localStorage.setItem('cartItems', JSON.stringify(cartItems));
}

var validateAndGet = () =>{
    var checkIn = new Date(document.getElementById('checkInDate').value); 
    var checkOut = new Date(document.getElementById('checkOutDate').value);
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
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    }).then(data => {
        displayRoomDetails(data);
    }).catch(error => {
        alert(error);
        console.error(error);
   });
    
}