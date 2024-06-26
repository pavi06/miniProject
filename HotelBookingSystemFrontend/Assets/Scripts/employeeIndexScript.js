var displayBookings = (data) =>{
    var bookingList = "";
    if(data.length === 0){
        document.getElementById('bookingsCount').classList.add('hideDiv');
        console.log("Empty data");
        bookingList = `
            <div class="px-3 pb-5 mb-10 m-auto text-2xl uppercase text-center" style="width:80%;border:1px solid #FFA456; color:#FFA456; align-items: center;"><b>No bookings!</b></div>
        `;
        document.getElementById('displayAllBookings').innerHTML = bookingList;
        return;
    }
    data.forEach(booking => {
        //check whether ths id is needed!!!        
        var textColor = booking.bookingStatus === "Cancelled" ? "red":"green";        
        bookingList+=`
                    <div class="px-3 pb-5 mb-10 mx-auto h-auto cardDesign" style="width:80%">
                    <div class="flex flex-col justify-between">                      
                        <div class="p-3">
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
                    <div class="px-3 pb-5 mb-10 mx-auto h-auto cardDesign" style="width:80%">
                    <div class="flex flex-col justify-between">                      
                        <div class="p-3">
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
        alert(error);
        console.error(error);
    });
}

var filterBookings = () => {
    var attributeName = document.getElementById('filterBy').value;
    var value = document.getElementById('filterValue').value;
    console.log(attributeName)
    console.log(value)
    if(attributeName && value){
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
            console.log(data);
            displayBookings(data);
        }).catch(error => {
            alert(error);
            console.error(error);
        });
    }
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
                console.log("login successfully");
                alert("Login successfull");
                //check and redirect to the page most recently viwed
                window.location.href="../EmployeeTemplate/employeeIndex.html";
        }).catch( error => {
            alert(error);
            resetFormValues('employeeLoginForm');
            }
        );
    }
    else{
        alert("login failed! Try again Later!!");
    }

}

var logout = () =>{
    localStorage.removeItem('loggedInUser');
    document.getElementById('logOutBtn').classList.add('hideDiv');
    document.getElementById('bookingsLi').classList.remove('active');
    document.getElementById('checkInBtn').classList.remove('active');
    document.getElementById('logInBtn').classList.remove('hideDiv');
    document.getElementById('displayAllBookings').innerHTML = "";
    document.getElementById('loginInfo').classList.remove('hideDiv');
    document.getElementById('filter').classList.add('hideDiv');
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
        console.log(data);
        displaycheckInDetail(data);
    }).catch(error => {
        alert(error);
        console.error(error);
    });
}

document.addEventListener('DOMContentLoaded', function() {
    var user = JSON.parse(localStorage.getItem('loggedInUser'));
    if(user && user.role === "HotelEmployee"){
        document.getElementById('loginInfo').classList.add('hideDiv');
        document.getElementById('logInBtn').classList.add('hideDiv');
        document.getElementById('bookingsLi').classList.add('active');
        document.getElementById('filter').classList.remove('hideDiv');
        fetchBookings('All');
    }else{
        document.getElementById('logOutBtn').classList.add('hideDiv');
    }
})