"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/applicationHub").build();
connection.start().then().catch(function (err) {
    return console.error(err.toString());
});
// Listen for messages sent from the server
connection.on("PengajuanAkseptasiNotification", (userId, nomor_pengajuan, status) => {

    if($("#UserLogin").val() === userId){
        $(document).Toasts('create', {
            title: 'Notification',
            body: `<p>Nomor Pengajuan: ${nomor_pengajuan} status ${status}</p>`
        })

        $("#notificationAudio")[0].play();
    }
});