var validateAndLogin = () => {
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
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            resetFormValues('loginForm');
            return res.json();
        })
        .then( data => {
                localStorage.setItem('loggedInUser',JSON.stringify(data));
                localStorage.setItem('isLoggedIn',true)
                alert("Login successfull");
                startSession();
                //check and redirect to the page most recently viwed
                checkAndRedirectUrlAfterRegistrationOrLogin();
        }).catch( error => {
            alert(error);
            resetFormValues('loginForm');
            }
        );
    }
    else{
        alert("login failed! Try again Later!!");
    }
}