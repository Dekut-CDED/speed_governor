var dgram = require("dgram")

i = 0
while (i< 10) {
 SendUdpMessage("localhost", 5022,Buffer.from("b"b'GVNR,KCU 808H,451282484,001.6,0,1,-00.39955,17/03/21,13:43:47, 036.96272,4.232,000'"

 i++
}

function SendUdpMessage(targetIP, targetPORT, message){
    var client = dgram.createSocket("udp4");
    client.on("error", function (e){
      throw e
    } 
    );

    client.on("message", function(msg, rinfo){
     console.log("got: \t" + msg + "\t from\t" + rinfo.address + ":" + rinfo.port);
     client.close();
    })

    client.connect(targetPORT, targetIP, (err) => {
    client.send(message, 0, message.length,(err, bytes) => {
    console.log(bytes);
      }
    )
 })
}