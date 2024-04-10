
"use strict"

var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:7071/imagehub").build();

connection.start().then(function () {
    console.log("Started second")

}).catch(function (err) {
    return console.error(err.toString());
})

connection.on("Connect", function (info) {
    console.log("Connect Work second");
})
connection.on("Disconnect", function (info) {
    console.log("DisConnect Work second");
})



connection.on("ImageUploaded", function () {
    GetLastImage();
})

connection.on("AllImages", function () {
    GetAllImages();
})

function GetLastImage() {
    $.ajax({
        url: `/SecondHome/GetLastImage`,
        type: 'GET',
        success: function (data) {
            if (data) {
                let content = `<div class="img-thumbnail" ><img style="width:100%" src="${data}"/></div>`;
                $("#images-container").append(content)
            }
        },

        error: function (xhr) {
            console.log(xhr)
        }
    });
}


function GetAllImages() {
    $.ajax({
        url: `/SecondHome/GetAllImages`,
        type: 'GET',
        success: function (data) {
            let content = "";
            if (data) {
                console.log(data)
                for (var i = 0; i < data.length; i++) {
                    let element = data[i];
                    content += `
                    <div class="img-thumbnail" style="margin-bottom:15px" ><img style="width:100%" src="${element}"/></div>

                    `
                }
            }
            $("#images-container").html(content)
        },
        error: function (xhr) {
            console.log(xhr)
            $("#images-container").html("")

        }
    });
}
