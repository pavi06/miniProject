var hotelDetail = {};
var totalAmount = 0;
var advancePayment = 0;

document.addEventListener('DOMContentLoaded', function () {
    checkUserLoggedInOrNot();
    let cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
    if (cartItems.length > 0) {
        fetch('http://localhost:5058/api/AdminHotel/GetHotel', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(localStorage.getItem('currentHotel'))
        })
            .then(async (res) => {
                if (!res.ok) {
                    if (res.status === 401) {
                        throw new Error('Unauthorized Access!');
                    }
                    const errorResponse = await res.json();
                    throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
                }
                return await res.json();
            }).then(data => {
                displayHotelDetail(data);
            }).catch(error => {
                addAlert(error.message)
            });
        displayCartItems();
    } else {
        document.getElementById('bookRooms').innerHTML = "<div class=' flex justify-center align-middle text-center text-xl fw-bolder p-3 mx-auto' style='width:50%'>No Rooms where added yet!</div>";
        document.getElementById("clearBtn").disabled = true;
        document.getElementById("proceedBtn").disabled = true;
    }
});

var displayHotelDetail = (data) => {
    hotelDetail["name"] = data.name;
    hotelDetail["address"] = data.address;
    document.getElementById('hotelDetail').innerHTML = `
        <div class="px-3">
            <p class="font-bold text-orange-400 align-middle text-2xl"><a href="#">${data.name}</a></p>
            <p class="leading-6"> ${data.address} </p>
        </div>
    `;
}

async function displayDetailsForModal() {
    await updateModalWithData();
}

