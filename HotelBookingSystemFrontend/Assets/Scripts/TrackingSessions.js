// // Function to increment active sessions count in localStorage
// function incrementActiveSessions() {
//     let activeSessions = parseInt(localStorage.getItem('activeSessions')) || 0;
//     activeSessions++;
//     localStorage.setItem('activeSessions', activeSessions.toString());
//   }
  
//   // Function to decrement active sessions count in localStorage
//   function decrementActiveSessions() {
//     let activeSessions = parseInt(localStorage.getItem('activeSessions')) || 0;
//     if(!activeSessions === 0)
//         activeSessions--;
//     localStorage.setItem('activeSessions', activeSessions.toString());
  
//     // If activeSessions becomes 0, perform logout actions
//     if (activeSessions === 0) {
//         logoutUser();
//         console.log('Automatically logged out due to all application windows closed');
//     }
    
//   }
  
//   // Function to perform logout actions
//   function logoutUser() {
//     // Clear localStorage for the logged in user
//     localStorage.removeItem('loggedInUser');
//     // Clear activeSessions count
//     localStorage.removeItem('activeSessions');
//     console.log('User logged out');
//   }
  
//   // Listen for tab/window open events to increment active sessions count
//   window.addEventListener('load', function() {
//     incrementActiveSessions();
//   });
  
//   // Listen for tab/window close events to decrement active sessions count
//   window.addEventListener('beforeunload', function() {
//     decrementActiveSessions();
//   });
  

const SESSION_EXPIRATION_TIME = 5 * 60;
const CHECK_ACTIVITY_INTERVAL = 30;
// const HEARTBEAT_INTERVAL = 60;
let logoutTimer;
// let heartbeatInterval;


function startSession() {
    clearTimeout(logoutTimer);
    logoutTimer = setTimeout(this.logout, SESSION_EXPIRATION_TIME * 1000);
    // heartbeatInterval = setInterval(sendHeartbeat, HEARTBEAT_INTERVAL * 1000);
}

function resetSession() {
    clearTimeout(logoutTimer);
    startSession();
}

function logout() {
    localStorage.clear();
    document.querySelectorAll('.logOutNavs').forEach(nav => nav.classList.add('show'));
    document.querySelectorAll('.logInNavs').forEach(nav => nav.classList.add('hide'));
    window.location.href = 'login.html';
}

document.addEventListener('mousemove', resetSession);
document.addEventListener('keypress', resetSession);

