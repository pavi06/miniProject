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
        addAlert(error.message)
    });
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
    console.log(data);
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
        addSuccessAlert('Roomtype updated Successfully!')
        document.querySelector('[data-bs-dismiss="modal"]').click();
        fetchData();
    })
    .catch(error => {
        addAlert(error.message)
    });
}


var displayRoomTypes = (data) =>{
    console.log(data)
    var roomTypesHtml="";
    data.forEach(roomType => {
        var finalAmount = roomType.amount - (roomType.amount * roomType.discount/100);
        var images = roomType.images.split(',')
        console.log(images)
        roomTypesHtml +=`
            <div class="px-3 pb-5 mb-10 h-auto mx-auto cardDesign roomCard">
        <div class="flex flex-column">                      
            <div class="w-80 h-50 mt-10 mx-auto" style="object-fit: cover;">
                <img src="${images[0]}" alt="Image gallery"/>
            </div>
            <div class="p-3 text-sm text-pretty">
              <p class="uppercase fw-bolder mb-3 text-center" style="color: #FFA456;">General Info</p>
              <div class="grid grid-cols-2 py-1">
                <p class="fw-bolder">Roomtype </p>
                <p class="lowercase fw-normal">${roomType.type}</p>
              </div>
              <div class="grid grid-cols-2 py-1">
                <p class="fw-bolder">occupancy </p>
                <p class="lowercase fw-normal">${roomType.occupancy}</p>
              </div>
              <div class="grid grid-cols-2 py-1">
                <p class="fw-bolder">cotsAvailable </p>
                <p class="lowercase fw-normal">${roomType.cotsAvailable}</p>
              </div>
              <div class="grid grid-cols-2 py-1">
                <p class="fw-bolder">amenities </p>
                <p class="lowercase fw-normal">${roomType.amenities}</p>
              </div>
              <p class="uppercase fw-bolder my-3 text-center" style="color: #FFA456;">Pricing</p>
              <div class="grid grid-cols-2 py-1">
                    <p class="fw-bolder">amount </p>
                    <p class="lowercase fw-normal">${roomType.amount}</p>
                  </div>
                  <div class="grid grid-cols-2 py-1">
                    <p class="fw-bolder">Discount </p>
                    <p class="lowercase fw-normal">${roomType.discount}</p>
                  </div>
                  <div class="grid grid-cols-2 py-1">
                    <p class="fw-bolder">Final amount</p>
                    <p class="lowercase fw-normal ">${finalAmount}</p>
                  </div>
            </div>
        </div>
        <div class="flex flex-row flex-wrap px-3 mt-3 justify-center">
            <button type="button" class="buttonStyle mr-5" id="editHotelBtn" onclick="openModalForRoomTypeEdit(${roomType.roomTypeId}, ${roomType.hotelId})"><span>Edit RoomType</span></button>
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
        addAlert(error.message)
    });
}

document.addEventListener('DOMContentLoaded', function() {
    checkUserLoggedInOrNot();
    fetchData();
})