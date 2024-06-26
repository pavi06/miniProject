// document.addEventListener('DOMContentLoaded', function() {
//     var element = document.getElementsByClassName("discount");
//     element.forEach(ele => {
//     });
//     var discount = document.getElementById("discountPercent");
//     if(discount.textContent>0){
       
//         element.classList.add("reveal");
//     }
//     else{
//         element.classList.add("hide")
//     }
//     console.log(discount);
// }, false);


function changeRoomCount(countID, task) {
    var currElement = document.getElementById(countID);
    console.log(currElement);
    var currCount = parseInt(currElement.textContent);
    console.log(currCount)
    console.log(task)
    if (task === 'increase') {
        console.log("increase")
        currElement.textContent = currCount + 1;
    } else if (task === 'decrease' && currCount > 1) {
        console.log("decrease")
        currElement.textContent = currCount - 1;
    }
    else if(task === 'decrease' && currCount == 1){
        var parentDiv = currElement.parentElement.parentElement.parentElement;
        var preParent = parentDiv.parentElement;
        parentDiv.parentNode.removeChild(parentDiv);
        console.log(preParent);
        console.log(preParent.childNodes.length)
        if(preParent.childNodes.length === 0){
            console.log("innnn")
            var prePreParent = preParent.parentElement;
            preParent.parentNode.removeChild(preParent);
            console.log(prePreParent);
            prePreParent.parentNode.removeChild(prePreParent.parentElement);
        }
    }
}

function toggleMenu(menu) {
    var ele = menu.previousElementSibling;
    console.log(ele)
    ele.classList.toggle('active');
}

function deleteItem(currEle) {
    console.log(currEle)
    var element = currEle.parentNode;
    element = element.parentNode;
    element.parentNode.removeChild(element);
    console.log('Item deleted!');
}


function clearAll(){
    var rooms = document.getElementById("roomStart");
    rooms.parentNode.removeChild(rooms);
}

var cancelBooking = (id) =>{
    console.log(id)
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
        return await res.json();
    }).then(data => {
        console.log("Cancelled successfully");
        fetchBookings();
    }).catch(error => {
        alert(error);
        console.error(error);
   });
}



var displayMyBookings = (data) =>{
    var bookingList = "";
    data.forEach(bookedHotel => {
        var checkInDate = new Date(bookedHotel.checkInDate).toLocaleDateString('en-US');
        var checkOutDate = new Date(bookedHotel.checkOutDate).toLocaleDateString('en-US');
        modifyDetailsHtml = new Date(checkInDate) < new Date() ? ``: `
                <div class="flex flex-row justify-center my-3" id="modifyBookingBtn">
                    <button class="buttonStyle p-2 mr-5"><span>Modify Booking</span></button>
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
        console.log(data);
        displayMyBookings(data);
    })
    .catch(error => {
        alert(error);
        console.error(error);
    });
}
document.addEventListener('DOMContentLoaded',function(){
    fetchBookings();
})