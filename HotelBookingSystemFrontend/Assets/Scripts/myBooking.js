var displayMyBookings = (data) =>{
    var bookingList = "";
    data.forEach(bookedHotel => {
        var checkInDate = new Date(bookedHotel.checkInDate).toLocaleDateString('en-US');
        var checkOutDate = new Date(bookedHotel.checkOutDate).toLocaleDateString('en-US');
        modifyDetailsHtml = new Date(checkInDate) < new Date() ? ``: `
                <div class="flex flex-row justify-center my-3" id="modifyBookingBtn">
                    <button class="buttonStyle p-2 mr-5" onclick="modifyBooking(${bookedHotel.bookId},${bookedHotel.hotelId})"><span>Modify Booking</span></button>
                    <button class="buttonStyle p-2" onclick="cancelBooking(${bookedHotel.bookId})"><span>Cancel Booking</span></button>
                </div>
            `;
        dateDisplayHtml = new Date(checkOutDate).toLocaleDateString('en-US')===new Date('1/1/1').toLocaleDateString('en-US') ? `` : `<p class="info">CheckInDate : ${checkInDate}<br>CheckOutDate : ${checkOutDate}</p>`;
        
        var discountTemplate = bookedHotel.discountPercent>0? ` <p class="info discount">Discount Percent : <span id="discountPercent" style="color: green;">${bookedHotel.discountPercent}</span>%</p>` : ``;
        bookingList+=`
            <div class="h-75 m-5 ml-20 bg-white bookedCard" style="width: 80%;box-shadow: rgba(0, 0, 0, 0.24) 0px 3px 8px;">
                <span class="ribbon font-bold uppercase">${bookedHotel.bookingStatus}</span>
                <div class="flex flex-row justify-between" style="margin-top: 3rem;padding: 2%;">
                    <div class="ml-5">
                        <p class="info"><a class="font-bold text-2xl">${bookedHotel.hotelName}</a></p>
                        <p class="info">${bookedHotel.hotelLocation}</p>
                        <p class="info mt-3">No of rooms booked : ${bookedHotel.noOfRoomsBooked}</p>
                    </div>
                    <div class="mr-5">
                        ${dateDisplayHtml}
                        <p class="info mt-10 mp">Total Amount : &#x20b9; ${bookedHotel.totalAmount}</p>
                        ${discountTemplate}
                        <p class="info">Final Amount : &#x20b9; ${bookedHotel.finalAmount}</p>
                    </div>                    
                </div>
                ${modifyDetailsHtml}
            </div>
        `;
    });
    document.getElementById('myBookings').innerHTML = bookingList;
}

var fetchBookings = () =>{
    fetch('http://localhost:5058/api/GuestBooking/GetMyBookings', {
        method: 'GET',
        headers:{
            'Authorization':`Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`,
            'Content-Type' : 'application/json'
        }
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    })
    .then(data => {
        displayMyBookings(data);
    })
    .catch(error => {
        alert(error);
        console.error(error);
    });
}

document.addEventListener('DOMContentLoaded',function(){
    checkUserLoggedInOrNot();
    fetchBookings();
})



//Modify booking Region
var modifyBooking = (bookingId, hotelId) => {
    localStorage.setItem('currentBookingId',bookingId);
    localStorage.setItem('currentHotelIdOfBooking',hotelId);
    addRoomTypes('currentHotelIdOfBooking');
    const modifyBookingModal = new bootstrap.Modal(document.getElementById('modifyBookingModal'));
    modifyBookingModal.show();
    document.getElementById('modifyBookingModal').addEventListener('hidden.bs.modal', function (e) {
        document.getElementById('previewRoomsToCancel').innerHTML="";
    });
}

const roomsList = [];

var displayDiv = () =>{
    document.getElementById('previewRoomsToCancel').innerHTML+=`
        <div>
            <div>
                <p>${roomsList[roomsList.length-1].roomType}</p>
                <p>${roomsList[roomsList.length-1].noOfRoomsToCancel}</p>
            </div>
        </div>
    `;
}

var storeRoomToCancel = () =>{
    const room = {
        roomType : document.getElementById('roomTypesSelect').innerHTML,
        noOfRoomsToCancel : document.getElementById('roomsCount').value
    }
    roomsList.push(room);
    document.getElementById('modifyBookingForm').reset();
    displayDiv();
}

var modifyBookingFromModal = () =>{
    fetch('http://localhost:5058/api/GuestBooking/ModifyBooking',{
        method:'PUT',
        headers:{
            'Content-Type':'application/json',
            'Authorization':`Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`,
        },
        body:JSON.stringify({
            bookingId:localStorage.getItem('currentBookingId'),
            cancelRooms :roomsList
        })
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    }).then(data => {
        alert("Booking updated successfully!");
        //popup()
        fetchBookings();
        //refund payment if available with delay!!
    }).catch(error => {
        alert(error);
        console.error(error);
   });
}
//endRegion


var cancelBooking = (id) =>{
    fetch('http://localhost:5058/api/GuestBooking/CancelBooking',{
        method:'PUT',
        headers:{
            'Content-Type':'application/json',
            'Authorization':`Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`,
        },
        body:JSON.stringify({id})
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    }).then(data => {
        console.log("Cancelled successfully");
        //popup()
        fetchBookings();
        //refund payment if available with delay!!
    }).catch(error => {
        alert(error);
        console.error(error);
   });
}

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
        data.roomTypes.forEach(roomtype => {
            roomTypesHtml+=`
                <option value ="${roomtype.roomTypeId}">${roomtype.type}</option>
            `;
        })
        document.getElementById('roomTypesSelect').innerHTML=roomTypesHtml;
    })
    .catch(error => {
        alert(error);
        console.error(error);
    });
}