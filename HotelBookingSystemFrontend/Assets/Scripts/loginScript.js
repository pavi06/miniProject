var validateAndLogin = () => {
    var userData = {
        email: document.getElementById('email').value,
        password: document.getElementById('password').value
    }
    console.log(userData);
    if(validateEmail() && validatePassword()){
        fetch('http://localhost:5058/api/User/Login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(userData)
          })
          .then(async (res) => {
            resetFormValues('loginForm');
            if (!res.ok) {
                const errorResponse = await res.json();
                console.log("Inside error!");
                throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
            }
            return await res.json();
          })
          .then( data => {
                sessionStorage.setItem('loggedInUser',data);
                console.log(sessionStorage.getItem('loggedInUser'));
                console.log("login successfully");
                alert(JSON.parse(data));
                console.log(data);
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