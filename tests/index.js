let dgram = require("dgram");
let cpusNo = require("os").cpus().length;
let cluster = require("cluster");
let faker = require("faker");
let v4 = require("uuid");
const { random } = require("faker");

// speed gov available in the database
const speedgov = ["12345", "12346", "12347", "12348", "12349"];

class Location {
    constructor(lat, long, gpscourse, speedGov) {
        this.Latitude = lat;
        this.Long = long;
        this.Time = Date.now();
        this.GpsCourse = gpscourse;
        this.EngineON = true;
        this.SpeedSignalStatus = true;
        this.Speed = 45;
        this.SpeedGovId = speedGov;
    }
}

var ownerId = v4.v1();
const fakeLocation = new Location(
    faker.address.latitude(),
    faker.address.longitude(),
    ownerId,
    speedgov[Math.floor(Math.random() * speedgov.length)]
);

if (cluster.isMaster) {
    for (var i = 0; i < cpusNo; i++) {
        cluster.fork();
    }
} else {
    sendNewLocation(fakeLocation);
}

async function sendNewLocation(fakeLocation) {
    var client = dgram.createSocket("udp4");

    async function send(client, location) {
        const loc = JSON.stringify(location);
        try {
            client.send(loc);
        } catch (error) {
            console.log(error);
        }
    }

    try {
        await client.connect("3030", "41.89.227.168");
        setInterval(async () => {
            await send(client, fakeLocation);
            console.log(fakeLocation);
        }, 3000);
    } catch (error) {
        console.log(error);
    }
}
