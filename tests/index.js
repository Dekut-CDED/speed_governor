let dgram = require('dgram')
let cpusNo = require('os').cpus().length
let cluster = require("cluster")
let faker = require("faker")
let v4 = require('uuid')

class SpeedGov {
    constructor(phone, ownerId, imei, plateNumber,) {
        this.imei = imei
        this.phone = phone
        this.plateNumber = plateNumber
        this.ownerId = ownerId
    }
}
class Location {
    constructor(lat, long, ownerid, speedGov) {
        this.lat = lat
        this.long = long
        this.ownerid = ownerid
        this.speedGov = speedGov
    }
}

var ownerId = v4.v1();
const fakespeedgov = new SpeedGov(faker.phone.phoneNumber(), ownerId, v4.v4(), faker.vehicle.vrm())
const fakeLocation = new Location(faker.address.latitude(), faker.address.longitude(), ownerId, fakespeedgov)

if (cluster.isMaster) {
    for (var i = 0; i < cpusNo; i++) {
        cluster.fork()
    }
} else {
    sendNewLocation(fakeLocation)
}

async function sendNewLocation(fakeLocation) {
    var client = dgram.createSocket("udp4")

    async function send(client, location) {
        const loc = JSON.stringify(location)
        try {
            client.send(loc)
        } catch (error) {
            console.log(error)
        }
    }

    try {
        await client.connect("9999", "localhost")
        setInterval(async () => {
            await send(client, fakeLocation)
            console.log(fakeLocation)
        }, 3000);

    } catch (error) {
        console.log(error)
    }
}