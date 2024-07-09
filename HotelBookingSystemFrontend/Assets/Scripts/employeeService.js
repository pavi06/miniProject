var displayBookings = (data) =>{
    var bookingList = "";
    if(data.length === 0){
        document.getElementById('bookingsCount').classList.add('hideDiv');
        bookingList = `
            <div class="px-3 pb-5 mb-10 m-auto text-2xl uppercase text-center" style="width:80%;border:1px solid #FFA456; color:#FFA456; align-items: center;"><b>No bookings!</b></div>
        `;
        document.getElementById('displayAllBookings').innerHTML = bookingList;
        return;
    }
    data.forEach(booking => {       
        var textColor = booking.bookingStatus === "Cancelled" ? "red":"green";        
        bookingList+=`
                   <div class="px-3 pb-5 mb-10 mx-auto h-auto cardDesign" style="width: 60%;">
                   <h2 class="text-center fw-bolder pt-3 text-2xl" style="color:#FFA456">GUEST DETAILS</h2>
                    <div class="flex flex-col justify-between" style="width: 100%;">                      
                        <div class="p-3 mx-auto">
                            <div class="flex flex-row">
                                <p class="fw-bold">Guest Name :</p>
                                <p class="ml-5 text-xl">${booking.guestName}</p>
                            </div>
                            <div class="flex flex-row my-2">
                                <p class="fw-bold">Phone Number :</p>
                                <p class="ml-5 text-xl">${booking.phoneNumber}</p>
                            </div>
                            ${generateRoomsBookedTemplate(booking)}
                            <div class="flex flex-row">
                                <p class="fw-bold">BookingStatus :</p>
                                <p class="ml-5 text-xl" style="color:${textColor}">${booking.bookingStatus}</p>
                            </div>
                        </div>
                    </div>
                </div>

                `;
    });
    document.getElementById('displayCount').innerHTML = data.length;
    document.getElementById('bookingsCount').classList.remove('hideDiv');
    document.getElementById('displayAllBookings').innerHTML = bookingList;
}

var generateRoomsBookedTemplate = (booking) =>{
    var roomsBookedString = [];
        for (let roomType in booking.roomsBooked) {
            if (booking.roomsBooked.hasOwnProperty(roomType)) {
                let roomTypeCountString = `${roomType} - ${booking.roomsBooked[roomType]}`;
                roomsBookedString.push(roomTypeCountString);
            }
        }
    var rooms = roomsBookedString.join(', ');
    var roomsBookedDisplay = roomsBookedString.length > 0 ? `
            <div class="flex flex-row my-2" id="roomsBookedDisplay">
                <p class="fw-bold">RoomsBooked :</p>
                <p class="ml-5 text-xl">${rooms}</p>
            </div>
        ` : '';
    return roomsBookedDisplay;
}

var displaycheckInDetail = (data) =>{
    var bookingList = "";
    if(data.length === 0){
        document.getElementById('bookingsCount').classList.add('hideDiv');
        bookingList = `
            <div class="px-3 pb-5 mb-10 m-auto text-2xl uppercase text-center" style="width:80%;border:1px solid #FFA456; color:#FFA456; align-items: center;"><b>No bookings!</b></div>
        `;
        document.getElementById('displayAllBookings').innerHTML = bookingList;
        return;
    }
    
    data.forEach(booking => {
        //check whether ths id is needed!!!
        bookingList+=`
                    <div class="px-3 pb-5 mb-10 mx-auto h-auto cardDesign" style="width:60%">
                    <h2 class="text-center fw-bolder pt-3 text-2xl" style="color:#FFA456">GUEST DETAILS</h2>
                    <div class="flex flex-col justify-between">                      
                        <div class="p-3 mx-auto">
                        <div class="flex flex-row">
                            <p class="fw-bold">Guest Name :</p>
                            <p class="ml-5 text-xl">${booking.guestName}</p>
                        </div>
                        <div class="flex flex-row my-2">
                            <p class="fw-bold">Phone Number :</p>
                            <p class="ml-5 text-xl">${booking.guestPhoneNumber}</p>
                        </div>
                        ${generateRoomsBookedTemplate(booking)}
                        </div>
                    </div>
                </div> 
                `;
    });
    document.getElementById('displayCount').innerHTML = data.length;
    document.getElementById('bookingsCount').classList.remove('hideDiv');
    document.getElementById('displayAllBookings').innerHTML = bookingList;
}

