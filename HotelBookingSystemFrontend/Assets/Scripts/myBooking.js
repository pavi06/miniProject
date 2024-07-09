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
        dateDisplayHtml = new Date(checkOutDate).toLocaleDateString('en-US')===new Date('1/1/1').toLocaleDateString('en-US') ? `` : `<p class="info fw-bolder">CheckInDate : <span style="color:#FFA456">${checkInDate}</span><br>CheckOutDate : <span style="color:#FFA456;">${checkOutDate}</span></p>`;
        
        var discountTemplate = bookedHotel.discountPercent>0? ` <p class="info discount">Discount Percent : <span id="discountPercent" style="color: green;">${bookedHotel.discountPercent}</span>%</p>` : ``;
        bookingList+=`
            <div class="h-75 m-5 ml-20 bg-white bookedCard" style="width: 60%;box-shadow: rgba(0, 0, 0, 0.24) 0px 3px 8px;border-radius:20px">
                <span class="ribbon font-bold uppercase">${bookedHotel.bookingStatus}</span>
                <h3 class="fw-bolder text-center text-2xl pt-3" style="color:#FFA456;">BOOKING DETAILS</h3>
                <div class="flex flex-row justify-between" style="margin-top: 2rem;padding: 2%;">
                    <div class="ml-5">
                        <p class="info"><a class="font-bold text-2xl">${bookedHotel.hotelName}</a></p>
                        <p class="info">${bookedHotel.hotelLocation}</p>
                        <p class="info mt-3">No of rooms booked : ${bookedHotel.noOfRoomsBooked}</p>
                         ${dateDisplayHtml}
                    </div>
                    <div class="mr-5">
                        <p>Booked On : ${new Date(bookedHotel.bookedDate).toLocaleDateString('en-US')}</p>
                        <p class="info mt-10 mp">Total Amount : &#x20b9; ${bookedHotel.totalAmount}</p>
                        ${discountTemplate}
                        <p class="info fw-bolder">Final Amount : &#x20b9; ${bookedHotel.finalAmount}</p>
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
        addAlert(error.message)
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
    var div = document.createElement('div');
    div.innerHTML=`
        <div class="flex flex-row justify-between">
            <p>${roomsList[roomsList.length-1].roomType}</p>
            <p>${roomsList[roomsList.length-1].noOfRoomsToCancel}</p>
        </div>
    `;
    document.getElementById('previewRoomsToCancel').appendChild(div);
    
}

var storeRoomToCancel = () =>{
    const room = {
        roomType : document.getElementById('roomTypesSelect').value,
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
        return await res.text();
    }).then(data => {
        if(data === 'Cannot Modify instead proceed with cancel booking!'){
            addAlert(data);
        }else{
            addSuccessAlert(data)
        }
        document.querySelector('[data-bs-dismiss="modal"]').click();
        fetchBookings();
        refundPopup(localStorage.getItem('currentBookingId'));
    }).catch(error => {
        addAlert(error.message)
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
        body:JSON.stringify(id)
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.text();
    }).then(data => {
        addSuccessAlert(data)
        fetchBookings();
        refundPopup(id);
    }).catch(error => {
        addAlert(error.message)
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
        console.log(data.roomTypes)
        for (let key in data.roomTypes) {
            if (data.roomTypes.hasOwnProperty(key)) {
              roomTypesHtml+=`
                <option value ="${data.roomTypes[key]}">${data.roomTypes[key]}</option>
            `;
            }
        }
        document.getElementById('roomTypesSelect').innerHTML=roomTypesHtml;
    })
    .catch(error => {
        addAlert(error.message)
    });
}

var refundPopup = (id) =>{
    fetch('http://localhost:5058/api/GuestBooking/CheckRefundDone',{
        method:'POST',
        headers:{
            'Content-Type':'application/json',
            'Authorization':`Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`,
        },
        body:JSON.stringify(id)        
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.text();
    }).then(data => {
        if(data !== "No Refund"){
            setTimeout(function() {
                addSuccessAlert(data)
            }, 60000);
        } 
    }).catch(error => {
        addAlert(error.message)
   });
}

var addSuccessAlert = (message) =>{
    console.log("--inside alert")
    if(document.getElementById('successAlertModal')){
        document.getElementById('successAlertModal').innerHTML = message;
        const modal = new bootstrap.Modal(document.getElementById('successAlertModal'));
        modal.show();
        console.log("modal show")
        return;
    }
    const alert = document.createElement('div')
    alert.innerHTML = `
         <div class="modal" id="successAlertModal">
            <div class="modal-dialog">
                <div class="modal-content">
                <div class="modal-header bg-green-400">
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <img class="flex mx-auto" src="../../Assets/Images/success.png" style="width:40%; height:40%;"/>
                <h5 class="text-2xl mt-0" style="font-weight:bolder;text-transform:uppercase;text-align:center;color:green;">SUCCESS</h5>
                <div class="modal-body text-center">
                    <p class="text-xl text-black" id="successAlertMessage">${message}</p>
                </div>
                <button type="button" class="btn uppercase w-25 text-center mx-auto my-3 bg-green-400  fw-bolder alertBtn" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    `;
    document.body.insertAdjacentElement('beforeend', alert);
    const modal = new bootstrap.Modal(document.getElementById('successAlertModal'));
    modal.show();
    if(message.includes('No hotels are available!') || message === "No more hotels available!"){
        if( document.getElementById('loadBtn')){
            document.getElementById('loadBtn').classList.add('hide');
        }
        if(document.getElementById('loadBtnWithBody')){
            document.getElementById('loadBtnWithBody').classList.add('hide');
        }
    }
}

var addAlert = (message) =>{
    if(document.getElementById('alertModal')){
        document.getElementById('alertMessage').innerHTML = message;
        const modal = new bootstrap.Modal(document.getElementById('alertModal'));
        modal.show();
        return;
    }
    const alert = document.createElement('div')
    alert.innerHTML = `
         <div class="modal" id="alertModal" style="border-radius:50px">
            <div class="modal-dialog">
                <div class="modal-content">
                <div class="modal-header bg-red-400">
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <img class="flex mx-auto" src="../../Assets/Images/error.png" style="width:40%; height:40%;"/>
                <h5 class="text-2xl mt-0" style="font-weight:bolder;text-transform:uppercase;text-align:center;color:red;">Oops!</h5>
                <div class="modal-body text-center">
                    <p class="text-xl text-black" id="alertMessage">${message}</p>
                </div>
                <button type="button" class="btn uppercase w-25 text-center mx-auto my-3 bg-red-400  fw-bolder alertBtn" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    `;
    document.body.insertAdjacentElement('beforeend', alert);
    const modal = new bootstrap.Modal(document.getElementById('alertModal'));
    modal.show();
    if(message === '404 Error! - No hotels are available!' || message === "No more hotels available!"){
        if( document.getElementById('loadBtn')){
            document.getElementById('loadBtn').classList.add('hide');
        }
        if(document.getElementById('loadBtnWithBody')){
            document.getElementById('loadBtnWithBody').classList.add('hide');
        }
    }
}

var checkUserLoggedInOrNot = () =>{
    if( localStorage.getItem('isLoggedIn')){
        if(window.location.pathname === '/Templates/login.html'){
            return;
        }
        document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('showNav'));
        var cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
        if(document.querySelector('.bookRooms')){
            if(cartItems.length === 0){
                document.querySelector('.bookRooms').classList.add('hide');
            }else{
                document.querySelector('.bookRooms').classList.add('show');
            }
        }
        document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('hide'));
    }
    else{
        document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('hide'));
        document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('showNav')); 
    }
}

var logOut = () => {
    localStorage.clear();
    if(window.location.pathname === '/Templates/UserTemplate/myBookings.html' || '/Templates/UserTemplate/booking.html'){
        window.location.href = '/Templates/hotels.html';
    }
    document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('show'));
    document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('hide'));

};
