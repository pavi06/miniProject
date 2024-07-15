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
            if (res.status === 401) {
                throw new Error('Unauthorized Access!');
            }
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
        if (error.message === 'Unauthorized Access!') {
            addAlert("Unauthorized Access!");
        } else {
            addAlert(error.message);
        }
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
    const attributeValues = {}
    var count=0;
    Array.from(data).forEach(ele => {
        if(ele.value){
            attributeValues[ele.name] = ele.value;
        }
        else{
            count+=1
        }
    });
    if(count === data.length){
        addAlert("Provide some value to update!")
        return;
    }
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
            if (res.status === 401) {
                throw new Error('Unauthorized Access!');
            }
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
        document.querySelector('[data-bs-dismiss="modal"]').click();
        if (error.message === 'Unauthorized Access!') {
            addAlert("Unauthorized Access!");
        } else {
            addAlert(error.message);
        }
    });
}


var displayRoomTypes = (data) =>{
    var roomTypesHtml="";
    data.forEach(roomType => {
        var finalAmount = roomType.amount - (roomType.amount * roomType.discount/100);
        var images = roomType.images.split(',')
        roomTypesHtml +=`
            <div class="px-3 pb-5 mb-10 h-auto mx-auto cardDesign roomCard">
        <div class="flex flex-column">   
            <div class="mt-10 mx-auto" style="width:100%;height:20vw"><img src="${images[0]}" class="images" style="width: 100%;height: 100%;object-fit:cover;" loading="lazy"/></div>                   
            <div class="p-3 text-sm text-pretty">
              <p class="uppercase fw-bolder mb-3 text-center" style="color: #FFA456;">General Info</p>
              <div class="grid grid-cols-2 gap-7 py-1">
                <p class="fw-bolder">Roomtype </p>
                <p class="lowercase fw-normal" style="overflow-wrap: break-word;">${roomType.type}</p>
              </div>
              <div class="grid grid-cols-2 gap-7 py-1">
                <p class="fw-bolder">occupancy </p>
                <p class="lowercase fw-normal" style="overflow-wrap: break-word;">${roomType.occupancy}</p>
              </div>
              <div class="grid grid-cols-2 gap-7 py-1">
                <p class="fw-bolder">cotsAvailable </p>
                <p class="lowercase fw-normal" style="overflow-wrap: break-word;">${roomType.cotsAvailable}</p>
              </div>
              <div class="grid grid-cols-2 gap-7 py-1">
                <p class="fw-bolder">amenities </p>
                <p class="lowercase fw-normal" style="overflow-wrap: break-word;">${roomType.amenities}</p>
              </div>
              <p class="uppercase fw-bolder my-3 text-center" style="color: #FFA456;">Pricing</p>
              <div class="grid grid-cols-2 gap-7 py-1">
                    <p class="fw-bolder">amount </p>
                    <p class="lowercase fw-normal" style="overflow-wrap: break-word;">${roomType.amount}</p>
                  </div>
                  <div class="grid grid-cols-2 gap-7 py-1">
                    <p class="fw-bolder">Discount </p>
                    <p class="lowercase fw-normal" style="overflow-wrap: break-word;">${roomType.discount}</p>
                  </div>
                  <div class="grid grid-cols-2 gap-7 py-1">
                    <p class="fw-bolder">Final amount</p>
                    <p class="lowercase fw-normal " style="overflow-wrap: break-word;">${finalAmount}</p>
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
            if (res.status === 401) {
                throw new Error('Unauthorized Access!');
            }
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    })
    .then(data => {
        displayRoomTypes(data);
    })
    .catch(error => {
        if (error.message === 'Unauthorized Access!') {
            addAlert("Unauthorized Access!");
        } else {
            addAlert(error.message);
        }
    });
}

document.addEventListener('DOMContentLoaded', function() {
    checkUserLoggedInOrNot();
    fetchData();
})