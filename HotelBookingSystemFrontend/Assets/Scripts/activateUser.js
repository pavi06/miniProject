var displayUser = (data) => {
    var usersHtml = "";
    data.forEach(user => {
        usersHtml+=`
             <div class="px-3 pb-5 mb-10 h-auto mx-auto cardDesign" style="width: 60%;">
                <div class="flex flex-row justify-between">                      
                    <div class="p-3">
                        <p class="uppercase fw-bolder mb-3" style="color: #FFA456;font-size: 30px;">User</p>
                        <div class="grid grid-cols-2">
                            <p class="uppercase fw-bolder">Guest Id</p>
                            <p class="lowercase fw-normal px-3" style="font-size:25px;">${user.guestId}</p>
                        </div>
                        <div class="grid grid-cols-2">
                            <p class="uppercase fw-bolder">Email</p>
                            <p class="lowercase fw-normal px-3" style="font-size:25px;">${user.email}</p>
                        </div>
                        <div class="grid grid-cols-2">
                            <p class="uppercase fw-bolder">PhoneNumber</p>
                            <p class="lowercase fw-normal px-3" style="font-size:25px;">${user.phoneNumber}</p>
                        </div>
                        <div class="grid grid-cols-2">
                            <p class="uppercase fw-bolder">Role</p>
                            <p class="lowercase fw-normal px-3" style="font-size:25px;">${user.role}</p>
                        </div>
                    </div>
                </div>
                <div class="flex flex-row flex-wrap px-3 mt-3 justify-center">
                    <button type="button" class="buttonStyle mr-5" style="width:20%; padding:5px" id="activateUserBtn" onclick="updateUserStatus(${user.guestId},'Active')"><span>Activate User</span></button>
                    <button type="button" class="buttonStyle mr-5" style="width:20%; padding:5px" id="disableUserBtn" onclick="updateUserStatus(${user.guestId}, 'Disabled')"><span>Disable User</span></button>
                </div>
            </div>
        `;
    });
    document.getElementById('displayUsers').innerHTML = usersHtml;
}

var updateUserStatus = (guestId, status) => {
    fetch('http://localhost:5058/api/AdminBasic/UpdateUserStatus',{
        method:'PUT',
        headers:{
            'Content-Type' : 'application/json',
            'Authorization':`Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`,
        },
        body: JSON.stringify({
            guestId:guestId,
            status : status
        })
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    })
    .then(data => {
        alert(`User ${status} successfullly`)
        fetchUsersForActivation();
    })
    .catch(error => {
        alert(error);
    });
}

var fetchUsersForActivation = () =>{
    fetch('http://localhost:5058/api/AdminBasic/GetAllUsersForActivaion',{
        method:'GET',
        headers:{
            'Content-Type' : 'application/json',
            'Authorization':`Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`,
        },
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    })
    .then(data => {
        displayUser(data);
    })
    .catch(error => {
        alert(error);
    });
}

document.addEventListener('DOMContentLoaded', function(){
    fetchUsersForActivation();
})