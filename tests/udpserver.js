let udp = require('dgram');
let server = udp.createSocket('udp4');

// emits when any error occurs
server.on('error', function (error) {
    console.log('Error: ' + error);
    server.close();
});

// emits on new datagram msg
server.on('message', function (msg, info) {
    console.log('Data received from client');
    const speedgov = JSON.parse(msg.toString())
    console.log(speedgov)
    console.log('Received %d bytes from %s:%d\n', msg, info.address, info.port);

    //sending msg
    server.send(msg, info.port, 'localhost', function (error) {
        if (error) {
            client.close();
        } else {
            console.log("Received");
        }

    });

});

//emits when socket is ready and listening for datagram msgs
server.on('listening', function () {
    var address = server.address();
    var port = address.port;
    var family = address.family;
    var ipaddr = address.address;
    console.log('Server is listening at port' + port);
    console.log('Server ip :' + ipaddr);
    console.log('Server is IP4/IP6 : ' + family);
});

//emits after the socket is closed using socket.close();
server.on('close', function () {
    console.log('Socket is closed !');
});

server.bind(9999);


