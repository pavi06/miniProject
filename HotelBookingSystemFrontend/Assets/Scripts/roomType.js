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

var openModalForRoomTypeEdit = (roomtypeId, hotelId) =>{
    localStorage.setItem('currentRoomType', roomtypeId);
    localStorage.setItem('currentHotel', hotelId);
    const addRoomModal = new bootstrap.Modal(document.getElementById('editRoomTypeModal'));
    addRoomModal.show();
    document.getElementById('editRoomTypeModal').addEventListener('hidden.bs.modal', function () {
        resetFormValues('editRoomTypeForm', 'input');
    });
}

var editRoomTypeFromModal = () =>{
    var data = document.getElementById('editRoomTypeForm').querySelectorAll('input');
    const attributeValues = {}
    Array.from(data).forEach(ele => {
        if(ele.value){
            attributeValues[ele.name] = ele.value;
        }
    });
    const bodyValue = {
        hotelId : localStorage.getItem('currentHotel'),
        roomTypeId : localStorage.getItem('currentRoomType'),
        attributeValuesPair : attributeValues
    }
    fetch('http://localhost:5058/api/AdminRoom/UpdateRoomTypeForHotel',{
        method:'PUT',
        headers:{
            'Authorization':`Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`,
            'Content-Type' : 'application/json'
        },
        body:JSON.stringify(bodyValue)
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
        alert("Updated successfully")
        document.querySelector('[data-bs-dismiss="modal"]').click();
        fetchData();
    })
    .catch(error => {
        alert(error);
        console.error(error);
    });
}


var displayRoomTypes = (data) =>{
    var roomTypesHtml="";
    data.forEach(roomType => {
        var finalAmount = roomType.amount - (roomType.amount * roomType.discount/100);
        roomTypesHtml +=`
            <div class="px-3 pb-5 mb-10 h-auto mx-auto cardDesign" style="width: 60%;">
          <div class="flex flex-row justify-between">                      
              <div class="w-60 h-50 mt-10" style="object-fit: cover;">
                  <img src="../../Assets/Images/hotelImage5.jpg" alt="Image gallery"/>
              </div>
              <div class="p-3">
                <p class="uppercase fw-bolder mb-3" style="color: #FFA456;font-size: 30px;">General Info</p>
                <div class="grid grid-cols-2">
                  <p class="uppercase fw-bolder">room type </p>
                  <p class="lowercase fw-normal px-3" style="font-size:25px;">${roomType.type}</p>
                </div>
                <div class="grid grid-cols-2">
                  <p class="uppercase fw-bolder">occupancy </p>
                  <p class="lowercase fw-normal px-3" style="font-size:25px;">${roomType.occupancy}</p>
                </div>
                <div class="grid grid-cols-2">
                  <p class="uppercase fw-bolder">cotsAvailable </p>
                  <p class="lowercase fw-normal px-3" style="font-size:25px;">${roomType.cotsAvailable}</p>
                </div>
                <div class="grid grid-cols-2">
                  <p class="uppercase fw-bolder">amenities </p>
                  <p class="lowercase fw-normal px-3" style="font-size:25px;">${roomType.amenities}</p>
                </div>
                <p class="uppercase fw-bolder" style="color: #FFA456;font-size: 30px;">Pricing</p>
                <div class="grid grid-cols-2">
                      <p class="uppercase fw-bolder">amount </p>
                      <p class="lowercase fw-normal px-3" style="font-size:25px;">${roomType.amount}</p>
                    </div>
                    <div class="grid grid-cols-2">
                      <p class="uppercase fw-bolder">Discount </p>
                      <p class="lowercase fw-normal px-3" style="font-size:25px;">${roomType.discount}</p>
                    </div>
                    <div class="grid grid-cols-2">
                      <p class="uppercase fw-bolder">Final amount</p>
                      <p class="lowercase fw-normal px-3" style="font-size:25px;">${finalAmount}</p>
                    </div>
              </div>
          </div>
          <div class="flex flex-row flex-wrap px-3 mt-3 justify-center">
              <button type="button" class="buttonStyle mr-5" style="width:20%; padding:5px" id="editHotelBtn" onclick="openModalForRoomTypeEdit(${roomType.roomTypeId}, ${roomType.hotelId})"><span>Edit RoomType</span></button>
          </div>
        </div>
        `;
    })
    document.getElementById('roomTypesDiv').innerHTML=roomTypesHtml;
}

var fetchData = () =>{
    hotelId = localStorage.getItem('currentHotel');
    fetch('http://localhost:5058/api/AdminRoom/GetAllRoomTypes',{
        method:'POST',
        headers:{
            'Content-Type' : 'application/json',
            'Authorization':`Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`,
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
        displayRoomTypes(data);
    })
    .catch(error => {
        alert(error);
        console.error(error);
    });
}

document.addEventListener('DOMContentLoaded', function() {
    fetchData();
})