var hotelDetail = {};
var totalAmount = 0;
var advancePayment=0;

document.addEventListener('DOMContentLoaded',function(){
    checkUserLoggedInOrNot();
    let cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
    console.log(cartItems.length)
    if(cartItems.length>0){
        fetch('http://localhost:5058/api/AdminHotel/GetHotel',{
            method:'POST',
            headers:{
                'Content-Type':'application/json',
            },
            body:JSON.stringify(localStorage.getItem('currentHotel'))
        })
        .then(async(res) => {
            if (!res.ok) {
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            return await res.json();
        }).then(data => {
            displayHotelDetail(data);
        }).catch(error => {
            alert(error);
            console.error(error);
       });
        displayCartItems();
    }else{
        document.getElementById('bookRooms').innerHTML = "<div class='text-center text-xl fw-bolder p-3 mx-auto ring-1 ring-orange-400'>No Rooms where added yet!</div>";
        console.log("button disable");
        document.getElementById("clearBtn").disabled = true;
        document.getElementById("proceedBtn").disabled = true;
    }
}); 

var displayHotelDetail = (data) => {
    hotelDetail["name"] = data.name;
    hotelDetail["address"] = data.address;
    document.getElementById('hotelDetail').innerHTML=`
        <div class="px-3">
            <p class="font-bold text-orange-400 align-middle text-2xl"><a href="#">${data.name}</a></p>
            <p class="leading-6"> ${data.address} </p>
        </div>
    `;
}

var displayDetailsForModal = () =>{
    updateModalWithData();   
}

function updateModalWithData (){
    var rooms = JSON.parse(localStorage.getItem('cartItems')) || [];
    var roomsNeeded = [];
    console.log(rooms)
    rooms.forEach(room => 
        roomsNeeded.push({
            roomType : room.roomType,
            roomsNeeded : room.quantity
        })
    )
    fetch('http://localhost:5058/api/GuestBooking/BookRooms',{
        method:'POST',
        headers:{
            'Content-Type':'application/json',
            'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`
        },
        body:JSON.stringify(roomsNeeded)
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    }).then(data => {       
       displayModalWithData(data);
    }).catch(error => {
        alert(error);
        console.error(error);
   });
}

function showModal() {
    const modal = document.getElementById('bookingDetailsModal');
    modal.style.display = 'block';
    modal.classList.add('show');
}

var displayCartItems = () =>{
    var cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
    if(cartItems.length === 0 ){
        document.getElementById('bookRooms').innerHTML = "<div class='text-center text-xl fw-bolder mx-auto uppercase ring-1 ring-orange-400 p-3' style='width:50%;'>No Rooms where added yet!</div>";
        document.getElementById("proceedBtn").disabled = true;
        document.getElementById("clearBtn").disabled = true;
        return;
    }
    var cartHtml="";
    cartItems.forEach(item => {
        cartHtml += `
            <div class="grid grid-cols-5  py-3 items-center text-center shadow-md room">
                <p class="px-3 leading-6 text-left font-bold col-span-1">${item.roomType}</p>
                <div class="flex flex-row items-center col-span-2">
                    <p class="pr-3 leading-6">Rooms Needed</p>
                    <div class="flex flex-row items-center">
                        <button type="button" class="buttonStyle px-3 py-2 text-xl" id="increaseCountBtn" onclick="changeQuantity(${item.roomTypeId},-1)"><i class="bi bi-arrow-down-circle-fill" style="z-index: 1;position: relative;"></i></button>
                        <div class="w-11 h-11 mx-1 mt-2 text-center text-2xl" id="currCount">${item.quantity}</div>
                        <button type="button" class="buttonStyle px-3 py-2 text-xl" id="decreaseCountBtn" onclick="changeQuantity(${item.roomTypeId},1)"><i class="bi bi-arrow-up-circle-fill" style="z-index: 1;position: relative;"></i></button>
                    </div>
                </div>
                <p class="px-3 leading-6 col-span-1">&#x20B9;2000/-</p>
                <div class="px-3 col-span-1 flex flex-row">
                    <div class="options" id="options"><button class="px-3 py-2 text-xl" id="removeItemBtn" onclick="removeFromCart(${item.roomTypeId})">Delete</button></div>
                    <button class="px-3 py-2 text-xl" onclick="toggleMenu(this)"><i class="bi bi-three-dots-vertical" style="z-index: 1;position: relative;"></i></button>
                </div>
            </div>
        `;
    }); 
    document.getElementById('cartItemsDiv').innerHTML = cartHtml;
}

var removeFromCart = (id) => {
    var cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
    cartItems = cartItems.filter(item => item.roomTypeId !== id);
    localStorage.setItem('cartItems', JSON.stringify(cartItems));
    displayCartItems();
}

var changeQuantity = (id, increValue) => {
    var cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
    const itemToUpdate = cartItems.find(item => item.roomTypeId === id);
    if (itemToUpdate) {
        itemToUpdate.quantity += increValue;
        if (itemToUpdate.quantity <= 0) {
            removeFromCart(id);
            window.location.href='../hotels.html';
        } else {
            if(itemToUpdate.quantity > itemToUpdate.noOfRoomsAvailable){
                alert("Oops! No more rooms available!");
            }else{
                localStorage.setItem('cartItems', JSON.stringify(cart));
                displayCartItems();
            }
        }
    }
}

var clearBookingCart = () =>{
    console.log("Byutton clicked");
    localStorage.removeItem('cartItems');
    displayCartItems();
}

var payAndBookRoom = () => {
    var payment = document.paymentForm.payment.value;
    if(payment === ''){
        alert("Choose the payment amount");
        document.getElementById("proceedBtn").disabled = true;
        return ;
    }
    document.getElementById("proceedBtn").disabled = false;
    var paymentAmount=0.0;
    if(payment === 'halfPayment'){
        paymentAmount = advancePayment; 
    }else{
        paymentAmount = totalAmount;
    }
    fetch('http://localhost:5058/api/GuestBooking/MakePaymentAndConfirmBooking',{
        method:'POST',
        headers:{
            'Content-Type':'application/json',
            'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`
        },
        body:JSON.stringify(paymentAmount)
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    }).then(data => {
        alert("Payment done successfully and booking confirmed!")
        localStorage.removeItem('cartItems');
        window.location.href="./myBookings.html";
        // document.getElementById('bookingDetailsModal').hide();
        // displayCartItems();
    }).catch(error => {
        alert(error);
        console.error(error);
   });
}


var displayModalWithData = (data)=>{
    let cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
    if(cartItems.length === 0){
        document.getElementById('modalData').innerHTML = "<div>No room is selected for booking!</div>";
        document.getElementById("proceedPaymentAndBookBtn").disabled = true;
    }
    var roomsHtml = "";
    cartItems.forEach(item => {
        var amount = item.amount - item.amount*item.discount/100;
        totalAmount+=item.quantity*item.amount;
        roomsHtml += `
            <tr>
                <td>${item.roomType}</td>
                <td>${item.quantity}</td>
                <td>&#8377;${amount} </td>
            </tr>
        `;
    });
    roomsHtml+=`
        <tr>
            <td colspan = "2">Total Amount : </td>
            <td>${data.totalAmount}</td>
        </tr>
    `;
    totalAmount = data.finalAmount;
    advancePayment = data.advancePayment;

    document.getElementById('modalData').innerHTML = `
            <p class="font-bold text-orange-400">${hotelDetail.name}</p>
            <p class="leading-6">${hotelDetail.address}</p><br>
            <p class="text-orange-400 uppercase font-bold text-base mb-3">----------Rooms Booked----------</p>
            <div>
                <p>Total No of Rooms Booked : ${data.noOfRoomsBooked}</p>
                <table class="table table-striped">
                    <thead>
                        <tr>
                            <th>Type</th>
                            <th>Quantity</th>
                            <th>Price</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${roomsHtml}
                    </tbody>
                </table>
                <p>Discount : ${data.discountPercent}</p>
                <p>Final Amount : ${data.finalAmount}</p>
                <p>Advance Payment : ${data.advancePayment}</p>
            </div>
        `;
    showModal();
}









