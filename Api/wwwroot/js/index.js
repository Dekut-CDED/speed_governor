"use strict"
var connection = new signalR.HubConnectionBuilder()
                 .withAutomaticReconnect()
                 .withUrl("/realtime")
                 .build();


//Disable send button until connection is established
document.getElementById("sendButton").disabled = true

connection.on("ReceiveMessage", function (user, message) {
  var msg = message
    .replace(/&/g, "&amp;")
    .replace(/</g, "&lt;")
    .replace(/>/g, "&gt;")
  var encodedMsg = user + " says " + msg
  var li = document.createElement("li")
  li.textContent = encodedMsg
  document.getElementById("messagesList").appendChild(li)
})

let messagecount = 0;
connection.on("onMessageReceived", function(eventMessage){
 messagecount++;
 const msgCountH4 = document.getElementById("messageCount");
 msgCountH4.innerText = "Messages:" + messagecount.toString();
 const ul = document.getElementById("messagesList");
 const li = document.createElement("li");
 li.innerText = messagecount.toString();

 console.log(eventMessage)
for(const property in eventMessage){
   const newDiv = document.createElement("div");
   const classattribute = document.createAttribute("style");
   classattribute.value = "font-size: 80%";
   newDiv.setAttributeNode(classattribute);
   const newcontent = document.createTextNode(`${property}: ${eventMessage[property]}`);
   console.log(newcontent)
   newDiv.appendChild(newcontent);
   li.appendChild(newDiv);
}

ul.appendChild(li);
window.scrollTo(0, document.body.scrollHeight);

console.log(eventMessage);
})

connection.on('reconnecting', (count) => {
  console.log(`SignalR client reconnecting(${count}).`)
})
connection.on('disconnected', (code) => {
  console.log(`SignalR client disconnected(${code}).`)
})

connection.start()
  .then(function () {
    console.log("Signal Hub connected");
  })
  .catch(function (err) {
    return console.error(err.toString())
  })

document
  .getElementById("sendButton")
  .addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value
    var message = document.getElementById("messageInput").value
    connection.invoke("SendMessage", user, message).catch(function (err) {
      return console.error(err.toString())
    })
    event.preventDefault()
  })
