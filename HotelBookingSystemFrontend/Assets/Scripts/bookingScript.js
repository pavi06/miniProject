// document.addEventListener('DOMContentLoaded', function() {
//     var element = document.getElementsByClassName("discount");
//     element.forEach(ele => {
//     });
//     var discount = document.getElementById("discountPercent");
//     if(discount.textContent>0){
       
//         element.classList.add("reveal");
//     }
//     else{
//         element.classList.add("hide")
//     }
//     console.log(discount);
// }, false);


function changeRoomCount(countID, task) {
    var currElement = document.getElementById(countID);
    console.log(currElement);
    var currCount = parseInt(currElement.textContent);
    console.log(currCount)
    console.log(task)
    if (task === 'increase') {
        console.log("increase")
        currElement.textContent = currCount + 1;
    } else if (task === 'decrease' && currCount > 1) {
        console.log("decrease")
        currElement.textContent = currCount - 1;
    }
    else if(task === 'decrease' && currCount == 1){
        var parentDiv = currElement.parentElement.parentElement.parentElement;
        var preParent = parentDiv.parentElement;
        parentDiv.parentNode.removeChild(parentDiv);
        console.log(preParent);
        console.log(preParent.childNodes.length)
        if(preParent.childNodes.length === 0){
            console.log("innnn")
            var prePreParent = preParent.parentElement;
            preParent.parentNode.removeChild(preParent);
            console.log(prePreParent);
            prePreParent.parentNode.removeChild(prePreParent.parentElement);
        }
    }
}

function toggleMenu(menu) {
    var ele = menu.previousElementSibling;
    console.log(ele)
    ele.classList.toggle('active');
}

function deleteItem(currEle) {
    console.log(currEle)
    var element = currEle.parentNode;
    element = element.parentNode;
    element.parentNode.removeChild(element);
    console.log('Item deleted!');
}


function clearAll(){
    var rooms = document.getElementById("roomStart");
    rooms.parentNode.removeChild(rooms);
}
