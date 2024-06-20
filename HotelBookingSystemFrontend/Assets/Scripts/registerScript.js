var functionAddValidEffects = (element, attributeName) => {
    element.classList.remove("is-invalid");
    element.classList.add("is-valid");
    document.getElementById(`${attributeName}Valid`).innerHTML="valid input!";
    document.getElementById(`${attributeName}Invalid`).innerHTML="";
    return true;
}

var functionAddInValidEffects = (element, attributeName) => {
    element.classList.remove("is-valid");
    element.classList.add("is-invalid");
    document.getElementById(`${attributeName}Valid`).innerHTML="";
    document.getElementById(`${attributeName}Invalid`).innerHTML=`Please provide the valid ${attributeName}!`;
    return true;
}

var validateName = () =>{
    var element = document.registrationForm.name;
    var regString = /[a-zA-Z]/g
    if(element.value && element.value.match(regString)){
        return functionAddValidEffects(element, 'name');
    }
    else{
        return functionAddInValidEffects(element, 'name');
    }
}

function validatePhone(){
    var element = document.registrationForm.phoneNumber;
    var regPhone =  /^[+]{1}(?:[0-9\-\\(\\)\\/.]\s?){6,15}[0-9]{1}$/;
    if(element.value && element.value.match(regPhone)){
        return functionAddValidEffects(element, 'phone');
    }
    else{
        return functionAddInValidEffects(element, 'phone');
    }
}
// [A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}
var validateEmail=()=>{
    var element = document.registrationForm.email;
    if(element.value){
        return functionAddValidEffects(element, 'email');
    }
    else{
        return functionAddInValidEffects(element, 'email');
    }
}

var validateAddress=()=>{
    var element = document.registrationForm.address;
    var addressRegex = /^[a-zA-Z0-9\s,'-]*$/;
    if(element.value && element.value.match(addressRegex)){
        return functionAddValidEffects(element, 'address');
    }
    else{
        return functionAddInValidEffects(element, 'address');
    }
}

var validatePassword = () => {
    var regexExpression = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[@$&*_])(?=.*[0-9]).{6,}$/;
    var element = document.registrationForm.password;
    if(element.value && element.value.match(regexExpression)){
        return functionAddValidEffects(element, 'password');
    }
    else{
        return functionAddInValidEffects(element, 'password');
    }
}

var validateConfirmPassword = () => {
    var element = document.registrationForm.confirmPassword;
    var element2 = document.registrationForm.password;
    if(element.value && element.value === element2.value){
        return functionAddValidEffects(element, 'confPassword');
    }
    else{
        return functionAddInValidEffects(element, 'confPassword');
    }
}


var validateAndRegister = () => {
    var userData = {
        name: document.getElementById('name').value,
        email: document.getElementById('email').value,
        phoneNumber: document.getElementById('phoneNumber').value,
        address: document.getElementById('address').value,
        password: document.getElementById('password').value
    }
    console.log(userData);
    if(validateName() && validatePhone() && validateEmail() && validateAddress() && validatePassword() && validateConfirmPassword()){
        fetch('http://localhost:5058/api/User/Register', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(userData)
          })
          .then(async (res) => {
            if (!res.ok) {
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            return await res.json();
          })
          .then( data => {
                console.log("registered successfully");
                alert(data);
                console.log(data);
                window.location.href = './index.html';
        }).catch( error => alert(error)
        );
    }
    else{
        alert("Not registered successfully!Provide all the values properly!!");
    }
}