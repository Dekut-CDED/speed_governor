"use strict"
var connection = new signalR.HubConnectionBuilder()
                 .withAutomaticReconnect()
                 .withUrl("/realtime")
                 .build();
mapboxgl.accessToken = 'pk.eyJ1IjoiYmVubnl0cm92YXRvIiwiYSI6ImNrZDcwdTVwbTE4amEyem8yZWdkNHN3ZmoifQ.r3Llqtnwfqqju2zfzE-fvA'

var map = new mapboxgl.Map({
container: 'map',
style: 'mapbox://styles/mapbox/satellite-v9',
zoom: 0
});

var geojson = `
{
    "type": "FeatureCollection",
    "features": [
        {
            "type": "Feature",
            "geometry": {
                "type": "LineString",
                "coordinates": []
            }
        }
    ]
}`;


var jsonobject = JSON.parse(geojson);


map.on('load', function () {

var coordinates = jsonobject.features[0].geometry.coordinates;

console.log(coordinates);

map.addSource('trace', { type: 'geojson', data: jsonobject });
map.addLayer({
'id': 'trace',
'type': 'line',
'source': 'trace',
'paint': {
'line-color': 'yellow',
'line-opacity': 0.75,
'line-width': 5
}
});
 
// setup the viewport
map.jumpTo({ 'center': coordinates[0], 'zoom': 14 });
map.setPitch(30);

});


function createUlList(eventMessage) {
 const msgCountH4 = document.getElementById("messageCount");
 msgCountH4.innerText = "Messages:" + messagecount.toString();
 const ul = document.getElementById("messagesList");
 const li = document.createElement("li");
 li.innerText = messagecount.toString();

for(const property in eventMessage){
   const newDiv = document.createElement("div");
   const classattribute = document.createAttribute("style");
   classattribute.value = "font-size: 80%";
   newDiv.setAttributeNode(classattribute);
   const newcontent = document.createTextNode(`${property}: ${eventMessage[property]}`);
   newDiv.appendChild(newcontent);
   li.appendChild(newDiv);
}
ul.appendChild(li);
}

let messagecount = 0;
connection.on("onMessageReceived", function(eventMessage){
 messagecount++;
 createUlList(eventMessage)
 //console.log(eventMessage);
  if (eventMessage) {
  jsonobject.features[0].geometry.coordinates.push(
  eventMessage
  );
  map.getSource('trace').setData(jsonobject);
  map.panTo(eventMessage);
 } 
//window.scrollTo(0, document.body.scrollHeight);

console.log(jsonobject.features[0].geometry.coordinates)
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
