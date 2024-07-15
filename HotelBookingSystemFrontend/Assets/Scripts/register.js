var validateAndRegister = () => {
    var userData = {
        name: document.getElementById('name').value,
        email: document.getElementById('email').value,
        phoneNumber: document.getElementById('phoneNumber').value,
        address: document.getElementById('address').value,
        password: document.getElementById('password').value
    }
    if(validate('name') && validatePhone() && validateEmail() && validateAddress('address') && validatePassword() && validateConfirmPassword()){
        fetch('http://localhost:5058/api/User/Register', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(userData)
            })
            .then(async (res) => {
            if (!res.ok) {
                if (res.status === 401) {
                    throw new Error('Unauthorized Access!');
                }
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
             //reset form to previous state 
             resetFormValues('registrationForm','input, textarea');
            return await res.json();
          })
          .then( data => {
                var registeredUser = {
                    id:data.guestId,
                    name: data.name,
                    role:data.role
                }
                //storing user in local storage
                localStorage.setItem('loggedInUser', JSON.stringify(registeredUser));
                localStorage.setItem('isLoggedIn',true)
                startSession();
                //check and redirect to the recently viwedpage before register/login
                checkAndRedirectUrlAfterRegistrationOrLogin();
        }).catch( error => {
            addAlert(error.message)
            resetFormValues('registrationForm','input, textarea');
            }
        );
    }
    else{
        addAlert("Not registered successfully! Provide all the values properly!!")
    }
}
