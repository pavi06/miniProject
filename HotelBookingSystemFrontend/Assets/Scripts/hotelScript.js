var validateLocation = () =>{
    var locationElement = document.hotelForm.location;
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
    var dateElement = document.hotelForm.checkInDate;
    console.log(dateElement)
    var today = new Date();
    let formattedDate = `${today.getFullYear()}-${(today.getMonth() + 1).toString().padStart(2, '0')}-${today.getDate().toString().padStart(2, '0')}`;
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
    var location = document.hotelForm.location.value;
    console.log(location)
    var checkInDate = document.hotelForm.checkInDate.value;
    console.log(checkInDate)
    if(validateLocation() && validateDate()){
        console.log("Inside api");
        fetch('http://localhost:5058/api/GuestBooking/GetHotelsByLocationAndDate', {
            method: 'POST',
            headers: {'Content-Type' : 'application/json'},
            body:JSON.stringify({
                location:location,
                date:checkInDate
            })
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
    }
    else{
        alert("Provide values properly!");
    }
}