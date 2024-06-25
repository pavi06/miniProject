document.addEventListener('DOMContentLoaded', function() {
    fetch('http://localhost:5058/api/AdminHotel/GetAllHotels', {
        method: 'GET',
        }).then(async(res) => {
            if (!res.ok) {
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            return await res.json();
        }).then(data => {
            console.log(data);
            displayHotelsRetrieved(data);
        }).catch(error => {
            alert(error);
            console.error(error);
       });
});


var functionAddValidEffects = (element) => {
    var name = element.name;
    element.classList.remove("is-invalid");
    element.classList.add("is-valid");
    document.getElementById(`${name}Valid`).innerHTML="valid input!";
    document.getElementById(`${name}Invalid`).innerHTML="";
    return true;
}

var functionAddInValidEffects = (element) => {
    var name = element.name;
    element.classList.remove("is-valid");
    element.classList.add("is-invalid");
    document.getElementById(`${name}Valid`).innerHTML="";
    document.getElementById(`${name}Invalid`).innerHTML=`Please provide the valid ${name}!`;
    return true;
}

// [A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}
var validateEmail=()=>{
    var element = document.getElementById('email');
    if(element.value){
        return functionAddValidEffects(element, 'email');
    }
    else{
        return functionAddInValidEffects(element, 'email');
    }
}

var validatePassword = () => {
    var regexExpression = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[@$&*_])(?=.*[0-9]).{6,}$/;
    var element = document.getElementById('password');
    if(element.value && element.value.match(regexExpression)){
        return functionAddValidEffects(element);
    }
    else{
        return functionAddInValidEffects(element);
    }
}

var validateAddress=(id)=>{
    var element = document.getElementById(id);
    var addressRegex = /^[a-zA-Z0-9\s,'-]*$/;
    if(element.value && element.value.match(addressRegex)){
        return functionAddValidEffects(element);
    }
    else{
        return functionAddInValidEffects(element);
    }
}

var validate = (id) =>{
    var element = document.getElementById(`${id}`);
    var regString = /[a-zA-Z]/g
    if(element.value && element.value.match(regString)){
        return functionAddValidEffects(element);
    }
    else{
        return functionAddInValidEffects(element);
    }
}

var resetFormValues = (formName) => {
    document.getElementById(formName).reset();
    const formInputs = document.getElementById(formName).querySelectorAll('input, textarea');
    formInputs.forEach(input => {
    //removing the classlist added and empty small element
    input.classList.remove('is-valid', 'is-invalid');
    document.getElementById(`${input.name}Valid`).innerHTML="";
    document.getElementById(`${input.name}Invalid`).innerHTML="";
    });
}

var checkAndRedirectUrlAfterRegistrationOrLogin = () => {
    const redirectUrl = localStorage.getItem('redirectUrl');
    if(!localStorage.getItem('loggedInUser')){
        alert("something went wrong......Login properly!");
        window.location.href ="./login.html";
        return;
    }
    const userRole = JSON.parse(localStorage.getItem('loggedInUser')).role;
    if (redirectUrl) {
        // Clear the stored URL
        localStorage.removeItem('redirectUrl');                    
        // Redirect back to the original page
        window.location.href = redirectUrl;
    } else {
        //if no redirect url goes to the home page
        if(userRole === 'Admin'){
            window.location.href = './AdminIndex.html';
        }
        else{
            window.location.href = './index.html';
        }
    }
}

//method to toggle the dropdown used with filters ui
var toggleDropdown = (id) => {
    var dropdown = document.getElementById(id);
    dropdown.classList.toggle('active');
}

var displayHotelsRetrieved = (data) => {
    var hotelsList ="";
    data.forEach(hotel => {
        hotelsList+=`
            <div class="px-3 pb-5 mb-10 h-auto cardDesign">
                <div class="flex flex-row justify-between">                      
                    <div class="w-60 h-50 mt-4" style="object-fit: cover;">
                        <img src="./Assets/Images/hotelImage5.jpg" alt="HotelImage"/>
                    </div>
                    <div class="p-3">
                        <p class="hotelName">${hotel.name}</p>
                        <a href="#"><i class="bi bi-geo-alt-fill"></i>&nbsp;&nbsp;${hotel.address}</a>
                        <p class="description">Amenities:${hotel.amenities}  <br> Restriction : ${hotel.restrictions}</p>
                    </div>
                    <div class="flex flex-col justify-between mt-3" style="float:right">
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
                        <button type="button" class="buttonStyle" style="width:80%; padding:5px" id="availabilityBtn"><span>Check Room Availability</span></button>
                    </div>
                </div>
            </div> 
        `;
    });
    console.log(document.getElementById("hotelsDisplay"));
    document.getElementById("hotelsDisplay").innerHTML = hotelsList;
}

// var availabilityCheckBtn = document.getElementById('availabilityBtn');
// availabilityCheckBtn.addEventListener("click", function(){
    
// })

var refreshCheckBoxValues = (id)=>{
    // var checkedValues = [];
    // var checkBoxes  = document.querySelectorAll(`#${id}[type="checkbox"]`);
    // checkBoxes.forEach(function(checkbox) {
    //     checkbox.addEventListener('change', function() {
            checkedValues = getCheckedCheckboxes(`#${id}`);
            fetchDataFromServer(checkedValues);
    //     });
    // });
}

var getCheckedCheckboxes = (groupId) => {
    var checkedBoxes = [];
    var groupCheckboxes = document.querySelectorAll(groupId + '[type="checkbox"]');
    groupCheckboxes.forEach(function(cb) {
      if (cb.checked) {
        checkedBoxes.push(cb.value);
      }
    });
    return checkedBoxes;
  }


var fetchDataFromServer = (checkedValues) => {
    fetch('http://localhost:5058/api/GuestBooking/GetHotelsByFeatures', {
        method: 'POST',
        headers: {'Content-Type':'application/json'},
        body:JSON.stringify(checkedValues),
    }).then(async(res) => {
            if (!res.ok) {
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            return await res.json();
    }).then(data => {
            console.log(data);
            displayHotelsRetrieved(data);
    }).catch(error => {
            alert(error);
            console.error(error);
    });
}
// document.addEventListener("DOMContentLoaded", function() {
//     window.addEventListener('scroll', revealOnScroll);
// });

// function revealOnScroll() {
//     var sections = document.querySelectorAll('.sec');
    
//     sections.forEach(function(section) {
//       var sectop = section.getBoundingClientRect().top;
//       var windowHeight = window.innerHeight;
      
//       if (sectop < windowHeight/1.5) {
//         section.classList.add('visible');
//       } else {
//         section.classList.remove('visible');
//       }
//     });
// }
