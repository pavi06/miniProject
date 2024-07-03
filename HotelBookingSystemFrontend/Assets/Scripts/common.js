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
    var element = document.getElementById(`${id}`);
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
    console.log(dateElement)
    var today = new Date();
    let formattedDate = `${today.getFullYear()}-${(today.getMonth() + 1).toString().padStart(2, '0')}-${today.getDate().toString().padStart(2, '0')}`;
    console.log(formattedDate)
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
    if(!localStorage.getItem('isLoggedIn')){
        alert("something went wrong......Login properly!");
        //dynamically change value for login !!!!!!!
        window.location.href ="./login.html";
        return;
    }
    startSession();
    checkUserLoggedInOrNot();
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

var checkAdminLoggedInOrNot = () =>{
    if( localStorage.getItem('isLoggedIn')){
        document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('showNav'));        
        document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('hide'));
    }
    else{
        document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('hide'));
        document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('showNav')); 
    }
}


$(document).ready(function(){
    $('.sub-btn').click(function(){
      $(this).next('.sub-menu').slideToggle();
      $(this).find('.dropdown').toggleClass('rotate');
    });

    $('.menu-btn').click(function(){
      $('.side-bar').addClass('active');
      $('.menu-btn').css("visibility", "hidden");
    });

    $('.close-btn').click(function(){
      $('.side-bar').removeClass('active');
      $('.menu-btn').css("visibility", "visible");
    });

    checkAdminLoggedInOrNot();
});






