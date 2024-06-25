var validateNumber = (id) =>{
    var data = document.getElementById(`${id}`);
    var reg = /^\d+$/;
    if(data.value.match(reg)){
        return functionAddValidEffects(data);
    }
    else{
        return functionAddInValidEffects(data);
    }
}

var validateAndAddHotel = () =>{
    if(!localStorage.getItem('loggedInUser')){
        alert("Something went wrong..Login again to continue!");
        window.location.href='./login.html';
    }
    fetch('http://localhost:5058/api/AdminHotel/RegisterHotel',
        {
            method:'POST',
            headers:{
                'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`,
                'Content-Type' : 'application/json',                
            },
            body:JSON.stringify({
                name: document.getElementById('hotelName').value,
                address: document.getElementById('address').value,
                city: document.getElementById('city').value,
                totalNoOfRooms: document.getElementById('roomsCount').value,
                amenities: document.getElementById('amenities').value,
                restrictions: document.getElementById('restriction').value
            })
        }
    )
    .then(async (res) => {
        resetFormValues('AddHotelForm');
        if (!res.ok) {
            const errorResponse = await res.json();
            console.log(errorResponse);
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    })
    .then( data => {
            console.log(data);
            alert("Hotel Registered successfully!");
    }).catch( error => {
        if(error.message === 'F')
        alert(error);
        resetFormValues('AddHotelForm');
        }
    );
}
