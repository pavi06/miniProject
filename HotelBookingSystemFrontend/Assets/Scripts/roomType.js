var addRoomTypes = (itemName) =>{
    var hotelId = localStorage.getItem(`${itemName}`);
    fetch('http://localhost:5058/api/AdminHotel/GetHotel',{
        method:'POST',
        headers:{
            'Content-Type' : 'application/json'
        },
        body:JSON.stringify(hotelId)
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    })
    .then(data => {
        var roomTypesHtml ="";
        data.roomTypes.forEach(roomtype => {
            roomTypesHtml+=`
                <option value ="${roomtype.roomTypeId}">${roomtype.type}</option>
            `;
        })
        document.getElementById('roomTypesSelectOptions').innerHTML=roomTypesHtml;
    })
    .catch(error => {
        alert(error);
        console.error(error);
    });
}


var validateAndAddRoomType = () =>{
    if(validate('facilities') && validateDecimalNumber('discount') && validateDecimalNumber('price') && validateNumber('cots')
    && validateNumber('occupancy')){
        fetch('http://localhost:5058/api/AdminRoom/RegisterRoomTypeForHotel', {
            method: 'POST',
            headers:{
                'Authorization':`Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`,
                'Content-Type':'application/json'
            },
            body:JSON.stringify({
                type: document.getElementById('roomType').value,
                occupancy: document.getElementById('occupancy').value,
                images: document.getElementById('roomImages').value,
                amount: document.getElementById('price').value,
                cotsAvailable: document.getElementById('cots').value,
                amenities: document.getElementById('facilities').value,
                discount: document.getElementById('discount').value,
                hotelId: localStorage.getItem('currentHotel')
            })
        })
        .then(async(res) => {
            if (!res.ok) {
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            resetFormValues('AddRoomTypeForm');
            return await res.json();
        })
        .then(data => {
            alert("Room type added successfully!");
        })
        .catch(error => {
            alert(error);
            console.error(error);
        });
    }
}


var validateAndAddRoom = () =>{
    
}