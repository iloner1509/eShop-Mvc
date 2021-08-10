const connection = new signalR.HubConnectionBuilder().withUrl("/announcementHub").build();

connection.start().catch(err => console.error(err.toString()));
connection.on("BroadcastAnnouncement", function (announcement) {
    console.log(announcement);
});