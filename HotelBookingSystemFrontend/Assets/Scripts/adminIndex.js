document.addEventListener('DOMContentLoaded', function(){
    fetch('http://localhost:5058/api/AdminHotel/GetAppDetails', {
        method: 'GET',
        headers:{
            'Content-Type':'application/json',
            'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`
        }
    })
    .then(async(res) => {
        if (!res.ok) {
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    })
    .then(data => {
        document.getElementById('detailsDiv').innerHTML=`
            <div class="flex flex-column mx-auto p-3" style="width: 80%;">
                    <div class="my-2 fw-bolder flex flex-row justify-between">
                        <p>Hotels Available</p><span class="fw-normal">${data.totalNoOfHotelsAvailable} hotels</span>
                    </div>
                    <div class="my-2 fw-bolder flex flex-row justify-between"><p>Employees</p><span class="fw-normal">${data.totalNoOfEmployees} employees</span></div>
                    <div class="my-2 fw-bolder flex flex-row justify-between"><p>Users</p><span class="fw-normal">${data.totalNoOfUsers} users</span></div>              
                    <h2 class="uppercase m-auto fw-bolder text-orange-300 text-2xl mb-5 mt-2">Analytics</h2>
                    <div>
                        <div class="my-2 fw-bolder flex flex-row justify-between"><p>Average Usage Time</p><span class="fw-normal">${data.averageUsageTime}hrs</span></div>
                        <div class="my-2 fw-bolder flex flex-row justify-between"><p>Average bookings per month</p><span class="fw-normal">${data.averageBookingPerMonth}bookings</span></div>
                    </div>              
            </div>
        `;
        console.log(data);
    })
    .catch(error => {
        alert(error);
        console.error(error);
    });
})

