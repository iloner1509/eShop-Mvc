using System.Threading.Tasks;
using eShop_Mvc.Areas.Admin.Models;
using eShop_Mvc.Models.System;

namespace eShop_Mvc.SignalR.Hubs
{
    public interface IAnnouncementHub
    {
        Task BroadcastAnnouncement(AnnouncementViewModel announcement);
    }
}