using System.Threading.Tasks;
using eShop_Mvc.Areas.Admin.Models;
using eShop_Mvc.Models.System;
using Microsoft.AspNetCore.SignalR;

namespace eShop_Mvc.SignalR.Hubs
{
    public class AnnoucementHub : Hub<IAnnouncementHub>
    {
        public async Task BroadcastAnnouncement(AnnouncementViewModel announcement)
        {
            await Clients.All.BroadcastAnnouncement(announcement);
        }
    }
}