var fetchBookings = (fetchString) => {
    document.getElementById('filterForm').reset();
    document.getElementById('filter').classList.remove('hideDiv');
    document.getElementById('bookingsLi').classList.add('active');
    document.getElementById('checkInBtn').classList.remove('active');
    var url="";
    if(fetchString === "Today")
        url = 'http://localhost:5058/api/HotelEmployee/GetAllBookingRequestRaisedToday';
    else if(fetchString === "All")
        url = 'http://localhost:5058/api/HotelEmployee/GetAllBookingRequest';
    fetch(url,{
        method:'GET',
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
    }).then(data => {
        displayBookings(data);
    }).catch(error => {
        addAlert(error.message)
    });
}

var filterBookings = () => {
    var attributeName = document.getElementById('filterBy').value;
    var value = document.getElementById('filterValue').value;
    if(!(attributeName && value)){
        addAlert("Provide data properly!")
        return 
    }
    if (!((attributeName === "Month" && value.match(/^0[1-9]|1[0-2]$/)) || (attributeName === "Date" && value.match(/^\d{2}-\d{2}-\d{4}$/)))) {
        addAlert("Provide data properly!");
        return;
    }    
    fetch('http://localhost:5058/api/HotelEmployee/GetAllBookingRequestByFiltering',{
        method:'POST',
        headers:{
            'Authorization':`Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`,
            'Content-Type' : 'application/json'
        },
        body:JSON.stringify({
            attribute:attributeName,
            attributeValue:value
        })
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    }).then(data => {
        displayBookings(data);
    }).catch(error => {
        addAlert(error.message)
    });
}

var validateAndLoginEmployee = () => {
    var userData = {
        email: document.getElementById('email').value,
        password: document.getElementById('password').value
    }
    if(validateEmail() && validatePassword()){
        fetch('http://localhost:5058/api/HotelEmployee/EmployeeLogin', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(userData)
        })
        .then(async(res) => {
            if (!res.ok) {
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            resetFormValues('employeeLoginForm');
            return res.json();
        })
        .then( data => {
                localStorage.setItem('loggedInUser',JSON.stringify(data));
                addSuccessAlert('Login successfull!')
                window.location.href="../EmployeeTemplate/employeeIndex.html";
        }).catch( error => {
            addAlert(error.message)
            resetFormValues('employeeLoginForm');
            }
        );
    }
    else{
        addAlert("login failed! Try again Later!!", 'failed')
    }

}

var logout = () =>{
    localStorage.clear();
    document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.remove('hide'));
    document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('hide'));
    document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.remove('active'));
    document.getElementById('logInBtn').classList.remove('hide');
    document.getElementById('displayAllBookings').classList.add('hide');
    document.getElementById('loginInfo').classList.add('show');
    document.getElementById('filter').classList.add('hide');
    document.getElementById('bookingsCount').classList.add('hide');
}

var GetCheckIns = ()=>{
    document.getElementById('filterForm').reset();
    document.getElementById('bookingsLi').classList.remove('active');
    document.getElementById('checkInBtn').classList.add('active');
    document.getElementById('filter').classList.add('hideDiv');
    fetch('http://localhost:5058/api/HotelEmployee/GetAllCheckIn',{
        method:'GET',
        headers:{
            'Authorization':`Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`,
            'Content-Type' : 'application/json'
        },
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    }).then(data => {
        displaycheckInDetail(data);
    }).catch(error => {
        addAlert(error.message)
    });
}

document.addEventListener('DOMContentLoaded', function() {
    var user = JSON.parse(localStorage.getItem('loggedInUser'));
    if(user && user.role === "HotelEmployee"){
        document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('show'));
        document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('hide'));
        document.getElementById('loginInfo').classList.add('hide');
        document.getElementById('bookingsLi').classList.add('active');
        document.getElementById('filter').classList.add('show');
        fetchBookings('All');
    }
    else{
        document.getElementById('filter').classList.add('hide');
        document.getElementById('bookingsCount').classList.add('hide');
    }
})

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
    document.getElementById('bookingsCount').classList.add('hide');
    document.getElementById('displayAllBookings').innerHTML =  `
    <div class="px-3 pb-5 mb-10 m-auto text-2xl uppercase text-center" style="width:80%;border:1px solid #FFA456; color:#FFA456; align-items: center;"><b>No bookings!</b></div>`;
    return;
}

if(document.querySelector(".modalCloseBtn")){
    document.querySelector(".modalCloseBtn").addEventListener("click", () =>
        document.getElementById('alertModal').classList.remove("active")
    );
}


var addSuccessAlert = (message) =>{
    if(document.getElementById('successAlertModal')){
        document.getElementById('successAlertModal').innerHTML = message;
        const modal = new bootstrap.Modal(document.getElementById('successAlertModal'));
        modal.show();
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