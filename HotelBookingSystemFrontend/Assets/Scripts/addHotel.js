var validateAndAddHotel = async () =>{
    if(!localStorage.getItem('isLoggedIn')){
        addAlert("Login to continue!");
        window.location.href='../login.html';
        return;
    }
    if(validate('hotelName') && validateAddress('address') && validate('city') && validateNumber('roomsCount')
    && validate('amenities') && validate('restriction')){
        var res = await checkTokenAboutToExpiry(JSON.parse(localStorage.getItem('loggedInUser')).accessToken);
        if (res === "Refresh") {
            await refreshToken();
        } else if (res === "Invalid accessToken!") {
            addAlert("Invalid AccessToken!");
            return;
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
            if (!res.ok) {
                if (res.status === 401) {
                    throw new Error('Unauthorized Access!');
                }
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            resetFormValues('AddHotelForm','input, textarea');
            return await res.json();
        })
        .then( data => {
            addSuccessAlert("Hotel Registered Successfully!")
        }).catch( error => {
            addAlert(error.message)
            resetFormValues('AddHotelForm','input, textarea');
            if(error.message === "Unauthorized Access!"){
                adminLogOut();
            }
        });
    }else{
        addAlert("Provide all values properly to register!");
    }
}

document.addEventListener('DOMContentLoaded',function(){
    checkAdminLoggedInOrNot();
})
