// validation styles
function functionAddValidEffects(element){
    var name = element.name;
    element.classList.remove("is-invalid");
    element.classList.add("is-valid");
    document.getElementById(`${name}Valid`).innerHTML="valid input!";
    document.getElementById(`${name}Invalid`).innerHTML="";
    return true;
}

function functionAddInValidEffects(element){
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

function validatePassword() {
    var regexExpression = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[@$&*_])(?=.*[0-9]).{6,}$/;
    var element = document.getElementById('password');
    if(element.value && element.value.match(regexExpression)){
        return functionAddValidEffects(element);
    }
    else{
        return functionAddInValidEffects(element);
    }
}

function validateAddress(id){
    var element = document.getElementById(id);
    var addressRegex = /^[a-zA-Z0-9\s,'-]*$/;
    if(element.value && element.value.match(addressRegex)){
        return functionAddValidEffects(element);
    }
    else{
        return functionAddInValidEffects(element);
    }
}

function validate(id){
    var element = document.getElementById(id);
    var regString = /[a-zA-Z]/g
    if(element.value && element.value.match(regString)){
        return functionAddValidEffects(element);
    }
    else{
        return functionAddInValidEffects(element);
    }
}

function validateDate(){
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

function validateNumber(id){
    var data = document.getElementById(`${id}`);
    var reg = /^\d+$/;
    if(data.value.match(reg)){
        return functionAddValidEffects(data);
    }
    else{
        return functionAddInValidEffects(data);
    }
}

function validateDecimalNumber(id){
    var data = document.getElementById(`${id}`);
    var pattern = /^[+]?\d+(\.\d*)?$/;
    if(data.value.match(pattern)){
        return functionAddValidEffects(data);
    }
    else{
        return functionAddInValidEffects(data);
    }
}

function validateLocation(){
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

function validateConfirmPassword(){
    var element = document.registrationForm.confirmPassword;
    var element2 = document.registrationForm.password;
    if(element.value && element.value === element2.value){
        return functionAddValidEffects(element);
    }
    else{
        return functionAddInValidEffects(element);
    }
}

function resetFormValues(formName, formTypes){
    document.getElementById(formName).reset();
    const formInputs = document.getElementById(formName).querySelectorAll(formTypes);
    formInputs.forEach(input => {
    //removing the classlist added and empty small element
    input.classList.remove('is-valid', 'is-invalid');
    document.getElementById(`${input.name}Valid`).innerHTML="";
    document.getElementById(`${input.name}Invalid`).innerHTML="";
    });
}

// ---------------login-logout--------------
function checkUserLoggedInOrNot(){
    if( localStorage.getItem('isLoggedIn')){
        // if(window.location.pathname === '/Templates/login.html'){
        //     return;
        // }
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
        document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('show')); 
    }
}

function logOut(){
    localStorage.clear();
    if(window.location.pathname === '/Templates/UserTemplate/myBookings.html' || '/Templates/UserTemplate/booking.html'){
        window.location.href = '/Templates/hotels.html';
    }
    document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('show'));
    document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('hide'));

};
// -----------------------------------------------


//redirectionafterloginorregister
function checkAndRedirectUrlAfterRegistrationOrLogin(){
    const redirectUrl = localStorage.getItem('redirectUrl');
    if(!localStorage.getItem('isLoggedIn')){
        addAlert("something went wrong......Login properly!");
        window.location.href ="./login.html";
        return;
    }
    startSession();
    // checkUserLoggedInOrNot();
    const userRole = JSON.parse(localStorage.getItem('loggedInUser')).role;
    if (redirectUrl) {
        // Clear the stored URL
        localStorage.removeItem('redirectUrl');                    
        // Redirect back to the original page
        window.location.href = redirectUrl;
    } else {
        //if no redirect url goes to the home page
        if(userRole === 'Admin'){
            window.location.href = './AdminTemplate/AdminIndex.html';
        }
        else{
            window.location.href = './index.html';
        }
    }
}


// ------------Alerts-------------------
//add alert modal at the end of the body 
if(document.querySelector(".modalCloseBtn")){
    document.querySelector(".modalCloseBtn").addEventListener("click", () =>
        document.getElementById('alertModal').classList.remove("active")
    );
}

function addAlert(message){
    if(document.getElementById('alertModal')){
        document.getElementById('alertMessage').innerHTML = message;
        const modal = new bootstrap.Modal(document.getElementById('alertModal'));
        modal.show();
        return;
    }
    const alert = document.createElement('div')
    alert.innerHTML = `
         <div class="modal" id="alertModal">
            <div class="modal-dialog modal-dialog-centered w-100">
                <div class="modal-content" style="border-radius:25px">
                <div class="modal-header bg-red-400" style="border-bottom:none;height:5px;  border-top-left-radius: 25px;border-top-right-radius: 25px;">
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <img class="flex mx-auto mt-3" src="https://drive.google.com/thumbnail?id=11m7WDS-tg0HDD-8g7UzwThOlosJHdyGq&sz=w1000" style="width:20%; height:20%;"/>
                <h5 class="text-2xl mt-0" style="font-weight:bolder;text-transform:uppercase;text-align:center;color:red;">ERROR!</h5>
                <div class="modal-body text-center mt-0 pt-0">
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

function addSuccessAlert(message){
    if(document.getElementById('successAlertModal')){
        document.getElementById('successAlertMessage').innerHTML = message;
        const modal = new bootstrap.Modal(document.getElementById('successAlertModal'));
        modal.show();
        return;
    }
    const alert = document.createElement('div')
    alert.innerHTML = `
         <div class="modal" id="successAlertModal">
            <div class="modal-dialog modal-dialog-centered w-100">
                <div class="modal-content" style="border-radius:25px">
                <div class="modal-header bg-green-400" style="border-bottom:none;height:5px;  border-top-left-radius: 25px;border-top-right-radius: 25px;">
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" style="color:white"></button>
                </div>
                <img class="flex mx-auto" src="https://drive.google.com/thumbnail?id=1vp0UJO_D0qHjHvWetNMZm4BYAtYrnbNj&sz=w1000" style="width:20%; height:20%;"/>
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
}

function menu(element){
    let list = document.querySelector('ul');
    element.name === 'menu' ? (element.name = "close", list.classList.add('top-[80px]') , list.classList.add('opacity-100')) :( element.name = "menu" ,list.classList.remove('top-[80px]'), list.classList.remove('opacity-100'))
}

// ---------preloader----------
window.addEventListener("load", () => {
    const loader = document.querySelector(".loader");
  
    loader.classList.add("loader--hidden");
  
    loader.addEventListener("transitionend", () => {
      document.body.removeChild(loader);
    });
});
