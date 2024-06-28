var facilities = {'Wifi':'Stay connected with free, high-speed WiFi access.', 'Spa':'Relax and unwind with complimentary spa amenities.',
    'Gym':'Stay active at no extra cost in our state-of-the-art gym.','Scenic View':'Take in stunning vistas with picturesque views.',
    'AC':'Enjoy cool comfort with complimentary air conditioning.','Mini Restraurant':'Savor diverse cuisines at our complimentary dining spots.'
}


document.addEventListener('DOMContentLoaded', function() {
    fetch('http://localhost:5058/api/AdminHotel/GetHotel', {
        method: 'POST',
        headers:{
            'Content-Type':'application/json'
        },
        body:JSON.stringify(localStorage.getItem('currentHotelId'))
        }).then(async(res) => {
            if (!res.ok) {
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            return await res.json();
        }).then(data => {
            console.log(data);
            displayBasicHotelDetails(data);
            displayAmenitiesInfo(data.amenities);
            displayRestrictionsInfo(data.restrictions);
            displayTestimonials(data.ratings);
        }).catch(error => {
            alert(error);
            console.error(error);
       });
});

var displayBasicHotelDetails = (data) =>{
    var roomTypesAvailable = "";
    data.roomTypes.forEach(roomType => {
        var discountAmount = (roomType.discount/100)*roomType.amount;
        var finalAmount = roomType.amount - discountAmount;
        roomTypesAvailable+=`
            <div  class=" w-100 h-auto p-3 mb-5 flex flex-row shadow-lg">
                <div class="w-70">
                    <button type="button" data-bs-toggle="modal" data-bs-target="#roomTypeDetailsModal" class="subHeading">${roomType.type}</button>
                    <p class="roomCount text-red-600 font-bold"><span class="bg-red-200">Only ${roomType.noOfRoomsAvailable} rooms left</span></p>
                    <p>Amenities : ${roomType.amenities}</p>
                </div>
                <div class="w-60 px-3">
                    <p class="subHeading">Guests</p>
                    ${roomType.occupancy}
                    <!-- <i class="bi bi-person-fill icon"></i><i class="bi bi-person-fill icon"></i> -->
                </div>
                <div class="w-60">
                    <p class="subHeading">Price</p>
                    <p>&#x20B9;${roomType.amount} per night</p>
                    <p><span style="text-decoration: line-through; color: red; font-size: 28px;line-height: 20px;">&#x20B9;${roomType.amount}</span><br><span style="color: green; font-size: 20px; font-weight: bolder; line-height: 18px;">&#x20B9;-${discountAmount}</span><br>&#x20B9;${finalAmount} per night</p>
                    <button type="button" class="mt-3 buttonStyle">Book now</button>
                </div>
            </div>
        `;
    });

    document.getElementById('displayHotelDetail').innerHTML = `
        <div class="flex flex-row justify-between">
                <div>
                    <p class="heading">${data.name}</p>
                    <p class="address"><a href="#"><i class="bi bi-geo-alt-fill"></i>&nbsp;&nbsp;${data.address}</a></p>
                </div>
                <div>
                    <span class="fa fa-star checked star"></span>
                    <span class="fa fa-star checked star"></span>
                    <span class="fa fa-star checked star"></span>
                    <span class="fa fa-star checked star"></span>
                    <span class="fa fa-star star"></span>
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
            <div class="flex flex-col h-auto" id="displayRoomTypes">
            ${roomTypesAvailable}
            </div>
    `;
}


var displayAmenitiesInfo = (data) =>{
    var amenitiesHTML = "";
    let amenitiesList = data.split(',').map(item => item.trim());
    console.log(amenitiesList);
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
    document.getElementById('displayAmenities').innerHTML = amenitiesHTML;
}

var displayRestrictionsInfo = (data) => {
    var restrictionsHtml ="";
    let restrictionsList = data.split(',').map(item => item.trim());
    console.log(restrictionsList);
    restrictionsList.forEach(restriction =>{
        restrictionsHtml+=`
            <span class="inline-flex items-center rounded-md  px-2 py-3 mx-3 my-5 text-xl font-bold text-base ring-1 ring-inset ring-orange-400">${restriction}</span>
        `;
    })
    document.getElementById('restrictions').innerHTML = restrictionsHtml;
}

var displayTestimonials = (data) =>{
    console.log(data)
    var testimonialsHtml = "";
    data.forEach(review => {
        testimonialsHtml+=`
            <div class="card">
                    <i class="ribbon">
                        <span class="fa fa-star"></span>
                        <span class="fa fa-star"></span>
                        <span class="fa fa-star"></span>
                        <span class="fa fa-star"></span>
                        <span class="fa fa-star"></span>
                    </i>
                    <div class="cardContent">
                      <p class="cardBody">
                        <q>${review.comments}</q>
                      </p> 
                        <h4 style="margin-top: 5%;font-weight: bolder;color: #FFA456;">@${review.guest.name}</h4> 
                       <h4 style="margin-top: 5%;font-weight: bolder;color: #FFA456;">${review.date}</h4>  
                    </div>
            </div>
        `;
    });
    console.log(document.getElementById('displayTestimonials'))
    document.getElementById('displayTestimonials').innerHTML=testimonialsHtml;
}