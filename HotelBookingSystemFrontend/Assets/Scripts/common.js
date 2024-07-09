// validation styles
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

// validation
// [A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}
function validateEmail(){
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
    var element = document.getElementById(id);
    var regString = /[a-zA-Z]/g
    if(element.value && element.value.match(regString)){
        return functionAddValidEffects(element);
    }
    else{
        return functionAddInValidEffects(element);
    }
}

var validateDate = () =>{
    var dateElement = document.getElementById('checkInDate');
    var today = new Date();
    let formattedDate = `${today.getFullYear()}-${(today.getMonth() + 1).toString().padStart(2, '0')}-${today.getDate().toString().padStart(2, '0')}`;
    if(dateElement.value && Date.parse(dateElement.value) >= Date.parse(formattedDate)) {
        functionAddValidEffects(dateElement);
        return true;
    } else {
        functionAddInValidEffects(dateElement);
        return false;
    }
}

var validateNumber = (id) =>{
    var data = document.getElementById(`${id}`);
    var reg = /^\d+$/;
    if(data.value.match(reg)){
        return functionAddValidEffects(data);
    }
    else{
        return functionAddInValidEffects(data);
    }
}

var validateDecimalNumber = (id) =>{
    var data = document.getElementById(`${id}`);
    var pattern = /^[+]?\d+(\.\d*)?$/;
    if(data.value.match(pattern)){
        return functionAddValidEffects(data);
    }
    else{
        return functionAddInValidEffects(data);
    }
}

var validateLocation = () =>{
    var locationElement = document.getElementById('location');
    var reg=/^[a-zA-Z]+$/;
    if(locationElement.value && locationElement.value.match(reg)){
        functionAddValidEffects(locationElement);
        return true;
    }
    else{
        functionAddInValidEffects(locationElement);
        return false;
    }
}

function validatePhone(){
    var element = document.registrationForm.phoneNumber;
    var regPhone =  /^[+]{1}(?:[0-9\-\\(\\)\\/.]\s?){6,15}[0-9]{1}$/;
    if(element.value && element.value.match(regPhone)){
        return functionAddValidEffects(element);
    }
    else{
        return functionAddInValidEffects(element);
    }
}

var validateConfirmPassword = () => {
    var element = document.registrationForm.confirmPassword;
    var element2 = document.registrationForm.password;
    if(element.value && element.value === element2.value){
        return functionAddValidEffects(element);
    }
    else{
        return functionAddInValidEffects(element);
    }
}

var resetFormValues = (formName, formTypes) => {
    document.getElementById(formName).reset();
    const formInputs = document.getElementById(formName).querySelectorAll(formTypes);
    formInputs.forEach(input => {
    //removing the classlist added and empty small element
    input.classList.remove('is-valid', 'is-invalid');
    document.getElementById(`${input.name}Valid`).innerHTML="";
    document.getElementById(`${input.name}Invalid`).innerHTML="";
    });
}

var checkUserLoggedInOrNot = () =>{
    if( localStorage.getItem('isLoggedIn')){
        if(window.location.pathname === '/Templates/login.html'){
            return;
        }
        document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('showNav'));
        var cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
        if(document.querySelector('.bookRooms')){
            if(cartItems.length === 0){
                document.querySelector('.bookRooms').classList.add('hide');
            }else{
                document.querySelector('.bookRooms').classList.add('show');
            }
        }
        document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('hide'));
    }
    else{
        document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('hide'));
        document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('showNav')); 
    }
}

var logOut = () => {
    localStorage.clear();
    if(window.location.pathname === '/Templates/UserTemplate/myBookings.html' || '/Templates/UserTemplate/booking.html'){
        window.location.href = '/Templates/hotels.html';
    }
    document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('show'));
    document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('hide'));

};


//redirectionafterloginorregister
var checkAndRedirectUrlAfterRegistrationOrLogin = () => {
    const redirectUrl = localStorage.getItem('redirectUrl');
    console.log(redirectUrl)
    if(!localStorage.getItem('isLoggedIn')){
        console.log("inside isloggedin error")
        addAlert("something went wrong......Login properly!");
        window.location.href ="./login.html";
        return;
    }
    startSession();
    // checkUserLoggedInOrNot();
    const userRole = JSON.parse(localStorage.getItem('loggedInUser')).role;
    console.log(userRole)
    if (redirectUrl) {
        // Clear the stored URL
        localStorage.removeItem('redirectUrl');                    
        // Redirect back to the original page
        window.location.href = redirectUrl;
    } else {
        console.log("inside else")
        //if no redirect url goes to the home page
        if(userRole === 'Admin'){
            window.location.href = './AdminTemplate/AdminIndex.html';
        }
        else{
            console.log("redirectingggg")
            window.location.href = './index.html';
        }
    }
}

