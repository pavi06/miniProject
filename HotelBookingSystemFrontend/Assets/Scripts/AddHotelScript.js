var validateAndAddHotel = () =>{
    if(!localStorage.getItem('isLoggedIn')){
        alert("Login to continue!");
        window.location.href='../login.html';
        return;
    }
    if(validate('hotelName') && validateAddress('address') && validate('city') && validateNumber('roomsCount')
    && validate('amenities') && validate('restriction')){
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
            if (!res.ok) {
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            resetFormValues('AddHotelForm');
            return await res.json();
        })
        .then( data => {
                alert("Hotel Registered successfully!");
        }).catch( error => {
            alert(error);
            resetFormValues('AddHotelForm');
            }
        );
    }else{
        alert("Provide all values properly to register!");
    }
}
