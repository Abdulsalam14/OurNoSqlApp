
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

public class ImageHub : Hub
{
    public async override Task OnConnectedAsync()
    {
        await Clients.Others.SendAsync("Connect imagehub");
    }


    public async override Task OnDisconnectedAsync(Exception? ex)
    {
        await Clients.Others.SendAsync("Disconnect imagehub");
    }

    //public async Task ImageUploadedAsync()
    //{
    //    await Clients.Others.SendAsync("ImageUploaded");
    //}
}
