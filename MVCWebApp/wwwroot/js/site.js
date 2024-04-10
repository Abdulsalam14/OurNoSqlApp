//"use strict"

//var connection = new signalR.HubConnectionBuilder().withUrl("/imagehub").build();

//connection.start().then(function () {
//    console.log("Started first")
//}).catch(function (err) {
//    return console.error(err.toString());
//})


//connection.on("Connect", function (info) {
//    console.log("Connect Work first");
//})
//connection.on("Disconnect", function (info) {
//    console.log("DisConnect Work first");
//})





function showPdf(fileUrl) {
    var pdfViewer = document.getElementById('pdfViewer');
    pdfViewer.src = fileUrl;
    pdfViewer.style.display = 'block';
}


