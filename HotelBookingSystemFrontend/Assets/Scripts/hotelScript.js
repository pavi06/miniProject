var validateLocation = () =>{
    var locationElement = document.getElementById("location");
    var reg=/^[a-zA-Z]+$/;
    if(locationElement.value && locationElement.value.match(reg)){
        locationElement.classList.remove("is-invalid");
        locationElement.classList.add("is-valid");
        document.getElementById("locationValid").innerHTML="valid input!";
        document.getElementById("locationInValid").innerHTML="";
        return true;
    }
    else{
        locationElement.classList.remove("is-valid");
        locationElement.classList.add("is-invalid");
        document.getElementById("locationValid").innerHTML="";
        document.getElementById("locationInValid").innerHTML="Please provide the valid location!";
        return false;
    }
}

var validateDate = () =>{
    var dateElement = document.getElementById("checkInDate");
    var today = new Date();
    let formattedDate = `${today.getFullYear()}/${today.getMonth() + 1}/${today.getDate()}`;
    if(dateElement.value.valueOf() && dateElement.value.valueOf()>= formattedDate.valueOf()){
        dateElement.classList.remove("is-invalid");
        dateElement.classList.add("is-valid");
        document.getElementById("checkInDateValid").innerHTML="valid input!";
        document.getElementById("checkInDateInValid").innerHTML="";
        return true;
    }
    else{
        dateElement.classList.remove("is-valid");
        dateElement.classList.add("is-invalid");
        document.getElementById("checkInDateValid").innerHTML="";
        document.getElementById("checkInDateInValid").innerHTML="Please provide the valid date!";
        return false;
    }
}

var validateAndGet = () => {
    if(validateLocation() && validateDate()){
        console.log("Inside api");
        fetch('http://localhost:5058/api/GuestBooking/GetHotelsByLocationAndDate', {
            method: 'GET',
            }).then(response => {
               return response.json();
            }).then(data => {
                console.log(data);
            }).catch(error => {
                console.error(error);
           });
    }
}

{/* <div class="px-3 pb-5 mb-10 h-auto cardDesign">
                    <div class="flex flex-row justify-between">                      
                        <div class="w-60 h-50 mt-4" style="object-fit: cover;">
                            <img src="./Assets/Images/hotelImage5.jpg" alt="HotelImage"/>
                        </div>
                        <div class="p-3">
                            <p class="hotelName">Mr. Shivam Residency</p>
                            <a href="#"><i class="bi bi-geo-alt-fill"></i>&nbsp;&nbsp;Ooty, Tamil Nadu, India</a>
                            <p class="description">Amenities: Wifi, Parking, Laundry service,  <br> Restriction : No cancellation, No smoke and alcohol</p>
                        </div>
                        <div class="flex flex-col justify-between mt-3">
                            <div>
                                <p><span class="review">Review Score</span></p>
                                <div>
                                    <span class="fa fa-star checked"></span>
                                    <span class="fa fa-star checked"></span>
                                    <span class="fa fa-star checked"></span>
                                    <span class="fa fa-star checked"></span>
                                    <span class="fa fa-star"></span>
                                </div>
                                <p><a href="#">10 reviews</a></p>
                            </div>
                            <button type="button" class="buttonStyle">Check Room Availability</button>
                        </div>
                    </div>
                </div> */}