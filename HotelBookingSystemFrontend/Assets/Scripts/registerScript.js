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


var validateAndRegister = () => {
    var userData = {
        name: document.getElementById('name').value,
        email: document.getElementById('email').value,
        phoneNumber: document.getElementById('phoneNumber').value,
        address: document.getElementById('address').value,
        password: document.getElementById('password').value
    }
    if(validateName() && validatePhone() && validateEmail() && validateAddress() && validatePassword() && validateConfirmPassword()){
        fetch('http://localhost:5058/api/User/Register', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(userData)
            })
            .then(async (res) => {
            //reset form to previous state 
            resetFormValues('registrationForm');
            if (!res.ok) {
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            return await res.json();
          })
          .then( data => {
                var registeredUser = {
                    id:data.guestId,
                    name: data.name,
                    role:data.role
                }
                //storing user in local storage
                localStorage.setItem('registeredUser', registeredUser);
                console.log(localStorage.getItem('registeredUser'));
                console.log("registered successfully");
                alert(data);
                console.log(data);
                //check and redirect to the recently viwedpage before register/login
                checkAndRedirectUrlAfterRegistrationOrLogin();
        }).catch( error => {
            alert("Oops!Server error! try again later!");
            resetFormValues('registrationForm');
            }
        );
    }
    else{
        alert("Not registered successfully!Provide all the values properly!!");
    }
}