async function updateModalWithData() {
    var rooms = JSON.parse(localStorage.getItem('cartItems')) || [];
    var roomsNeeded = [];
    rooms.forEach(room =>
        roomsNeeded.push({
            roomType: room.roomType,
            roomsNeeded: room.quantity
        })
    )
    var res = await checkTokenAboutToExpiry(JSON.parse(localStorage.getItem('loggedInUser')).accessToken);
    if (res === "Refresh") {
        await refreshToken();
    } else if (res === "Invalid accessToken!") {
        addAlert("Invalid AccessToken!");
        return;
    }
    fetch('http://localhost:5058/api/GuestBooking/BookRooms', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`
        },
        body: JSON.stringify(roomsNeeded)
    })
        .then(async (res) => {
            if (!res.ok) {
                if (res.status === 401) {
                    throw new Error('Unauthorized Access!');
                }
                const errorResponse = await res.json();
                console.log("Here")
                throw new Error(`${errorResponse.message}`);
            }
            return await res.json();
        }).then(data => {
            displayModalWithData(data);
        }).catch(error => {
            addAlert(error.message)
            if(error.message === "Unauthorized Access!"){
                logOut();
            }
        });
}

function showModal() {
    const modal = new bootstrap.Modal(document.getElementById('bookingDetailsModal'));
    modal.show();
}

var displayCartItems = () => {
    var cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
    if (cartItems.length === 0) {
        document.getElementById('bookRooms').innerHTML = "<div class='text-center text-xl fw-bolder mx-auto uppercase  p-3 flex justify-center align-middle' style='width:50%;'>No Rooms where added yet!</div>";
        document.getElementById("proceedBtn").disabled = true;
        document.getElementById("clearBtn").disabled = true;
        return;
    }
    var cartHtml = "";
    cartItems.forEach(item => {
        cartHtml += `
            <div class="flex py-3 items-center text-center shadow-md mx-auto justify-center room">
                <div class="col-span-1"><p class="px-3 leading-6 text-left font-bold">${item.roomType}</p></div>                
                <div class="flex items-center col-span-2 justify-center roomsNeededCount">
                    <p class="pr-5 leading-6">Rooms Needed</p>
                    <div class="flex flex-row items-center">
                        <button type="button" class="buttonStyle px-2 py-2 text-xl" id="increaseCountBtn" onclick="changeQuantity(${item.roomTypeId},-1)"><i class="bi bi-arrow-down-circle-fill" style="z-index: 1;position: relative;"></i></button>
                        <div class="w-11 h-11 mx-1 mt-2 text-center text-2xl" id="currCount">${item.quantity}</div>
                        <button type="button" class="buttonStyle px-2 py-2 text-xl" id="decreaseCountBtn" onclick="changeQuantity(${item.roomTypeId},1)"><i class="bi bi-arrow-up-circle-fill" style="z-index: 1;position: relative;"></i></button>
                    </div>
                </div>
                <div class="col-span-1"><p class="px-3 leading-6">&#x20B9;2000/-</p></div>
                <div class="pl-3 col-span-1 float-right deleteButton">
                    <div><button class="text-xl" id="removeItemBtn" onclick="removeFromCart(${item.roomTypeId})"><i class="bi bi-trash-fill"></i></button></div>
                </div>
            </div>
        `;
    });
    document.getElementById('cartItemsDiv').innerHTML = cartHtml;
}

var removeFromCart = (id) => {
    var cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
    cartItems = cartItems.filter(item => item.roomTypeId !== id);
    if (cartItems.length > 0) {
        localStorage.setItem('cartItems', JSON.stringify(cartItems));
        displayCartItems();
        return;
    }
    localStorage.removeItem('cartItems');
    window.history.back(); //go to previous page
}

var changeQuantity = (id, increValue) => {
    var cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
    const itemToUpdate = cartItems.find(item => item.roomTypeId === id);
    if (itemToUpdate) {
        itemToUpdate.quantity += increValue;
        if (itemToUpdate.quantity <= 0) {
            removeFromCart(id);
        } else {
            if (itemToUpdate.quantity > itemToUpdate.noOfRoomsAvailable) {
                addAlert("Oops! No more rooms available!");
            } else {
                localStorage.setItem('cartItems', JSON.stringify(cartItems));
                displayCartItems();
            }
        }
    }
}

var clearBookingCart = () => {
    localStorage.removeItem('cartItems');
    displayCartItems();
}

async function payAndBookRoom(){
    var payment = document.paymentForm.payment.value;
    if (payment === '') {
        addAlert("Choose the payment amount");
        document.getElementById("proceedBtn").disabled = true;
        return;
    }
    document.getElementById("proceedBtn").disabled = false;
    var paymentAmount = 0.0;
    if (payment === 'halfPayment') {
        paymentAmount = advancePayment;
    } else {
        paymentAmount = totalAmount;
    }
    var res = await checkTokenAboutToExpiry(JSON.parse(localStorage.getItem('loggedInUser')).accessToken);
    if (res === "Refresh") {
        await refreshToken();
    } else if (res === "Invalid accessToken!") {
        addAlert("Invalid AccessToken!");
        return;
    }
    fetch('http://localhost:5058/api/GuestBooking/MakePaymentAndConfirmBooking', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`
        },
        body: JSON.stringify(paymentAmount)
    })
        .then(async (res) => {
            if (!res.ok) {
                if (res.status === 401) {
                    throw new Error('Unauthorized Access!');
                }
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            return await res.json();
        }).then(data => {
            setTimeout(addSuccessAlert("Payment done successfully and booking confirmed!"), 3000);
            localStorage.removeItem('cartItems');
            setTimeout(window.location.href = "./myBookings.html", 5000);
        }).catch(error => {
            addAlert(error.message)
        });
}


var displayModalWithData = (data) => {
    let cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
    if (cartItems.length === 0) {
        document.getElementById('modalData').innerHTML = "<div>No room is selected for booking!</div>";
        document.getElementById("proceedPaymentAndBookBtn").disabled = true;
    }
    var roomsHtml = "";
    cartItems.forEach(item => {
        var amount = item.amount - item.amount * item.discount / 100;
        totalAmount += item.quantity * item.amount;
        roomsHtml += `
            <tr>
                <td>${item.roomType}</td>
                <td>${item.quantity}</td>
                <td>&#8377;${amount} </td>
            </tr>
        `;
    });
    roomsHtml += `
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







