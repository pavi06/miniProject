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

var validateEmail=()=>{
    var element = document.loginForm.email;
    if(element.value){
        return functionAddValidEffects(element, 'email');
    }
    else{
        return functionAddInValidEffects(element, 'email');
    }
}

var validatePassword = () => {
    var regexExpression = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[@$&*_])(?=.*[0-9]).{6,}$/;
    var element = document.loginForm.password;
    if(element.value && element.value.match(regexExpression)){
        return functionAddValidEffects(element, 'password');
    }
    else{
        return functionAddInValidEffects(element, 'password');
    }
}

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
          .then(res => res.json())
          .then( data => {
                console.log("login successfully");
                alert(data);
                console.log(data);
                // window.location.href = './index.html';
        }).catch( error => alert(error)
        );
    }
    else{
        alert("login failed! Try again Later!!");
    }
}