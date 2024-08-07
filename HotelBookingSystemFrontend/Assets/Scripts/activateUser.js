var displayUser = (data) => {
    if(data.length === 0){
        document.getElementById('displayUsers').innerHTML = `
          <div class="px-3 py-5 mb-10 m-auto text-2xl uppercase text-center" style="width:50%;border:1px solid #FFA456; color:#FFA456; align-items: center;"><b>No Users for activation!</b></div>
        `;
        return ;
    }
    var usersHtml = "";
    data.forEach(user => {
        usersHtml+=`
            <div class="px-3 pb-3 mb-10 h-auto mx-auto cardDesign">
                <div class="flex flex-row justify-between">                      
                    <div class="p-3 mx-auto">
                        <p class="uppercase fw-bolder mb-3 text-center text-2xl" style="color: #FFA456;">User</p>
                        <div class="grid grid-cols-2">
                            <p class="uppercase fw-bolder text-lg">Guest Id</p>
                            <p class="lowercase fw-normal px-3 text-lg">${user.guestId}</p>
                        </div>
                        <div class="grid grid-cols-2">
                            <p class="uppercase fw-bolder text-lg">Email</p>
                            <p class="lowercase fw-normal px-3 text-lg">${user.email}</p>
                        </div>
                        <div class="grid grid-cols-2">
                            <p class="uppercase fw-bolder text-lg">PhoneNo.</p>
                            <p class="lowercase fw-normal px-3 text-lg">${user.phoneNumber}</p>
                        </div>
                        <div class="grid grid-cols-2">
                            <p class="uppercase fw-bolder text-lg">Role</p>
                            <p class="lowercase fw-normal px-3 text-lg">${user.role}</p>
                        </div>
                    </div>
                </div>
                <div class="flex flex-row flex-wrap px-3 mt-3 justify-center align-middle">
                    <button type="button" class="buttonStyle mr-5" id="activateUserBtn" onclick="updateUserStatus(${user.guestId},'Active')"><span>Activate User</span></button>
                    <button type="button" class="buttonStyle" id="disableUserBtn" onclick="updateUserStatus(${user.guestId}, 'Disabled')"><span>Disable User</span></button>
                </div>
            </div>
        `;
    });
    document.getElementById('displayUsers').innerHTML = usersHtml;
}

var updateUserStatus = async (guestId, status) => {
    var res = await checkTokenAboutToExpiry(JSON.parse(localStorage.getItem('loggedInUser')).accessToken);
    if (res === "Refresh") {
        await refreshToken();
    } else if (res === "Invalid accessToken!") {
        addAlert("Invalid AccessToken!");
        return;
    }
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
            if (res.status === 401) {
                throw new Error('Unauthorized Access!');
            }
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    })
    .then(data => {
        addSuccessAlert(`User ${status} successfullly`)
        fetchUsersForActivation();
    })
    .catch(error => {
        addAlert(error.message)
        if(error.message === "Unauthorized Access!"){
            adminLogOut();
        }
    });
}

var fetchUsersForActivation = async () =>{
    var res = await checkTokenAboutToExpiry(JSON.parse(localStorage.getItem('loggedInUser')).accessToken);
    if (res === "Refresh") {
        await refreshToken();
    } else if (res === "Invalid accessToken!") {
        addAlert("Invalid AccessToken!");
        return;
    }
    fetch('http://localhost:5058/api/AdminBasic/GetAllUsersForActivaion',{
        method:'GET',
        headers:{
            'Content-Type' : 'application/json',
            'Authorization':`Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`,
        },
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
        displayUser(data);
    })
    .catch(error => {
        addAlert(error.message)
        if(error.message === "Unauthorized Access!"){
            adminLogOut();
        }
    });
}  

document.addEventListener('DOMContentLoaded', function(){
    checkAdminLoggedInOrNot();
    dropDown();
    fetchUsersForActivation();
})

