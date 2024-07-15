function validateAndLogin(){
    var userData = {
        email: document.getElementById('email').value,
        password: document.getElementById('password').value
    }
    if(validateEmail() && validatePassword()){
        fetch('http://localhost:5058/api/User/Login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(userData)
        })
        .then(async(res) => {
            if (!res.ok) {
                if (res.status === 401) {
                    throw new Error('Unauthorized Access!');
                }
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            resetFormValues('loginForm');
            return res.json();
        })
        .then( data => {
                localStorage.setItem('loggedInUser',JSON.stringify(data));
                localStorage.setItem('isLoggedIn',true)
                startSession();
                //check and redirect to the page most recently viwed
                checkAndRedirectUrlAfterRegistrationOrLogin();
        }).catch( error => {
            addAlert(error.message)
            resetFormValues('loginForm','input');
            }
        );
    }
    else{
        addAlert("login failed! Try again Later!!");
    }
}