// var displayToast = (message,toastStatus) =>{
//     document.getElementById('toastMsg').innerHTML= message;
//     document.getElementById('toastStatus').innerHTML = toastStatus
//     if(toastStatus === 'sucess'){
//         document.getElementById('toastStatus').innerHTML = 'SUCCESS';
//         document.getElementById('toastStatus').style.color = '#FFA456';
//     }else{
//         document.getElementById('toastStatus').innerHTML = 'FAILED';
//         document.getElementById('toastStatus').style.color = 'black';
//     }
//     console.log(document.querySelector(".toast"))
//     console.log(document.querySelector(".progress"))
//     document.querySelector(".toast").classList.add("activeToast");
//     document.querySelector(".progress").classList.add("activeToast");
//     let timer1;
//     let timer2;
//     timer1 = setTimeout(() =>{
//         document.querySelector(".toast").classList.remove("activeToast");
//     },4000);

//     timer2 = setTimeout(() =>{
//         document.querySelector(".progress").classList.remove("activeToast");
//     },4300);
// }

// function closeToast(){
//     document.querySelector(".toast").classList.remove("active");
//     progress.classList.remove("active");
// }


//add alert modal at the end of the body 

if(document.querySelector(".modalCloseBtn")){
    document.querySelector(".modalCloseBtn").addEventListener("click", () =>
        document.getElementById('alertModal').classList.remove("active")
    );
}


var addAlert = (message) =>{
    if(document.getElementById('alertModal')){
        document.getElementById('alertMessage').innerHTML = message;
        const modal = new bootstrap.Modal(document.getElementById('alertModal'));
        modal.show();
        return;
    }
    const alert = document.createElement('div')
    alert.innerHTML = `
         <div class="modal" id="alertModal" style="border-radius:50px">
            <div class="modal-dialog">
                <div class="modal-content">
                <div class="modal-header bg-red-400">
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <img class="flex mx-auto" src="../../Assets/Images/error.png" style="width:40%; height:40%;"/>
                <h5 class="text-2xl mt-0" style="font-weight:bolder;text-transform:uppercase;text-align:center;color:red;">Oops!</h5>
                <div class="modal-body text-center">
                    <p class="text-xl text-black" id="alertMessage">${message}</p>
                </div>
                <button type="button" class="btn uppercase w-25 text-center mx-auto my-3 bg-red-400  fw-bolder alertBtn" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    `;
    document.body.insertAdjacentElement('beforeend', alert);
    const modal = new bootstrap.Modal(document.getElementById('alertModal'));
    modal.show();
    if(message === '404 Error! - No hotels are available!' || message === "No more hotels available!"){
        if( document.getElementById('loadBtn')){
            document.getElementById('loadBtn').classList.add('hide');
        }
        if(document.getElementById('loadBtnWithBody')){
            document.getElementById('loadBtnWithBody').classList.add('hide');
        }
    }
    return;
}

if(document.querySelector(".modalCloseBtn")){
    document.querySelector(".modalCloseBtn").addEventListener("click", () =>
        document.getElementById('alertModal').classList.remove("active")
    );
}


var addSuccessAlert = (message) =>{
    if(document.getElementById('successAlertModal')){
        document.getElementById('successAlertModal').innerHTML = message;
        const modal = new bootstrap.Modal(document.getElementById('successAlertModal'));
        modal.show();
        return;
    }
    const alert = document.createElement('div')
    alert.innerHTML = `
         <div class="modal" id="successAlertModal">
            <div class="modal-dialog">
                <div class="modal-content">
                <div class="modal-header bg-green-400">
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <img class="flex mx-auto" src="../../Assets/Images/success.png" style="width:40%; height:40%;"/>
                <h5 class="text-2xl mt-0" style="font-weight:bolder;text-transform:uppercase;text-align:center;color:green;">SUCCESS</h5>
                <div class="modal-body text-center">
                    <p class="text-xl text-black" id="successAlertMessage">${message}</p>
                </div>
                <button type="button" class="btn uppercase w-25 text-center mx-auto my-3 bg-green-400  fw-bolder alertBtn" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    `;
    document.body.insertAdjacentElement('beforeend', alert);
    const modal = new bootstrap.Modal(document.getElementById('successAlertModal'));
    modal.show();
    if(message.includes('No hotels are available!') || message === "No more hotels available!"){
        if( document.getElementById('loadBtn')){
            document.getElementById('loadBtn').classList.add('hide');
        }
        if(document.getElementById('loadBtnWithBody')){
            document.getElementById('loadBtnWithBody').classList.add('hide');
        }
    }
}