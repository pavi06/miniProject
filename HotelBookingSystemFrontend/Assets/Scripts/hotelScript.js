var validateLocation = () =>{
    var locationElement = document.getElementById('location');
    console.log(locationElement)
    var reg=/^[a-zA-Z]+$/;
    if(locationElement.value && locationElement.value.match(reg)){
        functionAddValidEffects(locationElement);
        return true;
    }
    else{
        functionAddInValidEffects(locationElement);
        return false;
    }
}

var validateDate = () =>{
    var dateElement = document.getElementById('checkInDate');
    console.log(dateElement)
    var today = new Date();
    let formattedDate = `${today.getFullYear()}-${(today.getMonth() + 1).toString().padStart(2, '0')}-${today.getDate().toString().padStart(2, '0')}`;
    console.log(formattedDate)
    if(dateElement.value && Date.parse(dateElement.value) >= Date.parse(formattedDate)) {
        functionAddValidEffects(dateElement);
        return true;
    } else {
        functionAddInValidEffects(dateElement);
        return false;
    }
}

var validateAndGet = () => {
    console.log("Inside validate")
    var locationInput = document.hotelForm.location.value;
    console.log("Hotel value")
    console.log(locationInput)
    var checkInDateInput = document.hotelForm.checkInDate.value;
    console.log("Date value")
    console.log(checkInDateInput)
    var hotelDetail = {
        location:locationInput,
        date:checkInDateInput
    }
    if(validateLocation() && validateDate()){
        console.log("Inside api");
        fetch('http://localhost:5058/api/GuestBooking/GetHotelsByLocationAndDate', {
            method: 'POST',
            headers: {'Content-Type' : 'application/json'},
            body:JSON.stringify(hotelDetail)
            }).then((res) => {
                if (!res.ok) {
                    const errorResponse = res.json();
                    throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
                }
                return res.json();
            }).then(data => {
                console.log(data);
                displayHotelsRetrieved(data);
            }).catch(error => {
                alert(error);
                console.error(error);
        });
    }
    else{
        alert("Provide values properly!");
    }
}

document.addEventListener('DOMContentLoaded', function() {
    fetch('http://localhost:5058/api/AdminHotel/GetAllHotels', {
        method: 'GET',
        }).then(async(res) => {
            if (!res.ok) {
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            return await res.json();
        }).then(data => {
            console.log(data);
            displayHotelsRetrieved(data);
        }).catch(error => {
            alert(error);
            console.error(error);
       });
});


